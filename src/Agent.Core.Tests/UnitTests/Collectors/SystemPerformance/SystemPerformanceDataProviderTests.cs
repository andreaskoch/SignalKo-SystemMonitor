using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

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
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			var systemPerformanceDataProvider = new SystemPerformanceDataProvider(
				agentControlDefinitionProvider.Object, processorStatusProvider.Object, systemMemoryStatusProvider.Object, systemStorageStatusProvider.Object);

			// Assert
			Assert.IsNotNull(systemPerformanceDataProvider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentControlDefinitionProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(null, processorStatusProvider.Object, systemMemoryStatusProvider.Object, systemStorageStatusProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ProcessorStatusProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(agentControlDefinitionProvider.Object, null, systemMemoryStatusProvider.Object, systemStorageStatusProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_SystemMemoryStatusProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(agentControlDefinitionProvider.Object, processorStatusProvider.Object, null, systemStorageStatusProvider.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_SystemStorageStatusProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var processorStatusProvider = new Mock<IProcessorStatusProvider>();
			var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();

			// Act
			new SystemPerformanceDataProvider(agentControlDefinitionProvider.Object, processorStatusProvider.Object, systemMemoryStatusProvider.Object, null);
		}

		#endregion
	}
}