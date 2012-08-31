using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Services
{
    public class SystemInformationProvider : ISystemInformationProvider
    {
        private readonly ITimeProvider timeProvider;

        private readonly IMachineNameProvider machineNameProvider;

        private readonly ISystemMemoryStatusProvider systemMemoryStatusProvider;

        private readonly ISystemStorageStatusProvider systemStorageStatusProvider;

        public SystemInformationProvider(ITimeProvider timeProvider, IMachineNameProvider machineNameProvider, ISystemMemoryStatusProvider systemMemoryStatusProvider, ISystemStorageStatusProvider systemStorageStatusProvider)
        {
            this.timeProvider = timeProvider;
            this.machineNameProvider = machineNameProvider;
            this.systemMemoryStatusProvider = systemMemoryStatusProvider;
            this.systemStorageStatusProvider = systemStorageStatusProvider;
        }

        public SystemInformation GetSystemInfo()
        {
            return new SystemInformation
                {
                    Timestamp = this.timeProvider.GetUTCDateAndTime(),
                    MachineName = this.machineNameProvider.GetMachineName(),
                    MemoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus(),
                    StorageStatus = this.systemStorageStatusProvider.GetStorageStatus()
                };
        }
    }
}