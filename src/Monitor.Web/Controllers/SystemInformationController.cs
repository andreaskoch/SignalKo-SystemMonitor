using System;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Hubs;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
    public class SystemInformationController : ApiController
    {
        public void Put(SystemInformation systemInformation)
        {
            var systemStatusViewModel = new SystemStatusViewModel
                {
                    
                    MachineName = systemInformation.MachineName,
                    Timestamp =  systemInformation.TimeStampString,
                    DataPoints =
                        new[]
                            { new SystemStatusPointViewModel { Name = "CPU Utilization in %", Value = systemInformation.ProcessorStatus.ProcessorUtilizationInPercent } }
                };

            var context = SignalR.GlobalHost.ConnectionManager.GetHubContext<SystemInformationHub>();
            context.Clients.displaySystemStatus(systemStatusViewModel);
        }
    }
}