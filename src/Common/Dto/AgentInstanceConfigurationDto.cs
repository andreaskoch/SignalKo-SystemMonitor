namespace SignalKo.SystemMonitor.Common.Dto
{
	public class AgentInstanceConfigurationDto
	{
		public string MachineName { get; set; }

		public bool AgentIsEnabled { get; set; }

		public CollectorDefinitionDto[] CollectorDefinitions { get; set; }
	}
}