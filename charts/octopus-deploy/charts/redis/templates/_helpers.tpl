{{/* vim: set filetype=mustache: */}}

{{/* Selector Labels for redis */}}
{{- define "redis.selectorLabels" -}}
app.kubernetes.io/name: {{ include "octopus.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/component: redis
{{- end -}}

{{/* Pod Labels for redis */}}
{{- define "redis.podLabels" -}}
{{ include "labels" . }}
app.kubernetes.io/component: redis
{{- end -}}
