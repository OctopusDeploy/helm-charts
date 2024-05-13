using Halibut;
using KubernetesAgent.Integration.Setup;
using KubernetesAgent.Integration.Setup.Common;
using Octopus.Tentacle.Client;
using Octopus.Tentacle.Client.Scripts.Models;
using Octopus.Tentacle.Client.Scripts.Models.Builders;
using Octopus.Tentacle.Contracts;
using Xunit.Abstractions;

namespace KubernetesAgent.Integration;

public class HelmUpgradeTests(ITestOutputHelper output) : IAsyncLifetime
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
        agentInstaller = new KubernetesAgentInstaller(workingDirectory , helmExePath, kubeCtlPath, clusterInstaller.KubeConfigPath, logger);
        client = await agentInstaller.InstallAgent();
    }

    public async Task DisposeAsync()
    {
        clusterInstaller.Dispose();
        workingDirectory.Dispose();
        await Task.CompletedTask;
    }
    
    [Fact]
    public async Task CanUpgradeAgentAndRunCommand()
    {
        var helmPackage = HelmChartBuilder.BuildHelmChart(helmExePath, workingDirectory);
        var helmPackageFile = new FileInfo(helmPackage);
        var packageName = helmPackageFile.Name;
        var packageBytes = await File.ReadAllBytesAsync(helmPackage);
        var helmUpgradeScript =
            $"""
             helm upgrade \
             --atomic \
             --namespace {agentInstaller.Namespace} \
             {agentInstaller.AgentName} \
             /tmp/{packageName}
             """;
        var upgradeHelmChartCommand = new ExecuteKubernetesScriptCommandBuilder(Guid.NewGuid().ToString())
            .WithScriptBody($"cp ./{packageName} /tmp/{packageName} && {helmUpgradeScript}")
            .WithScriptFile(new ScriptFile(packageName, DataStream.FromBytes(packageBytes)))
            .Build();
        void onScriptStatusResponseReceived(ScriptExecutionStatus res) => logger.Information("{Output}", res.ToString());
        async Task onScriptCompleted(CancellationToken t)
        {
            await Task.CompletedTask; 
            logger.Information("Script completed");
        }
        var testLogger = new TestLogger(logger);
        var result = await client.ExecuteScript(upgradeHelmChartCommand, onScriptStatusResponseReceived, onScriptCompleted, testLogger, CancellationToken.None);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Script failed with exit code {result.ExitCode}");
        }
        logger.Information("Upgrade executed successfully");
        
        var runHelloWorldCommand = new ExecuteKubernetesScriptCommandBuilder(Guid.NewGuid().ToString())
            .WithScriptBody("echo \"hello world\"")
            .Build();
        result = await client.ExecuteScript(runHelloWorldCommand, onScriptStatusResponseReceived, onScriptCompleted, testLogger, CancellationToken.None);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Script failed with exit code {result.ExitCode}");
        }
        logger.Information("Script executed successfully");
    }
}