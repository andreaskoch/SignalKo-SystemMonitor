using System;

namespace SignalKo.SystemMonitor.Common.Model
{
	public class SystemPerformanceData
	{
		public ProcessorUtilizationInformation ProcessorStatus { get; set; }

		public SystemMemoryInformation MemoryStatus { get; set; }

		public SystemStorageInformation StorageStatus { get; set; }

		public override string ToString()
		{
			return string.Format(
				"SystemPerformanceData (ProcessorStatus: {0}, MemoryStatus: {1}, StorageStatus: {2})", this.ProcessorStatus, this.MemoryStatus, this.StorageStatus);
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

			var otherObj = obj as SystemPerformanceData;
			if (otherObj != null)
			{
				return this.ToString().Equals(otherObj.ToString(), StringComparison.OrdinalIgnoreCase);
			}

			return false;
		}
	}
}