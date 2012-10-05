using System.Collections.Generic;
using System.Diagnostics;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
{
    public class SystemStorageStatusProvider : ISystemStorageStatusProvider
    {
        private readonly Dictionary<string, PerformanceCounter> freeDiskSpaceInPercentPerformanceCounters = new Dictionary<string, PerformanceCounter>();

        public SystemStorageStatusProvider(ILogicalDiscInstanceNameProvider logicalDiscInstanceNameProvider)
        {
            var logicalDiscNames = logicalDiscInstanceNameProvider.GetLogicalDiscInstanceNames();
            foreach (var logicalDiscName in logicalDiscNames)
            {
                this.freeDiskSpaceInPercentPerformanceCounters.Add(logicalDiscName, new PerformanceCounter("LogicalDisk", "% Free Space", logicalDiscName));
            }
        }

        public SystemStorageInformation GetStorageStatus()
        {
            var storageDeviceInfos = new List<SystemStorageDeviceInformation>();

            foreach (KeyValuePair<string, PerformanceCounter> deviceNamePerformanceCounterPair in this.freeDiskSpaceInPercentPerformanceCounters)
            {
                string deviceName = deviceNamePerformanceCounterPair.Key;
                double freeDiscSpaceInPercent = deviceNamePerformanceCounterPair.Value.NextValue();

                storageDeviceInfos.Add(new SystemStorageDeviceInformation
                    {
                        DeviceName = deviceName,
                        FreeDiscSpaceInPercent = freeDiscSpaceInPercent
                    });
            }

            return new SystemStorageInformation
                {
                    StorageDeviceInfos = storageDeviceInfos.ToArray()
                };
        }
    }
}