using KubernetesAgent.UpgradeManager.Migrations;
using Octopus.Versioning;

namespace KubernetesAgent.UpgradeManager
{
    interface IMigrationManager
    {
        string MigrateValues(IVersion from, IVersion to, string valuesJson);
    }

    class MigrationManager : IMigrationManager
    {
        static readonly Dictionary<int,IMigration?> MigratorMap;

        static MigrationManager()
        {
            MigratorMap = typeof(MigrationManager).Assembly.GetTypes()
                .Where(t => typeof(IMigration).IsAssignableFrom(t))
                .Select(t => (IMigration?)Activator.CreateInstance(t))
                .Where(m => m is not null)
                .ToDictionary(m => m!.Version, m => m);
        }

        public string MigrateValues(IVersion from, IVersion to, string valuesJson)
        {
            foreach (var majorVersion in Enumerable.Range(from.Major + 1, to.Major))
            {
                if (!MigratorMap.TryGetValue(majorVersion, out var migrator))
                {
                    throw new InvalidOperationException($"Unable to migrate values from v{majorVersion - 1} to {majorVersion}, migrator missing.");
                }

                valuesJson = migrator.MigrateValues(valuesJson);
            }

            return valuesJson;
        }
    }
}
