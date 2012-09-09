using System.Collections.Generic;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueue<T>
    {
        void Enqueue(IQueueItem<T> item);

        void Enqueue(IEnumerable<IQueueItem<T>> items);

        IQueueItem<T> Dequeue();

        IQueueItem<T>[] PurgeAllItems();

        int GetSize();
        
        bool IsEmpty();
    }
}