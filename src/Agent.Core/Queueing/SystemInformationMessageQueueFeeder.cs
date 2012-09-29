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

        private ServiceStatus serviceStatus = ServiceStatus.Running;

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

                Monitor.Enter(this.lockObject);
                if (this.serviceStatus == ServiceStatus.Stopped)
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                if (this.serviceStatus == ServiceStatus.Paused)
                {
                    Monitor.Exit(this.lockObject);
                    continue;
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

        public void Pause()
        {
            Monitor.Enter(this.lockObject);

            if (this.serviceStatus == ServiceStatus.Running)
            {
                this.serviceStatus = ServiceStatus.Paused;
            }

            Monitor.Exit(this.lockObject);
        }

        public void Resume()
        {
            Monitor.Enter(this.lockObject);

            if (this.serviceStatus == ServiceStatus.Paused)
            {
                this.serviceStatus = ServiceStatus.Running;
            }

            Monitor.Exit(this.lockObject);
        }

        public void Stop()
        {
            Monitor.Enter(this.lockObject);
            this.serviceStatus = ServiceStatus.Stopped;
            Monitor.Exit(this.lockObject);
        }

        public ServiceStatus GetStatus()
        {
            return this.serviceStatus;
        }
    }
}