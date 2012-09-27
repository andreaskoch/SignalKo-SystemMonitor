using System;

using RestSharp;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Sender
{
    public class RESTBasedSystemInformationSender : ISystemInformationSender
    {
        private readonly IRESTBasedSystemInformationSenderConfigurationProvider systemInformationSenderConfigurationProvider;

        private readonly IRestClient client;

        private readonly IRESTRequestFactory requestFactory;

        private readonly IRESTServiceConfiguration configuration;

        public RESTBasedSystemInformationSender(IRESTBasedSystemInformationSenderConfigurationProvider systemInformationSenderConfigurationProvider, IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory)
        {
            if (systemInformationSenderConfigurationProvider == null)
            {
                throw new ArgumentNullException("systemInformationSenderConfigurationProvider");
            }

            if (restClientFactory == null)
            {
                throw new ArgumentNullException("restClientFactory");
            }

            if (requestFactory == null)
            {
                throw new ArgumentNullException("requestFactory");
            }

            // initialize service configuration
            IRESTServiceConfiguration serviceConfiguration = systemInformationSenderConfigurationProvider.GetConfiguration();
            if (serviceConfiguration == null)
            {
                throw new SystemInformationSenderSetupException("Service configuration is null.");
            }

            if (!serviceConfiguration.IsValid())
            {
                throw new SystemInformationSenderSetupException("Service configuration \"{0}\" is invalid.", serviceConfiguration);
            }

            this.configuration = serviceConfiguration;
            

            // initialize REST client
            var restClient = restClientFactory.GetRESTClient(this.configuration.BaseUrl);
            if (restClient == null)
            {
                throw new SystemInformationSenderSetupException("Could not create a REST client using the supplied configuration ({0}).", this.configuration);
            }

            this.client = restClient;

            // save request factory
            this.systemInformationSenderConfigurationProvider = systemInformationSenderConfigurationProvider;
            this.requestFactory = requestFactory;
        }

        public void Send(SystemInformation systemInformation)
        {
            if (systemInformation == null)
            {
                throw new ArgumentNullException("systemInformation");
            }

            // Assemble requst
            var request = this.requestFactory.CreatePutRequest(this.configuration.ResourcePath);
            if (request == null)
            {
                throw new FatalSystemInformationSenderException(
                    "Could not create a request object for the base Url \"{0}\" and the resource path \"{1}\".",
                    this.configuration.BaseUrl,
                    this.configuration.ResourcePath);
            }

            request.AddBody(systemInformation);

            // Send request
            var response = this.client.Execute<SystemInformation>(request);

            // Evaluate response
            if (response.ErrorException != null)
            {
                throw new SendSystemInformationFailedException(
                    string.Format(
                        "Sending object \"{0}\" via a REST call to \"{1}\" failed. Please try again later.", systemInformation, this.configuration.ResourcePath),
                    response.ErrorException);
            }
        }
    }
}