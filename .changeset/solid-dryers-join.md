---
"octopus-deploy": minor
---

Fix server-log-volume to use global storage class name

Previously the server-log PVC omitted storageClassName when no explicit
octopus.serverLogVolume.storageClassName was provided, ignoring the
global override.