namespace KubernetesAgent.UpgradeManager.Migrations
{
    interface IMigration
    {
        public int Version { get; }
        public string MigrateValues(string previousValues);
    }
}
