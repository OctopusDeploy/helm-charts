namespace KubernetesAgent.Integration.Setup.Common;

public class TemporaryDirectory : IDisposable
{
    public DirectoryInfo Directory { get; }
    
    public TemporaryDirectory(DirectoryInfo directory)
    {
        Directory = directory;
        if (!Directory.Exists)
        {
            Directory.Create();
        }
    }

    public void Dispose()
    {
        Directory.Delete(true);
    }
}