using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation;

namespace Agent.Core.Tests.IntegrationTests.Collectors.SystemInformation
{
    [TestFixture]
    public class ProcessorStatusProviderTests
    {
        [Test]
        public void GetProcessorUtilizationInPercent()
        {
            // Arrange
            var processorStatusProvider = new ProcessorStatusProvider();

            // Act
            var result = processorStatusProvider.GetProcessorStatus();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetProcessorUtilizationInPercent_Every10Milliseconds_For5Seconds()
        {
            // Arrange
            int durationInMilliseconds = 5 * 1000;
            int waitPeriodInMilliseconds = 100;
            int timeWaited = 0;
            var values = new List<double>();

            // Act
            using (var processorStatusProvider = new ProcessorStatusProvider())
            {
                do
                {
                    values.Add(processorStatusProvider.GetProcessorStatus().ProcessorUtilizationInPercent);
                    Thread.Sleep(waitPeriodInMilliseconds);
                    timeWaited += waitPeriodInMilliseconds;
                }
                while (timeWaited <= durationInMilliseconds);
            }

            // Assert
            Assert.AreNotEqual(0d, values.Average());
        }
    }
}