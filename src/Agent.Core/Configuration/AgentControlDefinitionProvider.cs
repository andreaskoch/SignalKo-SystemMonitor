using System;
using System.Threading;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
	public class AgentControlDefinitionProvider : IAgentControlDefinitionProvider, IDisposable
	{
		public const int DefaultCheckIntervalInSeconds = 60;

		private readonly IAgentControlDefinitionAccessor agentControlDefinitionAccessor;

		private readonly Timer timer;

		private AgentControlDefinition controlDefinition;

		public AgentControlDefinitionProvider(IAgentControlDefinitionAccessor agentControlDefinitionAccessor)
		{
			if (agentControlDefinitionAccessor == null)
			{
				throw new ArgumentNullException("agentControlDefinitionAccessor");
			}

			this.agentControlDefinitionAccessor = agentControlDefinitionAccessor;

			var timerStartTime = new TimeSpan(0, 0, 0);
			var timerInterval = new TimeSpan(0, 0, 0, DefaultCheckIntervalInSeconds);
			this.timer = new Timer(state => this.UpdateControlDefinition(), null, timerStartTime, timerInterval);
		}

		public AgentControlDefinition GetControlDefinition()
		{
			return this.controlDefinition;
		}

		public void Dispose()
		{
			this.timer.Dispose();
		}

		private void UpdateControlDefinition()
		{
			this.controlDefinition = this.agentControlDefinitionAccessor.GetControlDefinition();

			// update the check interval
			if (this.controlDefinition != null && this.controlDefinition.CheckIntervalInSeconds > 0)
			{
				var timerStartTime = new TimeSpan(0, 0, this.controlDefinition.CheckIntervalInSeconds);
				var timerInterval = new TimeSpan(0, 0, 0, this.controlDefinition.CheckIntervalInSeconds);
				this.timer.Change(timerStartTime, timerInterval);
			}
		}
	}
}