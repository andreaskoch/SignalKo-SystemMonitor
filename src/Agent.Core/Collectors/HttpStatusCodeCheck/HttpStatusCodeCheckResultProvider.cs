using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck
{
	public class HttpStatusCodeCheckResultProvider : IHttpStatusCodeCheckResultProvider
	{
		public const int DefaultCheckIntervalInSeconds = 60;

		private readonly IAgentControlDefinitionProvider agentControlDefinitionProvider;

		private readonly IHttpStatusCodeFetcher httpStatusCodeFetcher;

		private readonly Timer timer;

		private HttpStatusCodeCheckResult httpStatusCodeCheckResult;

		public HttpStatusCodeCheckResultProvider(IAgentControlDefinitionProvider agentControlDefinitionProvider, IHttpStatusCodeFetcher httpStatusCodeFetcher)
		{
			if (agentControlDefinitionProvider == null)
			{
				throw new ArgumentNullException("agentControlDefinitionProvider");
			}

			if (httpStatusCodeFetcher == null)
			{
				throw new ArgumentNullException("httpStatusCodeFetcher");
			}

			this.agentControlDefinitionProvider = agentControlDefinitionProvider;
			this.httpStatusCodeFetcher = httpStatusCodeFetcher;

			// get initial check interval
			var agentControlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
			int checkIntervalInSeconds = DefaultCheckIntervalInSeconds;
			if (agentControlDefinition != null && agentControlDefinition.HttpStatusCodeCheck != null && agentControlDefinition.HttpStatusCodeCheck.CheckIntervalInSeconds > 0)
			{
				checkIntervalInSeconds = agentControlDefinition.HttpStatusCodeCheck.CheckIntervalInSeconds;
			}

			var timerStartTime = new TimeSpan(0, 0, 0);
			var timerInterval = new TimeSpan(0, 0, 0, checkIntervalInSeconds);
			this.timer = new Timer(state => this.UpdateHttpStatusCodeResult(), null, timerStartTime, timerInterval);
		}

		public HttpStatusCodeCheckResult GetHttpStatusCodeCheckResult()
		{
			return this.httpStatusCodeCheckResult;
		}

		private void UpdateHttpStatusCodeResult()
		{
			// get latest control definition
			var controlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
			if (controlDefinition == null || controlDefinition.HttpStatusCodeCheck == null)
			{
				return;
			}

			var httpStatusCodeCheckSettings = controlDefinition.HttpStatusCodeCheck;

			// get the latest http status code result
			this.httpStatusCodeCheckResult = this.httpStatusCodeFetcher.GetHttpStatusCode(httpStatusCodeCheckSettings);

			// update the check interval
			if (httpStatusCodeCheckSettings.CheckIntervalInSeconds > 0)
			{
				var timerStartTime = new TimeSpan(0, 0, httpStatusCodeCheckSettings.CheckIntervalInSeconds);
				var timerInterval = new TimeSpan(0, 0, 0, httpStatusCodeCheckSettings.CheckIntervalInSeconds);
				this.timer.Change(timerStartTime, timerInterval);
			}
		}
	}
}