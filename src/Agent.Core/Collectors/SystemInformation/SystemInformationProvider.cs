using System;

using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
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
            if (timeProvider == null)
            {
                throw new ArgumentNullException("timeProvider");
            }

            if (machineNameProvider == null)
            {
                throw new ArgumentNullException("machineNameProvider");
            }

            if (processorStatusProvider == null)
            {
                throw new ArgumentNullException("processorStatusProvider");
            }

            if (systemMemoryStatusProvider == null)
            {
                throw new ArgumentNullException("systemMemoryStatusProvider");
            }

            if (systemStorageStatusProvider == null)
            {
                throw new ArgumentNullException("systemStorageStatusProvider");
            }

            this.timeProvider = timeProvider;
            this.machineNameProvider = machineNameProvider;
            this.processorStatusProvider = processorStatusProvider;
            this.systemMemoryStatusProvider = systemMemoryStatusProvider;
            this.systemStorageStatusProvider = systemStorageStatusProvider;
        }

        public Common.Model.SystemInformation GetSystemInfo()
        {
            return new Common.Model.SystemInformation
                {
                    Timestamp = this.timeProvider.GetDateAndTime(),
                    MachineName = this.machineNameProvider.GetMachineName(),
                    ProcessorStatus = this.processorStatusProvider.GetProcessorStatus(),
                    MemoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus(),
                    StorageStatus = this.systemStorageStatusProvider.GetStorageStatus()
                };
        }
    }
}