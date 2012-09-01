namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemInformation
    {
        public DataCollectionTimeFrame TimeFrame { get; set; }

        public string MachineName { get; set; }

        public ProcessorUtilizationInformation ProcessorStatus { get; set; }

        public SystemMemoryInformation MemoryStatus { get; set; }

        public SystemStorageInformation StorageStatus { get; set; }
    }
}