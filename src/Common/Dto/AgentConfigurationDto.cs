namespace SignalKo.SystemMonitor.Common.Dto
{
    public class AgentConfigurationDto
    {
        public string Hostaddress { get; set; }

        public string Hostname { get; set; }

        public string SystemInformationSenderPath { get; set; }

        public bool AgentsAreEnabled { get; set; }

        public int CheckIntervalInSeconds { get; set; }

		public AgentInstanceConfigurationDto[] AgentInstanceConfigurations { get; set; }
    }
}