using System;
using System.Collections.Generic;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Controllers;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
	public class GroupConfigurationViewModelOrchestrator : IGroupConfigurationViewModelOrchestrator
	{
		public GroupConfigurationViewModel GetGroupConfigurationViewModel(GroupConfiguration groupConfiguration, AgentInstanceConfiguration[] agentInstanceConfigurations)
		{
			if (groupConfiguration == null)
			{
				return new GroupConfigurationViewModel();
			}

			var agentGroups = groupConfiguration.Groups.Select(g => new GroupOfAgents { Name = g.Name }).ToList();

			foreach (var agentInstanceConfiguration in agentInstanceConfigurations.Where(agentInstance => agentInstance.GroupNames != null && agentInstance.GroupNames.Any()))
			{
				foreach (var groupName in agentInstanceConfiguration.GroupNames)
				{
					if (!agentGroups.Any(agentGroup => agentGroup.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
					{
						agentGroups.Add(new GroupOfAgents { Name = groupName, Agents = new List<string> { agentInstanceConfiguration.MachineName } });
					}
					else
					{
						agentGroups.First(agentGroup => agentGroup.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase)).Agents.Add(agentInstanceConfiguration.MachineName);
					}
				}
			}

			var unassignedAgents =
				agentInstanceConfigurations.Where(agentInstance => agentInstance.GroupNames == null || !agentInstance.GroupNames.Any()).Select(
					agentInstance => agentInstance.MachineName).ToList();

			return new GroupConfigurationViewModel { Groups = agentGroups, UnassignedAgents = unassignedAgents };
		}
	}
}