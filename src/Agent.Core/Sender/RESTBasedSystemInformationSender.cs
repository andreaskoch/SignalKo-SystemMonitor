using System;

using RestSharp;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public class RESTBasedSystemInformationSender : ISystemInformationSender
    {
        private readonly IRESTServiceConfigurationProvider serviceConfigurationProvider;

        private RESTServiceConfiguration configuration;

        private RestClient client;

        public RESTBasedSystemInformationSender(IRESTServiceConfigurationProvider serviceConfigurationProvider)
        {
            if (serviceConfigurationProvider == null)
            {
                throw new ArgumentNullException("serviceConfigurationProvider");
            }

            this.serviceConfigurationProvider = serviceConfigurationProvider;
        }

        protected RESTServiceConfiguration Configuration
        {
            get
            {
                if (this.configuration == null)
                {
                    if ((this.configuration = this.serviceConfigurationProvider.GetConfiguration()) == null)
                    {
                        throw new InvalidConfigurationException("The configuration for the REST-based service information sender cannot be null. Please check your configuration.");
                    }
                }

                return this.configuration;
            }
        }

        protected IRestClient Client
        {
            get
            {
                if (this.client == null)
                {
                    try
                    {
                        this.client = new RestClient(this.Configuration.BaseUrl);
                    }
                    catch (Exception createRestClientException)
                    {
                        throw new UnableToCreateRESTClientException(
                            string.Format("Unable to create a REST client for the supplied base URL \"{0}\"", this.Configuration.BaseUrl),
                            createRestClientException);
                    }

                    if (this.client == null)
                    {
                        throw new UnableToCreateRESTClientException(
                            string.Format("Unable to create a REST client for the supplied base URL \"{0}\"", this.Configuration.BaseUrl));
                    }
                }

                return this.client;
            }
        }

        public void Send(SystemInformation systemInformation)
        {
            try
            {
                string resourcePath = this.Configuration.ResourcePath;
                var request = new RestRequest(resourcePath, Method.PUT) { RequestFormat = DataFormat.Json };
                request.AddBody(systemInformation);
                var response = this.client.Execute<SystemInformation>(request);
                if (response.ErrorException != null)
                {
                    throw new SendSystemInformationFailedException(
                        string.Format("Sending object \"{0}\" via a REST call to \"{1}\" failed. Please try again later.", systemInformation, resourcePath),
                        response.ErrorException);
                }
            }
            catch (InvalidConfigurationException invalidConfigurationException)
            {
                throw new FatalSystemInformationSenderException("Service configuration is invalid. Unable to send any data.", invalidConfigurationException);
            }
            catch (UnableToCreateRESTClientException unableToCreateRESTClientException)
            {
                throw new FatalSystemInformationSenderException("Rest client is invalid. Unable to send any data.", unableToCreateRESTClientException);
            }
        }
    }
}