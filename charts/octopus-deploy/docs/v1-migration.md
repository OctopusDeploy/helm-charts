# V1 Migration
Previously, the Octopus Deploy helm chart was versioned along with Octopus Deploy. This made it difficult to upgrade 
the chart separately from the Octopus Deploy version, and vice versa. To address this, along with other difficulties, 
we have migrated to semantic versioning for this helm chart. This also includes new chart functionality, allowing you 
to get started with Octopus Deploy faster and easier. The helm chart will be published at `1.0.0` and will be bumped
according to [semantic versioning rules](https://semver.org/#summary).

## New Chart Features
- MSSQL is now a sub-chart, allowing you to quickly start up Octopus Deploy with a new database
- The chart now automatically generates a master key and admin password for you if they are not provided
- Various bugfixes and documentation improvements

## Breaking Changes
- We have updated the labels to include a `component` to safely support additional components installed with the helm 
chart (like MSSQL)
  - This means that the StatefulSet created by the old chart will not be automatically upgraded and needs to be deleted  

_If you see this, you'll get this error:_
```
Error: UPGRADE FAILED: cannot patch "<release-name>" with kind StatefulSet: StatefulSet.apps "<release-name>" is invalid: spec: Forbidden: updates to statefulset spec for fields other than 'replicas', 'template', 'updateStrategy', 'persistentVolumeClaimRetentionPolicy' and 'minReadySeconds' are forbidden
```
## Migration Steps
### 1. Determine your current chart version
You will only need to perform this migration if you are using a version of the chart that is versioned along with 
Octopus Deploy. This will be something like `2024.1.1234`.

### 2. Delete the old StatefulSet
Due to changes in labelling, the chart is not able to automatically upgrade the old StatefulSet. This is due to label 
changes in the chart. You will need to delete the old StatefulSet manually. This will not delete your data, as it is 
safely stored in your DB and filesystem.

### 3. Install the new chart
You can now upgrade to the new chart version:
```bash
helm upgrade --values values.yaml <release-name> oci://ghcr.io/octopusdeploy/octopusdeploy-helm 
```