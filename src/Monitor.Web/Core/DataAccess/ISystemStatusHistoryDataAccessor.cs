using System.Collections.Generic;

using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Persistenz
{
    public interface ISystemStatusHistoryDataAccessor
    {
        void Add(SystemStatusViewModel systemData);

        IEnumerable<SystemStatusViewModel> GetServerHistory(string serverName);
    }
}