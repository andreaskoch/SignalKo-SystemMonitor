using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueue : IMessageQueue<SystemInformation>
    {
        private readonly object lockObject = new object();

        private ConcurrentQueue<IQueueItem<SystemInformation>> queue;

        public SystemInformationMessageQueue()
        {
            this.queue = new ConcurrentQueue<IQueueItem<SystemInformation>>();
        }

        public void Enqueue(IQueueItem<SystemInformation> systemInformationQueueItem)
        {
            if (systemInformationQueueItem == null)
            {
                throw new ArgumentNullException("systemInformationQueueItem");
            }

            systemInformationQueueItem.EnqueuCount++;

            Monitor.Enter(this.lockObject);
            this.queue.Enqueue(systemInformationQueueItem);
            Monitor.Exit(this.lockObject);
        }

        public void Enqueue(IEnumerable<IQueueItem<SystemInformation>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items)
            {
                this.Enqueue(item);
            }
        }

        public IQueueItem<SystemInformation> Dequeue()
        {
            IQueueItem<SystemInformation> systemInformationQueueItem;

            Monitor.Enter(this.lockObject);
            var success = this.queue.TryDequeue(out systemInformationQueueItem);
            Monitor.Exit(this.lockObject);

            return success ? systemInformationQueueItem : null;
        }

        public IQueueItem<SystemInformation>[] PurgeAllItems()
        {
            Monitor.Enter(this.lockObject);
            var items = this.queue.ToArray();
            this.queue = new ConcurrentQueue<IQueueItem<SystemInformation>>();
            Monitor.Exit(this.lockObject);

            return items;
        }

        public int GetSize()
        {
            return this.queue.Count;
        }

        public bool IsEmpty()
        {
            return this.queue.IsEmpty;
        }
    }
}