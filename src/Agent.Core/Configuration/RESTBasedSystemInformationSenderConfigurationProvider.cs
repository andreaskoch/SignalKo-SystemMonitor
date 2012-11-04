using System;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public class RESTBasedSystemInformationSenderConfigurationProvider : IRESTBasedSystemInformationSenderConfigurationProvider
	{
		private readonly IAgentControlDefinitionProvider agentControlDefinitionProvider;

		public RESTBasedSystemInformationSenderConfigurationProvider(IAgentControlDefinitionProvider agentControlDefinitionProvider)
		{
			if (agentControlDefinitionProvider == null)
			{
				throw new ArgumentNullException("agentControlDefinitionProvider");
			}

			this.agentControlDefinitionProvider = agentControlDefinitionProvider;
		}

		public IRESTServiceConfiguration GetConfiguration()
		{
			var agentControlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
			if (agentControlDefinition == null)
			{
				return null;
			}

			return new RESTServiceConfiguration
				{
					Hostaddress = agentControlDefinition.Hostaddress,
					Hostname = agentControlDefinition.Hostname,
					ResourcePath = agentControlDefinition.SystemInformationSenderPath
				};
		}
	}
}