using System;
using System.ServiceProcess;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Service.DependencyResolution;

using StructureMap;

namespace SignalKo.SystemMonitor.Agent.Service
{
    public class AgentService : ServiceBase
    {
        private readonly ISystemInformationDispatchingService systemInformationDispatchingService;

        public AgentService(ISystemInformationDispatchingService systemInformationDispatchingService)
        {
            if (systemInformationDispatchingService == null)
            {
                throw new ArgumentNullException("systemInformationDispatchingService");
            }

            this.systemInformationDispatchingService = systemInformationDispatchingService;
        }

        public static void Main(string[] args)
        {
            StructureMapSetup.Setup();

            var agentSerice = new AgentService(ObjectFactory.GetInstance<ISystemInformationDispatchingService>());

            var servicesToRun = new ServiceBase[] { agentSerice };
            ServiceBase.Run(servicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            var startService = new Task(this.systemInformationDispatchingService.Start);
            startService.Start();
        }

        protected override void OnStop()
        {
            this.systemInformationDispatchingService.Stop();
        }
    }
}