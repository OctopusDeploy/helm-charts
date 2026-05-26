---
"kubernetes-agent": minor
---

Add a `livenessProbe` for polling Kubernetes Agents using the new `tentacle livez` exec command. Detects "pod Running but Tentacle process stuck" — deadlock, GC stall, frozen polling thread — by reading the freshness of a heartbeat file written by the agent. Only takes effect when `agent.serverCommsAddress` or `agent.serverCommsAddresses` is set (polling mode); listening Tentacles are unchanged. Requires Tentacle image >= 9.2.3963. All four knobs are exposed under `agent.livenessProbe.*`.
