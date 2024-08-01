# Octopus Deploy Helm charts

![Octopus Deploy & Kubernetes](octopus-kubernetes.png)

This repository contains the officially supported [Helm](https://helm.sh) chart for deploying [Octopus Deploy](https://octopus.com).

- [Octopus Deploy](https://github.com/OctopusDeploy/helm-charts/tree/main/charts/octopus-deploy) - Self-host the Octopus Deploy server.

## Kubernetes agent

The Kubernetes agent is the recommended way to deploy to Kubernetes clusters using Octopus Deploy. The helm chart is hosted on [Docker Hub](https://hub.docker.com/r/octopusdeploy/kubernetes-agent).

The chart can be found at [here](./charts/kubernetes-agent).

### Values documentation

The [Readme.md](./charts/kubernetes-agent/readme.md) in the chart directory contains the documentation for the `values.yaml`.

As the structure of the `values.yaml` may change between major versions, see below to find the correct `readme.md` for each version.

### Versions

The Kubernetes agent Helm chart follows [Semantic Versioning](https://semver.org/). Generally, version updates can be interpreted as follows:

- *major* - Breaking changes to the chart. This may include adding or removing of resources, the executing container images or breaking changes to the structure of the `values.yaml`.
- *minor* - New non-breaking features. New features or improvements to either the containers or helm chart itself. 
- *patch* - Minor non-breaking bug fixes or changes that do not introduce new features.

The `main` branch will reflect the current development version of the chart. This may be the latest released version or if a new version is in development, may be a pre-release version.


| Version   | Branch                                                                                                                               | Readme                                                                                                                  | values.yaml                                                                                                               |
| --------- | ------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| 2 (alpha) | [main](./charts/kubernetes-agent)                                                                                                    | [here](./charts/kubernetes-agent/readme.md)                                                                             | [here](./charts/kubernetes-agent/values.yaml)                                                                             |
| 1         | [release/kubernetes-agent/v1](https://github.com/OctopusDeploy/helm-charts/tree/release/kubernetes-agent/v1/charts/kubernetes-agent) | [here](https://github.com/OctopusDeploy/helm-charts/blob/release/kubernetes-agent/v1/charts/kubernetes-agent/README.md) | [here](https://github.com/OctopusDeploy/helm-charts/blob/release/kubernetes-agent/v1/charts/kubernetes-agent/values.yaml) |