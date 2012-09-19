using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class SystemMonitorController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
