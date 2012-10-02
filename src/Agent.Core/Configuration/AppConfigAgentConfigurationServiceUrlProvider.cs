using System.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class AppConfigAgentConfigurationServiceUrlProvider : IAgentConfigurationServiceUrlProvider
    {
        private const string AppSettingsKeyRESTAgentConfigurationProviderUrl = "RESTAgentConfigurationProviderUrl";

        public string GetServiceUrl()
        {
            return ConfigurationManager.AppSettings[AppSettingsKeyRESTAgentConfigurationProviderUrl];
        }
    }
}