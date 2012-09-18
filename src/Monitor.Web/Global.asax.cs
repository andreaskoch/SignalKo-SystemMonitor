using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using SignalKo.SystemMonitor.Monitor.Web.App_Start;
using SignalKo.SystemMonitor.Monitor.Web.DependencyResolution;

namespace SignalKo.SystemMonitor.Monitor.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            StructureMapSetup.Initialize();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}