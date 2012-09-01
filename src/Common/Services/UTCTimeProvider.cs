using System;

namespace SignalKo.SystemMonitor.Common.Services
{
    public class UTCTimeProvider : ITimeProvider
    {
        public DateTimeOffset GetDateAndTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}