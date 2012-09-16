using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Hubs;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class SystemInformationController : ApiController
    {
        private readonly ISystemStatusOrchestrator systemStatusOrchestrator;

        public SystemInformationController(ISystemStatusOrchestrator systemStatusOrchestrator)
        {
            this.systemStatusOrchestrator = systemStatusOrchestrator;
        }

        public void Put(SystemInformation systemInformation)
        {
            var systemStatusViewModel = this.systemStatusOrchestrator.GetSystemStatusViewModel(systemInformation);

            var context = SignalR.GlobalHost.ConnectionManager.GetHubContext<SystemInformationHub>();
            context.Clients.displaySystemStatus(systemStatusViewModel);
        }
    }
}