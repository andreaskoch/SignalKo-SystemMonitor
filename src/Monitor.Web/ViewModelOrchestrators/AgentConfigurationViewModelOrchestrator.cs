using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
	public class AgentConfigurationViewModelOrchestrator : IAgentConfigurationViewModelOrchestrator
	{
		public AgentConfigurationViewModel GetAgentConfigurationViewModel(AgentConfiguration agentConfiguration)
		{
			return new AgentConfigurationViewModel { Configuration = agentConfiguration };
		}
	}
}