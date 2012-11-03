using System;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Collectors;

namespace Agent.Core.Tests.UnitTests.Collectors
{
    [TestFixture]
    public class CustomMachineNameProviderTests
    {
        #region constructor

        [Test]
        public void Constructor_MachineNameParameterIsNotNullOrEmpty_ObjectIsInstantiated()
        {
            // Arrange
            var machineName = "Some Machine Name";

            // Act
            var customMachineNameProvider = new CustomMachineNameProvider(machineName);

            // Assert
            Assert.IsNotNull(customMachineNameProvider);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_MachineNameParameterIsInvalid_ArgumentExceptionIsThrown(string machineName)
        {
            // Act
            new CustomMachineNameProvider(machineName);
        }

        #endregion

        #region GetMachineName

        [TestCase("A")]
        [TestCase("a")]
        [TestCase("Some Machine")]
        [TestCase("Some whitespace at the end ")]
        [TestCase(" Some whitespace at the beginning")]
        public void GetMachineName_MachineNameIsTheSameValueSuppliedToTheConstructor_ButTrimmed(string machineName)
        {
            // Arrange
            var customMachineNameProvider = new CustomMachineNameProvider(machineName);

            // Act
            var result = customMachineNameProvider.GetMachineName();

            // Assert
            Assert.AreEqual(machineName.Trim(), result);
        }

        #endregion
    }
}