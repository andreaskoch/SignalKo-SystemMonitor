using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public class AgentConfigurationProvider : IAgentConfigurationProvider
    {
        private readonly IAgentConfigurationServiceUrlProvider serviceUrlConfigurationServiceUrlProvider;

        public AgentConfigurationProvider(IAgentConfigurationServiceUrlProvider serviceUrlConfigurationServiceUrlProvider)
        {
            this.serviceUrlConfigurationServiceUrlProvider = serviceUrlConfigurationServiceUrlProvider;
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}