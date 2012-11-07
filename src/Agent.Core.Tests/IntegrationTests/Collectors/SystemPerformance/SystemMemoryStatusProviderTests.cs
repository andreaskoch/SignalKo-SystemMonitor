using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.IntegrationTests.Collectors.SystemPerformance
{
    [TestFixture]
    public class SystemMemoryStatusProviderTests
    {
        [Test]
        public void GetMemoryStatus()
        {
            // Arrange
            var memoryUnitConverter = new MemoryUnitConverter();
            var systemMemoryStatusProvider = new SystemMemoryStatusProvider(memoryUnitConverter);

            // Act
            var result = systemMemoryStatusProvider.GetMemoryStatus();

            // Assert
            Assert.AreNotEqual(0d, result.AvailableMemoryInGB);
            Assert.AreNotEqual(0d, result.UsedMemoryInGB);
        }

        [Test]
        public void GetMemoryStatus_Dispose()
        {
            // Arrange
            var memoryUnitConverter = new MemoryUnitConverter();
            using (var systemMemoryStatusProvider = new SystemMemoryStatusProvider(memoryUnitConverter))
            {
                // Act
                var result = systemMemoryStatusProvider.GetMemoryStatus();

                // Assert
                Assert.AreNotEqual(0d, result.AvailableMemoryInGB);
                Assert.AreNotEqual(0d, result.UsedMemoryInGB);                
            }
        }
    }
}