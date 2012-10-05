using System;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
{
    public class EnvironmentMachineNameProvider : IMachineNameProvider
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}