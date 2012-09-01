using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Services;

namespace Agent.Core.Tests.IntegrationTests
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