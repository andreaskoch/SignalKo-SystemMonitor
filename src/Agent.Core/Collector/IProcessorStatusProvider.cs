using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collector
{
    public interface IProcessorStatusProvider
    {
        ProcessorUtilizationInformation GetProcessorStatus();
    }
}