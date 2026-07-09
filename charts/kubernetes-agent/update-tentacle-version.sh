#!/usr/bin/env bash
set -euo pipefail

version="${1:-}"
if [[ -z "$version" ]]; then
    echo "Usage: $0 <version>" >&2
    exit 1
fi

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Read the current tentacle version from Chart.yaml
currentVersion="$(grep -E '^appVersion:[[:space:]]*"[0-9]+\.[0-9]+\.[0-9]+"' "$script_dir/Chart.yaml" \
    | head -n 1 \
    | sed -E 's/^appVersion:[[:space:]]*"([0-9]+\.[0-9]+\.[0-9]+)".*/\1/')"

if [[ -z "$currentVersion" ]]; then
    echo "Could not find appVersion in Chart.yaml" >&2
    exit 1
fi

echo "Current version: $currentVersion"
echo "New version: $version"

# Escape regex/sed-special characters in the version strings
escape_re() { printf '%s' "$1" | sed -E 's/[.[\*^$/]/\\&/g'; }
escapedCurrentVersion="$(escape_re "$currentVersion")"
escapedVersion="$(escape_re "$version")"

# Update all yaml files + snapshots with the new version
while IFS= read -r -d '' file; do
    if grep -q "$escapedCurrentVersion" "$file"; then
        tmp="$(mktemp)"
        sed -E "s/$escapedCurrentVersion/$escapedVersion/g" "$file" > "$tmp" && mv "$tmp" "$file"
        echo "Updated: $file"
    fi
done < <(find "$script_dir" \
    -type d -name node_modules -prune -o \
    -type f \( -name '*.yaml' -o -name '*.yaml.snap' -o -name 'README.md' \) -print0)
