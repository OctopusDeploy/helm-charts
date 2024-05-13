using System;
using System.Reflection;

namespace KubernetesAgent.Integration.Setup.Common;

public static class AssemblyExtensions
{
    public static Stream GetManifestResourceStreamFromPartialName(this Assembly assembly, string filename)
    {        
        var valuesFileName = assembly.GetManifestResourceNames().Single(n => n.Contains(filename, StringComparison.OrdinalIgnoreCase));
        return assembly.GetManifestResourceStream(valuesFileName)!;
    }
}