using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.IntegrationTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueWorkerFactoryTests
    {
        #region GetMessageQueueFeeder

        [Test]
        public void GetMessageQueueWorker_WorkQueueIsNotNull_ErrorQueueIsNotNull_ResultIsNotNull()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();

            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            messageQueueProvider.Setup(m => m.WorkQueue).Returns(workQueue.Object);
            messageQueueProvider.Setup(m => m.ErrorQueue).Returns(errorQueue.Object);

            var systemInformationMessageQueueFeederFactory = new SystemInformationMessageQueueWorkerFactory(systemInformationSender.Object, messageQueueProvider.Object);

            // Act
            var result = systemInformationMessageQueueFeederFactory.GetMessageQueueWorker();

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}