namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class AppConfigJSONMessageQueuePersistenceConfigurationProvider : IJSONMessageQueuePersistenceConfigurationProvider
    {
        public JSONMessageQueuePersistenceConfiguration GetConfiguration()
        {
            return new JSONMessageQueuePersistenceConfiguration();
        }
    }
}