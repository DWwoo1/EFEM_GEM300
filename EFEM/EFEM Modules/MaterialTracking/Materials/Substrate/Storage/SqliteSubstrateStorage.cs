using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;
using System.Data.Common;

using EFEM.Database;
using EFEM.Database.DatabaseOnly;

namespace EFEM.MaterialTracking.SubstrateStorage
{
    public sealed class SqliteSubstrateStorage : ISubstrateStorage, IDisposable
    {
        private readonly MaterialDbContext _db;
        //private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();
        private readonly List<ISubstrateEventObserver> _listeners = new List<ISubstrateEventObserver>();
        private volatile bool _disposed;
        private string[] _extraKeys;

        public SqliteSubstrateStorage(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _extraKeys = _db.SubstrateExtraKeys;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(SqliteSubstrateStorage));
        }

        public void RegisterCallbackListner(ISubstrateEventObserver listner)
        {
            if (listner == null) return;
            _listeners.Add(listner);
        }

        public void InitializeStorage()
        {
            ThrowIfDisposed();
        }

        public bool LoadDataFromStorage(out List<SubstrateItem> dataFromStorage)
        {
            ThrowIfDisposed();
            
            dataFromStorage = LoadDataFromStorageAsync().ConfigureAwait(false)
                                                        .GetAwaiter()
                                                        .GetResult();
            return true;
            //return dataFromStorage.Count > 0;

            //            dataFromStorage = new List<SubstrateItem>();

            //            using (var conn = _db.OpenConnection())
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.CommandText = @"
            //SELECT UniqueKey, Name, LocationId,
            //       SourcePortId, SourceSlot,
            //       SourceCarrierId, CurrentCarrierKey,
            //       DestinationPortId, DestinationSlot,
            //       LotId, RecipeId, ProcessJobId, ControlJobId,
            //       TransportStatus, ProcessingStatus, IdReadingStatus,
            //       DoNotProcessFlag, Usage
            //FROM Substrate;
            //";
            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                    {
            //                        var dto = MapSubstrate(reader);
            //                        dto.Extra = LoadSubstrateExtra(conn, dto.UniqueKey);
            //                        dataFromStorage.Add(dto);
            //                    }
            //                }
            //            }

            //            return dataFromStorage.Count > 0;
        }

        public bool IsExists(string key)
        {
            ThrowIfDisposed();

            return true;

            DbUtil.ValidateKey(key, nameof(key));

            return IsExistsAsync(key).ConfigureAwait(false).GetAwaiter().GetResult();

            //using (var conn = _db.OpenConnection())
            //using (var cmd = conn.CreateCommand())
            //{
            //    cmd.CommandText = "SELECT 1 FROM Substrate WHERE UniqueKey = $key LIMIT 1;";
            //    cmd.Parameters.AddWithValue("$key", key);
            //    var r = cmd.ExecuteScalar();

            //    return r != null;
            //}
        }        
        public async Task<SubstrateItem> GetByKeyAsync(string key)
        {
            ThrowIfDisposed();
            DbUtil.ValidateKey(key, nameof(key));

            // 여기서는 단순히 ExecuteReadAsync에 람다를 넘겨주기만 함
            return await _db.ExecuteReadAsync(async conn =>
            {
                SubstrateItem dto = null;

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT UniqueKey, Name, LocationId,
       SourcePortId, SourceSlot,
       SourceCarrierId, CurrentCarrierKey,
       DestinationPortId, DestinationSlot,
       LotId, RecipeId, ProcessJobId, ControlJobId,
       TransportStatus, ProcessingStatus, IdReadingStatus,
       DoNotProcessFlag, Usage
FROM Substrate
WHERE UniqueKey = $key;
";
                    cmd.Parameters.Add("$key", DbType.String).Value = key;

                    MaterialDbContext.LogCommand(cmd);
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            dto = MapSubstrate(reader);
                        }
                    }
                }

                if (dto != null)
                {
                    // 기존과 동일하게 같은 커넥션을 활용
                    dto.Extra = LoadSubstrateExtra(conn, dto.UniqueKey);
                }

                return dto;

            }).ConfigureAwait(false);

