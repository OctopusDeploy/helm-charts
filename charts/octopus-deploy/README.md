# Octopus Deploy Helm Chart

This chart installs [Octopus Deploy](https://octopus.com) into a Kubernetes cluster using the [Helm](https://helm.sh) package manager.

The chart packages are available on [DockerHub](https://hub.docker.com/r/octopusdeploy/octopusdeploy-helm).

## Usage

### tl;dr

```
helm install octopus-deploy oci://registry-1.docker.io/octopusdeploy/octopusdeploy-helm  --values values.yaml
```

### Pre-requisites

#### Database 

[Octopus requires a SQL Server database](https://octopus.com/docs/installation/sql-server-database).  

SQL Server can be [installed via Helm](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-containers-deploy-helm-charts-kubernetes) into your Kubernetes cluster.   
Alternatively, there are many other [installation options](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-setup), or your cloud provider may offer a hosted option. 

**You will need the database connection string.** 
This should look something like:

```
Server=tcp:octopus-deploy.database.windows.net,1433;Initial Catalog=OctopusDeploy;Persist Security Info=False;User ID=octopus-deploy;Password={your_password};Encrypt=True;Connection Timeout=30;
```

#### Master key

[Octopus uses a master key to encrypt sensitive values](https://octopus.com/docs/security/data-encryption). 

**You must generate a master key, and store it safely.**  

The master key can be generated with

```
openssl rand -base64 16
```

### Values

A minimal example of a values file for installing this chart:

```
octopus:
  acceptEula: "Y" # It is required to accept the Octopus EULA https://octopus.com/legal/customer-agreement
  masterKey: <generated master key - base64> 
  databaseConnectionString: <your sql server database connection string> 
  licenseKeyBase64: <your base64 encoded license key>   
  username: <admin username>
  password: <admin password> 
  packageRepositoryVolume:
    size: 1Gi 
    storageClassName: "azure-file"
    storageAccessMode: ReadWriteOnce
  # The volume used for persisting deployment artifacts: https://octopus.com/docs/projects/deployment-process/artifacts
  artifactVolume:
    size: 1Gi 
    storageClassName: "azure-file"
    storageAccessMode: ReadWriteOnce
 # Volume used for task logs: https://octopus.com/docs/support/get-the-raw-output-from-a-task
  taskLogVolume: 
    size: 1Gi 
    storageClassName: "azure-file"
    storageAccessMode: ReadWriteOnce

```

### Ingress

There are two types of traffic which you will typically want to allow from outside the cluster:
- HTTP requests to the Octopus web portal 
- Polling Tentacle communication 

For the web portal, a common approach is to use a [Kubernetes ingress resource](https://kubernetes.io/docs/concepts/services-networking/ingress/). This requires an [ingress controller](https://kubernetes.io/docs/concepts/services-networking/ingress-controllers/) to be running in your cluster.

An example of a values file which configures ingress for the web portal using [NGINX](https://kubernetes.github.io/ingress-nginx/) is shown below:

```
octopus:
  ingress:
    enabled: true
    annotations: 
      kubernetes.io/ingress.class: nginx
    path: /
    hosts:
      - octopus.example.com 
```

#### Polling Tentacles

Polling Tentacles are more complicated than web traffic, as [polling tentacles must poll every Octopus server node](https://octopus.com/docs/administration/high-availability/maintain/polling-tentacles-with-ha).

For this reason, this Helm chart doesn't provision an ingress resource for polling tentacles.  

If the chart is configured to create a single Octopus node (`replicaCount: 1`) then the polling tentacle port is exposed on the same service as the Octopus server.  If a replica count of greater than 1 is specified, then a kubernetes service will be created for each node.  When registering your polling tentacles, you will need to configure them to poll each node. 

### Persistent Volumes

This chart requires persistent volumes to store:

- [Packages](https://octopus.com/docs/packaging-applications/package-repositories/built-in-repository) 
- [Artifacts](https://octopus.com/docs/projects/deployment-process/artifacts)
- [Task Logs](https://octopus.com/docs/support/get-the-raw-output-from-a-task)

These volumes are shared across Octopus nodes. 

For each, an optional persistent volume claim class name can be supplied. This storage class must support ReadWriteMany access modes when the chart is configured to create more than one Octopus node (`replicaCount` > 0). 
ReadWriteOnce or ReadWriteMany can be used for single node clusters.
A dash (i.e. "-") means use an empty string as the storageClass attribute. This effectively means there is no automatic provisioning of persistent volumes, and the volumes need to be created externally outside of this chart.
A [falsy value](https://helm.sh/docs/chart_template_guide/control_structures/#ifelse) means the storageClass attribute is not defined, and the default value may be used. Most cloud providers support automatic provisioning of ReadWriteOnce volumes. 

An example of configuring the persistent volumes is shown below:

```
octopus:
  packageRepositoryVolume:
    size: 20Gi 
    storageClassName: "azure-file"
    storageAccessMode: ReadWriteOnce
  artifactVolume:
    size: 1Gi 
    storageClassName: "azure-file"
    storageAccessMode: ReadWriteOnce
  taskLogVolume: 
    size: 1Gi 
    storageClassName: "azure-file"
    storageAccessMode: ReadWriteOnce
```

