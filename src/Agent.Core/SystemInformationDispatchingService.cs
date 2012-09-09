using System;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.Core.Queuing;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core
{
    public class SystemInformationDispatchingService : ISystemInformationDispatchingService
    {
        private readonly IMessageQueueFeeder messageQueueFeeder;

        private readonly IMessageQueueWorker messageQueueWorker;

        private readonly IMessageQueueProvider<SystemInformation> messageQueueProvider;

        public SystemInformationDispatchingService(IMessageQueueFeeder messageQueueFeeder, IMessageQueueWorker messageQueueWorker, IMessageQueueProvider<SystemInformation> messageQueueProvider)
        {
            if (messageQueueFeeder == null)
            {
                throw new ArgumentNullException("messageQueueFeeder");
            }

            if (messageQueueWorker == null)
            {
                throw new ArgumentNullException("messageQueueWorker");
            }

            if (messageQueueProvider == null)
            {
                throw new ArgumentNullException("messageQueueProvider");
            }

            this.messageQueueFeeder = messageQueueFeeder;
            this.messageQueueWorker = messageQueueWorker;
            this.messageQueueProvider = messageQueueProvider;
        }

        public void Start()
        {
            this.messageQueueProvider.Restore();

            Action messageFeederAction = () => this.messageQueueFeeder.Start();
            Action messageWorkerAction = () => this.messageQueueWorker.Start();
            Parallel.Invoke(messageFeederAction, messageWorkerAction);

            this.messageQueueProvider.Persist();
        }

        public void Stop()
        {
            this.messageQueueFeeder.Stop();
            this.messageQueueWorker.Stop();
        }
    }
}