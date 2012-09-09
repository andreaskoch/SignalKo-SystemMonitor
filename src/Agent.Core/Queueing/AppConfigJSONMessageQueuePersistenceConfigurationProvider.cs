using System.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class AppConfigJSONMessageQueuePersistenceConfigurationProvider : IJSONMessageQueuePersistenceConfigurationProvider
    {
        private const string AppSettingsKeyMessageQueuePersistenceFilePath = "MessageQueuePersistenceFilePath";

        public JSONMessageQueuePersistenceConfiguration GetConfiguration()
        {
            return new JSONMessageQueuePersistenceConfiguration { FilePath = ConfigurationManager.AppSettings[AppSettingsKeyMessageQueuePersistenceFilePath] };
        }
    }
}