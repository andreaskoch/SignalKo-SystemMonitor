namespace SignalKo.SystemMonitor.Agent.Core.Collector
{
    public class CustomMachineNameProvider : IMachineNameProvider
    {
        private readonly string machineName;

        public CustomMachineNameProvider(string machineName)
        {
            this.machineName = machineName;
        }

        public string GetMachineName()
        {
            return this.machineName;
        }
    }
}