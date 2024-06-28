namespace KubernetesAgent.Integration.Setup.Tooling;

public interface IToolDownloader
{
    Task<string> Download(string targetDirectory, CancellationToken cancellationToken);
}