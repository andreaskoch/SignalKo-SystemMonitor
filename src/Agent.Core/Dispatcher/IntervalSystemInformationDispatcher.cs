using System;
using System.Threading;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.Core.Collector;

namespace SignalKo.SystemMonitor.Agent.Core.Dispatcher
{
    public class IntervalSystemInformationDispatcher : ISystemInformationDispatcher
    {
        public const int SendIntervalInMilliseconds = 1000;

        private readonly ISystemInformationProvider systemInformationProvider;

        private readonly IMessageQueue messageQueue;

        private readonly IMessageQueueWorker messageQueueWorker;

        private readonly object lockObject = new object();

        private bool stop;

        public IntervalSystemInformationDispatcher(ISystemInformationProvider systemInformationProvider, IMessageQueue messageQueue, IMessageQueueWorker messageQueueWorker)
        {
            if (systemInformationProvider == null)
            {
                throw new ArgumentNullException("systemInformationProvider");
            }

            if (messageQueue == null)
            {
                throw new ArgumentNullException("messageQueue");
            }

            if (messageQueueWorker == null)
            {
                throw new ArgumentNullException("messageQueueWorker");
            }

            this.systemInformationProvider = systemInformationProvider;
            this.messageQueue = messageQueue;
            this.messageQueueWorker = messageQueueWorker;
        }

        public void Start()
        {
            var collector = new Task(
                () =>
                    {
                        while (true)
                        {
                            Thread.Sleep(SendIntervalInMilliseconds);

                            // check if service has been stopped
                            Monitor.Enter(this.lockObject);
                            if (this.stop)
                            {
                                this.messageQueueWorker.Stop();

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
                    });

            var queueAgent = new Task(() => this.messageQueueWorker.Start());

            collector.Start();
            queueAgent.Start();
            Task.WaitAll(collector, queueAgent);
        }

        public void Stop()
        {
            Monitor.Enter(this.lockObject);
            this.stop = true;
            Monitor.Exit(this.lockObject);          
        }
    }
}