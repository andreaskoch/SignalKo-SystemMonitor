using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public class JSONRequestFactory : IRESTRequestFactory
    {
        public const DataFormat RequestFormat = DataFormat.Json;

        public const string RequestHeaderFieldHost = "Host";

        public IRestRequest CreatePutRequest(string resourcePath, string hostname = null)
        {
            var request = new RestRequest(resourcePath, Method.PUT) { RequestFormat = RequestFormat };

            if (!string.IsNullOrWhiteSpace(hostname))
            {
                request.AddHeader(RequestHeaderFieldHost, hostname);
            }

            return request;
        }

        public IRestRequest CreateGetRequest(string resourcePath, string hostname = null)
        {
            var request = new RestRequest(resourcePath, Method.GET) { RequestFormat = RequestFormat };

            if (!string.IsNullOrWhiteSpace(hostname))
            {
                request.AddHeader(RequestHeaderFieldHost, hostname);
            }

            return request;
        }
    }
}