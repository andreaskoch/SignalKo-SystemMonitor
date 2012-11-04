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
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			// Act
			var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentControlDefinitionProvider.Object);

			// Assert
			Assert.IsNotNull(systemInformationSenderConfigurationProvider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentControlDefinitionProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			new RESTBasedSystemInformationSenderConfigurationProvider(null);
		}

		#endregion

		#region GetConfiguration

		[Test]
		public void GetConfiguration_AgentControlDefinitionProviderReturnsNull_ResultIsNull()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentControlDefinitionProvider.Object);

			// Act
			var result = systemInformationSenderConfigurationProvider.GetConfiguration();

			// Assert
			Assert.IsNull(result);
		}

		[Test]
		public void GetConfiguration_AgentControlDefinitionProviderReturnsAgentConfiguration_ResultIsNotNull()
		{
			// Arrange
			var agentControlDefinition = new AgentControlDefinition
				{
					AgentIsEnabled = true,
					Hostaddress = "127.0.0.1",
					Hostname = "www.example.com",
					CheckIntervalInSeconds = 30,
					SystemInformationSenderPath = "/api/systeminformation"
				};

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentControlDefinitionProvider.Object);

			// Act
			var result = systemInformationSenderConfigurationProvider.GetConfiguration();

			// Assert
			Assert.IsNotNull(result);
		}

		[Test]
		public void GetConfiguration_ResultContainsAgentControlDefinitionHostaddress()
		{
			// Arrange
			string hostaddress = "127.0.0.1";
			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = hostaddress,
				Hostname = "www.example.com",
				CheckIntervalInSeconds = 30,
				SystemInformationSenderPath = "/api/systeminformation"
			};

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentControlDefinitionProvider.Object);

			// Act
			var result = systemInformationSenderConfigurationProvider.GetConfiguration();

			// Assert
			Assert.AreEqual(hostaddress, result.Hostaddress);
		}

		[Test]
		public void GetConfiguration_ResultContainsAgenControlDefinitionHostname()
		{
			// Arrange
			string hostname = "www.example.com";
			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = hostname,
				CheckIntervalInSeconds = 30,
				SystemInformationSenderPath = "/api/systeminformation"
			};

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentControlDefinitionProvider.Object);

			// Act
			var result = systemInformationSenderConfigurationProvider.GetConfiguration();

			// Assert
			Assert.AreEqual(hostname, result.Hostname);
		}

		[Test]
		public void GetConfiguration_ResultContainsAgentControlDefinitionSystemInformationSenderPath()
		{
			// Arrange
			string systemInformationSenderPath = "/api/systeminformation";
			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = "www.example.com",
				CheckIntervalInSeconds = 30,
				SystemInformationSenderPath = systemInformationSenderPath
			};

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var systemInformationSenderConfigurationProvider = new RESTBasedSystemInformationSenderConfigurationProvider(agentControlDefinitionProvider.Object);

			// Act
			var result = systemInformationSenderConfigurationProvider.GetConfiguration();

			// Assert
			Assert.AreEqual(systemInformationSenderPath, result.ResourcePath);
		}

		#endregion
	}
}