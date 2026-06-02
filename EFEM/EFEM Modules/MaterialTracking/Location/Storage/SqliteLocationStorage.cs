using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using EFEM.Database;

namespace EFEM.MaterialTracking.LocationStorage
{
    public sealed class SqliteLocationStorage : ILocationStorage, IDisposable
    {
        private readonly MaterialDbContext _db;
        private volatile bool _disposed;

        public SqliteLocationStorage(MaterialDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            _db = db;
        }

        public async Task AddOrUpdateLocationsAsync(IEnumerable<LocationItem> items)
        {
            ThrowIfDisposed();

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                var command = @"
INSERT INTO Location
(Id, LocationKind, Capacity, Name)
VALUES
($id, $kind, $capacity, $displayName)
ON CONFLICT(Id) DO UPDATE SET
    LocationKind = excluded.LocationKind,
    Capacity     = excluded.Capacity,
    Name         = excluded.Name;
";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = command;

                    var idParam = cmd.Parameters.Add("$id", DbType.String);
                    var kindParam = cmd.Parameters.Add("$kind", DbType.Int32);
                    var capacityParam = cmd.Parameters.Add("$capacity", DbType.Int32);
                    var displayNameParam = cmd.Parameters.Add("$displayName", DbType.String);

                    foreach (var item in items)
                    {
                        if (item == null)
                            continue;

                        idParam.Value = item.Id;
                        kindParam.Value = item.LocationKind;
                        capacityParam.Value = item.Capacity;
                        displayNameParam.Value = item.Name ?? string.Empty;

                        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }).ConfigureAwait(false);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SqliteLocationStorage));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}