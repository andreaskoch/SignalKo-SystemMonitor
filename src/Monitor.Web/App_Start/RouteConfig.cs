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
			routes.MapRoute(MVC.Charts.Name + MVC.Charts.ActionNames.AgentGroupOverview, "charts/overview.html", MVC.Charts.AgentGroupOverview());

			/* ui configuration */
			routes.MapRoute(MVC.UIConfiguration.Name + MVC.UIConfiguration.ActionNames.Editor, "configuration/ui/editor", MVC.UIConfiguration.Editor());
			routes.MapRoute(MVC.UIConfiguration.Name + MVC.UIConfiguration.ActionNames.Load, "configuration/ui/load", MVC.UIConfiguration.Load());
			routes.MapRoute(MVC.UIConfiguration.Name + MVC.UIConfiguration.ActionNames.Save, "configuration/ui/save", MVC.UIConfiguration.Save());

			/* agent configuration */
			routes.MapRoute(MVC.AgentConfiguration.Name + MVC.AgentConfiguration.ActionNames.Editor, "configuration/agents/editor", MVC.AgentConfiguration.Editor());
			routes.MapRoute(MVC.AgentConfiguration.Name + MVC.AgentConfiguration.ActionNames.Load, "configuration/agents/load", MVC.AgentConfiguration.Load());
			routes.MapRoute(MVC.AgentConfiguration.Name + MVC.AgentConfiguration.ActionNames.Save, "configuration/agents/save", MVC.AgentConfiguration.Save());
		}
	}
}