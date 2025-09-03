/*
Copyright 2025.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

package v1beta1

import (
	corev1 "k8s.io/api/core/v1"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
)

// EDIT THIS FILE!  THIS IS SCAFFOLDING FOR YOU TO OWN!
// NOTE: json tags are required.  Any new fields you add must have json tags for the fields to be serialized.

type PodMetadata struct {
	// +optional
	PodLabels map[string]string `json:"labels,omitempty"`

	// +optional
	PodAnnotations map[string]string `json:"annotations,omitempty"`
}

// ScriptPodTemplateSpec defines the desired state of ScriptPodTemplate
type ScriptPodTemplateSpec struct {
	// INSERT ADDITIONAL SPEC FIELDS - desired state of cluster
	// Important: Run "make" to regenerate code after modifying this file
	// The following markers will use OpenAPI v3 schema to validate the value
	// More info: https://book.kubebuilder.io/reference/markers/crd-validation.html

	// +optional
	PodSpec corev1.PodSpec `json:"podSpec,omitempty"`

	// +optional
	PodMetadata *PodMetadata `json:"podMetadata,omitempty"`

	// +optional
	ScriptContainerSpec *corev1.Container `json:"scriptContainerSpec,omitempty"`

	// +optional
	WatchdogContainerSpec *corev1.Container `json:"watchdogContainerSpec,omitempty"`
}

// ScriptPodTemplateStatus defines the observed state of ScriptPodTemplate.
type ScriptPodTemplateStatus struct {
	// INSERT ADDITIONAL STATUS FIELD - define observed state of cluster
	// Important: Run "make" to regenerate code after modifying this file
}

// +kubebuilder:object:root=true
// +kubebuilder:subresource:status

// ScriptPodTemplate is the Schema for the scriptpodtemplates API
type ScriptPodTemplate struct {
	metav1.TypeMeta `json:",inline"`

	// metadata is a standard object metadata
	// +optional
	metav1.ObjectMeta `json:"metadata,omitempty,omitzero"`

	// spec defines the desired state of ScriptPodTemplate
	// +required
	Spec ScriptPodTemplateSpec `json:"spec"`

	// status defines the observed state of ScriptPodTemplate
	// +optional
	Status ScriptPodTemplateStatus `json:"status,omitempty,omitzero"`
}

// +kubebuilder:object:root=true

// ScriptPodTemplateList contains a list of ScriptPodTemplate
type ScriptPodTemplateList struct {
	metav1.TypeMeta `json:",inline"`
	metav1.ListMeta `json:"metadata,omitempty"`
	Items           []ScriptPodTemplate `json:"items"`
}

func init() {
	SchemeBuilder.Register(&ScriptPodTemplate{}, &ScriptPodTemplateList{})
}
