using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Common.Services;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess
{
    public class FilesystemSystemInformationArchiveAccessor : ISystemInformationArchiveAccessor
    {
        private const string ArchiveFilename = "C:\\tmp\\archive.json";

        private readonly IEncodingProvider encodingProvider;

        private readonly object lockObject = new object();

        private static ConcurrentBag<SystemInformation> archive;

        public FilesystemSystemInformationArchiveAccessor(IEncodingProvider encodingProvider)
        {
            this.encodingProvider = encodingProvider;
            this.LoadFromDisk();
        }

        public void Store(SystemInformation systemInformation)
        {
            System.Threading.Monitor.Enter(this.lockObject);
            archive.Add(systemInformation);
            System.Threading.Monitor.Exit(this.lockObject);
        }

        public IEnumerable<SystemInformation> SearchFor(Func<SystemInformation, bool> predicate)
        {
            System.Threading.Monitor.Enter(this.lockObject);
            var result = archive.Where(predicate).ToList();
            System.Threading.Monitor.Exit(this.lockObject);

            return result;
        }

        public IEnumerable<TResult> Select<TResult>(Func<SystemInformation, TResult> selector)
        {
            System.Threading.Monitor.Enter(this.lockObject);
            var result = archive.Select(selector).ToList();
            System.Threading.Monitor.Exit(this.lockObject);

            return result;
        }

        public void Dispose()
        {
            this.StoreToDisk();
        }

        private void LoadFromDisk()
        {
            try
            {
                System.Threading.Monitor.Enter(this.lockObject);

                if (!File.Exists(ArchiveFilename))
                {
                    archive = new ConcurrentBag<SystemInformation>();
                    return;
                }

                var json = File.ReadAllText(ArchiveFilename, this.encodingProvider.GetEncoding());
                archive = new ConcurrentBag<SystemInformation>(JsonConvert.DeserializeObject<SystemInformation[]>(json).ToList());
            }
            finally
            {
                System.Threading.Monitor.Exit(this.lockObject);   
            }
        }

        private void StoreToDisk()
        {
            try
            {
                System.Threading.Monitor.Enter(this.lockObject);
                var json = JsonConvert.SerializeObject(archive.ToArray());
                File.WriteAllText(ArchiveFilename, json, this.encodingProvider.GetEncoding());
            }
            finally
            {
                archive = new ConcurrentBag<SystemInformation>();
                System.Threading.Monitor.Exit(this.lockObject);
            }
        }
    }
}