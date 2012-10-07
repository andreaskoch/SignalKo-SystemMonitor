using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace Agent.Core.Tests.IntegrationTests.Configuration
{
    [TestFixture]
    public class AppConfigAgentConfigurationServiceUrlProviderTests
    {
        #region GetServiceConfiguration

        [Test]
        public void GetServiceConfiguration_ResultIsNotNull()
        {
            // Arrange
            var appConfigAgentConfigurationServiceUrlProvider = new AppConfigAgentConfigurationServiceUrlProvider();

            // Act
            var result = appConfigAgentConfigurationServiceUrlProvider.GetServiceConfiguration();

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}