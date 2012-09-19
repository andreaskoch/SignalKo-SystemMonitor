using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public partial class GroupConfigurationController : Controller
    {
        public virtual ActionResult EditGroups()
        {
            return this.View();
        }

        public virtual ActionResult EditGroup(string groupName)
        {
            return this.View();
        }
    }
}
