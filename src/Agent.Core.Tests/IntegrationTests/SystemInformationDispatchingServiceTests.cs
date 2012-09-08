using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Queuing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.IntegrationTests
{
    [TestFixture]
    public class SystemInformationDispatchingServiceTests
    {
        [Test]
        public void RunFor10Seconds_SendFailsForAllItems_DispatcherStopsOnlyIfTheQueueIsEmptyAndAllRetryAttempsHaveFailed()
        {
            // Arrange
            int runtimeInMilliseconds = 10 * 1000;
            int itemsReturnedFromSystemInformationProvider = 0;
            int attemptsToSend = 0;

            // prepare system information provider
            var provider = new Mock<ISystemInformationProvider>();
            provider.Setup(p => p.GetSystemInfo()).Returns(() =>
                {
                    itemsReturnedFromSystemInformationProvider++;
                    return new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow };
                });

            // prepare sender
            var sender = new Mock<ISystemInformationSender>();
            sender.Setup(s => s.Send(It.IsAny<SystemInformation>())).Callback(() => { attemptsToSend++; }).Throws(new SendSystemInformationFailedException("Send failed.", null));

            IMessageQueue<SystemInformation> queue = new SystemInformationMessageQueue();
            IMessageQueueFeeder messageQueueFeeder = new SystemInformationMessageQueueFeeder(provider.Object, queue);
            IMessageQueueWorker messageQueueWorker = new SystemInformationMessageQueueWorker(queue, sender.Object);

            var systemInformationDispatchingService = new SystemInformationDispatchingService(messageQueueFeeder, messageQueueWorker);

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var dispatcher = new Task(systemInformationDispatchingService.Start);
            dispatcher.Start();

            Thread.Sleep(runtimeInMilliseconds);
            systemInformationDispatchingService.Stop();

            Task.WaitAll(new[] { dispatcher });

            stopwatch.Stop();

            // Assert
            int queueSize = queue.GetSize();
            Console.WriteLine(
                "After a runtime of {0} milliseconds the dispatcher has been stopped with {1} items in queue. It took {2} milliseconds until the queue worker stopped sending out all queue items (Attempts To Send: {3}).",
                runtimeInMilliseconds,
                itemsReturnedFromSystemInformationProvider,
                stopwatch.ElapsedMilliseconds,
                attemptsToSend);

            Assert.AreEqual(0, queueSize);
        }
    }
}