using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Service.DependencyResolution;

namespace Agent.Service.Tests.IntegrationTests.DependencyResolution
{
    [TestFixture]
    public class StructureMapSetupTests
    {
        [Test]
        public void Setup()
        {
            // Act
            StructureMapSetup.Setup();
        }
    }
}