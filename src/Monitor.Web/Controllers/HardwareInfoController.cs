using System.Web.Http;

using HardwareStatus.Server.Hubs;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class HardwareInfoController : ApiController
    {
        public void Put(HardwareInfo hardwareInfo)
        {
            var context = SignalR.GlobalHost.ConnectionManager.GetHubContext<HardwareStatusHub>();
            context.Clients.displayHardwareInfo(hardwareInfo);
        }
    }
}