using System;

namespace SignalKo.SystemMonitor.Agent.Core.Exceptions
{
    public class MessageQueuePersistenceException : Exception
    {
        public MessageQueuePersistenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}