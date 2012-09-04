namespace SignalKo.SystemMonitor.Agent.Core.Dispatcher
{
    public interface IMessageQueueWorker
    {
        void Start();

        void Stop();
    }
}