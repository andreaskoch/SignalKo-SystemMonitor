using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
    public class AgentConfigurationService : IAgentConfigurationService
    {
        private readonly IAgentConfigurationDataAccessor agentConfigurationDataAccessor;

        private readonly IDefaultAgentConfigurationProvider defaultAgentConfigurationProvider;

        public AgentConfigurationService(IAgentConfigurationDataAccessor agentConfigurationDataAccessor, IDefaultAgentConfigurationProvider defaultAgentConfigurationProvider)
        {
            this.agentConfigurationDataAccessor = agentConfigurationDataAccessor;
            this.defaultAgentConfigurationProvider = defaultAgentConfigurationProvider;
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            var agentConfiguration = this.agentConfigurationDataAccessor.Load();
            return agentConfiguration ?? this.defaultAgentConfigurationProvider.GetDefaultAgentConfiguration();
        }

        public void SaveAgentConfiguration(AgentConfiguration agentConfiguration)
        {
            this.agentConfigurationDataAccessor.Store(agentConfiguration);
        }
    }
}