using System;
using System.Net.Http;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
	public class AgentConfigurationController : ApiController
	{
		private readonly IAgentConfigurationService agentConfigurationService;

		private readonly IAgentConfigurationViewModelOrchestrator agentConfigurationViewModelOrchestrator;

		public AgentConfigurationController(IAgentConfigurationService agentConfigurationService, IAgentConfigurationViewModelOrchestrator agentConfigurationViewModelOrchestrator)
		{
			if (agentConfigurationService == null)
			{
				throw new ArgumentNullException("agentConfigurationService");
			}

			if (agentConfigurationViewModelOrchestrator == null)
			{
				throw new ArgumentNullException("agentConfigurationViewModelOrchestrator");
			}

			this.agentConfigurationService = agentConfigurationService;
			this.agentConfigurationViewModelOrchestrator = agentConfigurationViewModelOrchestrator;
		}

		public AgentConfigurationViewModel Get()
		{
			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();

			AgentConfigurationViewModel viewModel = this.agentConfigurationViewModelOrchestrator.GetAgentConfigurationViewModel(agentConfiguration);
			return viewModel;
		}

		public void Post([FromBody]AgentConfiguration agentConfiguration)
		{
			if (agentConfiguration == null)
			{
				throw new HttpRequestException("Supplied agent configuration cannot be null.");
			}

			if (agentConfiguration.IsValid() == false)
			{
				throw new HttpRequestException("Supplied agent configuration is not valid.");
			}

			this.agentConfigurationService.SaveAgentConfiguration(agentConfiguration);
		}
	}
}
