using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Collector;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class MessageQueueFeeder : IMessageQueueFeeder
    {
        public const int SendIntervalInMilliseconds = 1000;

        private readonly ISystemInformationProvider systemInformationProvider;

        private readonly IMessageQueue messageQueue;

        private readonly object lockObject = new object();

        private bool stop;

        public MessageQueueFeeder(ISystemInformationProvider systemInformationProvider, IMessageQueue messageQueue)
        {
            if (systemInformationProvider == null)
            {
                throw new ArgumentNullException("systemInformationProvider");
            }

            if (messageQueue == null)
            {
                throw new ArgumentNullException("messageQueue");
            }

            this.systemInformationProvider = systemInformationProvider;
            this.messageQueue = messageQueue;
        }

        public void Start()
        {
            while (true)
            {
                Thread.Sleep(SendIntervalInMilliseconds);

                // check if service has been stopped
                Monitor.Enter(this.lockObject);
                if (this.stop)
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                Monitor.Exit(this.lockObject);

                // retrieve data
                var systemInfo = this.systemInformationProvider.GetSystemInfo();
                if (systemInfo == null)
                {
                    // skip this run
                    continue;
                }

                // add message to queue
                this.messageQueue.Enqueue(systemInfo);
            }
        }

        public void Stop()
        {
            Monitor.Enter(this.lockObject);
            this.stop = true;
            Monitor.Exit(this.lockObject);          
        }
    }
}