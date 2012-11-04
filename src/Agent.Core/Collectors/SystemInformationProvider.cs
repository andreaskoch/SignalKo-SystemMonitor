using System;

using SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors
{
	public class SystemInformationProvider : ISystemInformationProvider
	{
		private readonly ITimeProvider timeProvider;

		private readonly ISystemPerformanceDataProvider systemPerformanceDataProvider;

		private readonly IHttpStatusCodeCheckResultProvider httpStatusCodeCheckResultProvider;

		private readonly IMachineNameProvider machineNameProvider;

		public SystemInformationProvider(ITimeProvider timeProvider, IMachineNameProvider machineNameProvider, ISystemPerformanceDataProvider systemPerformanceDataProvider, IHttpStatusCodeCheckResultProvider httpStatusCodeCheckResultProvider)
		{
			if (timeProvider == null)
			{
				throw new ArgumentNullException("timeProvider");
			}

			if (machineNameProvider == null)
			{
				throw new ArgumentNullException("machineNameProvider");
			}

			if (systemPerformanceDataProvider == null)
			{
				throw new ArgumentNullException("systemPerformanceDataProvider");
			}

			if (httpStatusCodeCheckResultProvider == null)
			{
				throw new ArgumentNullException("httpStatusCodeCheckResultProvider");
			}

			this.timeProvider = timeProvider;
			this.machineNameProvider = machineNameProvider;
			this.systemPerformanceDataProvider = systemPerformanceDataProvider;
			this.httpStatusCodeCheckResultProvider = httpStatusCodeCheckResultProvider;
		}

		public Common.Model.SystemInformation GetSystemInfo()
		{
			return new Common.Model.SystemInformation
				{
					Timestamp = this.timeProvider.GetDateAndTime(),
					MachineName = this.machineNameProvider.GetMachineName(),
					SystemPerformance = this.systemPerformanceDataProvider.GetSystemPerformanceData(),
					HttpStatusCodeCheck = this.httpStatusCodeCheckResultProvider.GetHttpStatusCodeCheckResult()
				};
		}
	}
}