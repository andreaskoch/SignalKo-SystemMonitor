using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Coordination;

namespace Agent.Core.Tests.IntegrationTests.Coordination
{
	[TestFixture]
	public class AgentCoordinationServiceFactoryTests
	{
		#region GetAgentCoordinationService

		[Test]
		public void GetAgentCoordinationService_CallbacksAreSet_ResultIsNotNull()
		{
			// Arrange
			Action pauseCallback = () => { };
			Action resumeCallback = () => { };

			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var agentCoordinationServiceFactory = new AgentCoordinationServiceFactory(agentControlDefinitionProvider.Object);

			// Act
			var result = agentCoordinationServiceFactory.GetAgentCoordinationService(pauseCallback, resumeCallback);

			// Assert
			Assert.IsNotNull(result);
		}

		#endregion
	}
}