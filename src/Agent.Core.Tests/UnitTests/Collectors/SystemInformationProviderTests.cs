using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.UnitTests.Collectors
{
	[TestFixture]
	public class SystemInformationProviderTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var timeProvider = new Mock<ITimeProvider>();
			var machineNameProvider = new Mock<IMachineNameProvider>();
			var systemPerformanceDataProvider = new Mock<ISystemPerformanceDataProvider>();

			// Act
			var systemInformationProvider = new SystemInformationProvider(timeProvider.Object, machineNameProvider.Object, systemPerformanceDataProvider.Object);

			// Assert
			Assert.IsNotNull(systemInformationProvider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_TimeProviderParametersIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var machineNameProvider = new Mock<IMachineNameProvider>();
			var systemPerformanceDataProvider = new Mock<ISystemPerformanceDataProvider>();

			// Act
			new SystemInformationProvider(null, machineNameProvider.Object, systemPerformanceDataProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_MachineNameProviderParametersIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var timeProvider = new Mock<ITimeProvider>();
			var systemPerformanceDataProvider = new Mock<ISystemPerformanceDataProvider>();

			// Act
			new SystemInformationProvider(timeProvider.Object, null, systemPerformanceDataProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_SystemPerformanceDataProviderParametersIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var timeProvider = new Mock<ITimeProvider>();
			var machineNameProvider = new Mock<IMachineNameProvider>();

			// Act
			new SystemInformationProvider(timeProvider.Object, machineNameProvider.Object, null);
		}

		#endregion
	}
}