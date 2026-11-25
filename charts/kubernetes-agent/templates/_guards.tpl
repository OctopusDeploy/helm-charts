{{/*
Fails the render if the removed NFS server option is still enabled (e.g. carried forward via --reuse-values from v3).
*/}}
{{- define "kubernetes-agent.nfs-validation" -}}
{{- if dig "nfs" "enabled" false .Values.persistence }}
{{- fail "persistence.nfs.enabled is no longer supported as of v4. NFS-backed storage was removed; switch to the default ReadWriteOnce storage or an existing ReadWriteMany storageClassName before upgrading. See migrations.md for details." }}
{{- end }}