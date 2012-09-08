using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Queuing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Agent.Core.Sender.Configuration;
using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

using StructureMap;

namespace SignalKo.SystemMonitor.Agent.Console.DependencyResolution
{
    public static class StructureMapSetup
    {
        public static void Setup()
        {
            ObjectFactory.Configure(
                config =>
                    {
                        /* common */
                        config.For<IMemoryUnitConverter>().Use<MemoryUnitConverter>();
                        config.For<ITimeProvider>().Use<UTCTimeProvider>();

                        /* collector */
                        config.For<ILogicalDiscInstanceNameProvider>().Use<LogicalDiscInstanceNameProvider>();
                        config.For<IMachineNameProvider>().Use<EnvironmentMachineNameProvider>();
                        config.For<IProcessorStatusProvider>().Use<ProcessorStatusProvider>();

                        config.For<ISystemStorageStatusProvider>().Use<SystemStorageStatusProvider>();
                        config.For<ISystemMemoryStatusProvider>().Use<SystemMemoryStatusProvider>();
                        config.For<ISystemInformationProvider>().Use<SystemInformationProvider>();

                        /* queuing */
                        config.For<IMessageQueuePersistence<SystemInformation>>().Use<JSONSystemInformationMessageQueuePersistence>();
                        config.For<IMessageQueuePersistence<SystemInformation>>().Use<JSONSystemInformationMessageQueuePersistence>();
                        config.For<IMessageQueue<SystemInformation>>().Use<SystemInformationMessageQueue>();
                        config.For<IMessageQueueFeeder>().Use<SystemInformationMessageQueueFeeder>();
                        config.For<IMessageQueueWorker>().Use<SystemInformationMessageQueueWorker>();

                        config.For<IMessageQueueProvider<SystemInformation>>().Singleton().Use<SystemInformationMessageQueueProvider>();

                        /* sender configuration */
                        config.For<IRESTServiceConfigurationProvider>().Use<AppSettingsRESTServiceConfigurationProvider>();

                        /* sender */
                        config.For<IRESTClientFactory>().Use<RESTClientFactory>();
                        config.For<IRESTRequestFactory>().Use<JSONRequestFactory>();
                        config.For<ISystemInformationSender>().Use<RESTBasedSystemInformationSender>();

                        config.For<ISystemInformationDispatchingService>().Use<SystemInformationDispatchingService>();
                    });
        }
    }
}
