using System;
using System.Threading;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Configuration
{
	[TestFixture]
	public class AgentControlDefinitionProviderTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();

			// Act
			var agentControlDefinitionProvider = new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);

			// Assert
			Assert.IsNotNull(agentControlDefinitionProvider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentControlDefinitionAccessorParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Act
			new AgentControlDefinitionProvider(null);
		}

		#endregion

		#region Timer

		[Test]
		public void Timer_GetControlDefinition_IsCalled_On_AgentConfigurationAccessor_FromWithinTheConstructor()
		{
			// Arrange
			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();

			// Act
			new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);

			// Assert
			agentControlDefinitionAccessor.Verify(a => a.GetControlDefinition(), Times.Once());
		}

		[Test]
		public void Timer_GetControlDefinition_IsCalled_On_AgentConfigurationAccessor_TwiceDuringTheDefaultInterval()
		{
			// Arrange
			var slackTimeInMilliseconds = 2000;
			var waitTimeInMilliseconds = (AgentControlDefinitionProvider.DefaultCheckIntervalInSeconds * 1000) + slackTimeInMilliseconds;

			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();

			// Act
			new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);
			Thread.Sleep(waitTimeInMilliseconds);

			// Assert
			agentControlDefinitionAccessor.Verify(a => a.GetControlDefinition(), Times.Exactly(2));
		}

		[Test]
		public void Timer_IntervalIsChangedToOnceSecond_GetControlDefinitionIsCalledEverySecond()
		{
			// Arrange
			int newCheckIntervalInSeconds = 1;
			int waitTimeInSeconds = AgentControlDefinitionProvider.DefaultCheckIntervalInSeconds;
			int expectedNumberOfTimesGetControlDefinitionIsCalled = (int)((double)waitTimeInSeconds / newCheckIntervalInSeconds);

			int waitTimeInMilliseconds = waitTimeInSeconds * 1000;

			var agentControlDefinitionWithReducedInterval = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = "www.example.com",
				CheckIntervalInSeconds = newCheckIntervalInSeconds,
				SystemInformationSenderPath = "/api/systeminformation"
			};

			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();
			agentControlDefinitionAccessor.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinitionWithReducedInterval);

			// Act
			var agentControlDefinitionProvider = new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);
			Thread.Sleep(waitTimeInMilliseconds);

			// Assert
			agentControlDefinitionAccessor.Verify(a => a.GetControlDefinition(), Times.AtLeast(expectedNumberOfTimesGetControlDefinitionIsCalled));
		}

		#endregion

		#region GetControlDefinition

		[Test]
		public void GetControlDefinition_ReturnsNullIfTheAgentControlDefinitionAccessorReturnsNull()
		{
			// Arrange
			AgentControlDefinition agentControlDefinition = null;
			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();
			agentControlDefinitionAccessor.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);
			var agentControlDefinitionProvider = new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);
			Thread.Sleep(2000);

			// Act
			var result = agentControlDefinitionProvider.GetControlDefinition();

			// Assert
			Assert.IsNull(result);
		}

		[Test]
		public void GetControlDefinition_ReturnsTheSameAgentControlDefinitionThatIsReturnedByTheAgentControlDefinitionAccessor()
		{
			// Arrange
			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = "www.example.com",
				CheckIntervalInSeconds = 10,
				SystemInformationSenderPath = "/api/systeminformation"
			};
			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();
			agentControlDefinitionAccessor.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);
			var agentControlDefinitionProvider = new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);
			Thread.Sleep(2000);

			// Act
			var result = agentControlDefinitionProvider.GetControlDefinition();

			// Assert
			Assert.AreEqual(agentControlDefinition, result);
		}

		#endregion

		#region Dispose

		[Test]
		public void Dispose_TimerIsNotExecutedAfterDisposeHasBeenCalled()
		{
			// Arrange
			int newCheckIntervalInSeconds = 1;
			int waitTimeInMilliseconds = (newCheckIntervalInSeconds * 1000) * 5;
			var agentControlDefinitionWithReducedInterval = new AgentControlDefinition
				{
					AgentIsEnabled = true,
					Hostaddress = "127.0.0.1",
					Hostname = "www.example.com",
					CheckIntervalInSeconds = newCheckIntervalInSeconds,
					SystemInformationSenderPath = "/api/systeminformation"
				};

			var agentControlDefinitionAccessor = new Mock<IAgentControlDefinitionAccessor>();
			agentControlDefinitionAccessor.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinitionWithReducedInterval);

			var agentControlDefinitionProvider = new AgentControlDefinitionProvider(agentControlDefinitionAccessor.Object);

			// Act
			agentControlDefinitionProvider.Dispose();
			Thread.Sleep(waitTimeInMilliseconds);

			// Assert
			agentControlDefinitionAccessor.Verify(a => a.GetControlDefinition(), Times.AtMostOnce());
		}

		#endregion
	}
}