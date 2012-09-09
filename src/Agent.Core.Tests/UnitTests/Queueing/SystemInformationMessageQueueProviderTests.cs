using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueProviderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParamatersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            var messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue.Object, errorQueue.Object);

            // Assert
            Assert.IsNotNull(messageQueueProvider);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WorkQueueParamaterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueProvider(null, errorQueue.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ErrorQueueParamaterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>();

            // Act
            new SystemInformationMessageQueueProvider(workQueue.Object, null);
        }

        #endregion

        #region Work Queue Property

        [Test]
        public void WorkQueueProperty_IsNotNull()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue, errorQueue);

            // Act
            var result = messageQueueProvider.WorkQueue;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void WorkQueueProperty_ReturnsTheSameObjectThatHasBeenPassedToTheConstructor()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue, errorQueue);

            // Act
            var result = messageQueueProvider.WorkQueue;

            // Assert
            Assert.AreSame(workQueue, result);
        }

        #endregion

        #region Error Queue Property

        [Test]
        public void ErrorQueueProperty_IsNotNull()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue, errorQueue);

            // Act
            var result = messageQueueProvider.ErrorQueue;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ErrorQueueProperty_ReturnsTheSameObjectThatHasBeenPassedToTheConstructor()
        {
            // Arrange
            var workQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var errorQueue = new Mock<IMessageQueue<SystemInformation>>().Object;
            var messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue, errorQueue);

            // Act
            var result = messageQueueProvider.ErrorQueue;

            // Assert
            Assert.AreSame(errorQueue, result);
        }

        #endregion
    }
}