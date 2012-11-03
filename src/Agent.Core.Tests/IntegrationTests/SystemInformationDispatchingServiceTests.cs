using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Agent.Core.Coordination;
using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.IntegrationTests
{
    [TestFixture]
    public class SystemInformationDispatchingServiceTests
    {
        private IJSONMessageQueuePersistenceConfigurationProvider jsonMessageQueuePersistenceConfigurationProvider;

        private IEncodingProvider encodingProvider;

        private JSONMessageQueuePersistenceConfiguration persistenceConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {
            this.jsonMessageQueuePersistenceConfigurationProvider = new AppConfigJSONMessageQueuePersistenceConfigurationProvider();
            this.persistenceConfiguration = this.jsonMessageQueuePersistenceConfigurationProvider.GetConfiguration();
            this.encodingProvider = new DefaultEncodingProvider();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            File.Delete(this.persistenceConfiguration.FilePath);
        }

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
                    return new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTime.UtcNow };
                });

            // prepare sender
            var sender = new Mock<ISystemInformationSender>();
            sender.Setup(s => s.Send(It.IsAny<SystemInformation>())).Callback(() => { attemptsToSend++; }).Throws(new SendSystemInformationFailedException("Send failed.", null));

            IMessageQueue<SystemInformation> workQueue = new SystemInformationMessageQueue();
            IMessageQueue<SystemInformation> errorQueue = new SystemInformationMessageQueue();
            IMessageQueueProvider<SystemInformation> messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue, errorQueue);

            IMessageQueueFeeder messageQueueFeeder = new SystemInformationMessageQueueFeeder(provider.Object, workQueue);
            IMessageQueueWorker messageQueueWorker = new SystemInformationMessageQueueWorker(sender.Object, workQueue, errorQueue);

            var agentCoordinationService = new Mock<IAgentCoordinationService>();
            var agentCoordinationServiceFactory = new Mock<IAgentCoordinationServiceFactory>();
            agentCoordinationServiceFactory.Setup(f => f.GetAgentCoordinationService(It.IsAny<Action>(), It.IsAny<Action>())).Returns(
                agentCoordinationService.Object);

            var messageQueueFeederFactory = new Mock<IMessageQueueFeederFactory>();
            messageQueueFeederFactory.Setup(f => f.GetMessageQueueFeeder()).Returns(messageQueueFeeder);

            var messageQueueWorkerFactory = new Mock<IMessageQueueWorkerFactory>();
            messageQueueWorkerFactory.Setup(f => f.GetMessageQueueWorker()).Returns(messageQueueWorker);

            IMessageQueuePersistence<SystemInformation> messageQueuePersistence =
                new JSONSystemInformationMessageQueuePersistence(this.jsonMessageQueuePersistenceConfigurationProvider, this.encodingProvider);

            var systemInformationDispatchingService = new SystemInformationDispatchingService(
                agentCoordinationServiceFactory.Object,
                messageQueueFeederFactory.Object,
                messageQueueWorkerFactory.Object,
                messageQueueProvider,
                messageQueuePersistence);

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
            int queueSize = workQueue.GetSize();
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