using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public interface IAgentControlDefinitionAccessor
	{
		AgentControlDefinition GetControlDefinition();
	}
}