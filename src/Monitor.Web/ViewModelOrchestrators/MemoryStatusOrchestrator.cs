using System;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class MemoryStatusOrchestrator : IMemoryStatusOrchestrator
    {
        private const string MemoryUtilizationDataSeriesName = "Memory Utilization in %";

        public SystemStatusPointViewModel GetMemoryUtilizationInPercent(SystemMemoryInformation systemMemoryInformation)
        {
            if (systemMemoryInformation == null)
            {
                throw new ArgumentNullException("systemMemoryInformation");
            }

            var totalMemory = systemMemoryInformation.UsedMemoryInGB + systemMemoryInformation.AvailableMemoryInGB;
            double memoryUtilizationInPercent = 0d;
            if (totalMemory > 0d)
            {
                memoryUtilizationInPercent = systemMemoryInformation.UsedMemoryInGB * 100 / totalMemory;
            }

            return new SystemStatusPointViewModel
                {
                    Name = MemoryUtilizationDataSeriesName,
                    Value = memoryUtilizationInPercent
                };
        }
    }
}