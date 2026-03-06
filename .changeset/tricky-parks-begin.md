---
"kubernetes-agent": minor
---

Add stuck pending script pod monitoring background process. If a script pod has been in the pending state for more than the defined number of minutes (default: 60 minutes), the script pod is terminated.
