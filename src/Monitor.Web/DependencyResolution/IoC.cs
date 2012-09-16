using StructureMap;

namespace SignalKo.SystemMonitor.Monitor.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.Scan(
                        scan =>
                        {
                            scan.TheCallingAssembly();
                            scan.WithDefaultConventions();
                        });
                });
            return ObjectFactory.Container;
        }
    }
}