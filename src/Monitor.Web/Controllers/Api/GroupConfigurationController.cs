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

		private readonly IGroupConfigurationViewModelOrchestrator groupConfigurationViewModelOrchestrator;

		public GroupConfigurationController(IGroupConfigurationService groupConfigurationService, IGroupConfigurationViewModelOrchestrator groupConfigurationViewModelOrchestrator)
		{
			if (groupConfigurationService == null)
			{
				throw new ArgumentNullException("groupConfigurationService");
			}

			if (groupConfigurationViewModelOrchestrator == null)
			{
				throw new ArgumentNullException("groupConfigurationViewModelOrchestrator");
			}

			this.groupConfigurationService = groupConfigurationService;
			this.groupConfigurationViewModelOrchestrator = groupConfigurationViewModelOrchestrator;
		}

		public GroupConfigurationViewModel Get()
		{
			var groupConfiguration = this.groupConfigurationService.GetGroupConfiguration();

			GroupConfigurationViewModel viewModel = this.groupConfigurationViewModelOrchestrator.GetGroupConfigurationViewModel(groupConfiguration);
			return viewModel;
		}

		public void Post(GroupConfigurationViewModel groupConfigurationViewModel)
		{
			if (groupConfigurationViewModel == null)
			{
				throw new HttpRequestException("The supplied group configuration view model cannot be null.");
			}

			GroupConfiguration groupConfiguration = this.groupConfigurationViewModelOrchestrator.GetGroupConfiguration(groupConfigurationViewModel);
			this.groupConfigurationService.SaveGroupConfiguration(groupConfiguration);
		}
	}
}
