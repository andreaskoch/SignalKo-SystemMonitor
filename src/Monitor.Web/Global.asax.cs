using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Monitor.Web.App_Start;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;
using SignalKo.SystemMonitor.Monitor.Web.DependencyResolution;

using StructureMap;

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

		protected void Application_End()
		{
			var systemInformationArchive = ObjectFactory.GetInstance<ISystemInformationArchiveAccessor>();
			if (systemInformationArchive != null)
			{
				systemInformationArchive.Dispose();
			}
		}
	}
}