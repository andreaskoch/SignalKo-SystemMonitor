using System;
using System.Diagnostics;
using System.Threading;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Core.Coordination;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests
{
	[TestFixture]
	public class SystemInformationDispatchingServiceTests
	{
		#region constructor

		[Test]
		public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
		{
			// Arrange
			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			// Act
			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Assert
			Assert.IsNotNull(systemInformationDispatchingService);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_AgentCoordinationServiceFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			// Act
			new SystemInformationDispatchingService(
				null,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_MessageQueueFeederFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			// Act
			new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				null,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_MessageQueueWorkerFactoryParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			// Act
			new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				null,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_MessageQueueProviderParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			// Act
			new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				null,
				messageQueuePersistence.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_MessageQueuePersistenceParameterIsNull_ArgumentNullExceptionIsThrown()
		{
			// Arrange
			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();

			// Act
			new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				null);
		}

		#endregion

		#region Start

		[Test]
		public void Start_AgentCoordinationService_StartIsCalled()
		{
			// Arrange
			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
			var messageQueueWorker = new Mock<IMessageQueueWorker>();

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder.Object);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker.Object);

			var workQueue = new Mock<IMessageQueue<SystemInformation>>();
			var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			messageQueueProvider.Setup(m => m.WorkQueue).Returns(workQueue.Object);
			messageQueueProvider.Setup(m => m.ErrorQueue).Returns(errorQueue.Object);

			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			systemInformationDispatchingService.Start();

			// Assert
			agentCoordinationService.Verify(f => f.Start(), Times.Once());
		}

		[Test]
		public void Start_MessageQueueFeeder_StartIsCalled()
		{
			// Arrange
			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
			var messageQueueWorker = new Mock<IMessageQueueWorker>();

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder.Object);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker.Object);

			var workQueue = new Mock<IMessageQueue<SystemInformation>>();
			var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			messageQueueProvider.Setup(m => m.WorkQueue).Returns(workQueue.Object);
			messageQueueProvider.Setup(m => m.ErrorQueue).Returns(errorQueue.Object);

			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			systemInformationDispatchingService.Start();

			// Assert
			messageQueueFeeder.Verify(f => f.Start(), Times.Once());
		}

		[Test]
		public void Start_MessageQueueWorker_StartIsCalled()
		{
			// Arrange
			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
			var messageQueueWorker = new Mock<IMessageQueueWorker>();

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder.Object);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker.Object);

			var workQueue = new Mock<IMessageQueue<SystemInformation>>();
			var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			messageQueueProvider.Setup(m => m.WorkQueue).Returns(workQueue.Object);
			messageQueueProvider.Setup(m => m.ErrorQueue).Returns(errorQueue.Object);

			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			systemInformationDispatchingService.Start();

			// Assert
			messageQueueWorker.Verify(f => f.Start(), Times.Once());
		}

		#endregion

		#region Stop

		[Test]
		public void Stop_AgentCoordinationService_StopIsCalled()
		{
			// Arrange
			// Arrange
			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
			var messageQueueWorker = new Mock<IMessageQueueWorker>();

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder.Object);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker.Object);

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			systemInformationDispatchingService.Stop();

			// Assert
			agentCoordinationService.Verify(f => f.Stop(), Times.Once());
		}

		[Test]
		public void Stop_MessageQueueFeeder_StopIsCalled()
		{
			// Arrange
			// Arrange
			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
			var messageQueueWorker = new Mock<IMessageQueueWorker>();

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder.Object);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker.Object);

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			systemInformationDispatchingService.Stop();

			// Assert
			messageQueueFeeder.Verify(f => f.Stop(), Times.Once());
		}

		[Test]
		public void Stop_MessageQueueWorker_StopIsCalled()
		{
			// Arrange
			// Arrange
			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
			var messageQueueWorker = new Mock<IMessageQueueWorker>();

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder.Object);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker.Object);

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			systemInformationDispatchingService.Stop();

			// Assert
			messageQueueWorker.Verify(f => f.Stop(), Times.Once());
		}

		#endregion

		#region parallel

		[Test]
		public void Start_MethodDoesNotEndbeforeTheFeederAndWorkerHaveExited()
		{
			// Arrange
			var durationTheWorkerIsRunning = 1000;
			var durationTheFeederIsRunning = 500;

			var agentCoordinationService = new Mock<IAgentCoordinationService>();
			var messageQueueFeeder = new WaitingFeeder(durationTheFeederIsRunning);
			var messageQueueWorker = new WaitingWorker(durationTheWorkerIsRunning);

			var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
			agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(agentCoordinationService.Object);

			var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
			messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder);

			var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
			messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker);

			var workQueue = new Mock<IMessageQueue<SystemInformation>>();
			var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

			var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
			messageQueueProvider.Setup(m => m.WorkQueue).Returns(workQueue.Object);
			messageQueueProvider.Setup(m => m.ErrorQueue).Returns(errorQueue.Object);

			var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

			var systemInformationDispatchingService = new SystemInformationDispatchingService(
				agentCoordinationServiceFactory.Object,
				messageQueueFeederFactory.Object,
				messageQueueWorkerFactory.Object,
				messageQueueProvider.Object,
				messageQueuePersistence.Object);

			// Act
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			systemInformationDispatchingService.Start();

			stopwatch.Stop();

			// Assert
			int tolerance = 100;
			Assert.GreaterOrEqual(stopwatch.ElapsedMilliseconds, Math.Max(durationTheFeederIsRunning, durationTheWorkerIsRunning) - tolerance);
		}

		#endregion
	}

	internal class WaitingFeeder : IMessageQueueFeeder
	{
		private readonly int waitTime;

		public WaitingFeeder(int waitTime)
		{
			this.waitTime = waitTime;
		}

		public void Start()
		{
			Thread.Sleep(this.waitTime);
		}

		public void Pause()
		{
		}

		public void Resume()
		{
		}

		public void Stop()
		{
		}

		public ServiceStatus GetStatus()
		{
			throw new NotImplementedException();
		}
	}

	internal class WaitingWorker : IMessageQueueWorker
	{
		private readonly int waitTime;

		public WaitingWorker(int waitTime)
		{
			this.waitTime = waitTime;
		}

		public void Start()
		{
			Thread.Sleep(this.waitTime);
		}

		public void Pause()
		{
		}

		public void Resume()
		{
		}

		public void Stop()
		{
		}

		public ServiceStatus GetStatus()
		{
			throw new NotImplementedException();
		}
	}
}