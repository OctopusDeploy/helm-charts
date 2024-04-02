---
"kubernetes-agent": patch
---

Make pods that mount NFS timeout when trying to access an invalid NFS mount rather than making the NFS pod wait for all connections to close before shutting down.
