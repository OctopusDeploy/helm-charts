---
"kubernetes-agent": minor
---

Add `agent.strategy` to set the agent Deployment's rollout strategy. Set `{type: Recreate}` when the agent uses a ReadWriteOnce volume on a cluster with more than one node, so an upgrade doesn't wait on the volume until it times out.
