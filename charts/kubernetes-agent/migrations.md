# Migrations
The Kubernetes Agent adheres to the principles of "Semantic Versioning" to define its version, this implies there are three ways
the chart may be upgraded:
* Patch upgrade (\*.\*.1) - backwards compatible bugfix, upgrade can be executed without additional changes
* Minor upgrade (\*.1.\*) - backwards compatible enhancement, upgrade can be executed without additional changes (new version adds new capabilities)
* Major upgrade (1.\*.\*) - breaking-change (eg Values file structure changes), will require user-input to perform upgrade

## Version 2 --> 3

### Overview

Version 3 of the Kubernetes agent changes the default storage from the in-cluster backed NFS server utilizing `ReadWriteMany`, to a single node `ReadWriteOnce` utilising the default storage class of the cluster.

This decision was made due to customer feedback and general concerns about the performance, reliability and security of the NFS server.

A side-effect of the change is that by default, the v3 agent no longer scales horizontally across nodes, but schedules the script pods on the same node as the agent/tentacle pod.

The changes to the `values.yaml` file are the following additions:

| Value name | Value | Comment |
|--|--|--|
| `persistence.nfs.enabled` | `false` | Indicates if the NFS pod should be used |
| `persistence.accessModes` | `["ReadWriteOnce"]` | Sets the access mode used for the PVC |

Kubernetes agents upgraded via Octopus Server will automatically continue to use the existing NFS-backed storage with no changes.

To enable NFS-backed storage on new installations, set

```yaml
persistence.nfs.enabled: true
persistence.accessModes: ["ReadWriteMany"]
```
as values during installation.

## Version 1 --> 2

### Overview

Version 2 of the Kubernetes agent was created to allow the agent to be used as a scalable worker in Octopus Server. As
the chart can now be installed in two different scenario's (deployment target or worker), changes were made to
the `values.yaml` structure.

To make it clearer which values apply to which scenario, certain Deployment Target specific values were moved under
a `deploymentTarget` specific parent nodes in `values.yml`.

This change in structure, demands a manual process to move your existing values to the new shape when upgrading from
v1 to v2.

The following value must be set during the upgrade:

| Value name | Value | Comment |
|--|--|--|
|agent.deploymentTarget.enabled | true | Specifies the helm install should act as a deployment target (not worker) |

The following data items have been moved or renamed; data types, and content remain unchanged:

| From                                        | To                                                                  | Comment                                               |
|---------------------------------------------|---------------------------------------------------------------------|-------------------------------------------------------|
| agent.targetName                            | agent.name                                                          | Generalised name, as may be worker or target          |
| agent.targetEnvironments                    | agent.deploymentTarget.initial.environments                         | N/A                                                   |
| agent.defaultNamespace                      | agent.deploymentTarget.initial.defaultNamespace                     | May be unset - can be ignored if null                 |                                           
| agent.targetRoles                           | agent.deploymentTarget.initial.tags                                 | In 2024.3 target roles have been replaced with 'tags' |                                              
| agent.targetTenantTags                      | agent.deploymentTarget.initial.tenantTag                            | May be unset - can be ignored if null.                |                                       
| agent.targetTenants                         | agent.deploymentTarget.initial.tenants                              | May be unset - can be ignored if null.                |                                        
| agent.targetTenantedDeploymentParticipation | agent.deploymentTarget.initial.<br/>targetTenantedDeploymentParticipation | May be unset - can be ignored if null.                |
| scriptPods.image                      | scriptPods.deploymentTarget.image                                   | Child fields are unchanged                            |

### Steps

1. Fetch overriden values from your installed agent

```
# Release and namespace can be found in Octopus Server, on the 'Connectivity' page of the Deployment Target or Worker being upgraded
RELEASE=<Helm Release Name>
NAMESPACE=<Namespace>

helm get values --namespace $NAMESPACE $RELEASE > overridden_values.yaml
```

2. Edit `overridden_values.yaml` in your editor of choice to move/add the fields specified above (and save!).

3. Upgrade your release to V2.0.0 of the helm-chart, applying your modified values

```
helm upgrade --atomic --reset-then-reuse-values --namespace=$NAMESPACE $RELEASE -f overriden_values.yaml --version="2.*.*" "oci://registry-1.docker.io/octopusdeploy/kubernetes-agent"
```