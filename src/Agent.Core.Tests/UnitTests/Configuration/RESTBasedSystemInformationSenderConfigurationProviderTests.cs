using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Configuration
{
    [TestFixture]
    public class RESTBasedSystemInformationSenderConfigurationProviderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            // Act
            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Assert
            Assert.IsNotNull(systemInformationSenderConfigurationProvider);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AgentConfigurationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            new RESTBasedSystemInformationSenderConfigurationProvider(null);
        }

        #endregion

        #region GetConfiguration

        [Test]
        public void GetConfiguration_AgentConfigurationProviderReturnsNull_ResultIsNull()
        {
            // Arrange
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Act
            var result = systemInformationSenderConfigurationProvider.GetConfiguration();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetConfiguration_AgentConfigurationProviderReturnsAgentConfiguration_ResultIsNotNull()
        {
            // Arrange
            var agentConfiguration = new AgentConfiguration
                {
                    AgentsAreEnabled = true,
                    BaseUrl = "http://www.example.com",
                    CheckIntervalInSeconds = 30,
                    SystemInformationSenderPath = "/api/systeminformation"
                };

            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Act
            var result = systemInformationSenderConfigurationProvider.GetConfiguration();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetConfiguration_ResultContainsAgentConfigurationBaseUrl()
        {
            // Arrange
            string baseUrl = "http://www.example.com";
            var agentConfiguration = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                BaseUrl = baseUrl,
                CheckIntervalInSeconds = 30,
                SystemInformationSenderPath = "/api/systeminformation"
            };

            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Act
            var result = systemInformationSenderConfigurationProvider.GetConfiguration();

            // Assert
            Assert.AreEqual(baseUrl, result.BaseUrl);
        }

        [Test]
        public void GetConfiguration_ResultContainsAgentConfigurationSystemInformationSenderPath()
        {
            // Arrange
            string systemInformationSenderPath = "/api/systeminformation";
            var agentConfiguration = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                BaseUrl = "http://www.example.com",
                CheckIntervalInSeconds = 30,
                SystemInformationSenderPath = systemInformationSenderPath
            };

            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Act
            var result = systemInformationSenderConfigurationProvider.GetConfiguration();

            // Assert
            Assert.AreEqual(systemInformationSenderPath, result.ResourcePath);
        }

        #endregion
    }
}