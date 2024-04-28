# kubernetes-agent


![Version: 1.0.1](https://img.shields.io/badge/Version-1.0.1-informational?style=flat-square) ![Type: application](https://img.shields.io/badge/Type-application-informational?style=flat-square) ![AppVersion: 8.1.1507](https://img.shields.io/badge/AppVersion-8.1.1507-informational?style=flat-square) 

A Helm chart for the Octopus Kubernetes Agent

**Homepage:** <https://octopus.com>

## Maintainers

| Name | Email | Url |
| ---- | ------ | --- |
| Octopus Deploy | <support@octopus.com> | <https://octopus.com> |

## Source Code

* <https://github.com/OctopusDeploy/helm-charts>

## Values

### Agent values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.acceptEula | string | `"N"` | Setting to Y accepts the [Customer Agreement](https://octopus.com/company/legal) |
| agent.bearerToken | string | `""` | A JWT bearer token use to authenticate with the target Octopus Server |
| agent.debug.disableAutoPodCleanup | bool | `false` | Disables automatic pod cleanup |
| agent.defaultNamespace | string | `""` | The default Kubernetes namespace for deployments |
| agent.image | object | `{"pullPolicy":"IfNotPresent","repository":"octopusdeploy/kubernetes-tentacle","tag":"8.1.1507"}` | The image, repository &  to use for the agent container |
| agent.logLevel | string | `"Info"` | The log level of the agent. Logs are written to the pod logs as well as to file |
| agent.metadata | object | `{"annotations":{},"labels":{}}` | Additional metadata to add to the agent pod & container |
| agent.pollingConnectionCount | int | `5` | The number of polling TCP connections to open with the target Octopus Server |
| agent.resources | object | `{"requests":{"cpu":"100m","memory":"150Mi"}}` | The resource limits and requests assigned to the agent container |
| agent.serverApiKey | string | `""` | An Octopus Server API key use to authenticate with the target Octopus Server |
| agent.serverCommsAddress | string | `""` | The polling communication URL of the target Octopus Server |
| agent.serverUrl | string | `""` | The URL of the target Octopus Server to register this agent with |
| agent.serviceAccount.annotations | object | `{}` | Annotations to add to the created service account`` |
| agent.serviceAccount.name | string | Generates a name based on `targetName` | The name of the service account for the agent pod |
| agent.space | string | `"Default"` | The Space to register the agent in |
| agent.targetEnvironments | list | `[]` | The target environments to register the agent with |
| agent.targetName | string | `""` | The name of the deployment target |
| agent.targetRoles | list | `[]` | The target roles to register the agent with |

### Other Values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| imagePullSecrets | list | `[]` |  |
| nameOverride | string | `""` |  |
| persistence.nfs.image.pullPolicy | string | `"IfNotPresent"` |  |
| persistence.nfs.image.repository | string | `"octopusdeploy/nfs-server"` |  |
| persistence.nfs.image.tag | string | `"1.0.1"` |  |
| persistence.nfs.metadata.annotations | object | `{}` |  |
| persistence.nfs.metadata.labels | object | `{}` |  |
| persistence.nfs.watchdog.enabled | bool | `true` |  |
| persistence.nfs.watchdog.image.pullPolicy | string | `"IfNotPresent"` |  |
| persistence.nfs.watchdog.image.repository | string | `"octopusdeploy/kubernetes-agent-nfs-watchdog"` |  |
| persistence.nfs.watchdog.image.tag | string | `"0.0.2"` |  |
| persistence.nfs.watchdog.initial_backoff_seconds | string | `""` |  |
| persistence.nfs.watchdog.loop_seconds | string | `""` |  |
| persistence.nfs.watchdog.timeout_seconds | string | `""` |  |
| persistence.size | string | `"10Gi"` |  |
| persistence.storageClassName | string | `""` |  |
| scriptPods.disruptionBudgetEnabled | bool | `true` |  |
| scriptPods.serviceAccount.annotations | object | `{}` |  |
| scriptPods.serviceAccount.clusterRole.rules | list | `[]` |  |
| scriptPods.serviceAccount.name | string | `""` |  |
| scriptPods.serviceAccount.targetNamespaces | list | `[]` |  |

