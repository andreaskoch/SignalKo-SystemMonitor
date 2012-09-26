using System.IO;
using System.Web.Http;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
    public class GroupConfigurationController : ApiController
    {
        public string Get()
        {
            var repository = new ConfigurationRepository();
            string data = repository.LoadConfiguration();

            return data;
        }

        public void Put(string configuration)
        {
            var repository = new ConfigurationRepository();
            repository.SaveConfiguration(configuration);
        }
    }

    public class ConfigurationRepository
    {
        public string LoadConfiguration()
        {
            if (!File.Exists(@"C:\tmp\configuration.json"))
            {
                return string.Empty;
            }

            TextReader tr = new StreamReader(@"C:\tmp\configuration.json");
            string data = tr.ReadToEnd();
            tr.Close();
            return data;
        }

        public void SaveConfiguration(string configuration)
        {
            TextWriter tw = new StreamWriter(@"C:\tmp\configuration.json", false);
            tw.WriteLine(configuration);
            tw.Close();
        }
    }
}
