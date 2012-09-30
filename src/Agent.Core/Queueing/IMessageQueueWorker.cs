namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueWorker
    {
        void Start();

        void Pause();

        void Resume();

        void Stop();

        ServiceStatus GetStatus();
    }
}