using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collector
{
    public interface ISystemMemoryStatusProvider
    {
        SystemMemoryInformation GetMemoryStatus();
    }
}