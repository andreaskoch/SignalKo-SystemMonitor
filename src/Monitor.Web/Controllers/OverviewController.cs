using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class OverviewController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
