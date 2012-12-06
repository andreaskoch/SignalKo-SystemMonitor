using System;
using System.Net.Http;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
	public class GroupConfigurationController : ApiController
	{
		private readonly IGroupConfigurationService groupConfigurationService;

		private readonly IAgentConfigurationService agentConfigurationService;

		private readonly IGroupConfigurationViewModelOrchestrator groupConfigurationViewModelOrchestrator;

		public GroupConfigurationController(IGroupConfigurationService groupConfigurationService, IAgentConfigurationService agentConfigurationService, IGroupConfigurationViewModelOrchestrator groupConfigurationViewModelOrchestrator)
		{
			if (groupConfigurationService == null)
			{
				throw new ArgumentNullException("groupConfigurationService");
			}
			if (agentConfigurationService == null)
			{
				throw new ArgumentNullException("agentConfigurationService");
			}

			if (groupConfigurationViewModelOrchestrator == null)
			{
				throw new ArgumentNullException("groupConfigurationViewModelOrchestrator");
			}

			this.groupConfigurationService = groupConfigurationService;
			this.agentConfigurationService = agentConfigurationService;
			this.groupConfigurationViewModelOrchestrator = groupConfigurationViewModelOrchestrator;
		}

		public GroupConfigurationViewModel Get()
		{
			var groupConfiguration = this.groupConfigurationService.GetGroupConfiguration();
			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();

			GroupConfigurationViewModel viewModel = this.groupConfigurationViewModelOrchestrator.GetGroupConfigurationViewModel(groupConfiguration, agentConfiguration.AgentInstanceConfigurations.ToArray());
			return viewModel;
		}

		public void Post(GroupConfiguration groupConfiguration)
		{
			if (groupConfiguration == null)
			{
				throw new HttpRequestException("The supplied group configuration cannot be null.");
			}

			this.groupConfigurationService.SaveGroupConfiguration(groupConfiguration);
		}
	}
}
