using System;

using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Coordination
{
    public class AgentCoordinationServiceFactory : IAgentCoordinationServiceFactory
    {
        private readonly IAgentConfigurationProvider agentConfigurationProvider;

        public AgentCoordinationServiceFactory(IAgentConfigurationProvider agentConfigurationProvider)
        {
            if (agentConfigurationProvider == null)
            {
                throw new ArgumentNullException("agentConfigurationProvider");
            }

            this.agentConfigurationProvider = agentConfigurationProvider;
        }

        public IAgentCoordinationService GetAgentCoordinationService(Action pauseCallback, Action resumeCallback)
        {
            return new AgentCoordinationService(this.agentConfigurationProvider, pauseCallback, resumeCallback);
        }
    }
}