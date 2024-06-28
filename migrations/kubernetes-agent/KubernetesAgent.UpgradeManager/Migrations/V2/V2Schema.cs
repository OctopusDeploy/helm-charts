using System;
using Newtonsoft.Json;

namespace KubernetesAgent.UpgradeManager.Migrations.V2
{
    public record V2Schema
    {
        public V2Agent Agent { get; init; }

        [JsonExtensionData]
        public Dictionary<string, object> OtherProperties { get; init; }
    }

    public record V2Agent
    {
        public string Name { get; init; }
        public V2DeploymentTarget DeploymentTarget { get; init; }
        public V2Worker Worker { get; init; }

        public List<string> ServerCommsAddresses { get; init; }

        [JsonExtensionData]
        public Dictionary<string, object> OtherProperties { get; init; }
    }

    public record V2DeploymentTarget
    {
        public bool IsEnabled { get; init; }
        public V2DeploymentTargetInitial Initial { get; init; }
    }

    public record V2Worker
    {
        public bool IsEnabled { get; init; }
        public V2WorkerInitial Initial { get; init; }
    }

    public record V2DeploymentTargetInitial
    {
        public List<string> Environments { get; init; }
        public List<string> Tags { get; init; }
        public string TenantedDeploymentParticipation { get; init; }
        public List<string> Tenants { get; init; }
        public List<string> TenantTags { get; init; }
        public string DefaultNamespace { get; init; }
    }

    public record V2WorkerInitial
    {
        public List<string> WorkerPools { get; init; }
    }
}
