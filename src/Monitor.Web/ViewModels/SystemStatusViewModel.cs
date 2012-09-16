using System;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModels
{
    public class SystemStatusViewModel
    {
        public string MachineName { get; set; }

        public DateTime Timestamp { get; set; }

        public SystemStatusPointViewModel[] DataPoints { get; set; }
    }
}