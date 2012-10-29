using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
    public class GroupConfigurationController : ApiController
    {
        private readonly IServerConfigurationRepository configurationRepository;

        public GroupConfigurationController(IServerConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public MachineGroupConfiguration Get()
        {

            MachineGroupConfiguration configuration = this.configurationRepository.LoadConfiguration();
            return configuration;
        }

        public void Put(MachineGroupConfiguration configuration)
        {
            this.configurationRepository.SaveConfiguration(configuration);
        }
    }
}
