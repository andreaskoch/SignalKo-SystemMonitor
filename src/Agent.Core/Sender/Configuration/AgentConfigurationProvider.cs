using System;
using System.Threading;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public class AgentConfigurationProvider : IAgentConfigurationProvider, IDisposable
    {
        private const int DefaultCheckIntervalInSeconds = 60;

        private readonly IAgentConfigurationServiceUrlProvider configurationServiceUrlProvider;

        private readonly IRESTClientFactory restClientFactory;

        private readonly IRESTRequestFactory requestFactory;

        private readonly Timer timer;

        private AgentConfiguration agentConfiguration;

        public AgentConfigurationProvider(IAgentConfigurationServiceUrlProvider configurationServiceUrlProvider, IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory)
        {
            this.configurationServiceUrlProvider = configurationServiceUrlProvider;
            this.restClientFactory = restClientFactory;
            this.requestFactory = requestFactory;

            var timerStartTime = new TimeSpan(0, 0, 0);
            var timerInterval = new TimeSpan(0, 0, 0, DefaultCheckIntervalInSeconds);
            this.timer = new Timer(state => this.UpdateAgentConfiguration(), null, timerStartTime, timerInterval);
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            return this.agentConfiguration;
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }

        private void UpdateAgentConfiguration()
        {
            var serviceUrl = new Uri(this.configurationServiceUrlProvider.GetServiceUrl());
            string baseUrl = serviceUrl.Scheme + "://" + serviceUrl.Host + ":" + serviceUrl.Port;
            string resourcePath = serviceUrl.PathAndQuery;

            var restClient = this.restClientFactory.GetRESTClient(baseUrl);
            var request = this.requestFactory.CreateGetRequest(resourcePath);

            var response = restClient.Execute<AgentConfiguration>(request);
            this.agentConfiguration = response.Data;

            // update the check interval
            if (this.agentConfiguration != null && this.agentConfiguration.CheckIntervalInSeconds > 0)
            {
                var timerStartTime = new TimeSpan(0, 0, this.agentConfiguration.CheckIntervalInSeconds);
                var timerInterval = new TimeSpan(0, 0, 0, this.agentConfiguration.CheckIntervalInSeconds);
                this.timer.Change(timerStartTime, timerInterval);
            }
        }
    }
}