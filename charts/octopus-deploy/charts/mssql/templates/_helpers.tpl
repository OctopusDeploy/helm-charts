{{/* vim: set filetype=mustache: */}}

{{/*
Expand the name of the chart.
*/}}
{{- define "mssql.name" -}}
{{- default "mssql" .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "mssql.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- $name := default "octopus-deploy" .Values.nameOverride -}}
{{- if contains $name .Release.Name -}}
{{- printf "%s-mssql" .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- printf "%s-%s-mssql" .Release.Name $name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end -}}
{{- end -}}


{{/* Selector Labels for mssql */}}
{{- define "mssql.selectorLabels" -}}
{{include "labels" . }}
component: mssql
{{- end -}}

{{- define "mssql.storageClass" -}}
{{- if .Values.storageClass -}}
{{ .Values.storageClass }}
{{- else if .Values.global.storageClass -}}
{{ .Values.global.storageClass }}
{{- end -}}
{{- end -}}