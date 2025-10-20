# Octopus Deploy Helm charts

![Octopus Deploy & Kubernetes](octopus-kubernetes.png)

This repository contains the [Helm](https://helm.sh) charts supported by [Octopus Deploy](https://octopus.com).

-   [Octopus Deploy](https://github.com/OctopusDeploy/helm-charts/tree/main/charts/octopus-deploy) - Self-host the Octopus Deploy server on your own Kubernetes cluster.
-   [Kubernetes Agent](https://github.com/OctopusDeploy/helm-charts/tree/main/charts/kubernetes-agent) - The agent which allows Octopus to run workloads on your Kubernetes clusters.

After making changes to either Helm Chart, run `pnpm changeset` for a guided process to generating changeset details for consumers.
