using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collector
{
    public class ProcessorStatusProvider : IProcessorStatusProvider, IDisposable
    {
        private readonly PerformanceCounter processorUtilizationInPercentPerformanceCounter;

        private double previousValue;

        public ProcessorStatusProvider()
        {
            this.previousValue = 0.0d;
            this.processorUtilizationInPercentPerformanceCounter = new PerformanceCounter(
                "Processor", "% Processor Time", "_Total");
        }

        public ProcessorUtilizationInformation GetProcessorStatus()
        {
            int maxRetries = 5;
            int retry = 0;
            var values = new List<double>();

            do
            {
                var value = this.processorUtilizationInPercentPerformanceCounter.NextValue();
                if (value > 0d && value < 100d)
                {
                    values.Add(value);
                }

                retry++;
            }
            while (retry < maxRetries);

            double processorTimeInPercent = this.previousValue;
            if (values.Any())
            {
                processorTimeInPercent = values.Average();
                this.previousValue = processorTimeInPercent;
            }

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