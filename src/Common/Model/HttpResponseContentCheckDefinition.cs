using System.Text.RegularExpressions;

namespace SignalKo.SystemMonitor.Common.Model
{
	public class HttpResponseContentCheckDefinition
	{
		public int CheckIntervalInSeconds { get; set; }

		public string CheckUrl { get; set; }

		public string Hostheader { get; set; }

		public Regex CheckPattern { get; set; }

		public bool IsValid()
		{
			return CheckIntervalInSeconds > 0 && string.IsNullOrWhiteSpace(this.CheckUrl) == false && this.CheckPattern != null;
		}
	}
}