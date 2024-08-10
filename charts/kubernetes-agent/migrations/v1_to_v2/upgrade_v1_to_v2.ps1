$NAMESPACE="octopus-agent-theagent"
$RELEASE="theagent"
$CHART="oci://docker.packages.octopushq.com/kubernetes-agent"

[string]$CURRENT_MANUALLY_SET_VALUES = helm get values --namespace $NAMESPACE $RELEASE -o json
Write-host  $CURRENT_MANUALLY_SET_VALUES

$JSON_OBJECT = ConvertFrom-Json $CURRENT_MANUALLY_SET_VALUES

$DEPLOYMENT_TARGET = [PSCustomObject]@{
  enabled=$true;
  initial = [PSCustomObject]@{
    defaultNamespace = $JSON_OBJECT.agent.defaultNamespace
    environments = $JSON_OBJECT.agent.targetEnvironments
    tags = $JSON_OBJECT.agent.targetRoles
    tenantTags = $JSON_OBJECT.agent.targetTenantTags
    tenantedDeploymentParticipation = "Untenanted"
    tenants = $JSON_OBJECT.agent.targetTenants
  }
}
$JSON_OBJECT.agent | Add-Member -MemberType NoteProperty -TypeName string -Name name -Value $JSON_OBJECT.agent.targetName
$JSON_OBJECT.agent | Add-Member -MemberType NoteProperty -TypeName psobject -Name deploymentTarget -Value $DEPLOYMENT_TARGET
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetName") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetEnvironments") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetRoles") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetTenantTags") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetTenantedDeploymentParticipation") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetTenants") 
#$JSON_OBJECT.agent.deploymentTarger
#$JSON_OBJECT.agent | Add-Member -MemberType NoteProperty -TypeName object -Name deploymentTarget
#$JSON_OBJECT.agent.deploymentTarget = New-Object PSObject -Property @{
#    enabled = true
#}
#$JSON_OBJECT.agent.Name
$JSON_OBJECT | ConvertTo-Json -Depth 5
