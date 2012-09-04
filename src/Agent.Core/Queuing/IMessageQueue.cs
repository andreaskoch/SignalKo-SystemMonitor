using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IMessageQueue
    {
        void Enqueue(SystemInformation systemInformation);

        SystemInformation Dequeue();

        bool IsEmpty();
    }
}