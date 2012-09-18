using NUnit.Framework;

using SignalKo.SystemMonitor.Monitor.Web.DependencyResolution;

namespace Monitor.Web.Tests.IntegrationTests.DependencyResolution
{
    [TestFixture]
    public class StructureMapSetupTests
    {
        [Test]
        public void Initialize_ResultIsNotNull()
        {
            // Act
            var container = StructureMapSetup.Initialize();

            // Assert
            Assert.IsNotNull(container);
        }
    }
}