using Halibut;
using Halibut.Diagnostics;
using KubernetesAgent.Integration.Setup.Common;

namespace KubernetesAgent.Integration.Setup
{
    public class ServerHalibutRuntime
    {
        public HalibutRuntime ServerHalibutRuntimeInstance { get; set; } = null!;
        public int BuildServerHalibutRuntimeAndListen()
        {
            var serverHalibutRuntimeBuilder = new HalibutRuntimeBuilder()
                .WithServerCertificate(TestCertificates.Server)
                .WithHalibutTimeoutsAndLimits(HalibutTimeoutsAndLimits.RecommendedValues());

            ServerHalibutRuntimeInstance = serverHalibutRuntimeBuilder.Build();

            return ServerHalibutRuntimeInstance.Listen();
        }
        
        public void TrustThumbprint(string thumbprint)
        {
            ServerHalibutRuntimeInstance.Trust(thumbprint);
        }

    }
}
