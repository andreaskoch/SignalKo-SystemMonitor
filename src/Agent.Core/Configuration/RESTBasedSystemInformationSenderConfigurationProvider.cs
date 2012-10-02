namespace SignalKo.SystemMonitor.Agent.Core.Configuration
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
            return new RESTServiceConfiguration { BaseUrl = agentConfiguration.BaseUrl, ResourcePath = agentConfiguration.SystemInformationSenderPath };
        }
    }
}