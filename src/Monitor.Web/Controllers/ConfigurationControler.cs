using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
	public partial class ConfigurationController : Controller
	{
		public virtual ActionResult AgentConfiguration()
		{
			return this.View();
		}
	}
}
