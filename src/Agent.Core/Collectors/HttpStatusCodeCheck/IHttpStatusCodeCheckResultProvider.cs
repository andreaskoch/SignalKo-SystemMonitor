using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck
{
	public interface IHttpStatusCodeCheckResultProvider
	{
		HttpStatusCodeCheckResult GetHttpStatusCodeCheckResult();
	}
}