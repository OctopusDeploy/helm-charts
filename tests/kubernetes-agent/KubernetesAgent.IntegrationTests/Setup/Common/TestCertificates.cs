using System.Security.Cryptography.X509Certificates;

namespace KubernetesAgent.Integration.Setup.Common
{
    public static class TestCertificates
    {
        public static X509Certificate2 Tentacle;
        public static string TentaclePublicThumbprint;
        public static X509Certificate2 Server;
        public static string ServerPublicThumbprint;

        static TestCertificates()
        {
            using var tempDir = new TemporaryDirectory(Directory.CreateTempSubdirectory());
            var tentaclePfxPath = tempDir.WriteFileToTemporaryDirectory("Tentacle.pfx");
            Tentacle = X509CertificateLoader.LoadPkcs12FromFile(tentaclePfxPath, null);
            TentaclePublicThumbprint = Tentacle.Thumbprint;

            var serverPfxPath = tempDir.WriteFileToTemporaryDirectory("Server.pfx");
            Server = X509CertificateLoader.LoadPkcs12FromFile(serverPfxPath, null);
            ServerPublicThumbprint = Server.Thumbprint;
        }
    }
}