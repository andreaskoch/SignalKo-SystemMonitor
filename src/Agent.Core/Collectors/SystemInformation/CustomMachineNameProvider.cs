using System;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
{
    public class CustomMachineNameProvider : IMachineNameProvider
    {
        private readonly string machineName;

        public CustomMachineNameProvider(string machineName)
        {
            if (string.IsNullOrWhiteSpace(machineName))
            {
                throw new ArgumentException("machineName");
            }

            this.machineName = machineName.Trim();
        }

        public string GetMachineName()
        {
            return this.machineName;
        }
    }
}