using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public interface IAgentConfigurationProvider
    {
        AgentConfiguration GetAgentConfiguration();
    }
}