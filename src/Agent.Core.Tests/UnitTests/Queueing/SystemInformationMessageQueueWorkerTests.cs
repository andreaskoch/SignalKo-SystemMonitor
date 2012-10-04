using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueWorkerTests
    {
        #region Test Setup
        
        private readonly Mutex sequentialTestExecutionMonitor;

        public SystemInformationMessageQueueWorkerTests()
        {
            this.sequentialTestExecutionMonitor = new Mutex(false);
        }

        [SetUp]
        public void BeforeEachTest()
        {
            Thread.Sleep(2000);
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
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object);

            // Assert
            Assert.IsNotNull(messageQueueWorker);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationSenderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueWorker(null, workQueue.Object, errorQueue.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WorkQueueParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueWorker(systemInformationSender.Object, null, errorQueue.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ErrorQueueParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, null);
        }

        [Test]
        public void Constructor_InitialStatusIsStopped()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Assert
                Assert.AreEqual(ServiceStatus.Stopped, messageQueueWorker.GetStatus());                
            }
        }

        #endregion
        
        #region Stop

        [Test]
        public void Stop_EndsTheWorkerLoop()
        {
            // Arrange
            int timeToWaitBeforeStop = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 5;

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(true);

            SystemInformationQueueItem queueItem = null;
            workQueue.Setup(q => q.Dequeue()).Returns(queueItem);

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var worker = new Task(messageQueueWorker.Start);
                worker.Start();

                Thread.Sleep(timeToWaitBeforeStop);
                messageQueueWorker.Stop();

                Task.WaitAll(new[] { worker }, timeToWaitBeforeStop);

                stopwatch.Stop();

                // Assert
                Assert.LessOrEqual(stopwatch.ElapsedMilliseconds - timeToWaitBeforeStop, SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds + 100);                
            }
        }

        [Test]
        public void Stop_ServiceIsNotStartet_StatusIsNotChanged()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var statusBefore = messageQueueWorker.GetStatus();

                // Act
                messageQueueWorker.Stop();
                var statusAfter = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(statusBefore, statusAfter);                
            }
        }

        [Test]
        public void Stop_StatusIsChangedToStopped()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();
                Thread.Sleep(500);

                // Act
                messageQueueWorker.Stop();

                Thread.Sleep(500);

                // Assert
                Assert.AreEqual(ServiceStatus.Stopped, messageQueueWorker.GetStatus());                
            }
        }

        #endregion

        #region Empty Queue

        [Test]
        public void WorkerIsStarted_QueueIsEmpty_WorkerKeepsPollingForItemsInQueue()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 5;

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(true);

            SystemInformationQueueItem queueItem = null;
            workQueue.Setup(q => q.Dequeue()).Returns(queueItem);

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var worker = new Task(messageQueueWorker.Start);
                worker.Start();
                Task.WaitAll(new[] { worker }, maxRuntime);
                messageQueueWorker.Stop();

                // Assert
                workQueue.Verify(q => q.Dequeue(), Times.Between(4, 5, Range.Inclusive));                
            }
        }

        [Test]
        public void WorkerIsStarted_QueueIsEmpty_SendIsNotCalled()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(true);

            SystemInformationQueueItem queueItem = null;
            workQueue.Setup(q => q.Dequeue()).Returns(queueItem);

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();

            var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object);

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

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(false);

            int maxCount = 3;
            int itemIndex = 1;
            workQueue.Setup(q => q.Dequeue()).Returns(() =>
                {
                    if (itemIndex <= maxCount)
                    {
                        itemIndex++;
                        return new SystemInformationQueueItem(new SystemInformation { MachineName = itemIndex.ToString(), Timestamp = DateTime.UtcNow });
                    }

                    return null;
                });

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var worker = new Task(messageQueueWorker.Start);
                worker.Start();
                Task.WaitAll(new[] { worker }, maxRuntime);

                // Assert
                systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.Exactly(maxCount));                
            }
        }

        [Test]
        public void QueueIsFilled_SendCausesExceptionWhichJustifiesARetry_RetryCountHasNotBeenExceeded_ItemIsAddedToQueue()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(false);

            bool itemHasBeenAccessed = false;
            var systemInfo = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow };
            var queueItem = new SystemInformationQueueItem(systemInfo);
            workQueue.Setup(q => q.Dequeue()).Returns(() =>
                {
                    if (!itemHasBeenAccessed)
                    {
                        itemHasBeenAccessed = true;
                        return queueItem;
                    }

                    return null;
                });

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(systemInfo)).Throws(
                new SendSystemInformationFailedException("Some minor exception which justifies a retry", null));

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var worker = new Task(messageQueueWorker.Start);
                worker.Start();
                Task.WaitAll(new[] { worker }, maxRuntime);

                // Assert
                workQueue.Verify(q => q.Enqueue(It.IsAny<SystemInformationQueueItem>()), Times.Once());                
            }
        }

        [Test]
        public void QueueIsFilled_SendCausesExceptionWhichJustifiesARetry_RetryCountHasBeenExceeded_ItemIsAddedToFailedRequestQueue()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(false);

            bool itemHasBeenAccessed = false;
            var systemInfo = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow };
            var queueItem = new SystemInformationQueueItem(systemInfo) { EnqueuCount = SystemInformationMessageQueueWorker.MaxRetryCount };
            workQueue.Setup(q => q.Dequeue()).Returns(() =>
            {
                if (!itemHasBeenAccessed)
                {
                    itemHasBeenAccessed = true;
                    return queueItem;
                }

                return null;
            });

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(systemInfo)).Throws(
                new SendSystemInformationFailedException("Some minor exception which justifies a retry", null));

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var worker = new Task(messageQueueWorker.Start);
                worker.Start();
                Task.WaitAll(new[] { worker }, maxRuntime);

                // Assert
                workQueue.Verify(q => q.Enqueue(queueItem), Times.Never());
                errorQueue.Verify(q => q.Enqueue(queueItem), Times.Once());                
            }
        }

        [Test]
        public void QueueIsFilled_SendCausesFatalException_AllItemsInQueueAreMovedToTheFailedRequestQueue()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(false);
            workQueue.Setup(q => q.Dequeue()).Returns(
                () => new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow }));

            var items = new[]
                {
                    new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName + "1", Timestamp = DateTime.UtcNow }),
                    new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName + "2", Timestamp = DateTime.UtcNow }),
                    new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName + "3", Timestamp = DateTime.UtcNow })
                };

            workQueue.Setup(q => q.PurgeAllItems()).Returns(items);

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(It.IsAny<SystemInformation>())).Throws(new FatalSystemInformationSenderException("Some fatal exception."));

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                messageQueueWorker.Start();

                // Assert
                errorQueue.Verify(q => q.Enqueue(items), Times.Once());                
            }
        }

        [Test]
        public void QueueIsFilled_SendCausesFatalException_WorkerStops()
        {
            // Arrange
            int maxRuntime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 2;

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(q => q.IsEmpty()).Returns(false);
            workQueue.Setup(q => q.Dequeue()).Returns(
                () => new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow }));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var systemInformationSender = new Mock<ISystemInformationSender>();
            systemInformationSender.Setup(s => s.Send(It.IsAny<SystemInformation>())).Throws(new FatalSystemInformationSenderException("Some fatal exception."));

            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
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
        }

        #endregion

        #region Start

        [Test]
        public void Start_StatusIsChangedToRunning()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(500);

                var status = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(ServiceStatus.Running, status);                
            }
        }

        [Test]
        public void Start_RunFor3Intervals_SendIsCalledAtLeastTwoTimes()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 3;

            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Task.WaitAll(new[] { workerTaks }, durationInMilliseconds);

                // Assert
                systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.AtLeast(2));                
            }
        }

        #endregion

        #region Pause

        [Test]
        public void Pause_ServiceIsRunning_StatusIsChangedToPaused()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();
                Thread.Sleep(500);

                // Act
                messageQueueWorker.Pause();

                // Assert
                Assert.AreEqual(ServiceStatus.Paused, messageQueueWorker.GetStatus());                
            }
        }

        [Test]
        public void Pause_RunFor6Intervals_PauseIsCalledAfter3Intervals_SendIsCalledAtMostThreeTimes()
        {
            // Arrange
            int durationInMilliseconds = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 6;

            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(durationInMilliseconds / 2);

                messageQueueWorker.Pause();

                // Assert
                systemInformationSender.Verify(s => s.Send(It.IsAny<SystemInformation>()), Times.AtMost(3));                
            }
        }

        [Test]
        public void Pause_ServiceIsPaused_StatusIsNotChanged()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(500);
                messageQueueWorker.Pause();
                var statusBefore = messageQueueWorker.GetStatus();
                Thread.Sleep(500);

                // Act
                messageQueueWorker.Pause();
                var statusAfter = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(statusBefore, statusAfter);                
            }
        }

        #endregion

        #region Resume

        [Test]
        public void Resume_ServiceIsRunning_StatusIsNotChanged()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(500);

                var statusBeforeResume = messageQueueWorker.GetStatus();

                // Act
                messageQueueWorker.Resume();
                Thread.Sleep(500);

                var statusAfterResume = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(statusBeforeResume, statusAfterResume);                
            }
        }

        [Test]
        public void Resume_ServiceIsStopped_StatusIsStillStopped()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                messageQueueWorker.Resume();

                // Assert
                Assert.AreEqual(ServiceStatus.Stopped, messageQueueWorker.GetStatus());                
            }
        }

        [Test]
        public void Resume_ServiceIsPaused_StatusIsChangedToRunning()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(500);
                messageQueueWorker.Pause();

                // Act
                Thread.Sleep(500);
                messageQueueWorker.Resume();

                // Assert
                Assert.AreEqual(ServiceStatus.Running, messageQueueWorker.GetStatus());                
            }
        }

        #endregion

        #region GetStatus

        [Test]
        public void GetStatus_ServiceIsNotStarted_ResultIsStopped()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                // Act
                var status = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(ServiceStatus.Stopped, status);
            }
        }

        [Test]
        public void GetStatus_ServiceIsStarted_ResultIsRunning()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(500);

                // Act
                var status = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(ServiceStatus.Running, status);
            }
        }

        [Test]
        public void GetStatus_ServiceIsPaused_ResultIsPaused()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.Dequeue()).Returns(new SystemInformationQueueItem(new SystemInformation()));

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            using (var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object))
            {
                var workerTaks = new Task(messageQueueWorker.Start);
                workerTaks.Start();

                Thread.Sleep(500);
                messageQueueWorker.Pause();
                Thread.Sleep(500);

                // Act
                var status = messageQueueWorker.GetStatus();

                // Assert
                Assert.AreEqual(ServiceStatus.Paused, status);
            }
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_ServiceIsStopped()
        {
            // Arrange
            var maxWaitTime = SystemInformationMessageQueueWorker.WorkIntervalInMilliseconds * 3;

            var systemInformationSender = new Mock<ISystemInformationSender>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            workQueue.Setup(w => w.IsEmpty()).Returns(true);

            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var messageQueueWorker = new SystemInformationMessageQueueWorker(systemInformationSender.Object, workQueue.Object, errorQueue.Object);
            var workerTaks = new Task(messageQueueWorker.Start);
            workerTaks.Start();

            Thread.Sleep(500);

            // Act
            messageQueueWorker.Dispose();
            Task.WaitAll(new[] { workerTaks }, maxWaitTime);

            // Assert
            Assert.AreEqual(ServiceStatus.Stopped, messageQueueWorker.GetStatus());
           
        }

        #endregion
    }
}