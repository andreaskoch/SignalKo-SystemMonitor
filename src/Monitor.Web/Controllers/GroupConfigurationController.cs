using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class GroupConfigurationController : Controller
    {
        public ActionResult EditGroups()
        {
            return this.View();
        }

        public ActionResult EditGroup(string groupName)
        {
            return this.View();
        }
    }
}
