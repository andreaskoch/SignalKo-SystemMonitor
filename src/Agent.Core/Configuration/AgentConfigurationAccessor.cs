using System;

using Newtonsoft.Json;

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
			var serviceConfiguration = this.configurationServiceUrlProvider.GetServiceConfiguration();

			var restClient = this.restClientFactory.GetRESTClient(serviceConfiguration.Hostaddress);
			var request = this.requestFactory.CreateGetRequest(serviceConfiguration.ResourcePath, serviceConfiguration.Hostname);

			var response = restClient.Execute<AgentConfiguration>(request);
			string json = response.Content;
			try
			{
				var deserialized = JsonConvert.DeserializeObject<AgentConfiguration>(json);
				return deserialized;
			}
			catch (JsonSerializationException exception)
			{
				Console.WriteLine(exception.StackTrace);
			}

			return null;
		}
	}
}