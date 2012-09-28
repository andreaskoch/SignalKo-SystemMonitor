using System.Configuration;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public class AppConfigDefaultAgentConfigurationProvider : IDefaultAgentConfigurationProvider
    {
        private const string AppSettingsKeyDefaultAgentConfigurationBaseUrl = "DefaultAgentConfigurationBaseUrl";

        private const string AppSettingsKeyDefaultAgentConfigurationSystemInformationSenderPath = "DefaultAgentConfigurationSystemInformationSenderPath";

        private const string AppSettingsKeyDefaultAgentConfigurationCheckIntervalInSeconds = "DefaultAgentConfigurationCheckIntervalInSeconds";

        private const string AppSettingsKeyDefaultAgentConfigurationAgentsAreEnabled = "DefaultAgentConfigurationAgentsAreEnabled";

        public AgentConfiguration GetDefaultAgentConfiguration()
        {
            var agentsAreEnabled = bool.Parse(ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationAgentsAreEnabled]);
            var checkIntervalInSeconds = int.Parse(ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationCheckIntervalInSeconds]);
            var baseUrl = ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationBaseUrl];
            var systemInformationSenderPath = ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationSystemInformationSenderPath];

            return new AgentConfiguration
                {
                    AgentsAreEnabled = agentsAreEnabled,
                    CheckIntervalInSeconds = checkIntervalInSeconds, 
                    BaseUrl = baseUrl,
                    SystemInformationSenderPath = systemInformationSenderPath
                };
        }
    }
}