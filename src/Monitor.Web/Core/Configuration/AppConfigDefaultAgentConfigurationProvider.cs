using System.Configuration;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public class AppConfigDefaultAgentConfigurationProvider : IDefaultAgentConfigurationProvider
    {
        private const string AppSettingsKeyDefaultAgentConfigurationHostaddress = "DefaultAgentConfigurationHostaddress";

        private const string AppSettingsKeyDefaultAgentConfigurationHostname = "DefaultAgentConfigurationHostname";

        private const string AppSettingsKeyDefaultAgentConfigurationSystemInformationSenderPath = "DefaultAgentConfigurationSystemInformationSenderPath";

        private const string AppSettingsKeyDefaultAgentConfigurationCheckIntervalInSeconds = "DefaultAgentConfigurationCheckIntervalInSeconds";

        private const string AppSettingsKeyDefaultAgentConfigurationAgentsAreEnabled = "DefaultAgentConfigurationAgentsAreEnabled";

        public AgentConfiguration GetDefaultAgentConfiguration()
        {
            var agentsAreEnabled = bool.Parse(ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationAgentsAreEnabled]);
            var checkIntervalInSeconds = int.Parse(ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationCheckIntervalInSeconds]);
            var hostaddress = ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationHostaddress];
            var hostname = ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationHostname];
            var systemInformationSenderPath = ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationSystemInformationSenderPath];

            return new AgentConfiguration
                {
                    AgentsAreEnabled = agentsAreEnabled,
                    CheckIntervalInSeconds = checkIntervalInSeconds,
                    Hostaddress = hostaddress,
                    Hostname = hostname,
                    SystemInformationSenderPath = systemInformationSenderPath
                };
        }
    }
}