using System;
using System.Text;
using System.Xml.Linq;

namespace KubernetesAgent.Integration.Utils
{
    /// <summary>
    /// Builds up a buffer of characters via the <code>Append()</code> function, and
    /// extracts service message content if the buffer matches certain criteria.
    /// If the buffer does not match the ServiceMessage criteria, it's content is sent
    /// to the "output" handler.
    /// 
    /// NOTE: This is not Thread-safe, if multiple threads call "Append" simultaneously - the
    /// buffer will become interleaved between the threads.
    /// </summary>
    public class ServiceMessageParser
    {
        readonly Action<string> output;
        readonly Action<ServiceMessage> serviceMessage;
        readonly ServiceMessageParserState state;

        public ServiceMessageParser(Action<string> output, Action<ServiceMessage> serviceMessage, ServiceMessageParserState state)
        {
            this.output = output;
            this.serviceMessage = serviceMessage;
            this.state = state;
        }

        StringBuilder Buffer => state.Buffer;
        State CurrentState
        {
            get => state.State;
            set => state.State = value;
        }

        public ServiceMessageParser(Action<string> output, Action<ServiceMessage> serviceMessage)
            : this(output, serviceMessage, new ServiceMessageParserState())
        {
        }

        public void Append(string line)
        {
            foreach (var c in line)
            {
                switch (CurrentState)
                {
                    case State.Default:
                        if (c == '\r')
                        {
                        }
                        else if (c == '\n')
                        {
                            Flush(output);
                        }
                        else if (c == '#')
                        {
                            CurrentState = State.PossibleMessage;
                            Buffer.Append(c);
                        }
                        else
                        {
                            Buffer.Append(c);
                        }

                        break;

                    case State.PossibleMessage:
                        if (c == '\r')
                        {
                        }
                        else if (c == '\n')
                        {
                            Flush(output);
                        }
                        else
                        {
                            Buffer.Append(c);
                            var progress = Buffer.ToString();
                            if ("##octopus" == progress)
                            {
                                CurrentState = State.InMessage;
                                Buffer.Clear();
                            }
                            else if (!"##octopus".StartsWith(progress))
                            {
                                CurrentState = State.Default;
                            }
                        }

                        break;

                    case State.InMessage:
                        if (c == ']')
                        {
                            Flush(ProcessMessage);
                            CurrentState = State.Default;
                        }
                        else
                        {
                            Buffer.Append(c);
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (CurrentState != State.InMessage)
            {
                Flush();
            }
        }

        public void Flush()
        {
            if (Buffer.Length > 0)
            {
                Flush(output);
            }
        }

        void ProcessMessage(string message)
        {
            try
            {
                message = message.Trim().TrimStart('[').Replace("\r", "").Replace("\n", "");

                var element = XElement.Parse("<" + message + "/>");
                var name = element.Name.LocalName;
                var values = element.Attributes().ToDictionary(s => s.Name.LocalName, s => Encoding.UTF8.GetString(Convert.FromBase64String(s.Value)), StringComparer.OrdinalIgnoreCase);
                serviceMessage(new ServiceMessage(name, values));
            }
            catch
            {
                serviceMessage(new ServiceMessage("stdout-warning"));
                output($"Could not parse '##octopus[{message}]'");
                serviceMessage(new ServiceMessage("stdout-default"));
            }
        }

        void Flush(Action<string> to)
        {
            var result = Buffer.ToString();
            Buffer.Clear();

            if (result.Length > 0)
            {
                to(result);
            }
        }

        public enum State
        {
            Default,
            PossibleMessage,
            InMessage
        }
    }
}
