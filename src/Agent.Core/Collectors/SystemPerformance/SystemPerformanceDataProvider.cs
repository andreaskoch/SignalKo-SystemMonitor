using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance
{
	public class SystemPerformanceDataProvider : ISystemPerformanceDataProvider
	{
		public const int DefaultCheckIntervalInSeconds = 1;

		private readonly IAgentControlDefinitionProvider agentControlDefinitionProvider;

		private readonly IProcessorStatusProvider processorStatusProvider;

		private readonly ISystemMemoryStatusProvider systemMemoryStatusProvider;

		private readonly ISystemStorageStatusProvider systemStorageStatusProvider;

		private readonly Timer timer;

		private SystemPerformanceData systemPerformanceData;

		public SystemPerformanceDataProvider(IAgentControlDefinitionProvider agentControlDefinitionProvider, IProcessorStatusProvider processorStatusProvider, ISystemMemoryStatusProvider systemMemoryStatusProvider, ISystemStorageStatusProvider systemStorageStatusProvider)
		{
			if (agentControlDefinitionProvider == null)
			{
				throw new ArgumentNullException("agentControlDefinitionProvider");
			}

			if (processorStatusProvider == null)
			{
				throw new ArgumentNullException("processorStatusProvider");
			}

			if (systemMemoryStatusProvider == null)
			{
				throw new ArgumentNullException("systemMemoryStatusProvider");
			}

			if (systemStorageStatusProvider == null)
			{
				throw new ArgumentNullException("systemStorageStatusProvider");
			}

			this.agentControlDefinitionProvider = agentControlDefinitionProvider;
			this.processorStatusProvider = processorStatusProvider;
			this.systemMemoryStatusProvider = systemMemoryStatusProvider;
			this.systemStorageStatusProvider = systemStorageStatusProvider;

			// get initial check interval
			var agentControlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
			int checkIntervalInSeconds = DefaultCheckIntervalInSeconds;
			if (agentControlDefinition != null && agentControlDefinition.HttpStatusCodeCheck != null && agentControlDefinition.HttpStatusCodeCheck.CheckIntervalInSeconds > 0)
			{
				checkIntervalInSeconds = agentControlDefinition.HttpStatusCodeCheck.CheckIntervalInSeconds;
			}

			var timerStartTime = new TimeSpan(0, 0, 0);
			var timerInterval = new TimeSpan(0, 0, 0, checkIntervalInSeconds);
			this.timer = new Timer(state => this.UpdateSystemPerformanceData(), null, timerStartTime, timerInterval);
		}

		public SystemPerformanceData GetSystemPerformanceData()
		{
			return this.systemPerformanceData;
		}

		private void UpdateSystemPerformanceData()
		{
			// get latest control definition
			var controlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
			if (controlDefinition == null || controlDefinition.SystemPerformanceCheck == null)
			{
				return;
			}

			var systemPerformanceCheckSettings = controlDefinition.SystemPerformanceCheck;

			// get the latest performance data
			this.systemPerformanceData = new SystemPerformanceData
				{
					ProcessorStatus = this.processorStatusProvider.GetProcessorStatus(),
					MemoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus(),
					StorageStatus = this.systemStorageStatusProvider.GetStorageStatus()
				};

			// update the check interval
			if (systemPerformanceCheckSettings.CheckIntervalInSeconds > 0)
			{
				var timerStartTime = new TimeSpan(0, 0, systemPerformanceCheckSettings.CheckIntervalInSeconds);
				var timerInterval = new TimeSpan(0, 0, 0, systemPerformanceCheckSettings.CheckIntervalInSeconds);
				this.timer.Change(timerStartTime, timerInterval);
			}
		}
	}
}