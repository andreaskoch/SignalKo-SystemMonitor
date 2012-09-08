using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.UnitTests.Collector
{
    [TestFixture]
    public class SystemMemoryStatusProviderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var memoryUnitConverter = new Mock<IMemoryUnitConverter>();

            // Act
            var systemMemoryStatusProvider = new SystemMemoryStatusProvider(memoryUnitConverter.Object);

            // Assert
            Assert.IsNotNull(systemMemoryStatusProvider);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MemoryUnitConverterParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Act
            new SystemMemoryStatusProvider(null);
        }

        #endregion
    }
}