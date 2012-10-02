using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Coordination
{
    public class AgentCoordinationService : IAgentCoordinationService
    {
        private const int AgentConfigurationCheckIntervalInMilliseconds = 10000;

        private readonly IAgentConfigurationProvider agentConfigurationProvider;

        private readonly Action pauseCallback;

        private readonly Action resumeCallback;

        private readonly object lockObject = new object();

        private bool stop;

        public AgentCoordinationService(IAgentConfigurationProvider agentConfigurationProvider, Action pauseCallback, Action resumeCallback)
        {
            if (agentConfigurationProvider == null)
            {
                throw new ArgumentNullException("agentConfigurationProvider");
            }

            if (pauseCallback == null)
            {
                throw new ArgumentNullException("pauseCallback");
            }

            if (resumeCallback == null)
            {
                throw new ArgumentNullException("resumeCallback");
            }

            this.agentConfigurationProvider = agentConfigurationProvider;
            this.pauseCallback = pauseCallback;
            this.resumeCallback = resumeCallback;
        }

        public void Start()
        {
            while (true)
            {
                // check if the service has been stopped
                Monitor.Enter(this.lockObject);

                if (this.stop)
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                Monitor.Exit(this.lockObject);

                var agentConfiguration = this.agentConfigurationProvider.GetAgentConfiguration();
                if (agentConfiguration == null)
                {
                    // pause as long as the configuration is invalid
                    this.pauseCallback();
                    continue;
                }

                // check status
                if (agentConfiguration.AgentsAreEnabled == false)
                {
                    this.pauseCallback();
                }
                else
                {
                    this.resumeCallback();
                }

                // sleep
                Thread.Sleep(AgentConfigurationCheckIntervalInMilliseconds);
            }
        }

        public void Stop()
        {
            Monitor.Enter(this.lockObject);
            this.stop = true;
            Monitor.Exit(this.lockObject);
        }
    }
}