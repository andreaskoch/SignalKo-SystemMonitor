namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueFeeder
    {
        void Start();

        void Pause();

        void Resume();

        void Stop();

        ServiceStatus GetStatus();
    }
}