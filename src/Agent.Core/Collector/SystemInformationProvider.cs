using System;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Agent.Core.Collector
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

        public SystemInformation GetSystemInfo()
        {
            var timestamp = this.timeProvider.GetDateAndTime();
            return new SystemInformation
                {
                    Timestamp = timestamp,
                    TimeStampString = timestamp.ToString(),
                    MachineName = this.machineNameProvider.GetMachineName(),
                    ProcessorStatus = this.processorStatusProvider.GetProcessorStatus(),
                    MemoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus(),
                    StorageStatus = this.systemStorageStatusProvider.GetStorageStatus()
                };
        }
    }
}