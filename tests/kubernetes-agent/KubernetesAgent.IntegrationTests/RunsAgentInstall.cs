using Halibut;
using KubernetesAgent.Integration.Setup;
using KubernetesAgent.Integration.Setup.Common;
using Octopus.Tentacle.Client;
using Octopus.Tentacle.Client.Scripts.Models;
using Octopus.Tentacle.Client.Scripts.Models.Builders;
using Xunit.Abstractions;

namespace KubernetesAgent.Integration;

public class HelmInstallTests(ITestOutputHelper output) : IAsyncLifetime
{
    readonly ITestOutputHelper output = output;
    ILogger logger = null!;
    readonly TemporaryDirectory workingDirectory = new(Directory.CreateTempSubdirectory());
    KubernetesClusterInstaller clusterInstaller = null!;
    KubernetesAgentInstaller agentInstaller  = null!;
    TentacleClient client = null!;
    string kindExePath = null!;
    string helmExePath = null!;
    string kubeCtlPath = null!;
    
    public async Task InitializeAsync()
    {
        logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        var requiredToolDownloader = new RequiredToolDownloader(workingDirectory, logger);
        (kindExePath, helmExePath, kubeCtlPath) = await requiredToolDownloader.DownloadRequiredTools(CancellationToken.None);
        clusterInstaller =  new KubernetesClusterInstaller(workingDirectory, kindExePath, helmExePath, kubeCtlPath, logger);
        await clusterInstaller.Install();
    }

    public async Task DisposeAsync()
    {
        clusterInstaller.Dispose();
        workingDirectory.Dispose();
        await Task.CompletedTask;
    }
    
    [Fact]
    public async Task CanInstallAgentAndRunCommand()
    {        
        var chartDirectory = HelmChartBuilder.GetChartsDirectory();
        
        agentInstaller = new KubernetesAgentInstaller(workingDirectory , helmExePath, kubeCtlPath, clusterInstaller.KubeConfigPath, logger, chartDirectory.FullName);
        client = await agentInstaller.InstallAgent();

        var testLogger = new TestLogger(logger);
        
        var runHelloWorldCommand = new ExecuteKubernetesScriptCommandBuilder(Guid.NewGuid().ToString())
            .WithScriptBody("echo \"hello world\"")
            .Build();
        var result = await client.ExecuteScript(runHelloWorldCommand, OnScriptStatusResponseReceived, OnScriptCompleted, testLogger, CancellationToken.None);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Script failed with exit code {result.ExitCode}");
        }
        
        logger.Information("Script executed successfully");
        return;

        async Task OnScriptCompleted(CancellationToken t)
        {
            await Task.CompletedTask; 
            logger.Information("Script completed");
        }

        void OnScriptStatusResponseReceived(ScriptExecutionStatus res) => logger.Information("{Output}", res.ToString());
    }
}