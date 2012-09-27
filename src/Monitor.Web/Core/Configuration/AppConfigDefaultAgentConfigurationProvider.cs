using System.Configuration;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public class AppConfigDefaultAgentConfigurationProvider : IDefaultAgentConfigurationProvider
    {
        private const string AppSettingsKeyDefaultAgentConfigurationUrl = "DefaultAgentConfigurationUrl";

        private const string AppSettingsKeyDefaultAgentConfigurationCheckIntervalInSeconds = "DefaultAgentConfigurationCheckIntervalInSeconds";

        private const string AppSettingsKeyDefaultAgentConfigurationAgentsAreEnabled = "DefaultAgentConfigurationAgentsAreEnabled";

        public AgentConfiguration GetDefaultAgentConfiguration()
        {
            var agentsAreEnabled = bool.Parse(ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationAgentsAreEnabled]);
            var checkIntervalInSeconds = int.Parse(ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationCheckIntervalInSeconds]);
            var url = ConfigurationManager.AppSettings[AppSettingsKeyDefaultAgentConfigurationUrl];

            return new AgentConfiguration
                {
                    AgentsAreEnabled = agentsAreEnabled,
                    CheckIntervalInSeconds = checkIntervalInSeconds, 
                    SystemInformationSenderUrl = url
                };
        }
    }
}