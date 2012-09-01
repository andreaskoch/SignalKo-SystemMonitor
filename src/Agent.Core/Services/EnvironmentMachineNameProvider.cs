using System;

namespace SignalKo.SystemMonitor.Agent.Core.Services
{
    public class EnvironmentMachineNameProvider : IMachineNameProvider
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}