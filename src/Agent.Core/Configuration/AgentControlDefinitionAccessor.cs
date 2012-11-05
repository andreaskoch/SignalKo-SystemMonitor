using System;

using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public class AgentControlDefinitionAccessor : IAgentControlDefinitionAccessor
	{
		private readonly IAgentControlDefinitionServiceUrlProvider controlDefinitionServiceUrlProvider;

		private readonly IMachineNameProvider machineNameProvider;

		private readonly IRESTClientFactory restClientFactory;

		private readonly IRESTRequestFactory requestFactory;

		public AgentControlDefinitionAccessor(IAgentControlDefinitionServiceUrlProvider controlDefinitionServiceUrlProvider, IMachineNameProvider machineNameProvider,  IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory)
		{
			if (controlDefinitionServiceUrlProvider == null)
			{
				throw new ArgumentNullException("controlDefinitionServiceUrlProvider");
			}

			if (machineNameProvider == null)
			{
				throw new ArgumentNullException("machineNameProvider");
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
			this.machineNameProvider = machineNameProvider;
			this.restClientFactory = restClientFactory;
			this.requestFactory = requestFactory;
		}

		public AgentControlDefinition GetControlDefinition()
		{
			var serviceConfiguration = this.controlDefinitionServiceUrlProvider.GetServiceConfiguration();

			var restClient = this.restClientFactory.GetRESTClient(serviceConfiguration.Hostaddress);

			string machineName = this.machineNameProvider.GetMachineName();
			string resourcePath = serviceConfiguration.ResourcePath;

			var request = this.requestFactory.CreateGetRequest(resourcePath, serviceConfiguration.Hostname);
			request.AddParameter("machineName", machineName);

			var response = restClient.Execute<AgentControlDefinition>(request);
			return response.Data;
		}
	}
}