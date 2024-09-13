---
"kubernetes-agent": minor
---

Updated the Kubernetes Tentacle to version 8.2.2165, featuring an upgrade from .NET 6 to .NET 8
- This release now uses a Debian 12 base image and features a libssl update from v1 to v3

Introduced support for an optional tag suffix in the agent image tag configuration within the Helm chart
- This allows for choosing an alternative base distribution for the Kubernetes Tentacle image based on available options
