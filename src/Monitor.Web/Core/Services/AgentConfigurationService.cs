using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
    public class AgentConfigurationService : IAgentConfigurationService
    {
        private readonly IAgentConfigurationDataAccessor agentConfigurationDataAccessor;

        public AgentConfigurationService(IAgentConfigurationDataAccessor agentConfigurationDataAccessor)
        {
            this.agentConfigurationDataAccessor = agentConfigurationDataAccessor;
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            return this.agentConfigurationDataAccessor.Load();
        }

        public void SaveAgentConfiguration(AgentConfiguration agentConfiguration)
        {
            this.agentConfigurationDataAccessor.Store(agentConfiguration);
        }
    }
}