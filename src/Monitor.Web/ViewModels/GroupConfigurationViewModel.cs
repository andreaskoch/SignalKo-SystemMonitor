using System.Collections.Generic;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Controllers;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModels
{
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
}