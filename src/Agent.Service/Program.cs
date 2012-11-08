using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Service.DependencyResolution;

using StructureMap;

using Topshelf;

namespace SignalKo.SystemMonitor.Agent.Service
{
	public class Program
	{
		public static void Main()
		{
			StructureMapSetup.Setup();

			HostFactory.Run(
				x =>
					{
						x.Service<ISystemInformationDispatchingService>(
							s =>
								{
									s.ConstructUsing(name => ObjectFactory.GetInstance<ISystemInformationDispatchingService>());
									s.WhenStarted(d => d.Start());
									s.WhenStopped(d => d.Stop());
								});

						x.RunAsLocalSystem();

						x.SetDisplayName("SignalKo.SystemMonitor.Agent.Service");
						x.SetServiceName("SignalKo.SystemMonitor.Agent.Service");
						x.SetDescription("The SignalKo.SystemMonitor agent service collects information such as system performance data and sends them over the configured monitoring service where the information is displayed in real-time. For further details and the source code to this service visit https://github.com/andreaskoch/SignalKo-SystemMonitor");
					});
		}
	}
}