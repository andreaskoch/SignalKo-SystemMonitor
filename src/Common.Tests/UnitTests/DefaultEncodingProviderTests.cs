using System.Text;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Services;

namespace Common.Tests.UnitTests
{
    [TestFixture]
    public class DefaultEncodingProviderTests
    {
        [Test]
        public void GetEncoding_ReturnsUTF8Encoding()
        {
            // Arrange
            var defaultEncodingProvider = new DefaultEncodingProvider();

            // Act
            var result = defaultEncodingProvider.GetEncoding();

            // Assert
            Assert.AreEqual(Encoding.UTF8, result);
        }
    }
}