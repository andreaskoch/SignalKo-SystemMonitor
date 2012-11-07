using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

using StructureMap;

namespace SignalKo.SystemMonitor.Monitor.Web.DependencyResolution
{
    /// <summary>
    /// This class is for initiating the StructureMap Container
    /// </summary>
    public static class StructureMapSetup
    {
        /// <summary>
        /// Initializes a instance of StructureMap.
        /// </summary>
        /// <returns>The initiated StructureMap Container</returns>
        public static IContainer Initialize()
        {
            ObjectFactory.Configure(
                config =>
                    {
                        config.For<IFileSystemDataStoreConfigurationProvider>().Use<AppConfigFileSystemDataStoreConfigurationProvider>();
                        config.For<IServerConfigurationRepository>().Use<JsonFileConfigurationRepository>();
                        config.For<ISystemStatusOrchestrator>().Use<SystemStatusOrchestrator>();

                        config.For<IProcessorStatusOrchestrator>().Use<ProcessorStatusOrchestrator>();
                        config.For<IMemoryStatusOrchestrator>().Use<MemoryStatusOrchestrator>();
                        config.For<IStorageStatusOrchestrator>().Use<StorageStatusOrchestrator>();
                    });
            return ObjectFactory.Container;
        }
    }
}