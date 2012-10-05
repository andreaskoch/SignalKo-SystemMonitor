using System;
using System.Diagnostics;
using System.Linq;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation
{
    public class LogicalDiscInstanceNameProvider : ILogicalDiscInstanceNameProvider
    {
        private readonly PerformanceCounterCategory logicalDiscCategoryPerformanceCounterCategory;

        private readonly string[] logicalDrivesToExclude = new[] { "_Total" };

        public LogicalDiscInstanceNameProvider()
        {
            this.logicalDiscCategoryPerformanceCounterCategory = new PerformanceCounterCategory("LogicalDisk");
        }

        public string[] GetLogicalDiscInstanceNames()
        {
            return
                this.logicalDiscCategoryPerformanceCounterCategory.GetInstanceNames().Where(
                    instanceName =>
                    this.logicalDrivesToExclude.Any(l => l.Equals(instanceName, StringComparison.OrdinalIgnoreCase))
                    == false).ToArray();
        }
    }
}