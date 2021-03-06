﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Core.Services;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers.Api
{
	public class AgentControlDefinitionController : ApiController
	{
		private readonly IAgentControlDefinitionService agentControlDefinitionService;

		public AgentControlDefinitionController(IAgentControlDefinitionService agentControlDefinitionService)
		{
			if (agentControlDefinitionService == null)
			{
				throw new ArgumentNullException("agentControlDefinitionService");
			}

			this.agentControlDefinitionService = agentControlDefinitionService;
		}

		// GET api/agentcontroldefinition/5
		public AgentControlDefinition Get(string machineName)
		{
			if (string.IsNullOrWhiteSpace(machineName))
			{
				throw new ArgumentException("machineName");
			}

			return this.agentControlDefinitionService.GetAgentControlDefinition(machineName);
		}
	}
}
