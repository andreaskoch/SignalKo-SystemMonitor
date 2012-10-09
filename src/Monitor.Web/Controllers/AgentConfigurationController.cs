using System;
using System.Web.Mvc;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Monitor.Web.Core.Services;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public partial class AgentConfigurationController : Controller
    {
        private readonly IKnownAgentsProvider knownAgentsProvider;

        public AgentConfigurationController(IKnownAgentsProvider knownAgentsProvider)
        {
            if (knownAgentsProvider == null)
            {
                throw new ArgumentNullException("knownAgentsProvider");
            }

            this.knownAgentsProvider = knownAgentsProvider;
        }

        public virtual ActionResult Index()
        {
            var knownAgents = this.knownAgentsProvider.GetKnownAgents();

            this.ViewBag.AgentsJSON = JsonConvert.SerializeObject(knownAgents);
            return this.View();
        }
    }
}
