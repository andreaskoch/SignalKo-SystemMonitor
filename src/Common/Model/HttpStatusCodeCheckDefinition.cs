namespace SignalKo.SystemMonitor.Common.Model
{
	public class HttpStatusCodeCheckDefinition : ICollectorDefinition
	{
		public DataCollectorType CollectorType { get; set; }

		public int CheckIntervalInSeconds { get; set; }

		public string CheckUrl { get; set; }

		public string Hostheader { get; set; }

		public int ExpectedStatusCode { get; set; }

		public bool IsValid()
		{
			return CollectorType.Equals(DataCollectorType.HttpStatusCodeCheck) && CheckIntervalInSeconds > 0 && string.IsNullOrWhiteSpace(this.CheckUrl) == false
				   && this.ExpectedStatusCode > 0;
		}
	}
}