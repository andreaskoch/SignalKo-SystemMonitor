namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IQueueItem<T>
    {
        T Item { get; }

        int EnqueuCount { get; set; }
    }
}