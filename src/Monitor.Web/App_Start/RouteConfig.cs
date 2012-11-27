using System.Web.Mvc;
using System.Web.Routing;

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(MVC.Charts.ActionNames.AgentGroupOverview, "Charts/All", MVC.Charts.AgentGroupOverview());
			routes.MapRoute(MVC.Configuration.ActionNames.UIConfiguration, "Configuration/UI", MVC.Configuration.UIConfiguration());
			routes.MapRoute(MVC.Configuration.ActionNames.AgentConfiguration, "Configuration/Agents", MVC.Configuration.AgentConfiguration());
			routes.MapRoute(MVC.Configuration.ActionNames.GetAgentConfiguration, "Configuration/Agents/JSON/EditorViewModel", MVC.Configuration.GetAgentConfigurationEditorViewModel());
		}
	}
}