using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
	public class AgentConfigurationService : IAgentConfigurationService
	{
		private readonly IAgentConfigurationDataAccessor agentConfigurationDataAccessor;

		private readonly IDefaultAgentConfigurationProvider defaultAgentConfigurationProvider;

		private readonly IKnownAgentsProvider knownAgentsProvider;

		public AgentConfigurationService(IAgentConfigurationDataAccessor agentConfigurationDataAccessor, IDefaultAgentConfigurationProvider defaultAgentConfigurationProvider, IKnownAgentsProvider knownAgentsProvider)
		{
			if (agentConfigurationDataAccessor == null)
			{
				throw new ArgumentNullException("agentConfigurationDataAccessor");
			}

			if (defaultAgentConfigurationProvider == null)
			{
				throw new ArgumentNullException("defaultAgentConfigurationProvider");
			}

			if (knownAgentsProvider == null)
			{
				throw new ArgumentNullException("knownAgentsProvider");
			}

			this.agentConfigurationDataAccessor = agentConfigurationDataAccessor;
			this.defaultAgentConfigurationProvider = defaultAgentConfigurationProvider;
			this.knownAgentsProvider = knownAgentsProvider;
		}

		public AgentConfiguration GetAgentConfiguration()
		{
			var agentConfiguration = this.agentConfigurationDataAccessor.Load();
			agentConfiguration = agentConfiguration ?? this.defaultAgentConfigurationProvider.GetDefaultAgentConfiguration();

			return agentConfiguration;
		}

		public string[] GetNamesOfUnconfiguredAgents()
		{
			var agentConfiguration = this.agentConfigurationDataAccessor.Load();
			agentConfiguration = agentConfiguration ?? this.defaultAgentConfigurationProvider.GetDefaultAgentConfiguration();

			var agentsWithConfigurations = agentConfiguration.AgentInstanceConfigurations.Select(agent => agent.MachineName);
			var allKnownAgents = this.knownAgentsProvider.GetKnownAgents();

			return allKnownAgents.Where(agent => agentsWithConfigurations.Any(a => a.Equals(agent, StringComparison.OrdinalIgnoreCase)) == false).ToArray();
		}

		public void SaveAgentConfiguration(AgentConfiguration agentConfiguration)
		{
			this.agentConfigurationDataAccessor.Store(agentConfiguration);
		}
	}
}