using System.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public class AppSettingsRESTServiceConfigurationProvider : IRESTServiceConfigurationProvider
    {
        private const string AppSettingsKeyRESTSystemInformationSenderBaseUrl = "RESTSystemInformationSenderBaseUrl";

        private const string AppSettingsKeyRESTSystemInformationSenderResourcePath = "RESTSystemInformationSenderResourcePath";

        public IRESTServiceConfiguration GetConfiguration()
        {
            string baseUrl = ConfigurationManager.AppSettings[AppSettingsKeyRESTSystemInformationSenderBaseUrl];
            string resourcePath = ConfigurationManager.AppSettings[AppSettingsKeyRESTSystemInformationSenderResourcePath];

            return new RESTServiceConfiguration { BaseUrl = baseUrl, ResourcePath = resourcePath };
        }
    }
}