using System;
using System.Diagnostics;
using System.Threading;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Core.Queuing;

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
            var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            // Act
            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object);

            // Assert
            Assert.IsNotNull(systemInformationDispatchingService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueFeederParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            // Act
            new SystemInformationDispatchingService(null, messageQueueWorker.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueWorkerParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder>();

            // Act
            new SystemInformationDispatchingService(messageQueueFeeder.Object, null);
        }

        #endregion

        #region Start

        [Test]
        public void Start_MessageFeederStartIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();
            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object);

            // Act
            systemInformationDispatchingService.Start();

            // Assert
            messageQueueFeeder.Verify(f => f.Start(), Times.Once());
        }

        [Test]
        public void Start_MessageWorkerStartIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();
            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object);

            // Act
            systemInformationDispatchingService.Start();

            // Assert
            messageQueueWorker.Verify(w => w.Start(), Times.Once());
        }

        #endregion

        #region Stop

        [Test]
        public void Stop_MessageFeederStopIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();
            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object);

            // Act
            systemInformationDispatchingService.Stop();

            // Assert
            messageQueueFeeder.Verify(f => f.Stop(), Times.Once());
        }

        [Test]
        public void Stop_MessageWorkerStopIsCalled()
        {
            // Arrange
            var messageQueueFeeder = new Mock<IMessageQueueFeeder>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();
            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder.Object, messageQueueWorker.Object);

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
            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder, messageQueueWorker);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            systemInformationDispatchingService.Start();

            stopwatch.Stop();

            // Assert
            Assert.GreaterOrEqual(stopwatch.ElapsedMilliseconds, Math.Max(durationTheFeederIsRunning, durationTheWorkerIsRunning));
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

        public void Stop()
        {
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

        public void Stop()
        {
        }
    }
}