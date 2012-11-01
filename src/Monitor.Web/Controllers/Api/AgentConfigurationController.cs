using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Dto;
using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Mapper;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
	public class AgentConfigurationController : ApiController
	{
		private readonly IAgentConfigurationService agentConfigurationService;

		private readonly IAgentConfigurationMapper agentConfigurationMapper;

		public AgentConfigurationController(IAgentConfigurationService agentConfigurationService, IAgentConfigurationMapper agentConfigurationMapper)
		{
			if (agentConfigurationService == null)
			{
				throw new ArgumentNullException("agentConfigurationService");
			}

			if (agentConfigurationMapper == null)
			{
				throw new ArgumentNullException("agentConfigurationMapper");
			}

			this.agentConfigurationService = agentConfigurationService;
			this.agentConfigurationMapper = agentConfigurationMapper;
		}

		public AgentConfiguration Get()
		{
			return this.agentConfigurationService.GetAgentConfiguration();
		}

		public HttpResponseMessage Post(AgentConfigurationDto agentConfigurationDto)
		{
			if (agentConfigurationDto == null)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}

			var agentConfiguration = this.agentConfigurationMapper.Map(agentConfigurationDto);
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
