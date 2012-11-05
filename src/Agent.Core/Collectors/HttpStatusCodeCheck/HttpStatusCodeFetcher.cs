using System;

using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors.HttpStatusCodeCheck
{
	public class HttpStatusCodeFetcher : IHttpStatusCodeFetcher
	{
		private readonly IRESTClientFactory restClientFactory;

		private readonly IRESTRequestFactory requestFactory;

		private readonly IUrlComponentExtractor urlComponentExtractor;

		public HttpStatusCodeFetcher(IRESTClientFactory restClientFactory, IRESTRequestFactory requestFactory, IUrlComponentExtractor urlComponentExtractor)
		{
			if (restClientFactory == null)
			{
				throw new ArgumentNullException("restClientFactory");
			}

			if (requestFactory == null)
			{
				throw new ArgumentNullException("requestFactory");
			}

			if (urlComponentExtractor == null)
			{
				throw new ArgumentNullException("urlComponentExtractor");
			}

			this.restClientFactory = restClientFactory;
			this.requestFactory = requestFactory;
			this.urlComponentExtractor = urlComponentExtractor;
		}

		public HttpStatusCodeCheckResult GetHttpStatusCode(HttpStatusCodeCheckDefinition statusCodeCheckSettings)
		{
			if (statusCodeCheckSettings == null)
			{
				throw new ArgumentNullException("statusCodeCheckSettings");
			}

			string hostAddress = this.urlComponentExtractor.GetHostAddressFromUrl(statusCodeCheckSettings.CheckUrl);
			string hostname = statusCodeCheckSettings.Hostheader;
			string resourcePath = this.urlComponentExtractor.GetResourcePathFromUrl(statusCodeCheckSettings.CheckUrl);

			var restClient = this.restClientFactory.GetRESTClient(hostAddress);

			var request = this.requestFactory.CreateGetRequest(resourcePath, hostname);
			request.Timeout = statusCodeCheckSettings.CheckIntervalInSeconds * 1000;

			var response = restClient.Execute(request);
			return new HttpStatusCodeCheckResult
				{
					ExpectedStatusCode = statusCodeCheckSettings.ExpectedStatusCode,
					ActualStatusCode = (int)response.StatusCode
				};
		}
	}
}