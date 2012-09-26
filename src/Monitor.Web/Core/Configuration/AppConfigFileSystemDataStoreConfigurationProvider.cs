using System.Configuration;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public class AppConfigFileSystemDataStoreConfigurationProvider : IFileSystemDataStoreConfigurationProvider
    {
        private const string AppSettingsKeyConfigurationFolder = "SystemDataStoreConfigurationFolder";

        public FileSystemDataStoreConfiguration GetConfiguration()
        {
            string configurationFolder = ConfigurationManager.AppSettings[AppSettingsKeyConfigurationFolder];
            return new FileSystemDataStoreConfiguration { ConfigurationFolder = configurationFolder };
        }
    }
}