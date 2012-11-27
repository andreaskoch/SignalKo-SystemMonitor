using System;
using System.Web.Mvc;

using SignalKo.SystemMonitor.Monitor.Web.Core.Services;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
	public partial class ConfigurationController : Controller
	{
		private readonly IAgentConfigurationService agentConfigurationService;

		private readonly IAgentConfigurationViewModelOrchestrator agentConfigurationViewModelOrchestrator;

		public ConfigurationController(IAgentConfigurationService agentConfigurationService, IAgentConfigurationViewModelOrchestrator agentConfigurationViewModelOrchestrator)
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

		public virtual JsonResult GetAgentConfigurationEditorViewModel()
		{
			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();
			var viewModel = this.agentConfigurationViewModelOrchestrator.GetAgentConfigurationViewModel(agentConfiguration);

			return this.Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		public virtual ActionResult AgentConfiguration()
		{
			return this.View();
		}

		public virtual ActionResult UIConfiguration()
		{
			return this.View();
		}
	}
}
