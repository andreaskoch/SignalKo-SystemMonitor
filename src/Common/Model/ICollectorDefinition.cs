namespace SignalKo.SystemMonitor.Common.Model
{
	public interface ICollectorDefinition
	{
		DataCollectorType CollectorType { get; set; }

		int CheckIntervalInSeconds { get; set; }

		bool IsValid();
	}
}