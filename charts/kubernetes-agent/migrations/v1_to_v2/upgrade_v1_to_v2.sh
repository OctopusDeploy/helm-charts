#!/bin/bash

# This script is responsible for migrating a v1 Kubernetes Agent to v2.
# The primary concern with this migration, is that some Values have changed location
# in the Values file.
# This script:
# 1. Extracts values from currently installed kubernetes agent (in json)
# 2. Transforms the values into the shape expected by a V2 install, and stores in a helm override file using a jq filter
# 3. Upgrades the helm chart - by resetting the release's values to those in the new chart then applying the previous
#    current release modified values "over the top" then migrating the moved values

NAMESPACE=octopus-agent-theagent
RELEASE=theagent
CHART="oci://docker.packages.octopushq.com/kubernetes-agent"

if ! command -v jq &> /dev/null
then
    echo "jq could not be found, and is required for this operation"
    exit 1
fi

if ! command -v helm &> /dev/null
then
    echo "helm could not be found, and is required for this operation"
    exit 1
fi


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
  targetTenantTags: null,
  targetTenantedDeploymentParticipation: null,
  targetTenants: null,
  targetEnvironments: null
}"


CURRENT_MANUALLY_SET_VALUES=`helm get values --namespace $NAMESPACE $RELEASE -o json`
MIGRATED_VALUES=`jq $FILTER <<< $CURRENT_MANUALLY_SET_VALUES`
#echo $MIGRATED_VALUES

helm upgrade --atomic --reset-then-reuse-values --namespace=$NAMESPACE $RELEASE --set-json "agent=$MIGRATED_VALUES" --version=2.*.* $CHART
