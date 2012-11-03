using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;

namespace Agent.Core.Tests.IntegrationTests.Collectors.SystemPerformance
{
    [TestFixture]
    public class LogicalDiscInstanceNameProviderTests
    {
        [Test]
        public void GetLogicalDiscInstanceNames_ResultIsNotEmpty()
        {
            // Arrange
            var logicalDiscInstanceNameProvider = new LogicalDiscInstanceNameProvider();

            // Act
            var result = logicalDiscInstanceNameProvider.GetLogicalDiscInstanceNames();

            // Assert
            Assert.IsTrue(result.Length > 0);
        }
    }
}