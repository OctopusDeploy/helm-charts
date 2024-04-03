---
"kubernetes-agent": patch
---

Change the nfs deployment to be a stateful set because it makes more sense considering only one nfs pod can be run at a time.
