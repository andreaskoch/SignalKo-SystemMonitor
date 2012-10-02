using System;
using System.Threading;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class AgentConfigurationProvider : IAgentConfigurationProvider, IDisposable
    {
        private const int DefaultCheckIntervalInSeconds = 60;

        private readonly IAgentConfigurationAccessor agentConfigurationAccessor;

        private readonly Timer timer;

        private AgentConfiguration agentConfiguration;

        public AgentConfigurationProvider(IAgentConfigurationAccessor agentConfigurationAccessor)
        {
            if (agentConfigurationAccessor == null)
            {
                throw new ArgumentNullException("agentConfigurationAccessor");
            }

            this.agentConfigurationAccessor = agentConfigurationAccessor;

            var timerStartTime = new TimeSpan(0, 0, 0);
            var timerInterval = new TimeSpan(0, 0, 0, DefaultCheckIntervalInSeconds);
            this.timer = new Timer(state => this.UpdateAgentConfiguration(), null, timerStartTime, timerInterval);
        }

        public AgentConfiguration GetAgentConfiguration()
        {
            return this.agentConfiguration;
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }

        private void UpdateAgentConfiguration()
        {
            this.agentConfiguration = this.agentConfigurationAccessor.GetAgentConfiguration();

            // update the check interval
            if (this.agentConfiguration != null && this.agentConfiguration.CheckIntervalInSeconds > 0)
            {
                var timerStartTime = new TimeSpan(0, 0, this.agentConfiguration.CheckIntervalInSeconds);
                var timerInterval = new TimeSpan(0, 0, 0, this.agentConfiguration.CheckIntervalInSeconds);
                this.timer.Change(timerStartTime, timerInterval);
            }
        }
    }
}