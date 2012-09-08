using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class SystemInformationMessageQueueWorker : IMessageQueueWorker
    {
        public const int WorkIntervalInMilliseconds = 1000;

        public const int MaxRetryCount = 3;

        private readonly object lockObject = new object();

        private readonly IMessageQueue<SystemInformation> messageQueue;

        private readonly IMessageQueue<SystemInformation> failedRequestQueue;

        private readonly ISystemInformationSender systemInformationSender;

        private bool stop;

        public SystemInformationMessageQueueWorker(IMessageQueue<SystemInformation> messageQueue, IMessageQueue<SystemInformation> failedRequestQueue, ISystemInformationSender systemInformationSender)
        {
            if (messageQueue == null)
            {
                throw new ArgumentNullException("messageQueue");
            }

            if (failedRequestQueue == null)
            {
                throw new ArgumentNullException("failedRequestQueue");
            }

            if (systemInformationSender == null)
            {
                throw new ArgumentNullException("systemInformationSender");
            }

            this.messageQueue = messageQueue;
            this.failedRequestQueue = failedRequestQueue;
            this.systemInformationSender = systemInformationSender;
        }

        public void Start()
        {
            while (true)
            {
                Thread.Sleep(WorkIntervalInMilliseconds);

                // check if service has been stopped
                Monitor.Enter(this.lockObject);
                if (this.stop && this.messageQueue.IsEmpty())
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                Monitor.Exit(this.lockObject);

                // dequeue message
                var queueEntry = this.messageQueue.Dequeue();
                if (queueEntry == null)
                {
                    continue;
                }

                // send messages
                try
                {
                    this.systemInformationSender.Send(queueEntry.Item);
                }
                catch (SendSystemInformationFailedException sendFailedException)
                {
                    // retry later
                    if (queueEntry.EnqueuCount < MaxRetryCount)
                    {
                        this.messageQueue.Enqueue(queueEntry);
                    }
                    else
                    {
                        this.failedRequestQueue.Enqueue(queueEntry);
                    }
                }
                catch (FatalSystemInformationSenderException fatalException)
                {
                    // persist queue and exit
                    Monitor.Enter(this.lockObject);
                    var unfinishedQueueItems = this.messageQueue.PurgeAllItems();
                    this.failedRequestQueue.Enqueue(unfinishedQueueItems);
                    Monitor.Exit(this.lockObject);

                    break;
                }
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