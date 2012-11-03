namespace SignalKo.SystemMonitor.Common.Dto
{
	public class CollectorDefinitionDto
	{
		public string CollectorType { get; set; }

		public string CheckUrl { get; set; }

		public string Hostheader { get; set; }

		public int CheckIntervalInSeconds { get; set; }

		public int ExpectedStatusCode { get; set; }

		public int MaxResponseTimeInSeconds { get; set; }

		public string CheckPattern { get; set; }

		public string ConnectionString { get; set; }

		public string SqlQuery { get; set; }
	}
}