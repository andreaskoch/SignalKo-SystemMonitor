
namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueWorkerFactory
    {
        SystemInformationMessageQueueWorker GetMessageQueueWorker();
    }
}