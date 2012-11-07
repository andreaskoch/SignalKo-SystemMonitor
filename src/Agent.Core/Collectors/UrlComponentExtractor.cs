using System;

namespace SignalKo.SystemMonitor.Agent.Core.Collectors
{
	public class UrlComponentExtractor : IUrlComponentExtractor
	{
		public string GetHostAddressFromUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException("url");
			}

			var uri = new Uri(url);
			var port = !uri.IsDefaultPort ? ":" + uri.Port : string.Empty;
			var hostAddress = uri.Scheme + "://" + uri.Host + port;
			return hostAddress;
		}

		public string GetResourcePathFromUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException("url");
			}

			var uri = new Uri(url);
			return uri.PathAndQuery;
		}
	}
}