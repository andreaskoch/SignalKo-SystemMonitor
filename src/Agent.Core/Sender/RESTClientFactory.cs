using System;

using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public class RESTClientFactory : IRESTClientFactory
    {
        public IRestClient GetRESTClient(string hostaddress)
        {
            if (string.IsNullOrWhiteSpace(hostaddress))
            {
                throw new ArgumentException("The base url of the REST client cannot be null or empty.", "hostaddress");
            }

            return new RestClient(hostaddress);
        }
    }
}