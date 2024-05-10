﻿using System.Diagnostics;
using System.Reflection;
using KubernetesAgent.Integration.Setup.Common;
using Newtonsoft.Json;
using Octopus.Client.Extensions;
using Octopus.Tentacle.Kubernetes.Tests.Integration.Support;

namespace KubernetesAgent.Integration.Setup;

public class KubernetesClusterInstaller : IDisposable
{
    readonly string clusterName;
    readonly string kubeConfigName;

    readonly TemporaryDirectory tempDir;
    readonly string kindExePath;
    readonly string helmExePath;
    readonly string kubeCtlPath;
    readonly ILogger logger;

    public string KubeConfigPath => Path.Combine(tempDir.Directory.FullName, kubeConfigName);
    public string ClusterName => clusterName;

    public KubernetesClusterInstaller(TemporaryDirectory tempDirectory, string kindExePath, string helmExePath, string kubeCtlPath, ILogger logger)
    {
        tempDir = tempDirectory;
        this.kindExePath = kindExePath;
        this.helmExePath = helmExePath;
        this.kubeCtlPath = kubeCtlPath;
        this.logger = logger;

        clusterName = $"tentacleint-{DateTime.Now:yyyyMMddhhmmss}";
        kubeConfigName = $"{clusterName}.config";
    }

    public async Task Install()
    {
        var configFilePath = await WriteFileToTemporaryDirectory("kind-config.yaml");

        var sw = new Stopwatch();
        sw.Restart();

        var result = ProcessRunner.RunWithLogger(kindExePath, tempDir, logger,
            $"create cluster --name={clusterName} --config=\"{configFilePath}\" --kubeconfig=\"{kubeConfigName}\"");

        sw.Stop();

        if (result.ExitCode != 0)
        {
            logger.Error("Failed to create Kind Kubernetes cluster {ClusterName}", clusterName);
            throw new InvalidOperationException($"Failed to create Kind Kubernetes cluster {clusterName}");
        }

        logger.Information("Test cluster kubeconfig path: {Path:l}", KubeConfigPath);

        logger.Information("Created Kind Kubernetes cluster {ClusterName} in {ElapsedTime}", clusterName, sw.Elapsed);

        await SetLocalhostRouting();
        
        await InstallNfsCsiDriver();
    }

    async Task SetLocalhostRouting()
    {
        var filename = OperatingSystem.IsLinux() ? "linux-network-routing.yaml" : "docker-desktop-network-routing.yaml";

        var manifestFilePath = await WriteFileToTemporaryDirectory(filename, "manifest.yaml");

        var result = ProcessRunner.RunWithLogger(kubeCtlPath, tempDir, logger,
            "apply", 
            "-n default", 
            $"-f \"{manifestFilePath}\"", 
            $"--kubeconfig=\"{KubeConfigPath}\"");

        if (result.ExitCode != 0)
        {
            logger.Error("Failed to apply localhost routing to cluster {ClusterName}", clusterName);
            throw new InvalidOperationException($"Failed to apply localhost routing to cluster {clusterName}.");
        }
    }

    async Task<string> WriteFileToTemporaryDirectory(string resourceFileName, string? outputFilename = null)
    {
        await using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStreamFromPartialName(resourceFileName);

        var filePath = Path.Combine(tempDir.Directory.FullName, outputFilename ?? resourceFileName);
        await using var file = File.Create(filePath);

        resourceStream.Seek(0, SeekOrigin.Begin);
        await resourceStream.CopyToAsync(file);

        return filePath;
    }

    async Task InstallNfsCsiDriver()
    {
        //we need to perform a repo update in helm first
        // var exitCode = SilentProcessRunner.ExecuteCommand(
        //     helmPath,
        //     "repo update",
        //     tempDir.DirectoryPath,
        //     logger.Debug,
        //     logger.Information,
        //     logger.Error,
        //     CancellationToken.None);

        var installArgs = BuildNfsCsiDriverInstallArguments();
        var result = ProcessRunner.RunWithLogger(helmExePath, tempDir, logger, installArgs);

        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException($"Failed to install NFS CSI driver into cluster {clusterName}.");
        }
    }
    
    string BuildNfsCsiDriverInstallArguments()
    {
        return string.Join(" ",
            "install",
            "--atomic",
            "--repo https://raw.githubusercontent.com/kubernetes-csi/csi-driver-nfs/master/charts",
            "--namespace kube-system",
            "--version v4.6.0",
            $"--kubeconfig \"{KubeConfigPath}\"",
            "csi-driver-nfs",
            "csi-driver-nfs"
        );
    }
    
    public void Dispose()
    {
        var result = ProcessRunner.Run(kindExePath,
            $"delete cluster --name={clusterName}");
        if (result.ExitCode != 0)
        {
            logger.Error("Failed to delete Kind kubernetes cluster {ClusterName}", clusterName);
        }
    }
}