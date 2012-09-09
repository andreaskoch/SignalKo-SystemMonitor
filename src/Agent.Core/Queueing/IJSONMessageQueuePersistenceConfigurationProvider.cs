namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IJSONMessageQueuePersistenceConfigurationProvider
    {
        JSONMessageQueuePersistenceConfiguration GetConfiguration();
    }
}