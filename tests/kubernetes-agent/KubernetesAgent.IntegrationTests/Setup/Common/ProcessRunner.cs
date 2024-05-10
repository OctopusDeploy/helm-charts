using System.Diagnostics;

namespace KubernetesAgent.Integration.Setup.Common;

public static class ProcessRunner
{
    public static Process Run(string command, TemporaryDirectory workingDirectory, params string[] arguments)
    {
        var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments.Length > 0 ? string.Join(" ", arguments) : string.Empty;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.WorkingDirectory = workingDirectory.Directory.FullName;
        process.Start();
        process.WaitForExit();
        return process;
    }
    public static Process Run(string command, params string[] arguments)
    {
        var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments.Length > 0 ? string.Join(" ", arguments) : string.Empty;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();
        process.WaitForExit();
        return process;
    }

    
    public static Process RunWithLogger(string command, TemporaryDirectory directory, ILogger logger, params string[] arguments)
    {
        
        var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments.Length > 0 ? string.Join(" ", arguments) : string.Empty;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.WorkingDirectory = directory.Directory.FullName;

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                logger.Information("{Data}",e.Data);
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                logger.Error("{Data}",e.Data);
            }
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        return process;
    }
}