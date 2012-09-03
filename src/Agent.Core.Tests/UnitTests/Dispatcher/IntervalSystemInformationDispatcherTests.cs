using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Dispatcher;
using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;
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
            var systemInformationSender = new Mock<ISystemInformationSender>();

            // Act
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Assert
            Assert.IsNotNull(systemInformationDispatcher);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();

            // Act
            new IntervalSystemInformationDispatcher(null, systemInformationSender.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationSenderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();

            // Act
            new IntervalSystemInformationDispatcher(systemInformationProvider.Object, null);
        }

        #endregion

        #region Send

        [Test]
        public void Send_SystemInformationProviderReturnsNull_InfoIsNotSend()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 2;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            SystemInformation systemInformation = null;
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(systemInformation);

            var systemInformationSender = new Mock<ISystemInformationSender>();
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            systemInformationSender.Verify(s => s.Send(It.Is<SystemInformation>(info => info == null)), Times.Never());
        }

        [Test]
        public void Send_SystemInformationSenderThrows_FatalSystemInformationSenderException_DispatcherIsStoppedAheadOfTime_AfterTheFirstRun()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 10;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(
                () => new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });

            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(It.IsAny<SystemInformation>())).Throws(new FatalSystemInformationSenderException("Some fatal error."));

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            stopwatch.Stop();

            // Assert
            Assert.Greater(stopwatch.ElapsedMilliseconds, IntervalSystemInformationDispatcher.SendIntervalInMilliseconds);
            Assert.Less(stopwatch.ElapsedMilliseconds, durationInMilliseconds);
        }

        [Test]
        public void Send_RunsFor3Seconds_GetSystemInfoIsCalledAtLeast2Times()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            systemInformationProvider.Verify(s => s.GetSystemInfo(), Times.AtLeast(2));
        }

        [Test]
        public void Send_RunsFor3Seconds_SendIsCalledAtLeast2Times()
        {
            // Arrange
            int durationInMilliseconds = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            systemInformationProvider.Setup(s => s.GetSystemInfo()).Returns(
                () => new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            var dispatcherTask = new Task(systemInformationDispatcher.Start);
            dispatcherTask.Start();
            Task.WaitAll(new[] { dispatcherTask }, durationInMilliseconds);
            systemInformationDispatcher.Stop();

            // Assert
            systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.AtLeast(2));
        }

        #endregion

        #region Stop

        [Test]
        public void Stop_IsExecutedBeforeStart_DispatcherRunsOnlyForOneInterval()
        {
            // Arrange
            int slack = 100;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            systemInformationDispatcher.Stop();
            systemInformationDispatcher.Start();

            stopwatch.Stop();

            // Assert
            Assert.IsTrue(IntervalSystemInformationDispatcher.SendIntervalInMilliseconds - stopwatch.ElapsedMilliseconds > slack * -1);
            Assert.Less(stopwatch.ElapsedMilliseconds, IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 2);
        }

        [Test]
        public void Stop_IsExecutedBeforeStart_SystemInformationProviderIsNeverCalled()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            systemInformationDispatcher.Stop();
            systemInformationDispatcher.Start();

            // Assert
            systemInformationProvider.Verify(s => s.GetSystemInfo(), Times.Never());
        }

        [Test]
        public void Stop_IsExecutedBeforeStart_SystemInformationSenderIsNeverCalled()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            systemInformationDispatcher.Stop();
            systemInformationDispatcher.Start();

            // Assert
            systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.Never());
        }

        [Test]
        public void Stop_IsExecutedAfterThreeIntervals_TotalDurationIsLessThanFourIntervals()
        {
            // Arrange
            int slack = 100;
            int waitDuration = IntervalSystemInformationDispatcher.SendIntervalInMilliseconds * 3;

            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var systemInformationDispatcher = new IntervalSystemInformationDispatcher(systemInformationProvider.Object, systemInformationSender.Object);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var startTask = new Task(systemInformationDispatcher.Start);
            startTask.Start();

            Thread.Sleep(waitDuration);
            systemInformationDispatcher.Stop();

            stopwatch.Stop();

            // Assert
            Assert.IsTrue(stopwatch.ElapsedMilliseconds - waitDuration > slack * -1);
        }

        #endregion
    }
}