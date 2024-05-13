using System.Reflection;

namespace KubernetesAgent.Integration.Setup.Common
{
    public static class TemporaryDirectoryExtensionMethods
    {
        public static string WriteFileToTemporaryDirectory(this TemporaryDirectory tempDir, string resourceFileName, string? outputFilename = null)
        {
            using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStreamFromPartialName(resourceFileName);

            var filePath = Path.Combine(tempDir.Directory.FullName, outputFilename ?? resourceFileName);
            using var file = File.Create(filePath);

            resourceStream.Seek(0, SeekOrigin.Begin);
            resourceStream.CopyTo(file);

            return filePath;
        }
    }
}
