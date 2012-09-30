using System;

using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueWorkerFactory : IMessageQueueWorkerFactory
    {
        private readonly ISystemInformationSender systemInformationSender;

        private readonly IMessageQueueProvider<SystemInformation> messageQueueProvider;

        public SystemInformationMessageQueueWorkerFactory(ISystemInformationSender systemInformationSender, IMessageQueueProvider<SystemInformation> messageQueueProvider)
        {
            if (systemInformationSender == null)
            {
                throw new ArgumentNullException("systemInformationSender");
            }

            if (messageQueueProvider == null)
            {
                throw new ArgumentNullException("messageQueueProvider");
            }

            this.systemInformationSender = systemInformationSender;
            this.messageQueueProvider = messageQueueProvider;
        }

        public SystemInformationMessageQueueWorker GetMessageQueueWorker()
        {
            IMessageQueue<SystemInformation> workQueue = this.messageQueueProvider.WorkQueue;
            IMessageQueue<SystemInformation> errorQueue = this.messageQueueProvider.ErrorQueue;

            return new SystemInformationMessageQueueWorker(this.systemInformationSender, workQueue, errorQueue);
        }
    }
}