using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Dispatcher
{
    public interface IMessageQueue
    {
        void Enqueue(SystemInformation systemInformation);

        SystemInformation Dequeue();
    }
}