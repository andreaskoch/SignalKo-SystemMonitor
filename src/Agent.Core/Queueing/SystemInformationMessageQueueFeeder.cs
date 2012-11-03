using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Collectors;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueFeeder : IMessageQueueFeeder, IDisposable
    {
        public const int SendIntervalInMilliseconds = 1000;

        private readonly ISystemInformationProvider systemInformationProvider;

        private readonly IMessageQueue<SystemInformation> workQueue;

        private readonly object lockObject = new object();

        private ServiceStatus serviceStatus = ServiceStatus.Stopped;

        public SystemInformationMessageQueueFeeder(ISystemInformationProvider systemInformationProvider, IMessageQueue<SystemInformation> workQueue)
        {
            if (systemInformationProvider == null)
            {
                throw new ArgumentNullException("systemInformationProvider");
            }

            if (workQueue == null)
            {
                throw new ArgumentNullException("workQueue");
            }

            this.systemInformationProvider = systemInformationProvider;
            this.workQueue = workQueue;
        }

        public void Start()
        {
            Monitor.Enter(this.lockObject);

            if (this.serviceStatus == ServiceStatus.Stopped)
            {
                this.serviceStatus = ServiceStatus.Running;
            }

            Monitor.Exit(this.lockObject);

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
                this.workQueue.Enqueue(new SystemInformationQueueItem(systemInfo));
            }

            Monitor.Enter(this.lockObject);

            this.serviceStatus = ServiceStatus.Stopped;

            Monitor.Exit(this.lockObject);
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

        public void Dispose()
        {
            this.Stop();
        }
    }
}