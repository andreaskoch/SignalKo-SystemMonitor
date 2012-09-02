namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public interface IRESTServiceConfigurationProvider
    {
        RESTServiceConfiguration GetConfiguration();
    }
}