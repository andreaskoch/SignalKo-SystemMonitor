namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IQueueItem<T>
    {
        int EnqueuCount { get; set; }

        T Item { get; set; }
    }
}