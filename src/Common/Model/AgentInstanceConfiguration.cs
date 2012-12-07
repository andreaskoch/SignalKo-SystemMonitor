using System.Collections.Generic;

namespace SignalKo.SystemMonitor.Common.Model
{
	public class AgentInstanceConfiguration
	{
		public string MachineName { get; set; }

		public bool AgentIsEnabled { get; set; }

		public SystemPerformanceCollectorDefinition SystemPerformanceCollector { get; set; }

		public HttpStatusCodeCheckDefinition HttpStatusCodeCheck { get; set; }

		public HttpResponseContentCheckDefinition HttpResponseContentCheck { get; set; }

		public HttpResponseTimeCheckDefinition HttpResponseTimeCheck { get; set; }

		public HealthPageCheckDefinition HealthPageCheck { get; set; }

		public SqlCheckDefinition SqlCheck { get; set; }

		public bool IsValid()
		{
			bool systemPerformanceCollectorIsValid = this.SystemPerformanceCollector == null || this.SystemPerformanceCollector.IsValid();
			bool httpStatusCodeCheckIsValid = this.HttpStatusCodeCheck == null || this.HttpStatusCodeCheck.IsValid();
			bool httpResponseContentCheckIsValid = this.HttpResponseContentCheck == null || this.HttpResponseContentCheck.IsValid();
			bool httpResponseTimeCheckIsValid = this.HttpResponseTimeCheck == null || this.HttpResponseTimeCheck.IsValid();
			bool healthCheckIsValid = this.HealthPageCheck == null || this.HealthPageCheck.IsValid();
			bool sqlCheckIsValid = this.SqlCheck == null || this.SqlCheck.IsValid();

			return string.IsNullOrWhiteSpace(this.MachineName) == false && systemPerformanceCollectorIsValid && httpStatusCodeCheckIsValid
			       && httpResponseContentCheckIsValid && httpResponseTimeCheckIsValid && healthCheckIsValid && sqlCheckIsValid;
		}
	}
}