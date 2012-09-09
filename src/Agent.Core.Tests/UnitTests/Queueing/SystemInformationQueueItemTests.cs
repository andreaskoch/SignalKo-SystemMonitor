using System;
using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queueing
{
    [TestFixture]
    public class SystemInformationQueueItemTests
    {
        #region constructor

        [Test]
        public void Constructor_SuppliedItemIsSet_ObjectIsInstantiated()
        {
            // Arrange
            var item = new SystemInformation();

            // Act
            var systemInformationQueueItem = new SystemInformationQueueItem(item);

            // Assert
            Assert.IsNotNull(systemInformationQueueItem);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_SuppliedItemIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            SystemInformation item = null;

            // Act
            new SystemInformationQueueItem(item);
        }

        #endregion

        #region EnqueuCount

        [Test]
        public void EnqueuCount_IsZeroWhenToObjectIsFirstCreated()
        {
            // Arrange
            var item = new SystemInformation();

            // Act
            var systemInformationQueueItem = new SystemInformationQueueItem(item);

            // Assert
            Assert.AreEqual(0, systemInformationQueueItem.EnqueuCount);
        }

        [Test]
        public void EnqueuCount_CanBeSet_ValueIsTheValueThatHasBeenAssigned()
        {
            // Arrange
            var newEnqueueCount = 5;
            var item = new SystemInformation();
            var systemInformationQueueItem = new SystemInformationQueueItem(item);

            // Act
            systemInformationQueueItem.EnqueuCount = newEnqueueCount;

            // Assert
            Assert.AreEqual(newEnqueueCount, systemInformationQueueItem.EnqueuCount);
        }

        #endregion

        #region Item

        [Test]
        public void Item_Get_ReturnsTheSameItemThatHasBeenAssignedViaTheConstructor()
        {
            // Arrange
            var assignedItem = new SystemInformation();
            var systemInformationQueueItem = new SystemInformationQueueItem(assignedItem);

            // Act
            var result = systemInformationQueueItem.Item;

            // Assert
            Assert.AreEqual(assignedItem, result);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_Item()
        {
            // Arrange
            var systemInformationItem = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow };
            var object1 = new SystemInformationQueueItem(systemInformationItem);

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.Item.ToString()));
        }

        [Test]
        public void ToString_DoesNotContain_UsedMemoryInGB()
        {
            // Arrange
            var systemInformationItem = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow };
            var object1 = new SystemInformationQueueItem(systemInformationItem) { EnqueuCount = 15 };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsFalse(result.Contains(object1.EnqueuCount.ToString()));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var timeStamp = DateTimeOffset.UtcNow;
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp });
            var object2 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp });

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoIdenticalObjects_WithDifferentEnqueueCountValues_ResultIsTrue()
        {
            // Arrange
            var timeStamp = DateTimeOffset.UtcNow;
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp })
                {
                    EnqueuCount = 1
                };
            var object2 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp })
                {
                    EnqueuCount = 2
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
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });
            SystemInformationQueueItem object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });
            var object2 = new object();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetHashCode

        [Test]
        public void GetHashCode_TwoIdenticalObjects_HashCodesAreEqual()
        {
            // Arrange
            var timeStamp = DateTimeOffset.UtcNow;
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp });
            var object2 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp });

            // Act
            int hashCodeObject1 = object1.GetHashCode();
            int hashCodeObject2 = object2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_TwoIdenticalObjects_WithDifferentEnqueueCountValues_HashCodesAreEqual()
        {
            // Arrange
            var timeStamp = DateTimeOffset.UtcNow;
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp })
                {
                    EnqueuCount = 1
                };

            var object2 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = timeStamp })
                {
                    EnqueuCount = 3
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
            var timeStamp = DateTimeOffset.UtcNow;
            var object1 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName + "A", Timestamp = timeStamp });
            var object2 = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName + "B", Timestamp = timeStamp });

            // Act
            int hashCodeObject1 = object1.GetHashCode();
            int hashCodeObject2 = object2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            int numberOfItems = 10000;
            var items = TestUtilities.GetSystemInformationObjects(numberOfItems);
            var hashCodes = new Dictionary<int, SystemInformationQueueItem>();

            for (var i = 0; i < numberOfItems; i++)
            {
                // Act
                var object1 = new SystemInformationQueueItem(items[i]);

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion                  
    }
}