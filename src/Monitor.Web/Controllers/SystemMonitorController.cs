using System.Web.Mvc;

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
    }
}