//            SubstrateItem dto = null;
//            using (var conn = _db.OpenConnection())
//            {
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//SELECT UniqueKey, Name, LocationId,
//       SourcePortId, SourceSlot,
//       SourceCarrierId, CurrentCarrierKey,
//       DestinationPortId, DestinationSlot,
//       LotId, RecipeId, ProcessJobId, ControlJobId,
//       TransportStatus, ProcessingStatus, IdReadingStatus,
//       DoNotProcessFlag, Usage
//FROM Substrate
//WHERE UniqueKey = $key;
//";
//                    cmd.Parameters.AddWithValue("$key", key);

//                    using (var reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
//                    {
//                        if (await reader.ReadAsync(ct).ConfigureAwait(false))
//                        {
//                            dto = MapSubstrate(reader);
//                        }
//                    }
//                }

//                if (dto != null)
//                {
//                    dto.Extra = LoadSubstrateExtra(conn, dto.UniqueKey);
//                }
//            }

//            return dto;
        }
        public async Task<IReadOnlyList<SubstrateItem>> ListByLocationAsync(string locationName)
        {
            ThrowIfDisposed();
            if (locationName == null) throw new ArgumentNullException(nameof(locationName));

            return await _db.ExecuteReadAsync(async conn =>
            {
                var list = new List<SubstrateItem>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT UniqueKey, Name, LocationId,
                       SourcePortId, SourceSlot,
                       SourceCarrierId, CurrentCarrierKey,
                       DestinationPortId, DestinationSlot,
                       LotId, RecipeId, ProcessJobId, ControlJobId,
                       TransportStatus, ProcessingStatus, IdReadingStatus,
                       DoNotProcessFlag, Usage
                FROM Substrate
                WHERE LocationId = $loc;
                ";
                    cmd.Parameters.Add("$loc", DbType.String).Value = locationName;

                    MaterialDbContext.LogCommand(cmd);
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            var dto = MapSubstrate(reader);
                            dto.Extra = LoadSubstrateExtra(conn, dto.UniqueKey);
                            list.Add(dto);
                        }
                    }
                }

                return list;
            }).ConfigureAwait(false);

//            var list = new List<SubstrateItem>();

//            using (var conn = _db.OpenConnection())
//            using (var cmd = conn.CreateCommand())
//            {
//                cmd.CommandText = @"
//SELECT UniqueKey, Name, LocationId,
//       SourcePortId, SourceSlot,
//       SourceCarrierId, CurrentCarrierKey,
//       DestinationPortId, DestinationSlot,
//       LotId, RecipeId, ProcessJobId, ControlJobId,
//       TransportStatus, ProcessingStatus, IdReadingStatus,
//       DoNotProcessFlag, Usage
//FROM Substrate
//WHERE LocationId = $loc;
//";
//                cmd.Parameters.AddWithValue("$loc", locationName);

//                using (var reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
//                {
//                    while (await reader.ReadAsync(ct).ConfigureAwait(false))
//                    {
//                        ct.ThrowIfCancellationRequested();
//                        var dto = MapSubstrate(reader);
//                        dto.Extra = LoadSubstrateExtra(conn, dto.UniqueKey);
//                        list.Add(dto);
//                    }
//                }
//            }

