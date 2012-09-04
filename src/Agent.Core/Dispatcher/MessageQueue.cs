using System;
using System.Collections.Concurrent;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Dispatcher
{
    public class MessageQueue : IMessageQueue
    {
        private readonly ConcurrentQueue<SystemInformation> queue;

        public MessageQueue()
        {
            this.queue = new ConcurrentQueue<SystemInformation>();
        }

        public void Enqueue(SystemInformation systemInformation)
        {
            if (systemInformation == null)
            {
                throw new ArgumentNullException("systemInformation");
            }

            this.queue.Enqueue(systemInformation);
        }

        public SystemInformation Dequeue()
        {
            SystemInformation systemInformation;
            return this.queue.TryDequeue(out systemInformation) ? systemInformation : null;
        }
    }
}