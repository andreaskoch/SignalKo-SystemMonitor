using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Sender;

namespace Agent.Core.Tests.IntegrationTests.Sender
{
    [TestFixture]
    public class RESTClientFactoryTests
    {
        [Test]
        public void GetRESTClient_HostaddressParamterIsValid_ResultIsNotNull()
        {
            // Arrange
            string hostaddress = "localhost";
            var clientFactory = new RESTClientFactory();

            // Act
            var result = clientFactory.GetRESTClient(hostaddress);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}