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
Create the name of the service account to use
*/}}
{{- define "kubernetes-agent.serviceAccountName" -}}
{{- .Values.agent.serviceAccount.name | default (printf "%s-tentacle" (include "kubernetes-agent.name" .)) }}
{{- end }}

{{/*
Create the name of the service account to use
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
Create the name of the pod (namespaced) role for creating pods, and updating tentacle
*/}}
{{- define "kubernetes-agent.scriptPodWorkerRoleName" -}}
{{- printf "%s-worker-role" (include "kubernetes-agent.scriptPodServiceAccountFullName" .) }}
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
