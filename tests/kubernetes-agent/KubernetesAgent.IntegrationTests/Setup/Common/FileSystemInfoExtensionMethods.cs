using System.Runtime.InteropServices;

namespace KubernetesAgent.Integration.Setup.Common;

public static class FileSystemInfoExtensionMethods
{
    public static void MakeExecutable(this FileSystemInfo fsObject)
    {
        //if we are on windows, we don't need to do this
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }
        
        var result = ProcessRunner.Run("chmod", "-R", "+x", fsObject.FullName);
        if (result.ExitCode != 0)
        {
            throw new Exception($"Failed to make {fsObject.FullName} executable. Exit code: {result.ExitCode}. stdout: {result.StandardOutput.ReadToEnd()}, stderr: {result.StandardError.ReadToEnd()}.");
        }
        result.Close();
    }
}