using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Services
{
    public interface ISystemInformationProvider
    {
        SystemInformation GetSystemInfo();
    }
}