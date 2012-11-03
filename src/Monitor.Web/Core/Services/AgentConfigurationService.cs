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

			// add configurations for known agents
			var newAgentInstanceConfigurationList = agentConfiguration.AgentInstanceConfigurations.ToList();

			foreach (string nameOfKnownAgent in this.knownAgentsProvider.GetKnownAgents())
			{
				if (agentConfiguration.AgentInstanceConfigurations.Any(c => c.MachineName.Equals(nameOfKnownAgent)) == false)
				{
					newAgentInstanceConfigurationList.Add(new AgentInstanceConfiguration { MachineName = nameOfKnownAgent, AgentIsEnabled = false });
				}
			}

			agentConfiguration.AgentInstanceConfigurations = newAgentInstanceConfigurationList.ToArray();

			return agentConfiguration;
		}

		public void SaveAgentConfiguration(AgentConfiguration agentConfiguration)
		{
			this.agentConfigurationDataAccessor.Store(agentConfiguration);
		}
	}
}