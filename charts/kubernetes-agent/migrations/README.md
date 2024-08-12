# Migrating From V1 to V2
## Overview
Version 2 of the Kubernetes-agent Helm chart was created such that the agent may also be utilised as a scalable worker
by an OctopusDeploy server.

With this change, deploymentTarget specific Values were moved under a deploymentTarget specific parent node in `values.yml`.

Because of this change in structure, upgrading from V1 to V2 of the helm chart requires some additional steps.

Specifically, the following data items have been moved:
* agent.targetName => agent.name
* agent.targetEnvironments => agent.deploymentTarget.initial.environments
* agent.defaultNamespace => agent.deploymentTarget.initial.defaultNamespace
* agent.targetRoles => agent.deploymentTarget.initial.tags
* agent.targetTenantTags => agent.deploymentTarget.initial.tenantTag
* agent.targetTenants => agent.deploymentTarget.initial.tenants
* agent.targetTenantedDeploymentParticipation => agent.deploymentTarget.initial.targetTenantedDeploymentParticipation
* agent.scriptPods.image => agent.scriptPods.deploymentTarget.image (fields unchanged otherwise)

The following value must be set during the upgrade:
* agent.deploymentTarget.enabled: true


## How to Migrate from V1 to V2

1. Fetch your overriden values from installed agent in pretty-printed json (from the agent subsection)
```
RELEASE=theagent
NAMESPACE=octopus-agent-$RELEASE
helm get values --namespace $NAMESPACE $RELEASE > overridden_values.yaml
```
2. Edit `overriden_values.yaml` in your editor of choice to move/add the fields specified above (and save!).

3. Upgrade your release to V2.0.0 of the helm-chart, applying your modified values
```
helm upgrade --atomic --reset-then-reuse-values --namespace=$NAMESPACE $RELEASE -f overriden_values.yaml --version=2.*.* "oci://docker.packages.octopushq.com/kubernetes-agent"
```
