using System;
using System.Configuration;
using System.Diagnostics;
using System.Management;
using System.Threading;

using RestSharp;

namespace SignalKo.SystemMonitor.Agent.Console
{
    using SignalKo.SystemMonitor.Common.Model;

    public class Program
    {
        private static PerformanceCounter processorCounter;

        private static PerformanceCounter memoryCounter;

        static void Main(string[] args)
        {
            processorCounter = new PerformanceCounter
                {
                    CategoryName = "Processor",
                    CounterName = "% Processor Time",
                    InstanceName = "_Total"
                };

            memoryCounter = new PerformanceCounter("Memory", "Available KBytes");

            string baseUrl = ConfigurationManager.AppSettings["ServiceBaseUrl"];
            var restClient = new RestClient(baseUrl);

            do
            {
                var hardwareInfo = GetHardwareInfo();
                var request = new RestRequest("api/HardwareInfo/", Method.PUT);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(hardwareInfo);
                restClient.ExecuteAsync<SystemInformation>(request, response => { });

                Thread.Sleep(100);
            }
            while (true);
        }

        static SystemInformation GetHardwareInfo()
        {
            var processorTime = (double)processorCounter.NextValue();
            var memUsage = (ulong)memoryCounter.NextValue();
            ulong totalMemory = 0;

            // Get total memory from WMI
            var memQuery = new ObjectQuery("SELECT * FROM CIM_OperatingSystem");
            var searcher = new ManagementObjectSearcher(memQuery);
            foreach (ManagementObject item in searcher.Get())
            {
                totalMemory += (ulong)item["TotalVisibleMemorySize"];
            }

            return new SystemInformation
                {
                    MachineName = Environment.MachineName, 
                    //MemUsage = memUsage, 
                    //Processor = processorTime,
                    //TotalMemory = totalMemory
                };
        }
    }
}
