using HardwareStatus.Common.Model;

using SignalR.Hubs;

namespace HardwareStatus.Server.Hubs
{
    [HubName("hardwareStatus")]
    public class HardwareStatusHub : Hub
    {
        public void SendHardwareInfo(HardwareInfo hardwareInfo)
        {
            this.Clients.displayHardwareInfo(hardwareInfo);
        }
    }
}