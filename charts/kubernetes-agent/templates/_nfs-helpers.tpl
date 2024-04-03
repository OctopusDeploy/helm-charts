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
{{- printf "%s-pv" (include "nfs.fullName" .) }}
{{- end }}

{{- define "nfs.nfsPodPvcName"}}
{{- printf "%s-pod-pvc" (include "nfs.fullName" .) }}
{{- end}}

{{- define "nfs.pvcName"}}
{{- printf "%s-pvc" (include "nfs.fullName" .) }}
{{- end }}

{{- define "nfs.storageClassName"}}
{{- printf "%s-csi" (include "nfs.fullName" .) }} 
{{- end }}

{{- define "nfs.serverAddress"}}
{{- printf "%s.%s.svc.cluster.local" (include "nfs.name" .) .Release.Namespace }}
{{- end }}
