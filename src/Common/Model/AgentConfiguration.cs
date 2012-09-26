namespace SignalKo.SystemMonitor.Common.Model
{
    public class AgentConfiguration
    {
        public string SystemInformationSenderUrl { get; set; }

        public bool AgentsAreEnabled { get; set; }

        public int CheckIntervalInSeconds { get; set; }
    }
}