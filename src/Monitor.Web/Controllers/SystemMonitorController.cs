using System;
using System.Web.Mvc;

using SignalKo.SystemMonitor.Monitor.Web.Controllers.Api;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public partial class SystemMonitorController : Controller
    {
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
        public void SaveConfig(string configuration)
        {
            ConfigurationRepository repository = new ConfigurationRepository();
            repository.SaveConfiguration(configuration);
        }

        [HttpGet]
        public JsonResult LoadConfig()
        {
            ConfigurationRepository repository = new ConfigurationRepository();
            string data = repository.LoadConfiguration();

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
