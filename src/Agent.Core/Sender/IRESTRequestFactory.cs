using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public interface IRESTRequestFactory
    {
        IRestRequest CreatePutRequest(string resourcePath, string hostname = null);

        IRestRequest CreateGetRequest(string resourcePath, string hostname = null);
    }
}