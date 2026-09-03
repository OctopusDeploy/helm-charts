---
"kubernetes-agent": minor
---

Add a `readinessProbe` for polling Tentacles. Polling Tentacles expose no port to check directly, so readiness is determined by an exec check (`ps aux | grep Tentacle`) confirming the Tentacle process is running. All knobs are exposed under `agent.readinessProbe.*`.
