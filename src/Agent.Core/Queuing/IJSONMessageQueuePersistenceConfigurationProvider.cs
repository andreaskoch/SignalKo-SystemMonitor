namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public interface IJSONMessageQueuePersistenceConfigurationProvider
    {
        JSONMessageQueuePersistenceConfiguration GetConfiguration();
    }
}