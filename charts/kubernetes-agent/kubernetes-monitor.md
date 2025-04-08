## Kubernetes monitor

The Kubernetes monitor is component installed alongside the Kubernetes agent that enables functionality used for [Kubernetes Live Object Status](https://octopus.com/docs/kubernetes/live-object-status).

The helm chart is hosted on [Docker Hub](https://hub.docker.com/r/octopusdeploy/kubernetes-monitor-chart), where you can pull it using Helm.

The source for this Helm chart is not yet published openly, check back soon for updates here.

## Versions

The Kubernetes Monitor Helm chart follows [Semantic Versioning](https://semver.org/). Generally, version updates can be interpreted as follows:

- *major* - Breaking changes to the chart. This may include adding or removing of resources, breaking changes in the Kubernetes monitor application image or breaking changes to the structure of the `values.yaml`.
- *minor* - New non-breaking features. New features or improvements to the Kubernetes monitor application or helm chart itself.
- *patch* - Minor non-breaking bug fixes or changes that do not introduce new features.

----------------------------------------------

## Values

### Monitor

| Key                         | Type   | Default | Description                                                                                                                                                                                                                              |
| --------------------------- | ------ | ------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| monitor.authenticationToken | string | `""`    | If setting up the agent without automatic registration, this is the authentication token. If you provide this, you must also provide the installation ID. If you provide this, the monitor will not attempt to register with the server. |
| monitor.installationId      | string | `""`    | If setting up the agent without automatic registration, this is the installation id. If you provide this, you must also provide the authentication token. If you provide this, the monitor will not attempt to register with the server. |
| monitor.serverGrpcUrl       | string | `""`    | The gRPC url (including the port) of the Octopus Deploy server to communicate with                                                                                                                                                       |
| monitor.serverThumbprint    | string | `""`    | The thumbprint of the Octopus Deploy server the monitor is communicating with. This should only be used if you wish to pin the certificate.                                                                                              |

### Registration

| Key                                                      | Type   | Default    | Description                                                                     |
| -------------------------------------------------------- | ------ | ---------- | ------------------------------------------------------------------------------- |
| registration.machineName                                 | string | `""`       | The machine name of the agent this monitor is responsible for                   |
| registration.register                                    | bool   | `true`     | Automatically register the monitor with the Octopus Deploy server               |
| registration.secretStoreType                             | string | `"secret"` | The type of secret store to use when saving configuration                       |
| registration.serverAccessToken                           | string | `""`       | The access token to authenticate to Octopus Deploy to register with             |
| registration.serviceAccount.annotations                  | object | `{}`       | Additional annotations for the service account                                  |
| registration.serviceAccount.automountServiceAccountToken | bool   | `true`     | Auto-mount service account token                                                |
| registration.serviceAccount.create                       | bool   | `true`     | Specifies whether a service account should be created for the registration hook |
| registration.serviceAccount.name                         | string | `""`       | Custom service account name                                                     |
| registration.spaceId                                     | string | `""`       | The space id that the monitor is registering with                               |

### Other Values

| Key                                         | Type   | Default                              | Description                                                                                                                                                           |
| ------------------------------------------- | ------ | ------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| affinity                                    | object | `{}`                                 | Custom affinity for kubernetes monitor pods                                                                                                                           |
| fullnameOverride                            | string | `""`                                 | Override for the fully qualified app name for generated resources                                                                                                     |
| image.pullPolicy                            | string | `"IfNotPresent"`                     | Image pull policy                                                                                                                                                     |
| image.registry                              | string | `"docker.io"`                        | Registry host to pull images from                                                                                                                                     |
| image.repository                            | string | `"octopusdeploy/kubernetes-monitor"` | Image name to use                                                                                                                                                     |
| image.tag                                   | string | `.Chart.AppVersion`                  | Image tag to use                                                                                                                                                      |
| image.tagSuffix                             | string | `""`                                 |                                                                                                                                                                       |
| imagePullSecrets                            | list   | `[]`                                 | Optional array of imagePullSecrets containing private registry credentials Ref: https://kubernetes.io/docs/tasks/configure-pod-container/pull-image-private-registry/ |
| nameOverride                                | string | `""`                                 | Override for the short name for generated resources                                                                                                                   |
| nodeSelector                                | object | `{}`                                 | Custom node selector for kubernetes monitor pods                                                                                                                      |
| podAnnotations                              | object | `{}`                                 | Annotations to be added to kubernetes monitor pods                                                                                                                    |
| podLabels                                   | object | `{}`                                 | Labels to be added to kubernetes monitor pods                                                                                                                         |
| podSecurityContext                          | object | `{}`                                 | Security context for kubernetes monitor pods                                                                                                                          |
| registration.serverApiUrl                   | string | `""`                                 | The API URL of Octopus Deploy for registration                                                                                                                        |
| resources                                   | string | `nil`                                | Resources to allocate for the kubernetes monitor pod                                                                                                                  |
| securityContext                             | object | `{}`                                 | Security context for kubernetes monitor containers                                                                                                                    |
| serviceAccount.annotations                  | object | `{}`                                 | Additional annotations for the service account                                                                                                                        |
| serviceAccount.automountServiceAccountToken | bool   | `true`                               | Auto-mount service account token or not                                                                                                                               |
| serviceAccount.create                       | bool   | `true`                               | Specifies whether a service account should be created                                                                                                                 |
| serviceAccount.name                         | string | `""`                                 | Custom service account name                                                                                                                                           |
| tolerations                                 | list   | `[]`                                 | Custom tolerations for kubernetes monitor pods                                                                                                                        |

Autogenerated from chart metadata using [helm-docs](https://github.com/norwoodj/helm-docs)