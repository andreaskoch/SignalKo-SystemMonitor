using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModels
{
	public class AgentConfigurationViewModel
	{
		public string[] UnconfiguredAgents { get; set; }

		public AgentConfiguration Configuration { get; set; }
	}
}