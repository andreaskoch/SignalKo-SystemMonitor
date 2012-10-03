using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueWorkerFactoryTests
    {
        #region Constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();

            // Act
            var systemInformationMessageQueueFeederFactory = new SystemInformationMessageQueueWorkerFactory(
                systemInformationSender.Object, messageQueueProvider.Object);

            // Assert
            Assert.IsNotNull(systemInformationMessageQueueFeederFactory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationSenderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueWorkerFactory(null, messageQueueProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationSender = new Mock<ISystemInformationSender>();

            // Act
            new SystemInformationMessageQueueWorkerFactory(systemInformationSender.Object, null);
        }

        #endregion
    }
}