using System.Diagnostics;
using System.Reflection;
using System.Text;
using Halibut;
using Halibut.Diagnostics;
using KubernetesAgent.Integration.Setup.Common;
using Octopus.Client.Model;
using Octopus.Tentacle.Client;
using Octopus.Tentacle.Client.Retries;
using Octopus.Tentacle.Client.Scripts;
using Octopus.Tentacle.Contracts.Observability;

namespace KubernetesAgent.Integration.Setup;

public class KubernetesAgentInstaller
{
    //This is the DNS of the localhost Kubernetes Server we add to the cluster in the KubernetesClusterInstaller.SetLocalhostRouting()
    const string LocalhostKubernetesServiceDns = "dockerhost.default.svc.cluster.local";
    const string ArtifactoryAgentOciRepository = "oci://docker.packages.octopushq.com/kubernetes-agent";

    readonly string helmExePath;
    readonly string kubeCtlExePath;
    readonly TemporaryDirectory temporaryDirectory;
    readonly ILogger logger;
    readonly string kubeConfigPath;
    protected HalibutRuntime ServerHalibutRuntime { get; private set; } = null!;
    protected TentacleClient TentacleClient { get; private set; } = null!;

    bool isAgentInstalled;

    public KubernetesAgentInstaller(TemporaryDirectory temporaryDirectory, string helmExePath, string kubeCtlExePath, string kubeConfigPath, ILogger logger)
    {
        this.temporaryDirectory = temporaryDirectory;
        this.helmExePath = helmExePath;
        this.kubeCtlExePath = kubeCtlExePath;
        this.kubeConfigPath = kubeConfigPath;
        this.logger = logger;

        AgentName = Guid.NewGuid().ToString("N");
    }

    public string AgentName { get; }

    public string Namespace => $"octopus-agent-{AgentName}";

    public Uri SubscriptionId { get; } = PollingSubscriptionIdGenerator.Generate();

    public async Task<TentacleClient> InstallAgent()
    {
        var listeningPort = BuildServerHalibutRuntimeAndListen();
        var valuesFilePath = await WriteValuesFile(listeningPort);
        var arguments = BuildAgentInstallArguments(valuesFilePath);

        var sw = new Stopwatch();
        sw.Restart();


        var result = ProcessRunner.RunWithLogger(helmExePath, temporaryDirectory, logger, arguments);
        sw.Stop();

        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException($"Failed to install Kubernetes Agent via Helm.");
        }

        isAgentInstalled = true;

        var thumbprint = await GetAgentThumbprint();

        logger.Information("Agent certificate thumbprint: {Thumbprint:l}", thumbprint);
        
        ServerHalibutRuntime.Trust(thumbprint);
        
        BuildTentacleClient(thumbprint);
        
