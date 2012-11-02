using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using SignalKo.SystemMonitor.Common.Dto;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public class AgentInstanceConfigurationMapper : IAgentInstanceConfigurationMapper
	{
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
					SystemPerformanceCollector = GetSystemPerformanceCollectorDefinition(dto.CollectorDefinitions),
					HttpStatusCodeCheck = GetHttpStatusCodeCheckDefinition(dto.CollectorDefinitions),
					HttpResponseContentCheck = GetHttpResponseContentCheckDefinition(dto.CollectorDefinitions),
					HttpResponseTimeCheck = GetHttpResponseTimeCheckDefinition(dto.CollectorDefinitions),
					HealthPageCheck = GetHealthPageCheckDefinition(dto.CollectorDefinitions)
				};

			return agentInstanceConfiguration;
		}

		private SystemPerformanceCollectorDefinition GetSystemPerformanceCollectorDefinition(IEnumerable<CollectorDefinitionDto> collectorDefinitionDtos)
		{
			if (collectorDefinitionDtos == null)
			{
				return null;
			}

			var dto = collectorDefinitionDtos.FirstOrDefault(d => d.CollectorType.Equals("System Performance"));
			if (dto == null)
			{
				return null;
			}

			return new SystemPerformanceCollectorDefinition { CheckIntervalInSeconds = dto.CheckIntervalInSeconds };
		}

		private HttpStatusCodeCheckDefinition GetHttpStatusCodeCheckDefinition(IEnumerable<CollectorDefinitionDto> collectorDefinitionDtos)
		{
			if (collectorDefinitionDtos == null)
			{
				return null;
			}

			var dto = collectorDefinitionDtos.FirstOrDefault(d => d.CollectorType.Equals("HTTP Status Code Check"));
			if (dto == null)
			{
				return null;
			}

			return new HttpStatusCodeCheckDefinition
				{
					CheckUrl = dto.CheckUrl,
					Hostheader = dto.Hostheader,
					ExpectedStatusCode = dto.ExpectedStatusCode,
					CheckIntervalInSeconds = dto.CheckIntervalInSeconds
				};
		}

		private HttpResponseContentCheckDefinition GetHttpResponseContentCheckDefinition(IEnumerable<CollectorDefinitionDto> collectorDefinitionDtos)
		{
			if (collectorDefinitionDtos == null)
			{
				return null;
			}

			var dto = collectorDefinitionDtos.FirstOrDefault(d => d.CollectorType.Equals("HTTP Page Content Check"));
			if (dto == null)
			{
				return null;
			}

			return new HttpResponseContentCheckDefinition
			{
				CheckUrl = dto.CheckUrl,
				Hostheader = dto.Hostheader,
				CheckPattern = new Regex(dto.CheckPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline),
				CheckIntervalInSeconds = dto.CheckIntervalInSeconds
			};
		}

		private HttpResponseTimeCheckDefinition GetHttpResponseTimeCheckDefinition(IEnumerable<CollectorDefinitionDto> collectorDefinitionDtos)
		{
			if (collectorDefinitionDtos == null)
			{
				return null;
			}

			var dto = collectorDefinitionDtos.FirstOrDefault(d => d.CollectorType.Equals("Response Time Check"));
			if (dto == null)
			{
				return null;
			}

			return new HttpResponseTimeCheckDefinition
			{
				CheckUrl = dto.CheckUrl,
				Hostheader = dto.Hostheader,
				MaxResponseTimeInSeconds = dto.MaxResponseTimeInSeconds,
				CheckIntervalInSeconds = dto.CheckIntervalInSeconds
			};
		}

		private HealthPageCheckDefinition GetHealthPageCheckDefinition(IEnumerable<CollectorDefinitionDto> collectorDefinitionDtos)
		{
			if (collectorDefinitionDtos == null)
			{
				return null;
			}

			var dto = collectorDefinitionDtos.FirstOrDefault(d => d.CollectorType.Equals("Health Page Check"));
			if (dto == null)
			{
				return null;
			}

			return new HealthPageCheckDefinition
			{
				CheckUrl = dto.CheckUrl,
				Hostheader = dto.Hostheader,
				MaxResponseTimeInSeconds = dto.MaxResponseTimeInSeconds,
				CheckIntervalInSeconds = dto.CheckIntervalInSeconds
			};
		}
	}
}