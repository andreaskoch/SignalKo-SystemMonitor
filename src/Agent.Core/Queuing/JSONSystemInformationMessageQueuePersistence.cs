using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class JSONSystemInformationMessageQueuePersistence : IMessageQueuePersistence<SystemInformation>
    {
        private readonly JSONMessageQueuePersistenceConfiguration jsonMessageQueuePersistenceConfiguration;

        public JSONSystemInformationMessageQueuePersistence(IJSONMessageQueuePersistenceConfigurationProvider jsonMessageQueuePersistenceConfigurationProvider)
        {
            if (jsonMessageQueuePersistenceConfigurationProvider == null)
            {
                throw new ArgumentNullException("jsonMessageQueuePersistenceConfigurationProvider");
            }

            this.jsonMessageQueuePersistenceConfiguration = jsonMessageQueuePersistenceConfigurationProvider.GetConfiguration();
        }

        public IQueueItem<SystemInformation>[] Load()
        {
            throw new NotImplementedException();
        }

        public void Save(IQueueItem<SystemInformation>[] items)
        {
            throw new NotImplementedException();
        }
    }
}