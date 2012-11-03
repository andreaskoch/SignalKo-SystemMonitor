namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance
{
    public interface ISystemInformationProvider
    {
        Common.Model.SystemInformation GetSystemInfo();
    }
}