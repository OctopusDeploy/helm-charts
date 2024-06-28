using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Octopus.Versioning;

namespace KubernetesAgent.UpgradeManager;

public record ScriptResult(int ExitCode, string Output);

public delegate Task<ScriptResult> ScriptExecutor(string script, CancellationToken cancellationToken);

public interface IKubernetesAgentUpgradeManager
{
    Task UpgradeAgent(string helmRelease,
        string @namespace,
        IVersion currentVersion,
        IVersion newVersion,
        ScriptExecutor scriptExecutor,
        CancellationToken cancellationToken);

    Task<IVersion?> GetLatestVersion(ScriptExecutor scriptExecutor, CancellationToken cancellationToken);
}

public class KubernetesAgentUpgradeManager : IKubernetesAgentUpgradeManager
{
    const string KubernetesAgentOciRegistry = "oci://registry-1.docker.io/octopusdeploy/kubernetes-agent";
    const string KubernetesAgentPreReleaseOciRegistry = "oci://docker.packages.octopushq.com/kubernetes-agent";

    static readonly (int Version, bool isPreRelease) PackageVersion = (2, true);
    readonly (int Version, bool isPreRelease) latestSupportedMajorVersion;
    readonly string ociRegistry;
    readonly IMigrationManager migrationManager;

    public KubernetesAgentUpgradeManager(bool includePreReleases = false)
    {
        latestSupportedMajorVersion = GetLatestSupportedMajorVersion(includePreReleases);
        ociRegistry = latestSupportedMajorVersion.isPreRelease ? KubernetesAgentPreReleaseOciRegistry : KubernetesAgentOciRegistry;
        migrationManager = new MigrationManager();
    }

    internal KubernetesAgentUpgradeManager(string customOciRegistry, bool includePreReleases = false) : this(includePreReleases)
    {
        ociRegistry = customOciRegistry;
    }

    public async Task UpgradeAgent(string helmRelease,
        string @namespace,
        IVersion currentVersion,
        IVersion newVersion,
        ScriptExecutor scriptExecutor,
        CancellationToken cancellationToken)
    {
        if (newVersion.Major > latestSupportedMajorVersion.Version)
        {
            throw new InvalidOperationException($"Unable to upgrade Kubernetes Agent to {newVersion} because it has a higher major version than this migrator supports.");
        }

        if (currentVersion.Major == newVersion.Major)
        {
            await DoBasicUpgrade(helmRelease, @namespace, newVersion, scriptExecutor, cancellationToken);
        }

        await DoUpgradeWithMigrations(helmRelease, @namespace, currentVersion, newVersion, scriptExecutor, cancellationToken);
    }

    public async Task<IVersion?> GetLatestVersion(ScriptExecutor scriptExecutor, CancellationToken cancellationToken)
    {
        var versionParameter = $"{latestSupportedMajorVersion.Version}.*.*{(latestSupportedMajorVersion.isPreRelease ? "-alpha.*" : string.Empty)}";
        var getLatestVersionScript = $"helm show chart {ociRegistry} --version '{versionParameter}' 2>&1 | grep version: | awk -F'[ ]' '{{print $2}}' | tr -d '\n\t\r '";

        var result = await scriptExecutor(getLatestVersionScript, cancellationToken);

        if (result is { ExitCode: 0 } &&
            VersionFactory.TryCreateSemanticVersion(result.Output) is { } version)
        {
            return version;
        }

        return null;
    }

    async Task DoUpgradeWithMigrations(string helmRelease, string @namespace, IVersion currentVersion, IVersion newVersion, ScriptExecutor scriptExecutor, CancellationToken cancellationToken)
    {
        var getHelmValuesScript = $"helm get values {helmRelease} --namespace={@namespace} -o json";
        var result = await scriptExecutor(getHelmValuesScript, cancellationToken);
        if (result is not { ExitCode: 0 })
        {
            throw new InvalidOperationException($"Failed to retrieve current helm release values with exit code {result.ExitCode}");
        }

        var valuesJson = result.Output;

        var updatedValuesJson = migrationManager.MigrateValues(currentVersion, newVersion, valuesJson);

        var valuesYaml = ConvertJsonToYaml(updatedValuesJson);

        var upgradeWithMigrationsScript =
            $"""
             echo '{valuesYaml}' > values.yaml && \
             helm upgrade \
             --atomic \
             --version {newVersion} \
             --namespace {@namespace} \
             {helmRelease} \
             {KubernetesAgentOciRegistry} \
             --values values.yaml
             """;

        var upgradeResult = await scriptExecutor(upgradeWithMigrationsScript, cancellationToken);

        if (upgradeResult is not { ExitCode: 0 })
        {
            throw new InvalidOperationException($"Kubernetes agent upgrade failed with exit code {upgradeResult.ExitCode}");
        }
    }

    string ConvertJsonToYaml(string json)
    {
        dynamic? deserializedObject = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        if (deserializedObject is null)
        {
            throw new InvalidOperationException("Failed to upgrade Kubernetes agent, unable to parse json values after migration.");
        }
        var serializer = new YamlDotNet.Serialization.Serializer();
        return serializer.Serialize(deserializedObject);
    }

    async Task DoBasicUpgrade(
        string helmRelease,
        string @namespace,
        IVersion version,
        ScriptExecutor scriptExecutor,
        CancellationToken cancellationToken)
    {
        var basicUpgradeScript =
            $"""
             helm upgrade \
             --atomic \
             --version {version} \
             --namespace {@namespace} \
             {helmRelease} \
             {KubernetesAgentOciRegistry}
             """;

        var result = await scriptExecutor(basicUpgradeScript, cancellationToken);

        if (result is not { ExitCode: 0 })
        {
            throw new InvalidOperationException($"Kubernetes agent upgrade failed with exit code {result.ExitCode}");
        }
    }

    static (int Version, bool isPreRelease) GetLatestSupportedMajorVersion(bool includePreReleases = false)
    {
        if (PackageVersion.isPreRelease && !includePreReleases)
        {
            return (PackageVersion.Version - 1, false);
        }

        return (PackageVersion.Version, PackageVersion.isPreRelease);
    }
}
