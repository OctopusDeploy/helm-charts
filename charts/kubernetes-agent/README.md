# kubernetes-agent


![Version: 1.0.1](https://img.shields.io/badge/Version-1.0.1-informational?style=flat-square) ![Type: application](https://img.shields.io/badge/Type-application-informational?style=flat-square) ![AppVersion: 8.1.1507](https://img.shields.io/badge/AppVersion-8.1.1507-informational?style=flat-square) 

A Helm chart for the Octopus Kubernetes Agent

**Homepage:** <https://octopus.com>







## Values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| agent.acceptEula | string | `"N"` | Setting to Y accepts the [Customer Agreement](https://octopus.com/company/legal) |
| agent.bearerToken | string | `""` |  |
| agent.debug.disableAutoPodCleanup | bool | `false` |  |
| agent.defaultNamespace | string | `""` |  |
| agent.image.pullPolicy | string | `"IfNotPresent"` |  |
| agent.image.repository | string | `"octopusdeploy/kubernetes-tentacle"` |  |
| agent.image.tag | string | `"8.1.1507"` |  |
| agent.logLevel | string | `"Info"` |  |
| agent.metadata.annotations | object | `{}` |  |
| agent.metadata.labels | object | `{}` |  |
| agent.pollingConnectionCount | int | `5` |  |
| agent.resources.requests.cpu | string | `"100m"` |  |
| agent.resources.requests.memory | string | `"150Mi"` |  |
| agent.serverApiKey | string | `""` |  |
| agent.serverCommsAddress | string | `""` | The polling URL of the Octopus Server to register this agent with |
| agent.serverUrl | string | `""` | The URL of the Octopus Server to register this agent with |
| agent.serviceAccount.annotations | object | `{}` |  |
| agent.serviceAccount.name | string | `""` |  |
| agent.space | string | `"Default"` |  |
| agent.targetEnvironments | list | `[]` |  |
| agent.targetName | string | `""` | The name of the deployment target |
| agent.targetRoles | list | `[]` |  |
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

