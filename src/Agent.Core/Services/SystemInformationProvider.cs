using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Services
{
    public class SystemInformationProvider : ISystemInformationProvider
    {
        private readonly ITimeProvider timeProvider;

        private readonly IMachineNameProvider machineNameProvider;

        private readonly IProcessorStatusProvider processorStatusProvider;

        private readonly ISystemMemoryStatusProvider systemMemoryStatusProvider;

        private readonly ISystemStorageStatusProvider systemStorageStatusProvider;

        public SystemInformationProvider(ITimeProvider timeProvider, IMachineNameProvider machineNameProvider, IProcessorStatusProvider processorStatusProvider, ISystemMemoryStatusProvider systemMemoryStatusProvider, ISystemStorageStatusProvider systemStorageStatusProvider)
        {
            this.timeProvider = timeProvider;
            this.machineNameProvider = machineNameProvider;
            this.processorStatusProvider = processorStatusProvider;
            this.systemMemoryStatusProvider = systemMemoryStatusProvider;
            this.systemStorageStatusProvider = systemStorageStatusProvider;
        }

        public SystemInformation GetSystemInfo()
        {
            DateTimeOffset startTime = this.timeProvider.GetUTCDateAndTime();

            // collect data
            string machineName = this.machineNameProvider.GetMachineName();
            var processorStatus = this.processorStatusProvider.GetProcessorStatus();
            var memoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus();
            var storageStatus = this.systemStorageStatusProvider.GetStorageStatus();

            // return result
            var timeFrame = new DataCollectionTimeFrame
                {
                    Start = startTime,
                    End = this.timeProvider.GetUTCDateAndTime()
                };

            return new SystemInformation
                {
                    TimeFrame = timeFrame,
                    MachineName = machineName,
                    ProcessorStatus = processorStatus,
                    MemoryStatus = memoryStatus,
                    StorageStatus = storageStatus
                };
        }
    }
}