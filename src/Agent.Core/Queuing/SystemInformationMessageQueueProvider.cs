using System;

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

            this.RestoreLastSession();
        }

        public IMessageQueue<SystemInformation> GetWorkQueue()
        {
            return this.workQueue;
        }

        public IMessageQueue<SystemInformation> GetErrorQueue()
        {
            return this.errorQueue;
        }

        public void Persist()
        {
            
        }

        private void RestoreLastSession()
        {
            var items = this.messageQueuePersistence.Load();
            this.workQueue.Enqueue(items);
        }
    }
}