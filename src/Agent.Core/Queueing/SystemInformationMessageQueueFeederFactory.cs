using System;

using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemInformation;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueFeederFactory : IMessageQueueFeederFactory
    {
        private readonly ISystemInformationProvider systemInformationProvider;

        private readonly IMessageQueueProvider<SystemInformation> messageQueueProvider;

        public SystemInformationMessageQueueFeederFactory(ISystemInformationProvider systemInformationProvider, IMessageQueueProvider<SystemInformation> messageQueueProvider)
        {
            if (systemInformationProvider == null)
            {
                throw new ArgumentNullException("systemInformationProvider");
            }

            if (messageQueueProvider == null)
            {
                throw new ArgumentNullException("messageQueueProvider");
            }

            this.systemInformationProvider = systemInformationProvider;
            this.messageQueueProvider = messageQueueProvider;
        }

        public IMessageQueueFeeder GetMessageQueueFeeder()
        {
            IMessageQueue<SystemInformation> workQueue = this.messageQueueProvider.WorkQueue;
            return new SystemInformationMessageQueueFeeder(this.systemInformationProvider, workQueue);
        }
    }
}