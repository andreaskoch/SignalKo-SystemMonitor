using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
	public interface IGroupConfigurationViewModelOrchestrator
	{
		GroupConfigurationViewModel GetGroupConfigurationViewModel(GroupConfiguration groupConfiguration);

		GroupConfiguration GetGroupConfiguration(GroupConfigurationViewModel groupConfigurationViewModel);
	}
}