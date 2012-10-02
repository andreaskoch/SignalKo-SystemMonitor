using System;

using Moq;

using NUnit.Framework;

using RestSharp;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Sender
{
    [TestFixture]
    public class RESTBasedSystemInformationSenderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var serviceConfiguration = new Mock<IRESTServiceConfiguration>();
            serviceConfiguration.Setup(s => s.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(serviceConfiguration.Object);

            var client = new Mock<IRestClient>();
            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(client.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();

            // Act
            var systemInformationSender = new RESTBasedSystemInformationSender(
                systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Assert
            Assert.IsNotNull(systemInformationSender);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationSenderConfigurationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var restClientFactory = new Mock<IRESTClientFactory>();
            var requestFactory = new Mock<IRESTRequestFactory>();

            // Act
            new RESTBasedSystemInformationSender(null, restClientFactory.Object, requestFactory.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RESTClientFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var serviceConfiguration = new Mock<IRESTServiceConfiguration>();
            serviceConfiguration.Setup(s => s.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(serviceConfiguration.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();

            // Act
            new RESTBasedSystemInformationSender(systemInformationSenderConfigurationProvider.Object, null, requestFactory.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RESTRequestFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var serviceConfiguration = new Mock<IRESTServiceConfiguration>();
            serviceConfiguration.Setup(s => s.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(serviceConfiguration.Object);

            var restClientFactory = new Mock<IRESTClientFactory>();

            // Act
            new RESTBasedSystemInformationSender(systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, null);
        }

        #endregion

        #region Send

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_SystemInformationParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            SystemInformation systemInformation = null;

            var configuration = new Mock<IRESTServiceConfiguration>();
            configuration.Setup(c => c.BaseUrl).Returns("http://localhost");
            configuration.Setup(c => c.ResourcePath).Returns("api/systeminformation");
            configuration.Setup(c => c.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(configuration.Object);

            var client = new Mock<IRestClient>();
            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(client.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();

            var systemInformationSender = new RESTBasedSystemInformationSender(
                systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        [Test]
        [ExpectedException(typeof(SystemInformationSenderSetupException))]
        public void Send_ServiceConfigurationProviderReturnsNull_SystemInformationSenderSetupExceptionIsThrown()
        {
            // Arrange
            var systemInformation = new SystemInformation();
            IRESTServiceConfiguration serviceConfiguration = null;

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(serviceConfiguration);

            var restClientFactory = new Mock<IRESTClientFactory>();
            var requestFactory = new Mock<IRESTRequestFactory>();

            var systemInformationSender = new RESTBasedSystemInformationSender(systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        [Test]
        [ExpectedException(typeof(SystemInformationSenderSetupException))]
        public void Send_ServiceConfigurationIsInvalid_SystemInformationSenderSetupExceptionIsThrown()
        {
            // Arrange
            var systemInformation = new SystemInformation();
            var serviceConfiguration = new Mock<IRESTServiceConfiguration>();
            serviceConfiguration.Setup(s => s.IsValid()).Returns(false);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(serviceConfiguration.Object);

            var restClientFactory = new Mock<IRESTClientFactory>();
            var requestFactory = new Mock<IRESTRequestFactory>();

            var systemInformationSender = new RESTBasedSystemInformationSender(systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        [Test]
        [ExpectedException(typeof(SystemInformationSenderSetupException))]
        public void Send_RESTClientCannotBeInstantiated_ObjectIsInstantiated()
        {
            // Arrange
            var systemInformation = new SystemInformation();
            var configuration = new Mock<IRESTServiceConfiguration>();
            configuration.Setup(c => c.BaseUrl).Returns("http://localhost");
            configuration.Setup(c => c.ResourcePath).Returns("api/systeminformation");
            configuration.Setup(c => c.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(configuration.Object);

            IRestClient client = null;
            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(client);

            var requestFactory = new Mock<IRESTRequestFactory>();

            var systemInformationSender = new RESTBasedSystemInformationSender(systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        [Test]
        [ExpectedException(typeof(FatalSystemInformationSenderException))]
        public void Send_RequestCannotBeCreated_FatalSystemInformationSenderExceptionIsThrown()
        {
            // Arrange
            var systemInformation = new SystemInformation();

            var configuration = new Mock<IRESTServiceConfiguration>();
            configuration.Setup(c => c.BaseUrl).Returns("http://localhost");
            configuration.Setup(c => c.ResourcePath).Returns("api/systeminformation");
            configuration.Setup(c => c.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(configuration.Object);

            var client = new Mock<IRestClient>();
            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(client.Object);

            IRestRequest request = null;
            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(r => r.CreatePutRequest(It.IsAny<string>())).Returns(request);

            var systemInformationSender = new RESTBasedSystemInformationSender(
                systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        [Test]
        [ExpectedException(typeof(SendSystemInformationFailedException))]
        public void Send_ResponseContainsException_SendSystemInformationFailedExceptionIsThrown()
        {
            // Arrange
            var systemInformation = new SystemInformation();

            var configuration = new Mock<IRESTServiceConfiguration>();
            configuration.Setup(c => c.BaseUrl).Returns("http://localhost");
            configuration.Setup(c => c.ResourcePath).Returns("api/systeminformation");
            configuration.Setup(c => c.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(configuration.Object);

            var response = new Mock<IRestResponse<SystemInformation>>();
            response.Setup(r => r.ErrorException).Returns(new Exception("Some exception"));

            var client = new Mock<IRestClient>();
            client.Setup(c => c.Execute<SystemInformation>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(client.Object);

            var request = new Mock<IRestRequest>();
            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(r => r.CreatePutRequest(It.IsAny<string>())).Returns(request.Object);

            var systemInformationSender = new RESTBasedSystemInformationSender(
                systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        [Test]
        public void Send_ResponseContainsNoException()
        {
            // Arrange
            var systemInformation = new SystemInformation();

            var configuration = new Mock<IRESTServiceConfiguration>();
            configuration.Setup(c => c.BaseUrl).Returns("http://localhost");
            configuration.Setup(c => c.ResourcePath).Returns("api/systeminformation");
            configuration.Setup(c => c.IsValid()).Returns(true);

            var systemInformationSenderConfigurationProvider =
                new Mock<IRESTBasedSystemInformationSenderConfigurationProvider>();

            systemInformationSenderConfigurationProvider.Setup(s => s.GetConfiguration()).Returns(configuration.Object);

            var response = new Mock<IRestResponse<SystemInformation>>();

            var client = new Mock<IRestClient>();
            client.Setup(c => c.Execute<SystemInformation>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(client.Object);

            var request = new Mock<IRestRequest>();
            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(r => r.CreatePutRequest(It.IsAny<string>())).Returns(request.Object);

            var systemInformationSender = new RESTBasedSystemInformationSender(
                systemInformationSenderConfigurationProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            systemInformationSender.Send(systemInformation);
        }

        #endregion
    }
}