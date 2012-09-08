using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;

namespace Agent.Core.Tests.IntegrationTests.Collector
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