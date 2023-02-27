# Octopus Deploy Helm Chart

This chart installs [Octopus Deploy](https://octopus.com) into a Kubernetes cluster using the [Helm](https://helm.sh) package manager.

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
