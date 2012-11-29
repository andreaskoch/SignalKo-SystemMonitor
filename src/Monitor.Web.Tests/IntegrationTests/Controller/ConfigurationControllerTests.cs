using NUnit.Framework;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Common.Model;

namespace Monitor.Web.Tests.IntegrationTests.Controller
{
	[TestFixture]
	public class ConfigurationControllerTests
	{
		[Test]
		 public void Deserialize_AgentConfiguration()
		{
			var json =
				"{\"Hostaddress\":\"http://127.0.0.1:49785\",\"Hostname\":\"localhost\",\"SystemInformationSenderPath\":\"/api/systeminformation\",\"AgentsAreEnabled\":true,\"CheckIntervalInSeconds\":60,\"CheckIntervalHumanReadable\":\"60 seconds\",\"AgentInstanceConfigurations\":[{\"MachineName\":\"ANDYWORKSTATION\",\"AgentIsEnabled\":true,\"CollectorDefinitions\":[{\"CollectorType\":\"System Performance\",\"CheckIntervalInSeconds\":1,\"CheckIntervalHumanReadable\":\"1 seconds\"}],\"SystemPerformanceCollector\":{\"CollectorType\":\"System Performance\",\"CheckIntervalInSeconds\":1,\"CheckIntervalHumanReadable\":\"1 seconds\"},\"HttpStatusCodeCheck\":null,\"HttpResponseContentCheck\":null,\"HttpResponseTimeCheck\":null,\"HealthPageCheck\":null,\"SqlCheck\":null,\"AvailableCollectorTypes\":[\"HTTP Status Code Check\",\"Web Page Content Check\",\"Response Time Check\",\"Health Page Check\",\"Sql Check\"]}],\"NewAgentInstanceConfigurationId\":\"\",\"UnconfiguredAgents\":[\"Machine-B\"]}";

			var result = JsonConvert.DeserializeObject<AgentConfiguration>(json);

			Assert.IsNotNull(result);
		}
	}
}