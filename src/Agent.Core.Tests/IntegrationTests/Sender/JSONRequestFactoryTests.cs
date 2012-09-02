using NUnit.Framework;

using RestSharp;

using SignalKo.SystemMonitor.Agent.Core.Sender;

namespace Agent.Core.Tests.IntegrationTests.Sender
{
    [TestFixture]
    public class JSONRequestFactoryTests
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("api/systeminformation")]
        public void CreatePutRequest_ResultIsNotNull(string resourcePath)
        {
            // Arrange
            var requestFactory = new JSONRequestFactory();

            // Act
            var result = requestFactory.CreatePutRequest(resourcePath);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreatePutRequest_RequestFormatIsJSON()
        {
            // Arrange
            string resourcePath = "some/path";
            var requestFactory = new JSONRequestFactory();

            // Act
            var result = requestFactory.CreatePutRequest(resourcePath);

            // Assert
            Assert.AreEqual(DataFormat.Json, result.RequestFormat);
        }

        [Test]
        public void CreatePutRequest_MethodIsPut()
        {
            // Arrange
            string resourcePath = "some/path";
            var requestFactory = new JSONRequestFactory();

            // Act
            var result = requestFactory.CreatePutRequest(resourcePath);

            // Assert
            Assert.AreEqual(Method.PUT, result.Method);
        }
    }
}