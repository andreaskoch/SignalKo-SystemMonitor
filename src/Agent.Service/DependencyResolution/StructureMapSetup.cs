using SignalKo.SystemMonitor.Agent.Core;
using SignalKo.SystemMonitor.Agent.Core.Collectors.SystemPerformance;
using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Coordination;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Agent.Core.Sender;
using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

using StructureMap;

namespace SignalKo.SystemMonitor.Agent.Service.DependencyResolution
{
    public static class StructureMapSetup
    {
        public static void Setup()
        {
            ObjectFactory.Configure(
                config =>
                    {
                        /* common */
                        config.For<IEncodingProvider>().Use<DefaultEncodingProvider>();
                        config.For<IMemoryUnitConverter>().Use<MemoryUnitConverter>();
                        config.For<ITimeProvider>().Use<UTCTimeProvider>();

                        /* collector */
                        config.For<ILogicalDiscInstanceNameProvider>().Use<LogicalDiscInstanceNameProvider>();
                        config.For<IMachineNameProvider>().Use<EnvironmentMachineNameProvider>();
                        config.For<IProcessorStatusProvider>().Use<ProcessorStatusProvider>();

                        config.For<ISystemStorageStatusProvider>().Use<SystemStorageStatusProvider>();
                        config.For<ISystemMemoryStatusProvider>().Use<SystemMemoryStatusProvider>();
                        config.For<ISystemInformationProvider>().Use<SystemInformationProvider>();

                        /* coordination */
                        config.For<IAgentCoordinationServiceFactory>().Use<AgentCoordinationServiceFactory>();

                        /* queuing */
                        var workQueue = new SystemInformationMessageQueue();
                        var errorQueue = new SystemInformationMessageQueue();
                        var messageQueueProvider = new SystemInformationMessageQueueProvider(workQueue, errorQueue);
                        config.For<IMessageQueueProvider<SystemInformation>>().Singleton().Use(() => messageQueueProvider);

                        config.For<IMessageQueuePersistence<SystemInformation>>().Use<JSONSystemInformationMessageQueuePersistence>();
                        config.For<IJSONMessageQueuePersistenceConfigurationProvider>().Use<AppConfigJSONMessageQueuePersistenceConfigurationProvider>();

                        config.For<IMessageQueueFeederFactory>().Use<SystemInformationMessageQueueFeederFactory>();
                        config.For<IMessageQueueWorkerFactory>().Use<SystemInformationMessageQueueWorkerFactory>();

                        /* sender configuration */
                        config.For<IAgentConfigurationAccessor>().Use<AgentConfigurationAccessor>();
                        config.For<IAgentConfigurationProvider>().Use<AgentConfigurationProvider>();
                        config.For<IAgentConfigurationServiceUrlProvider>().Use<AppConfigAgentConfigurationServiceUrlProvider>();
                        config.For<IRESTBasedSystemInformationSenderConfigurationProvider>().Use<RESTBasedSystemInformationSenderConfigurationProvider>();

                        /* sender */
                        config.For<IRESTClientFactory>().Use<RESTClientFactory>();
                        config.For<IRESTRequestFactory>().Use<JSONRequestFactory>();
                        config.For<ISystemInformationSender>().Use<RESTBasedSystemInformationSender>();

                        config.For<ISystemInformationDispatchingService>().Use<SystemInformationDispatchingService>();
                    });
        }
    }
}
