namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemStorageDeviceInformation
    {
        public string DeviceName { get; set; }

        public double TotalStorageInGB { get; set; }

        public double UsedStorageInGB { get; set; }
    }
}