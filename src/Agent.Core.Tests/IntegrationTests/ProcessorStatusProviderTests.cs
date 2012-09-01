using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Services;

namespace Agent.Core.Tests.IntegrationTests
{
    [TestFixture]
    public class ProcessorStatusProviderTests
    {
        [Test]
        public void GetProcessorUtilizationInPercent()
        {
            // Arrange
            var processorStatusProvider = new ProcessorStatusProvider();

            // Act
            var result = processorStatusProvider.GetProcessorUtilizationInPercent();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}