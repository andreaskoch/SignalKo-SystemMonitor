using SignalKo.SystemMonitor.Common.Model.Configuration;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public interface IServerConfigurationRepository
    {
        MachineGroupConfiguration LoadConfiguration();

        void SaveConfiguration(MachineGroupConfiguration configuration);
    }
}