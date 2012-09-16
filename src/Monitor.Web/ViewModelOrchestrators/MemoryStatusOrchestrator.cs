using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class MemoryStatusOrchestrator : IMemoryStatusOrchestrator
    {
        private const string MemoryUtilizationDataSeriesName = "Memory Utilization in %";

        public SystemStatusPointViewModel GetMemoryUtilizationInPercent(SystemMemoryInformation systemMemoryInformation)
        {
            return new SystemStatusPointViewModel
                {
                    Name = MemoryUtilizationDataSeriesName,
                    Value = systemMemoryInformation.UsedMemoryInGB * 100 / (systemMemoryInformation.UsedMemoryInGB + systemMemoryInformation.AvailableMemoryInGB)
                };
        }
    }
}