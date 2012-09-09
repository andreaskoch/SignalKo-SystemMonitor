namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueWorker<T>
    {
        void Start(IMessageQueue<T> workQueue, IMessageQueue<T> errorQueue);

        void Stop();
    }
}