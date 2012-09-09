using System;
using System.Threading;
using System.Threading.Tasks;

using SignalKo.SystemMonitor.Agent.CommandLine.DependencyResolution;
using SignalKo.SystemMonitor.Agent.Core;

using StructureMap;

namespace SignalKo.SystemMonitor.Agent.CommandLine
{
    public class Program
    {
        private readonly ISystemInformationDispatchingService systemInformationDispatchingService;

        public Program(ISystemInformationDispatchingService systemInformationDispatchingService)
        {
            if (systemInformationDispatchingService == null)
            {
                throw new ArgumentNullException("systemInformationDispatchingService");
            }

            this.systemInformationDispatchingService = systemInformationDispatchingService;
        }

        public static int Main(string[] args)
        {
            StructureMapSetup.Setup();
            
            var program = new Program(ObjectFactory.GetInstance<ISystemInformationDispatchingService>());
            return program.Run(args);
        }

        public int Run(string[] commandLineArguments)
        {
            try
            {
                var dispatcher = new Task(this.systemInformationDispatchingService.Start);
                var escapeWatch = new Task(
                    () =>
                        {
                            System.Console.WriteLine("Hit <ESC> to stop.");

                            while (true)
                            {
                                var input = System.Console.ReadKey();
                                if (input.Key == ConsoleKey.Escape)
                                {
                                    this.systemInformationDispatchingService.Stop();
                                    break;
                                }

                                Thread.Sleep(1000);
                            }
                        });

                escapeWatch.Start();
                dispatcher.Start();

                Task.WaitAll(new[] { dispatcher });

                return 0;
            }
            catch (Exception exception)
            {
                return 1;
            }            
        }
    }
}
