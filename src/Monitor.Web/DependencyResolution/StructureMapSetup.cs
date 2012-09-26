using SignalKo.SystemMonitor.Monitor.Web.Core.Configuration;

using StructureMap;

namespace SignalKo.SystemMonitor.Monitor.Web.DependencyResolution
{
    public static class StructureMapSetup
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x => x.Scan(
                scan =>
                    {
                        scan.TheCallingAssembly();
                        scan.WithDefaultConventions();
                    }));
            return ObjectFactory.Container;
        }
    }
}