//            return list;
        }
        public Task UpsertsAsync(IEnumerable<SubstrateItem> dtos)
        {
            ThrowIfDisposed();
            if (dtos == null) throw new ArgumentNullException(nameof(dtos));

            // 각 DTO를 하나의 work 델리게이트로 구성
            var works = new List<Func<SQLiteConnection, SQLiteTransaction, Task>>();

            foreach (var dto in dtos)
            {
                if (dto == null)
                    throw new ArgumentException("Null element detected in dtos.", nameof(dtos));

                DbUtil.ValidateKey(dto.UniqueKey, nameof(dto.UniqueKey));

                if (dto.Extra == null)
                    dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);

                // 클로저 캡쳐 이슈 방지용
                var item = dto;

                works.Add(async (conn, tx) =>
                {
                    // 1) Substrate 테이블 Upsert
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tx;
                        cmd.CommandText = @"
INSERT INTO Substrate
(UniqueKey, Name, LocationId,
 SourcePortId, SourceSlot,
 SourceCarrierId, CurrentCarrierKey,
 DestinationPortId, DestinationSlot,
 LotId, RecipeId, ProcessJobId, ControlJobId,
 TransportStatus, ProcessingStatus, IdReadingStatus,
 DoNotProcessFlag, Usage)
VALUES
($key, $name, $loc,
 $srcPort, $srcSlot,
 $srcCid, $curCKey,
 $dstPort, $dstSlot,
 $lotId, $recipeId, $pjId, $cjId,
 $ts, $ps, $idRs,
 $dnp, $usage)
ON CONFLICT(UniqueKey) DO UPDATE SET
    Name             = excluded.Name,
    LocationId       = excluded.LocationId,
    SourcePortId     = excluded.SourcePortId,
    SourceSlot       = excluded.SourceSlot,
    SourceCarrierId  = excluded.SourceCarrierId,
    CurrentCarrierKey= excluded.CurrentCarrierKey,
    DestinationPortId= excluded.DestinationPortId,
    DestinationSlot  = excluded.DestinationSlot,
    LotId            = excluded.LotId,
    RecipeId         = excluded.RecipeId,
    ProcessJobId     = excluded.ProcessJobId,
    ControlJobId     = excluded.ControlJobId,
    TransportStatus  = excluded.TransportStatus,
    ProcessingStatus = excluded.ProcessingStatus,
    IdReadingStatus  = excluded.IdReadingStatus,
    DoNotProcessFlag = excluded.DoNotProcessFlag,
    Usage            = excluded.Usage;
";

                        //cmd.Parameters.AddWithValue("$key", item.UniqueKey);
                        //cmd.Parameters.AddWithValue("$name", (object)item.Name ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$loc", (object)item.LocationId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$srcPort", item.SourcePortId);
                        //cmd.Parameters.AddWithValue("$srcSlot", item.SourceSlot);
                        //cmd.Parameters.AddWithValue("$srcCid", (object)item.SourceCarrierId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$curCKey",
                        //    !string.IsNullOrWhiteSpace(item.CurrentCarrierKey)
                        //        ? (object)item.CurrentCarrierKey
                        //        : DBNull.Value);
                        //cmd.Parameters.AddWithValue("$dstPort", item.DestinationPortId);
                        //cmd.Parameters.AddWithValue("$dstSlot", item.DestinationSlot);
                        //cmd.Parameters.AddWithValue("$lotId", (object)item.LotId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$recipeId", (object)item.RecipeId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$pjId", (object)item.ProcessJobId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$cjId", (object)item.ControlJobId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("$ts", item.TransportStatus);
                        //cmd.Parameters.AddWithValue("$ps", item.ProcessingStatus);
                        //cmd.Parameters.AddWithValue("$idRs", item.IdReadingStatus);
                        //cmd.Parameters.AddWithValue("$dnp", DbUtil.BoolToInt(item.DoNotProcessFlag));
                        //cmd.Parameters.AddWithValue("$usage", DbUtil.BoolToInt(item.Usage));

                        cmd.Parameters.Add("$key", DbType.String).Value = dto.UniqueKey;
                        cmd.Parameters.Add("$name", DbType.String).Value = dto.Name;
                        cmd.Parameters.Add("$loc", DbType.String).Value = dto.LocationId;
                        cmd.Parameters.Add("$srcPort", DbType.Int32).Value = dto.SourcePortId;
                        cmd.Parameters.Add("$srcSlot", DbType.Int32).Value = dto.SourceSlot;
                        cmd.Parameters.Add("$srcCid", DbType.String).Value = dto.SourceCarrierId;
                        if (false == string.IsNullOrWhiteSpace(dto.CurrentCarrierKey))
                        {
                            cmd.Parameters.Add("$curCKey", DbType.String).Value = dto.CurrentCarrierKey;
                        }
                        else
                        {
                            cmd.Parameters.Add("$curCKey", DbType.String).Value = DBNull.Value;
                        }
                        cmd.Parameters.Add("$dstPort", DbType.Int32).Value = dto.DestinationPortId;
                        cmd.Parameters.Add("$dstSlot", DbType.Int32).Value = dto.DestinationSlot;
                        cmd.Parameters.Add("$lotId", DbType.String).Value = dto.LotId;
                        cmd.Parameters.Add("$recipeId", DbType.String).Value = dto.RecipeId;
                        cmd.Parameters.Add("$pjId", DbType.String).Value = dto.ProcessJobId;
                        cmd.Parameters.Add("$cjId", DbType.String).Value = dto.ControlJobId;
                        cmd.Parameters.Add("$ts", DbType.String).Value = dto.TransportStatus.ToString();
                        cmd.Parameters.Add("$ps", DbType.String).Value = dto.ProcessingStatus.ToString();
                        cmd.Parameters.Add("$idRs", DbType.String).Value = dto.IdReadingStatus.ToString();
                        cmd.Parameters.Add("$dnp", DbType.Int32).Value = DbUtil.BoolToInt(dto.DoNotProcessFlag);
                        cmd.Parameters.Add("$usage", DbType.Int32).Value = DbUtil.BoolToInt(dto.Usage);

                        MaterialDbContext.LogCommand(cmd);
                        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }

                    // 2) Extra 컬럼 Upsert
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tx;
                        cmd.CommandText = GetCommandForUpsert();

                        cmd.Parameters.Add("@SubstrateKey", DbType.String).Value = dto.UniqueKey;
                        //cmd.Parameters.AddWithValue("@SubstrateKey", item.UniqueKey);

                        // Dictionary 순서와 무관하게 "컬럼 이름"으로만 값을 매핑
                        foreach (var col in _extraKeys)
                        {
                            var pKey = $"@{col}";
                            dto.Extra.TryGetValue(col, out var value);
                            if (value == null || string.IsNullOrEmpty(value))
                            {
                                cmd.Parameters.Add(pKey, DbType.String).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.Add(pKey, DbType.String).Value = value;
                            }
                        }

                        MaterialDbContext.LogCommand(cmd);
                        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                });
            }

            // 한 트랜잭션에 몰아서 실행
            var result = _db.ExecuteWriteAsync(works);

            foreach (var item in dtos)
            {
                foreach (var obs in _listeners)
                {
                    obs.OnSubstrateCreated(item.UniqueKey);
                }
            }

            return result;
        }
        public Task UpsertAsync(SubstrateItem dto)
        {
            ThrowIfDisposed();
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            DbUtil.ValidateKey(dto.UniqueKey, nameof(dto.UniqueKey));

            if (dto.Extra == null)
                dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);

            var result = _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO Substrate
