---
"kubernetes-agent": minor
---

Add support for new Calamari image volume feature. By setting `scriptPods.calamariImageVolume.enabled` to `true`, the Calamari tooling is loaded into the script pod using the Kubernetes Image Volume as a read-only volume mount.

> [!IMPORTANT]
> This feature requires Kubernetes `1.35` or later as it requires the `ImageVolume` feature gate. If enabled on an older cluster, a warning will be written into the task log and the existing Octopus Server-distributed mechanism will be used.
>
> This feature also requires Octopus Server `2026.3.892` or later.

There are two other configuration options for controlling the image being loaded:

- `scriptPods.calamariImageVolume.image.repository` - Sets the repository where the Calamari image is loaded from. See the [documentation](https://oc.to/k8s-agent-calamari-image-volume) for more information about changing this.
- `scriptPods.calamariImageVolume.image.pullPolicy` - Defines the pull policy for the Calamari image