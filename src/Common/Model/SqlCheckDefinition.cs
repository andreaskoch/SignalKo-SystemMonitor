namespace SignalKo.SystemMonitor.Common.Model
{
	public class SqlCheckDefinition
	{
		public int CheckIntervalInSeconds { get; set; }

		public string ConnectionString { get; set; }

		public string SqlQuery { get; set; }

		public bool IsValid()
		{
			return CheckIntervalInSeconds > 0 && string.IsNullOrWhiteSpace(this.ConnectionString) == false && string.IsNullOrWhiteSpace(this.SqlQuery) == false;
		}
	}
}