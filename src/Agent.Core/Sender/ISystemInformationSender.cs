using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public interface ISystemInformationSender
    {
        void Send(SystemInformation systemInformation);
    }
}