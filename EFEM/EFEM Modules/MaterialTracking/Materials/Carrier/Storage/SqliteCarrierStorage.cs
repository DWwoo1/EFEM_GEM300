using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;
using System.Data.Common;

using EFEM.Database;
using EFEM.Database.DatabaseOnly;

namespace EFEM.MaterialTracking.CarrierStorage
{
    public sealed class SqliteCarrierStorage : ICarrierStorage, IDisposable
    {
        private readonly MaterialDbContext _db;
        //private readonly MonitorKeyedLocker _keyedLocker = new MonitorKeyedLocker();
        private readonly List<ICarrierEventObserver> _carrierEventListeners
            = new List<ICarrierEventObserver>();
        private string[] _extraKeys;
        private volatile bool _disposed;

        public SqliteCarrierStorage(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _extraKeys = _db.CarrierExtraKeys;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(SqliteCarrierStorage));
        }
        public void RegisterListner(ICarrierEventObserver carrierEvent)
        {
            if (carrierEvent == null) return;
            _carrierEventListeners.Add(carrierEvent);
        }
        public void InitializeStorage()
        {
            ThrowIfDisposed();
            // DbContext에서 이미 EnsureSchema 호출됨
        }
        public bool LoadDataFromStorage(out List<CarrierItem> dataFromStorage)
        {
            ThrowIfDisposed();

            dataFromStorage = LoadDataFromStorageAsync().ConfigureAwait(false)
                                            .GetAwaiter()
                                            .GetResult();

            return dataFromStorage.Count > 0;

            //            dataFromStorage = new List<CarrierItem>();

            //            CarrierItem item;
            //            using (var conn = _db.OpenConnection())
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.CommandText = @"
            //SELECT UniqueKey, LotId, CarrierId, PortId,
            //       AccessStatus, Capacity,
            //       LoadTime, UnloadTime
            //FROM Carrier;
            //";
            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                    {
            //                        item = MapCarrier(reader);
            //                        item.Extra = LoadCarrierExtra(conn, item.UniqueKey);
            //                        item.SlotMaps = LoadCarrierSlotMap(conn, item.UniqueKey);
            //                        dataFromStorage.Add(item);
            //                    }
            //                }
            //            }

            //            return dataFromStorage.Count > 0;
        }
        public bool IsExists(int portId, out string findKey)
        {
            ThrowIfDisposed();
            findKey = IsExistsAsync(portId).ConfigureAwait(false).GetAwaiter().GetResult();

            return string.IsNullOrWhiteSpace(findKey);

            //            using (var conn = _db.OpenConnection())
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.CommandText = @"
            //SELECT UniqueKey
            //FROM Carrier
            //WHERE PortId = $portId
            //LIMIT 1;
            //";
            //                cmd.Parameters.AddWithValue("$portId", portId);
            //                var result = cmd.ExecuteScalar();
            //                if (result != null && result != DBNull.Value)
            //                {
            //                    findKey = (string)result;
            //                    return true;
            //                }
            //            }

            //            return false;
        }
        public bool IsExists(string key)
        {
            ThrowIfDisposed();
            DbUtil.ValidateKey(key, nameof(key));

            return IsExistsAsync(key).ConfigureAwait(false).GetAwaiter().GetResult();
            //using (var conn = _db.OpenConnection())
            //using (var cmd = conn.CreateCommand())
            //{
            //    cmd.CommandText = "SELECT 1 FROM Carrier WHERE UniqueKey = $key LIMIT 1;";
            //    cmd.Parameters.AddWithValue("$key", key);
            //    var r = cmd.ExecuteScalar();
            //    return r != null;
            //}
        }
        public async Task<CarrierItem> GetByKeyAsync(string key)
        {
            ThrowIfDisposed();
            DbUtil.ValidateKey(key, nameof(key));

            return await _db.ExecuteReadAsync(async conn =>
            {
                CarrierItem dto = null;

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT UniqueKey, LotId, CarrierId, PortId,
       AccessStatus, Capacity,
       LoadTime, UnloadTime
FROM Carrier
WHERE UniqueKey = $key;
";
                    cmd.Parameters.Add("$key", DbType.String).Value = key;

                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            dto = MapCarrier(reader);
                        }
                    }
                }

                // 2) CarrierExtra 와이드 테이블
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"
SELECT CarrierKey, {string.Join(", ", _extraKeys)}
FROM CarrierExtra
WHERE CarrierKey = $key;
";
                    //                    cmd.CommandText = $@"
                    //SELECT CarrierKey, {string.Join(", ", _extraKeys)}
                    //FROM CarrierExtra
                    //WHERE CarrierKey = @Key;
                    //";
                    //cmd.Parameters.AddWithValue("@Key", key);
                    cmd.Parameters.Add("$key", DbType.String).Value = key;

                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            var extra = new Dictionary<string, string>(StringComparer.Ordinal);
                            for (int i = 0; i < _extraKeys.Length; i++)
                            {
                                var col = _extraKeys[i];
                                var ordinal = reader.GetOrdinal(col);
                                if (!reader.IsDBNull(ordinal))
                                {
                                    var v = reader.GetString(ordinal);
                                    extra[col] = v;
                                }
                            }

                            dto.Extra = extra;
                        }
                        else
                        {
                            dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);
                        }
                    }
                }
                if (dto != null)
                {
                    dto.Extra = LoadCarrierExtra(conn, dto.UniqueKey);
                    dto.SlotMaps = LoadCarrierSlotMap(conn, dto.UniqueKey);
                }

                return dto;

            }).ConfigureAwait(false);

            //            CarrierItem dto = null;

            //            using (var conn = _db.OpenConnection())
            //            {
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.CommandText = @"
            //SELECT UniqueKey, LotId, CarrierId, PortId,
            //       AccessStatus, Capacity,
            //       LoadTime, UnloadTime
            //FROM Carrier
            //WHERE UniqueKey = $key;
            //";
            //                    cmd.Parameters.AddWithValue("$key", key);

            //                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            //                    {
            //                        if (await reader.ReadAsync().ConfigureAwait(false))
            //                        {
            //                            dto = MapCarrier(reader);
            //                        }
            //                    }
            //                }

            //                // 2) CarrierExtra 와이드 테이블
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.CommandText = $@"
            //SELECT CarrierKey, {string.Join(", ", _extraKeys)}
            //FROM CarrierExtra
            //WHERE CarrierKey = @Key;
            //";
            //                    cmd.Parameters.AddWithValue("@Key", key);

            //                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            //                    {
            //                        if (await reader.ReadAsync().ConfigureAwait(false))
            //                        {
            //                            var extra = new Dictionary<string, string>(StringComparer.Ordinal);
            //                            for (int i = 0; i < _extraKeys.Length; i++)
            //                            {
            //                                var col = _extraKeys[i];
            //                                var ordinal = reader.GetOrdinal(col);
            //                                if (!reader.IsDBNull(ordinal))
            //                                {
            //                                    var v = reader.GetString(ordinal);
            //                                    extra[col] = v;
            //                                }
            //                            }

            //                            dto.Extra = extra;
            //                        }
            //                        else
            //                        {
            //                            dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);
            //                        }
            //                    }
            //                }
            //                if (dto != null)
            //                {
            //                    dto.Extra = LoadCarrierExtra(conn, dto.UniqueKey);
            //                    dto.SlotMaps = LoadCarrierSlotMap(conn, dto.UniqueKey);
            //                }
            //            }

            //            return dto;
        }
        public Task UpsertAsync(CarrierItem dto)
        {
            ThrowIfDisposed();
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            DbUtil.ValidateKey(dto.UniqueKey, nameof(dto.UniqueKey));

            if (dto.Extra == null)
                dto.Extra = new Dictionary<string, string>(StringComparer.Ordinal);
            if (dto.SlotMaps == null)
                dto.SlotMaps = new Dictionary<int, int>();

            return UpsertInternalAsync(dto);
        }
        private Task<List<CarrierItem>> LoadDataFromStorageAsync()
        {
            ThrowIfDisposed();

            return _db.ExecuteReadAsync(async conn =>
            {
                var result = new List<CarrierItem>();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT UniqueKey, LotId, CarrierId, PortId,
        AccessStatus, Capacity,
        LoadTime, UnloadTime
FROM Carrier;
";

                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            CarrierItem item;
                            item = MapCarrier(reader);
                            item.Extra = LoadCarrierExtra(conn, item.UniqueKey);
                            item.SlotMaps = LoadCarrierSlotMap(conn, item.UniqueKey);
                            result.Add(item);
                        }
                    }
                }

                return result;
            });
        }
        private Task<string> IsExistsAsync(int portId)
        {
            ThrowIfDisposed();
            return _db.ExecuteReadAsync(async conn =>
            {
                var findKey = string.Empty;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT UniqueKey
FROM Carrier
WHERE PortId = $portId
LIMIT 1;
";
                    cmd.Parameters.Add("$portId", DbType.Int32).Value = portId;
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        findKey = (string)result;
                    }
                }

                return findKey;
            });
        }
        private Task<bool> IsExistsAsync(string key)
        {
            ThrowIfDisposed();

            return _db.ExecuteReadAsync(async conn =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM Carrier WHERE UniqueKey = $key LIMIT 1;";
                    cmd.Parameters.Add("$key", DbType.String).Value = key;
                    var r = cmd.ExecuteScalar();

                    return r != null;
                }
            });
        }
        private async Task UpsertInternalAsync(CarrierItem dto)
        {
            var result = _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO Carrier
(UniqueKey, LotId, CarrierId, PortId,
    AccessStatus, Capacity,
    LoadTime, UnloadTime)
VALUES
($key, $lotId, $cid, $portId,
    $accessStatus, $capacity,
    $loadTime, $unloadTime)
ON CONFLICT(UniqueKey) DO UPDATE SET
    LotId        = excluded.LotId,
    CarrierId    = excluded.CarrierId,
    PortId       = excluded.PortId,
    AccessStatus = excluded.AccessStatus,
    Capacity     = excluded.Capacity,
    LoadTime     = excluded.LoadTime,
    UnloadTime   = excluded.UnloadTime;
";
                    cmd.Parameters.Add("$key", DbType.String).Value = dto.UniqueKey;
                    cmd.Parameters.Add("$lotId", DbType.String).Value = dto.LotId;
                    cmd.Parameters.Add("$cid", DbType.String).Value = dto.CarrierId;
                    cmd.Parameters.Add("$portId", DbType.Int32).Value = dto.PortId;
                    cmd.Parameters.Add("$accessStatus", DbType.Int32).Value = dto.AccessStatus;
                    cmd.Parameters.Add("$capacity", DbType.Int32).Value = dto.Capacity;
                    cmd.Parameters.Add("$loadTime", DbType.String).Value = dto.LoadTime;
                    cmd.Parameters.Add("$unloadTime", DbType.String).Value = dto.UnloadTime;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                if (dto.Extra != null)
                {
                    await UpsertCarrierExtraAsync(conn, tx, dto).ConfigureAwait(false);
                }

                #region <Extra - EAV 버전>
                //                Extra 재작성
                //                using (var cmd = conn.CreateCommand())
                //                {
                //                    cmd.Transaction = tx;
                //                    cmd.CommandText = "DELETE FROM CarrierExtra WHERE CarrierKey = $key;";
                //                    cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
                //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                //                }

                //                if (dto.Extra != null && dto.Extra.Count > 0)
                //                {
                //                    using (var cmd = conn.CreateCommand())
                //                    {
                //                        cmd.Transaction = tx;
                //                        cmd.CommandText = @"
                //INSERT INTO CarrierExtra (CarrierKey, ExtraKey, ExtraValue)
                //VALUES ($key, $ek, $ev);
                //";
                //                        cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
                //                        var pEk = cmd.CreateParameter();
                //                        pEk.ParameterName = "$ek";
                //                        cmd.Parameters.Add(pEk);
                //                        var pEv = cmd.CreateParameter();
                //                        pEv.ParameterName = "$ev";
                //                        cmd.Parameters.Add(pEv);

                //                        foreach (var kv in dto.Extra)
                //                        {
                //                            ct.ThrowIfCancellationRequested();
                //                            pEk.Value = kv.Key;
                //                            pEv.Value = (object)kv.Value ?? DBNull.Value;
                //                            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                //                        }
                //                    }
                //                }
                #endregion </Extra - EAV 버전>

                // SlotMap 재작성
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = "DELETE FROM CarrierSlotMap WHERE CarrierKey = $key;";
                    cmd.Parameters.Add("$key", DbType.String).Value = dto.UniqueKey;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                if (dto.SlotMaps != null && dto.SlotMaps.Count > 0)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tx;
                        cmd.CommandText = @"
            INSERT INTO CarrierSlotMap (CarrierKey, SlotNo, MapValue)
            VALUES ($key, $slotNo, $mapVal);
            ";
                        cmd.Parameters.Add("$key", DbType.String).Value = dto.UniqueKey;
                        var pSlot = cmd.CreateParameter();
                        pSlot.ParameterName = "$slotNo";
                        cmd.Parameters.Add(pSlot);
                        var pVal = cmd.CreateParameter();
                        pVal.ParameterName = "$mapVal";
                        cmd.Parameters.Add(pVal);

                        foreach (var kv in dto.SlotMaps)
                        {
                            pSlot.Value = kv.Key;
                            pVal.Value = kv.Value;
                            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                        }
                    }
                }
            });

            //            using (var conn = _db.OpenConnection())
            //            using (var tx = conn.BeginTransaction())
            //            {
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO Carrier
            //(UniqueKey, LotId, CarrierId, PortId,
            // AccessStatus, Capacity,
            // LoadTime, UnloadTime)
            //VALUES
            //($key, $lotId, $cid, $portId,
            // $accessStatus, $capacity,
            // $loadTime, $unloadTime)
            //ON CONFLICT(UniqueKey) DO UPDATE SET
            //    LotId        = excluded.LotId,
            //    CarrierId    = excluded.CarrierId,
            //    PortId       = excluded.PortId,
            //    AccessStatus = excluded.AccessStatus,
            //    Capacity     = excluded.Capacity,
            //    LoadTime     = excluded.LoadTime,
            //    UnloadTime   = excluded.UnloadTime;
            //";
            //                    cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
            //                    cmd.Parameters.AddWithValue("$lotId", (object)dto.LotId ?? DBNull.Value);
            //                    cmd.Parameters.AddWithValue("$cid", (object)dto.CarrierId ?? DBNull.Value);
            //                    cmd.Parameters.AddWithValue("$portId", dto.PortId);
            //                    cmd.Parameters.AddWithValue("$accessStatus", dto.AccessStatus);
            //                    cmd.Parameters.AddWithValue("$capacity", dto.Capacity);
            //                    cmd.Parameters.AddWithValue("$loadTime", (object)dto.LoadTime ?? DBNull.Value);
            //                    cmd.Parameters.AddWithValue("$unloadTime", (object)dto.UnloadTime ?? DBNull.Value);

            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                if (dto.Extra != null)
            //                {
            //                    await UpsertCarrierExtraAsync(conn, tx, dto, ct).ConfigureAwait(false);
            //                }

            //                #region <Extra - EAV 버전>
            //                //                Extra 재작성
            //                //                using (var cmd = conn.CreateCommand())
            //                //                {
            //                //                    cmd.Transaction = tx;
            //                //                    cmd.CommandText = "DELETE FROM CarrierExtra WHERE CarrierKey = $key;";
            //                //                    cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
            //                //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                //                }

            //                //                if (dto.Extra != null && dto.Extra.Count > 0)
            //                //                {
            //                //                    using (var cmd = conn.CreateCommand())
            //                //                    {
            //                //                        cmd.Transaction = tx;
            //                //                        cmd.CommandText = @"
            //                //INSERT INTO CarrierExtra (CarrierKey, ExtraKey, ExtraValue)
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
            //                //                            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                //                        }
            //                //                    }
            //                //                }
            //                #endregion </Extra - EAV 버전>

            //                // SlotMap 재작성
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = "DELETE FROM CarrierSlotMap WHERE CarrierKey = $key;";
            //                    cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                if (dto.SlotMaps != null && dto.SlotMaps.Count > 0)
            //                {
            //                    using (var cmd = conn.CreateCommand())
            //                    {
            //                        cmd.Transaction = tx;
            //                        cmd.CommandText = @"
            //INSERT INTO CarrierSlotMap (CarrierKey, SlotNo, MapValue)
            //VALUES ($key, $slotNo, $mapVal);
            //";
            //                        cmd.Parameters.AddWithValue("$key", dto.UniqueKey);
            //                        var pSlot = cmd.CreateParameter();
            //                        pSlot.ParameterName = "$slotNo";
            //                        cmd.Parameters.Add(pSlot);
            //                        var pVal = cmd.CreateParameter();
            //                        pVal.ParameterName = "$mapVal";
            //                        cmd.Parameters.Add(pVal);

            //                        foreach (var kv in dto.SlotMaps)
            //                        {
            //                            ct.ThrowIfCancellationRequested();
            //                            pSlot.Value = kv.Key;
            //                            pVal.Value = kv.Value;
            //                            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                        }
            //                    }
            //                }

            //                tx.Commit();
            //            }
        }
        private async Task UpsertCarrierExtraAsync(
            SQLiteConnection conn,
            SQLiteTransaction tx,
            CarrierItem dto)
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO CarrierExtra (CarrierKey");
            foreach (var col in _extraKeys)
            {
                sb.Append(", ").Append(col);
            }
            sb.Append(") VALUES (@CarrierKey");
            foreach (var col in _extraKeys)
            {
                sb.Append(", @").Append(col);
            }
            sb.Append(") ON CONFLICT(CarrierKey) DO UPDATE SET ");

            for (int i = 0; i < _extraKeys.Length; i++)
            {
                var col = _extraKeys[i];
                sb.Append(col).Append(" = @").Append(col);
                if (i < _extraKeys.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(";");

            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = sb.ToString();

                cmd.Parameters.Add("@CarrierKey", DbType.String).Value = dto.UniqueKey;

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

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
        public async Task DeleteAsync(string key)
        {
            // 아카이브 없이 삭제. 트리거는 archive.*에 의존하므로
            // 임시 메모리 DB를 archive로 attach 후 삭제해서 에러를 피한다.
            ThrowIfDisposed();
            DbUtil.ValidateKey(key, nameof(key));

            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Carrier WHERE UniqueKey = $key;";
                    cmd.Parameters.Add("$key", DbType.String).Value = key;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }

        public Task ArchiveAsync(string key, int portId, string archiveDbPath)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(archiveDbPath))
                throw new ArgumentNullException(nameof(archiveDbPath));
            DbUtil.ValidateKey(key, nameof(key));

            return ArchiveInternalAsync(key, portId, archiveDbPath);
        }

        private async Task PrepareToArchiveAsync(string archiveFilePath, string carrierKey)
        {
            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                // 1) 날짜별 아카이브 DB 붙이기 (YYYYMMDD.db)
                var escapedPath = archiveFilePath.Replace("'", "''");
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = $"ATTACH DATABASE '{escapedPath}' AS archive;";
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                _db.EnsureArchiveSchemaAndMigrate(conn, tx);

                // 2) archive DB 안에 Archive 테이블 + ArchiveAt 테이블 생성 보장
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    //cmd.CommandText = ArchiveSchemaSql.ArchiveSchema;

                    cmd.CommandText = _db.GetArchiveCommand();
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 3) Carrier -> archive.CarrierArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.Carrier (
        UniqueKey, LotId, CarrierId, PortId,
        AccessStatus, Capacity,
        LoadTime, UnloadTime
    )
    SELECT
        UniqueKey, LotId, CarrierId, PortId,
        AccessStatus, Capacity,
        LoadTime, UnloadTime
    FROM Carrier
    WHERE UniqueKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 4) CarrierSlotMap -> archive.CarrierSlotMapArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.CarrierSlotMap (
        CarrierKey, SlotNo, MapValue
    )
    SELECT
        CarrierKey, SlotNo, MapValue
    FROM CarrierSlotMap
    WHERE CarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 5) CarrierExtra -> archive.CarrierExtraArchive
                using (var cmd = conn.CreateCommand())
                {
                    // 조합
                    cmd.Transaction = tx;
                    cmd.CommandText = _db.GetArchiveCarrierExtraCommand();

                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 6) 현재 이 Carrier 위에 있는 Substrate -> archive.SubstrateArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.Substrate (
        UniqueKey, Name, LocationId,
        SourcePortId, SourceSlot,
        SourceCarrierId, CurrentCarrierKey,
        DestinationPortId, DestinationSlot,
        LotId, RecipeId, ProcessJobId, ControlJobId,
        TransportStatus, ProcessingStatus, IdReadingStatus,
        DoNotProcessFlag, Usage
    )
    SELECT
        s.UniqueKey, s.Name, s.LocationId,
        s.SourcePortId, s.SourceSlot,
        s.SourceCarrierId, s.CurrentCarrierKey,
        s.DestinationPortId, s.DestinationSlot,
        s.LotId, s.RecipeId, s.ProcessJobId, s.ControlJobId,
        s.TransportStatus, s.ProcessingStatus, s.IdReadingStatus,
        s.DoNotProcessFlag, s.Usage
    FROM Substrate AS s
    WHERE s.CurrentCarrierKey = $key;
    ";

                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 7) SubstrateExtra -> archive.SubstrateExtraArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = _db.GetArchiveSubstrateExtraCommand();
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                //            // 8) StayHistory -> archive.SubstrateStayHistoryArchive
                //            using (var cmd = conn.CreateCommand())
                //            {
                //                cmd.Transaction = tx;
                //                cmd.CommandText = @"
                //INSERT INTO archive.SubstrateStayHistory (
                //    Id, SubstrateKey, LocationName, LocationType,
                //    StayStartTime, StayEndTime,
                //    StartAction, EndAction
                //)
                //SELECT
                //    h.Id, h.SubstrateKey, h.LocationName, h.LocationType,
                //    h.StayStartTime, h.StayEndTime,
                //    h.StartAction, h.EndAction
                //FROM SubstrateStayHistory AS h
                //JOIN Substrate AS s
                //  ON s.UniqueKey = h.SubstrateKey
                //WHERE s.CurrentCarrierKey = $key;
                //";
                //                cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                //                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                //            }

                // 8) LocationHistory -> archive.SubstrateStayHistoryArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO archive.SubstrateLocationHistory (
    SubstrateKey,
    FromLocationName, FromLocationKind,
    ToLocationName, ToLocationKind,
    ChangeTime, Reason
)
SELECT
    h.SubstrateKey,
    h.FromLocationName, h.FromLocationKind,
    h.ToLocationName, h.ToLocationKind,
    h.ChangeTime, h.Reason
