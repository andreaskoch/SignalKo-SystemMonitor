namespace SignalKo.SystemMonitor.Common.Model
{
	public class SystemPerformanceCollectorDefinition : ICollectorDefinition
	{
		public DataCollectorType CollectorType { get; set; }

		public int CheckIntervalInSeconds { get; set; }

		public bool IsValid()
		{
			return CollectorType.Equals(DataCollectorType.SystemPerformance) && CheckIntervalInSeconds > 0;
		}
	}
}