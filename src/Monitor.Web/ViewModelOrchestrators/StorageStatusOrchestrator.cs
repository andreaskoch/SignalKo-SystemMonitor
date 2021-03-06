using System;
using System.Collections.Generic;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class StorageStatusOrchestrator : IStorageStatusOrchestrator
    {
        private const string StorageUtilizationDataSeriesNamePattern = "Storage Utilization Drive {0} in %";

        public IEnumerable<SystemStatusPointViewModel> GetStorageUtilizationInPercent(SystemStorageInformation systemStorageInformation)
        {
            if (systemStorageInformation == null)
            {
                throw new ArgumentNullException("systemStorageInformation");
            }

            if (systemStorageInformation.StorageDeviceInfos == null || systemStorageInformation.StorageDeviceInfos.Length == 0)
            {
                return new SystemStatusPointViewModel[] { };
            }

            return systemStorageInformation.StorageDeviceInfos.Select(deviceInfo => new SystemStatusPointViewModel
                {
                    Name = string.Format(StorageUtilizationDataSeriesNamePattern, deviceInfo.DeviceName),
                    Value = 100d - deviceInfo.FreeDiscSpaceInPercent
                });
        }
    }
}