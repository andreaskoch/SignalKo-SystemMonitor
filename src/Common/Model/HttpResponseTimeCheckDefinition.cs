namespace SignalKo.SystemMonitor.Common.Model
{
	public class HttpResponseTimeCheckDefinition
	{
		public int CheckIntervalInSeconds { get; set; }

		public string CheckUrl { get; set; }

		public string Hostheader { get; set; }

		public int MaxResponseTimeInSeconds { get; set; }

		public bool IsValid()
		{
			return CheckIntervalInSeconds > 0 && string.IsNullOrWhiteSpace(this.CheckUrl) == false && this.MaxResponseTimeInSeconds > 0;
		}
	}
}