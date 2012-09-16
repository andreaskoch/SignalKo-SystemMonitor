using System;

namespace SignalKo.SystemMonitor.Common.Services
{
    public class UTCTimeProvider : ITimeProvider
    {
        public DateTime GetDateAndTime()
        {
            return DateTime.UtcNow;
        }
    }
}