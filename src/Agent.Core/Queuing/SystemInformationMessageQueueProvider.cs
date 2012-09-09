using System;
using System.Linq;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class SystemInformationMessageQueueProvider : IMessageQueueProvider<SystemInformation>
    {
        private readonly IMessageQueuePersistence<SystemInformation> messageQueuePersistence;

        private readonly IMessageQueue<SystemInformation> workQueue;

        private readonly IMessageQueue<SystemInformation> errorQueue;

        public SystemInformationMessageQueueProvider(IMessageQueuePersistence<SystemInformation> messageQueuePersistence)
        {
            if (messageQueuePersistence == null)
            {
                throw new ArgumentNullException("messageQueuePersistence");
            }

            this.messageQueuePersistence = messageQueuePersistence;
            this.errorQueue = new SystemInformationMessageQueue();
            this.workQueue = new SystemInformationMessageQueue();
        }

        public IMessageQueue<SystemInformation> GetWorkQueue()
        {
            return this.workQueue;
        }

        public IMessageQueue<SystemInformation> GetErrorQueue()
        {
            return this.errorQueue;
        }

        public void Restore()
        {
            var itemsFromPreviousSession = this.messageQueuePersistence.Load();
            if (itemsFromPreviousSession == null)
            {
                return;
            }

            this.workQueue.Enqueue(itemsFromPreviousSession.Select(queueItem => new SystemInformationQueueItem(queueItem.Item)));
        }

        public void Persist()
        {
            var failedRequests = this.errorQueue.PurgeAllItems();
            this.messageQueuePersistence.Save(failedRequests);
        }
    }
}