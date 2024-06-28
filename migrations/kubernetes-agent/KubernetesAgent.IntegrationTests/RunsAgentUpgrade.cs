using System.Text;
using KubernetesAgent.Integration.Setup;
using KubernetesAgent.Integration.Setup.Common;
using KubernetesAgent.Integration.Utils;
using KubernetesAgent.UpgradeManager;
using Octopus.Tentacle.Client;
using Octopus.Tentacle.Client.Scripts.Models;
using Octopus.Tentacle.Client.Scripts.Models.Builders;
using Octopus.Versioning;
using Xunit.Abstractions;
using File = KubernetesAgent.UpgradeManager.File;

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
        var packageBytes = await System.IO.File.ReadAllBytesAsync(helmPackage.Path);

        var upgradeManager = new KubernetesAgentUpgradeManager($"./{packageName}", includePreReleases: true);

        var upgradeManagerWrapper = new UpgradeManagerWrapper(upgradeManager, client, logger);

        await upgradeManagerWrapper.UpgradeAgent(agentInstaller.AgentName, agentInstaller.Namespace, version, helmPackage.Version, new File(packageName, packageBytes), CancellationToken.None);
        logger.Information("Upgrade executed successfully");

        var result = await RunScript("echo \"hello world\"", CancellationToken.None);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Script failed with exit code {result.ExitCode}");
        }
        logger.Information("Script executed successfully");
    }

    async Task<ScriptResult> RunScript(string script, CancellationToken _)
    {
        var commandBuilder = new ExecuteKubernetesScriptCommandBuilder(Guid.NewGuid().ToString())
            .IsRawScript()
            .WithScriptBody(script);

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
            foreach (var log in res.Logs)
            {
                logs.AppendLine(log.Text);
            }
            logger.Information("{Output}", res.ToString());
        }
    }
}