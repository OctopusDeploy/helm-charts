using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KubernetesAgent.Integration.Utils
{
    public class ServiceMessageParserState
    {
        [NotMapped]
        public StringBuilder Buffer { get; init; } = new();

        // Only use by EF
        // ReSharper disable once UnusedMember.Global
        public string BufferString
        {
            get => Buffer.ToString();
            set => Buffer.Clear().Append(value);
        }
        public ServiceMessageParser.State State { get; set; } = ServiceMessageParser.State.Default;
    }
}
