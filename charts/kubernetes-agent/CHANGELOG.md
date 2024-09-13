# kubernetes-agent

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
