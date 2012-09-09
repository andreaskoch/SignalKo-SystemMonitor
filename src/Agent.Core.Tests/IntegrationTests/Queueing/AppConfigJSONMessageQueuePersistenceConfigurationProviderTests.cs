using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queueing;

namespace Agent.Core.Tests.IntegrationTests.Queueing
{
    [TestFixture]
    public class AppConfigJSONMessageQueuePersistenceConfigurationProviderTests
    {
        [Test]
        public void GetConfiguration_ResultIsNotNull()
        {
            // Arrange
            var configProvider = new AppConfigJSONMessageQueuePersistenceConfigurationProvider();

            // Act
            var result = configProvider.GetConfiguration();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetConfiguration_FilePathIsSet()
        {
            // Arrange
            var configProvider = new AppConfigJSONMessageQueuePersistenceConfigurationProvider();

            // Act
            var result = configProvider.GetConfiguration();

            // Assert
            Assert.IsNotNullOrEmpty(result.FilePath);
        }
    }
}