using System;

namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public class RESTBasedSystemInformationSenderConfigurationProvider : IRESTBasedSystemInformationSenderConfigurationProvider
    {
        private readonly IAgentConfigurationProvider agentConfigurationProvider;

        public RESTBasedSystemInformationSenderConfigurationProvider(IAgentConfigurationProvider agentConfigurationProvider)
        {
            this.agentConfigurationProvider = agentConfigurationProvider;
        }

        public IRESTServiceConfiguration GetConfiguration()
        {
            var agentConfiguration = this.agentConfigurationProvider.GetAgentConfiguration();

            var url = new Uri(agentConfiguration.SystemInformationSenderUrl);
            var baseUrl = url.Scheme + url.Host + url.Port;
            var resourcePath = url.AbsolutePath;

            return new RESTServiceConfiguration { BaseUrl = baseUrl, ResourcePath = resourcePath };
        }
    }
}