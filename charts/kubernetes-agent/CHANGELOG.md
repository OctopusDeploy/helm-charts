# kubernetes-agent

## 2.19.1

### Patch Changes

- f963fe5: Added custom access modes (for testing and dev purposes only)

## 2.19.0

### Minor Changes

- 32645b1: Upgrade kubernetes-monitor-chart to v0.18.0

## 2.18.0

### Minor Changes

- eb17bd6: Updated kubernetes monitor chart to v0.17.0

## 2.17.2

### Patch Changes

- b2de557: Updated the Kubernetes Monitor chart to 0.14.0

## 2.17.1

### Patch Changes

- 92df954: Update Tentacle to 8.3.3034 to fix an issue with fallback container tags and newer versions of Kubernetes.

## 2.17.0

### Minor Changes

- ea7d885: Added backing volume support for the NFS server

## 2.16.2

### Patch Changes

- 818d842: Fixed hook delete policy to correctly remove pre-installation resources on success

## 2.16.1

### Patch Changes

- b4718d1: Upgrade kubernetes-monitor chart to v0.13.0

  Kubernetes monitor changes:

  - fix: Invalidate k8s discovery cache more frequently to detect new CRDs

## 2.16.0

### Minor Changes

- 7458377: Upgrade kubernetes-monitor chart to v0.12.0

## 2.15.0

### Minor Changes

- 2c69b27: Forward merge from release/kubernetes-agent/v1 version 1.24.0. Add ability to bind PVC to specific PV via a volumeName

## 2.14.3

### Patch Changes

- b428a40: Update Kubernetes Monitor subchart to 0.11.0

## 2.14.2

### Patch Changes

- f3c8e17: Update Kubernetes Monitor subchart to 0.10.0

## 2.14.1

### Patch Changes

- ff8f144: Update Kubernetes Monitor subchart to 0.9.0

## 2.14.0

### Minor Changes

- cf05e3c: Forward merge from release/kubernetes-agent/v1 version 1.23.0. Adds support for setting annotations on script pods.

## 2.13.0

### Minor Changes

- 0aef448: Update Kubernetes Monitor subchart to 0.8.0

## 2.12.0

### Minor Changes

- 8cdadde: Forward merge from release/kubernetes-agent/v1 version 1.22.0. Includes:
  - Update Kubernetes SDK to support Kubernetes 1.32
  - Updated Tentacle to version 8.3.2757 which adds support for specifying init script pod resource limits.

## 2.11.3

### Patch Changes

- 441a785: Update Kubernetes Monitor subchart to 0.7.0

## 2.11.2

### Patch Changes

- 92dbe73: K8s Agent: Pass through Kubernetes Monitor enabled value for health checks

## 2.11.1

### Patch Changes

- b46689d: Update Kubenetes Monitor subchart to 0.5.0

## 2.11.0

### Minor Changes

- 163f262: Updated tentacle to fix issue with registering agent behind a proxy

## 2.10.1

### Patch Changes

- a47bb4e: Update Kubernetes Monitor subchart to 0.4.1

## 2.10.0

### Minor Changes

- c4b0588: Add kubernetes monitor as a sub-chart

## 2.9.1

### Patch Changes

- b43aef0: Forward merge from release/kubernetes-agent/v1 version `1.20.1`. Includes the increase to the maximum buffer size for the output scanner in the bootstrap runner.

## 2.9.0

### Minor Changes

- 42f1092: Forward merge from release/kubernetes-agent/v1 version `1.20.0`. Includes all changes in `1.20.0`.
  The agent now supports Kubernetes server versions 1.28–1.31.

## 2.8.2

### Patch Changes

- afb6307: Forward merge from release/kubernetes-agent/v1 version `1.19.2`. Includes all changes in `1.19.2`
- 3f5398d: Don't include an empty WorkerPools env var if not a worker

## 2.8.1

### Patch Changes

- d740589: Forward merge from release/kubernetes-agent/v1 version `1.19.1`. Includes all changes in `1.19.1`

## 2.8.0

### Minor Changes

