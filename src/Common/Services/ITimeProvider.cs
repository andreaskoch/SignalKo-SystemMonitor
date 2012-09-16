using System;

namespace SignalKo.SystemMonitor.Common.Services
{
    public interface ITimeProvider
    {
        DateTime GetDateAndTime();
    }
}