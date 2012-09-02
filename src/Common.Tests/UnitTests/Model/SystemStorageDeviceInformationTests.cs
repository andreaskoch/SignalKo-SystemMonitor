using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;

namespace Common.Tests.UnitTests.Model
{
    [TestFixture]
    public class SystemStorageDeviceInformationTests
    {
        #region Default Values

        [Test]
        public void Default_DeviceName_IsNull()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation();

            // Assert
            Assert.IsNull(object1.DeviceName);
        }

        [Test]
        public void Default_FreeDiscSpaceInPercent_IsSetToZero()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation();

            // Assert
            Assert.AreEqual(0.0d, object1.FreeDiscSpaceInPercent);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_DeviceName()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.DeviceName));
        }

        [Test]
        public void ToString_Contains_FreeDiscSpaceInPercent()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.FreeDiscSpaceInPercent.ToString()));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };
            var object2 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoUninitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation();
            var object2 = new SystemStorageDeviceInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoIdenticalObjectsWithDifferentCasing_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:", FreeDiscSpaceInPercent = 30.0d };
            var object2 = new SystemStorageDeviceInformation { DeviceName = "c:", FreeDiscSpaceInPercent = 30.0d };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNull_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };
            SystemStorageDeviceInformation object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };
            var object2 = new SystemStorageDeviceInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };
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
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };
            var object2 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };

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
            var object1 = new SystemStorageDeviceInformation { DeviceName = "C:",  FreeDiscSpaceInPercent = 30.0d };
            var object2 = new SystemStorageDeviceInformation { DeviceName = "E:", FreeDiscSpaceInPercent = 10.0d };

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
            var package1 = new SystemStorageDeviceInformation();
            var package2 = new SystemStorageDeviceInformation();

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
            var deviceName = "C:";
            var freeDiscSpaceInPercent = 30.0d;
            var object1 = new SystemStorageDeviceInformation { DeviceName = deviceName, FreeDiscSpaceInPercent = freeDiscSpaceInPercent };

            int expectedHashcode = object1.GetHashCode();

            for (var i = 0; i < 100; i++)
            {
                // Act
                object1.DeviceName = deviceName;
                object1.FreeDiscSpaceInPercent = freeDiscSpaceInPercent;
                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.AreEqual(expectedHashcode, generatedHashCode);
            }
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            var hashCodes = new Dictionary<int, SystemStorageDeviceInformation>();

            for (var i = 0; i < 1000; i++)
            {
                // Act
                var object1 = new SystemStorageDeviceInformation { DeviceName = i.ToString(), FreeDiscSpaceInPercent = i };

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion                    
    }
}