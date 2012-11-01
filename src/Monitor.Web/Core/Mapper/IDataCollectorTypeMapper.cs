using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public interface IDataCollectorTypeMapper
	{
		DataCollectorType Map(string collectorType);
	}
}