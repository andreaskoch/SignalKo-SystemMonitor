using System;
using System.Linq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

namespace Monitor.Web.Tests.UnitTests.ViewModelOrchestrators
{
    [TestFixture]
    public class StorageStatusOrchestratorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStorageUtilizationInPercent_SystemStorageInformationParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var storageStatusOrchestrator = new StorageStatusOrchestrator();

            // Act
            storageStatusOrchestrator.GetStorageUtilizationInPercent(null);
        }

        [Test]
        public void GetStorageUtilizationInPercent_SystemStorageInformationParameterDeviceInfoPropertyIsNull_ResultIsEmpty()
        {
            // Arrange
            var systemStorageInformation = new SystemStorageInformation { StorageDeviceInfos = null };
            var storageStatusOrchestrator = new StorageStatusOrchestrator();

            // Act
            var result = storageStatusOrchestrator.GetStorageUtilizationInPercent(systemStorageInformation);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetStorageUtilizationInPercent_SystemStorageInformationParameterDeviceInfoPropertyIsEmpty_ResultIsEmpty()
        {
            // Arrange
            var systemStorageInformation = new SystemStorageInformation { StorageDeviceInfos = new SystemStorageDeviceInformation[] { } };
            var storageStatusOrchestrator = new StorageStatusOrchestrator();

            // Act
            var result = storageStatusOrchestrator.GetStorageUtilizationInPercent(systemStorageInformation);

            // Assert
            Assert.IsEmpty(result);
        }


        [Test]
        public void GetStorageUtilizationInPercent_SystemStorageInformationParameterDeviceInfoPropertyIsNotEmpty_NameContainsDeviceName()
        {
            // Arrange
            var device1 = new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 10d };
            var systemStorageInformation = new SystemStorageInformation { StorageDeviceInfos = new[] { device1 } };
            var storageStatusOrchestrator = new StorageStatusOrchestrator();

            // Act
            var result = storageStatusOrchestrator.GetStorageUtilizationInPercent(systemStorageInformation);

            // Assert
            Assert.IsTrue(result.First().Name.Contains(device1.DeviceName));
        }

        [Test]
        public void GetStorageUtilizationInPercent_SystemStorageInformationParameterDeviceInfoPropertyIsNotEmpty_ValueIsInverseOfFreeDiscSpaceInPercent()
        {
            // Arrange
            var device1 = new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 10d };
            var systemStorageInformation = new SystemStorageInformation { StorageDeviceInfos = new[] { device1 } };
            var storageStatusOrchestrator = new StorageStatusOrchestrator();

            // Act
            var result = storageStatusOrchestrator.GetStorageUtilizationInPercent(systemStorageInformation);

            // Assert
            Assert.AreEqual(100d - device1.FreeDiscSpaceInPercent, result.First().Value);
        }
    }
}