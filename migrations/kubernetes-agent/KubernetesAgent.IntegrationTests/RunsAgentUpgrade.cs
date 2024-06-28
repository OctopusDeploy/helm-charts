using System.Text;
using Halibut;
using KubernetesAgent.Integration.Setup;
using KubernetesAgent.Integration.Setup.Common;
using KubernetesAgent.UpgradeManager;
using Octopus.Tentacle.Client;
using Octopus.Tentacle.Client.Scripts.Models;
using Octopus.Tentacle.Client.Scripts.Models.Builders;
using Octopus.Tentacle.Contracts;
using Octopus.Versioning;
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
    IVersion version;

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
        var agent = await agentInstaller.InstallAgent();
        client = agent.TentacleClient;
        version = agent.Version;
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
        var helmPackageFile = new FileInfo(helmPackage.Path);
        var packageName = helmPackageFile.Name;
        var packageBytes = await File.ReadAllBytesAsync(helmPackage.Path);

        var upgradeManager = new KubernetesAgentUpgradeManager($"./{packageName}", includePreReleases: true);

        await upgradeManager.UpgradeAgent(agentInstaller.AgentName, agentInstaller.Namespace, version, helmPackage.Version, async (script, ct) => await RunScript(script, ct, (packageName, packageBytes)), CancellationToken.None);
        logger.Information("Upgrade executed successfully");

        var result = await RunScript("echo \"hello world\"", CancellationToken.None);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Script failed with exit code {result.ExitCode}");
        }
        logger.Information("Script executed successfully");
    }

    async Task<ScriptResult> RunScript(string script, CancellationToken _, (string Name, byte[] Bytes)? package = null)
    {
        var commandBuilder = new ExecuteKubernetesScriptCommandBuilder(Guid.NewGuid().ToString())
            .IsRawScript()
            .WithScriptBody(script);
        if (package is not null)
        {
            commandBuilder.WithScriptFile(new ScriptFile(package.Value.Name, DataStream.FromBytes(package.Value.Bytes)));
        }

        var command = commandBuilder.Build();

        var logs = new StringBuilder();

        var testLogger = new TestLogger(logger);
        var result = await client.ExecuteScript(command, OnScriptStatusResponseReceived, OnScriptCompleted, testLogger, CancellationToken.None);
        return new ScriptResult(result.ExitCode, logs.ToString());

        async Task OnScriptCompleted(CancellationToken t)
        {
            await Task.CompletedTask;
            logger.Information("Script completed");
        }

        void OnScriptStatusResponseReceived(ScriptExecutionStatus res)
        {
            logs.Append(res.Logs);
            logger.Information("{Output}", res.ToString());
        }
    }
}