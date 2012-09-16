using System.Collections.Generic;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class SystemStatusOrchestrator : ISystemStatusOrchestrator
    {
        private readonly IProcessorStatusOrchestrator processorStatusOrchestrator;

        private readonly IMemoryStatusOrchestrator memoryStatusOrchestrator;

        private readonly IStorageStatusOrchestrator storageStatusOrchestrator;

        public SystemStatusOrchestrator(IProcessorStatusOrchestrator processorStatusOrchestrator, IMemoryStatusOrchestrator memoryStatusOrchestrator, IStorageStatusOrchestrator storageStatusOrchestrator)
        {
            this.processorStatusOrchestrator = processorStatusOrchestrator;
            this.memoryStatusOrchestrator = memoryStatusOrchestrator;
            this.storageStatusOrchestrator = storageStatusOrchestrator;
        }

        public SystemStatusViewModel GetSystemStatusViewModel(SystemInformation systemInformation)
        {
            var systemStatusViewModel = new SystemStatusViewModel { MachineName = systemInformation.MachineName, Timestamp = systemInformation.Timestamp, };

            var dataSerieses = new List<SystemStatusPointViewModel>
                {
                    this.processorStatusOrchestrator.GetProcessorUtilizationInPercent(systemInformation.ProcessorStatus),
                    this.memoryStatusOrchestrator.GetMemoryUtilizationInPercent(systemInformation.MemoryStatus)
                };
            dataSerieses.AddRange(this.storageStatusOrchestrator.GetStorageUtilizationInPercent(systemInformation.StorageStatus));

            systemStatusViewModel.DataPoints = dataSerieses.ToArray();

            return systemStatusViewModel;
        }
    }
}