- 3e1c038: Forward merge from release/kubernetes-agent/v1 version `1.19.0`. Includes all changes in `1.19.0`. This resolves [CVE-2024-12226](https://advisories.octopus.com/post/2024/sa2024-10/).

## 2.7.1

### Patch Changes

- 0778065: Update the AppVersion in Chart.yaml to match values file.

## 2.7.0

### Minor Changes

- a3c9dec: Add `agent.upgrade.dockerAuth` to support authenticating the docker calls when performing an automatic upgrade via Octopus Server.

## 2.6.0

### Minor Changes

- fcae25e: Add support for custom HTTP/S proxies in script pods

### Patch Changes

- 158ce99: Forward merge from release/kubernetes-agent/v1 version 1.18.1. Includes all changes in 1.18.1

## 2.5.0

### Minor Changes

- 05adf88: Forward merge from release/kubernetes-agent/v1 version `1.18.0`. Includes all changes in `1.18.0`

## 2.4.0

### Minor Changes

- c716463: Added migration to facilitate automatic upgrades from Kubernetes agent v1 to v2

## 2.3.0

### Minor Changes

- afc20ca: Forward merge from release/kubernetes-agent/v1 version `1.17.0`. Includes all changes in `1.17.0`

## 2.2.1

### Patch Changes

- 175382f: Forward merge from release/kubernetes-agent/v1 version `1.16.1`. Includes all changes in `1.16.1`

## 2.2.0

### Minor Changes

- 250a69d: Forward merge from release/kubernetes-agent/v1 version `1.16.0`. Includes all changes in `1.16.0`

## 2.1.0

### Minor Changes

- f335f7e: Forward merge from release/kubernetes-agent/v1 version `1.15.0`. Includes all changes in `1.15.0`

## 2.0.2

### Patch Changes

- c6d5a7c: Limit worker influence to local namespace

## 2.0.1

### Patch Changes

- 12fd2af: Forward merge from release/kubernetes-agent/v1. Includes changes added in `1.14.2`.

## 2.0.0

### ⚠️ Breaking Changes

Version 2 has breaking changes and upgrading from Version 1 requires manual migration of some `values.yaml` values. See the [migration notes](./migrations.md) for more information.

### Major Changes

- 0e07367: Version 2.0.0 of the Octopus Kubernetes Agent that can be installed as either a deployment target or a worker

## 2.0.0-alpha.6

### Patch Changes

- a304f07: Forward merge 1.14.1 to latest

## 2.0.0-alpha.5

### Patch Changes

- 470f8c0: Forward merge 1.14.0 to latest
- 811e5cb: Forward merge from 1.12.0 to latest

## 2.0.0-alpha.4

### Minor Changes

- e333640: Allow the container used by script pods to be modified for both worker and deployment target

### Patch Changes

- 63badca: Forward merge changes from 1.11.0 to latest

## 2.0.0-alpha.3

### Patch Changes

- 7c683de: Forward merge changes from 1.10.1 - 1.10.3 to latest

## 2.0.0-alpha.2

### Minor Changes

- 76ed7d8: Added role for worker pods

## 2.0.0-alpha.1

### Major Changes

- 0e07367: New values schema to allow installation of the agent as either an Octopus Deployment Target or Worker

### Patch Changes

- 84f7f36: Merge changes from 1.7.1 - 1.10.0 to latest

## 2.0.0-alpha.0

### Major Changes

- 05fa04c: Creating Kubernetes Agent v2 alpha prerelease

## 1.24.0

### Minor Changes

- 9dd2042: Add ability to bind PVC to specific PV via a volumeName

## 1.23.0

### Minor Changes

- 9825818: Add support for setting annotations on script pods

## 1.22.0

### Minor Changes

- d6875bd: Update Kubernetes SDK to support Kubernetes 1.32

### Patch Changes

- d6875bd: Updated Tentacle to version 8.3.2757 which adds support for specifying init script pod resource limits.

## 1.21.0

### Minor Changes

- f183f60: Updated tentacle to fix issue with registering agent behind a proxy

# 1.20.1

### Patch Changes

- 2d38d37: Updated Tentacle to version 8.2.2585, which increases the maximum buffer size for the output scanner in the bootstrap runner. This fix addresses an issue where output lines longer than the previous buffer limit would fail to be written.

## 1.20.0

### Minor Changes

- 38cc3ba: Bump Tentacle version to 8.2.2540 to upgrade the version of the dotnet Kubernetes Client library. The agent now supports Kubernetes server versions 1.28–1.31.

## 1.19.2

### Patch Changes

- 897f59b: Fix scriptPods.image.pullPolicy not working

## 1.19.1

### Patch Changes

- f55beb1: Updated tentacle to resolve errors during calls to 'helm registry login'

## 1.19.0

### Minor Changes

- 248cf57: Script pod logs are now encrypted to avoid leaking sensitive values. This resolves [CVE-2024-12226](https://advisories.octopus.com/post/2024/sa2024-10/).

## 1.18.2

### Patch Changes

- d117535: Add `agent.upgrade.dockerAuth` to support authenticating the docker calls when performing an automatic upgrade via Octopus Server.

## 1.18.1

### Patch Changes

- dc457bf: Fix a quoting issue with agent.pollingProxy.port

## 1.18.0

### Minor Changes

- 5700a03: Bump Tentacle version to 8.2.2264 to include an image pull policy workaround that ensures the agent uses the latest tools image

## 1.17.0

### Minor Changes

- eb21cd0: Updated the Kubernetes Tentacle to version 8.2.2165, featuring an upgrade from .NET 6 to .NET 8

  - This release now uses a Debian 12 base image and features a libssl update from v1 to v3

  Introduced support for an optional tag suffix in the agent image tag configuration within the Helm chart

  - This allows for choosing an alternative base distribution for the Kubernetes Tentacle image based on available options

## 1.16.1

### Patch Changes

- d2f8ec0: Bump kubernetes-agent-nfs-watchdog default tag to 0.2.0

## 1.16.0

### Minor Changes

- ad4867c: Add service account used by Octopus Server to perform automatic upgrades

## 1.15.0

### Minor Changes

- 0392acd: Update kubernetes-agent-tentacle to 8.1.2099. Adds support for writing script pod events to task log and also improves script cancellation performance.

## 1.14.2

### Patch Changes

- c8a7da3: Update kubernetes-agent-tentacle to 8.1.2049. Removes the `KubernetesScriptServiceV1Alpha` service

## 1.14.1

### Patch Changes

- 0759fd8: Update kubernetes-agent-tentacle to 8.1.2007. Fixes an issue executing script pods when running on AWS Bottlerocket nodes

## 1.14.0

### Minor Changes

- ca3bc11: Add support for setting pods securityContext

## 1.13.0

### Minor Changes

- 196860f: Add ability to use existing secrets to provide auth info

## 1.12.0

### Minor Changes

- 083adb9: Backport support for changing script pod image, tag and pullPolicy

## 1.11.0

### Minor Changes

- 957f69f: Add ability to modify agent, nfs and script pod affinity and tolerations

## 1.10.3

### Patch Changes

- 2c2ef66: Update Tentacle to fix an issue with whitespaces not being allowed in target roles

## 1.10.2

### Patch Changes

- d8a1a9f: Fix an issue with imagePullSecrets not being serialized in environment variables correctly

## 1.10.1

### Patch Changes

- d8a61d7: Make v1 agent compatible with the latest changes to Tentacle

## 1.10.0

### Minor Changes

- 7999cbb: Support configuring script pod resource requirements

## 1.9.0

### Minor Changes

- 8536deb: Add support for polling proxy server via new `agent.pollingProxy` values

## 1.8.0

### Minor Changes

- 34ce133: Add support for using imagePullSecrets on script pods

## 1.7.3

### Patch Changes

- 43916a4: Bump the failureThreshold to fix startup probe problems

## 1.7.2

### Patch Changes

- 3a77239: Don't include the Readme.md.gotmpl in the chart

## 1.7.1

### Patch Changes

- 9a97ae5: Update kubernetes-agent-tentacle to 8.1.1858. Includes fix to mkdir failing during container startup

## 1.7.0

### Minor Changes

- f94b12d: Update kubernetes-agent to use a nfs-watchdog v0.1.0 which raises event on nfs timeout
- 2f1c75c: Add support for custom Octopus Server SSL/TLS certificates"

## 1.6.0

### Minor Changes

- b5be5dd: Upgrade kubernetes-agent-tentacle to 8.1.1819 to support more reliable server-driven upgrades

## 1.5.0

### Minor Changes

- 0705c16: Add value to enable/disable scraping of events for agent metrics
- 28417de: Add permission for octopus user to raise kubernetes events

## 1.4.0

### Minor Changes

- 3b236a0: Rename kubernetes-tentacle container image to kubernetes-agent-tentacle.
  - This includes upgrading the Tentacle version to 8.1.1757 which is published with the new container name.
  - The updated Tentacle version also allows Server to verify that the right number of ServerCommsAddresses have been provided to the Agent in scenarios where Octopus Server is running in an High Availability configuration.
- e2546e0: Add Tenant and Tenant Tag support

### Patch Changes

- 54de125: Bump Tentacle version to 8.1.1734 to include a fix for a memory leak when StartScript fails.

## 1.3.0

### Minor Changes

- f311893: Bump Tentacle version to 8.1.1717 to include ability to update ServerCommsAddresses via `helm upgrade` command
- a9bac56: Add startupProbe to Kubernetes tentacle container to ensure helm upgrade command doesn't return successful when the tentacle failed to initialise. This includes a Tentacle version bump to 8.1.1727.

## 1.2.0

### Minor Changes

- e9933f2: Add ConfigMap for agent metrics persistence
- 62d9919: Add HA Server configuration support

## 1.1.1

### Patch Changes

- 1b1d00d: Update Tentacle to 8.1.1682. Adds support for Calamari Package Retention on Kubernetes Agent

## 1.1.0

### Minor Changes

- 673cc6d: Added new optional values to allow configuring a server subscription id and a custom certificate

## 1.0.4

### Patch Changes

- 578d0c7: Update Tentacle to 8.1.1588. Introduces new `KubernetesScriptServiceV1`.

## 1.0.3

### Patch Changes

- 55eeab8: Allow users to specify custom machine policy
- 2b7eebf: Update Tentacle to 8.1.1567. Contains a number of improvements and bug fixes. Fixes a known issue when registering where environment slugs don't match the environment names

## 1.0.2

### Patch Changes

- 35a41d1: Update Tentacle reference to include NullReferenceException fix when finishedAt is null on the container state

## 1.0.1

### Patch Changes

- 45486a4: Tighten affinity rules for NFS and tentacle pods to only run on linux/amd64 and linux/arm64 nodes

## 1.0.0

### Major Changes

- 6350a9a: Version 1.0.0 release of the Octopus Kubernetes Agent helm chart

## 0.10.0

### Minor Changes

- e1c797b: Bump tentacle version for release

## 0.9.1

### Patch Changes

- 6c6e680: Fix test

## 0.9.0

### Minor Changes

- 7ba7548: Update to add required PERSISTENTVOLUMESIZE variable

## 0.8.4

### Patch Changes

- 60ee237: Add the nfs size as a suffix to the nfs pv and pvc name

## 0.8.3

### Patch Changes

- 7af696d: Increased the default NFS volume size to 10Gi from 1Gi
- 6e6eecb: Update Kubernetes Tentacle version to 8.1.1478

## 0.8.2

### Patch Changes

- 7b0196c: Change NFS back to use emptyDir

## 0.8.1

### Patch Changes

- 35481bb: Remove ability to change replica count
- ee2b7b6: Update Tentacle to 8.1.1426

## 0.8.0

### Minor Changes

- cf1cabf: Added an NFS watchdog to remove pods should the NFS mount become stale

## 0.7.2

### Patch Changes

- 8f172c2: Default to 5 polling connections
- 8043244: Support providing test data to ConfigMap

## 0.7.1

### Patch Changes

- c3854b9: Pass pod PVC name, not JSON. Also bump Tentacle to support this.
- 8df264c: Added PDB to script pods to prevent accidental failure of tasks whilst draining nodes
- b1d4c71: Give Tentacle permissions to read Pod logs

## 0.7.0

### Minor Changes

- fbd30ab: Update Tentacle to 8.1.1249 with breaking services change

### Patch Changes

- 3348ca4: Make pods that mount NFS timeout when trying to access an invalid NFS mount rather than making the NFS pod wait for all connections to close before shutting down.
- 7a6fb3b: Change the nfs deployment to be a stateful set because it makes more sense considering only one nfs pod can be run at a time.
- bfa6c53: Use a clusters default storage class for the base nfs directory rather than emptyDir.

## 0.6.3

### Patch Changes

- 584a339: Reduce termination grace period for nfs server deployment
- 4b5ed09: Update nfs server image tag to versioned option
- 18c71ea: Disable lookupcache so Tentacle can discover new files without a long delay
- 1f1134a: Bump Tentacle to 8.1.1079

## 0.6.2

### Patch Changes

- c6dafcd: Add default namespace environment variable to tentacle deployment

## 0.6.1

### Patch Changes

- cf25f61: Fix bug where pod monitoring stops working after a while

## 0.6.0

### Minor Changes

- 5abc402: Change to use NFS CSI driver
- 21c8361: Add support for defining custom storage volume

## 0.5.0

### Minor Changes

- 8e860f9: Update to support change to execute scripts in Pods, not Jobs

## 0.4.5

### Patch Changes

- 4f91e87: Bump tentacle version to test automated updates in server

## 0.4.4

### Patch Changes

- 51f9022: The tentacle now has the information required for automatic updates.

## 0.4.3

### Patch Changes

- 23a869b: Update repo to use pnpm and changesets at the root instead of in the /charts/kubernetes-agent directory

## 0.4.2

### Patch Changes

- fefaa57: Update nfsPort to nfsIpAddress as it's an IP address, not a port
- 38055ca: Add support for defining custom job service account permissions

## 0.4.1

### Patch Changes

- 73c9d72: bump tentacle version to include pod logs in kubernetes tentacle
- 89ecb22: Add Tentacle LogLevel environment variable
- ce18bb5: Delay shutting down the NFS Pod so the Tentacle Pod doesn't hang while umount'ing the fileshare

## 0.4.0

### Minor Changes

- bbf3212: Change to use DockerHub hosted container images

## 0.3.2

### Patch Changes

- 189042a: Add resource requests for NFS & Agent containers

## 0.3.1

### Patch Changes

- 0a30686: Set node OS affinity to Linux nodes

## 0.3.0

### Minor Changes

- 688221c: Update helm chart to use multi-arch tentacle container image
- fd250f1: Update agent to use Octopus managed nfs server container image

### Patch Changes

- 6e08ed3: Bump Tentacle to 8.1.761

## 0.2.2

### Patch Changes

- 131ed17: Update Tentacle to 8.1.671

## 0.2.1

### Patch Changes

- 8914fab: Rename resources to use octopus-agent as default

## 0.2.0

### Minor Changes

- 99ce92b: Rename Kubernetes Tentacle to Kubernetes Agent

## 0.1.6

### Patch Changes

- a9e3e54: Add secret API permissions for tentacle service account
- 014fd14: Adding blank tentacle-secret for storing the tentacle machinekey

## 0.1.5

### Patch Changes

- b642711: Update Tentacle to 8.1.602
- 5851fbd: Add Helm release name and chart version to Tentacle environment variables

## 0.1.4

### Patch Changes

- bb9057a: Add minor changes to support tentacles using configmap for configuration storage

## 0.1.3

### Patch Changes

- fbc9a76: Add support for changeset versioning
- 3704cc2: Allow Tentacle service account to interact with config maps

## 0.1.2

### Patch Changes

- Update Job ClusterRole and ClusterRoleBinding names to be clearer

## 0.1.1

### Patch

- Update appVersion to 8.1.33

## 0.1.0

### Minor

- First pre-release version of the Kubernetes Tentacle Helm Chart
