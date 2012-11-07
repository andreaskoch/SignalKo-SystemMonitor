namespace SignalKo.SystemMonitor.Agent.Core.Collectors
{
	public interface IUrlComponentExtractor
	{
		string GetHostAddressFromUrl(string url);

		string GetResourcePathFromUrl(string url);
	}
}