using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public class JSONRequestFactory : IRESTRequestFactory
    {
        public const DataFormat RequestFormat = DataFormat.Json;

        public IRestRequest CreatePutRequest(string resourcePath)
        {
            return new RestRequest(resourcePath, Method.PUT) { RequestFormat = RequestFormat };
        }
    }
}