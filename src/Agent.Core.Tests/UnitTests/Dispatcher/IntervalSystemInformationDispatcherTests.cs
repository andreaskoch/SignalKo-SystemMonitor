using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Dispatcher;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Dispatcher
{
    [TestFixture]
    public class IntervalSystemInformationDispatcherTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            // Act
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object, messageQueueWorker.Object);

            // Assert
            Assert.IsNotNull(systemInformationDispatcher);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            // Act
            new IntervalSystemInformationDispatcher(null, messageQueue.Object, messageQueueWorker.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            // Act
            new IntervalSystemInformationDispatcher(systemInformationProvider.Object, null, messageQueueWorker.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueWorkerParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueue = new Mock<IMessageQueue>();

            // Act
            new IntervalSystemInformationDispatcher(systemInformationProvider.Object, messageQueue.Object, null);
        }

        #endregion

        #region Start

        [Test]
        public void Start_RunsFor3Intervals_SystemInfoIsPulledAtLeastTwoTimes()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object, messageQueueWorker.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            systemInformationProvider.Verify(s => s.GetSystemInfo(), Times.AtLeast(2));
        }

        [Test]
        public void Start_MessageQueueWorkerIsStarted()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object, messageQueueWorker.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            messageQueueWorker.Verify(s => s.Start(), Times.Once());
        }

        [Test]
        public void Start_WaitsForMessageQueueWorkerToFinish()
        {
            // Arrange
            int dispatcherRuntime = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 1;
            int timeMessageWorkerTakesToFinish = dispatcherRuntime * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new WaitingMessageQueueWorker(timeMessageWorkerTakesToFinish);

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object, messageQueueWorker);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, dispatcherRuntime);
            systemInformationDispatcher.Stop();

            stopwatch.Stop();

            // Assert
            Assert.GreaterOrEqual(stopwatch.ElapsedMilliseconds, timeMessageWorkerTakesToFinish);
        }


        [Test]
        public void Start_SystemInformationProviderReturnsNull_InfoIsNotQueued()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 2;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            SystemInformation systemInformation = null;
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(systemInformation);

            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object, messageQueueWorker.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            messageQueue.Verify(s => s.Enqueue(It.IsAny<SystemInformation>()), Times.Never());
        }

        [Test]
        public void Start_SystemInformationProviderReturnsSystemInformation_SystemInformationIsAddedToQueue()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 2;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(() => new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });

            var messageQueue = new Mock<IMessageQueue>();
            var messageQueueWorker = new Mock<IMessageQueueWorker>();

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object, messageQueueWorker.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            messageQueue.Verify(s => s.Enqueue(It.IsAny<SystemInformation>()), Times.AtLeastOnce());
        }

        #endregion
    }

    internal class WaitingMessageQueueWorker : IMessageQueueWorker
    {
        private readonly int waitTimeInMilliseconds;

        public WaitingMessageQueueWorker(int waitTimeInMilliseconds)
        {
            this.waitTimeInMilliseconds = waitTimeInMilliseconds;
        }

        public void Start()
        {
            Console.WriteLine("Starting to wait.");
            Thread.Sleep(this.waitTimeInMilliseconds * 10);
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("Sleeping " + i);
                Thread.Sleep(this.waitTimeInMilliseconds);
            }
            Console.WriteLine("Done waiting.");
        }

        public void Stop()
        {
        }
    }
}