namespace SignalKo.SystemMonitor.Common.Model
{
	public class SystemPerformanceCollectorDefinition
	{
		public int CheckIntervalInSeconds { get; set; }

		public bool IsValid()
		{
			return CheckIntervalInSeconds > 0;
		}
	}
}