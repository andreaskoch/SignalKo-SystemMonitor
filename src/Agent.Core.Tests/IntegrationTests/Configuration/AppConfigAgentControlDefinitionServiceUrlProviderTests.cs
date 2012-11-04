using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace Agent.Core.Tests.IntegrationTests.Configuration
{
	[TestFixture]
	public class AppConfigAgentControlDefinitionServiceUrlProviderTests
	{
		#region GetServiceConfiguration

		[Test]
		public void GetServiceConfiguration_ResultIsNotNull()
		{
			// Arrange
			var appConfigAgentControlDefinitionServiceUrlProvider = new AppConfigAgentControlDefinitionServiceUrlProvider();

			// Act
			var result = appConfigAgentControlDefinitionServiceUrlProvider.GetServiceConfiguration();

			// Assert
			Assert.IsNotNull(result);
		}

		#endregion
	}
}