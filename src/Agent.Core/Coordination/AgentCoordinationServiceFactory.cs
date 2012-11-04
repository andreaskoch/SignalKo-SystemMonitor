using System;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Coordination
{
    public class AgentCoordinationServiceFactory : IAgentCoordinationServiceFactory
    {
        private readonly IAgentControlDefinitionProvider agentControlDefinitionProvider;

        public AgentCoordinationServiceFactory(IAgentControlDefinitionProvider agentControlDefinitionProvider)
        {
            if (agentControlDefinitionProvider == null)
            {
                throw new ArgumentNullException("agentControlDefinitionProvider");
            }

            this.agentControlDefinitionProvider = agentControlDefinitionProvider;
        }

        public IAgentCoordinationService GetAgentCoordinationService(Action pauseCallback, Action resumeCallback)
        {
            if (pauseCallback == null)
            {
                throw new ArgumentNullException("pauseCallback");
            }

            if (resumeCallback == null)
            {
                throw new ArgumentNullException("resumeCallback");
            }
            
            return new AgentCoordinationService(this.agentControlDefinitionProvider, pauseCallback, resumeCallback);
        }
    }
}