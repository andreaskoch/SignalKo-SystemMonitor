namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public interface IRESTServiceConfigurationProvider
    {
        IRESTServiceConfiguration GetConfiguration();
    }
}