using System.Web.Mvc;
using SignalKo.SystemMonitor.Common.Model.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.Controllers.Api;
using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public partial class SystemMonitorController : Controller
    {
        private readonly IServerConfigurationRepository configurationRepository;

        public SystemMonitorController(IServerConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public virtual ActionResult GroupOverview()
        {
            return this.View();
        }

        public virtual ActionResult Group(string groupName)
        {
            return this.View();
        }

        public virtual ActionResult ServerConfiguration()
        {
            return this.View();
        }

        [HttpPost]
        public void SaveConfig(MachineGroupConfiguration configuration)
        {
            if (configuration == null)
            {
                return;
            }

            this.configurationRepository.SaveConfiguration(configuration);
        }

        [HttpGet]
        public JsonResult LoadConfig()
        {
            MachineGroupConfiguration configuration = this.configurationRepository.LoadConfiguration();

            return this.Json(configuration, JsonRequestBehavior.AllowGet);
        }
    }
}
