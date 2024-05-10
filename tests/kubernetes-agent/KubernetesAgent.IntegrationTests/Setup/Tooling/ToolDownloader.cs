using System.Net;
using System.Runtime.InteropServices;
using KubernetesAgent.Integration.Setup.Common;

namespace KubernetesAgent.Integration.Setup.Tooling;

public abstract class ToolDownloader : IToolDownloader
{
    readonly OperatingSystem os;

    protected ILogger Logger { get; }
    protected string ExecutableName { get; }

    protected ToolDownloader(string executableName, ILogger logger)
    {
        ExecutableName = executableName;
        Logger = logger;

        os = GetOperationSystem();

        //we assume that windows always has .exe suffixed
        if (os is OperatingSystem.Windows)
        {
            ExecutableName += ".exe";
        }
    }

    public async Task<string> Download(string targetDirectory, CancellationToken cancellationToken)
    {
        var downloadUrl = BuildDownloadUrl(RuntimeInformation.ProcessArchitecture, os);

        //we download to a random file name
        var downloadFilePath = Path.Combine(targetDirectory, Guid.NewGuid().ToString("N"));

        Logger.Information("Downloading {DownloadUrl} to {DownloadFilePath}", downloadUrl, downloadFilePath);
        using (var client = new HttpClient())
        {
            
            await using (var s = await client.GetStreamAsync(downloadUrl, cancellationToken))
            {
                await using (var fs = new FileStream(downloadFilePath, FileMode.OpenOrCreate))
                {
                    await s.CopyToAsync(fs, cancellationToken);
                }
            }
        }
        
        downloadFilePath = PostDownload(targetDirectory, downloadFilePath, RuntimeInformation.ProcessArchitecture, os);
        
        return downloadFilePath;
    }

    protected abstract string BuildDownloadUrl(Architecture processArchitecture, OperatingSystem operatingSystem);

    protected virtual string PostDownload(string downloadDirectory, string downloadFilePath, Architecture processArchitecture, OperatingSystem operatingSystem)
    {
        var targetFilename = Path.Combine(downloadDirectory, ExecutableName);
        File.Move(downloadFilePath, targetFilename);
        new FileInfo(targetFilename).MakeExecutable();

        return targetFilename;
    }

    static OperatingSystem GetOperationSystem()
    {
        if (System.OperatingSystem.IsWindows())
        {
            return OperatingSystem.Windows;
        }
        if (System.OperatingSystem.IsLinux())
        {
            return OperatingSystem.Nix;
        }

        if (System.OperatingSystem.IsMacOS())
        {
            return OperatingSystem.Mac;
        }

        throw new InvalidOperationException("Unsupported OS");
    }
}

public enum OperatingSystem
{
    Windows,
    Nix,
    Mac
}