(UniqueKey, Name, LocationId,
 SourcePortId, SourceSlot,
 SourceCarrierId, CurrentCarrierKey,
 DestinationPortId, DestinationSlot,
 LotId, RecipeId, ProcessJobId, ControlJobId,
 TransportStatus, ProcessingStatus, IdReadingStatus,
 DoNotProcessFlag, Usage)
VALUES
($key, $name, $loc,
 $srcPort, $srcSlot,
 $srcCid, $curCKey,
 $dstPort, $dstSlot,
 $lotId, $recipeId, $pjId, $cjId,
 $ts, $ps, $idRs,
 $dnp, $usage)
ON CONFLICT(UniqueKey) DO UPDATE SET
    Name             = excluded.Name,
    LocationId       = excluded.LocationId,
    SourcePortId     = excluded.SourcePortId,
    SourceSlot       = excluded.SourceSlot,
    SourceCarrierId  = excluded.SourceCarrierId,
    CurrentCarrierKey= excluded.CurrentCarrierKey,
    DestinationPortId= excluded.DestinationPortId,
    DestinationSlot  = excluded.DestinationSlot,
    LotId            = excluded.LotId,
    RecipeId         = excluded.RecipeId,
    ProcessJobId     = excluded.ProcessJobId,
    ControlJobId     = excluded.ControlJobId,
    TransportStatus  = excluded.TransportStatus,
    ProcessingStatus = excluded.ProcessingStatus,
    IdReadingStatus  = excluded.IdReadingStatus,
    DoNotProcessFlag = excluded.DoNotProcessFlag,
    Usage            = excluded.Usage;
