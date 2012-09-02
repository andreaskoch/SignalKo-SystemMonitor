using System;

namespace SignalKo.SystemMonitor.Agent.Core.Exceptions
{
    public class SystemInformationSenderSetupException : Exception
    {
        public SystemInformationSenderSetupException(string message, params object[] arguments) : base(string.Format(message, arguments))
        {
        }
    }
}