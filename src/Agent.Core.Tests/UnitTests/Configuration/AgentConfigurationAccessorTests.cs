using System;

using Moq;

using NUnit.Framework;

using RestSharp;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Configuration
{
    [TestFixture]
    public class AgentConfigurationAccessorTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            var restClientFactory = new Mock<IRESTClientFactory>();
            var requestFactory = new Mock<IRESTRequestFactory>();

            // Act
            var agentConfigurationAccessor = new AgentConfigurationAccessor(
                configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Assert
            Assert.IsNotNull(agentConfigurationAccessor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AgentConfigurationServiceUrlProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var restClientFactory = new Mock<IRESTClientFactory>();
            var requestFactory = new Mock<IRESTRequestFactory>();

            // Act
            new AgentConfigurationAccessor(null, restClientFactory.Object, requestFactory.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RESTClientFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            var requestFactory = new Mock<IRESTRequestFactory>();

            // Act
            new AgentConfigurationAccessor(configurationServiceUrlProvider.Object, null, requestFactory.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RESTRequestFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            var restClientFactory = new Mock<IRESTClientFactory>();

            // Act
            new AgentConfigurationAccessor(configurationServiceUrlProvider.Object, restClientFactory.Object, null);
        }

        #endregion

        #region GetAgentConfiguration

        [Test]
        public void GetAgentConfiguration_GetServiceConfiguration_IsCalled_On_AgentConfigurationServiceUrlProvider()
        {
            // Arrange
            var serviceConfiguration = new AgentConfigurationServiceConfiguration { Hostaddress = "127.0.0.0", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };

            var response = new Mock<IRestResponse<AgentConfiguration>>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute<AgentConfiguration>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var request = new Mock<IRestRequest>();

            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

            var agentConfigurationAccessor = new AgentConfigurationAccessor(
                configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            agentConfigurationAccessor.GetAgentConfiguration();

            // Assert
            configurationServiceUrlProvider.Verify(c => c.GetServiceConfiguration(), Times.Once());
        }

        [Test]
        public void GetAgentConfiguration_GetRESTClient_IsCalled_On_RestClientFactory()
        {
            // Arrange
            var serviceConfiguration = new AgentConfigurationServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };

            var response = new Mock<IRestResponse<AgentConfiguration>>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute<AgentConfiguration>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var request = new Mock<IRestRequest>();

            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

            var agentConfigurationAccessor = new AgentConfigurationAccessor(
                configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            agentConfigurationAccessor.GetAgentConfiguration();

            // Assert
            restClientFactory.Verify(c => c.GetRESTClient(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void GetAgentConfiguration_CreateGetRequest_IsCalled_On_RequestFactory()
        {
            // Arrange
            var serviceConfiguration = new AgentConfigurationServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };
            var response = new Mock<IRestResponse<AgentConfiguration>>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute<AgentConfiguration>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var request = new Mock<IRestRequest>();

            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

            var agentConfigurationAccessor = new AgentConfigurationAccessor(
                configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            agentConfigurationAccessor.GetAgentConfiguration();

            // Assert
            requestFactory.Verify(c => c.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void GetAgentConfiguration_Execute_IsCalled_On_RestClient_with_RequestObject()
        {
            // Arrange
            var serviceConfiguration = new AgentConfigurationServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };
            var response = new Mock<IRestResponse<AgentConfiguration>>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute<AgentConfiguration>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var request = new Mock<IRestRequest>();

            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

            var agentConfigurationAccessor = new AgentConfigurationAccessor(
                configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            agentConfigurationAccessor.GetAgentConfiguration();

            // Assert
            restClient.Verify(c => c.Execute<AgentConfiguration>(request.Object), Times.Once());
        }

        [Test]
        public void GetAgentConfiguration_ResponseData_IsReturned()
        {
            // Arrange
            var serviceConfiguration = new AgentConfigurationServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };

            var responseData = new AgentConfiguration
                {
                    AgentsAreEnabled = true,
                    Hostaddress = "127.0.0.1",
                    Hostname = "www.example.com",
                    CheckIntervalInSeconds = 1,
                    SystemInformationSenderPath = Guid.NewGuid().ToString()
                };

            var response = new Mock<IRestResponse<AgentConfiguration>>();
            response.Setup(r => r.Data).Returns(responseData);

            var restClient = new Mock<IRestClient>();
            restClient.Setup(c => c.Execute<AgentConfiguration>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var request = new Mock<IRestRequest>();

            var configurationServiceUrlProvider = new Mock<IAgentConfigurationServiceUrlProvider>();
            configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

            var restClientFactory = new Mock<IRESTClientFactory>();
            restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

            var requestFactory = new Mock<IRESTRequestFactory>();
            requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

            var agentConfigurationAccessor = new AgentConfigurationAccessor(
                configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

            // Act
            var result = agentConfigurationAccessor.GetAgentConfiguration();

            // Assert
            Assert.AreEqual(responseData, result);
        }

        #endregion
    }
}