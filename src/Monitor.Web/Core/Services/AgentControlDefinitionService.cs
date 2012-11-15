using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
	public class AgentControlDefinitionService : IAgentControlDefinitionService
	{
		private readonly IAgentConfigurationService agentConfigurationService;

		public AgentControlDefinitionService(IAgentConfigurationService agentConfigurationService)
		{
			if (agentConfigurationService == null)
			{
				throw new ArgumentNullException("agentConfigurationService");
			}

			this.agentConfigurationService = agentConfigurationService;
		}

		public AgentControlDefinition GetAgentControlDefinition(string machineName)
		{
			if (string.IsNullOrWhiteSpace(machineName))
			{
				throw new ArgumentException("machineName");
			}

			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();

			// check if there is a specific configuration for the agent with the supplied machine name
			var agentInstanceConfiguration =
				agentConfiguration.AgentInstanceConfigurations.FirstOrDefault(config => config.MachineName.Equals(machineName, StringComparison.OrdinalIgnoreCase));

			// no agent instance configuration available
			if (agentInstanceConfiguration == null)
			{
				return new AgentControlDefinition
					{
						AgentIsEnabled = agentConfiguration.AgentsAreEnabled,
						CheckIntervalInSeconds = agentConfiguration.CheckIntervalInSeconds,
						Hostaddress = agentConfiguration.Hostaddress,
						Hostname = agentConfiguration.Hostname,
						SystemInformationSenderPath = agentConfiguration.SystemInformationSenderPath
					};
			}

			// agent instance configuration must be considered
			var agentIsEnabled = agentConfiguration.AgentsAreEnabled && agentInstanceConfiguration.AgentIsEnabled;
			return new AgentControlDefinition
				{
					AgentIsEnabled = agentIsEnabled,
					CheckIntervalInSeconds = agentConfiguration.CheckIntervalInSeconds,
					Hostaddress = agentConfiguration.Hostaddress,
					Hostname = agentConfiguration.Hostname,
					SystemInformationSenderPath = agentConfiguration.SystemInformationSenderPath,
					SystemPerformanceCheck = agentInstanceConfiguration.SystemPerformanceCollector,
					HttpStatusCodeCheck = agentInstanceConfiguration.HttpStatusCodeCheck
				};
		}
	}
}