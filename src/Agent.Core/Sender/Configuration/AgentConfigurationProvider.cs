using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public class AgentConfigurationProvider : IAgentConfigurationProvider
    {
        private readonly IAgentConfigurationServiceUrlProvider configurationServiceUrlProvider;

        private readonly IRESTClientFactory restClientFactory;

        private readonly IRESTRequestFactory requestFactory;

        public AgentConfigurationProvider(IAgentConfigurationServiceUrlProvider configurationServiceUrlProvider, IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory)
        {
            this.configurationServiceUrlProvider = configurationServiceUrlProvider;
            this.restClientFactory = restClientFactory;
            this.requestFactory = requestFactory;
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            var serviceUrl = new Uri(this.configurationServiceUrlProvider.GetServiceUrl());
            string baseUrl = serviceUrl.Scheme + "://" + serviceUrl.Host + serviceUrl.Port;
            string resourcePath = serviceUrl.PathAndQuery;

            var restClient = this.restClientFactory.GetRESTClient(baseUrl);
            var request = this.requestFactory.CreateGetRequest(resourcePath);

            var response = restClient.Execute<AgentConfiguration>(request);
            var agentConfiguration = response.Data;

            return agentConfiguration;
        }
    }
}