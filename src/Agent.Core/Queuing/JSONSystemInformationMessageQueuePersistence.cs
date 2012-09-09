using System;
using System.IO;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class JSONSystemInformationMessageQueuePersistence : IMessageQueuePersistence<SystemInformation>
    {
        private readonly IEncodingProvider encodingProvider;

        private readonly JSONMessageQueuePersistenceConfiguration jsonMessageQueuePersistenceConfiguration;

        public JSONSystemInformationMessageQueuePersistence(IJSONMessageQueuePersistenceConfigurationProvider jsonMessageQueuePersistenceConfigurationProvider, IEncodingProvider encodingProvider)
        {
            if (jsonMessageQueuePersistenceConfigurationProvider == null)
            {
                throw new ArgumentNullException("jsonMessageQueuePersistenceConfigurationProvider");
            }

            if (encodingProvider == null)
            {
                throw new ArgumentNullException("encodingProvider");
            }

            this.jsonMessageQueuePersistenceConfiguration = jsonMessageQueuePersistenceConfigurationProvider.GetConfiguration();
            this.encodingProvider = encodingProvider;
        }

        public IQueueItem<SystemInformation>[] Load()
        {
            string filePath = this.jsonMessageQueuePersistenceConfiguration.FilePath;
            if (!File.Exists(filePath))
            {
                return new IQueueItem<SystemInformation>[] { };
            }

            try
            {
                var json = File.ReadAllText(this.jsonMessageQueuePersistenceConfiguration.FilePath, this.encodingProvider.GetEncoding());
                return JsonConvert.DeserializeObject<SystemInformationQueueItem[]>(json);
            }
            catch (Exception exception)
            {
                return new IQueueItem<SystemInformation>[] { };
            }
        }

        public void Save(IQueueItem<SystemInformation>[] items)
        {
            string filePath = this.jsonMessageQueuePersistenceConfiguration.FilePath;
            var json = JsonConvert.SerializeObject(items);
            File.WriteAllText(filePath, json, this.encodingProvider.GetEncoding());
        }
    }
}