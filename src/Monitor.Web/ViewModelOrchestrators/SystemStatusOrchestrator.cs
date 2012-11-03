using System;
using System.Collections.Generic;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
	public class SystemStatusOrchestrator : ISystemStatusOrchestrator
	{
		private readonly IProcessorStatusOrchestrator processorStatusOrchestrator;

		private readonly IMemoryStatusOrchestrator memoryStatusOrchestrator;

		private readonly IStorageStatusOrchestrator storageStatusOrchestrator;

		public SystemStatusOrchestrator(IProcessorStatusOrchestrator processorStatusOrchestrator, IMemoryStatusOrchestrator memoryStatusOrchestrator, IStorageStatusOrchestrator storageStatusOrchestrator)
		{
			if (processorStatusOrchestrator == null)
			{
				throw new ArgumentNullException("processorStatusOrchestrator");
			}

			if (memoryStatusOrchestrator == null)
			{
				throw new ArgumentNullException("memoryStatusOrchestrator");
			}

			if (storageStatusOrchestrator == null)
			{
				throw new ArgumentNullException("storageStatusOrchestrator");
			}

			this.processorStatusOrchestrator = processorStatusOrchestrator;
			this.memoryStatusOrchestrator = memoryStatusOrchestrator;
			this.storageStatusOrchestrator = storageStatusOrchestrator;
		}

		public SystemStatusViewModel GetSystemStatusViewModel(SystemInformation systemInformation)
		{
			if (systemInformation == null)
			{
				throw new ArgumentNullException("systemInformation");
			}

			var systemStatusViewModel = new SystemStatusViewModel { MachineName = systemInformation.MachineName, Timestamp = systemInformation.Timestamp, };

			// add data series
			var dataSerieses = new List<SystemStatusPointViewModel>();

			// systerm performance
			if (systemInformation.SystemPerformance != null)
			{
				// Processor status data series
				if (systemInformation.SystemPerformance.ProcessorStatus != null)
				{
					dataSerieses.Add(this.processorStatusOrchestrator.GetProcessorUtilizationInPercent(systemInformation.SystemPerformance.ProcessorStatus));
				}

				// Memory status data series
				if (systemInformation.SystemPerformance.MemoryStatus != null)
				{
					dataSerieses.Add(this.memoryStatusOrchestrator.GetMemoryUtilizationInPercent(systemInformation.SystemPerformance.MemoryStatus));
				}

				// Storage status data series
				if (systemInformation.SystemPerformance.StorageStatus != null)
				{
					dataSerieses.AddRange(this.storageStatusOrchestrator.GetStorageUtilizationInPercent(systemInformation.SystemPerformance.StorageStatus));
				}
			}

			systemStatusViewModel.DataPoints = dataSerieses.ToArray();

			return systemStatusViewModel;
		}
	}
}