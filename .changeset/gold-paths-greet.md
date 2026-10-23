---
"kubernetes-agent": patch
---

Add env var to tentacle deployment indicating what the storageClassName is. This value doesn't exactly represent the value in the PVC or values and is used in Octopus Server.
