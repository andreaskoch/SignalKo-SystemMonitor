using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public class SystemInformationMessageQueueWorker : IMessageQueueWorker<SystemInformation>
    {
        public const int WorkIntervalInMilliseconds = 200;

        public const int MaxRetryCount = 3;

        private readonly object lockObject = new object();

        private readonly ISystemInformationSender systemInformationSender;

        private bool stop;

        public SystemInformationMessageQueueWorker(ISystemInformationSender systemInformationSender)
        {
            if (systemInformationSender == null)
            {
                throw new ArgumentNullException("systemInformationSender");
            }

            this.systemInformationSender = systemInformationSender;
        }

        public void Start(IMessageQueue<SystemInformation> workQueue, IMessageQueue<SystemInformation> errorQueue)
        {
            while (true)
            {
                Thread.Sleep(WorkIntervalInMilliseconds);

                // check if service has been stopped
                Monitor.Enter(this.lockObject);
                if (this.stop && workQueue.IsEmpty())
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                Monitor.Exit(this.lockObject);

                // dequeue message
                var queueEntry = workQueue.Dequeue();
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
                        workQueue.Enqueue(queueEntry);
                    }
                    else
                    {
                        errorQueue.Enqueue(queueEntry);
                    }
                }
                catch (FatalSystemInformationSenderException fatalException)
                {
                    // persist queue and exit
                    Monitor.Enter(this.lockObject);
                    var unfinishedQueueItems = workQueue.PurgeAllItems();
                    errorQueue.Enqueue(unfinishedQueueItems);
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