using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;

namespace Agent.Core.Tests.IntegrationTests.Sender
{
    [TestFixture]
    public class AppSettingsRESTServiceConfigurationProviderTests
    {
        [Test]
        public void GetConfiguration_ValuesArePresentInAppConfig_ValuesAreSet()
        {
            // Arrange
            var appSettingsRESTServiceConfigurationProvider = new AppSettingsRESTServiceConfigurationProvider();

            // Act
            var result = appSettingsRESTServiceConfigurationProvider.GetConfiguration();

            // Assert
            Assert.IsNotNullOrEmpty(result.BaseUrl);
            Assert.IsNotNullOrEmpty(result.ResourcePath);
        }
    }
}