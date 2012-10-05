using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
{
    public interface IProcessorStatusProvider
    {
        ProcessorUtilizationInformation GetProcessorStatus();
    }
}