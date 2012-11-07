namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public interface IRESTBasedSystemInformationSenderConfigurationProvider
	{
		IRESTServiceConfiguration GetConfiguration();
	}
}