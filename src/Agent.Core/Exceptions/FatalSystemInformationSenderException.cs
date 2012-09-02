using System;

namespace SignalKo.SystemMonitor.Agent.Core.Exceptions
{
    public class FatalSystemInformationSenderException : Exception
    {
        public FatalSystemInformationSenderException(string message) : base(message)
        {
        }

        public FatalSystemInformationSenderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}