using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;

namespace Agent.Core.Tests.IntegrationTests
{
    [TestFixture]
    public class SystemStorageStatusProviderTests
    {
        [Test]
        public void GetStorageStatus_ResultIsNotNullAndContainsAtLeastOneEntry()
        {
            // Arrange
            ILogicalDiscInstanceNameProvider logicalDiscInstanceNameProvider = new LogicalDiscInstanceNameProvider();
            var systemStorageStatusProvider = new SystemStorageStatusProvider(logicalDiscInstanceNameProvider);

            // Act
            var result = systemStorageStatusProvider.GetStorageStatus();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StorageDeviceInfos.Length > 0);
        }
    }
}