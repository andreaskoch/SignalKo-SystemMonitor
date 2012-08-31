using System;

namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemInformation
    {
        public DateTimeOffset Timestamp { get; set; }

        public string MachineName { get; set; }

        public double ProcessorUtilizationInPercent { get; set; }

        public SystemMemoryInformation MemoryStatus { get; set; }

        public SystemStorageInformation StorageStatus { get; set; }
    }
}