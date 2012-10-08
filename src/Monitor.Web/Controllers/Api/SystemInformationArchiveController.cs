using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
    public class SystemInformationArchiveController : ApiController
    {
        private readonly ISystemInformationArchiveAccessor systemInformationArchiveAccessor;

        public SystemInformationArchiveController(ISystemInformationArchiveAccessor systemInformationArchiveAccessor)
        {
            if (systemInformationArchiveAccessor == null)
            {
                throw new ArgumentNullException("systemInformationArchiveAccessor");
            }

            this.systemInformationArchiveAccessor = systemInformationArchiveAccessor;
        }

        public IEnumerable<SystemInformation> GetByDate(DateTime dateTime)
        {
            var dateMin = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, DateTimeKind.Utc);
            var dateMax = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 0, DateTimeKind.Utc);

            return
                this.systemInformationArchiveAccessor.SearchFor(
                    systemInformation => systemInformation.Timestamp >= dateMin && systemInformation.Timestamp <= dateMax).OrderBy(
                        information => information.Timestamp);
        }
    }
}
