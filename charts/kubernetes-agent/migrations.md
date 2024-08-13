# Migrations
## Version 1 --> 2
### Overview
Version 2 of the Kubernetes agent was created to allow the agent to be used as a scalable worker in Octopus Server. As the chart can now be installed in two different scenario's (deployment target or worker), changes were made to the `values.yaml` structure.

To support this change, deploymentTarget specific Values were moved under a deploymentTarget specific parent node in `values.yml`.

This change in structure, demands a manual process to move your existing values to the new shape when upgrading from
v1 to v2.

Specifically, the following data items have been moved:
* agent.targetName => agent.name
* agent.targetEnvironments => agent.deploymentTarget.initial.environments
* agent.defaultNamespace => agent.deploymentTarget.initial.defaultNamespace
* agent.targetRoles => agent.deploymentTarget.initial.tags
* agent.targetTenantTags => agent.deploymentTarget.initial.tenantTag
* agent.targetTenants => agent.deploymentTarget.initial.tenants
* agent.targetTenantedDeploymentParticipation => agent.deploymentTarget.initial.targetTenantedDeploymentParticipation
* agent.scriptPods.image => agent.scriptPods.deploymentTarget.image (child-fields are unchanged)

The following value must be set during the upgrade:
* agent.deploymentTarget.enabled: true

### Steps

1. Fetch overriden values from your installed agent
```
RELEASE=theagent
NAMESPACE=octopus-agent-$RELEASE
helm get values --namespace $NAMESPACE $RELEASE > overridden_values.yaml
```
2. Edit `overridden_values.yaml` in your editor of choice to move/add the fields specified above (and save!).

3. Upgrade your release to V2.0.0 of the helm-chart, applying your modified values
```
helm upgrade --atomic --reset-then-reuse-values --namespace=$NAMESPACE $RELEASE -f overriden_values.yaml --version=2.*.* "oci://docker.packages.octopushq.com/kubernetes-agent"
```
