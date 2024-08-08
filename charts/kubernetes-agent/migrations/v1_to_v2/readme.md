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
* agent.targetTenantedDeploymentParticipation => agent.deploymentTarget.initial.tenantedDeploymentParticipation
* agent.targetTenants => agent.deploymentTarget.initial.tenants
* agent.scriptPods.image => agent.scriptPods.deploymentTarget.image (fields unchanged otherwise)

The following value must be set during the upgrade:
* agent.deploymentTarget.enabled: true

