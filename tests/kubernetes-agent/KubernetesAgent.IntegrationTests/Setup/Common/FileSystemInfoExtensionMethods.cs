namespace KubernetesAgent.Integration.Setup.Common;

public static class FileSystemInfoExtensionMethods
{
    public static void MakeExecutable(this FileSystemInfo fsObject)
    {
        var result = ProcessRunner.Run("chmod", "+x", "-R", fsObject.FullName);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Failed to make {fsObject.FullName} executable. Exit code: {result.ExitCode}. stdout: {result.StandardOutput.ReadToEnd()}, stderr: {result.StandardError.ReadToEnd()}.");
        }
        result.Close();
    }
}