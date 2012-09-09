namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IQueueItem<T>
    {
        T Item { get; }

        int EnqueuCount { get; set; }
    }
}