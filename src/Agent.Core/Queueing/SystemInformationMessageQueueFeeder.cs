using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueFeeder : IMessageQueueFeeder<SystemInformation>
    {
        public const int SendIntervalInMilliseconds = 1000;

        private readonly ISystemInformationProvider systemInformationProvider;

        private readonly object lockObject = new object();

        private bool stop;

        public SystemInformationMessageQueueFeeder(ISystemInformationProvider systemInformationProvider)
        {
            if (systemInformationProvider == null)
            {
                throw new ArgumentNullException("systemInformationProvider");
            }

            this.systemInformationProvider = systemInformationProvider;
        }

        public void Start(IMessageQueue<SystemInformation> workQueue)
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
                workQueue.Enqueue(new SystemInformationQueueItem(systemInfo));
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