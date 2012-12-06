using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Controllers;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
	public interface IGroupConfigurationService
	{
		GroupConfiguration GetGroupConfiguration();

		void SaveGroupConfiguration(GroupConfiguration groupConfiguration);
	}
}