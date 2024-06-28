using System;
using KubernetesAgent.Integration.Setup.Common;
using Octopus.Versioning;

namespace KubernetesAgent.Integration
{
    public static class HelmChartBuilder
    {
        public record HelmPackage(IVersion Version, string Path);
        public static HelmPackage BuildHelmChart(string helmExecutable, TemporaryDirectory directory)
        {
            var versionString = GetChartVersion();
            var version = VersionFactory.CreateSemanticVersion(versionString);
            var timeStampedVersion = $"{versionString}-{DateTime.Now:yyyymdHHmmss}";
            
            var packager = ProcessRunner.Run(helmExecutable, directory, GetHelmChartPackageArguments(timeStampedVersion));
            if (packager.ExitCode != 0)
            {
                throw new Exception($"Failed to package Helm chart. Exit code: {packager.ExitCode}");
            }

            var output = packager.StandardOutput.ReadToEnd();
            return new HelmPackage(version, output.Split(":")[^1].ToString().Trim());
        }

        static string[] GetHelmChartPackageArguments(string version)
        {
            var chartsDirectory = GetChartsDirectory().FullName;
            return
            [
                "package",
                chartsDirectory,
                "--version",
                version
            ];
        }
        
        static DirectoryInfo GetChartsDirectory()
        {
            var chartsDirectory = Path.Combine(AppContext.BaseDirectory);
            var currentDirectory = new DirectoryInfo(chartsDirectory);
            while (currentDirectory != null && currentDirectory.EnumerateDirectories().All(d => d.Name != "charts"))
            {
                currentDirectory = currentDirectory.Parent;
            }
            if (currentDirectory == null)
            {
                throw new DirectoryNotFoundException("Could not find the charts directory");
            }
            return new DirectoryInfo(Path.Combine(currentDirectory.FullName, "charts", "kubernetes-agent"));
        }

        static string GetChartVersion()
        {
            var chartDirectory = GetChartsDirectory();
            var chartYaml = Path.Combine(chartDirectory.FullName, "Chart.yaml");
            var chart = File.ReadAllText(chartYaml);
            return chart.Split("\n").First(l => l.StartsWith("version:")).Split(":")[1].Trim([' ','"']).Trim();
        }
    }
}
