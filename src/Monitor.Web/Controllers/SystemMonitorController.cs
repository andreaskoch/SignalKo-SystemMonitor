using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class SystemMonitorController : Controller
    {
        public ActionResult GroupOverview()
        {
            return this.View();
        }

        public ActionResult Group(string groupName)
        {
            return this.View();
        }
    }
}
