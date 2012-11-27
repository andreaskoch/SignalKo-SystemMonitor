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
			routes.MapRoute(MVC.Charts.ActionNames.AgentGroupOverview, "charts/overview.html", MVC.Charts.AgentGroupOverview());

			/* ui configuration */
			routes.MapRoute(MVC.Configuration.ActionNames.UIConfiguration, "configuration/display.html", MVC.Configuration.UIConfiguration());

			/* agent configuration */
			routes.MapRoute(MVC.Configuration.ActionNames.AgentConfiguration, "configuration/agents.html", MVC.Configuration.AgentConfiguration());
			routes.MapRoute(MVC.Configuration.ActionNames.GetAgentConfigurationEditorViewModel, "configuration/agents/viewmodel.json", MVC.Configuration.GetAgentConfigurationEditorViewModel());
		}
	}
}