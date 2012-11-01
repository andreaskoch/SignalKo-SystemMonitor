using System;
using System.IO;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;
using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess
{
	public class JsonAgentConfigurationDataAccessor : IAgentConfigurationDataAccessor
	{
		private const string ConfigurationFilename = "AgentConfiguration.json";

		private readonly IEncodingProvider encodingProvider;

		private readonly string configurationFilePath;

		public JsonAgentConfigurationDataAccessor(IFileSystemDataStoreConfigurationProvider fileSystemDataStoreConfigurationProvider, IEncodingProvider encodingProvider)
		{
			this.configurationFilePath = this.GetConfigurationFilePath(fileSystemDataStoreConfigurationProvider.GetConfiguration());
			this.encodingProvider = encodingProvider;
		}

		public AgentConfiguration Load()
		{
			if (!File.Exists(this.configurationFilePath))
			{
				return null;
			}

			try
			{
				string json = File.ReadAllText(this.configurationFilePath, this.encodingProvider.GetEncoding());
				return JsonConvert.DeserializeObject<AgentConfiguration>(json);
			}
			catch (JsonSerializationException)
			{
				File.Delete(this.configurationFilePath);
				return null;
			}
		}

		public void Store(AgentConfiguration agentConfiguration)
		{
			string json = JsonConvert.SerializeObject(agentConfiguration);
			File.WriteAllText(this.configurationFilePath, json, this.encodingProvider.GetEncoding());
		}

		private string GetConfigurationFilePath(FileSystemDataStoreConfiguration fileSystemDataStoreConfiguration)
		{
			if (fileSystemDataStoreConfiguration == null)
			{
				throw new ArgumentNullException("fileSystemDataStoreConfiguration");
			}

			if (string.IsNullOrWhiteSpace(fileSystemDataStoreConfiguration.ConfigurationFolder))
			{
				throw new ArgumentException("The configuration folder cannot be null or empty.", "fileSystemDataStoreConfiguration");
			}

			if (Directory.Exists(fileSystemDataStoreConfiguration.ConfigurationFolder) == false)
			{
				throw new DirectoryNotFoundException(string.Format("The configuration folder \"{0}\" does not exist.", fileSystemDataStoreConfiguration.ConfigurationFolder));
			}

			return Path.Combine(fileSystemDataStoreConfiguration.ConfigurationFolder, ConfigurationFilename);
		}
	}
}