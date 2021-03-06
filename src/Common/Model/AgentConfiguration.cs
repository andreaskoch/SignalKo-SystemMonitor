﻿namespace SignalKo.SystemMonitor.Common.Model
{
	public class AgentConfiguration
	{
		public AgentConfiguration()
		{
			this.AgentInstanceConfigurations = new AgentInstanceConfiguration[] { };
		}

		public string Hostaddress { get; set; }

		public string Hostname { get; set; }

		public string SystemInformationSenderPath { get; set; }

		public bool AgentsAreEnabled { get; set; }

		public int CheckIntervalInSeconds { get; set; }

		public AgentInstanceConfiguration[] AgentInstanceConfigurations { get; set; }

		public bool IsValid()
		{
			return this.CheckIntervalInSeconds > 0 && string.IsNullOrWhiteSpace(this.SystemInformationSenderPath) == false
				   && string.IsNullOrWhiteSpace(this.Hostaddress) == false;
		}
	}
}