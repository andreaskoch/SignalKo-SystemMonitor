using System.Web.Http;

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("api-default", "api/{controller}/{data}", new { data = RouteParameter.Optional });
        }
    }
}
