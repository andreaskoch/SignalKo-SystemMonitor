using System;

using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests
{
    public class TestUtilities
    {
        public static SystemInformation[] GetSystemInformationObjects(int count)
        {
            var items = new SystemInformation[count];
            var machineName = Environment.MachineName;
            DateTime timestamp = DateTime.UtcNow;
            var incremet = new TimeSpan(0, 0, 0, 1, 0);

            for (int i = 0; i < count; i++)
            {
                items[i] = new SystemInformation { MachineName = machineName, Timestamp = timestamp, };
                timestamp += incremet;
            }

            return items;
        }

        public static int GetRandNumber(int min, int max)
        {
            return new Random(DateTime.UtcNow.GetHashCode()).Next(min, max);
        }
    }
}