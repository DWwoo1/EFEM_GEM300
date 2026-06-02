using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using System.Data;
//using System.Data.SQLite;
//using System.Data.Common;

using EFEM.Database;
using EFEM.Database.DatabaseOnly;

namespace EFEM.MaterialTracking.ProcessingHistory.Storage
{
    public sealed class SqliteSubstrateProcessingHistoryStorage : ISubstrateProcessingHistoryStorage, IDisposable
    {
        private readonly MaterialDbContext _db;
        //private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();
        private volatile bool _disposed;

        public SqliteSubstrateProcessingHistoryStorage(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(SqliteSubstrateProcessingHistoryStorage));
        }
        private async Task RecordAsync(SubstrateProcessingHistoryItem item)
        {
            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO SubstrateProcessingHistory
(SubstrateKey, EventTime,
 OldState, NewState,
 ControlJobId, ProcessJobId,
 LocationId, Description)
VALUES
($key, $eventTime,
 $old, $new,
 $cj, $pj,
 $locId, $desc);
";

                    cmd.Parameters.Add("$key", DbType.String).Value = item.SubstrateKey;
                    cmd.Parameters.Add("$eventTime", DbType.String).Value = DbUtil.ToIsoString(item.EventTime);
                    cmd.Parameters.Add("$old", DbType.String).Value = item.OldState;
                    cmd.Parameters.Add("$new", DbType.String).Value = item.NewState;
                    cmd.Parameters.Add("$cj", DbType.String).Value = item.ControlJobId;
                    cmd.Parameters.Add("$pj", DbType.String).Value = item.ProcessJobId;
                    cmd.Parameters.Add("$locId", DbType.String).Value = item.LocationId;
                    cmd.Parameters.Add("$desc", DbType.String).Value = item.Description;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }
        public void Record(SubstrateProcessingHistoryItem item)
        {
            ThrowIfDisposed();

            if (item == null) throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.SubstrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(item));

            RecordAsync(item);

            //            var command = @"
            //INSERT INTO SubstrateProcessingHistory
            //(SubstrateKey, EventTime,
            // OldState, NewState,
            // ControlJobId, ProcessJobId,
            // LocationId, Description)
            //VALUES
            //($key, $eventTime,
            // $old, $new,
            // $cj, $pj,
            // $locId, $desc);
            //";
            //            List<string> keys = new List<string>();
            //            List<string> values = new List<string>();
            //            keys.Add("$key"); values.Add(item.SubstrateKey);
            //            keys.Add("$eventTime"); values.Add(DbUtil.ToIsoString(item.EventTime));
            //            keys.Add("$old"); values.Add(item.OldState);
            //            keys.Add("$new"); values.Add(item.NewState);
            //            keys.Add("$cj"); values.Add(item.ControlJobId);
            //            keys.Add("$pj"); values.Add(item.ProcessJobId);
            //            keys.Add("$locId"); values.Add(item.LocationId);
            //            keys.Add("$desc"); values.Add(item.Description);


            //            _db.ExecuteRecordingHistory(command, keys, values);

            ////using (_keyedLocker.Acquire(item.SubstrateKey))
            //using (var conn = _db.OpenConnection())
            //using (var cmd = conn.CreateCommand())
            //{
            //    cmd.Parameters.AddWithValue("$key", item.SubstrateKey);
            //    cmd.Parameters.AddWithValue("$eventTime", DbUtil.ToIsoString(item.EventTime));
            //    cmd.Parameters.AddWithValue("$old", item.OldState ?? string.Empty);
            //    cmd.Parameters.AddWithValue("$new", item.NewState ?? string.Empty);
            //    cmd.Parameters.AddWithValue("$cj", (object)item.ControlJobId ?? DBNull.Value);
            //    cmd.Parameters.AddWithValue("$pj", (object)item.ProcessJobId ?? DBNull.Value);
            //    cmd.Parameters.AddWithValue("$locId", (object)item.LocationId ?? DBNull.Value);
            //    cmd.Parameters.AddWithValue("$desc", (object)item.Description ?? DBNull.Value);

            //    cmd.ExecuteNonQuery();
            //}
        }

        public void OnSubstrateCreated(string substrateKey)
        {
        }

        public void OnSubstrateArchived(string substrateKey, string destinationPath)
        {
        }

        public void OnSubstrateDeleted(string substrateKey)
        {
            // Substrate FK + ON DELETE CASCADE
            return;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            //_keyedLocker?.Dispose();
        }
    }

}
