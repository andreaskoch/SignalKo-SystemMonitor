namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueue<T>
    {
        void Enqueue(IQueueItem<T> item);

        IQueueItem<T> Dequeue();

        bool IsEmpty();
    }
}