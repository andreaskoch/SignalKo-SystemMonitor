using System.Web.Http;

using HardwareStatus.Common.Model;
using HardwareStatus.Server.Hubs;

namespace HardwareStatus.Server.Controllers
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