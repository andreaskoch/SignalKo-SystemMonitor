using System;
using System.Collections.Concurrent;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class SystemInformationMessageQueue : IMessageQueue<SystemInformation>
    {
        private readonly ConcurrentQueue<IQueueItem<SystemInformation>> queue;

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
            this.queue.Enqueue(systemInformationQueueItem);
        }

        public IQueueItem<SystemInformation> Dequeue()
        {
            IQueueItem<SystemInformation> systemInformationQueueItem;
            return this.queue.TryDequeue(out systemInformationQueueItem) ? systemInformationQueueItem : null;
        }

        public bool IsEmpty()
        {
            return this.queue.IsEmpty;
        }
    }
}