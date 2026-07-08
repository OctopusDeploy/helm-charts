[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [string]
    $Version
)

# Read the current tentacle version from Chart.yaml
$chartYaml = Get-Content -Path "$PSScriptRoot/Chart.yaml"
$currentVersionLine = $chartYaml | Where-Object { $_.StartsWith("appVersion") } | Select-Object -First 1

if ($currentVersionLine -notmatch 'appVersion:\s*"(\d+\.\d+\.\d+)"') {
    throw "Could not find appVersion in Chart.yaml"
}

$currentVersion = $Matches[1]

Write-Output "Current version: $currentVersion"
Write-Output "New version: $Version"

$escapedCurrentVersion = [regex]::Escape($currentVersion)

# Update all yaml files + snapshots with the new version
Get-ChildItem -Path "$PSScriptRoot" -Recurse -Include "*.yaml", "*.yaml.snap" |
    Where-Object { $_.FullName -notmatch '\\node_modules\\' } |
    ForEach-Object {
        $file = $_
        $content = Get-Content -Path $file -Raw
        if ($content -match $escapedCurrentVersion) {
            $content -replace $escapedCurrentVersion, $Version | Set-Content -Path $file -NoNewline
            Write-Output "Updated: $($file.FullName)"
        }
    }
