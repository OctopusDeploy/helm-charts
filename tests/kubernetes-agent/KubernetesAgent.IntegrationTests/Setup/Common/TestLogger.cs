using System;
using Octopus.Tentacle.Contracts.Logging;

namespace KubernetesAgent.Integration.Setup.Common
{
    public sealed class TestLogger(ILogger logger) : ITentacleClientTaskLog
    {
        public void Info(string message)
        {
            logger.Information(message);
        }

        public void Verbose(string message)
        {
            logger.Verbose(message);
        }

        public void Verbose(Exception exception)
        {
            logger.Verbose(exception, "");
        }

        public void Warn(string message)
        {
            logger.Warning(message);
        }

        public void Warn(Exception exception, string message)
        {
            logger.Warning(exception, message);
        }

        public void Dispose()
        {
        }
    }
}