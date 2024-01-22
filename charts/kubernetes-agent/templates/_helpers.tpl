{{/*
Expand the name of the chart.
*/}}
{{- define "kubernetes-agent.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "kubernetes-agent.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
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
{{- default (include "kubernetes-agent.fullname" .) .Values.jobServiceAccount.name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "kubernetes-agent.jobServiceAccountName" -}}
{{- default (printf "%s-job" (include "kubernetes-agent.fullname" .)) .Values.serviceAccount.name }}
{{- end }}

{{/*
Create the name of the job cluster role to use
*/}}
{{- define "kubernetes-agent.jobClusterRoleName" -}}
{{- printf "%s-role" (include "kubernetes-agent.jobServiceAccountName" .) }}
{{- end }}

{{/*
Create the name of the job cluster role binding to use
*/}}
{{- define "kubernetes-agent.jobClusterRoleBindingName" -}}
{{- printf "%s-binding" (include "kubernetes-agent.jobServiceAccountName" .) }}
{{- end }}

{{- define "kubernetes-agent.secrets.serverAuth" -}}
{{- printf "%s-server-auth" ( include "kubernetes-agent.fullname" . ) }}
{{- end }}

{{- define "kubernetes-agent.jobVolumeYaml" -}}
volumes:
- name: tentacle-home
  nfs:
    path: /
    readOnly: false
    server: {{ .Values.storage.nfsPort }}
{{- end }}