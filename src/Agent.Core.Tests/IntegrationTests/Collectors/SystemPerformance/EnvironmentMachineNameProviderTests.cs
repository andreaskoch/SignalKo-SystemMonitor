using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;

namespace Agent.Core.Tests.IntegrationTests.Collectors.SystemPerformance
{
    [TestFixture]
    public class EnvironmentMachineNameProviderTests
    {
        [Test]
        public void GetMachineName_ResultIsNotNullOrEmpty()
        {
            // Arrange
            var machineNameProvider = new EnvironmentMachineNameProvider();

            // Act
            var result = machineNameProvider.GetMachineName();

            // Assert
            Assert.IsNotNullOrEmpty(result);
        }
    }
}