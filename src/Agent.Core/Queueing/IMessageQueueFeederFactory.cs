
namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueFeederFactory
    {
        IMessageQueueFeeder GetMessageQueueFeeder();
    }
}