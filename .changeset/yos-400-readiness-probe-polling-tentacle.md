---
"kubernetes-agent": minor
---

Add an opt-in `readinessProbe` for polling Tentacles, disabled by default. Polling Tentacles expose no port to check directly, so readiness is determined by an exec check (`ps aux | grep Tentacle`) confirming the Tentacle process is running. Set `agent.readinessProbe.enabled: true` to turn it on; all knobs are exposed under `agent.readinessProbe.*`.
