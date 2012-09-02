using System;

namespace SignalKo.SystemMonitor.Agent.Core.Collector
{
    public class EnvironmentMachineNameProvider : IMachineNameProvider
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}