using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Dto;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public class AgentInstanceConfigurationMapper : IAgentInstanceConfigurationMapper
	{
		private readonly ICollectorDefinitionMapper collectorDefinitionMapper;

		public AgentInstanceConfigurationMapper(ICollectorDefinitionMapper collectorDefinitionMapper)
		{
			if (collectorDefinitionMapper == null)
			{
				throw new ArgumentNullException("collectorDefinitionMapper");
			}

			this.collectorDefinitionMapper = collectorDefinitionMapper;
		}

		public AgentInstanceConfiguration Map(AgentInstanceConfigurationDto dto)
		{
			if (dto == null)
			{
				throw new ArgumentNullException("dto");
			}

			var agentInstanceConfiguration = new AgentInstanceConfiguration
				{
					MachineName = dto.MachineName,
					AgentIsEnabled = dto.AgentIsEnabled,
					CollectorDefinitions =
						dto.CollectorDefinitions != null ? dto.CollectorDefinitions.Select(this.collectorDefinitionMapper.Map).ToArray() : new ICollectorDefinition[] { }
				};

			return agentInstanceConfiguration;
		}
	}
}