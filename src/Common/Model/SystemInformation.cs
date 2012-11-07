using System;

namespace SignalKo.SystemMonitor.Common.Model
{
	public class SystemInformation
	{
		public DateTime Timestamp { get; set; }

		public string MachineName { get; set; }

		public SystemPerformanceData SystemPerformance { get; set; }

		public HttpStatusCodeCheckResult HttpStatusCodeCheck { get; set; }

		public override string ToString()
		{
			return string.Format(
				"SystemInformation (Timestamp: {0}, MachineName: {1}, SystemPerformance: {2})", this.Timestamp.ToString(), this.MachineName, this.SystemPerformance);
		}

		public override int GetHashCode()
		{
			int hash = 37;
			hash = (hash * 23) + this.ToString().GetHashCode();
			return hash;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			var otherObj = obj as SystemInformation;
			if (otherObj != null)
			{
				return this.ToString().Equals(otherObj.ToString(), StringComparison.OrdinalIgnoreCase);
			}

			return false;
		}
	}
}