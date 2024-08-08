#!/bin/bash

# This script is responsible for migrating a v1 Kubernetes Agent to v2.
# The primary concern with this migration, is that some Values have changed location
# in the Values file.
# This script:
## extracts values from currently installed kuberentes agent (in json)
## transforms the values into the shape expected by a V2 install, and stores in a helm override file using a jq filter
## upgrades the helm chart - by resetting the release's values, and applying the migrated values created in prior step
NAMESPACE=octopus-agent-theagent
RELEASE=theagent
CHART="oci://docker.packages.octopushq.com/kubernetes-agent"

IFS='#'
FILTER="{
  name: .agent.targetName,
  deploymentTarget: {
     enabled: true,
     initial: {
       defaultNamespace: .agent.defaultNamespace,
       environments: .agent.targetEnvironments,
       tags: .agent.targetRoles,
       tenantTags: .agent.targetTentantTags,
       tenantedDeploymentParticipation: .agent.targetTenantedDeploymentParticipation,
       tenants: .agent.targetTenants

     }
  },
  scriptPods: {
    deploymentTarget: {
      image: .agent.scriptPods.image
    }
  }
} | del(..|nulls) +
{
  targetName : null,
  defaultNamespace: null,
  targetRoles: null,
  targetTentantTags: null,
  targetTentantedDeploymentParticipation: null,
  targetTenants: null,
  targetEnvironments: null
}"

MIGRATED_VALUES=`helm get values --namespace=$NAMESPACE $RELEASE -o json | jq $FILTER | jq .`
#echo $MIGRATED_VALUES

helm upgrade --atomic --reset-then-reuse-values --namespace=$NAMESPACE $RELEASE --set-json "agent=$MIGRATED_VALUES" --version=2.*.* $CHART
