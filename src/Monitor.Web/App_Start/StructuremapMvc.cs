using System.Web.Http;
using System.Web.Mvc;

using SignalKo.SystemMonitor.Monitor.Web.DependencyResolution;

using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SignalKo.SystemMonitor.Monitor.Web.App_Start.StructuremapMvc), "Start")]

namespace SignalKo.SystemMonitor.Monitor.Web.App_Start
{
	public static class StructuremapMvc
	{
		public static void Start()
		{
			IContainer container = StructureMapSetup.Initialize();
			DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
		}
	}
}