# Forward merge release branches

When a new major version of the `kubernetes-agent` chart is released (e.g. `v3`), we create a release branch for the previous major release in the format `release/kubernetes-agent/v(N-1)` (e.g. `release/kubernetes-agent/v2`).
This is because we still have versions of Octopus Server that install the previous major version by default and we need to continue to support that "release stream".

When we want to release changes to the previous version, those changes need to be made against the release branch for that release stream.

Once those changes are merged into the release branch and "Version Charts" PR is merged, it will release a new version in that release stream.

However, if we also want to apply the same change (such as update to the `kubernetes-agent-tentacle` container) to the current release (e.g. `main`), we want to perform a _forward merge_ of the release branch into `main`, not a cherry pick or separate commit.

We do this so changes to the chart don't diverge too much (except when expected) and also to keep a continuous history & changelog in the `main` of all changes for all versions.

## Process

When we are going to change the chart, first we need to decide if it's a change that relevant to all active versions, or just for a specific version.

**Examples:**

- Bug fix in the Tentacle application: This is relevant to all versions
- Documentation change in `values.yaml` for specific version: Relevant onto the specific version
- Change to `values.yaml` that affects all versions: Relevant to all versions

If the decision is that the change only affects a specific version, then no forward merge process needs to occur and the change can just be applied to the branch for that version.

If the change affects all active versions, then we should make the change in the _oldest_ release branch and forward merge through to `main`. The following steps detail the process as well as key things to consider.

For the following steps, we are going to assume that `v2` is the "LTS" release and `v3` is the current `main` release.

### Step 1 - Make the change against v2 branch

The changes should be made against the `release/kubernetes-agent/v2` branch. If this is a change to the application, a changeset is required. If it's just a documentation or workflow change, no changeset is needed.

Example PR: [#571](https://github.com/OctopusDeploy/helm-charts/pull/571)

> [!IMPORTANT]
> This pull request should be merged as a **Squash** commit

### Step 2 - Release the v2 changes

When the original PR is merged and had a changeset, the changeset bot will create a PR to release the changes for `v2`.

Example PR: [#565](https://github.com/OctopusDeploy/helm-charts/pull/565)

> [!IMPORTANT]
> This pull request should be merged as a **Squash** commit

### Step 3 - Merge release branch into branch off `main`

To forward merge the release branch, you will need to create a branch off `main`. This name can explicitly indicate a forward merge, e.g. `ap/forward-merge-v2-20260504`,

It is extremely likely you will need to manually fix some merge conflicts during this forward merge. Obviously this can be due to the main changes between versions, but also you run into conflicts as the chart version is different and that ends up being through all the unit test specs.

Another source of conflicts is [CHANGELOG.md](../CHANGELOG.md). You will likely need to manually move the new `v2` changelog under the `v3` changes. We do this so we have a linear and sequential versions.

Once manually resolved and merged, you should _just_ have your changes, adjusted to fit the target branch.

If the original PR had a changeset, then a changeset will need to be added to this forward merge PR. This is so target version also gets the version bump.

Example PR: [#XXX](https://github.com/OctopusDeploy/helm-charts/pull/XXX)

> [!WARNING]
> This pull request MUST be merged as a normal merge commit, NOT a **Squash** commit.
> This is so that the original commit SHA in the release branch is maintained through to `main`.

## Step 4 - Release the v3 changes

When the merge forward PR is merged and if it had a changeset, the changeset bot will create a PR to release the changes for `v3`.

Example PR: [#XXX](https://github.com/OctopusDeploy/helm-charts/pull/XXX)

> [!IMPORTANT]
> This pull request should be merged as a **Squash** commit