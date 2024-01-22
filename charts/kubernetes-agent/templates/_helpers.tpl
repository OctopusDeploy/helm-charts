{{/*
The name for the agent
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
*/}}
{{- define "kubernetes-agent.name" -}}
{{ .Values.nameOverride | default "octopus-agent" | trunc 63 | trimSuffix "-" }}
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
{{- .Values.serviceAccount.name | default (printf "%s-tentacle" (include "kubernetes-agent.name" .)) }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "kubernetes-agent.jobServiceAccountName" -}}
{{- .Values.jobServiceAccount.name | default (printf "%s-job" (include "kubernetes-agent.name" .)) }}
{{- end }}


{{/*
Used for the job cluster role & clusterrole binding as they are not namespaced.
*/}}
{{- define "kubernetes-agent.jobServiceAccountFullName" -}}
{{- printf "%s-%s" ( include "kubernetes-agent.jobServiceAccountName" .) .Release.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create the name of the job cluster role to use
*/}}
{{- define "kubernetes-agent.jobClusterRoleName" -}}
{{- printf "%s-role" (include "kubernetes-agent.jobServiceAccountFullName" .) }}
{{- end }}

{{/*
Create the name of the job cluster role binding to use
*/}}
{{- define "kubernetes-agent.jobClusterRoleBindingName" -}}
{{- printf "%s-binding" (include "kubernetes-agent.jobServiceAccountFullName" .) }}
{{- end }}

{{/*
The name of the secret to store the authentication information (bearer token/api key)
*/}}
{{- define "kubernetes-agent.secrets.serverAuth" -}}
{{- printf "%s-tentacle-server-auth" ( include "kubernetes-agent.name" . ) }}
{{- end }}

{{- define "kubernetes-agent.jobVolumeYaml" -}}
volumes:
- name: tentacle-home
  nfs:
    path: /
    readOnly: false
    server: {{ .Values.storage.nfsPort }}
{{- end }}