using System;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance
{
    public class EnvironmentMachineNameProvider : IMachineNameProvider
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}