using System;
using System.Diagnostics;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Agent.Core.Collector
{
    public class SystemMemoryStatusProvider : ISystemMemoryStatusProvider, IDisposable
    {
        private readonly IMemoryUnitConverter memoryUnitConverter;

        private readonly PerformanceCounter freeMemoryInMegaBytesPerformanceCounter;

        private readonly PerformanceCounter usedMemoryInBytesPerformanceCounter;

        public SystemMemoryStatusProvider(IMemoryUnitConverter memoryUnitConverter)
        {
            if (memoryUnitConverter == null)
            {
                throw new ArgumentNullException("memoryUnitConverter");
            }

            this.memoryUnitConverter = memoryUnitConverter;

            // initialize performance counters
            this.freeMemoryInMegaBytesPerformanceCounter = new PerformanceCounter("Memory", "Available MBytes");
            this.usedMemoryInBytesPerformanceCounter = new PerformanceCounter("Memory", "Committed Bytes");
        }

        public SystemMemoryInformation GetMemoryStatus()
        {
            var usedMemoryInBytes = this.usedMemoryInBytesPerformanceCounter.NextValue();
            var availableMemoryInMegabytes = this.freeMemoryInMegaBytesPerformanceCounter.NextValue();

            return new SystemMemoryInformation
                {
                    UsedMemoryInGB = this.memoryUnitConverter.ConvertBytesToGigabyte(usedMemoryInBytes),
                    AvailableMemoryInGB = this.memoryUnitConverter.ConvertMegabyteToGigabyte(availableMemoryInMegabytes)
                };
        }

        public void Dispose()
        {
            try
            {
                if (this.usedMemoryInBytesPerformanceCounter != null)
                {
                    this.usedMemoryInBytesPerformanceCounter.Dispose();
                }

                if (this.freeMemoryInMegaBytesPerformanceCounter != null)
                {
                    this.freeMemoryInMegaBytesPerformanceCounter.Dispose();
                }
            }
            finally
            {
                PerformanceCounter.CloseSharedResources();
            }
        }
    }
}