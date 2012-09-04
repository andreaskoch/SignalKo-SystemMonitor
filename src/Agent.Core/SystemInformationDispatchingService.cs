using System;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.Core.Queuing;

namespace SignalKo.SystemMonitor.Agent.Core
{
    public class SystemInformationDispatchingService : ISystemInformationDispatchingService
    {
        private readonly IMessageQueueFeeder messageQueueFeeder;

        private readonly IMessageQueueWorker messageQueueWorker;

        public SystemInformationDispatchingService(IMessageQueueFeeder messageQueueFeeder, IMessageQueueWorker messageQueueWorker)
        {
            if (messageQueueFeeder == null)
            {
                throw new ArgumentNullException("messageQueueFeeder");
            }

            if (messageQueueWorker == null)
            {
                throw new ArgumentNullException("messageQueueWorker");
            }

            this.messageQueueFeeder = messageQueueFeeder;
            this.messageQueueWorker = messageQueueWorker;
        }

        public void Start()
        {
            Action messageFeederAction = () => this.messageQueueFeeder.Start();
            Action messageWorkerAction = () => this.messageQueueWorker.Start();
            Parallel.Invoke(messageFeederAction, messageWorkerAction);
        }

        public void Stop()
        {
            this.messageQueueFeeder.Stop();
            this.messageQueueWorker.Stop();
        }
    }
}