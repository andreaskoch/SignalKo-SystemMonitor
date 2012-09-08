using System;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.UnitTests.Collector
{
    [TestFixture]
    public class SystemInformationProviderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var timeProvider = new Mock<ITimeProvider>();
            var machineNameProvider = new Mock<IMachineNameProvider>();
            var processorStatusProvider = new Mock<IProcessorStatusProvider>();
            var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
            var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

            // Act
            var systemInformationProvider = new SystemInformationProvider(
                timeProvider.Object,
                machineNameProvider.Object,
                processorStatusProvider.Object,
                systemMemoryStatusProvider.Object,
                systemStorageStatusProvider.Object);

            // Assert
            Assert.IsNotNull(systemInformationProvider);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_TimeProviderParametersIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var machineNameProvider = new Mock<IMachineNameProvider>();
            var processorStatusProvider = new Mock<IProcessorStatusProvider>();
            var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
            var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

            // Act
            new SystemInformationProvider(
                null,
                machineNameProvider.Object,
                processorStatusProvider.Object,
                systemMemoryStatusProvider.Object,
                systemStorageStatusProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MachineNameProviderParametersIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var timeProvider = new Mock<ITimeProvider>();
            var processorStatusProvider = new Mock<IProcessorStatusProvider>();
            var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
            var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

            // Act
            new SystemInformationProvider(
                timeProvider.Object,
                null,
                processorStatusProvider.Object,
                systemMemoryStatusProvider.Object,
                systemStorageStatusProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ProcessorStatusProviderParametersIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var timeProvider = new Mock<ITimeProvider>();
            var machineNameProvider = new Mock<IMachineNameProvider>();
            var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();
            var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

            // Act
            new SystemInformationProvider(
                timeProvider.Object,
                machineNameProvider.Object,
                null,
                systemMemoryStatusProvider.Object,
                systemStorageStatusProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemMemoryStatusProviderParametersIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var timeProvider = new Mock<ITimeProvider>();
            var machineNameProvider = new Mock<IMachineNameProvider>();
            var processorStatusProvider = new Mock<IProcessorStatusProvider>();
            var systemStorageStatusProvider = new Mock<ISystemStorageStatusProvider>();

            // Act
            new SystemInformationProvider(
                timeProvider.Object,
                machineNameProvider.Object,
                processorStatusProvider.Object,
                null,
                systemStorageStatusProvider.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SystemStorageStatusProviderParametersIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var timeProvider = new Mock<ITimeProvider>();
            var machineNameProvider = new Mock<IMachineNameProvider>();
            var processorStatusProvider = new Mock<IProcessorStatusProvider>();
            var systemMemoryStatusProvider = new Mock<ISystemMemoryStatusProvider>();

            // Act
            new SystemInformationProvider(
                timeProvider.Object,
                machineNameProvider.Object,
                processorStatusProvider.Object,
                systemMemoryStatusProvider.Object,
                null);
        }

        #endregion
    }
}