using System;
using Octopus.Diagnostics;
using ILog = Octopus.Diagnostics.ILog;

namespace KubernetesAgent.Integration.Setup.Common
{
    public sealed class TestLogger(ILogger logger) : ILog, IDisposable
    {
        readonly ILogger logger = logger;
        
        public string CorrelationId { get; }
        
        public void Dispose()
        {
            Flush();
        }

        public void WithSensitiveValues(string[] sensitiveValues)
        {
        }

        public void WithSensitiveValue(string sensitiveValue)
        {
        }
        
        public void Write(LogCategory category, string messageText)
        {
            Write(category, null, messageText);
        }

        public void Write(LogCategory category, Exception error)
        {
            WriteFormat(category, error.Message);
        }

        public void Write(LogCategory category, Exception? error, string messageText)
        {
            WriteFormat(category, "Exception: {Message}, Message: {Message}", error?.Message ?? "", messageText);
        }

        public void WriteFormat(LogCategory category, string messageFormat, params object[] args)
        {
            WriteFormat(category, null, messageFormat, args);
        }

        public void WriteFormat(LogCategory category, Exception? error, string messageFormat, params object[] args)
        {
            switch (category)
            {
                case LogCategory.Trace:
                case LogCategory.Verbose:
                    logger.Debug(messageFormat, args);
                    break;
                case LogCategory.Info:
                case LogCategory.Planned:
                case LogCategory.Highlight:
                case LogCategory.Abandoned:
                case LogCategory.Wait:
                case LogCategory.Progress:
                    logger.Information(messageFormat, args);
                    break;
                case LogCategory.Finished:
                case LogCategory.Warning:
                case LogCategory.Error:
                case LogCategory.Fatal:
                    logger.Error(messageFormat, args);
                    break;
            }
        }
        
        public void Trace(string messageText)
        {
            Write(LogCategory.Trace, messageText);
        }

        public void Trace(Exception error)
        {
            Write(LogCategory.Trace, error);
        }

        public void Trace(Exception error, string message)
        {
            Write(LogCategory.Trace, error, message);
        }

        public void TraceFormat(string messageFormat, params object[] args)
        {
            WriteFormat(LogCategory.Trace, messageFormat, args);
        }

        public void TraceFormat(Exception error, string format, params object[] args)
        {
            WriteFormat(LogCategory.Trace, error, format, args);
        }

        public void Verbose(string messageText)
        {
            Write(LogCategory.Verbose, messageText);
        }

        public void Verbose(Exception error)
        {
            Write(LogCategory.Verbose, error);
        }

        public void Verbose(Exception error, string message)
        {
            Write(LogCategory.Verbose, error, message);
        }

        public void VerboseFormat(string format, params object[] args)
        {
            WriteFormat(LogCategory.Verbose, format, args);
        }

        public void VerboseFormat(Exception error, string format, params object[] args)
        {
            WriteFormat(LogCategory.Verbose, error, format, args);
        }

        public void Info(string messageText)
        {
            Write(LogCategory.Info, messageText);
        }

        public void Info(Exception error)
        {
            Write(LogCategory.Info, error);
        }

        public void Info(Exception error, string message)
        {
            Write(LogCategory.Info, error, message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            WriteFormat(LogCategory.Info, format, args);
        }

        public void InfoFormat(Exception error, string format, params object[] args)
        {
            WriteFormat(LogCategory.Info, error, format, args);
        }

        public void Warn(string messageText)
        {
            Write(LogCategory.Warning, messageText);
        }

        public void Warn(Exception error)
        {
            Write(LogCategory.Warning, error);
        }

        public void Warn(Exception error, string messageText)
        {
            Write(LogCategory.Warning, error, messageText);
        }

        public void WarnFormat(string format, params object[] args)
        {
            WriteFormat(LogCategory.Warning, format, args);
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
            WriteFormat(LogCategory.Warning, exception, format, args);
        }

        public void Error(string messageText)
        {
            Write(LogCategory.Error, messageText);
        }

        public void Error(Exception error)
        {
            Write(LogCategory.Error, error);
        }

        public void Error(Exception error, string messageText)
        {
            Write(LogCategory.Error, error, messageText);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            WriteFormat(LogCategory.Error, format, args);
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            WriteFormat(LogCategory.Error, exception, format, args);
        }

        public void Fatal(string messageText)
        {
            Write(LogCategory.Fatal, messageText);
        }

        public void Fatal(Exception error)
        {
            Write(LogCategory.Fatal, error);
        }

        public void Fatal(Exception error, string messageText)
        {
            Write(LogCategory.Fatal, error, messageText);
        }

        public void FatalFormat(string messageFormat, params object[] args)
        {
            WriteFormat(LogCategory.Fatal, messageFormat, args);
        }

        public void FatalFormat(Exception exception, string format, params object[] args)
        {
            WriteFormat(LogCategory.Fatal, exception, format, args);
        }

        public void Flush()
        { }

    }
}