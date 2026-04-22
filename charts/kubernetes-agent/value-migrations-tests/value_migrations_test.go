package value_migrations_tests

import (
	"github.com/octopusdeploylabs/helm-migrate-values/pkg"
	"github.com/stretchr/testify/require"
	"gopkg.in/yaml.v2"
	"os"
	"testing"
)

var migrationTestCases = []struct {
	name         string
	vFrom        int
	vTo          int
	configPath   string
	expectedPath string
}{
	{
		"migrate from v1 to v2 with all values",
		1,
		2,
		"v1-allValues-initial.yaml",
		"to-v2-allValues-expected.yaml",
	},
	{
		"migrate from v1 to v2 with only required values",
		1,
		2,
		"v1-requiredValuesOnly-initial.yaml",
		"to-v2-requiredValuesOnly-expected.yaml",
	},
}

func Test_ValuesMigrations_IntegrationTests(t *testing.T) {
	for _, tc := range migrationTestCases {
		t.Run(tc.name, func(t *testing.T) {

			req := require.New(t)

			config, err := loadConfigFromFile(tc.configPath)
			req.NoError(err, "error loading config")
			expected, err := loadConfigFromFile(tc.expectedPath)
			req.NoError(err, "error loading expected values")

			migrationsPath := "../value-migrations"
			result, err := pkg.MigrateFromPath(config, tc.vFrom, &tc.vTo, migrationsPath, pkg.Logger{})
			req.NoError(err, "error migrating values")

			req.EqualValues(expected, result, "expected values do not match result")
		})
	}
}

func loadConfigFromFile(path string) (map[string]interface{}, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}

	config := make(map[string]interface{})
	err = yaml.Unmarshal(data, config)
	if err != nil {
		return nil, err
	}
	return config, nil
}
