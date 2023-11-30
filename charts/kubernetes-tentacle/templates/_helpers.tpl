{{/*
Expand the name of the chart.
*/}}
{{- define "kubernetes-tentacle.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "kubernetes-tentacle.fullname" -}}
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
{{- define "kubernetes-tentacle.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{/*
Common labels
*/}}
{{- define "kubernetes-tentacle.labels" -}}
helm.sh/chart: {{ include "kubernetes-tentacle.chart" . }}
{{ include "kubernetes-tentacle.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "kubernetes-tentacle.selectorLabels" -}}
app.kubernetes.io/name: {{ include "kubernetes-tentacle.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "kubernetes-tentacle.serviceAccountName" -}}
{{- default (include "kubernetes-tentacle.fullname" .) .Values.jobServiceAccount.name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "kubernetes-tentacle.jobServiceAccountName" -}}
{{- default (printf "%s-job" (include "kubernetes-tentacle.fullname" .)) .Values.serviceAccount.name }}
{{- end }}

{{- define "kubernetes-tentacle.secrets.installId" -}}
{{- printf "$s-install-id" ( include "kubernetes-tentacle.fullname" . ) }}
{{- end }}

{{- define "kubernetes-tentacle.jobVolumeYaml" -}}
volumes:
- name: tentacle-home
  nfs:
    path: /
    readOnly: false
    server: 10.96.42.42
{{- end }}