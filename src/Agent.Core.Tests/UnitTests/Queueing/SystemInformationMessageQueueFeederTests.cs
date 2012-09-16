using System;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueFeederTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();

            // Act
            var systemInformationDispatcher = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object);

            // Assert
            Assert.IsNotNull(systemInformationDispatcher);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Act
            new SystemInformationMessageQueueFeeder(null);
        }

        #endregion

        #region Start

        [Test]
        public void Start_RunsFor3Intervals_SystemInfoIsPulledAtLeastTwoTimes()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var systemInformationDispatcher = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object);

            // Act
            var dispatcherTask = new Task(() => systemInformationDispatcher.Start(workQueue.Object));
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
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 2;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            SystemInformation systemInformation = null;
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(systemInformation);

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var systemInformationDispatcher = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object);

            // Act
            var dispatcherTask = new Task(() => systemInformationDispatcher.Start(workQueue.Object));
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            workQueue.Verify(s => s.Enqueue(It.IsAny<SystemInformationQueueItem>()), Times.Never());
        }

        [Test]
        public void Start_SystemInformationProviderReturnsSystemInformation_SystemInformationIsAddedToQueue()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 2;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(() => new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow });

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var systemInformationDispatcher = new SystemInformationMessageQueueFeeder(
                systemInformationProvider.Object);

            // Act
            var dispatcherTask = new Task(() => systemInformationDispatcher.Start(workQueue.Object));
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            workQueue.Verify(s => s.Enqueue(It.IsAny<SystemInformationQueueItem>()), Times.AtLeastOnce());
        }

        #endregion
    }
}