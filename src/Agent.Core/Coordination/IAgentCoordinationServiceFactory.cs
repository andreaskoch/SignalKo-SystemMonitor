using System;

namespace SignalKo.SystemMonitor.Agent.Core.Coordination
{
    public interface IAgentCoordinationServiceFactory
    {
        IAgentCoordinationService GetAgentCoordinationService(Action pauseCallback, Action resumeCallback);
    }
}