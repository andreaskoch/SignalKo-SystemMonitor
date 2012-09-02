using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Sender;

namespace Agent.Core.Tests.IntegrationTests.Sender
{
    [TestFixture]
    public class RESTClientFactoryTests
    {
        [Test]
        public void GetRESTClient_BaseUrlParamterIsValid_ResultIsNotNull()
        {
            // Arrange
            string baseUrl = "http://localhost";
            var clientFactory = new RESTClientFactory();

            // Act
            var result = clientFactory.GetRESTClient(baseUrl);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}