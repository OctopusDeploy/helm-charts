$NAMESPACE="octopus-agent-theagent"
$RELEASE="theagent"
$CHART="oci://docker.packages.octopushq.com/kubernetes-agent"

[string]$CURRENT_MANUALLY_SET_VALUES = helm get values --namespace $NAMESPACE $RELEASE -o json
#Write-host  $CURRENT_MANUALLY_SET_VALUES

$JSON_OBJECT = ConvertFrom-Json $CURRENT_MANUALLY_SET_VALUES

$INITIAL_VALUES = [PSCustomObject]@{
    defaultNamespace = $JSON_OBJECT.agent.defaultNamespace
    environments = $JSON_OBJECT.agent.targetEnvironments
    tags = $JSON_OBJECT.agent.targetRoles
    tenantTags = $JSON_OBJECT.agent.targetTenantTags
    tenantedDeploymentParticipation = "Untenanted"
    tenants = $JSON_OBJECT.agent.targetTenants
}

$INITIAL_VALUES | ForEach-Object {
    # Get array of names of object properties that can be cast to boolean TRUE
    # PSObject.Properties - https://msdn.microsoft.com/en-us/library/system.management.automation.psobject.properties.aspx
    $NonEmptyProperties = $_.psobject.Properties | Where-Object {$_.Value} | Select-Object -ExpandProperty Name
    $REDUCED_INITIAL_VALUES = $_ | Select-Object -Property $NonEmptyProperties
}

$DEPLOYMENT_TARGET = [PSCustomObject]@{
  enabled=$true;
  initial = $REDUCED_INITIAL_VALUES
}


$JSON_OBJECT.agent | Add-Member -MemberType NoteProperty -TypeName string -Name name -Value $JSON_OBJECT.agent.targetName
$JSON_OBJECT.agent | Add-Member -MemberType NoteProperty -TypeName psobject -Name deploymentTarget -Value $DEPLOYMENT_TARGET
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetName") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetEnvironments") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetRoles") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetTenantTags") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetTenantedDeploymentParticipation") 
$JSON_OBJECT.agent.PSObject.Properties.Remove("targetTenants") 

$JSON_OBJECT | ConvertTo-Json -Depth 5
