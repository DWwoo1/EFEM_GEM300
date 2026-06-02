using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using System.Data;
//using System.Data.SQLite;
//using System.Data.Common;

using EFEM.Database;
using EFEM.Database.DatabaseOnly;

namespace EFEM.MaterialTracking.LocationHistory.Storage
{
    public sealed class SqliteSubstrateLocationHistoryStorage : ISubstrateLocationHistoryStorage, IDisposable
    {
        private readonly MaterialDbContext _db;
        //private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();
        private volatile bool _disposed;

        public SqliteSubstrateLocationHistoryStorage(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(SqliteSubstrateLocationHistoryStorage));
        }

        public void RecordChange(SubstrateLocationChangeItem entry)
        {
            ThrowIfDisposed();
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            RecordChangeAsync(entry);
        }
        private async Task RecordChangeAsync(SubstrateLocationChangeItem entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO SubstrateLocationHistory
(SubstrateKey,
 FromLocationName,
 FromLocationKind,
 ToLocationName,
 ToLocationKind,
 ChangeTime,
 Reason)
VALUES
($key,
 $fromLoc,
 $fromLocKind,
 $toLoc,
 $toLocKind,
 $changeTime,
 $reason);
";

                    cmd.Parameters.Add("$key", DbType.String).Value = entry.SubstrateKey;
                    if (string.IsNullOrWhiteSpace(entry.FromLocationName))
                    {
                        cmd.Parameters.Add("$fromLoc", DbType.String).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("$fromLoc", DbType.String).Value = entry.FromLocationName;
                    }
                    cmd.Parameters.Add("$fromLocKind", DbType.Int16).Value = entry.FromLocationKind;

                    if (string.IsNullOrWhiteSpace(entry.ToLocationName))
                    {
                        cmd.Parameters.Add("$toLoc", DbType.String).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("$toLoc", DbType.String).Value = entry.ToLocationName;
                    }
                    cmd.Parameters.Add("$toLocKind", DbType.Int16).Value = entry.ToLocationKind;

                    cmd.Parameters.Add("$changeTime", DbType.String).Value = DbUtil.ToIsoString(entry.ChangeTime);
                    cmd.Parameters.Add("$reason", DbType.String).Value = entry.Reason;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }
        public Task<IReadOnlyList<SubstrateLocationChangeItem>> ReadChangesAsync(string substrateKey)
        {
            return _db.ExecuteReadAsync<IReadOnlyList<SubstrateLocationChangeItem>>(async conn =>
            {
                var list = new List<SubstrateLocationChangeItem>();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT
    SubstrateKey,
    FromLocationName,
    FromLocationKind,
    ToLocationName,
    ToLocationKind,
    ChangeTime,
    Reason
FROM SubstrateLocationHistory
WHERE SubstrateKey = $key
ORDER BY ChangeTime ASC, Id ASC;
";
                    cmd.Parameters.Add("$key", DbType.String).Value = substrateKey;

                    using (var r = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await r.ReadAsync().ConfigureAwait(false))
                        {
                            var key = r.GetString(0);

                            // NULL / "" 혼재 대비: DBNull이면 null, ""이면 null로 정규화 권장
                            string from = r.IsDBNull(1) ? null : r.GetString(1);
                            if (string.IsNullOrWhiteSpace(from)) from = null;

                            int fromKind = r.GetInt16(2);

                            string to = r.IsDBNull(3) ? null : r.GetString(3);
                            if (string.IsNullOrWhiteSpace(to)) to = null;

                            int toKind = r.GetInt16(4);

                            var time = DbUtil.FromIsoString(r.GetString(5));
                            var reason = r.IsDBNull(6) ? string.Empty : r.GetString(6);

                            list.Add(new SubstrateLocationChangeItem(key, from, fromKind, to, toKind, time, reason));
                        }
                    }
                }

                return list;
            });
        }
        public void OnSubstrateCreated(string substrateKey)
        {
        }

        public void OnSubstrateArchived(string substrateKey, string destinationPath)
        {
        }

        public void OnSubstrateDeleted(string substrateKey)
        {
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            //_keyedLocker?.Dispose();
        }
    }

}
