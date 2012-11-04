using System;

using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public class AgentControlDefinitionAccessor : IAgentControlDefinitionAccessor
	{
		private readonly IAgentControlDefinitionServiceUrlProvider controlDefinitionServiceUrlProvider;

		private readonly IRESTClientFactory restClientFactory;

		private readonly IRESTRequestFactory requestFactory;

		public AgentControlDefinitionAccessor(IAgentControlDefinitionServiceUrlProvider controlDefinitionServiceUrlProvider, IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory)
		{
			if (controlDefinitionServiceUrlProvider == null)
			{
				throw new ArgumentNullException("controlDefinitionServiceUrlProvider");
			}

			if (restClientFactory == null)
			{
				throw new ArgumentNullException("restClientFactory");
			}

			if (requestFactory == null)
			{
				throw new ArgumentNullException("requestFactory");
			}

			this.controlDefinitionServiceUrlProvider = controlDefinitionServiceUrlProvider;
			this.restClientFactory = restClientFactory;
			this.requestFactory = requestFactory;
		}

		public AgentControlDefinition GetControlDefinition()
		{
			var serviceConfiguration = this.controlDefinitionServiceUrlProvider.GetServiceConfiguration();

			var restClient = this.restClientFactory.GetRESTClient(serviceConfiguration.Hostaddress);
			var request = this.requestFactory.CreateGetRequest(serviceConfiguration.ResourcePath, serviceConfiguration.Hostname);

			var response = restClient.Execute<AgentControlDefinition>(request);
			return response.Data;
		}
	}
}