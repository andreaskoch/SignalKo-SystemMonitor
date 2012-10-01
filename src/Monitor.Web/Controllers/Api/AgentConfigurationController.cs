using System.Net;
using System.Net.Http;
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

        public HttpResponseMessage Post(AgentConfiguration agentConfiguration)
        {
            if (agentConfiguration == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (agentConfiguration.IsValid() == false)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }

            this.agentConfigurationService.SaveAgentConfiguration(agentConfiguration);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
