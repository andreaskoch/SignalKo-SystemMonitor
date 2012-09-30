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
        private readonly IMessageQueueFeeder messageQueueFeeder;

        private readonly IMessageQueueWorker messageQueueWorker;

        private readonly IMessageQueueProvider<SystemInformation> messageQueueProvider;

        private readonly IMessageQueuePersistence<SystemInformation> messageQueuePersistence;

        private readonly IAgentCoordinationService agentCoordinationService;

        public SystemInformationDispatchingService(IMessageQueueFeederFactory messageQueueFeederFactory, IMessageQueueWorkerFactory messageQueueWorkerFactory, IMessageQueueProvider<SystemInformation> messageQueueProvider, IMessageQueuePersistence<SystemInformation> messageQueuePersistence, IAgentCoordinationServiceFactory agentCoordinationServiceFactory)
        {
            if (messageQueueFeederFactory == null)
            {
                throw new ArgumentNullException("messageQueueFeederFactory");
            }

            if (messageQueueWorkerFactory == null)
            {
                throw new ArgumentNullException("messageQueueWorkerFactory");
            }

            if (messageQueueProvider == null)
            {
                throw new ArgumentNullException("messageQueueProvider");
            }

            if (messageQueuePersistence == null)
            {
                throw new ArgumentNullException("messageQueuePersistence");
            }

            this.messageQueueFeeder = messageQueueFeederFactory.GetMessageQueueFeeder();
            this.messageQueueWorker = messageQueueWorkerFactory.GetMessageQueueWorker();
            this.messageQueueProvider = messageQueueProvider;
            this.messageQueuePersistence = messageQueuePersistence;

            this.agentCoordinationService = agentCoordinationServiceFactory.GetAgentCoordinationService(
                () => this.messageQueueWorker.Pause(), () => this.messageQueueWorker.Resume());
        }

        public void Start()
        {
            this.RestorePreviousQueue();

            Action agentCoordination = () => this.agentCoordinationService.Start();
            Action messageFeederAction = () => this.messageQueueFeeder.Start();
            Action messageWorkerAction = () => this.messageQueueWorker.Start();
            Parallel.Invoke(messageFeederAction, messageWorkerAction, agentCoordination);

            this.PersistErrorQueue();
        }

        public void Stop()
        {
            this.agentCoordinationService.Stop();
            this.messageQueueFeeder.Stop();
            this.messageQueueWorker.Stop();
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