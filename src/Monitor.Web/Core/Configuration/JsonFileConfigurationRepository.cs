using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using SignalKo.SystemMonitor.Common.Model.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.Controllers.Api;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Configuration
{
    public class JsonFileConfigurationRepository : IServerConfigurationRepository
    {
        private readonly IFileSystemDataStoreConfigurationProvider configurationProvider;

        public JsonFileConfigurationRepository(IFileSystemDataStoreConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public MachineGroupConfiguration LoadConfiguration()
        {
            string filePath = Path.Combine(this.configurationProvider.GetConfiguration().ConfigurationFolder, "configuration.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            TextReader tr = new StreamReader(filePath);
            string data = tr.ReadToEnd();
            tr.Close();
            JsonSerializer ser = new JsonSerializer();
            MachineGroupConfiguration configuration = JsonConvert.DeserializeObject<MachineGroupConfiguration>(data);
            return configuration;
        }

        public void SaveConfiguration(MachineGroupConfiguration configuration)
        {
            
            string congigAsString = JsonConvert.SerializeObject(configuration);
            string filePath = Path.Combine(this.configurationProvider.GetConfiguration().ConfigurationFolder, "configuration.json");
            TextWriter tw = new StreamWriter(filePath, false);
            tw.WriteLine(congigAsString);
            tw.Close();
        }
    }
}