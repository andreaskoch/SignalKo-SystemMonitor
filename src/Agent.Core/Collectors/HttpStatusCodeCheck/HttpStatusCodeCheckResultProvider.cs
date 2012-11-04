using System;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck
{
	public class HttpStatusCodeCheckResultProvider : IHttpStatusCodeCheckResultProvider
	{
		private readonly IAgentControlDefinitionProvider agentControlDefinitionProvider;

		public HttpStatusCodeCheckResultProvider(IAgentControlDefinitionProvider agentControlDefinitionProvider)
		{
			if (agentControlDefinitionProvider == null)
			{
				throw new ArgumentNullException("agentControlDefinitionProvider");
			}

			this.agentControlDefinitionProvider = agentControlDefinitionProvider;
		}

		public HttpStatusCodeCheckResult GetHttpStatusCodeCheckResult()
		{
			var agentControlDefinition = this.agentControlDefinitionProvider.GetControlDefinition();
			if (agentControlDefinition == null)
			{
				return null;
			}
			
			throw new NotImplementedException();
		}
	}
}