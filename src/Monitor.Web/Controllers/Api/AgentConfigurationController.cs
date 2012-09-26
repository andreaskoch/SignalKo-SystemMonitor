using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
    public class AgentConfigurationController : ApiController
    {
        private readonly IAgentConfigurationService agentConfigurationService;

        public AgentConfigurationController(IAgentConfigurationService agentConfigurationService)
        {
            this.agentConfigurationService = agentConfigurationService;
        }

        public AgentConfiguration Get()
        {
            return this.agentConfigurationService.GetAgentConfiguration();
        }

        public void Post(AgentConfiguration agentConfiguration)
        {
            this.agentConfigurationService.SaveAgentConfiguration(agentConfiguration);
        }
    }
}
