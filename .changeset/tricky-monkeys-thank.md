---
"kubernetes-agent": minor
---

Rename kubernetes-tentacle container image to kubernetes-agent-tentacle.
- This includes upgrading the Tentacle version to 8.1.1757 which is published with the new container name.
- The updated Tentacle version also allows Server to verify that the right number of ServerCommsAddresses have been provided to the Agent in scenarios where Octopus Server is running in an High Availability configuration.
