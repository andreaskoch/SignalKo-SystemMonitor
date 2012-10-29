using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;

using StructureMap;

namespace SignalKo.SystemMonitor.Monitor.Web.DependencyResolution
{
    public static class StructureMapSetup
    {
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

            //ObjectFactory.Initialize(x => x.Scan(
            //    scan =>
            //        {
            //            scan.TheCallingAssembly();
            //            scan.WithDefaultConventions();
            //        }));

            return ObjectFactory.Container;
        }
    }
}