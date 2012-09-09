using System;
using System.IO;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Agent.Core.Queueing
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
                return null;
            }

            try
            {
                var json = File.ReadAllText(this.jsonMessageQueuePersistenceConfiguration.FilePath, this.encodingProvider.GetEncoding());
                return JsonConvert.DeserializeObject<SystemInformationQueueItem[]>(json);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public void Save(IQueueItem<SystemInformation>[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            string filePath = this.jsonMessageQueuePersistenceConfiguration.FilePath;
            try
            {
                var json = JsonConvert.SerializeObject(items);
                File.WriteAllText(filePath, json, this.encodingProvider.GetEncoding());
            }
            catch (Exception serializationException)
            {
                throw new MessageQueuePersistenceException("Could not persist the supplied item to file \"{0}\".", serializationException);
            }
        }
    }
}