        return TentacleClient;
        
    }

    async Task<string> WriteValuesFile(int listeningPort)
    {
        using var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStreamFromPartialName("agent-values.yaml"));

        var valuesFile = await reader.ReadToEndAsync();

        var serverCommsAddress = $"https://{LocalhostKubernetesServiceDns}:{listeningPort}";

        var configMapData = $@"
        Octopus.Home: /octopus
        Tentacle.Deployment.ApplicationDirectory: /octopus/Applications
        Tentacle.Communication.TrustedOctopusServers: >-
          [{{""Thumbprint"":""{TestCertificates.ServerPublicThumbprint}"",""CommunicationStyle"":{(int)CommunicationStyle.TentacleActive},""Address"":""{serverCommsAddress}"",""Squid"":null,""SubscriptionId"":""{SubscriptionId}""}}]
        Tentacle.Services.IsRegistered: 'true'
        Tentacle.Services.NoListen: 'true'";

        valuesFile = valuesFile
            .Replace("#{TargetName}", AgentName)
            .Replace("#{ServerCommsAddress}", serverCommsAddress)
            .Replace("#{ConfigMapData}", configMapData);

        var valuesFilePath = Path.Combine(temporaryDirectory.Directory.FullName, "agent-values.yaml");
        await File.WriteAllTextAsync(valuesFilePath, valuesFile, Encoding.UTF8);

        return valuesFilePath;
    }

    string BuildAgentInstallArguments(string valuesFilePath)
    {
        var chartVersion = GetChartVersion();
        var args = new[]
        {
            "upgrade",
            "--install",
            "--atomic",
            $"-f \"{valuesFilePath}\"",
            $"--version \"{chartVersion}\"",
            "--create-namespace",
            NamespaceFlag,
            KubeConfigFlag,
            AgentName,
            ArtifactoryAgentOciRepository
        };

        return string.Join(" ", args.WhereNotNull());
    }

    static string GetChartVersion()
    {
        var customHelmChartVersion = Environment.GetEnvironmentVariable("KubernetesIntegrationTests_HelmChartVersion");

        return !string.IsNullOrWhiteSpace(customHelmChartVersion) ? customHelmChartVersion : "1.*.*";
    }

    async Task<string> GetAgentThumbprint()
    {
        string? thumbprint = null;

        var attempt = 0;
        do
        {
            var result = ProcessRunner.Run(kubeCtlExePath, temporaryDirectory, $"get cm tentacle-config --namespace {Namespace} --kubeconfig=\"{kubeConfigPath}\" -o \"jsonpath={{.data['Tentacle\\.CertificateThumbprint']}}\"");
            thumbprint = await result.StandardOutput.ReadToEndAsync();
            if (result.ExitCode != 0)
            {
                logger.Error("Failed to load thumbprint. Exit code {ExitCode}", result.ExitCode); 
            }

            if (!string.IsNullOrWhiteSpace(thumbprint))
            {
                return thumbprint;
            }

            if (attempt == 60)
            {
                break;
            }

            attempt++;
            await Task.Delay(1000);
        } while (string.IsNullOrWhiteSpace(thumbprint));

        throw new InvalidOperationException("Failed to load the generated thumbprint after 60 attempts");
    }

    void BuildTentacleClient(string thumbprint)
    {
        var endpoint = new ServiceEndPoint(SubscriptionId, thumbprint, ServerHalibutRuntime.TimeoutsAndLimits);

        var retrySettings = new RpcRetrySettings(true, TimeSpan.FromMinutes(2));
        var clientOptions = new TentacleClientOptions(retrySettings);

        TentacleClient.CacheServiceWasNotFoundResponseMessages(ServerHalibutRuntime);

        TentacleClient = new TentacleClient(
            endpoint,
            ServerHalibutRuntime,
            new PollingTentacleScriptObserverBackoffStrategy(),
            new NoTentacleClientObserver(),
            clientOptions);
    }

    int BuildServerHalibutRuntimeAndListen()
    {
        var serverHalibutRuntimeBuilder = new HalibutRuntimeBuilder()
            .WithServerCertificate(TestCertificates.Server)
            .WithHalibutTimeoutsAndLimits(HalibutTimeoutsAndLimits.RecommendedValues());

        ServerHalibutRuntime = serverHalibutRuntimeBuilder.Build();

        return ServerHalibutRuntime.Listen();
    }
    string NamespaceFlag => $"--namespace \"{Namespace}\"";
    string KubeConfigFlag => $"--kubeconfig \"{kubeConfigPath}\"";

    public void Dispose()
    {
        if (isAgentInstalled)
        {
            var uninstallArgs = string.Join(" ",
                "uninstall",
                KubeConfigFlag,
                NamespaceFlag,
                AgentName);

            var result = ProcessRunner.RunWithLogger(helmExePath, temporaryDirectory, logger, uninstallArgs);

            if (result.ExitCode != 0)
            {
                logger.Error("Failed to uninstall Kubernetes Agent {AgentName} via Helm", AgentName);
            }
        }
    }
}