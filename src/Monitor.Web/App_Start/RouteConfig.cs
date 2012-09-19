using System.Web.Mvc;
using System.Web.Routing;

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", string.Empty, new { controller = "Overview", action = "Index" });
            routes.MapRoute("SystemMonitorOverview", "SystemMonitor/Overview", new { controller = "SystemMonitor", action = "Index" });
        }
    }
}