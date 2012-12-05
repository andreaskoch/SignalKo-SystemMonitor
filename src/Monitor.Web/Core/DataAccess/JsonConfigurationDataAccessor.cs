using System;
using System.IO;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Common.Services;
using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess
{
	public class JsonConfigurationDataAccessor<T> : IConfigurationDataAccessor<T> where T : class
	{
		private readonly IEncodingProvider encodingProvider;

		private readonly string configurationFilePath;

		public JsonConfigurationDataAccessor(IFileSystemDataStoreConfigurationProvider fileSystemDataStoreConfigurationProvider, IEncodingProvider encodingProvider)
		{
			this.configurationFilePath = this.GetConfigurationFilePath(fileSystemDataStoreConfigurationProvider.GetConfiguration());
			this.encodingProvider = encodingProvider;
		}

		public T Load()
		{
			if (!File.Exists(this.configurationFilePath))
			{
				return null;
			}

			string json = File.ReadAllText(this.configurationFilePath, this.encodingProvider.GetEncoding());
			return JsonConvert.DeserializeObject<T>(json);
		}

		public void Store(T configuration)
		{
			string json = JsonConvert.SerializeObject(configuration);
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

			string configurationFilename = typeof(T).Name + ".json";
			return Path.Combine(fileSystemDataStoreConfiguration.ConfigurationFolder, configurationFilename);
		}
	}
}