FROM SubstrateLocationHistory AS h
JOIN Substrate AS s
  ON s.UniqueKey = h.SubstrateKey
WHERE s.CurrentCarrierKey = $key;
";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 9) ProcessingHistory -> archive.SubstrateProcessingHistoryArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.SubstrateProcessingHistory (
        SubstrateKey, EventTime,
        OldState, NewState,
        ControlJobId, ProcessJobId,
        LocationId, Description
    )
    SELECT
        ph.SubstrateKey, ph.EventTime,
        ph.OldState, ph.NewState,
        ph.ControlJobId, ph.ProcessJobId,
        ph.LocationId, ph.Description
    FROM SubstrateProcessingHistory AS ph
    JOIN Substrate AS s
      ON s.UniqueKey = ph.SubstrateKey
    WHERE s.CurrentCarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 10) ArchiveAt – Carrier
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.ArchiveAt (ItemKey, ItemKind, ArchivedAt)
    VALUES ($key, 0, datetime('now'));
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 11) ArchiveAt – 이 Carrier 위에 있던 Substrate 들
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.ArchiveAt (ItemKey, ItemKind, ArchivedAt)
    SELECT
        s.UniqueKey, 1, datetime('now')
    FROM Substrate AS s
    WHERE s.CurrentCarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 12) 메인 DB에서 Carrier 삭제 -> FK CASCADE로 Substrate/History 전부 삭제
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = "DELETE FROM Carrier WHERE UniqueKey = $key;";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                //// 13) DETACH archive
                //using (var cmd = conn.CreateCommand())
                //{
                //    cmd.CommandText = "DETACH DATABASE archive;";
                //    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                //}
            });
        }
        private async Task DetachArchiveAsync()
        {
            await _db.ExecuteWriteAsync(async (conn) =>
            {
                // 13) DETACH archive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DETACH DATABASE archive;";
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
        private async Task MoveToArchiveData(string carrierKey)
        {
            await _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                // 3) Carrier -> archive.CarrierArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.Carrier (
        UniqueKey, LotId, CarrierId, PortId,
        AccessStatus, Capacity,
        LoadTime, UnloadTime
    )
    SELECT
        UniqueKey, LotId, CarrierId, PortId,
        AccessStatus, Capacity,
        LoadTime, UnloadTime
    FROM Carrier
    WHERE UniqueKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 4) CarrierSlotMap -> archive.CarrierSlotMapArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.CarrierSlotMap (
        CarrierKey, SlotNo, MapValue
    )
    SELECT
        CarrierKey, SlotNo, MapValue
    FROM CarrierSlotMap
    WHERE CarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 5) CarrierExtra -> archive.CarrierExtraArchive
                using (var cmd = conn.CreateCommand())
                {
                    // 조합
                    cmd.Transaction = tx;
                    cmd.CommandText = _db.GetArchiveCarrierExtraCommand();

                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 6) 현재 이 Carrier 위에 있는 Substrate -> archive.SubstrateArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.Substrate (
        UniqueKey, Name, LocationId,
        SourcePortId, SourceSlot,
        SourceCarrierId, CurrentCarrierKey,
        DestinationPortId, DestinationSlot,
        LotId, RecipeId, ProcessJobId, ControlJobId,
        TransportStatus, ProcessingStatus, IdReadingStatus,
        DoNotProcessFlag, Usage
    )
    SELECT
        s.UniqueKey, s.Name, s.LocationId,
        s.SourcePortId, s.SourceSlot,
        s.SourceCarrierId, s.CurrentCarrierKey,
        s.DestinationPortId, s.DestinationSlot,
        s.LotId, s.RecipeId, s.ProcessJobId, s.ControlJobId,
        s.TransportStatus, s.ProcessingStatus, s.IdReadingStatus,
        s.DoNotProcessFlag, s.Usage
    FROM Substrate AS s
    WHERE s.CurrentCarrierKey = $key;
    ";

                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 7) SubstrateExtra -> archive.SubstrateExtraArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = _db.GetArchiveSubstrateExtraCommand();
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 8) StayHistory -> archive.SubstrateStayHistoryArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.SubstrateStayHistory (
        Id, SubstrateKey, LocationName, LocationType,
        StayStartTime, StayEndTime,
        StartAction, EndAction
    )
    SELECT
        h.Id, h.SubstrateKey, h.LocationName, h.LocationType,
        h.StayStartTime, h.StayEndTime,
        h.StartAction, h.EndAction
    FROM SubstrateStayHistory AS h
    JOIN Substrate AS s
      ON s.UniqueKey = h.SubstrateKey
    WHERE s.CurrentCarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 9) ProcessingHistory -> archive.SubstrateProcessingHistoryArchive
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.SubstrateProcessingHistory (
        Id, SubstrateKey, EventTime,
        OldState, NewState,
        ControlJobId, ProcessJobId,
        LocationId, Description
    )
    SELECT
        ph.Id, ph.SubstrateKey, ph.EventTime,
        ph.OldState, ph.NewState,
        ph.ControlJobId, ph.ProcessJobId,
        ph.LocationId, ph.Description
    FROM SubstrateProcessingHistory AS ph
    JOIN Substrate AS s
      ON s.UniqueKey = ph.SubstrateKey
    WHERE s.CurrentCarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 10) ArchiveAt – Carrier
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.ArchiveAt (ItemKey, ItemKind, ArchivedAt)
    VALUES ($key, 0, datetime('now'));
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 11) ArchiveAt – 이 Carrier 위에 있던 Substrate 들
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
    INSERT INTO archive.ArchiveAt (ItemKey, ItemKind, ArchivedAt)
    SELECT
        s.UniqueKey, 1, datetime('now')
    FROM Substrate AS s
    WHERE s.CurrentCarrierKey = $key;
    ";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                // 12) 메인 DB에서 Carrier 삭제 -> FK CASCADE로 Substrate/History 전부 삭제
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = "DELETE FROM Carrier WHERE UniqueKey = $key;";
                    cmd.Parameters.Add("$key", DbType.String).Value = carrierKey;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });

        }
        private async Task ArchiveInternalAsync(string carrierKey, int portId, string archiveDbPath)
        {
            var date = DateTime.Now;
            archiveDbPath = $@"{_db.DataBasePath}\Archive\{date.Year:0000}{date.Month:00}{date.Day:00}.db";

            var dir = Path.GetDirectoryName(archiveDbPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            await PrepareToArchiveAsync(archiveDbPath, carrierKey).ConfigureAwait(false);

            await DetachArchiveAsync().ConfigureAwait(false);

            //await DetachArchiveAsync().ConfigureAwait(false);

            //await MoveToArchiveData(carrierKey, ct).ConfigureAwait(false);

            //await DetachArchiveAsync().ConfigureAwait(false);

            //        using (var conn = new SQLiteConnection(_connectionString))
            //        {
            //            conn.Open();

            //            using (var tx = conn.BeginTransaction())
            //            {
            //                // 1) 날짜별 아카이브 DB 붙이기 (YYYYMMDD.db)
            //                var escapedPath = archiveDbPath.Replace("'", "''");
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = $"ATTACH DATABASE '{escapedPath}' AS archive;";
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 2) archive DB 안에 Archive 테이블 + ArchiveAt 테이블 생성 보장                    
            //                _db.EnsureArchiveSchemaWithSameTablesAndArchiveAt(conn, tx);

            //                tx.Commit();
            //            }

            //            using (var tx = conn.BeginTransaction())
            //            {
            //                // 3) Carrier -> archive.CarrierArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.Carrier (
            //    UniqueKey, LotId, CarrierId, PortId,
            //    AccessStatus, Capacity,
            //    LoadTime, UnloadTime
            //)
            //SELECT
            //    UniqueKey, LotId, CarrierId, PortId,
            //    AccessStatus, Capacity,
            //    LoadTime, UnloadTime
            //FROM Carrier
            //WHERE UniqueKey = $key;
            //";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 4) CarrierSlotMap -> archive.CarrierSlotMapArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.CarrierSlotMap (
            //    CarrierKey, SlotNo, MapValue
            //)
            //SELECT
            //    CarrierKey, SlotNo, MapValue
            //FROM CarrierSlotMap
            //WHERE CarrierKey = $key;
            //";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 5) CarrierExtra -> archive.CarrierExtraArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    // 조합
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = GetArchiveCarrierExtraSchema();

            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 6) 현재 이 Carrier 위에 있는 Substrate -> archive.SubstrateArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.Substrate (
            //    UniqueKey, Name, LocationId,
            //    SourcePortId, SourceSlot,
            //    SourceCarrierId, CurrentCarrierKey,
            //    DestinationPortId, DestinationSlot,
            //    LotId, RecipeId, ProcessJobId, ControlJobId,
            //    TransportStatus, ProcessingStatus, IdReadingStatus,
            //    DoNotProcessFlag, Usage
            //)
            //SELECT
            //    s.UniqueKey, s.Name, s.LocationId,
            //    s.SourcePortId, s.SourceSlot,
            //    s.SourceCarrierId, s.CurrentCarrierKey,
            //    s.DestinationPortId, s.DestinationSlot,
            //    s.LotId, s.RecipeId, s.ProcessJobId, s.ControlJobId,
            //    s.TransportStatus, s.ProcessingStatus, s.IdReadingStatus,
            //    s.DoNotProcessFlag, s.Usage
            //FROM Substrate AS s
            //WHERE s.CurrentCarrierKey = $key;
            //";

            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 7) SubstrateExtra -> archive.SubstrateExtraArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = GetArchiveSubstrateExtraSchema();
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 8) StayHistory -> archive.SubstrateStayHistoryArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.SubstrateStayHistory (
            //    Id, SubstrateKey, LocationName, LocationType,
            //    StayStartTime, StayEndTime,
            //    StartAction, EndAction
            //)
            //SELECT
            //    h.Id, h.SubstrateKey, h.LocationName, h.LocationType,
            //    h.StayStartTime, h.StayEndTime,
            //    h.StartAction, h.EndAction
            //FROM SubstrateStayHistory AS h
            //JOIN Substrate AS s
            //  ON s.UniqueKey = h.SubstrateKey
            //WHERE s.CurrentCarrierKey = $key;
            //";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 9) ProcessingHistory -> archive.SubstrateProcessingHistoryArchive
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.SubstrateProcessingHistory (
            //    Id, SubstrateKey, EventTime,
            //    OldState, NewState,
            //    ControlJobId, ProcessJobId,
            //    LocationId, Description
            //)
            //SELECT
            //    ph.Id, ph.SubstrateKey, ph.EventTime,
            //    ph.OldState, ph.NewState,
            //    ph.ControlJobId, ph.ProcessJobId,
            //    ph.LocationId, ph.Description
            //FROM SubstrateProcessingHistory AS ph
            //JOIN Substrate AS s
            //  ON s.UniqueKey = ph.SubstrateKey
            //WHERE s.CurrentCarrierKey = $key;
            //";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 10) ArchiveAt – Carrier
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.ArchiveAt (ItemKey, ItemKind, ArchivedAt)
            //VALUES ($key, 0, datetime('now'));
            //";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 11) ArchiveAt – 이 Carrier 위에 있던 Substrate 들
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = @"
            //INSERT INTO archive.ArchiveAt (ItemKey, ItemKind, ArchivedAt)
            //SELECT
            //    s.UniqueKey, 1, datetime('now')
            //FROM Substrate AS s
            //WHERE s.CurrentCarrierKey = $key;
            //";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                // 12) 메인 DB에서 Carrier 삭제 -> FK CASCADE로 Substrate/History 전부 삭제
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText = "DELETE FROM Carrier WHERE UniqueKey = $key;";
            //                    cmd.Parameters.AddWithValue("$key", carrierKey);
            //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //                }

            //                tx.Commit();
            //            }

            //            // 13) DETACH archive
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.CommandText = "DETACH DATABASE archive;";
            //                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            //            }
            //        }

            // 상위 Observer 통보
            foreach (var obs in _carrierEventListeners)
            {
                obs?.OnCarrierArchived(portId, archiveDbPath);
            }
        }

        private static CarrierItem MapCarrier(DbDataReader reader)
        {
            var dto = new CarrierItem
            {
                UniqueKey = reader.GetString(0),
                LotId = reader.IsDBNull(1) ? null : reader.GetString(1),
                CarrierId = reader.IsDBNull(2) ? null : reader.GetString(2),
                PortId = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3)),
                AccessStatus = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4)),
                Capacity = reader.IsDBNull(5) ? 0 : Convert.ToInt32(reader.GetValue(5)),
                LoadTime = reader.IsDBNull(6) ? null : reader.GetString(6),
                UnloadTime = reader.IsDBNull(7) ? null : reader.GetString(7),
                SlotMaps = new Dictionary<int, int>(),
                Extra = new Dictionary<string, string>(StringComparer.Ordinal)
            };

            return dto;
        }

        private Dictionary<string, string> LoadCarrierExtra(SQLiteConnection conn, string key)
        {
            var dict = new Dictionary<string, string>(StringComparer.Ordinal);

            // 2) CarrierExtra 와이드 테이블
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $@"
SELECT CarrierKey, {string.Join(", ", _extraKeys)}
FROM CarrierExtra
WHERE CarrierKey = $key;
";
                cmd.Parameters.Add("$key", DbType.String).Value = key;
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
        private static Dictionary<int, int> LoadCarrierSlotMap(SQLiteConnection conn, string key)
        {
            var dict = new Dictionary<int, int>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT SlotNo, MapValue
FROM CarrierSlotMap
WHERE CarrierKey = $key
ORDER BY SlotNo;
";
                cmd.Parameters.Add("$key", DbType.String).Value = key;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var slot = Convert.ToInt32(reader.GetValue(0));
                        var mv = Convert.ToInt32(reader.GetValue(1));
                        dict[slot] = mv;
                    }
                }
            }
            return dict;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            //_keyedLocker?.Dispose();
        }
    }
}
