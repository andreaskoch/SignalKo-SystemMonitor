using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck
{
	public interface IHttpStatusCodeFetcher
	{
		HttpStatusCodeCheckResult GetHttpStatusCode(HttpStatusCodeCheckDefinition statusCodeCheckSettings);
	}
}