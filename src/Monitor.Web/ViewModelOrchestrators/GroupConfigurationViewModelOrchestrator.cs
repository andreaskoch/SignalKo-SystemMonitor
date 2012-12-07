using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
	public class GroupConfigurationViewModelOrchestrator : IGroupConfigurationViewModelOrchestrator
	{
		private readonly IAgentConfigurationService agentConfigurationService;

		public GroupConfigurationViewModelOrchestrator(IAgentConfigurationService agentConfigurationService)
		{
			if (agentConfigurationService == null)
			{
				throw new ArgumentNullException("agentConfigurationService");
			}

			this.agentConfigurationService = agentConfigurationService;
		}

		public GroupConfigurationViewModel GetGroupConfigurationViewModel(GroupConfiguration groupConfiguration)
		{
			if (groupConfiguration == null)
			{
				return new GroupConfigurationViewModel();
			}

			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();
			var agents = agentConfiguration.AgentInstanceConfigurations.Select(c => c.MachineName);

			var agentGroups = groupConfiguration.Groups ?? new GroupOfAgents[] { };

			return new GroupConfigurationViewModel { Groups = agentGroups.ToArray(), KnownAgents = agents.ToArray() };
		}

		public GroupConfiguration GetGroupConfiguration(GroupConfigurationViewModel groupConfigurationViewModel)
		{
			var groups = groupConfigurationViewModel.Groups.Select(g => new GroupOfAgents { Name = g.Name, Agents = g.Agents });
			return new GroupConfiguration { Groups = groups.ToArray() };
		}
	}
}