using System;
using System.Diagnostics;
using System.Threading;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core;
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
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>(); 

            // Act
            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                messageQueueFeeder.Object, messageQueueWorker.Object, messageQueueProvider.Object, messageQueuePersistence.Object);

            // Assert
            Assert.IsNotNull(systemInformationDispatchingService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueFeederParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

            // Act
            new SystemInformationDispatchingService(null, messageQueueWorker.Object, messageQueueProvider.Object, messageQueuePersistence.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueWorkerParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

            // Act
            new SystemInformationDispatchingService(messageQueueFeeder.Object, null, messageQueueProvider.Object, messageQueuePersistence.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();
            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

            // Act
            new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object, null, messageQueuePersistence.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueuePersistenceParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();

            // Act
            new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object, messageQueueProvider.Object, null);
        }

        #endregion

        #region Start

        [Test]
        public void Start_MessageFeederStartIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();

            var workQueue = new Mock<IMessageQueue<SystemInformation>>(); 
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>(); 
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            messageQueueProvider.Setup(p => p.WorkQueue).Returns(workQueue.Object);
            messageQueueProvider.Setup(p => p.ErrorQueue).Returns(errorQueue.Object);

            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();
            messageQueuePersistence.Setup(p => p.Load()).Returns(new SystemInformationQueueItem[] { });

            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                messageQueueFeeder.Object, messageQueueWorker.Object, messageQueueProvider.Object, messageQueuePersistence.Object);

            // Act
            systemInformationDispatchingService.Start();

            // Assert
            messageQueueFeeder.Verify(f => f.Start(It.IsAny<IMessageQueue<SystemInformation>>()), Times.Once());
        }

        [Test]
        public void Start_MessageWorkerStartIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            messageQueueProvider.Setup(p => p.WorkQueue).Returns(workQueue.Object);
            messageQueueProvider.Setup(p => p.ErrorQueue).Returns(errorQueue.Object);

            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();
            messageQueuePersistence.Setup(p => p.Load()).Returns(new SystemInformationQueueItem[] { });

            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                messageQueueFeeder.Object, messageQueueWorker.Object, messageQueueProvider.Object, messageQueuePersistence.Object);

            // Act
            systemInformationDispatchingService.Start();

            // Assert
            messageQueueWorker.Verify(w => w.Start(It.IsAny<IMessageQueue<SystemInformation>>(), It.IsAny<IMessageQueue<SystemInformation>>()), Times.Once());
        }

        #endregion

        #region Stop

        [Test]
        public void Stop_MessageFeederStopIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                messageQueueFeeder.Object, messageQueueWorker.Object, messageQueueProvider.Object, messageQueuePersistence.Object);

            // Act
            systemInformationDispatchingService.Stop();

            // Assert
            messageQueueFeeder.Verify(f => f.Stop(), Times.Once());
        }

        [Test]
        public void Stop_MessageWorkerStopIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder<SystemInformation>>();
            var messageQueueWorker = new Mock<IMessageQueueWorker<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();

            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                messageQueueFeeder.Object, messageQueueWorker.Object, messageQueueProvider.Object, messageQueuePersistence.Object);

            // Act
            systemInformationDispatchingService.Stop();

            // Assert
            messageQueueWorker.Verify(w => w.Stop(), Times.Once());
        }

        #endregion

        #region parallel

        [Test]
        public void Start_MethodDoesNotEndbeforeTheFeederAndWorkerHaveExited()
        {
            // Arrange
            var durationTheWorkerIsRunning = 1000;
            var durationTheFeederIsRunning = 500;

            var messageQueueFeeder = new WaitingFeeder(durationTheFeederIsRunning);
            var messageQueueWorker = new WaitingWorker(durationTheWorkerIsRunning);

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            messageQueueProvider.Setup(p => p.WorkQueue).Returns(workQueue.Object);
            messageQueueProvider.Setup(p => p.ErrorQueue).Returns(errorQueue.Object);

            var messageQueuePersistence = new Mock<IMessageQueuePersistence<SystemInformation>>();
            messageQueuePersistence.Setup(p => p.Load()).Returns(new SystemInformationQueueItem[] { });

            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                messageQueueFeeder, messageQueueWorker, messageQueueProvider.Object, messageQueuePersistence.Object);

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

    internal class WaitingFeeder : IMessageQueueFeeder<SystemInformation>
    {
        private readonly int waitTime;

        public WaitingFeeder(int waitTime)
        {
            this.waitTime = waitTime;
        }

        public void Start(IMessageQueue<SystemInformation> workQueue)
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

    internal class WaitingWorker : IMessageQueueWorker<SystemInformation>
    {
        private readonly int waitTime;

        public WaitingWorker(int waitTime)
        {
            this.waitTime = waitTime;
        }

        public void Start(IMessageQueue<SystemInformation> workQueue, IMessageQueue<SystemInformation> errorQueue)
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