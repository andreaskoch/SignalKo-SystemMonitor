using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class SystemStatusOrchestrator : ISystemStatusOrchestrator
    {
        private const string CPUUtilizationDataSeriesName = "CPU Utilization in %";

        public SystemStatusViewModel GetSystemStatusViewModel(SystemInformation systemInformation)
        {
            return new SystemStatusViewModel
                {
                    MachineName = systemInformation.MachineName,
                    Timestamp = systemInformation.Timestamp,
                    DataPoints =
                        new[]
                            {
                                new SystemStatusPointViewModel
                                    {
                                        Name = CPUUtilizationDataSeriesName, 
                                        Value = systemInformation.ProcessorStatus.ProcessorUtilizationInPercent
                                    }
                            }
                };
        }
    }
}