using System;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Services;

namespace Common.Tests.UnitTests
{
    /// <summary>
    /// See:
    /// http://en.wikipedia.org/wiki/Megabyte
    /// https://www.google.com/search?q=convert+126593+bytes+to+megabytes
    /// </summary>
    [TestFixture]
    public class MemoryUnitConverterTests
    {
        private IMemoryUnitConverter memoryUnitConverter;

        [SetUp]
        public void Setup()
        {
            this.memoryUnitConverter = new MemoryUnitConverter();
        }

        #region Bytes To MB

        [TestCase(1000f, 0.000953674d, 0.000000001)]
        [TestCase(1048576f, 1d, 0.000000001)]
        public void ConvertBytesToMegabytes(float bytes, double expectedResultInMegabytes, double precision)
        {
            // Act
            var result = this.memoryUnitConverter.ConvertBytesToMegabytes(bytes);

            // Assert
            Assert.IsTrue(AlmostEquals(expectedResultInMegabytes, result, precision), "Expected: {0}, Actual: {1}", expectedResultInMegabytes, result);
        }

        #endregion

        #region Bytes To GB

        [TestCase(1073741824f, 1.0d, 0.000000001)]
        public void ConvertBytesToGigabyte(float bytes, double expectedResultInGigabytes, double precision)
        {
            // Act
            var result = this.memoryUnitConverter.ConvertBytesToGigabyte(bytes);

            // Assert
            Assert.IsTrue(AlmostEquals(expectedResultInGigabytes, result, precision), "Expected: {0}, Actual: {1}", expectedResultInGigabytes, result);
        }

        #endregion

        #region MB To GB

        [TestCase(1f, 0.000976563d, 0.000000001)]
        [TestCase(1024f, 1d, 0.000000001)]
        public void ConvertMegabyteToGigabyte(float megabytes, double expectedResultInGigabytes, double precision)
        {
            // Act
            var result = this.memoryUnitConverter.ConvertMegabyteToGigabyte(megabytes);

            // Assert
            Assert.IsTrue(AlmostEquals(expectedResultInGigabytes, result, precision), "Expected: {0}, Actual: {1}", expectedResultInGigabytes, result);
        }

        #endregion

        #region Utility Methods

        private static bool AlmostEquals(double double1, double double2, double precision)
        {
            return Math.Abs(double1 - double2) <= precision;
        }

        #endregion
    }
}