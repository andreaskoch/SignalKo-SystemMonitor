using System.Threading;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.IntegrationTests.Collectors
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

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var agentControlDefinition = new AgentControlDefinition { AgentIsEnabled = true, CheckIntervalInSeconds = 60 };
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var httpStatusCodeFetcher = new Mock<IHttpStatusCodeFetcher>();

			IHttpStatusCodeCheckResultProvider httpStatusCodeCheckResultProvider = new HttpStatusCodeCheckResultProvider(
				agentControlDefinitionProvider.Object, httpStatusCodeFetcher.Object);

			var systemInformationProvider = new SystemInformationProvider(
				timeProvider, machineNameProvider, systemPerformanceDataProvider, httpStatusCodeCheckResultProvider);

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

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var agentControlDefinition = new AgentControlDefinition { AgentIsEnabled = true, CheckIntervalInSeconds = 60 };
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var httpStatusCodeFetcher = new Mock<IHttpStatusCodeFetcher>();

			IHttpStatusCodeCheckResultProvider httpStatusCodeCheckResultProvider = new HttpStatusCodeCheckResultProvider(
				agentControlDefinitionProvider.Object, httpStatusCodeFetcher.Object);

			var systemInformationProvider = new SystemInformationProvider(
				timeProvider, machineNameProvider, systemPerformanceDataProvider, httpStatusCodeCheckResultProvider);

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