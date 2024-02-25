# kubernetes-agent

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
