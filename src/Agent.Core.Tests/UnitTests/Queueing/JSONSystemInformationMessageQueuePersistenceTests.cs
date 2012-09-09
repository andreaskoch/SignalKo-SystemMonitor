using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class JSONSystemInformationMessageQueuePersistenceTests
    {
        #region constructor
        
        [Test]
        public void Constructotor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            var encodingProvider = new Mock<IEncodingProvider>();

            // Act
            var messageQueuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, encodingProvider.Object);

            // Assert
            Assert.IsNotNull(messageQueuePersistence);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructotor_ConfigurationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var encodingProvider = new Mock<IEncodingProvider>();

            // Act
            new JSONSystemInformationMessageQueuePersistence(null, encodingProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructotor_EncodingProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();

            // Act
            new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, null);
        }

        #endregion
    }
}