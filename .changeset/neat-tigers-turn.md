---
"kubernetes-agent": minor
---

Add startupProbe to Kubernetes tentacle container to ensure helm upgrade command doesn't return successful when the tentacle failed to initialise. This includes a Tentacle version bump to 8.1.1727.
