using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;

namespace Common.Tests.UnitTests.Model
{
    [TestFixture]
    public class SystemMemoryInformationTests
    {
        #region Default Values

        [Test]
        public void Default_AvailableMemoryInGB_IsSetToZero()
        {
            // Arrange
            var processorUtilizationInformation = new SystemMemoryInformation();

            // Assert
            Assert.AreEqual(0.0d, processorUtilizationInformation.AvailableMemoryInGB);
        }

        [Test]
        public void Default_UsedMemoryInGB_IsSetToZero()
        {
            // Arrange
            var processorUtilizationInformation = new SystemMemoryInformation();

            // Assert
            Assert.AreEqual(0.0d, processorUtilizationInformation.UsedMemoryInGB);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_AvailableMemoryInGB()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.AvailableMemoryInGB.ToString()));
        }

        [Test]
        public void ToString_Contains_UsedMemoryInGB()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.UsedMemoryInGB.ToString()));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };
            var object2 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoUninitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemMemoryInformation();
            var object2 = new SystemMemoryInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNull_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };
            SystemMemoryInformation object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };
            var object2 = new SystemMemoryInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };
            var object2 = new object();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetHashCode

        [Test]
        public void GetHashCode_TwoIdenticalObjects_BothInitialized_HashCodesAreEqual()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };
            var object2 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };

            // Act
            int hashCodeObject1 = object1.GetHashCode();
            int hashCodeObject2 = object2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_TwoDistinctObjects_HashCodesAreDifferent()
        {
            // Arrange
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = 8.0d, UsedMemoryInGB = 3.0d };
            var object2 = new SystemMemoryInformation { AvailableMemoryInGB = 16.0d, UsedMemoryInGB = 7.0d };

            // Act
            int hashCodeObject1 = object1.GetHashCode();
            int hashCodeObject2 = object2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_TwoIdenticalObjects_BothUninitialized_HashCodesAreEqual()
        {
            // Arrange
            var package1 = new SystemMemoryInformation();
            var package2 = new SystemMemoryInformation();

            // Act
            int hashCodeObject1 = package1.GetHashCode();
            int hashCodeObject2 = package2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_SameHashCodeIsReturnedEveryTimeTheMethodIsCalled_AsLongAsTheObjectDoesNotChange()
        {
            // Arrange
            var availableMemoryInGB = 8.0d;
            var usedMemoryInGB = 3.0d;
            var object1 = new SystemMemoryInformation { AvailableMemoryInGB = availableMemoryInGB, UsedMemoryInGB = usedMemoryInGB };

            int expectedHashcode = object1.GetHashCode();

            for (var i = 0; i < 100; i++)
            {
                // Act
                object1.AvailableMemoryInGB = availableMemoryInGB;
                object1.UsedMemoryInGB = usedMemoryInGB;
                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.AreEqual(expectedHashcode, generatedHashCode);
            }
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            var hashCodes = new Dictionary<int, SystemMemoryInformation>();

            for (var i = 0; i < 1000; i++)
            {
                // Act
                var object1 = new SystemMemoryInformation { AvailableMemoryInGB = i, UsedMemoryInGB = i / 2.0d };

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion                  
    }
}