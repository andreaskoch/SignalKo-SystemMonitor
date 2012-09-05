using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Queuing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queuing
{
    [TestFixture]
    public class SystemInformationMessageQueueWorkerTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();

            // Act
            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Assert
            Assert.IsNotNull(messageQueueWorker);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();

            // Act
            new SystemInformationMessageQueueWorker(null, systemInformationSender.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorSystemInformationSenderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueWorker(messageQueue.Object, null);
        }

        #endregion

        #region Stop Before Start

        [Test]
        public void WorkerIsStoppedBeforeItHasBeenStarted_QueueIsEmpty_DequeueIsNotCalled()
        {
            // Arrange
            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(true);

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            messageQueueWorker.Stop();
            messageQueueWorker.Start();

            // Assert
            messageQueue.Verify(q => q.Dequeue(), Times.Never());
        }

        [Test]
        public void WorkerIsStoppedBeforeItHasBeenStarted_QueueIsEmpty_ExecutesTakesOnlyOneWorkInterval()
        {
            // Arrange
            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(true);

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            messageQueueWorker.Stop();
            messageQueueWorker.Start();

            stopwatch.Stop();

            // Assert
            Assert.GreaterOrEqual(stopwatch.ElapsedMilliseconds - SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds, -100);
        }

        #endregion

        #region Stop

        [Test]
        public void Stop_EndsTheWorkerLoop()
        {
            // Arrange
            int timeToWaitBeforeStop = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 5;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(true);

            SystemInformationQueueItem queueItem = null;
            messageQueue.Setup(q => q.Dequeue()).Returns(queueItem);

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var worker = new Task(messageQueueWorker.Start);
            worker.Start();

            Thread.Sleep(timeToWaitBeforeStop);
            messageQueueWorker.Stop();

            Task.WaitAll(worker);

            stopwatch.Stop();

            // Assert
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds - timeToWaitBeforeStop, SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds + 100);
        }

        #endregion

        #region Empty Queue

        [Test]
        public void WorkerIsStarted_QueueIsEmpty_WorkerKeepsPollingForItemsInQueue()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 5;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(true);

            SystemInformationQueueItem queueItem = null;
            messageQueue.Setup(q => q.Dequeue()).Returns(queueItem);

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var worker = new Task(messageQueueWorker.Start);
            worker.Start();
            Task.WaitAll(new[] { worker }, maxRuntime);
            messageQueueWorker.Stop();

            // Assert
            messageQueue.Verify(q => q.Dequeue(), Times.Between(4, 5, Range.Inclusive));
        }

        [Test]
        public void WorkerIsStarted_QueueIsEmpty_SendIsNotCalled()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(true);

            SystemInformationQueueItem queueItem = null;
            messageQueue.Setup(q => q.Dequeue()).Returns(queueItem);

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var worker = new Task(messageQueueWorker.Start);
            worker.Start();
            Task.WaitAll(new[] { worker }, maxRuntime);
            messageQueueWorker.Stop();

            // Assert
            systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.Never());
        }

        #endregion

        #region Filled Queue

        [Test]
        public void QueueIsFilled_EveryItemInQueueIsSent()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 5;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(false);

            int maxCount = 3;
            int itemIndex = 1;
            messageQueue.Setup(q => q.Dequeue()).Returns(() =>
                {
                    if (itemIndex <= maxCount)
                    {
                        itemIndex++;
                        return new SystemInformationQueueItem(new SystemInformation { MachineName = itemIndex.ToString(), Timestamp = DateTimeOffset.UtcNow });
                    }

                    return null;
                });

            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var worker = new Task(messageQueueWorker.Start);
            worker.Start();
            Task.WaitAll(new[] { worker }, maxRuntime);
            messageQueueWorker.Stop();

            // Assert
            systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.Exactly(maxCount));
        }

        [Test]
        public void QueueIsFilled_SendCausesExceptionWhichJustifiesARetry_RetryCountHasNotBeenExceeded_ItemIsAddedToQueue()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(false);

            bool itemHasBeenAccessed = false;
            var systemInfo = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow };
            var queueItem = new SystemInformationQueueItem(systemInfo);
            messageQueue.Setup(q => q.Dequeue()).Returns(() =>
                {
                    if (!itemHasBeenAccessed)
                    {
                        itemHasBeenAccessed = true;
                        return queueItem;
                    }

                    return null;
                });

            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(systemInfo)).Throws(
                new SendSystemInformationFailedException("Some minor exception which justifies a retry", null));

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var worker = new Task(messageQueueWorker.Start);
            worker.Start();
            Task.WaitAll(new[] { worker }, maxRuntime);
            messageQueueWorker.Stop();

            // Assert
            messageQueue.Verify(q => q.Enqueue(queueItem), Times.Once());
        }

        [Test]
        public void QueueIsFilled_SendCausesExceptionWhichJustifiesARetry_RetryCountHasBeenExceeded_ItemIsDropped()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(false);

            bool itemHasBeenAccessed = false;
            var systemInfo = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow };
            var queueItem = new SystemInformationQueueItem(systemInfo) { EnqueuCount = SystemInformationMessageQueueWorker.MaxRetryCount };
            messageQueue.Setup(q => q.Dequeue()).Returns(() =>
            {
                if (!itemHasBeenAccessed)
                {
                    itemHasBeenAccessed = true;
                    return queueItem;
                }

                return null;
            });

            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(systemInfo)).Throws(
                new SendSystemInformationFailedException("Some minor exception which justifies a retry", null));

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var worker = new Task(messageQueueWorker.Start);
            worker.Start();
            Task.WaitAll(new[] { worker }, maxRuntime);
            messageQueueWorker.Stop();

            // Assert
            messageQueue.Verify(q => q.Enqueue(queueItem), Times.Never());
        }

        [Test]
        public void QueueIsFilled_SendCausesFatalException_WorkerStops()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var messageQueue = new Mock<IMessageQueue<SystemInformation>>();
            messageQueue.Setup(q => q.IsEmpty()).Returns(false);
            messageQueue.Setup(q => q.Dequeue()).Returns(
                () => new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow }));

            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(It.IsAny<SystemInformation>())).Throws(new FatalSystemInformationSenderException("Some fatal exception."));

            var messageQueueWorker = new SystemInformationMessageQueueWorker(messageQueue.Object, systemInformationSender.Object);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var worker = new Task(messageQueueWorker.Start);
            worker.Start();
            Task.WaitAll(new[] { worker }, maxRuntime);

            stopwatch.Stop();

            // Assert
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxRuntime);
        }

        #endregion
    }
}