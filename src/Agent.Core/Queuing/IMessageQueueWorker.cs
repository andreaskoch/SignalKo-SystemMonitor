namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueueWorker
    {
        void Start();

        void Stop();
    }
}