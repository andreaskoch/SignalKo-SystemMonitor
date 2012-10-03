using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueFeederFactoryTests
    {
        #region Constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();

            // Act
            var systemInformationMessageQueueFeederFactory = new SystemInformationMessageQueueFeederFactory(
                systemInformationProvider.Object, messageQueueProvider.Object);

            // Assert
            Assert.IsNotNull(systemInformationMessageQueueFeederFactory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemInformationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var messageQueueProvider = new Mock<IMessageQueueProvider<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueFeederFactory(null, messageQueueProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MessageQueueProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemInformationProvider = new Mock<ISystemInformationProvider>();

            // Act
            new SystemInformationMessageQueueFeederFactory(systemInformationProvider.Object, null);
        }

        #endregion
    }
}