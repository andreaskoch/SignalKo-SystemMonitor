using System;
using System.Linq;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace Monitor.Web.Tests.UnitTests.ViewModelOrchestrators
{
    [TestFixture]
    public class SystemStatusOrchestratorTests
    {
        #region constructor
        
        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrage
            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            // Act
            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Assert
            Assert.IsNotNull(systemStatusOrchestrator);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ProcessorStatusOrchestratorParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrage
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            // Act
            new SystemStatusOrchestrator(null, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MemoryStatusOrchestratorParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrage
            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            // Act
            new SystemStatusOrchestrator(processorStatusOrchestrator.Object, null, storageStatusOrchestrator.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_StorageStatusOrchestratorParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrage
            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();

            // Act
            new SystemStatusOrchestrator(processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, null);
        }

        #endregion

        #region GetSystemStatusViewModel

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSystemStatusViewModel_ParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrage
            SystemInformation sytemInformation = null;

            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Act
            systemStatusOrchestrator.GetSystemStatusViewModel(sytemInformation);
        }

        [TestCase("Some Machine Name")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetSystemStatusViewModel_ParameterIsValid_SystemStatusViewModelContainsSuppliedMachineName(string machineName)
        {
            // Arrage
            var sytemInformation = new SystemInformation { MachineName = machineName };

            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Act
            var result = systemStatusOrchestrator.GetSystemStatusViewModel(sytemInformation);

            // Assert
            Assert.AreEqual(machineName, result.MachineName);
        }

        [Test]
        public void GetSystemStatusViewModel_ParameterIsValid_SystemStatusViewModelContainsSuppliedTimestamp()
        {
            // Arrage
            var timestamp = DateTime.UtcNow;
            var sytemInformation = new SystemInformation { Timestamp = timestamp };

            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Act
            var result = systemStatusOrchestrator.GetSystemStatusViewModel(sytemInformation);

            // Assert
            Assert.AreEqual(timestamp, result.Timestamp);
        }

        [Test]
        public void GetSystemStatusViewModel_ProcessorStatusIsNotNull_ResultContainsProcessorStatusDataPoint()
        {
            // Arrage
            var processorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50d };
            var systemInformation = new SystemInformation { ProcessorStatus = processorStatus };
            var processorStatusViewModel = new SystemStatusPointViewModel { Name = "Processor Status", Value = processorStatus.ProcessorUtilizationInPercent };

            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            processorStatusOrchestrator.Setup(p => p.GetProcessorUtilizationInPercent(It.IsAny<ProcessorUtilizationInformation>())).Returns(
                processorStatusViewModel);

            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Act
            var result = systemStatusOrchestrator.GetSystemStatusViewModel(systemInformation);

            // Assert
            Assert.AreEqual(processorStatusViewModel.Value, result.DataPoints.First(d => d.Name.Equals(processorStatusViewModel.Name)).Value);
        }

        [Test]
        public void GetSystemStatusViewModel_MemoryStatusIsNotNull_ResultContainsMemoryStatusDataPoint()
        {
            // Arrage
            var memoryStatus = new SystemMemoryInformation { UsedMemoryInGB = 1d, AvailableMemoryInGB = 10d };
            var systemInformation = new SystemInformation { MemoryStatus = memoryStatus };
            var memoryStatusViewModel = new SystemStatusPointViewModel { Name = "Memory Status", Value = 10d };

            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            memoryStatusOrchestrator.Setup(p => p.GetMemoryUtilizationInPercent(It.IsAny<SystemMemoryInformation>())).Returns(memoryStatusViewModel);

            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();

            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Act
            var result = systemStatusOrchestrator.GetSystemStatusViewModel(systemInformation);

            // Assert
            Assert.AreEqual(memoryStatusViewModel.Value, result.DataPoints.First(d => d.Name.Equals(memoryStatusViewModel.Name)).Value);
        }

        [Test]
        public void GetSystemStatusViewModel_StorageStatusIsNotNull_ResultContainsStorageStatusDataPoint()
        {
            // Arrage
            var deviceInfo1 = new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 20d };
            var deviceInfos = new[] { deviceInfo1 };
            var storageStatus = new SystemStorageInformation { StorageDeviceInfos = deviceInfos };
            var systemInformation = new SystemInformation { StorageStatus = storageStatus };
            var storageStatusViewModel = new SystemStatusPointViewModel
                { Name = "Storage Status" + deviceInfo1.DeviceName, Value = deviceInfo1.FreeDiscSpaceInPercent };

            var processorStatusOrchestrator = new Mock<IProcessorStatusOrchestrator>();
            var memoryStatusOrchestrator = new Mock<IMemoryStatusOrchestrator>();
            var storageStatusOrchestrator = new Mock<IStorageStatusOrchestrator>();
            storageStatusOrchestrator.Setup(s => s.GetStorageUtilizationInPercent(It.IsAny<SystemStorageInformation>())).Returns(
                new[] { storageStatusViewModel });

            var systemStatusOrchestrator = new SystemStatusOrchestrator(
                processorStatusOrchestrator.Object, memoryStatusOrchestrator.Object, storageStatusOrchestrator.Object);

            // Act
            var result = systemStatusOrchestrator.GetSystemStatusViewModel(systemInformation);

            // Assert
            Assert.AreEqual(storageStatusViewModel.Value, result.DataPoints.First(d => d.Name.Equals(storageStatusViewModel.Name)).Value);
        }

        #endregion
    }
}