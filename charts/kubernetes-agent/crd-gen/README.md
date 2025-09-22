# crd-gen
This is a kubebuilder scaffold for generating Custom Resource Definitions (CRDs) used in the agent.

# To generate
Run the following command to generate the CRDs:
```bash
make manifests
```
The generated CRDs will be located in the `config/crd/bases` directory.

You can then sanitise and copy the generated CRDs to the appropriate location in the chart:
```bash
./crd-field-sanitise.sh ./config/crd/bases/agent.octopus.com_scriptpodtemplates.yaml
cp ./config/crd/bases/agent.octopus.com_scriptpodtemplates.yaml ../templates/crd-script-pod-template.yaml
```