namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueuePersistence<T>
    {
        IQueueItem<T>[] Load();

        void Save(IQueueItem<T>[] queueItems);
    }
}