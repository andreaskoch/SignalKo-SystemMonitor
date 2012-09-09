using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueProvider : IMessageQueueProvider<SystemInformation>
    {
        public SystemInformationMessageQueueProvider(IMessageQueue<SystemInformation> workQueue, IMessageQueue<SystemInformation> errorQueue)
        {
            if (workQueue == null)
            {
                throw new ArgumentNullException("workQueue");
            }

            if (errorQueue == null)
            {
                throw new ArgumentNullException("errorQueue");
            }

            this.WorkQueue = workQueue;
            this.ErrorQueue = errorQueue;
        }

        public IMessageQueue<SystemInformation> WorkQueue { get; private set; }

        public IMessageQueue<SystemInformation> ErrorQueue { get; private set; }
    }
}