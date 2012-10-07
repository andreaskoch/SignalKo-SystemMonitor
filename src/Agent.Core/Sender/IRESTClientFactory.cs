using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public interface IRESTClientFactory
    {
        IRestClient GetRESTClient(string hostaddress);
    }
}