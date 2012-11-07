using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
	public interface IAgentConfigurationService
	{
		AgentConfiguration GetAgentConfiguration();

		void SaveAgentConfiguration(AgentConfiguration agentConfiguration);
	}
}