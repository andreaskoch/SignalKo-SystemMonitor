using System;
using System.Text.RegularExpressions;

using SignalKo.SystemMonitor.Common.Dto;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public class CollectorDefinitionMapper : ICollectorDefinitionMapper
	{
		private readonly IDataCollectorTypeMapper dataCollectorTypeMapper;

		public CollectorDefinitionMapper(IDataCollectorTypeMapper dataCollectorTypeMapper)
		{
			if (dataCollectorTypeMapper == null)
			{
				throw new ArgumentNullException("dataCollectorTypeMapper");
			}

			this.dataCollectorTypeMapper = dataCollectorTypeMapper;
		}

		public ICollectorDefinition Map(CollectorDefinitionDto dto)
		{
			var collectorType = this.dataCollectorTypeMapper.Map(dto.CollectorType);
			switch (collectorType)
			{
				case DataCollectorType.SystemPerformance:
					return new SystemPerformanceCollectorDefinition
						{
							CollectorType = collectorType,
							CheckIntervalInSeconds = dto.CheckIntervalInSeconds
						};

				case DataCollectorType.HttpStatusCodeCheck:
					return new HttpStatusCodeCheckDefinition
						{
							CollectorType = collectorType,
							CheckIntervalInSeconds = dto.CheckIntervalInSeconds,
							CheckUrl = dto.CheckUrl,
							Hostheader = dto.Hostheader,
							ExpectedStatusCode = dto.ExpectedStatusCode
						};

				case DataCollectorType.HttpResponseContentCheck:
					return new HttpResponseContentCheckDefinition
						{
							CollectorType = collectorType,
							CheckIntervalInSeconds = dto.CheckIntervalInSeconds,
							CheckUrl = dto.CheckUrl,
							Hostheader = dto.Hostheader,
							CheckPattern = new Regex(dto.CheckPattern)
						};

				case DataCollectorType.HttpResponseTimeCheck:
					return new HttpResponseTimeCheckDefinition
						{
							CollectorType = collectorType,
							CheckIntervalInSeconds = dto.CheckIntervalInSeconds,
							CheckUrl = dto.CheckUrl,
							Hostheader = dto.Hostheader,
							MaxResponseTimeInMilliseconds = dto.MaxResponseTimeInSeconds
						};

				case DataCollectorType.HealthPageCheck:
					return new HealthPageCheckDefinition
						{
							CollectorType = collectorType,
							CheckIntervalInSeconds = dto.CheckIntervalInSeconds,
							CheckUrl = dto.CheckUrl,
							Hostheader = dto.Hostheader,
							MaxResponseTimeInMilliseconds = dto.MaxResponseTimeInSeconds
						};

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}