using System;
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

        private IList<SystemInformation> archive;

        public FilesystemSystemInformationArchiveAccessor(IEncodingProvider encodingProvider)
        {
            this.encodingProvider = encodingProvider;
            this.LoadFromDisk();
        }

        public void Store(SystemInformation systemInformation)
        {
            this.archive.Add(systemInformation);
        }

        public IEnumerable<SystemInformation> SearchFor(Func<SystemInformation, bool> predicate)
        {
            return this.archive.Where(predicate);
        }

        public void Dispose()
        {
            this.StoreToDisk();
        }

        private void LoadFromDisk()
        {
            if (!File.Exists(ArchiveFilename))
            {
                this.archive = new List<SystemInformation>();
                return;
            }

            var json = File.ReadAllText(ArchiveFilename, this.encodingProvider.GetEncoding());
            this.archive = JsonConvert.DeserializeObject<SystemInformation[]>(json).ToList();
        }

        private void StoreToDisk()
        {
            var json = JsonConvert.SerializeObject(this.archive.ToArray());
            File.WriteAllText(ArchiveFilename, json, this.encodingProvider.GetEncoding());
        }
    }
}