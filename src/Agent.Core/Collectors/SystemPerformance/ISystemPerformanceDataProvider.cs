using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance
{
	public interface ISystemPerformanceDataProvider
	{
		SystemPerformanceData GetSystemPerformanceData();
	}
}