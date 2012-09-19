using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public partial class AgentConfigurationController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
