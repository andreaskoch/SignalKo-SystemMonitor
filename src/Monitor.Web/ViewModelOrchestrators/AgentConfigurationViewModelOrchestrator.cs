using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
	public class AgentConfigurationViewModelOrchestrator : IAgentConfigurationViewModelOrchestrator
	{
		private readonly IKnownAgentsProvider knownAgentsProvider;

		public AgentConfigurationViewModelOrchestrator(IKnownAgentsProvider knownAgentsProvider)
		{
			if (knownAgentsProvider == null)
			{
				throw new ArgumentNullException("knownAgentsProvider");
			}

			this.knownAgentsProvider = knownAgentsProvider;
		}

		public AgentConfigurationViewModel GetAgentConfigurationViewModel(AgentConfiguration agentConfiguration)
		{
			if (agentConfiguration == null)
			{
				throw new ArgumentNullException("agentConfiguration");
			}

			var unconfiguredAgents =
				this.knownAgentsProvider.GetKnownAgents().Where(
					agentName => agentConfiguration.AgentInstanceConfigurations.Select(a => a.MachineName).Any(b => b.Equals(agentName)) == false).ToArray();

			return new AgentConfigurationViewModel
				{
					UnconfiguredAgents = unconfiguredAgents,
					Configuration = agentConfiguration
				};
		}
	}
}