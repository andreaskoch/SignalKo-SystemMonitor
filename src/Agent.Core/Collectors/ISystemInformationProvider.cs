namespace SignalKo.SystemMonitor.Agent.Core.Collectors
{
    public interface ISystemInformationProvider
    {
        Common.Model.SystemInformation GetSystemInfo();
    }
}