";

                    //cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
                    //cmd.Parameters.AddWithValue("$name", (object)dto.Name ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$loc", (object)dto.LocationId ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$srcPort", dto.SourcePortId);
                    //cmd.Parameters.AddWithValue("$srcSlot", dto.SourceSlot);
                    //cmd.Parameters.AddWithValue("$srcCid", (object)dto.SourceCarrierId ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$curCKey", !string.IsNullOrWhiteSpace(dto.CurrentCarrierKey) ? (object)dto.CurrentCarrierKey : DBNull.Value);
                    //cmd.Parameters.AddWithValue("$dstPort", dto.DestinationPortId);
                    //cmd.Parameters.AddWithValue("$dstSlot", dto.DestinationSlot);
                    //cmd.Parameters.AddWithValue("$lotId", (object)dto.LotId ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$recipeId", (object)dto.RecipeId ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$pjId", (object)dto.ProcessJobId ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$cjId", (object)dto.ControlJobId ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("$ts", dto.TransportStatus);
                    //cmd.Parameters.AddWithValue("$ps", dto.ProcessingStatus);
                    //cmd.Parameters.AddWithValue("$idRs", dto.IdReadingStatus);
                    //cmd.Parameters.AddWithValue("$dnp", DbUtil.BoolToInt(dto.DoNotProcessFlag));
                    //cmd.Parameters.AddWithValue("$usage", DbUtil.BoolToInt(dto.Usage));

                    cmd.Parameters.Add("$key", DbType.String).Value = dto.UniqueKey;
                    cmd.Parameters.Add("$name", DbType.String).Value = dto.Name;
                    cmd.Parameters.Add("$loc", DbType.String).Value = dto.LocationId;
                    cmd.Parameters.Add("$srcPort", DbType.Int32).Value = dto.SourcePortId;
                    cmd.Parameters.Add("$srcSlot", DbType.Int32).Value = dto.SourceSlot;
                    cmd.Parameters.Add("$srcCid", DbType.String).Value = dto.SourceCarrierId;
                    if (false == string.IsNullOrWhiteSpace(dto.CurrentCarrierKey))
                    {
                        cmd.Parameters.Add("$curCKey", DbType.String).Value = dto.CurrentCarrierKey;
                    }
                    else
                    {
                        cmd.Parameters.Add("$curCKey", DbType.String).Value = DBNull.Value;
                    }
                    cmd.Parameters.Add("$dstPort", DbType.Int32).Value = dto.DestinationPortId;
                    cmd.Parameters.Add("$dstSlot", DbType.Int32).Value = dto.DestinationSlot;
                    cmd.Parameters.Add("$lotId", DbType.String).Value = dto.LotId;
                    cmd.Parameters.Add("$recipeId", DbType.String).Value = dto.RecipeId;
                    cmd.Parameters.Add("$pjId", DbType.String).Value = dto.ProcessJobId;
                    cmd.Parameters.Add("$cjId", DbType.String).Value = dto.ControlJobId;
                    cmd.Parameters.Add("$ts", DbType.String).Value = dto.TransportStatus.ToString();
                    cmd.Parameters.Add("$ps", DbType.String).Value = dto.ProcessingStatus.ToString();
                    cmd.Parameters.Add("$idRs", DbType.String).Value = dto.IdReadingStatus.ToString();
                    cmd.Parameters.Add("$dnp", DbType.Int32).Value = DbUtil.BoolToInt(dto.DoNotProcessFlag);
                    cmd.Parameters.Add("$usage", DbType.Int32).Value = DbUtil.BoolToInt(dto.Usage);

                    MaterialDbContext.LogCommand(cmd);
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = GetCommandForUpsert();

                    cmd.Parameters.Add("@SubstrateKey", DbType.String).Value = dto.UniqueKey;


                    // Dictionary 순서와 무관하게 "컬럼 이름"으로만 값을 매핑
                    foreach (var col in _extraKeys)
                    {
                        var pKey = $"@{col}";
                        dto.Extra.TryGetValue(col, out var value);
                        if (value == null || string.IsNullOrEmpty(value))
                        {
                            cmd.Parameters.Add(pKey, DbType.String).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add(pKey, DbType.String).Value = value;
                        }
                    }

                    MaterialDbContext.LogCommand(cmd);
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });

            foreach (var obs in _listeners)
            {
                obs.OnSubstrateCreated(dto.UniqueKey);
            }

            return result;
        }

//        private async Task UpsertInternalAsync(SubstrateItem dto, CancellationToken ct)
//        {
//            ct.ThrowIfCancellationRequested();

//            //using (_keyedLocker.Acquire(dto.UniqueKey))
//            using (var conn = _db.OpenConnection())
//            using (var tx = conn.BeginTransaction())
//            {
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.Transaction = tx;
//                    cmd.CommandText = @"
//INSERT INTO Substrate
//(UniqueKey, Name, LocationId,
// SourcePortId, SourceSlot,
// SourceCarrierId, CurrentCarrierKey,
// DestinationPortId, DestinationSlot,
// LotId, RecipeId, ProcessJobId, ControlJobId,
// TransportStatus, ProcessingStatus, IdReadingStatus,
// DoNotProcessFlag, Usage)
//VALUES
//($key, $name, $loc,
// $srcPort, $srcSlot,
// $srcCid, $curCKey,
// $dstPort, $dstSlot,
// $lotId, $recipeId, $pjId, $cjId,
// $ts, $ps, $idRs,
// $dnp, $usage)
//ON CONFLICT(UniqueKey) DO UPDATE SET
//    Name             = excluded.Name,
//    LocationId       = excluded.LocationId,
//    SourcePortId     = excluded.SourcePortId,
//    SourceSlot       = excluded.SourceSlot,
//    SourceCarrierId  = excluded.SourceCarrierId,
//    CurrentCarrierKey= excluded.CurrentCarrierKey,
//    DestinationPortId= excluded.DestinationPortId,
//    DestinationSlot  = excluded.DestinationSlot,
//    LotId            = excluded.LotId,
//    RecipeId         = excluded.RecipeId,
//    ProcessJobId     = excluded.ProcessJobId,
//    ControlJobId     = excluded.ControlJobId,
//    TransportStatus  = excluded.TransportStatus,
//    ProcessingStatus = excluded.ProcessingStatus,
//    IdReadingStatus  = excluded.IdReadingStatus,
//    DoNotProcessFlag = excluded.DoNotProcessFlag,
//    Usage            = excluded.Usage;
//";

