using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace Agent.Core.Tests.IntegrationTests.Sender
{
    [TestFixture]
    public class AppConfigAgentConfigurationServiceUrlProviderTests
    {
        [Test]
        public void GetConfiguration_ValueIsPresentInAppConfig_ResultIsNotNullOrEmpty()
        {
            // Arrange
            var appSettingsRESTServiceConfigurationProvider = new AppConfigAgentConfigurationServiceUrlProvider();

            // Act
            var result = appSettingsRESTServiceConfigurationProvider.GetServiceUrl();

            // Assert
            Assert.IsNotNullOrEmpty(result);
        }
    }
}