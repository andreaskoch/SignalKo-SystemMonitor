using System;

namespace SignalKo.SystemMonitor.Agent.Core.Exceptions
{
    public class UnableToCreateRESTClientException : Exception
    {
        public UnableToCreateRESTClientException(string message)
            : base(message)
        {
        }

        public UnableToCreateRESTClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}