using System.Web.Http;

namespace HardwareStatus.Server.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "HardwareInfoCollector",
                routeTemplate: "api/{controller}/{data}",
                defaults: new { data = RouteParameter.Optional }
            );
        }
    }
}
