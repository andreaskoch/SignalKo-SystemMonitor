using System;
using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;

namespace Common.Tests.UnitTests.Model
{
    [TestFixture]
    public class SystemInformationTests
    {
        #region Default Values

        [Test]
        public void Default_Timestamp_ISDateTimeMinValue()
        {
            // Arrange
            var object1 = new SystemInformation();

            // Assert
            Assert.AreEqual(DateTime.MinValue, object1.Timestamp);
        }

        [Test]
        public void Default_MachineName_IsNull()
        {
            // Arrange
            var object1 = new SystemInformation();

            // Assert
            Assert.IsNull(object1.MachineName);
        }

        [Test]
        public void Default_ProcessorStatus_IsNull()
        {
            // Arrange
            var object1 = new SystemInformation();

            // Assert
            Assert.IsNull(object1.ProcessorStatus);
        }

        [Test]
        public void Default_MemoryStatus_IsNull()
        {
            // Arrange
            var object1 = new SystemInformation();

            // Assert
            Assert.IsNull(object1.MemoryStatus);
        }

        [Test]
        public void Default_StorageStatus_IsNull()
        {
            // Arrange
            var object1 = new SystemInformation();

            // Assert
            Assert.IsNull(object1.StorageStatus);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_MachineName()
        {
            // Arrange
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = DateTime.UtcNow,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.MachineName));
        }

        [Test]
        public void ToString_Contains_Timestamp()
        {
            // Arrange
            var object1 = new SystemInformation
            {
                MachineName = Environment.MachineName,
                Timestamp = DateTime.UtcNow,
                MemoryStatus = new SystemMemoryInformation(),
                ProcessorStatus = new ProcessorUtilizationInformation(),
                StorageStatus = new SystemStorageInformation()
            };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.Timestamp.ToString()));
        }

        [Test]
        public void ToString_Contains_MemoryStatus()
        {
            // Arrange
            var memorySatus = new SystemMemoryInformation();
            var object1 = new SystemInformation
            {
                MachineName = Environment.MachineName,
                Timestamp = DateTime.UtcNow,
                MemoryStatus = memorySatus,
                ProcessorStatus = new ProcessorUtilizationInformation(),
                StorageStatus = new SystemStorageInformation()
            };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(memorySatus.ToString()));
        }

        [Test]
        public void ToString_Contains_ProcessorStatus()
        {
            // Arrange
            var processorSatus = new ProcessorUtilizationInformation();
            var object1 = new SystemInformation
            {
                MachineName = Environment.MachineName,
                Timestamp = DateTime.UtcNow,
                MemoryStatus = new SystemMemoryInformation(),
                ProcessorStatus = processorSatus,
                StorageStatus = new SystemStorageInformation()
            };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(processorSatus.ToString()));
        }

        [Test]
        public void ToString_Contains_StorageStatus()
        {
            // Arrange
            var storageSatus = new SystemStorageInformation();
            var object1 = new SystemInformation
            {
                MachineName = Environment.MachineName,
                Timestamp = DateTime.UtcNow,
                MemoryStatus = new SystemMemoryInformation(),
                ProcessorStatus = new ProcessorUtilizationInformation(),
                StorageStatus = storageSatus
            };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(storageSatus.ToString()));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var timestamp = DateTime.UtcNow;
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = timestamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
            var object2 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = timestamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoUninitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new SystemInformation();
            var object2 = new SystemInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoIdenticalObjectsWithDifferentCasing_ResultIsTrue()
        {
            // Arrange
            var timestamp = DateTime.UtcNow;
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName.ToLower(),
                    Timestamp = timestamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
            var object2 = new SystemInformation
                {
                    MachineName = Environment.MachineName.ToUpper(),
                    Timestamp = timestamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNull_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = DateTime.UtcNow,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
            SystemInformation object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = DateTime.UtcNow,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
            var object2 = new SystemInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = DateTime.UtcNow,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
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
            var timestamp = DateTime.UtcNow;
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = timestamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
            var object2 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = timestamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };

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
            var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName,
                    Timestamp = DateTime.UtcNow,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };
            var object2 = new SystemInformation
            {
                MachineName = "Different Machine",
                Timestamp = DateTime.UtcNow,
                MemoryStatus = new SystemMemoryInformation(),
                ProcessorStatus = new ProcessorUtilizationInformation(),
                StorageStatus = new SystemStorageInformation()
            };

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
            var package1 = new SystemInformation();
            var package2 = new SystemInformation();

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
            var machineName = Environment.MachineName;
            var timeStamp = DateTime.UtcNow;
            var object1 = new SystemInformation
                {
                    MachineName = machineName,
                    Timestamp = timeStamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };

            int expectedHashcode = object1.GetHashCode();

            for (var i = 0; i < 100; i++)
            {
                // Act
                object1.MachineName = machineName;
                object1.Timestamp = timeStamp;
                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.AreEqual(expectedHashcode, generatedHashCode);
            }
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            var timeStamp = DateTime.UtcNow;
            var hashCodes = new Dictionary<int, SystemInformation>();

            for (var i = 0; i < 1000; i++)
            {
                // Act
                var object1 = new SystemInformation
                {
                    MachineName = Environment.MachineName + i,
                    Timestamp = timeStamp,
                    MemoryStatus = new SystemMemoryInformation(),
                    ProcessorStatus = new ProcessorUtilizationInformation(),
                    StorageStatus = new SystemStorageInformation()
                };

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion
    }
}