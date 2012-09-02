using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;

namespace Common.Tests.UnitTests.Model
{
    [TestFixture]
    public class SystemStorageInformationTests
    {
        #region Default Values

        [Test]
        public void Default_StorageDeviceInfos_IsNull()
        {
            // Arrange
            var processorUtilizationInformation = new SystemStorageInformation();

            // Assert
            Assert.IsNull(processorUtilizationInformation.StorageDeviceInfos);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_AvailableMemoryInGB()
        {
            // Arrange
            var entry1 = new SystemStorageDeviceInformation();
            var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { entry1 } };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(entry1.ToString()));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemStorageInformation
                { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };
            var object2 = new SystemStorageInformation
                { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoUninitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemStorageInformation();
            var object2 = new SystemStorageInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNull_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemStorageInformation
                { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };

            SystemStorageInformation object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };
            var object2 = new SystemStorageInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };
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
            var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };
            var object2 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };

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
            var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d } } };
            var object2 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "D:", FreeDiscSpaceInPercent = 50.0d } } };

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
            var package1 = new SystemStorageInformation();
            var package2 = new SystemStorageInformation();

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
            var device1 = new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 50.0d };
            var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { device1 } };

            int expectedHashcode = object1.GetHashCode();

            for (var i = 0; i < 100; i++)
            {
                // Act
                object1.StorageDeviceInfos = new[] { device1 };
                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.AreEqual(expectedHashcode, generatedHashCode);
            }
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            var hashCodes = new Dictionary<int, SystemStorageInformation>();

            for (var i = 0; i < 10000; i++)
            {
                // Act
                var object1 = new SystemStorageInformation { StorageDeviceInfos = new[] { new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = i } } };

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion                  
    }
}