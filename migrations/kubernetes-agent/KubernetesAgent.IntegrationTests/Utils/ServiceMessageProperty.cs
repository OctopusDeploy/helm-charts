using System;

namespace KubernetesAgent.Integration.Utils
{
    public class ServiceMessageProperty(string name, string value)
    {
        // EF Primary key
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Guid Id { get; private set; }

        public string Name { get; private set; } = name;
        public string Value { get; private set; } = value;
    }
}
