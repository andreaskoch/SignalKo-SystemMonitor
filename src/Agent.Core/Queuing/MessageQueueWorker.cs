using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class MessageQueueWorker : IMessageQueueWorker
    {
        public const int SendIntervalInMilliseconds = 1000;

        private readonly object lockObject = new object();

        private readonly IMessageQueue messageQueue;

        private readonly ISystemInformationSender systemInformationSender;

        private bool stop;

        public MessageQueueWorker(IMessageQueue messageQueue, ISystemInformationSender systemInformationSender)
        {
            if (messageQueue == null)
            {
                throw new ArgumentNullException("messageQueue");
            }

            if (systemInformationSender == null)
            {
                throw new ArgumentNullException("systemInformationSender");
            }

            this.messageQueue = messageQueue;
            this.systemInformationSender = systemInformationSender;
        }

        public void Start()
        {
            while (true)
            {
                Thread.Sleep(SendIntervalInMilliseconds);

                // check if service has been stopped
                Monitor.Enter(this.lockObject);
                if (this.stop && this.messageQueue.)
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                Monitor.Exit(this.lockObject);

                // dequeue message
                SystemInformation systemInformation = this.messageQueue.Dequeue();
                if (systemInformation == null)
                {
                    continue;
                }

                // send messages
                try
                {
                    this.systemInformationSender.Send(systemInformation);
                }
                catch (SendSystemInformationFailedException sendFailedException)
                {
                    // retry later
                    this.messageQueue.Enqueue(systemInformation);
                }
                catch (FatalSystemInformationSenderException fatalException)
                {
                    // abort
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