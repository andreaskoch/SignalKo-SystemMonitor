using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Queuing;

namespace Agent.Core.Tests.IntegrationTests.Queueing
{
    [TestFixture]
    public class SystemInformationMessageQueueTests
    {
        private readonly object dequeueItemsWithTwoParallelThreadsAllItemsAreDequeuedLockObject = new object();

        #region Enqueue

        [Test]
        public void EnqueueAndDequeue_OneMillionItems()
        {
            // Arrange
            int itemCount = 1000000;
            var aLotOfItems = TestUtilities.GetSystemInformationObjects(itemCount);
            var queue = new SystemInformationMessageQueue();

            // Act
            var enqueueWatch = new Stopwatch();
            enqueueWatch.Start();

            foreach (var item in aLotOfItems)
            {
                queue.Enqueue(new SystemInformationQueueItem(item));
            }

            enqueueWatch.Stop();
            Console.WriteLine("Enqueing {0} items took {1} milliseconds.", itemCount, enqueueWatch.ElapsedMilliseconds);

            // Assert
            for (int i = 0; i < aLotOfItems.Length; i++)
            {
                var dequedItem = queue.Dequeue();
                Assert.AreEqual(aLotOfItems[i], dequedItem.Item);
            }
        }

        #endregion

        #region parallel

        [Test]
        public void EnqueueItemsWithTwoParallelThreads_AllItemsAreAddedToQueue()
        {
            // Arrange
            var itemPoolSize = 10000000;
            var itemPool = TestUtilities.GetSystemInformationObjects(itemPoolSize);
            var thread1Items = itemPool.Take(itemPoolSize / 2).ToList();
            var thread2Items = itemPool.Skip(itemPoolSize / 2).Take(itemPoolSize / 2).ToList();

            var queue = new SystemInformationMessageQueue();

            // Act
            var thread1 = new Action(
                () =>
                    {
                        foreach (var item in thread1Items)
                        {
                            queue.Enqueue(new SystemInformationQueueItem(item));
                        }
                    });

            var thread2 = new Action(
                () =>
                {
                    foreach (var item in thread2Items)
                    {
                        queue.Enqueue(new SystemInformationQueueItem(item));
                    }
                });

            var enqueueWatch = new Stopwatch();
            enqueueWatch.Start();

            Parallel.Invoke(thread1, thread2);

            enqueueWatch.Stop();
            Console.WriteLine("Enqueing {0} items with two parallel threads took {1} milliseconds.", itemPoolSize, enqueueWatch.ElapsedMilliseconds);

            // Assert
            Assert.AreEqual(itemPoolSize, queue.GetSize());
        }

        [Test]
        public void DequeueItemsWithTwoParallelThreads_AllItemsAreDequeued()
        {
            // Arrange
            var itemPoolSize = 10000000;
            var itemPool = TestUtilities.GetSystemInformationObjects(itemPoolSize);
            var queue = new SystemInformationMessageQueue();
            foreach (var item in itemPool)
            {
                queue.Enqueue(new SystemInformationQueueItem(item));
            }

            // Act
            var dequeuedItemCount = 0;
            var thread1 = new Action(
                () =>
                {
                    while (queue.Dequeue() != null)
                    {
                        Monitor.Enter(this.dequeueItemsWithTwoParallelThreadsAllItemsAreDequeuedLockObject);
                        dequeuedItemCount++;
                        Monitor.Exit(this.dequeueItemsWithTwoParallelThreadsAllItemsAreDequeuedLockObject);
                    }
                });

            var thread2 = new Action(
                () =>
                {
                    while (queue.Dequeue() != null)
                    {
                        Monitor.Enter(this.dequeueItemsWithTwoParallelThreadsAllItemsAreDequeuedLockObject);
                        dequeuedItemCount++;
                        Monitor.Exit(this.dequeueItemsWithTwoParallelThreadsAllItemsAreDequeuedLockObject);
                    }
                });

            var dequeueWatch = new Stopwatch();
            dequeueWatch.Start();

            Parallel.Invoke(thread1, thread2);

            dequeueWatch.Stop();
            Console.WriteLine("Dequeuing {0} items with two parallel threads took {1} milliseconds.", dequeuedItemCount, dequeueWatch.ElapsedMilliseconds);

            // Assert
            Assert.AreEqual(itemPoolSize, dequeuedItemCount);
        }

        #endregion
    }
}