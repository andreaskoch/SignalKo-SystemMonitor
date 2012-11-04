using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Coordination;

namespace Agent.Core.Tests.UnitTests.Coordination
{
	[TestFixture]
	public class AgentCoordinationServiceFactoryTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			// Act
			var agentCoordinationServiceFactory = new AgentCoordinationServiceFactory(agentControlDefinitionProvider.Object);

			// Assert
			Assert.IsNotNull(agentCoordinationServiceFactory);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentConfigurationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Act
			new AgentCoordinationServiceFactory(null);
		}

		#endregion

		#region GetAgentCoordinationService

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetAgentCoordinationService_PauseCallbackIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			Action pauseCallback = null;
			Action resumeCallback = () => { };

			var agentConfigurationProvider = new Mock<IAgentControlDefinitionProvider>();
			var agentCoordinationServiceFactory = new AgentCoordinationServiceFactory(agentConfigurationProvider.Object);

			// Act
			agentCoordinationServiceFactory.GetAgentCoordinationService(pauseCallback, resumeCallback);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetAgentCoordinationService_ResumeCallbackIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			Action pauseCallback = () => { };
			Action resumeCallback = null;

			var agentConfigurationProvider = new Mock<IAgentControlDefinitionProvider>();
			var agentCoordinationServiceFactory = new AgentCoordinationServiceFactory(agentConfigurationProvider.Object);

			// Act
			agentCoordinationServiceFactory.GetAgentCoordinationService(pauseCallback, resumeCallback);
		}

		#endregion
	}
}