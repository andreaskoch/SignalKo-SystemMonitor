using System.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public class AppConfigAgentControlDefinitionServiceUrlProvider : IAgentControlDefinitionServiceUrlProvider
	{
		private const string AppSettingsKeyAgentControlDefinitionServiceHostaddress = "AgentControlDefinitionServiceHostaddress";

		private const string AppSettingsKeyAgentControlDefinitionServiceHostname = "AgentControlDefinitionServiceHostname";

		private const string AppSettingsKeyAgentControlDefinitionServiceResourcePath = "AgentControlDefinitionServiceResourcePath";

		public AgentControlDefinitionServiceConfiguration GetServiceConfiguration()
		{
			string hostaddress = ConfigurationManager.AppSettings[AppSettingsKeyAgentControlDefinitionServiceHostaddress];
			string hostname = ConfigurationManager.AppSettings[AppSettingsKeyAgentControlDefinitionServiceHostname];
			string resourcePath = ConfigurationManager.AppSettings[AppSettingsKeyAgentControlDefinitionServiceResourcePath];

			return new AgentControlDefinitionServiceConfiguration { Hostaddress = hostaddress, Hostname = hostname, ResourcePath = resourcePath };
		}
	}
}