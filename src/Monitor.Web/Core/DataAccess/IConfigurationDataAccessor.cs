namespace SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess
{
	public interface IConfigurationDataAccessor<T>
	{
		T Load();

		void Store(T configuration);
	}
}