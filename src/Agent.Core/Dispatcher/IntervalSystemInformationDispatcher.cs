using System;
using System.Threading;

using SignalKo.SystemMonitor.Agent.Core.Collector;
using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Sender;

namespace SignalKo.SystemMonitor.Agent.Core.Dispatcher
{
    public class IntervalSystemInformationDispatcher : ISystemInformationDispatcher
    {
        public const int SendIntervalInMilliseconds = 1000;

        private readonly ISystemInformationProvider systemInformationProvider;

        private readonly ISystemInformationSender systemInformationSender;

        private readonly object lockObject = new object();

        private bool stop;

        public IntervalSystemInformationDispatcher(ISystemInformationProvider systemInformationProvider, ISystemInformationSender systemInformationSender)
        {
            if (systemInformationProvider == null)
            {
                throw new ArgumentNullException("systemInformationProvider");
            }

            if (systemInformationSender == null)
            {
                throw new ArgumentNullException("systemInformationSender");
            }

            this.systemInformationProvider = systemInformationProvider;
            this.systemInformationSender = systemInformationSender;
        }

        public void Start()
        {
            while (true)
            {
                Thread.Sleep(SendIntervalInMilliseconds);

                // check if service has been stopped
                Monitor.Enter(this.lockObject);
                if (this.stop)
                {
                    Monitor.Exit(this.lockObject);
                    break;
                }

                Monitor.Exit(this.lockObject);

                // retrieve data
                var systemInfo = this.systemInformationProvider.GetSystemInfo();
                if (systemInfo == null)
                {
                    // skip this run
                    continue;
                }

                // send messages
                try
                {
                    this.systemInformationSender.Send(systemInfo);
                }
                catch (SendSystemInformationFailedException sendFailedException)
                {
                    // retry later
                }
                catch (FatalSystemInformationSenderException fatalException)
                {
                    // abort
                    break;
                }
            }          
        }

        public void Stop()
        {
            Monitor.Enter(this.lockObject);
            this.stop = true;
            Monitor.Exit(this.lockObject);          
        }
    }
}