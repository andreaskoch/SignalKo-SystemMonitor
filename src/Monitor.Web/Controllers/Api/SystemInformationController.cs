using System;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;
using SignalKo.SystemMonitor.Monitor.Web.Hubs;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
    public class SystemInformationController : ApiController
    {
        private readonly ISystemStatusOrchestrator systemStatusOrchestrator;

        private readonly ISystemInformationArchiveAccessor systemInformationArchiveAccessor;

        public SystemInformationController(ISystemStatusOrchestrator systemStatusOrchestrator, ISystemInformationArchiveAccessor systemInformationArchiveAccessor)
        {
            if (systemStatusOrchestrator == null)
            {
                throw new ArgumentNullException("systemStatusOrchestrator");
            }

            if (systemInformationArchiveAccessor == null)
            {
                throw new ArgumentNullException("systemInformationArchiveAccessor");
            }

            this.systemStatusOrchestrator = systemStatusOrchestrator;
            this.systemInformationArchiveAccessor = systemInformationArchiveAccessor;
        }

        public void Put(SystemInformation systemInformation)
        {
            this.systemInformationArchiveAccessor.Store(systemInformation);
            var systemStatusViewModel = this.systemStatusOrchestrator.GetSystemStatusViewModel(systemInformation);

            var context = SignalR.GlobalHost.ConnectionManager.GetHubContext<SystemInformationHub>();
            context.Clients.displaySystemStatus(systemStatusViewModel);
        }
    }
}