using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueueProvider<T>
    {
        IMessageQueue<SystemInformation> GetWorkQueue();

        IMessageQueue<SystemInformation> GetErrorQueue();

        void Persist();
    }
}