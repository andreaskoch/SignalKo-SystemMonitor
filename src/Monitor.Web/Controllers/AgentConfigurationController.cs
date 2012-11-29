using System;
using System.Web.Mvc;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
	public partial class AgentConfigurationController : Controller
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

		public virtual JsonResult Load()
		{
			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();
			var viewModel = this.agentConfigurationViewModelOrchestrator.GetAgentConfigurationViewModel(agentConfiguration);

			return this.Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public virtual ActionResult Save(AgentConfiguration agentConfiguration)
		{
			if (agentConfiguration == null)
			{
				return new HttpStatusCodeResult(400);
			}

			if (agentConfiguration.IsValid() == false)
			{
				return new HttpStatusCodeResult(400);
			}

			this.agentConfigurationService.SaveAgentConfiguration(agentConfiguration);
			return new ContentResult { Content = "" };
		}

		public virtual ActionResult Editor()
		{
			return this.View();
		}
	}
}
