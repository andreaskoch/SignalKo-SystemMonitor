using System;

namespace SignalKo.SystemMonitor.Agent.Core.Exceptions
{
    public class FatalSystemInformationSenderException : Exception
    {
        public FatalSystemInformationSenderException(string message, params object[] arguments) : base(string.Format(message, arguments))
        {
        }
    }
}