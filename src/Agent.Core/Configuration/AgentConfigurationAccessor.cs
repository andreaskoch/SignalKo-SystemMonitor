using System;

using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class AgentConfigurationAccessor : IAgentConfigurationAccessor
    {
        private readonly IAgentConfigurationServiceUrlProvider configurationServiceUrlProvider;

        private readonly IRESTClientFactory restClientFactory;

        private readonly IRESTRequestFactory requestFactory;

        public AgentConfigurationAccessor(IAgentConfigurationServiceUrlProvider configurationServiceUrlProvider, IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory)
        {
            if (configurationServiceUrlProvider == null)
            {
                throw new ArgumentNullException("configurationServiceUrlProvider");
            }

            if (restClientFactory == null)
            {
                throw new ArgumentNullException("restClientFactory");
            }

            if (requestFactory == null)
            {
                throw new ArgumentNullException("requestFactory");
            }

            this.configurationServiceUrlProvider = configurationServiceUrlProvider;
            this.restClientFactory = restClientFactory;
            this.requestFactory = requestFactory;            
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            var serviceUrl = new Uri(this.configurationServiceUrlProvider.GetServiceUrl());
            string baseUrl = serviceUrl.Scheme + "://" + serviceUrl.Host + ":" + serviceUrl.Port;
            string resourcePath = serviceUrl.PathAndQuery;

            var restClient = this.restClientFactory.GetRESTClient(baseUrl);
            var request = this.requestFactory.CreateGetRequest(resourcePath);

            var response = restClient.Execute<AgentConfiguration>(request);
            return response.Data;
        }
    }
}