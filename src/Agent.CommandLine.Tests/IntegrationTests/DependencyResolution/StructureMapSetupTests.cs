using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.CommandLine.DependencyResolution;

namespace Agent.CommandLine.Tests.IntegrationTests.DependencyResolution
{
    [TestFixture]
    public class StructureMapSetupTests
    {
        [Test]
        public void Setup_NoMachineNameParameter_ThrowsNoException()
        {
            // Act
            StructureMapSetup.Setup();
        }

        [Test]
        public void Setup_MachineNameParameterIsSet_ThrowsNoException()
        {
            // Arrange
            string machineName = "Test Machine Name";

            // Act
            StructureMapSetup.Setup(machineName);
        }
    }
}