using System.Web.Mvc;

namespace HardwareStatus.Server.Controllers
{
    public class OverviewController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
