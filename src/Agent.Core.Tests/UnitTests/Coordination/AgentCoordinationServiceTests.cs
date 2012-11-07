using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Coordination;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Coordination
{
	[TestFixture]
	public class AgentCoordinationServiceTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			// Act
			var agentCoordinationService = new AgentCoordinationService(agentControlDefinitionProvider.Object, () => { }, () => { });

			// Assert
			Assert.IsNotNull(agentCoordinationService);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentControlDefinitionProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Act
			new AgentCoordinationService(null, pauseCallback: () => { }, resumeCallback: () => { });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_PauseCallbackParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			// Act
			new AgentCoordinationService(agentControlDefinitionProvider.Object, pauseCallback: null, resumeCallback: () => { });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ResumeCallbackParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			// Act
			new AgentCoordinationService(agentControlDefinitionProvider.Object, pauseCallback: () => { }, resumeCallback: null);
		}

		#endregion

		#region Start

		[Test]
		public void Start_GetControlDefinitionReturnsNull_PauseCallbackIsCalled()
		{
			// Arrange
			bool pauseCallbackWasCalled = false;
			var runDurationInMilliseconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds * 1;
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			AgentControlDefinition agentControlDefinition = null;
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var agentCoordinationService = new AgentCoordinationService(
				agentControlDefinitionProvider.Object,
				pauseCallback: () =>
					{
						pauseCallbackWasCalled = true;
					},
				resumeCallback: () => { });

			// Act
			var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
			agentCoordinationServiceTask.Start();

			Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

			// Assert
			Assert.IsTrue(pauseCallbackWasCalled);
		}

		[Test]
		public void Start_AgentsAreDisabled_PauseCallbackIsCalled()
		{
			// Arrange
			bool pauseCallbackWasCalled = false;
			var runDurationInMilliseconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds * 1;
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			var agentControlDefinition = new AgentControlDefinition
				{
					AgentIsEnabled = false,
					Hostaddress = "127.0.0.1",
					Hostname = "www.example.com",
					CheckIntervalInSeconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds,
					SystemInformationSenderPath = "/api/systeminformation"
				};
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var agentCoordinationService = new AgentCoordinationService(
				agentControlDefinitionProvider.Object,
				pauseCallback: () =>
				{
					pauseCallbackWasCalled = true;
				},
				resumeCallback: () => { });

			// Act
			var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
			agentCoordinationServiceTask.Start();

			Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

			// Assert
			Assert.IsTrue(pauseCallbackWasCalled);
		}

		[Test]
		public void Start_AgentsAreEnabled_ResumeCallbackIsCalled()
		{
			// Arrange
			bool resumeCallbackWasCalled = false;
			var runDurationInMilliseconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds * 1;
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = "www.example.com",
				CheckIntervalInSeconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds,
				SystemInformationSenderPath = "/api/systeminformation"
			};
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var agentCoordinationService = new AgentCoordinationService(
				agentControlDefinitionProvider.Object,
				pauseCallback: () => { },
				resumeCallback: () =>
				{
					resumeCallbackWasCalled = true;
				});

			// Act
			var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
			agentCoordinationServiceTask.Start();

			Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

			// Assert
			Assert.IsTrue(resumeCallbackWasCalled);
		}

		[Test]
		public void Start_RunsForThreeIntervals_GetControlDefinitionIsCalledThreeTimes()
		{
			// Arrange
			var runDurationInMilliseconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds * 3;
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();
			var agentControlDefinition = new AgentControlDefinition
				{
					AgentIsEnabled = true,
					Hostaddress = "127.0.0.1",
					Hostname = "www.example.com",
					CheckIntervalInSeconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds,
					SystemInformationSenderPath = "/api/systeminformation"
				};
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var agentCoordinationService = new AgentCoordinationService(agentControlDefinitionProvider.Object, () => { }, () => { });

			// Act
			var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
			agentCoordinationServiceTask.Start();

			Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

			// Assert
			agentControlDefinitionProvider.Verify(a => a.GetControlDefinition(), Times.Between(3, 4, Range.Inclusive));
		}

		#endregion

		#region Stop

		[Test]
		public void Stop_EndsTheService()
		{
			// Arrange
			var maxDurationInMilliseconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds * 3;
			var agentConfigurationProvider = new Mock<IAgentControlDefinitionProvider>();

			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = "www.example.com",
				CheckIntervalInSeconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds,
				SystemInformationSenderPath = "/api/systeminformation"
			};
			agentConfigurationProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var agentCoordinationService = new AgentCoordinationService(agentConfigurationProvider.Object, () => { }, () => { });

			// Act
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
			agentCoordinationServiceTask.Start();
			Thread.Sleep(500);
			agentCoordinationService.Stop();
			Task.WaitAll(new[] { agentCoordinationServiceTask }, maxDurationInMilliseconds);

			stopwatch.Stop();

			// Assert
			Assert.Less(stopwatch.ElapsedMilliseconds, maxDurationInMilliseconds);
		}

		#endregion

		#region Stop

		[Test]
		public void Dispose_EndsTheService()
		{
			// Arrange
			var maxDurationInMilliseconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds * 3;
			var agentControlDefinitionProvider = new Mock<IAgentControlDefinitionProvider>();

			var agentControlDefinition = new AgentControlDefinition
			{
				AgentIsEnabled = true,
				Hostaddress = "127.0.0.1",
				Hostname = "www.example.com",
				CheckIntervalInSeconds = AgentCoordinationService.AgentControlDefinitionCheckIntervalInMilliseconds,
				SystemInformationSenderPath = "/api/systeminformation"
			};
			agentControlDefinitionProvider.Setup(a => a.GetControlDefinition()).Returns(agentControlDefinition);

			var agentCoordinationService = new AgentCoordinationService(agentControlDefinitionProvider.Object, () => { }, () => { });

			// Act
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
			agentCoordinationServiceTask.Start();
			Thread.Sleep(500);
			agentCoordinationService.Dispose();
			Task.WaitAll(new[] { agentCoordinationServiceTask }, maxDurationInMilliseconds);

			stopwatch.Stop();

			// Assert
			Assert.Less(stopwatch.ElapsedMilliseconds, maxDurationInMilliseconds);
		}

		#endregion
	}
}