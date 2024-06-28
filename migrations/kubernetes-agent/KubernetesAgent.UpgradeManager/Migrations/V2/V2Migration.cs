using System;
using Newtonsoft.Json;

namespace KubernetesAgent.UpgradeManager.Migrations.V2
{
    class V2Migration : IMigration
    {
        public int Version => 2;

        public string MigrateValues(string previousValues)
        {

            var oldSchema = JsonConvert.DeserializeObject<V1Schema>(previousValues);

            if (oldSchema is null)
            {
                throw new InvalidOperationException("Unable to migrate values from v1 to v2, could not deserialize v1 values");
            }

            var serverCommsAddresses = oldSchema.Agent.ServerCommsAddresses;
            if (!string.IsNullOrWhiteSpace(oldSchema.Agent.ServerCommsAddress))
            {
                serverCommsAddresses.Add(oldSchema.Agent.ServerCommsAddress);
            }

            var newSchema = new V2Schema
            {
                Agent = new V2Agent
                {
                    Name = oldSchema.Agent.TargetName,
                    DeploymentTarget = new V2DeploymentTarget
                    {
                        IsEnabled = true,
                        Initial = new V2DeploymentTargetInitial
                        {
                            Environments = oldSchema.Agent.TargetEnvironments,
                            Tags = oldSchema.Agent.TargetRoles,
                            TenantedDeploymentParticipation = oldSchema.Agent.TargetTenantedDeploymentParticipation,
                            Tenants = oldSchema.Agent.TargetTenants,
                            TenantTags = oldSchema.Agent.TargetTenantTags,
                            DefaultNamespace = oldSchema.Agent.DefaultNamespace
                        }
                    },
                    Worker = new V2Worker
                    {
                        IsEnabled = false,
                        Initial = new V2WorkerInitial
                        {
                            WorkerPools = new List<string>()
                        }
                    },
                    ServerCommsAddresses = serverCommsAddresses,
                    OtherProperties = oldSchema.Agent.OtherProperties
                },
                OtherProperties = oldSchema.OtherProperties
            };

            return JsonConvert.SerializeObject(newSchema);
        }
    }
}
