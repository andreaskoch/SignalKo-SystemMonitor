using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Mapper
{
	public class DataCollectorTypeMapper : IDataCollectorTypeMapper
	{
		public DataCollectorType Map(string collectorType)
		{
			if (String.IsNullOrWhiteSpace(collectorType))
			{
				throw new ArgumentException("collectorType");
			}

			switch (collectorType)
			{
				case "Response Time Check":
					return DataCollectorType.HttpResponseTimeCheck;

				case "Web Page Content Check":
					return DataCollectorType.HttpResponseContentCheck;

				case "System Information":
				case "System Performance":
					return DataCollectorType.SystemPerformance;

				case "HTTP Status Code Check":
					return DataCollectorType.HttpStatusCodeCheck;

				case "Health Page Check":
					return DataCollectorType.HealthPageCheck;

				default:
					throw new ArgumentOutOfRangeException(string.Format("{0} is not a valid collector type.", collectorType));
			}
		}
	}
}