//                    cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
//                    cmd.Parameters.AddWithValue("$name", (object)dto.Name ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$loc", (object)dto.LocationId ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$srcPort", dto.SourcePortId);
//                    cmd.Parameters.AddWithValue("$srcSlot", dto.SourceSlot);
//                    cmd.Parameters.AddWithValue("$srcCid", (object)dto.SourceCarrierId ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$curCKey", !string.IsNullOrWhiteSpace(dto.CurrentCarrierKey) ? (object)dto.CurrentCarrierKey : DBNull.Value);
//                    cmd.Parameters.AddWithValue("$dstPort", dto.DestinationPortId);
//                    cmd.Parameters.AddWithValue("$dstSlot", dto.DestinationSlot);
//                    cmd.Parameters.AddWithValue("$lotId", (object)dto.LotId ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$recipeId", (object)dto.RecipeId ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$pjId", (object)dto.ProcessJobId ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$cjId", (object)dto.ControlJobId ?? DBNull.Value);
//                    cmd.Parameters.AddWithValue("$ts", dto.TransportStatus);
//                    cmd.Parameters.AddWithValue("$ps", dto.ProcessingStatus);
//                    cmd.Parameters.AddWithValue("$idRs", dto.IdReadingStatus);
//                    cmd.Parameters.AddWithValue("$dnp", DbUtil.BoolToInt(dto.DoNotProcessFlag));
//                    cmd.Parameters.AddWithValue("$usage", DbUtil.BoolToInt(dto.Usage));

//                    await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
//                }

//                // 2) SubstrateExtra 와이드 테이블 Upsert
//                await UpsertSubstrateExtraAsync(conn, tx, dto, ct).ConfigureAwait(false);

//                #region <Extra - EAV 버전>
//                //                // Extra 재작성
//                //                using (var cmd = conn.CreateCommand())
//                //                {
//                //                    cmd.Transaction = tx;
//                //                    cmd.CommandText = "DELETE FROM SubstrateExtra WHERE SubstrateKey = $key;";
//                //                    cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
//                //                    await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
//                //                }

//                //                if (dto.Extra != null && dto.Extra.Count > 0)
//                //                {
//                //                    using (var cmd = conn.CreateCommand())
//                //                    {
//                //                        cmd.Transaction = tx;
//                //                        cmd.CommandText = @"
//                //INSERT INTO SubstrateExtra (SubstrateKey, ExtraKey, ExtraValue)
//                //VALUES ($key, $ek, $ev);
//                //";
//                //                        cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
//                //                        var pEk = cmd.CreateParameter();
//                //                        pEk.ParameterName = "$ek";
//                //                        cmd.Parameters.Add(pEk);
//                //                        var pEv = cmd.CreateParameter();
//                //                        pEv.ParameterName = "$ev";
//                //                        cmd.Parameters.Add(pEv);

//                //                        foreach (var kv in dto.Extra)
//                //                        {
//                //                            ct.ThrowIfCancellationRequested();
//                //                            pEk.Value = kv.Key;
//                //                            pEv.Value = (object)kv.Value ?? DBNull.Value;
//                //                            await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
//                //                        }
//                //                    }
//                //                }
//                #endregion </Extra - EAV 버전>

//                tx.Commit();
//            }

//            foreach (var obs in _listeners)
//            {
//                obs.OnSubstrateCreated(dto.UniqueKey, ct);
//            }
//        }

