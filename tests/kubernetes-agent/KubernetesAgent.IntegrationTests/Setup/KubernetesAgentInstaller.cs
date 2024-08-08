using System.Diagnostics;
using System.Reflection;
using System.Text;
using Halibut;
using Halibut.Diagnostics;
using KubernetesAgent.Integration.Setup.Common;
using Newtonsoft.Json;
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
    readonly string? chartDirectoryName;
    readonly string kubeConfigPath;
    protected HalibutRuntime ServerHalibutRuntime { get; private set; } = null!;
    protected TentacleClient TentacleClient { get; private set; } = null!;

    bool isAgentInstalled;

    public KubernetesAgentInstaller(TemporaryDirectory temporaryDirectory, string helmExePath, string kubeCtlExePath, string kubeConfigPath, ILogger logger, string? chartDirectoryName = null)
    {
        this.temporaryDirectory = temporaryDirectory;
        this.helmExePath = helmExePath;
        this.kubeCtlExePath = kubeCtlExePath;
        this.kubeConfigPath = kubeConfigPath;
        this.logger = logger;
        this.chartDirectoryName = chartDirectoryName;

        AgentName = Guid.NewGuid().ToString("N");
    }

    public string AgentName { get; }

    public string Namespace => $"octopus-agent-{AgentName}";

    const string RegistrySecretName = "docker-registry-secret";

    public Uri SubscriptionId { get; } = PollingSubscriptionIdGenerator.Generate();

    public async Task<TentacleClient> InstallAgent()
    {
        var listeningPort = BuildServerHalibutRuntimeAndListen();
        var valuesFilePath = await WriteValuesFile(listeningPort);

        var sw = new Stopwatch();
        sw.Restart();

        CreateNamespace();

        var hasRegistrySecret = await AddDockerHubRegistryCredentialSecret(logger);
        
        var arguments = BuildAgentInstallArguments(valuesFilePath, hasRegistrySecret);

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

    string BuildAgentInstallArguments(string valuesFilePath, bool hasRegistrySecret)
    {
        var chartVersion = GetChartVersion();
        var args = new[]
        {
            "upgrade",
            "--install",
            "--atomic",
            $"-f \"{valuesFilePath}\"",
            hasRegistrySecret ? $"--set imagePullSecrets[0].name=\"{RegistrySecretName}\"" : null,
            $"--version \"{chartVersion}\"",
            NamespaceFlag,
            KubeConfigFlag,
            AgentName,
            //Use the local directory if it exists, otherwise pull from artifactory
            chartDirectoryName ?? ArtifactoryAgentOciRepository
        };

        return string.Join(" ", args.WhereNotNull());
    }

    void CreateNamespace()
    {
        var result = ProcessRunner.Run(kubeCtlExePath, temporaryDirectory, "create", "namespace", Namespace, KubeConfigFlag);

        if (result.ExitCode != 0)
        {
            logger.Error("Failed to create namespace {Namespace}", Namespace);
            throw new InvalidOperationException($"Failed to create namespace {Namespace}.");
        }
    }

    async Task<bool> AddDockerHubRegistryCredentialSecret(ILogger logger)
    {
        var username = Environment.GetEnvironmentVariable("DockerHub_Username");
        var password = Environment.GetEnvironmentVariable("DockerHub_Password");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        using var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStreamFromPartialName("docker-registry-credentials-secret.yaml"));

        var secretYaml = await reader.ReadToEndAsync();

        var config = new Dictionary<string, object>
        {
            ["auths"] = new Dictionary<string, object>
            {
                ["https://index.docker.io/v1/"] = new
                {
                    username,
                    password,
                    auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"))
                }
            }
        };

        var configJson = JsonConvert.SerializeObject(config, Formatting.None);

        secretYaml = secretYaml
            .Replace("#{Name}", RegistrySecretName)
            .Replace("#{Namespace}", Namespace)
            .Replace("#{DockerConfigJson}", Convert.ToBase64String(Encoding.UTF8.GetBytes(configJson)));

        var filePath = Path.Combine(temporaryDirectory.Directory.FullName, "docker-registry-credentials-secret.yaml");
        await File.WriteAllTextAsync(filePath, secretYaml, Encoding.UTF8);

        var result = ProcessRunner.RunWithLogger(kubeCtlExePath, temporaryDirectory, this.logger, "apply", $"-f {filePath}", KubeConfigFlag);
        
        if (result.ExitCode != 0)
        {
            logger.Error("Failed to create docker hub registry secret {SecretName}", RegistrySecretName);
            throw new InvalidOperationException($"Failed to create docker hub registry secret {RegistrySecretName}.");
        }

        return true;
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
            var uninstallArgs = string.Join(
                " ",
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
