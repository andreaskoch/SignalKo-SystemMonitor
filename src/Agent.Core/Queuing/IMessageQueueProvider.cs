namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueueProvider<T>
    {
        IMessageQueue<T> GetWorkQueue();

        IMessageQueue<T> GetErrorQueue();

        void Restore();

        void Persist();
    }
}