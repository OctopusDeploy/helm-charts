---
"kubernetes-agent": major
---

Changes the default storage from NFS to default single node `ReadWriteOnce`. This is being done as we have had increasing reports of the NFS server's stability, performance, and security concerns.

The primary changes to the default values file are:
- A new field: `persistence.nfs.enabled`, which is set to `false`
- Changed values: `persistence.accessModes` which is now set to `["ReadWriteOnce"]`

Existing v2 agents that use NFS and upgrade via Octopus Server (with the values file migration) will not change to this new storage, however new installations will do this.
The result of this change is that script pods are now scheduled, by default, on the same node as the tentacle pod. This reduces/removes some of the scalability that NFS provided, but comes with increased performance, reduced footprint and reduced security footprint.

To enable scaling of the script pods across nodes, a `persistence.storageClassName` should be set to the name of a storage class that provides `ReadWriteMany` access modes, and the `persistence.accessModes` should be set to `["ReadWriteMany"]`.
