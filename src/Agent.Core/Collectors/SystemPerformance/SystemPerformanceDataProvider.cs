using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance
{
	public class SystemPerformanceDataProvider : ISystemPerformanceDataProvider
	{
		private IProcessorStatusProvider processorStatusProvider;

		private ISystemMemoryStatusProvider systemMemoryStatusProvider;

		private ISystemStorageStatusProvider systemStorageStatusProvider;

		public SystemPerformanceDataProvider(IProcessorStatusProvider processorStatusProvider, ISystemMemoryStatusProvider systemMemoryStatusProvider, ISystemStorageStatusProvider systemStorageStatusProvider)
		{
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

			this.processorStatusProvider = processorStatusProvider;
			this.systemMemoryStatusProvider = systemMemoryStatusProvider;
			this.systemStorageStatusProvider = systemStorageStatusProvider;
		}

		public SystemPerformanceData GetSystemPerformanceData()
		{
			return new SystemPerformanceData
				{
					ProcessorStatus = this.processorStatusProvider.GetProcessorStatus(),
					MemoryStatus = this.systemMemoryStatusProvider.GetMemoryStatus(),
					StorageStatus = this.systemStorageStatusProvider.GetStorageStatus()
				};
		}
	}
}