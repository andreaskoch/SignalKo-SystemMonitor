using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueWorker : IMessageQueueWorker, IDisposable
    {
        public const int WorkIntervalInMilliseconds = 200;

        public const int MaxRetryCount = 3;

        private readonly object lockObject = new object();

        private readonly ISystemInformationSender systemInformationSender;

        private readonly IMessageQueue<SystemInformation> workQueue;

        private readonly IMessageQueue<SystemInformation> errorQueue;

        private ServiceStatus serviceStatus = ServiceStatus.Stopped;

        private bool forceStop = false;

        public SystemInformationMessageQueueWorker(ISystemInformationSender systemInformationSender, IMessageQueue<SystemInformation> workQueue, IMessageQueue<SystemInformation> errorQueue)
        {
            if (systemInformationSender == null)
            {
                throw new ArgumentNullException("systemInformationSender");
            }

            if (workQueue == null)
            {
                throw new ArgumentNullException("workQueue");
            }

            if (errorQueue == null)
            {
                throw new ArgumentNullException("errorQueue");
            }

            this.systemInformationSender = systemInformationSender;
            this.workQueue = workQueue;
            this.errorQueue = errorQueue;
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
                Thread.Sleep(WorkIntervalInMilliseconds);

                Monitor.Enter(this.lockObject);

                if (this.serviceStatus == ServiceStatus.Stopped && (this.workQueue.IsEmpty() || this.forceStop))
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

                // dequeue message
                var queueEntry = this.workQueue.Dequeue();
                if (queueEntry == null)
                {
                    continue;
                }

                // send messages
                try
                {
                    this.systemInformationSender.Send(queueEntry.Item);
                }
                catch (SendSystemInformationFailedException)
                {
                    // retry later
                    if (queueEntry.EnqueuCount < MaxRetryCount)
                    {
                        this.workQueue.Enqueue(queueEntry);
                    }
                    else
                    {
                        this.errorQueue.Enqueue(queueEntry);
                    }
                }
                catch (FatalSystemInformationSenderException)
                {
                    // persist queue and exit
                    Monitor.Enter(this.lockObject);

                    var unfinishedQueueItems = this.workQueue.PurgeAllItems();
                    this.errorQueue.Enqueue(unfinishedQueueItems);

                    this.serviceStatus = ServiceStatus.Stopped;

                    Monitor.Exit(this.lockObject);

                    break;
                }
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

        public void Dispose()
        {
            Monitor.Enter(this.lockObject);
            this.forceStop = true;
            Monitor.Exit(this.lockObject);
            
            this.Stop();
        }
    }
}