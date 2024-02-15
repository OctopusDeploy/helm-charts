{{/* 
These are used for the NFS container & resources
*/}}

{{- define "nfs.name"}}
{{- printf "%s-nfs" (include "kubernetes-agent.name" .) }}
{{- end }}

{{- define "nfs.fullName"}}
{{- printf "%s-%s" (include "nfs.name" .) .Release.Name }}
{{- end }}

{{- define "nfs.pvcName"}}
{{- printf "%s-pvc" (include "nfs.fullName" .) }}
{{- end }}

{{- define "nfs.storageClassName"}}
{{- printf "%s-csi" (include "nfs.fullName" .) }} 
{{- end }}
