---
"kubernetes-agent": patch
---

Update Kubernetes monitor to 0.36. This changes the gRPC connection to use compression by default.

Note: If you are using a custom proxy between the monitor and Octopus Server, you may experience issues. Please contact support@octopus.com
