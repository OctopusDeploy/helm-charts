using System;
using System.Text;

namespace KubernetesAgent.Integration.Utils
{
    public class ServiceMessage
    {
        // EF Primary key
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Guid Id { get; private set; }
        // EF Navigation property
        public IReadOnlyList<ServiceMessageProperty> PropertiesList { get; private set; }

        public ServiceMessage(string name) : this(name, new Dictionary<string, string>())
        {
        }

        public ServiceMessage(string name, IDictionary<string, string> properties)
        {
            Name = name;
            PropertiesList = properties
                .Select(p => new ServiceMessageProperty(p.Key, p.Value))
                .ToList();
        }

        public string Name { get; private set; }

        public IDictionary<string, string> Properties => PropertiesList.ToDictionary(
            p => p.Name, 
            p => p.Value,
            StringComparer.OrdinalIgnoreCase);

        public string? GetValue(string key) => PropertiesList.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("##octopus[").Append(Name);

            foreach (var property in PropertiesList)
            {
                sb.Append(" ").Append(property.Name).Append("=\"").Append(EncodeValue(property.Value)).Append("\"");
            }

            sb.Append("]");

            return sb.ToString();
        }

        static string EncodeValue(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }
}
