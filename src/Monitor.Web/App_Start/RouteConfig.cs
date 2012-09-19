using System.Web.Mvc;
using System.Web.Routing;

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", string.Empty, MVC.SystemMonitor.GroupOverview());

            routes.MapRoute("SystemMonitorDefault", "SystemMonitor", MVC.SystemMonitor.GroupOverview());
            routes.MapRoute("SystemMonitorGroupOverview", "SystemMonitor/GroupOverview", MVC.SystemMonitor.GroupOverview());
            routes.MapRoute("SystemMonitorGroup", "SystemMonitor/Group/{groupName}", MVC.SystemMonitor.Group());

            routes.MapRoute("GroupConfigurationAll", "Configuration/Groups", MVC.GroupConfiguration.EditGroups());
            routes.MapRoute("GroupConfigurationSpecific", "Configuration/Group/{groupName}", MVC.GroupConfiguration.EditGroup());

            routes.MapRoute("AgentConfiguration", "Configuration/Agent", MVC.AgentConfiguration.Index());
        }
    }
}