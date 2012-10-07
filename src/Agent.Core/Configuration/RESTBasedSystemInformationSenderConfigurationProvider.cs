using System;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class RESTBasedSystemInformationSenderConfigurationProvider : IRESTBasedSystemInformationSenderConfigurationProvider
    {
        private readonly IAgentConfigurationProvider agentConfigurationProvider;

        public RESTBasedSystemInformationSenderConfigurationProvider(IAgentConfigurationProvider agentConfigurationProvider)
        {
            if (agentConfigurationProvider == null)
            {
                throw new ArgumentNullException("agentConfigurationProvider");
            }

            this.agentConfigurationProvider = agentConfigurationProvider;
        }

        public IRESTServiceConfiguration GetConfiguration()
        {
            var agentConfiguration = this.agentConfigurationProvider.GetAgentConfiguration();
            if (agentConfiguration == null)
            {
                return null;
            }

            return new RESTServiceConfiguration
                {
                    Hostaddress = agentConfiguration.Hostaddress,
                    Hostname = agentConfiguration.Hostname,
                    ResourcePath = agentConfiguration.SystemInformationSenderPath
                };
        }
    }
}