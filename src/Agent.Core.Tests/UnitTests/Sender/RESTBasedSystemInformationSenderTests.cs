using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;

namespace Agent.Core.Tests.UnitTests.Sender
{
    [TestFixture]
    public class RESTBasedSystemInformationSenderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var serviceConfigurationProvider = new Mock<IRESTServiceConfigurationProvider>();

            // Act
            var systemInformationSender = new RESTBasedSystemInformationSender(serviceConfigurationProvider.Object);

            // Assert
            Assert.IsNotNull(systemInformationSender);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RESTServiceConfigurationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Act
            new RESTBasedSystemInformationSender(null);
        }

        #endregion

        #region Configuration Properts

        [Test]
        public void ConfigurationProperty()
        {
            
        }

        #endregion
    }
}