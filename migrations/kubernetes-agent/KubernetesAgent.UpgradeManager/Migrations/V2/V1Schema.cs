using System;
using Newtonsoft.Json;

namespace KubernetesAgent.UpgradeManager.Migrations.V2
{
    public record V1Schema
    {
        public V1Agent Agent { get; init; }

        [JsonExtensionData]
        public Dictionary<string, object> OtherProperties { get; init; }
    }

    public record V1Agent
    {
        public string DefaultNamespace { get; init; }
        public string ServerCommsAddress { get; init; }
        public List<string> ServerCommsAddresses { get; init; }
        public List<string> TargetEnvironments { get; init; }
        public string TargetName { get; init; }
        public List<string> TargetRoles { get; init; }
        public List<string> TargetTenantTags { get; init; }
        public string TargetTenantedDeploymentParticipation { get; init; }
        public List<string> TargetTenants { get; init; }

        [JsonExtensionData]
        public Dictionary<string, object> OtherProperties { get; init; }
    }
}
