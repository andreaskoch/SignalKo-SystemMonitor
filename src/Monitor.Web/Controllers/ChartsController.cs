using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
	public partial class ChartsController : Controller
	{
		public virtual ActionResult AgentGroupOverview()
		{
			return this.View();
		}

		public virtual ActionResult AgentGroup(string groupName)
		{
			return this.View();
		}
	}
}
