using System;

namespace SignalKo.SystemMonitor.Agent.Core.Exceptions
{
    public class SendSystemInformationFailedException : Exception
    {
        public SendSystemInformationFailedException(string message) : base(message)
        {
        }

        public SendSystemInformationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}