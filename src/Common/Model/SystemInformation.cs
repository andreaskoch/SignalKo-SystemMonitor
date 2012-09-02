using System;

namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemInformation
    {
        public DateTimeOffset Timestamp { get; set; }

        public string MachineName { get; set; }

        public ProcessorUtilizationInformation ProcessorStatus { get; set; }

        public SystemMemoryInformation MemoryStatus { get; set; }

        public SystemStorageInformation StorageStatus { get; set; }

        public override string ToString()
        {
            return string.Format(
                "SystemInformation (Timestamp: {0}, MachineName: {1}, ProcessorStatus: {2}, MemoryStatus: {3}, StorageStatus: {4})",
                this.Timestamp.ToString(),
                this.MachineName,
                this.ProcessorStatus,
                this.MemoryStatus,
                this.StorageStatus);
        }

        public override int GetHashCode()
        {
            int hash = 37;
            hash = (hash * 23) + this.ToString().GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var otherObj = obj as SystemInformation;
            if (otherObj != null)
            {
                return this.ToString().Equals(otherObj.ToString());
            }

            return false;
        }
    }
}