using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModels
{
	public class GroupConfigurationViewModel
	{
		public GroupOfAgents[] Groups { get; set; }

		public string[] KnownAgents { get; set; }
	}
}