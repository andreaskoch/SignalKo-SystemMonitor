using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.DependencyResolution;

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
	}
}