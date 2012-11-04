using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
	public interface IAgentControlDefinitionService
	{
		AgentControlDefinition GetAgentControlDefinition(string machineName);
	}
}