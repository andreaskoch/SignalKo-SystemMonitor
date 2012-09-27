using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public interface IDefaultAgentConfigurationProvider
    {
        AgentConfiguration GetDefaultAgentConfiguration();
    }
}