using System;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

namespace Monitor.Web.Tests.UnitTests.ViewModelOrchestrators
{
    [TestFixture]
    public class MemoryStatusOrchestratorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMemoryUtilizationInPercent_SystemMemoryInformationParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var memoryStatusOrchestrator = new MemoryStatusOrchestrator();

            // Act
            memoryStatusOrchestrator.GetMemoryUtilizationInPercent(null);
        }

        [Test]
        public void GetMemoryUtilizationInPercent_SystemMemoryInformationParameterIsValid_NameParameterIsSet()
        {
            // Arrange
            var systemMemoryInformation = new SystemMemoryInformation();
            var memoryStatusOrchestrator = new MemoryStatusOrchestrator();

            // Act
            var result = memoryStatusOrchestrator.GetMemoryUtilizationInPercent(systemMemoryInformation);

            // Assert
            Assert.IsNotNullOrEmpty(result.Name);
        }

        [Test]
        public void GetMemoryUtilizationInPercent_SystemMemoryInformationParameterIsNotInitialized_ValueIsZero()
        {
            // Arrange
            var systemMemoryInformation = new SystemMemoryInformation();
            var memoryStatusOrchestrator = new MemoryStatusOrchestrator();

            // Act
            var result = memoryStatusOrchestrator.GetMemoryUtilizationInPercent(systemMemoryInformation);

            // Assert
            Assert.AreEqual(0d, result.Value);
        }

        [Test]
        public void GetMemoryUtilizationInPercent_SystemMemoryInformationParameterValuesAreZero_ValueIsZero()
        {
            // Arrange
            var systemMemoryInformation = new SystemMemoryInformation { AvailableMemoryInGB = 0d, UsedMemoryInGB = 0d };
            var memoryStatusOrchestrator = new MemoryStatusOrchestrator();

            // Act
            var result = memoryStatusOrchestrator.GetMemoryUtilizationInPercent(systemMemoryInformation);

            // Assert
            Assert.AreEqual(0d, result.Value);
        }

        [Test]
        public void GetMemoryUtilizationInPercent_SystemMemoryInformationParameterIsValid_ValueIsCorrect()
        {
            // Arrange
            var availableMemory = 9d;
            var usedMemory = 1d;
            var systemMemoryInformation = new SystemMemoryInformation { AvailableMemoryInGB = availableMemory, UsedMemoryInGB = usedMemory };
            var memoryStatusOrchestrator = new MemoryStatusOrchestrator();

            // Act
            var result = memoryStatusOrchestrator.GetMemoryUtilizationInPercent(systemMemoryInformation);

            // Assert
            Assert.AreEqual(10d, result.Value);
        }
    }
}