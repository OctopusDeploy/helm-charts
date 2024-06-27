using System.Formats.Tar;
using System.IO.Compression;

namespace KubernetesAgent.Integration.Setup.Common.Extraction;

public static class TarFileExtractor
{
    public static void Extract(FileSystemInfo tarFile, FileSystemInfo targetDirectory)
    {
        if (!targetDirectory.Exists)
        {
            Directory.CreateDirectory(targetDirectory.FullName);
        }
        using var compressedFileStream = File.Open(tarFile.FullName, FileMode.Open);
        using var tarStream = new GZipStream(compressedFileStream, CompressionMode.Decompress); 
        TarFile.ExtractToDirectory(tarStream, targetDirectory.FullName, true);
    }

    public static void ExtractAndMakeExecutable(FileSystemInfo tarFile, FileSystemInfo targetDirectory)
    {
        Extract(tarFile, targetDirectory);
        targetDirectory.MakeExecutable();
    }
    public static void ExtractAndMakeExecutable(string tarFilePath, string targetDirectoryPath)
    {
        var tarFile = new FileInfo(tarFilePath);
        var targetDirectory = new FileInfo(targetDirectoryPath);
        ExtractAndMakeExecutable(tarFile, targetDirectory);
    }
}