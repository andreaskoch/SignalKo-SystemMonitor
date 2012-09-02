using System;

using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public class RESTClientFactory : IRESTClientFactory
    {
        public IRestClient GetRESTClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("The base url of the REST client cannot be null or empty.", "baseUrl");
            }

            return new RestClient(baseUrl);
        }
    }
}