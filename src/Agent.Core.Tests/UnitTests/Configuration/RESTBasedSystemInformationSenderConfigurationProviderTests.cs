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
                    Hostaddress = "127.0.0.1",
                    Hostname = "www.example.com",
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
        public void GetConfiguration_ResultContainsAgentConfigurationHostaddress()
        {
            // Arrange
            string hostaddress = "127.0.0.1";
            var agentConfiguration = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                Hostaddress = hostaddress,
                Hostname = "www.example.com",
                CheckIntervalInSeconds = 30,
                SystemInformationSenderPath = "/api/systeminformation"
            };

            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Act
            var result = systemInformationSenderConfigurationProvider.GetConfiguration();

            // Assert
            Assert.AreEqual(hostaddress, result.Hostaddress);
        }

        [Test]
        public void GetConfiguration_ResultContainsAgentConfigurationHostname()
        {
            // Arrange
            string hostname = "www.example.com";
            var agentConfiguration = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                Hostaddress = "127.0.0.1",
                Hostname = hostname,
                CheckIntervalInSeconds = 30,
                SystemInformationSenderPath = "/api/systeminformation"
            };

            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentConfigurationProvider.Object);

            // Act
            var result = systemInformationSenderConfigurationProvider.GetConfiguration();

            // Assert
            Assert.AreEqual(hostname, result.Hostname);
        }

        [Test]
        public void GetConfiguration_ResultContainsAgentConfigurationSystemInformationSenderPath()
        {
            // Arrange
            string systemInformationSenderPath = "/api/systeminformation";
            var agentConfiguration = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                Hostaddress = "127.0.0.1",
                Hostname = "www.example.com",
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