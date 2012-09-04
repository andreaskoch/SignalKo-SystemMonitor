using System;
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

            // Act
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, messageQueue.Object);

            // Assert
            Assert.IsNotNull(systemInformationDispatcher);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueue = new Mock<IMessageQueue>();

            // Act
            new IntervalSystemInformationDispatcher(null, messageQueue.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();

            // Act
            new IntervalSystemInformationDispatcher(systemInformationProvider.Object, null);
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

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, messageQueue.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            systemInformationProvider.Verify(s => s.GetSystemInfo(), Times.AtLeast(2));
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

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object);

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

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(
                systemInformationProvider.Object, messageQueue.Object);

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
}