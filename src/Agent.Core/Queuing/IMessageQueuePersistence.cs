namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueuePersistence<T>
    {
        IQueueItem<T>[] Load();

        void Save(IQueueItem<T>[] items);
    }
}