//        private async Task UpsertSubstrateExtraAsync(
//            SQLiteConnection conn,
//            SQLiteTransaction tx,
//            SubstrateItem dto,
//            CancellationToken ct)
//        {
//            var sb = new StringBuilder();
//            sb.Append("INSERT INTO SubstrateExtra (SubstrateKey");
//            foreach (var col in _extraKeys)
//            {
//                sb.Append(", ").Append(col);
//            }
//            sb.Append(") VALUES (@SubstrateKey");
//            foreach (var col in _extraKeys)
//            {
//                sb.Append(", @").Append(col);
//            }
//            sb.Append(") ON CONFLICT(SubstrateKey) DO UPDATE SET ");

//            for (int i = 0; i < _extraKeys.Length; i++)
//            {
//                var col = _extraKeys[i];
//                sb.Append(col).Append(" = @").Append(col);
//                if (i < _extraKeys.Length - 1)
//                    sb.Append(", ");
//            }
//            sb.Append(";");

//            using (var cmd = conn.CreateCommand())
//            {
//                cmd.Transaction = tx;
//                cmd.CommandText = sb.ToString();

//                cmd.Parameters.AddWithValue("@SubstrateKey", dto.UniqueKey);


//                // Dictionary 순서와 무관하게 "컬럼 이름"으로만 값을 매핑
//                foreach (var col in _extraKeys)
//                {
//                    dto.Extra.TryGetValue(col, out var value);
//                    if (value == null || string.IsNullOrEmpty(value))
//                    {
//                        cmd.Parameters.AddWithValue("@" + col, DBNull.Value);
//                    }
//                    else
//                    {
//                        cmd.Parameters.AddWithValue("@" + col, (object)value);
//                    }
//                }

