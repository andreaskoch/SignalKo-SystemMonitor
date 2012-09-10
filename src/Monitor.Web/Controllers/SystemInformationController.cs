using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Hubs;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class SystemInformationController : ApiController
    {
        public void Put(SystemInformation systemInformation)
        {
            var context = SignalR.GlobalHost.ConnectionManager.GetHubContext<SystemInformationHub>();
            context.Clients.displaySystemInformation(systemInformation);
        }
    }
}