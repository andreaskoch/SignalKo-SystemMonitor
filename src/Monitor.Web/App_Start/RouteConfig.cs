using System.Web.Mvc;
using System.Web.Routing;

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", string.Empty, new { controller = "SystemMonitor", action = "GroupOverview" });

            routes.MapRoute("SystemMonitorGroupOverview", "SystemMonitor/GroupOverview", new { controller = "SystemMonitor", action = "GroupOverview" });
            routes.MapRoute("SystemMonitorGroup", "SystemMonitor/Group/{groupName}", new { controller = "SystemMonitor", action = "Group" });
        }
    }
}