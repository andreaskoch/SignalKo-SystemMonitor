using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public interface IRESTRequestFactory
    {
        IRestRequest CreatePutRequest(string resourcePath);
    }
}