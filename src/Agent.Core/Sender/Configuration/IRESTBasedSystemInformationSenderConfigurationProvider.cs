namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public interface IRESTBasedSystemInformationSenderConfigurationProvider
    {
        IRESTServiceConfiguration GetConfiguration();
    }
}