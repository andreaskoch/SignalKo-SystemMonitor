using System;
using System.Linq;

using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
    public class KnownAgentsProvider : IKnownAgentsProvider
    {
        private readonly ISystemInformationArchiveAccessor systemInformationArchiveAccessor;

        public KnownAgentsProvider(ISystemInformationArchiveAccessor systemInformationArchiveAccessor)
        {
            if (systemInformationArchiveAccessor == null)
            {
                throw new ArgumentNullException("systemInformationArchiveAccessor");
            }

            this.systemInformationArchiveAccessor = systemInformationArchiveAccessor;
        }

        public string[] GetKnownAgents()
        {
            return this.systemInformationArchiveAccessor.Select(information => information.MachineName).Distinct().ToArray();
        }
    }
}