//                await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
//            }
//        }

        public async Task DeleteAsync(string key)
        {
            ThrowIfDisposed();
            DbUtil.ValidateKey(key, nameof(key));

            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Substrate WHERE UniqueKey = $key;";
                    cmd.Parameters.Add("$key", DbType.String).Value = key;

                    MaterialDbContext.LogCommand(cmd);
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });

            foreach (var obs in _listeners)
            {
                obs.OnSubstrateDeleted(key);
            }

            //using (var conn = _db.OpenConnection())
            //using (var cmd = conn.CreateCommand())
            //{
            //    cmd.CommandText = "DELETE FROM Substrate WHERE UniqueKey = $key;";
            //    cmd.Parameters.AddWithValue("$key", key);
            //    await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            //}

            //foreach (var obs in _listeners)
            //{
            //    obs.OnSubstrateDeleted(key, ct);
            //}
        }

        public async Task ArchiveAsync(string key, string destinationPath)
        {
            // 개별 Substrate만 아카이브하는 정책이 필요하다면 별도 설계.
            // 지금 구조에서는 Carrier 아카이브에 딸려가는 게 기본 시나리오이므로,
            // 여기서는 단순 Delete + Observer 정도로 두어도 된다.
            //await DeleteAsync(key, ct).ConfigureAwait(false);

            foreach (var obs in _listeners)
            {
                obs.OnSubstrateArchived(key, destinationPath);
            }
        }

        private Task<List<SubstrateItem>> LoadDataFromStorageAsync()
        {
            ThrowIfDisposed();

            return _db.ExecuteReadAsync(async conn =>
            {
                var result = new List<SubstrateItem>();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT UniqueKey, Name, LocationId,
       SourcePortId, SourceSlot,
       SourceCarrierId, CurrentCarrierKey, 
       DestinationPortId, DestinationSlot,
       LotId, RecipeId, ProcessJobId, ControlJobId,
       TransportStatus, ProcessingStatus, IdReadingStatus,
       DoNotProcessFlag, Usage
FROM Substrate;
";

                    MaterialDbContext.LogCommand(cmd);
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            var dto = MapSubstrate(reader);
                            dto.Extra = LoadSubstrateExtra(conn, dto.UniqueKey);
                            result.Add(dto);
                        }
                    }
                }

                return result;
            });
        }
        private Task<bool> IsExistsAsync(string key)
        {
            ThrowIfDisposed();

            return _db.ExecuteReadAsync(async conn =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM Substrate WHERE UniqueKey = $key LIMIT 1;";
                    cmd.Parameters.Add("$key", DbType.String).Value = key;
                    MaterialDbContext.LogCommand(cmd);
                    var r = cmd.ExecuteScalar();

                    return r != null;
                }
            });
        }
        private string GetCommandForUpsert()
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO SubstrateExtra (SubstrateKey");
            foreach (var col in _extraKeys)
            {
                sb.Append(", ").Append(col);
            }
            sb.Append(") VALUES (@SubstrateKey");
            foreach (var col in _extraKeys)
            {
                sb.Append(", @").Append(col);
            }
            sb.Append(") ON CONFLICT(SubstrateKey) DO UPDATE SET ");

            for (int i = 0; i < _extraKeys.Length; i++)
            {
                var col = _extraKeys[i];
                sb.Append(col).Append(" = @").Append(col);
                if (i < _extraKeys.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(";");

            return sb.ToString();
        }
        private static SubstrateItem MapSubstrate(DbDataReader reader)
        {
            var dto = new SubstrateItem
            {
                UniqueKey = reader.GetString(0),
                Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                LocationId = reader.IsDBNull(2) ? null : reader.GetString(2),
                SourcePortId = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3)),
                SourceSlot = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4)),
                SourceCarrierId = reader.IsDBNull(5) ? null : reader.GetString(5),
                CurrentCarrierKey = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                DestinationPortId = reader.IsDBNull(7) ? 0 : Convert.ToInt32(reader.GetValue(7)),
                DestinationSlot = reader.IsDBNull(8) ? 0 : Convert.ToInt32(reader.GetValue(8)),
                LotId = reader.IsDBNull(9) ? null : reader.GetString(9),
                RecipeId = reader.IsDBNull(10) ? null : reader.GetString(10),
                ProcessJobId = reader.IsDBNull(11) ? null : reader.GetString(11),
                ControlJobId = reader.IsDBNull(12) ? null : reader.GetString(12),
                TransportStatus = reader.IsDBNull(13)
                    ? EFEM.Defines.MaterialTracking.TransportStates.AtSource
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(13)), EFEM.Defines.MaterialTracking.TransportStates.AtSource),
                ProcessingStatus = reader.IsDBNull(14)
                    ? EFEM.Defines.MaterialTracking.ProcessingStates.NeedsProcessing
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(14)), EFEM.Defines.MaterialTracking.ProcessingStates.NeedsProcessing),
                IdReadingStatus = reader.IsDBNull(15)
                    ? EFEM.Defines.MaterialTracking.IdReadingStates.NotConfirmed
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(15)), EFEM.Defines.MaterialTracking.IdReadingStates.NotConfirmed),
                DoNotProcessFlag = DbUtil.IntToBool(reader.IsDBNull(16) ? 0L : Convert.ToInt64(reader.GetValue(16))),
                Usage = DbUtil.IntToBool(reader.IsDBNull(17) ? 0L : Convert.ToInt64(reader.GetValue(17))),
                Extra = new Dictionary<string, string>(StringComparer.Ordinal)
            };
            return dto;
        }
        private Dictionary<string, string> LoadSubstrateExtra(SQLiteConnection conn, string key)
        {
            var dict = new Dictionary<string, string>(StringComparer.Ordinal);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $@"
SELECT SubstrateKey, {string.Join(", ", _extraKeys)}
FROM SubstrateExtra
WHERE SubstrateKey = $key;
";
                cmd.Parameters.Add("$key", DbType.String).Value = key;
                MaterialDbContext.LogCommand(cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dict = new Dictionary<string, string>();
                        for (int i = 0; i < _extraKeys.Length; ++i)
                        {
                            var col = _extraKeys[i];
                            var ordinal = reader.GetOrdinal(col);
                            if (!reader.IsDBNull(ordinal))
                            {
                                var v = reader.GetString(ordinal);
                                dict[col] = v;
                            }
                            else
                            {
                                dict[col] = string.Empty;
                            }
                        }
                    }
                }
            }
            return dict;
        }
        //        private static Dictionary<string, string> LoadSubstrateExtra(SQLiteConnection conn, string key)
        //        {
        //            var dict = new Dictionary<string, string>(StringComparer.Ordinal);
        //            using (var cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"
        //SELECT ExtraKey, ExtraValue
        //FROM SubstrateExtra
        //WHERE SubstrateKey = $key;
        //";
        //                cmd.Parameters.AddWithValue("$key", key);
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var k = reader.GetString(0);
        //                        var v = reader.IsDBNull(1) ? null : reader.GetString(1);
        //                        dict[k] = v;
        //                    }
        //                }
        //            }
        //            return dict;
        //        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            //_keyedLocker?.Dispose();
        }
    }

}
