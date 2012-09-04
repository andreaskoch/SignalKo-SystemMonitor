namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueueFeeder
    {
        void Start();

        void Stop();
    }
}