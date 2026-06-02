using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace EFEM.MaterialTracking.LocationStorage
{
    public sealed class JsonLocationStorage : ILocationStorage, IDisposable
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _ioLock;
        private volatile bool _disposed;

        private static readonly JsonSerializerSettings _jsonSettings =
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

        public JsonLocationStorage(string rootPath)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new ArgumentNullException(nameof(rootPath));

            Directory.CreateDirectory(rootPath);

            _filePath = Path.Combine(rootPath, "locations.json");
            _ioLock = new SemaphoreSlim(1, 1);
        }

        public async Task AddOrUpdateLocationsAsync(IEnumerable<LocationItem> items)
        {
            ThrowIfDisposed();

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var incomingRecords = items
                .Where(item => item != null)
                .Where(item => string.IsNullOrWhiteSpace(item.Id) == false)
                .Select(ToRecord)
                .ToArray();

            await _ioLock.WaitAsync().ConfigureAwait(false);

            try
            {
                var currentRecords = await ReadAllCoreAsync().ConfigureAwait(false);

                var byId = currentRecords
                    .Where(item => item != null)
                    .Where(item => string.IsNullOrWhiteSpace(item.Id) == false)
                    .ToDictionary(item => item.Id, StringComparer.OrdinalIgnoreCase);

                foreach (var record in incomingRecords)
                {
                    byId[record.Id] = record;
                }

                var merged = byId.Values
                    .OrderBy(item => item.Id, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                await WriteAllCoreAsync(merged).ConfigureAwait(false);
            }
            finally
            {
                _ioLock.Release();
            }
        }

        private async Task<IReadOnlyList<LocationItem>> ReadAllCoreAsync()
        {
            if (File.Exists(_filePath) == false)
                return Array.Empty<LocationItem>();

            var utf8NoBom = new UTF8Encoding(false);

            using (var fs = new FileStream(
                _filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                4096,
                true))
            using (var sr = new StreamReader(fs, utf8NoBom))
            {
                var json = await sr.ReadToEndAsync().ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json))
                    return Array.Empty<LocationItem>();

                var records = JsonConvert.DeserializeObject<List<LocationItem>>(
                    json,
                    _jsonSettings);

                if (records == null)
                    return Array.Empty<LocationItem>();

                return records;
            }
        }

        private async Task WriteAllCoreAsync(IReadOnlyList<LocationItem> records)
        {
            var utf8NoBom = new UTF8Encoding(false);
            var json = JsonConvert.SerializeObject(records, _jsonSettings);
            var tempPath = _filePath + ".tmp";

            using (var fs = new FileStream(
                tempPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                4096,
                true))
            using (var sw = new StreamWriter(fs, utf8NoBom))
            {
                await sw.WriteAsync(json).ConfigureAwait(false);
                await sw.FlushAsync().ConfigureAwait(false);

                try
                {
                    fs.Flush(true);
                }
                catch
                {
                    fs.Flush();
                }
            }

            if (File.Exists(_filePath))
                File.Delete(_filePath);

            File.Move(tempPath, _filePath);
        }

        private static LocationItem ToRecord(LocationItem item)
        {
            return new LocationItem
            {
                Id = item.Id,
                LocationKind = item.LocationKind,
                Capacity = item.Capacity,
                Name = item.Name ?? string.Empty
            };
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(JsonLocationStorage));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _ioLock.Dispose();
        }
    }
}