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
	public class AgentControlDefinitionAccessorTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			var restClientFactory = new Mock<IRESTClientFactory>();
			var requestFactory = new Mock<IRESTRequestFactory>();

			// Act
			var agentControlDefinitionAccessor = new AgentControlDefinitionAccessor(
				configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

			// Assert
			Assert.IsNotNull(agentControlDefinitionAccessor);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentControlDefinitionServiceUrlProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var restClientFactory = new Mock<IRESTClientFactory>();
			var requestFactory = new Mock<IRESTRequestFactory>();

			// Act
			new AgentControlDefinitionAccessor(null, restClientFactory.Object, requestFactory.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_RESTClientFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			var requestFactory = new Mock<IRESTRequestFactory>();

			// Act
			new AgentControlDefinitionAccessor(configurationServiceUrlProvider.Object, null, requestFactory.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_RESTRequestFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			var restClientFactory = new Mock<IRESTClientFactory>();

			// Act
			new AgentControlDefinitionAccessor(configurationServiceUrlProvider.Object, restClientFactory.Object, null);
		}

		#endregion

		#region GetControlDefinition

		[Test]
		public void GetControlDefinition_GetServiceConfiguration_IsCalled_On_AgentControlDefinitionServiceUrlProvider()
		{
			// Arrange
			var serviceConfiguration = new AgentControlDefinitionServiceConfiguration { Hostaddress = "127.0.0.0", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };

			var response = new Mock<IRestResponse<AgentControlDefinition>>();
			var restClient = new Mock<IRestClient>();
			restClient.Setup(c => c.Execute<AgentControlDefinition>(It.IsAny<IRestRequest>())).Returns(response.Object);
			var request = new Mock<IRestRequest>();

			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

			var restClientFactory = new Mock<IRESTClientFactory>();
			restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

			var requestFactory = new Mock<IRESTRequestFactory>();
			requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

			var agentControlDefinitionAccessor = new AgentControlDefinitionAccessor(
				configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

			// Act
			agentControlDefinitionAccessor.GetControlDefinition();

			// Assert
			configurationServiceUrlProvider.Verify(c => c.GetServiceConfiguration(), Times.Once());
		}

		[Test]
		public void GetControlDefinition_GetRESTClient_IsCalled_On_RestClientFactory()
		{
			// Arrange
			var serviceConfiguration = new AgentControlDefinitionServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };

			var response = new Mock<IRestResponse<AgentControlDefinition>>();
			var restClient = new Mock<IRestClient>();
			restClient.Setup(c => c.Execute<AgentControlDefinition>(It.IsAny<IRestRequest>())).Returns(response.Object);
			var request = new Mock<IRestRequest>();

			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

			var restClientFactory = new Mock<IRESTClientFactory>();
			restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

			var requestFactory = new Mock<IRESTRequestFactory>();
			requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

			var agentControlDefinitionAccessor = new AgentControlDefinitionAccessor(
				configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

			// Act
			agentControlDefinitionAccessor.GetControlDefinition();

			// Assert
			restClientFactory.Verify(c => c.GetRESTClient(It.IsAny<string>()), Times.Once());
		}

		[Test]
		public void GetControlDefinition_CreateGetRequest_IsCalled_On_RequestFactory()
		{
			// Arrange
			var serviceConfiguration = new AgentControlDefinitionServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };
			var response = new Mock<IRestResponse<AgentControlDefinition>>();
			var restClient = new Mock<IRestClient>();
			restClient.Setup(c => c.Execute<AgentControlDefinition>(It.IsAny<IRestRequest>())).Returns(response.Object);
			var request = new Mock<IRestRequest>();

			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

			var restClientFactory = new Mock<IRESTClientFactory>();
			restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

			var requestFactory = new Mock<IRESTRequestFactory>();
			requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

			var agentControlDefinitionAccessor = new AgentControlDefinitionAccessor(
				configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

			// Act
			agentControlDefinitionAccessor.GetControlDefinition();

			// Assert
			requestFactory.Verify(c => c.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
		}

		[Test]
		public void GetControlDefinition_Execute_IsCalled_On_RestClient_with_RequestObject()
		{
			// Arrange
			var serviceConfiguration = new AgentControlDefinitionServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };
			var response = new Mock<IRestResponse<AgentControlDefinition>>();
			var restClient = new Mock<IRestClient>();
			restClient.Setup(c => c.Execute<AgentControlDefinition>(It.IsAny<IRestRequest>())).Returns(response.Object);
			var request = new Mock<IRestRequest>();

			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

			var restClientFactory = new Mock<IRESTClientFactory>();
			restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

			var requestFactory = new Mock<IRESTRequestFactory>();
			requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

			var agentControlDefinitionAccessor = new AgentControlDefinitionAccessor(
				configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

			// Act
			agentControlDefinitionAccessor.GetControlDefinition();

			// Assert
			restClient.Verify(c => c.Execute<AgentControlDefinition>(request.Object), Times.Once());
		}

		[Test]
		public void GetControlDefinition_ResponseData_IsReturned()
		{
			// Arrange
			var serviceConfiguration = new AgentControlDefinitionServiceConfiguration { Hostaddress = "127.0.0.0:8181", Hostname = "localhost", ResourcePath = "/api/agentconfiguration" };

			var responseData = new AgentControlDefinition
				{
					AgentIsEnabled = true,
					Hostaddress = "127.0.0.1",
					Hostname = "www.example.com",
					CheckIntervalInSeconds = 1,
					SystemInformationSenderPath = Guid.NewGuid().ToString()
				};

			var response = new Mock<IRestResponse<AgentControlDefinition>>();
			response.Setup(r => r.Data).Returns(responseData);

			var restClient = new Mock<IRestClient>();
			restClient.Setup(c => c.Execute<AgentControlDefinition>(It.IsAny<IRestRequest>())).Returns(response.Object);
			var request = new Mock<IRestRequest>();

			var configurationServiceUrlProvider = new Mock<IAgentControlDefinitionServiceUrlProvider>();
			configurationServiceUrlProvider.Setup(c => c.GetServiceConfiguration()).Returns(serviceConfiguration);

			var restClientFactory = new Mock<IRESTClientFactory>();
			restClientFactory.Setup(r => r.GetRESTClient(It.IsAny<string>())).Returns(restClient.Object);

			var requestFactory = new Mock<IRESTRequestFactory>();
			requestFactory.Setup(f => f.CreateGetRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(request.Object);

			var agentControlDefinitionAccessor = new AgentControlDefinitionAccessor(
				configurationServiceUrlProvider.Object, restClientFactory.Object, requestFactory.Object);

			// Act
			var result = agentControlDefinitionAccessor.GetControlDefinition();

			// Assert
			Assert.AreEqual(responseData, result);
		}

		#endregion
	}
}