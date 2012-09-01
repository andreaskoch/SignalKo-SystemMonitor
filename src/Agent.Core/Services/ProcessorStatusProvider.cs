using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Services
{
    public class ProcessorStatusProvider : IProcessorStatusProvider, IDisposable
    {
        private readonly PerformanceCounter processorUtilizationInPercentPerformanceCounter;

        public ProcessorStatusProvider()
        {
            this.processorUtilizationInPercentPerformanceCounter = new PerformanceCounter(
                "Processor", "% Processor Time", "_Total");
        }

        public ProcessorUtilizationInformation GetProcessorUtilizationInPercent()
        {
            int maxRetries = 5;
            int retry = 0;
            var values = new List<double>();
            do
            {
                var value = this.processorUtilizationInPercentPerformanceCounter.NextValue();
                if (value > 0d)
                {
                    values.Add(value);
                }

                retry++;
            }
            while (retry < maxRetries);

            double processorTimeInPercent = values.Any() ? values.Average() : 0d;
            return new ProcessorUtilizationInformation
                {
                    ProcessorUtilizationInPercent = processorTimeInPercent
                };
        }

        public void Dispose()
        {
            try
            {
                if (this.processorUtilizationInPercentPerformanceCounter != null)
                {
                    this.processorUtilizationInPercentPerformanceCounter.Dispose();
                }
            }
            finally
            {
                PerformanceCounter.CloseSharedResources();
            }
        }
    }
}