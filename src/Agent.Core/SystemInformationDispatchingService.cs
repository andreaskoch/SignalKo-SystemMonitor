using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core
{
    public class SystemInformationDispatchingService : ISystemInformationDispatchingService
    {
        private const int DefaultAgentConfigurationCheckIntervalInSeconds = 60;

        private readonly IMessageQueueFeeder<SystemInformation> messageQueueFeeder;

        private readonly IMessageQueueWorker<SystemInformation> messageQueueWorker;

        private readonly IMessageQueueProvider<SystemInformation> messageQueueProvider;

        private readonly IMessageQueuePersistence<SystemInformation> messageQueuePersistence;

        private readonly IAgentConfigurationProvider agentConfigurationProvider;

        private bool run = true;

        public SystemInformationDispatchingService(IMessageQueueFeeder<SystemInformation> messageQueueFeeder, IMessageQueueWorker<SystemInformation> messageQueueWorker, IMessageQueueProvider<SystemInformation> messageQueueProvider, IMessageQueuePersistence<SystemInformation> messageQueuePersistence, IAgentConfigurationProvider agentConfigurationProvider)
        {
            if (messageQueueFeeder == null)
            {
                throw new ArgumentNullException("messageQueueFeeder");
            }

            if (messageQueueWorker == null)
            {
                throw new ArgumentNullException("messageQueueWorker");
            }

            if (messageQueueProvider == null)
            {
                throw new ArgumentNullException("messageQueueProvider");
            }

            if (messageQueuePersistence == null)
            {
                throw new ArgumentNullException("messageQueuePersistence");
            }

            if (agentConfigurationProvider == null)
            {
                throw new ArgumentNullException("agentConfigurationProvider");
            }

            this.messageQueueFeeder = messageQueueFeeder;
            this.messageQueueWorker = messageQueueWorker;
            this.messageQueueProvider = messageQueueProvider;
            this.messageQueuePersistence = messageQueuePersistence;
            this.agentConfigurationProvider = agentConfigurationProvider;
        }

        public void Start()
        {
            this.RestorePreviousQueue();

            int checkIntervalInSeconds = DefaultAgentConfigurationCheckIntervalInSeconds;
            Action agentCoordination = () =>
                {
                    do
                    {
                        Thread.Sleep(checkIntervalInSeconds * 1000);

                        var agentConfiguration = this.agentConfigurationProvider.GetAgentConfiguration();
                        if (agentConfiguration == null)
                        {
                            // stop as long as the configuration is invalid
                            this.Pause();
                            continue;
                        }

                        // update the check interval
                        checkIntervalInSeconds = agentConfiguration.CheckIntervalInSeconds > 0
                                                     ? agentConfiguration.CheckIntervalInSeconds
                                                     : DefaultAgentConfigurationCheckIntervalInSeconds;

                        // check status
                        if (agentConfiguration.AgentsAreEnabled == false)
                        {
                            this.Pause();
                        }
                        else
                        {
                            this.Resume();
                        }
                    }
                    while (this.run);
                };

            Action messageFeederAction = () => this.messageQueueFeeder.Start(this.messageQueueProvider.WorkQueue);
            Action messageWorkerAction = () => this.messageQueueWorker.Start(this.messageQueueProvider.WorkQueue, this.messageQueueProvider.ErrorQueue);
            Parallel.Invoke(messageFeederAction, messageWorkerAction, agentCoordination);

            this.PersistErrorQueue();
        }

        public void Stop()
        {
            this.run = false;
            this.messageQueueFeeder.Stop();
            this.messageQueueWorker.Stop();
        }

        public void Pause()
        {
            this.messageQueueFeeder.Pause();
            this.messageQueueWorker.Pause();
        }

        public void Resume()
        {
            this.messageQueueFeeder.Resume();
            this.messageQueueWorker.Resume();            
        }

        private void RestorePreviousQueue()
        {
            var itemsFromPreviousSession = this.messageQueuePersistence.Load();
            if (itemsFromPreviousSession == null)
            {
                return;
            }

            this.messageQueueProvider.WorkQueue.Enqueue(itemsFromPreviousSession.Select(queueItem => new SystemInformationQueueItem(queueItem.Item)));
        }

        private void PersistErrorQueue()
        {
            var failedRequests = this.messageQueueProvider.ErrorQueue.PurgeAllItems();
            if (failedRequests == null || failedRequests.Length == 0)
            {
                return;
            }

            this.messageQueuePersistence.Save(failedRequests);
        }
    }
}