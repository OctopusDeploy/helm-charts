---
"kubernetes-agent": patch
---

Bump Kubernetes Monitor chart to 0.40.0. This fixes a bug where child resources deleted while the monitor is offline linger in live status until the next periodic snapshot. See https://github.com/OctopusDeploy/helm-charts/issues/731 for more details.
