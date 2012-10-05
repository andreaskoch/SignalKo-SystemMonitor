namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
{
    public interface ISystemInformationProvider
    {
        Common.Model.SystemInformation GetSystemInfo();
    }
}