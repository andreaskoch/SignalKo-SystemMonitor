using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Dto;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public class AgentConfigurationMapper : IAgentConfigurationMapper
	{
		private readonly IAgentInstanceConfigurationMapper agentInstanceConfigurationMapper;

		public AgentConfigurationMapper(IAgentInstanceConfigurationMapper agentInstanceConfigurationMapper)
		{
			this.agentInstanceConfigurationMapper = agentInstanceConfigurationMapper;
		}

		public AgentConfiguration Map(AgentConfigurationDto dto)
		{
			if (dto == null)
			{
				throw new ArgumentNullException("dto");
			}

			var agentConfiguration = new AgentConfiguration
				{
					Hostaddress = dto.Hostaddress,
					Hostname = dto.Hostname,
					SystemInformationSenderPath = dto.SystemInformationSenderPath,
					AgentsAreEnabled = dto.AgentsAreEnabled,
					CheckIntervalInSeconds = dto.CheckIntervalInSeconds,
					AgentInstanceConfigurations =
						dto.AgentInstanceConfigurations != null
							? dto.AgentInstanceConfigurations.Select(this.agentInstanceConfigurationMapper.Map).ToArray()
							: new AgentInstanceConfiguration[] { }
				};

			return agentConfiguration;
		}
	}
}