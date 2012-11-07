using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.IntegrationTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueFeederFactoryTests
    {
        #region GetMessageQueueFeeder

        [Test]
        public void GetMessageQueueFeeder_WorkQueueIsNotNull_ResultIsNotNull()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();
            messageQueueProvider.Setup(m => m.WorkQueue).Returns(workQueue.Object);

            var systemInformationMessageQueueFeederFactory = new SystemInformationMessageQueueFeederFactory(
                systemInformationProvider.Object, messageQueueProvider.Object);

            // Act
            var result = systemInformationMessageQueueFeederFactory.GetMessageQueueFeeder();

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}