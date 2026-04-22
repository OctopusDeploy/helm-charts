{{/* 
These are used for the NFS container & resources
*/}}

{{- define "nfs.name"}}
{{- printf "%s-nfs" (include "kubernetes-agent.name" .) | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- define "nfs.fullName"}}
{{- printf "%s-%s" (include "nfs.name" .) .Release.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- define "nfs.pvName"}}
{{- printf "%s-pv-%s" (include "nfs.fullName" .) .Values.persistence.size | lower  }}
{{- end }}

{{- define "nfs.pvcName"}}
{{- printf "%s-pvc-%s" (include "nfs.fullName" .) .Values.persistence.size | lower }}
{{- end }}

{{- define "nfs.storageClassName"}}
{{- printf "%s-csi" (include "nfs.fullName" .) }} 
{{- end }}

{{- define "nfs.serverAddress"}}
{{- printf "%s.%s.svc.cluster.local" (include "nfs.name" .) .Release.Namespace }}
{{- end }}