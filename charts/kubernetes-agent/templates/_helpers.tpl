{{/*
The name for the agent
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
*/}}
{{- define "kubernetes-agent.name" -}}
{{ .Values.nameOverride | default "octopus-agent" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- define "kubernetes-agent.fullName" -}}
{{ (printf "%s-%s" ( include "kubernetes-agent.name" .) .Release.Name) | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "kubernetes-agent.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "kubernetes-agent.labels" -}}
helm.sh/chart: {{ include "kubernetes-agent.chart" . }}
{{ include "kubernetes-agent.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "kubernetes-agent.selectorLabels" -}}
app.kubernetes.io/name: {{ include "kubernetes-agent.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use for Tentacle
*/}}
{{- define "kubernetes-agent.serviceAccountName" -}}
{{- .Values.agent.serviceAccount.name | default (printf "%s-tentacle" (include "kubernetes-agent.name" .)) }}
{{- end }}

{{/*
Create the name of the service account to use for script pods
*/}}
{{- define "kubernetes-agent.scriptPodServiceAccountName" -}}
{{- .Values.scriptPods.serviceAccount.name | default (printf "%s-scripts" (include "kubernetes-agent.name" .)) }}
{{- end }}

{{/*
Used for the pod cluster role & clusterrole binding as they are not namespaced.
*/}}
{{- define "kubernetes-agent.scriptPodServiceAccountFullName" -}}
{{- printf "%s-%s" ( include "kubernetes-agent.scriptPodServiceAccountName" .) .Release.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create the name of the service account used by Octopus Server to perform automatic upgrades
*/}}
{{- define "kubernetes-agent.autoUpgraderServiceAccountName" -}}
{{- print "octopus-agent-auto-upgrader" }}
{{- end }}

{{/*
Used for the auto upgrader service account cluster role & clusterrole binding as they are not namespaced
*/}}
{{- define "kubernetes-agent.autoUpgraderServiceAccountFullName" -}}
{{- printf "%s-%s" ( include "kubernetes-agent.autoUpgraderServiceAccountName" .) .Release.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create the name of the pod cluster role to use
*/}}
{{- define "kubernetes-agent.scriptPodClusterRoleName" -}}
{{- printf "%s-role" (include "kubernetes-agent.scriptPodServiceAccountFullName" .) }}
{{- end }}

{{/*
Create the name of the pod cluster role for deleting pods
*/}}
{{- define "kubernetes-agent.scriptPodDeleterClusterRoleName" -}}
{{- printf "%s-delete-role" (include "kubernetes-agent.scriptPodServiceAccountFullName" .) }}
{{- end }}

{{/*
Create the name of the cluster role for performing auto upgrades
*/}}
{{- define "kubernetes-agent.autoUpgraderClusterRoleName" -}}
{{- printf "%s-role" (include "kubernetes-agent.autoUpgraderServiceAccountFullName" .) }}
{{- end }}

{{/*
Create the name of the auto upgrader cluster role binding to use
*/}}
{{- define "kubernetes-agent.autoUpgraderClusterRoleBindingName" -}}
{{- printf "%s-binding" (include "kubernetes-agent.autoUpgraderServiceAccountFullName" .) }}
{{- end }}

{{/*
Create the name of the pod cluster role binding to use
*/}}
{{- define "kubernetes-agent.scriptPodClusterRoleBindingName" -}}
{{- printf "%s-binding" (include "kubernetes-agent.scriptPodServiceAccountFullName" .) }}
{{- end }}

{{/*
The name of the secret to store the authentication information (bearer token/api key)
*/}}
{{- define "kubernetes-agent.secrets.serverAuth" -}}
{{- printf "%s-tentacle-server-auth" ( include "kubernetes-agent.name" . ) }}
{{- end }}

{{/*
The name of the secret to store the agent's base64 certificate
*/}}
{{- define "kubernetes-agent.secrets.certificate" -}}
{{- printf "%s-tentacle-certificate" ( include "kubernetes-agent.name" . ) }}
{{- end }}

{{/*
The name of the PersistentVolumeClaim to configure
*/}}
{{- define "kubernetes-agent.pvcName" -}}
{{- if .Values.persistence.storageClassName }}
{{- printf "%s-pvc" (include "kubernetes-agent.fullName" .) }}
{{- else }}
{{- include "nfs.pvcName" . }}
{{- end }}
{{- end }}

{{/* 
Turns the imagePullSecrets map into a CSV.
*/}}
{{- define "kubernetes-agent.imagePullSecretsCsv" -}}
{{- if .Values.imagePullSecrets }}
{{- $imagePullSecretCsv := (first .Values.imagePullSecrets).name }}
{{- range $i, $val := (rest .Values.imagePullSecrets) }}
    {{- $imagePullSecretCsv = (printf "%s,%s" $imagePullSecretCsv $val.name) }}
{{- end }}
{{- $imagePullSecretCsv }}
{{- end }}
{{- end }}

{{/*
The Env-var block required to set image name, tag and pullpolicy
*/}}
{{- define "kubernetes-agent.scriptPodEnvVars" -}}
{{- if .repository }}
- name: "OCTOPUS__K8STENTACLE__SCRIPTPODIMAGE"
  value: {{ .repository | quote}}
{{- end }}
{{- if .tag }}
- name: "OCTOPUS__K8STENTACLE__SCRIPTPODIMAGETAG"
  value: {{ .tag | quote}}
{{- end }}
{{- if .pullPolicy }}
- name: "OCTOPUS__K8STENTACLE__SCRIPTPODIMAGEPULLPOLICY"
  value: {{ .pullPolicy | quote}}
{{- end }}
{{- end }}

{{/*
The base image for the agent, without any suffixes.
Defaults to the Chart Appversion.
*/}}
{{- define "kubernetes-agent.image" -}}
{{- printf "%s:%s" .Values.agent.image.repository (.Values.agent.image.tag | default .Chart.AppVersion) }}
{{- end }}

{{/*
The complete image for the agent, including any optional suffixes.
*/}}
{{- define "kubernetes-agent.fullImage" -}}
{{- if .Values.agent.image.tagSuffix }}
{{- printf "%s-%s" (include "kubernetes-agent.image" .) .Values.agent.image.tagSuffix }}
{{- else }}
{{- (include "kubernetes-agent.image" .) }}
{{- end }}
{{- end }}