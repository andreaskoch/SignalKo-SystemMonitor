using System;
using System.Collections.Generic;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess
{
    public interface ISystemInformationArchiveAccessor : IDisposable
    {
        void Store(SystemInformation systemInformation);

        IEnumerable<SystemInformation> SearchFor(Func<SystemInformation, bool> predicate);
    }
}