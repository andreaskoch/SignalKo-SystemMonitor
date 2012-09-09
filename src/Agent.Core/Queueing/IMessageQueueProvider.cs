namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueProvider<T>
    {
        IMessageQueue<T> WorkQueue { get; }

        IMessageQueue<T> ErrorQueue { get; }
    }
}