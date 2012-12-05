using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
	public partial class UIConfigurationController : Controller
	{
		private readonly IGroupConfigurationService groupConfigurationService;

		private readonly IAgentConfigurationService agentConfigurationService;

		private readonly IGroupConfigurationViewModelOrchestrator groupConfigurationViewModelOrchestrator;

		public UIConfigurationController(IGroupConfigurationService groupConfigurationService, IAgentConfigurationService agentConfigurationService, IGroupConfigurationViewModelOrchestrator groupConfigurationViewModelOrchestrator)
		{
			if (groupConfigurationService == null)
			{
				throw new ArgumentNullException("groupConfigurationService");
			}
			if (agentConfigurationService == null)
			{
				throw new ArgumentNullException("agentConfigurationService");
			}

			if (groupConfigurationViewModelOrchestrator == null)
			{
				throw new ArgumentNullException("groupConfigurationViewModelOrchestrator");
			}

			this.groupConfigurationService = groupConfigurationService;
			this.agentConfigurationService = agentConfigurationService;
			this.groupConfigurationViewModelOrchestrator = groupConfigurationViewModelOrchestrator;
		}

		public virtual ActionResult Editor()
		{
			return View();
		}

		[HttpGet]
		public virtual JsonResult Load()
		{
			var groupConfiguration = this.groupConfigurationService.GetGroupConfiguration();
			var agentConfiguration = this.agentConfigurationService.GetAgentConfiguration();

			GroupConfigurationViewModel viewModel = this.groupConfigurationViewModelOrchestrator.GetGroupConfigurationViewModel(groupConfiguration, agentConfiguration.AgentInstanceConfigurations.ToArray());
			return this.Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public virtual ActionResult Save(GroupConfiguration groupConfiguration)
		{
			if (groupConfiguration == null)
			{
				return new HttpStatusCodeResult(400);
			}

			this.groupConfigurationService.SaveGroupConfiguration(groupConfiguration);
			return new ContentResult { Content = "" };
		}
	}

	public interface IGroupConfigurationService
	{
		GroupConfiguration GetGroupConfiguration();

		void SaveGroupConfiguration(GroupConfiguration groupConfiguration);
	}

	public class GroupConfigurationService : IGroupConfigurationService
	{
		private readonly IConfigurationDataAccessor<GroupConfiguration> groupConfigurationAccessor;

		public GroupConfigurationService(IConfigurationDataAccessor<GroupConfiguration> groupConfigurationAccessor)
		{
			if (groupConfigurationAccessor == null)
			{
				throw new ArgumentNullException("groupConfigurationAccessor");
			}

			this.groupConfigurationAccessor = groupConfigurationAccessor;
		}

		public GroupConfiguration GetGroupConfiguration()
		{
			return this.groupConfigurationAccessor.Load() ?? new GroupConfiguration();
		}

		public void SaveGroupConfiguration(GroupConfiguration groupConfiguration)
		{
			this.groupConfigurationAccessor.Store(groupConfiguration);
		}
	}

	public interface IGroupConfigurationViewModelOrchestrator
	{
		GroupConfigurationViewModel GetGroupConfigurationViewModel(GroupConfiguration groupConfiguration, AgentInstanceConfiguration[] agentInstanceConfigurations);
	}

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

	public class GroupConfigurationViewModel
	{
		public GroupConfigurationViewModel()
		{
			this.Groups = new List<GroupOfAgents>();
			this.UnassignedAgents = new List<string>();
		}

		public List<GroupOfAgents> Groups { get; set; }

		public List<string> UnassignedAgents { get; set; }
	}

	public class GroupConfiguration : IEnumerable<Group>
	{
		public GroupConfiguration()
		{
			this.Groups = new List<Group>();
		}

		public List<Group> Groups { get; set; }

		public IEnumerator<Group> GetEnumerator()
		{
			return this.Groups.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	public class Group
	{
		public string Name { get; set; }
	}

	public class GroupOfAgents : Group
	{
		public List<string> Agents { get; set; }
	}
}
