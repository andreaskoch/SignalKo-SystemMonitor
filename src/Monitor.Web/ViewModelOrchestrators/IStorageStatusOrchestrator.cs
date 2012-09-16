using System.Collections.Generic;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public interface IStorageStatusOrchestrator
    {
        IEnumerable<SystemStatusPointViewModel> GetStorageUtilizationInPercent(SystemStorageInformation systemStorageInformation);
    }
}