using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class SystemStatusOrchestrator : ISystemStatusOrchestrator
    {
        private const string CPUUtilizationDataSeriesName = "CPU Utilization in %";

        private const string MemorytilizationDataSeriesName = "Memory Utilization in %";

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
                                    },
                                new SystemStatusPointViewModel
                                    {
                                        Name = MemorytilizationDataSeriesName, 
                                        Value = systemInformation.MemoryStatus.UsedMemoryInGB * 100 / (systemInformation.MemoryStatus.UsedMemoryInGB + systemInformation.MemoryStatus.AvailableMemoryInGB)
                                    }
                            }
                };
        }
    }
}