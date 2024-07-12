using System.IO.Compression;
using System.Runtime.InteropServices;
using KubernetesAgent.Integration.Setup.Common.Extraction;

namespace KubernetesAgent.Integration.Setup.Tooling;

public class HelmDownloader : ToolDownloader
{
    const string LatestVersion = "v3.8.0";
    public HelmDownloader( ILogger logger)
        : base("helm", logger)
    {
    }

    protected override string BuildDownloadUrl(Architecture processArchitecture, OperatingSystem operatingSystem)
    {
        var architecture = GetArchitectureLabel(processArchitecture);
        var osName = GetOsName(operatingSystem);

        var suffix = operatingSystem is OperatingSystem.Windows ? "zip" : "tar.gz";

        return $"https://get.helm.sh/helm-{LatestVersion}-{osName}-{architecture}.{suffix}";
    }

    static string GetArchitectureLabel(Architecture processArchitecture) => processArchitecture == Architecture.Arm64 ? "arm64" : "amd64";

    protected override string PostDownload(string targetDirectory, string downloadFilePath, Architecture processArchitecture, OperatingSystem operatingSystem)
    {
        var architecture = GetArchitectureLabel(processArchitecture);
        var osName = GetOsName(operatingSystem);

        var extractionDir = Path.Combine(targetDirectory, "extracted");

        //the helm app is zipped, so we need to extract it
        if (operatingSystem is OperatingSystem.Windows)
        {
            //on windows we need to unzip the file
            ZipFile.ExtractToDirectory(downloadFilePath, extractionDir);
        }
        else
        {
            //everything else is tar.gz
            TarFileExtractor.ExtractAndMakeExecutable(downloadFilePath, extractionDir);
        }

        //move the extracted helm executable to the root target directory
        var targetFilePath = Path.Combine(targetDirectory, ExecutableName);
        File.Move(Path.Combine(extractionDir,$"{osName}-{architecture}", ExecutableName), targetFilePath);

        //delete the extracted directory
        Directory.Delete(extractionDir,true);
        File.Delete(downloadFilePath);

        return targetFilePath;
    }

    static string GetOsName(OperatingSystem operatingSystem)
        => operatingSystem switch
        {
            OperatingSystem.Windows => "windows",
            OperatingSystem.Nix => "linux",
            OperatingSystem.Mac => "darwin",
            _ => throw new ArgumentOutOfRangeException(nameof(operatingSystem), operatingSystem, null)
        };
}