namespace SignalKo.SystemMonitor.Common.Model
{
	public class AgentInstanceConfiguration
	{
		public string MachineName { get; set; }

		public bool AgentIsEnabled { get; set; }

		public ICollectorDefinition[] CollectorDefinitions { get; set; }
	}
}