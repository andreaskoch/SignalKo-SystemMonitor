using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace Agent.Core.Tests.IntegrationTests.Configuration
{
    [TestFixture]
    public class AppConfigAgentConfigurationServiceUrlProviderTests
    {
        #region GetServiceUrl

        [Test]
        public void GetServiceUrl_ResultIsNotNullOrEmpty()
        {
            // Arrange
            var appConfigAgentConfigurationServiceUrlProvider = new AppConfigAgentConfigurationServiceUrlProvider();

            // Act
            var result = appConfigAgentConfigurationServiceUrlProvider.GetServiceUrl();

            // Assert
            Assert.IsNotNullOrEmpty(result);
        }

        #endregion
    }
}