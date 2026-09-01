---
"kubernetes-agent": patch
---

Bump Kubernetes Monitor chart to 0.40.0.

- Fixes a bug where child resources deleted while the monitor is offline linger in live status until the next periodic snapshot
- Adds a new `ReplaceDesiredResourcesCommand` for Octopus Server to send all desired resources together when monitor connects
- See https://github.com/OctopusDeploy/helm-charts/issues/731 for more details
