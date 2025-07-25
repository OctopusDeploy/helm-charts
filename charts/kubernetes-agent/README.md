## Kubernetes agent

![Version: 2.19.1](https://img.shields.io/badge/Version-2.19.1-informational?style=flat-square) ![Type: application](https://img.shields.io/badge/Type-application-informational?style=flat-square) ![AppVersion: 8.3.3034](https://img.shields.io/badge/AppVersion-8.3.3034-informational?style=flat-square) ![Octopus Deploy Version: 2024.2.6580+](https://img.shields.io/badge/Octopus_Deploy-2024.2.6580%2B-2F93E0?style=flat-square&logo=octopusdeploy&logoColor=%232F93E0&logoSize=auto)

The Kubernetes agent is the recommended way to deploy to Kubernetes clusters using [Octopus Deploy](https://octopus.com).

The helm chart is hosted on [Docker Hub](https://hub.docker.com/r/octopusdeploy/kubernetes-agent), where you can pull it using Helm.

The source code for the chart can be found at [here](./charts/kubernetes-agent).

## Versions

The Kubernetes agent Helm chart follows [Semantic Versioning](https://semver.org/). Generally, version updates can be interpreted as follows:

- *major* - Breaking changes to the chart. This may include adding or removing of resources, breaking changes in the agent application image or breaking changes to the structure of the `values.yaml`.
- *minor* - New non-breaking features. New features or improvements to the agent application or helm chart itself.
- *patch* - Minor non-breaking bug fixes or changes that do not introduce new features.

The `main` branch will reflect the current development version of the chart. This may be the latest released version or if a new version is in development, may be a pre-release version.

| Version   | Branch                                                                                                                               | Readme                                                                                                                  | values.yaml                                                                                                               |
| --------- | ------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| 2         | [main](https://github.com/OctopusDeploy/helm-charts/tree/main/charts/kubernetes-agent)                                               | This file                                                                                                               | [here](./values.yaml)                                                                                                     |
| 1         | [release/kubernetes-agent/v1](https://github.com/OctopusDeploy/helm-charts/tree/release/kubernetes-agent/v1/charts/kubernetes-agent) | [here](https://github.com/OctopusDeploy/helm-charts/blob/release/kubernetes-agent/v1/charts/kubernetes-agent/README.md) | [here](https://github.com/OctopusDeploy/helm-charts/blob/release/kubernetes-agent/v1/charts/kubernetes-agent/values.yaml) |

### Migrations
Version 2 of the helm chart introduces breaking changes to `values.yaml`; some elements were renamed, while others were moved.

As such, upgrading from V1 to V2 of the helm chart requires user intervention.

This is documented [here](./migrations.md).

## Sub-charts

The Kubernetes agent is optionally installed alongside the Kubernetes agent, [read more here](./kubernetes-monitor.md).
## Maintainers

| Name | Email | Url |
| ---- | ------ | --- |
| Octopus Deploy | <support@octopus.com> | <https://octopus.com> |

## Values

### Agent values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.acceptEula | string | `"N"` | Setting to Y accepts the [Customer Agreement](https://octopus.com/company/legal) |
| agent.affinity | object | `{"nodeAffinity":{"requiredDuringSchedulingIgnoredDuringExecution":{"nodeSelectorTerms":[{"matchExpressions":[{"key":"kubernetes.io/os","operator":"In","values":["linux"]},{"key":"kubernetes.io/arch","operator":"In","values":["arm64","amd64"]}]}]}}}` | The affinities to apply to the agent pod |
| agent.bearerToken | string | `""` | A JWT bearer token used to authenticate with the target Octopus Server |
| agent.bearerTokenSecretName | string | `""` | The name of an existing Secret that contains a base64-encoded Octopus Server JWT bearer token. Value must be set in `data.bearer-token` in secret. |
| agent.certificate | string | `""` | A base64-encoded x509 certificate used to setup a trust between the agent and target Octopus Server |
| agent.debug.disableAutoPodCleanup | bool | `false` | Disables automatic pod cleanup |
| agent.enableMetricsCapture | bool | `true` | True if events should be scraped and added to the metrics config map |
| agent.image | object | `{"pullPolicy":"IfNotPresent","repository":"octopusdeploy/kubernetes-agent-tentacle","tag":"8.3.3034","tagSuffix":""}` | The repository, pullPolicy, tag & tagSuffix to use for the agent image |
| agent.logLevel | string | `"Info"` | The log level of the agent. Logs are written to the pod logs as well as to file |
| agent.machinePolicyName | string | `""` | The machine policy to register the agent with |
| agent.metadata | object | `{"annotations":{},"labels":{}}` | Additional metadata to add to the agent pod & container |
| agent.name | string | `""` | The name of the agent |
| agent.password | string | `""` | The password of the user used to authenticate with the target Octopus Server |
| agent.pollingConnectionCount | int | `5` | The number of polling TCP connections to open with the target Octopus Server |
| agent.pollingProxy | object | `{"host":"","password":"","port":80,"username":""}` | The host, port, username and password of the proxy server to use for polling connections |
| agent.preinstall.serviceAccount.annotations | object | `{}` | Annotations to add to the autogenerated pre-install registration service account |
| agent.preinstall.serviceAccount.name | string | Generates a name based on `agent.serviceAccount.name`, appending `-pre` | The name of the service account for the agent pre-install registration pod |
| agent.resources | object | `{"requests":{"cpu":"100m","memory":"150Mi"}}` | The resource limits and requests assigned to the agent container |
| agent.securityContext | object | `{}` | The security context to apply to the agent pod. runAsGroup and fsGroup should be blank or set to `0` |
| agent.serverApiKey | string | `""` | An Octopus Server API key used to authenticate with the target Octopus Server |
| agent.serverApiKeySecretName | string | `""` | The name of an existing Secret that contains a base64-encoded Octopus Server API Key.  Value must be set in `data.api-key` in secret. |
| agent.serverCertificate | string | `""` | The base64-encoded public key of the self-signed x509 certificate or root CA certificate used by the target Octopus Server. Must be in the PEM/CER format. See [documentation](https://octopus.com/docs/kubernetes/targets/kubernetes-agent#trusting-custominternal-octopus-server-certificates) for more information. |
| agent.serverCommsAddress | string | `""` | The polling communication URL of the target Octopus Server |
| agent.serverCommsAddresses | list | `[]` | The polling communication URLs of the target Octopus Servers when running in High Availability (HA) |
| agent.serverSubscriptionId | string | `""` | The subscription ID that is used to by the agent to identify itself with Octopus Server |
| agent.serverUrl | string | `""` | The URL of the target Octopus Server to register this agent with |
| agent.serviceAccount.annotations | object | `{}` | Annotations to add to the autogenerated service account |
| agent.serviceAccount.name | string | Generates a name based on `agent.name` | The name of the service account for the agent pod |
| agent.space | string | `"Default"` | The Space to register the agent in |
| agent.tolerations | list | `[]` | The tolerations to apply to the agent pod |
| agent.upgrade | object | `{"dockerAuth":{"password":"","registry":"","username":""}}` | Credentials used during agent-upgrade tasks. To be populated if encountering rate-limiting failures.  |
| agent.username | string | `""` | The username of the user used to authenticate with the target Octopus Server |
| agent.usernamePasswordSecretName | string | `""` | The name of an existing Secret that contains a base64-encoded username and password for an Octopus Server user. Values must be set in `data.username` and `data.password` in secret. |

### Agent as Deployment Target values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.deploymentTarget.enabled | bool | `false` | Set to register the agent as a Deployment Target using the provided initial values |

### Agent as Deployment Target initial values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.deploymentTarget.initial.defaultNamespace | string | `""` | The default Kubernetes namespace for deployments |
| agent.deploymentTarget.initial.environments | list | `[]` | The deployment target environments to register the agent with |
| agent.deploymentTarget.initial.tags | list | `[]` | The deployment target tags to register the agent with |
| agent.deploymentTarget.initial.tenantTags | list | `[]` | The deployment target tenant tags to register the agent with |
| agent.deploymentTarget.initial.tenantedDeploymentParticipation | string | `"Untenanted"` | Can be `Untenanted`, `TenantedOrUntenanted` or `Tenanted`. |
| agent.deploymentTarget.initial.tenants | list | `[]` | The deployment target tenants to register the agent with |

### Agent as Worker values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.worker.enabled | bool | `false` | Set to register the agent as a Worker using the provided initial values |

### Agent as Worker initial values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.worker.initial.workerPools | list | `[]` | The worker pools to associate with the worker |

### Persistence

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| persistence.nfs.affinity | object | `{"nodeAffinity":{"requiredDuringSchedulingIgnoredDuringExecution":{"nodeSelectorTerms":[{"matchExpressions":[{"key":"kubernetes.io/os","operator":"In","values":["linux"]},{"key":"kubernetes.io/arch","operator":"In","values":["arm64","amd64"]}]}]}}}` | The affinities to apply to the NFS pod |
| persistence.nfs.backingVolume.accessModes | list | `["ReadWriteOnce"]` | The access modes to use for the NFS Server's backing storage |
| persistence.nfs.backingVolume.storageClassName | string | `""` | The storage class name to use for the NFS Server's backing storage - if left as an empty string, an emptyDir will be used |
| persistence.nfs.image | object | `{"pullPolicy":"IfNotPresent","repository":"octopusdeploy/nfs-server","tag":"1.0.1"}` | The repository, pullPolicy & tag to use for the NFS server |
| persistence.nfs.metadata | object | `{"annotations":{},"labels":{}}` | Additional metadata to add to the NFS pod & container |
| persistence.nfs.tolerations | list | `[]` | The tolerations to apply to the NFS pod |
| persistence.nfs.watchdog.enabled | bool | `true` | If enabled, the NFS watchdog will monitor NFS availability and restart Tentacle and Script Pods if the NFS server is unresponsive |
| persistence.nfs.watchdog.image | object | `{"pullPolicy":"IfNotPresent","repository":"octopusdeploy/kubernetes-agent-nfs-watchdog","tag":"0.2.0"}` | The repository, pullPolicy & tag to use for the NFS watchdog |
| persistence.nfs.watchdog.initial_backoff_seconds | string | `""` | The initial backoff time in seconds to retry failed NFS checks @default 0.5 |
| persistence.nfs.watchdog.loop_seconds | string | `""` | The frequency in seconds to check the NFS server @default 5 |
| persistence.nfs.watchdog.timeout_seconds | string | `""` | The total time to retry failed NFS checks before giving up and deleting the pod @default 10 |
| persistence.size | string | `"10Gi"` | The size of the volume to create |
| persistence.storageClassName | string | `""` | if provided, will disable the default persistence configuration and create a PVC with the provided storage class |
| persistence.volumeName | string | `""` | if provided, will disable the default persistence configuration and create a PVC that is bound directly to the named PersistentVolume |

### Script pod values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| scriptPods.affinity | object | `{"nodeAffinity":{"requiredDuringSchedulingIgnoredDuringExecution":{"nodeSelectorTerms":[{"matchExpressions":[{"key":"kubernetes.io/os","operator":"In","values":["linux"]},{"key":"kubernetes.io/arch","operator":"In","values":["arm64","amd64"]}]}]}}}` | The affinities to apply to script pods |
| scriptPods.deploymentTarget.image | object | `{"pullPolicy":"","repository":"","tag":""}` | The repository, pullPolicy & tag to use for the script pod image when the agent is a deployment target |
| scriptPods.disruptionBudgetEnabled | bool | `true` | If true, the script pods will be created with a disruption budget to prevent them from being evicted |
| scriptPods.logging.disablePodEventsInTaskLog | bool | `false` | Disables script pod events being written to Octopus Server task log |
| scriptPods.metadata | object | `{"annotations":{}}` | Additional metadata to add to script pods |
| scriptPods.proxies.http_proxy | string | `""` | The URI of the HTTP proxy server to be used during script operations |
| scriptPods.proxies.https_proxy | string | `""` | The URI of the HTTPS proxy server to be used during script operations |
| scriptPods.proxies.no_proxy | string | `""` | A comma-separated list of host names or IP addresses that should not go through any proxy |
| scriptPods.resources | object | `{"requests":{"cpu":"25m","memory":"100Mi"}}` | The resource limits and requests assigned to script pod containers |
| scriptPods.securityContext | object | `{}` | The security context to apply to the script pods |
| scriptPods.serviceAccount.annotations | object | `{}` | Annotations to add to the service account |
| scriptPods.serviceAccount.clusterRole | object | `[{"apiGroups":["*"],"resources":["*"],"verbs":["*"]},{"nonResourceURLs":["*"],"verbs":["*"]}]` | if defined, overrides the default ClusterRole rules |
| scriptPods.serviceAccount.name | string | `""` | The name of the service account used for executing script pods |
| scriptPods.serviceAccount.targetNamespaces | list | Uses a ClusterRoleBinding to allow the service account to run in any namespace | Specifies that the pod service account should be constrained to target namespaces |
| scriptPods.tolerations | list | `[]` | The tolerations to apply to script pods |
| scriptPods.worker.image | object | `{"pullPolicy":"IfNotPresent","repository":"octopusdeploy/worker-tools","tag":"ubuntu.22.04"}` | The repository, pullPolicy & tag to use for the script pod image when the agent is a worker |

### Other Values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| imagePullSecrets | list | `[]` | custom registry pullSecret<br> See https://kubernetes.io/docs/concepts/containers/images/#specifying-imagepullsecrets-on-a-pod These are used for the tentacle and script pods |
| nameOverride | string | `""` | Override the name of the app |

----------------------------------------------
Autogenerated from chart metadata using [helm-docs](https://github.com/norwoodj/helm-docs)