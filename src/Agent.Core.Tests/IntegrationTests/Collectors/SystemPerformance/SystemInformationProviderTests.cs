using System.Threading;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.IntegrationTests.Collectors.SystemPerformance
{
	[TestFixture]
	public class SystemInformationProviderTests
	{
		[Test]
		public void GetSystemInfo_ResultIsNotNull()
		{
			// Arrange
			ITimeProvider timeProvider = new UTCTimeProvider();
			IMachineNameProvider machineNameProvider = new EnvironmentMachineNameProvider();
			IProcessorStatusProvider processorStatusProvider = new ProcessorStatusProvider();
			IMemoryUnitConverter memoryUnitConverter = new MemoryUnitConverter();
			ISystemMemoryStatusProvider systemMemoryStatusProvider = new SystemMemoryStatusProvider(memoryUnitConverter);
			ILogicalDiscInstanceNameProvider logicalDiscInstanceNameProvider = new LogicalDiscInstanceNameProvider();
			ISystemStorageStatusProvider systemStorageStatusProvider = new SystemStorageStatusProvider(logicalDiscInstanceNameProvider);
			ISystemPerformanceDataProvider systemPerformanceDataProvider = new SystemPerformanceDataProvider(
				processorStatusProvider, systemMemoryStatusProvider, systemStorageStatusProvider);

			var systemInformationProvider = new SystemInformationProvider(
				timeProvider,
				machineNameProvider,
				systemPerformanceDataProvider);

			// Act
			var result = systemInformationProvider.GetSystemInfo();

			// Assert
			Assert.IsNotNull(result);
		}

		[Test]
		public void GetSystemInfo_Every100Milliseconds_NoResultIsNull()
		{
			// Arrange
			ITimeProvider timeProvider = new UTCTimeProvider();
			IMachineNameProvider machineNameProvider = new EnvironmentMachineNameProvider();
			IProcessorStatusProvider processorStatusProvider = new ProcessorStatusProvider();
			IMemoryUnitConverter memoryUnitConverter = new MemoryUnitConverter();
			ISystemMemoryStatusProvider systemMemoryStatusProvider = new SystemMemoryStatusProvider(memoryUnitConverter);
			ILogicalDiscInstanceNameProvider logicalDiscInstanceNameProvider = new LogicalDiscInstanceNameProvider();
			ISystemStorageStatusProvider systemStorageStatusProvider = new SystemStorageStatusProvider(logicalDiscInstanceNameProvider);
			ISystemPerformanceDataProvider systemPerformanceDataProvider = new SystemPerformanceDataProvider(
				processorStatusProvider, systemMemoryStatusProvider, systemStorageStatusProvider);

			var systemInformationProvider = new SystemInformationProvider(timeProvider, machineNameProvider, systemPerformanceDataProvider);

			// Act
			for (var i = 0; i < 10; i++)
			{
				var result = systemInformationProvider.GetSystemInfo();

				// Assert
				Assert.IsNotNull(result);

				Thread.Sleep(100);
			}
		}
	}
}