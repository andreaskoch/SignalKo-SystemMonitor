using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;

namespace Agent.Core.Tests.UnitTests.Collectors.SystemPerformance
{
	[TestFixture]
	public class SystemPerformanceDataProviderTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			var systemPerformanceDataProvider = new SystemPerformanceDataProvider(
				processorStatusProvider.Object, systemMemoryStatusProvider.Object, systemStorageStatusProvider.Object);

			// Assert
			Assert.IsNotNull(systemPerformanceDataProvider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ProcessorStatusProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(null, systemMemoryStatusProvider.Object, systemStorageStatusProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_SystemMemoryStatusProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(processorStatusProvider.Object, null, systemStorageStatusProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_SystemStorageStatusProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(processorStatusProvider.Object, systemMemoryStatusProvider.Object, null);
		}

		#endregion

		#region GetSystemPerformanceData

		[Test]
		public void GetSystemPerformanceData_ResultIsNotNull()
		{
			// Arrange
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			var systemPerformanceDataProvider = new SystemPerformanceDataProvider(
				processorStatusProvider.Object, systemMemoryStatusProvider.Object, systemStorageStatusProvider.Object);

			// Act
			var result = systemPerformanceDataProvider.GetSystemPerformanceData();

			// Assert
			Assert.IsNotNull(result);
		}

		#endregion
	}
}