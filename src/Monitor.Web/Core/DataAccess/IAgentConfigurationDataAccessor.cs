using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess
{
    public interface IAgentConfigurationDataAccessor
    {
        AgentConfiguration Load();

        void Store(AgentConfiguration agentConfiguration);
    }
}