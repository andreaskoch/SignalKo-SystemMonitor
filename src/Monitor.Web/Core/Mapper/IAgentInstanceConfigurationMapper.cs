using SignalKo.SystemMonitor.Common.Dto;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public interface IAgentInstanceConfigurationMapper
	{
		AgentInstanceConfiguration Map(AgentInstanceConfigurationDto dto);
	}
}