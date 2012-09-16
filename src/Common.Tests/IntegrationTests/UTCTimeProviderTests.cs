using System;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Services;

namespace Common.Tests.IntegrationTests
{
    [TestFixture]
    public class UTCTimeProviderTests
    {
        private ITimeProvider timeProvider;

        [SetUp]
        public void Setup()
        {
            this.timeProvider = new UTCTimeProvider();
        }

        [Test]
        public void GetDateAndTime_ResultIsNotNull()
        {
            // Arrange
            // Act
            var result = this.timeProvider.GetDateAndTime();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetDateAndTime_ResultIsCurrentMachinesUTCDate()
        {
            // Arrange
            var expected = DateTime.UtcNow;
            var tolerance = new TimeSpan(0, 0, 0, 0, 100);

            // Act
            var result = this.timeProvider.GetDateAndTime();

            // Assert
            Assert.IsTrue(result.Subtract(expected) <= tolerance);
        }
    }
}