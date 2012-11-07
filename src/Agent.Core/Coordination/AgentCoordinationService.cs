using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace SignalKo.SystemMonitor.Agent.Core.Coordination
{
	public class AgentCoordinationService : IAgentCoordinationService, IDisposable
	{
		public const int AgentControlDefinitionCheckIntervalInMilliseconds = 1000;

		private readonly IAgentControlDefinitionProvider agentControlDefinitionProvider;

		private readonly Action pauseCallback;

		private readonly Action resumeCallback;

		private readonly object lockObject = new object();

		private bool stop;

		public AgentCoordinationService(IAgentControlDefinitionProvider agentControlDefinitionProvider, Action pauseCallback, Action resumeCallback)
		{
			if (agentControlDefinitionProvider == null)
			{
				throw new ArgumentNullException("agentControlDefinitionProvider");
			}

			if (pauseCallback == null)
			{
				throw new ArgumentNullException("pauseCallback");
			}

			if (resumeCallback == null)
			{
				throw new ArgumentNullException("resumeCallback");
			}

			this.agentControlDefinitionProvider = agentControlDefinitionProvider;
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

				var agentControlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
				if (agentControlDefinition == null)
				{
					// pause as long as the configuration is invalid
					this.pauseCallback();
					continue;
				}

				// check status
				if (agentControlDefinition.AgentIsEnabled == false)
				{
					this.pauseCallback();
				}
				else
				{
					this.resumeCallback();
				}

				// sleep
				Thread.Sleep(AgentControlDefinitionCheckIntervalInMilliseconds);
			}
		}

		public void Stop()
		{
			Monitor.Enter(this.lockObject);
			this.stop = true;
			Monitor.Exit(this.lockObject);
		}

		public void Dispose()
		{
			this.Stop();
		}
	}
}