using System;

namespace SignalKo.SystemMonitor.Agent.Core.Services
{
    public interface ITimeProvider
    {
        DateTimeOffset GetUTCDateAndTime();
    }
}