using System.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class AppConfigAgentConfigurationServiceUrlProvider : IAgentConfigurationServiceUrlProvider
    {
        private const string AppSettingsKeyAgentConfigurationServiceHostaddress = "AgentConfigurationServiceHostaddress";

        private const string AppSettingsKeyAgentConfigurationServiceHostname = "AgentConfigurationServiceHostname";

        private const string AppSettingsKeyAgentConfigurationServiceResourcePath = "AgentConfigurationServiceResourcePath";

        public AgentConfigurationServiceConfiguration GetServiceConfiguration()
        {
            string hostaddress = ConfigurationManager.AppSettings[AppSettingsKeyAgentConfigurationServiceHostaddress];
            string hostname = ConfigurationManager.AppSettings[AppSettingsKeyAgentConfigurationServiceHostname];
            string resourcePath = ConfigurationManager.AppSettings[AppSettingsKeyAgentConfigurationServiceResourcePath];

            return new AgentConfigurationServiceConfiguration { Hostaddress = hostaddress, Hostname = hostname, ResourcePath = resourcePath };
        }
    }
}