using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.DependencyResolution;

using StructureMap;

using Topshelf;

namespace SignalKo.SystemMonitor.Agent
{
	public class Program
	{
		public static int Main(string[] args)
		{
			StructureMapSetup.Setup();

			var systemInformationDispatchingService = ObjectFactory.GetInstance<ISystemInformationDispatchingService>();

			return (int)HostFactory.Run(
				x =>
				{
					x.Service<ISystemInformationDispatchingService>(
						s =>
						{
							s.ConstructUsing(name => systemInformationDispatchingService);
							s.WhenStarted(d => d.Start());
							s.WhenStopped(d => d.Stop());
						});

					x.RunAsLocalSystem();

					x.SetDisplayName("SystemMonitorAgent");
					x.SetServiceName("SystemMonitorAgent");
					x.SetDescription(
						"The service collects information such as system performance data and sends them over the configured monitoring service where the information is displayed in real-time. For further details and the source code to this service visit https://github.com/andreaskoch/SignalKo-SystemMonitor");
				});
		}
	}
}
