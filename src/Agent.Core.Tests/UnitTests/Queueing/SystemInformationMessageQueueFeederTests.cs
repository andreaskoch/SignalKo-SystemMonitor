using System;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueFeederTests
    {
        #region Test Setup
        
        private readonly Mutex sequentialTestExecutionMonitor;

        public SystemInformationMessageQueueFeederTests()
        {
            this.sequentialTestExecutionMonitor = new Mutex(false);
        }

        [SetUp]
        public void BeforeEachTest()
        {
            Thread.Sleep(1000);
            this.sequentialTestExecutionMonitor.WaitOne();
        }

        [TearDown]
        public void TearDown()
        {
            this.sequentialTestExecutionMonitor.ReleaseMutex();
        }

        #endregion

        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object);

            // Assert
            Assert.IsNotNull(messageQueueFeeder);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueFeeder(null, workQueue.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WorkQueueParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();

            // Act
            new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, null);
        }

        #endregion

        #region Start

        [Test]
        public void Start_RunFor3Intervals_SystemInfoIsPulledAtMostThreeTimes()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                // Act
                var feederTask = new Task(messageQueueFeeder.Start);
                feederTask.Start();
                Task.WaitAll(new[] { feederTask }, durationInMilliseconds);
                messageQueueFeeder.Stop();

                // Assert
                systemInformationProvider.Verify(s => s.GetSystemInfo(), Times.AtMost(3));                
            }
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

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                // Act
                var feederTaks = new Task(messageQueueFeeder.Start);
                feederTaks.Start();
                Task.WaitAll(new[] { feederTaks }, durationInMilliseconds);
                messageQueueFeeder.Stop();

                // Assert
                workQueue.Verify(s => s.Enqueue(It.IsAny<SystemInformationQueueItem>()), Times.Never());                
            }
        }

        [Test]
        public void Start_SystemInformationProviderReturnsSystemInformation_SystemInformationIsAddedToQueue()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 2;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(() => new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow });

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                // Act
                var feederTask = new Task(messageQueueFeeder.Start);
                feederTask.Start();
                Task.WaitAll(new[] { feederTask }, durationInMilliseconds);
                messageQueueFeeder.Stop();

                // Assert
                workQueue.Verify(s => s.Enqueue(It.IsAny<SystemInformationQueueItem>()), Times.AtLeastOnce());                
            }
        }

        #endregion

        #region Pause

        [Test]
        public void Pause_ServiceIsRunning_StatusIsChangedToPaused()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
                messageQueueFeederTask.Start();
                Thread.Sleep(durationInMilliseconds);

                // Act
                messageQueueFeeder.Pause();
                var statusAfterPause = messageQueueFeeder.GetStatus();

                // Assert
                Assert.AreEqual(ServiceStatus.Paused, statusAfterPause);
            }
        }

        [Test]
        public void Pause_ServiceIsPaused_StatusIsNotChanged()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
                messageQueueFeederTask.Start();

                Thread.Sleep(500);
                messageQueueFeeder.Pause();

                // Act
                messageQueueFeeder.Pause();
                Thread.Sleep(500);

                // Assert
                Assert.AreEqual(ServiceStatus.Paused, messageQueueFeeder.GetStatus());
            }
        }

        [Test]
        public void Pause_RunFor3Intervals_Pause_SystemInfoIsPulledAtMostTwoTimes()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                // Act
                var messageQueueFeederTask = new Task(messageQueueFeeder.Start);

                messageQueueFeederTask.Start();
                Thread.Sleep(durationInMilliseconds);

                messageQueueFeeder.Pause();
                Thread.Sleep(durationInMilliseconds);

                // Assert
                systemInformationProvider.Verify(s => s.GetSystemInfo(), Times.AtMost(3));                
            }
        }

        #endregion

        #region Resume

        [Test]
        public void Resume_ServiceIsRunning_StatusIsNotChanged()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
                messageQueueFeederTask.Start();

                var statusBeforeResume = messageQueueFeeder.GetStatus();

                // Act
                messageQueueFeeder.Resume();
                var statusAfterResume = messageQueueFeeder.GetStatus();

                // Assert
                Assert.AreEqual(statusBeforeResume, statusAfterResume);                
            }
        }

        [Test]
        public void Resume_ServiceIsStopped_StatusIsStillStopped()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
                messageQueueFeederTask.Start();
                messageQueueFeeder.Stop();

                // Act
                messageQueueFeeder.Resume();

                // Assert
                Assert.AreEqual(ServiceStatus.Stopped, messageQueueFeeder.GetStatus());                
            }
        }

        [Test]
        public void Resume_ServiceIsPaused_StatusIsChangedToRunning()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            using (var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object))
            {
                var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
                messageQueueFeederTask.Start();

                Thread.Sleep(500);
                messageQueueFeeder.Pause();

                // Act
                Thread.Sleep(500);
                messageQueueFeeder.Resume();

                // Assert
                Assert.AreEqual(ServiceStatus.Running, messageQueueFeeder.GetStatus());                
            }
        }

        #endregion

        #region Stop

        [Test]
        public void Stop_ServiceIsStopped_StatusIsNotChanged()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object);
            var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
            messageQueueFeederTask.Start();
            messageQueueFeeder.Stop();

            var statusBeforeStop = messageQueueFeeder.GetStatus();

            // Act
            messageQueueFeeder.Stop();
            var statusAfterStop = messageQueueFeeder.GetStatus();

            // Assert
            Assert.AreEqual(statusBeforeStop, statusAfterStop);
        }

        [Test]
        public void Stop_EndsARunningService()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueFeeder.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object);
            var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
            messageQueueFeederTask.Start();

            // Act
            Thread.Sleep(durationInMilliseconds);

            messageQueueFeeder.Stop();

            // Assert
            Assert.AreEqual(ServiceStatus.Stopped, messageQueueFeeder.GetStatus());
        }

        #endregion

        #region GetStatus

        [Test]
        public void GetStatus_ServiceIsNotStarted_ResultIs_Stopped()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object);

            // Act
            var result = messageQueueFeeder.GetStatus();

            // Assert
            Assert.AreEqual(ServiceStatus.Stopped, result);
        }

        [Test]
        public void GetStatus_ServiceIsStarted_ResultIs_Running()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object);
            var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
            messageQueueFeederTask.Start();

            Thread.Sleep(3000);

            // Act
            var result = messageQueueFeeder.GetStatus();

            // Assert
            Assert.AreEqual(ServiceStatus.Running, result);
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_ServiceIsStopped()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var messageQueueFeeder = new SystemInformationMessageQueueFeeder(systemInformationProvider.Object, workQueue.Object);
            var messageQueueFeederTask = new Task(messageQueueFeeder.Start);
            messageQueueFeederTask.Start();
            Thread.Sleep(500);

            // Act
            messageQueueFeeder.Dispose();
            Task.WaitAll(messageQueueFeederTask);
            
            // Assert
            Assert.AreEqual(ServiceStatus.Stopped, messageQueueFeeder.GetStatus());
        }

        #endregion
    }
}