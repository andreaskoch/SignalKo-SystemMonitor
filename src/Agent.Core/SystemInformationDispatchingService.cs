using System;
using System.Linq;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.Core.Coordination;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core
{
    public class SystemInformationDispatchingService : ISystemInformationDispatchingService
    {
        private readonly IMessageQueueFeeder<SystemInformation> messageQueueFeeder;

        private readonly IMessageQueueWorker<SystemInformation> messageQueueWorker;

        private readonly IMessageQueueProvider<SystemInformation> messageQueueProvider;

        private readonly IMessageQueuePersistence<SystemInformation> messageQueuePersistence;

        private readonly IAgentCoordinationService agentCoordinationService;

        public SystemInformationDispatchingService(IMessageQueueFeeder<SystemInformation> messageQueueFeeder, IMessageQueueWorker<SystemInformation> messageQueueWorker, IMessageQueueProvider<SystemInformation> messageQueueProvider, IMessageQueuePersistence<SystemInformation> messageQueuePersistence, IAgentCoordinationServiceFactory agentCoordinationServiceFactory)
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

            this.messageQueueFeeder = messageQueueFeeder;
            this.messageQueueWorker = messageQueueWorker;
            this.messageQueueProvider = messageQueueProvider;
            this.messageQueuePersistence = messageQueuePersistence;
            this.agentCoordinationService = agentCoordinationServiceFactory.GetAgentCoordinationService(this.Pause, this.Resume);
        }

        public void Start()
        {
            this.RestorePreviousQueue();

            Action agentCoordination = () => this.agentCoordinationService.Start();
            Action messageFeederAction = () => this.messageQueueFeeder.Start(this.messageQueueProvider.WorkQueue);
            Action messageWorkerAction = () => this.messageQueueWorker.Start(this.messageQueueProvider.WorkQueue, this.messageQueueProvider.ErrorQueue);
            Parallel.Invoke(messageFeederAction, messageWorkerAction, agentCoordination);

            this.PersistErrorQueue();
        }

        public void Stop()
        {
            this.agentCoordinationService.Stop();
            this.messageQueueFeeder.Stop();
            this.messageQueueWorker.Stop();
        }

        private void Pause()
        {
            this.messageQueueFeeder.Pause();
            this.messageQueueWorker.Pause();
        }

        private void Resume()
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