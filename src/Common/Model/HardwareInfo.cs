namespace SignalKo.SystemMonitor.Common.Model
{
    public class HardwareInfo
    {
        public string MachineName { get; set; }

        public double Processor { get; set; }

        public ulong MemUsage { get; set; }

        public ulong TotalMemory { get; set; }
    }
}