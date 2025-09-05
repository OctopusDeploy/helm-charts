---
"kubernetes-agent": patch
---

Fix an issue where the new `scriptpodtemplates` CRD was not in the special `crds` directory, causing issues when installing/upgradig in a cluster where the CRD already existed.
