using System;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators
{
    public class ProcessorStatusOrchestrator : IProcessorStatusOrchestrator
    {
        private const string CPUUtilizationDataSeriesName = "CPU Utilization in %";

        public SystemStatusPointViewModel GetProcessorUtilizationInPercent(ProcessorUtilizationInformation processorUtilizationInformation)
        {
            if (processorUtilizationInformation == null)
            {
                throw new ArgumentNullException("processorUtilizationInformation");
            }

            return new SystemStatusPointViewModel { Name = CPUUtilizationDataSeriesName, Value = processorUtilizationInformation.ProcessorUtilizationInPercent };
        }
    }
}