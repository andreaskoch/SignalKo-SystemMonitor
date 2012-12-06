using System.Web.Mvc;
using System.Web.Routing;

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			/* charts */
			routes.MapRoute(MVC.Charts.Name + MVC.Charts.ActionNames.AgentGroupOverview, "charts/overview", MVC.Charts.AgentGroupOverview());

			/* ui configuration */
			routes.MapRoute(MVC.UIConfiguration.Name + MVC.UIConfiguration.ActionNames.Editor, "configuration/ui/editor", MVC.UIConfiguration.Editor());

			/* agent configuration */
			routes.MapRoute(MVC.AgentConfiguration.Name + MVC.AgentConfiguration.ActionNames.Editor, "configuration/agents/editor", MVC.AgentConfiguration.Editor());
		}
	}
}