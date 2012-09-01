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
            return new SystemInformation
                {
                    Timestamp = this.timeProvider.GetUTCDateAndTime(),
                    MachineName = this.machineNameProvider.GetMachineName(),
                    ProcessorStatus = this.processorStatusProvider.GetProcessorUtilizationInPercent(),
                    MemoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus(),
                    StorageStatus = this.systemStorageStatusProvider.GetStorageStatus()
                };
        }
    }
}