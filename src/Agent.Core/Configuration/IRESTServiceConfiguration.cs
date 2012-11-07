namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public interface IRESTServiceConfiguration
	{
		string Hostaddress { get; set; }

		string Hostname { get; set; }

		string ResourcePath { get; set; }

		bool IsValid();
	}
}