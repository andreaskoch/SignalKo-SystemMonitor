using System;
using System.Linq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queuing;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Queuing
{
    [TestFixture]
    public class SystemInformationMessageQueueTests
    {
        #region Enqueue

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Enqueue_ItemIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            IQueueItem<SystemInformation> item = null;

            // Act
            queue.Enqueue(item);
        }

        [Test]
        public void Enqueue_Item_DequeueReturnsSameItem()
        {
            // Arrange
            var item = new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow };
            var queue = new SystemInformationMessageQueue();

            // Act
            queue.Enqueue(new SystemInformationQueueItem(item));
            var dequeuedItem = queue.Dequeue();

            // Assert
            Assert.AreEqual(item, dequeuedItem.Item);
        }

        [Test]
        public void Enqueue_QueueItemCounterIsIncreasedByOne()
        {
            // Arrange
            var item = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });
            int previousCount = item.EnqueuCount;

            var queue = new SystemInformationMessageQueue();
            queue.Enqueue(item);

            // Act
            var dequeuedItem = queue.Dequeue();

            // Assert
            Assert.AreEqual(previousCount + 1, dequeuedItem.EnqueuCount);
        }

        [Test]
        public void Enqueue_EveryTimeAnItemIsEnqueued_TheItemCounterIsIncreasedByOne()
        {
            // Arrange
            int timesItemIsQueued = 10;
            var item = new SystemInformationQueueItem(new SystemInformation { MachineName = Environment.MachineName, Timestamp = DateTimeOffset.UtcNow });

            var queue = new SystemInformationMessageQueue();

            // Act
            for (int i = 1; i <= timesItemIsQueued; i++)
            {
                queue.Enqueue(item);
                queue.Dequeue();
            }

            // Assert
            Assert.AreEqual(timesItemIsQueued, item.EnqueuCount);
        }

        #endregion

        #region Enqueue Collection

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Enqueue_ItemsParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            IQueueItem<SystemInformation>[] items = null;

            // Act
            queue.Enqueue(items);
        }

        [Test]
        public void Enqueue_EmptyCollection_NoItemIsEnqueued()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            var items = new IQueueItem<SystemInformation>[] { };

            // Act
            queue.Enqueue(items);

            // Assert
            Assert.AreEqual(0, queue.GetSize());
        }

        [Test]
        public void Enqueue_Collection_EachItemInTheSuppliedCollectionIsEnqueued()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            var items = TestUtilities.GetSystemInformationObjects(10).Select(i => new SystemInformationQueueItem(i)).ToArray();

            // Act
            queue.Enqueue(items);

            // Assert
            Assert.AreEqual(items.Length, queue.GetSize());
        }

        #endregion

        #region Dequeue

        [Test]
        public void Dequeue_OnAnEmptyQueue_ResultIsNull()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            // Act
            var result = queue.Dequeue();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Dequeue_FirstItemThatIsAddedIsReturned()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            var firstItem = new SystemInformation { MachineName = "1", Timestamp = DateTimeOffset.UtcNow };
            queue.Enqueue(new SystemInformationQueueItem(firstItem));
            queue.Enqueue(new SystemInformationQueueItem(new SystemInformation { MachineName = "2", Timestamp = DateTimeOffset.UtcNow }));
            queue.Enqueue(new SystemInformationQueueItem(new SystemInformation { MachineName = "3", Timestamp = DateTimeOffset.UtcNow }));

            // Act
            var dequeuedItem = queue.Dequeue();

            // Assert
            Assert.AreEqual(firstItem, dequeuedItem.Item);
        }

        [Test]
        public void Dequeue_ReturnsItemsInTheSameOrderAsTheyHaveBeenAdded()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            var items = new[]
                {
                    new SystemInformation { MachineName = "1", Timestamp = DateTimeOffset.UtcNow },
                    new SystemInformation { MachineName = "2", Timestamp = DateTimeOffset.UtcNow },
                    new SystemInformation { MachineName = "3", Timestamp = DateTimeOffset.UtcNow }
                };

            // Act
            foreach (var item in items)
            {
                queue.Enqueue(new SystemInformationQueueItem(item));
            }

            // Assert
            foreach (var systemInformation in items)
            {
                var dequeuedItem = queue.Dequeue();
                Assert.AreEqual(systemInformation, dequeuedItem.Item);
            }
        }

        #endregion

        #region IsEmpty

        [Test]
        public void NewQueue_IsEmptyReturnsTrue()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            // Act
            var result = queue.IsEmpty();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void NonEmptyQueue_IsEmptyReturnsFalse()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            queue.Enqueue(new SystemInformationQueueItem(new SystemInformation()));

            // Act
            var result = queue.IsEmpty();

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetSize

        [Test]
        public void GetSize_QueueIsEmpty_ResultIsZero()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            // Act
            var result = queue.GetSize();

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetSize_QueueContainsOneItem_ResultIsOne()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            queue.Enqueue(new SystemInformationQueueItem(new SystemInformation()));

            // Act
            var result = queue.GetSize();

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetSize_ResultIsWhatEverTheQueueSizeIs()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            int expectedCount = TestUtilities.GetRandNumber(1, 10000000);
            var items = TestUtilities.GetSystemInformationObjects(expectedCount);
            foreach (var item in items)
            {
                queue.Enqueue(new SystemInformationQueueItem(item));
            }
            

            // Act
            var result = queue.GetSize();

            // Assert
            Assert.AreEqual(expectedCount, result);
        }

        #endregion

        #region PurgeAllItems

        [Test]
        public void PurgeAllItems_OnEmptyQueue_ResultIsEmptyArray()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            
            // Act
            var results = queue.PurgeAllItems();

            // Assert
            Assert.IsEmpty(results);
        }

        [Test]
        public void PurgeAllItems_OnNonEmptyQueue_QueueIsEmptyAfterwards()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();
            queue.Enqueue(new SystemInformationQueueItem(new SystemInformation()));

            // Act
            queue.PurgeAllItems();

            // Assert
            Assert.IsTrue(queue.IsEmpty());
        }

        [Test]
        public void PurgeAllItems_QueueContainsThreeItems_ResultIsArrayWithThreeItems()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            var items = new[]
                {
                    new SystemInformation { MachineName = "1", Timestamp = DateTimeOffset.UtcNow },
                    new SystemInformation { MachineName = "2", Timestamp = DateTimeOffset.UtcNow },
                    new SystemInformation { MachineName = "3", Timestamp = DateTimeOffset.UtcNow }
                };

            foreach (var item in items)
            {
                queue.Enqueue(new SystemInformationQueueItem(item));
            }

            // Act
            var result = queue.PurgeAllItems();

            // Assert
            Assert.AreEqual(items.Length, result.Length);
        }

        [Test]
        public void PurgeAllItems_QueueContainsThreeItems_ResultIsArrayHasTheSameOrderAsTheQueue()
        {
            // Arrange
            var queue = new SystemInformationMessageQueue();

            var items = new[]
                {
                    new SystemInformation { MachineName = "1", Timestamp = DateTimeOffset.UtcNow },
                    new SystemInformation { MachineName = "2", Timestamp = DateTimeOffset.UtcNow },
                    new SystemInformation { MachineName = "3", Timestamp = DateTimeOffset.UtcNow }
                };

            foreach (var item in items)
            {
                queue.Enqueue(new SystemInformationQueueItem(item));
            }

            // Act
            var result = queue.PurgeAllItems();

            // Assert
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(items[i], result[i].Item);
            }
        }

        #endregion
    }
}