using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using EFEM.Database;
using EFEM.Database.DatabaseOnly;
using EFEM.History;

namespace EFEM.MaterialTracking.Inspection
{
    /// <summary>
    /// 2026.07.09. jhlim [ADD] DB 조회(검색) 페이지 전용 read-only 조회기.
    ///
    /// 기존 SqliteCarrierStorage/SqliteSubstrateStorage 는 키/포트 단건 read 이고 처리이력엔 read 가 없어,
    /// 검색(다중 조건) + 상세(캐리어↔안착기판/랏이력, 기판↔랏이력/처리이력/위치이력)를 이 클래스에서 새로 담당한다.
    /// 운영 스토리지/매니저는 건드리지 않는다.
    ///
    /// 범위: main DB(현재 설비 내/미제거) + 일자별 Archive\yyyyMMdd.db(배출/제거된 과거). 두 소스를 코드로 병합.
    /// 견고성(SqliteHistoryQuery 관용구 차용):
    /// - 모든 조회는 실패 시 던지지 않고 빈/부분 결과 반환.
    /// - archive 파일은 존재/테이블/컬럼 유무를 선확인(구버전 파일은 스키마·마이그레이션 편차 가능).
    /// - main 읽기는 MaterialDbContext.OpenConnection(WAL), archive 는 read-only 직접 커넥션.
    /// 스레드: UI 를 막지 않도록 호출측(서브뷰)이 Task.Run 백그라운드에서 부른다.
    /// </summary>
    public sealed class MaterialDatabaseQuery
    {
        #region <Constructors>
        public MaterialDatabaseQuery(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _carrierExtraKeys = _db.CarrierExtraKeys ?? new string[0];
            _substrateExtraKeys = _db.SubstrateExtraKeys ?? new string[0];
        }
        #endregion </Constructors>

        #region <Fields>
        // LotHistoryEvent.EventTime 기록 포맷(로컬, SqliteHistoryStore와 동일). 처리/위치 이력은 ISO(UTC)라 별도 파서.
        private const string LotEventTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static readonly Regex ArchiveFileRegex = new Regex(@"^(\d{4})(\d{2})(\d{2})\.db$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex IdentifierRegex = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$", RegexOptions.Compiled);

        /// <summary>"그 외 조건" 필드 선택 목록(UI)과 화이트리스트 검증(백엔드) 모두에 쓰는 Carrier 기본 컬럼명. Carrier 테이블 컬럼과 정확히 일치해야 한다.</summary>
        public static readonly string[] CarrierBaseFieldNames =
        {
            "UniqueKey", "LotId", "CarrierId", "PortId", "AccessStatus", "Capacity", "LoadTime", "UnloadTime"
        };
        /// <summary>"그 외 조건" 필드 선택 목록(UI)과 화이트리스트 검증(백엔드) 모두에 쓰는 Substrate 기본 컬럼명. Substrate 테이블 컬럼과 정확히 일치해야 한다.</summary>
        public static readonly string[] SubstrateBaseFieldNames =
        {
            "UniqueKey", "Name", "OriginName", "LocationId", "SourcePortId", "SourceSlot", "SourceCarrierId",
            "CurrentCarrierKey", "DestinationPortId", "DestinationSlot", "LotId", "RecipeId", "ProcessJobId",
            "ControlJobId", "TransportStatus", "ProcessingStatus", "IdReadingStatus", "DoNotProcessFlag", "Usage"
        };

        private readonly MaterialDbContext _db;
        private readonly string[] _carrierExtraKeys;
        private readonly string[] _substrateExtraKeys;
        #endregion </Fields>

        #region <Properties>
        /// <summary>"그 외 조건" 필드 선택 목록에 쓰는 Carrier Extra 속성 키 목록.</summary>
        public string[] CarrierExtraKeys => _carrierExtraKeys;
        /// <summary>"그 외 조건" 필드 선택 목록에 쓰는 Substrate Extra 속성 키 목록.</summary>
        public string[] SubstrateExtraKeys => _substrateExtraKeys;
        #endregion </Properties>

        #region <Archive 열거>
        /// <summary>날짜 범위(양끝 포함)에 해당하는 archive DB 파일 경로 목록. (파일명 yyyyMMdd 기준, 오름차순)</summary>
        public List<string> ListArchiveDbPathsInRange(DateTime start, DateTime end)
        {
            var result = new List<string>();
            try
            {
                var dir = Path.Combine(_db.DataBasePath, "Archive");
                if (false == Directory.Exists(dir))
                    return result;

                DateTime lo = start.Date;
                DateTime hi = end.Date;
                if (hi < lo) { var t = lo; lo = hi; hi = t; }

                var dated = new List<KeyValuePair<DateTime, string>>();
                foreach (var path in Directory.GetFiles(dir, "*.db"))
                {
                    var m = ArchiveFileRegex.Match(Path.GetFileName(path));
                    if (false == m.Success)
                        continue;

                    if (false == int.TryParse(m.Groups[1].Value, out int y) ||
                        false == int.TryParse(m.Groups[2].Value, out int mo) ||
                        false == int.TryParse(m.Groups[3].Value, out int d))
                        continue;

                    DateTime fileDate;
                    try { fileDate = new DateTime(y, mo, d); }
                    catch { continue; }

                    if (fileDate < lo || fileDate > hi)
                        continue;

                    dated.Add(new KeyValuePair<DateTime, string>(fileDate, path));
                }

                dated.Sort((a, b) => a.Key.CompareTo(b.Key));
                for (int i = 0; i < dated.Count; ++i)
                    result.Add(dated[i].Value);
            }
            catch
            {
            }
            return result;
        }
        #endregion </Archive 열거>

        #region <검색>
        public List<CarrierSearchRow> SearchCarriers(CarrierSearchCriteria criteria)
        {
            var result = new List<CarrierSearchRow>();
            if (criteria == null)
                return result;

            // 이력 조회는 아카이브(배출/제거된 자재)만 대상으로 한다. 현재 DB(main, 설비 내 진행 중 자재)는 제외.
            foreach (var archivePath in ListArchiveDbPathsInRange(criteria.StartDate, criteria.EndDate))
                RunOnSource(MaterialSource.Archive, archivePath, conn => SearchCarriersInConnection(conn, criteria, MaterialSource.Archive, archivePath, result));

            return result;
        }
        private void SearchCarriersInConnection(SQLiteConnection conn, CarrierSearchCriteria criteria, MaterialSource source, string archivePath, List<CarrierSearchRow> result)
        {
            if (false == TableExists(conn, "Carrier"))
                return;

            var where = new List<string>();
            using (var cmd = conn.CreateCommand())
            {
                if (false == string.IsNullOrWhiteSpace(criteria.CarrierId))
                {
                    where.Add("CarrierId LIKE $cid");
                    cmd.Parameters.Add("$cid", DbType.String).Value = Like(criteria.CarrierId);
                }
                if (false == string.IsNullOrWhiteSpace(criteria.LotId))
                {
                    where.Add("LotId LIKE $lot");
                    cmd.Parameters.Add("$lot", DbType.String).Value = Like(criteria.LotId);
                }
                if (criteria.PortId.HasValue)
                {
                    where.Add("PortId = $port");
                    cmd.Parameters.Add("$port", DbType.Int32).Value = criteria.PortId.Value;
                }

                if (false == TryAddOtherCondition(conn, cmd, where, "CarrierExtra", "CarrierKey",
                    CarrierBaseFieldNames, _carrierExtraKeys,
                    criteria.OtherFieldName, criteria.OtherFieldValue, criteria.OtherFieldExactMatch))
                    return;

                cmd.CommandText = BuildCarrierSelect(
                    (where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : string.Empty) + " ORDER BY LoadTime");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(ToSearchRow(MapCarrierBasic(reader), source, archivePath));
                }
            }
        }

        public List<SubstrateSearchRow> SearchSubstrates(SubstrateSearchCriteria criteria)
        {
            var result = new List<SubstrateSearchRow>();
            if (criteria == null)
                return result;

            // 이력 조회는 아카이브(배출/제거된 자재)만 대상으로 한다. 현재 DB(main, 설비 내 진행 중 자재)는 제외.
            foreach (var archivePath in ListArchiveDbPathsInRange(criteria.StartDate, criteria.EndDate))
                RunOnSource(MaterialSource.Archive, archivePath, conn => SearchSubstratesInConnection(conn, criteria, MaterialSource.Archive, archivePath, result));

            return result;
        }
        private void SearchSubstratesInConnection(SQLiteConnection conn, SubstrateSearchCriteria criteria, MaterialSource source, string archivePath, List<SubstrateSearchRow> result)
        {
            if (false == TableExists(conn, "Substrate"))
                return;

            bool hasOrigin = GetColumns(conn, "Substrate").Contains("OriginName");

            var where = new List<string>();
            using (var cmd = conn.CreateCommand())
            {
                if (false == string.IsNullOrWhiteSpace(criteria.Name))
                {
                    where.Add(hasOrigin ? "(Name LIKE $name OR OriginName LIKE $name)" : "(Name LIKE $name)");
                    cmd.Parameters.Add("$name", DbType.String).Value = Like(criteria.Name);
                }
                if (false == string.IsNullOrWhiteSpace(criteria.LotId))
                {
                    where.Add("LotId LIKE $lot");
                    cmd.Parameters.Add("$lot", DbType.String).Value = Like(criteria.LotId);
                }
                if (criteria.DestinationPortId.HasValue)
                {
                    where.Add("DestinationPortId = $destPort");
                    cmd.Parameters.Add("$destPort", DbType.Int32).Value = criteria.DestinationPortId.Value;
                }

                if (false == TryAddOtherCondition(conn, cmd, where, "SubstrateExtra", "SubstrateKey",
                    SubstrateBaseFieldNames, _substrateExtraKeys,
                    criteria.OtherFieldName, criteria.OtherFieldValue, criteria.OtherFieldExactMatch))
                    return;

                cmd.CommandText = BuildSubstrateSelect(hasOrigin,
                    (where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : string.Empty) + " ORDER BY Name");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(ToSearchRow(MapSubstrateFull(reader), source, archivePath));
                }
            }
        }
        #endregion </검색>

        #region <캐리어 상세>
        public CarrierItem GetCarrier(CarrierSearchRow row)
        {
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return null;

            CarrierItem dto = null;
            RunOnSource(row.Source, row.ArchiveDbPath, conn =>
            {
                if (false == TableExists(conn, "Carrier"))
                    return;

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = BuildCarrierSelect("WHERE UniqueKey = $key");
                    cmd.Parameters.Add("$key", DbType.String).Value = row.UniqueKey;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            dto = MapCarrierBasic(reader);
                    }
                }

                if (dto != null)
                {
                    dto.SlotMaps = LoadCarrierSlotMap(conn, row.UniqueKey);
                    dto.Extra = LoadExtra(conn, "CarrierExtra", "CarrierKey", _carrierExtraKeys, row.UniqueKey);
                }
            });
            return dto;
        }

        /// <summary>선택한 캐리어에 현재 안착(CurrentCarrierKey) 되어 있는 기판 목록. (같은 소스 DB)</summary>
        public List<SubstrateItem> GetSubstratesInCarrier(CarrierSearchRow row)
        {
            var result = new List<SubstrateItem>();
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return result;

            RunOnSource(row.Source, row.ArchiveDbPath, conn =>
            {
                if (false == TableExists(conn, "Substrate"))
                    return;

                bool hasOrigin = GetColumns(conn, "Substrate").Contains("OriginName");
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = BuildSubstrateSelect(hasOrigin, "WHERE CurrentCarrierKey = $key ORDER BY Name");
                    cmd.Parameters.Add("$key", DbType.String).Value = row.UniqueKey;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            result.Add(MapSubstrateFull(reader));
                    }
                }
            });
            return result;
        }

        /// <summary>선택한 캐리어의 랏 이력(LotHistoryEvent, CarrierKey 기준). (같은 소스 DB)</summary>
        public List<HistoryRecord> GetCarrierLotHistory(CarrierSearchRow row)
        {
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return new List<HistoryRecord>();

            return LoadLotHistory(row.Source, row.ArchiveDbPath, "CarrierKey", row.UniqueKey);
        }
        #endregion </캐리어 상세>

        #region <기판 상세>
        public SubstrateItem GetSubstrate(SubstrateSearchRow row)
        {
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return null;

            SubstrateItem dto = null;
            RunOnSource(row.Source, row.ArchiveDbPath, conn =>
            {
                if (false == TableExists(conn, "Substrate"))
                    return;

                bool hasOrigin = GetColumns(conn, "Substrate").Contains("OriginName");
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = BuildSubstrateSelect(hasOrigin, "WHERE UniqueKey = $key");
                    cmd.Parameters.Add("$key", DbType.String).Value = row.UniqueKey;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            dto = MapSubstrateFull(reader);
                    }
                }

                if (dto != null)
                    dto.Extra = LoadExtra(conn, "SubstrateExtra", "SubstrateKey", _substrateExtraKeys, row.UniqueKey);
            });
            return dto;
        }

        /// <summary>선택한 기판의 랏 이력(LotHistoryEvent, SubstrateKey 기준).</summary>
        public List<HistoryRecord> GetSubstrateLotHistory(SubstrateSearchRow row)
        {
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return new List<HistoryRecord>();

            return LoadLotHistory(row.Source, row.ArchiveDbPath, "SubstrateKey", row.UniqueKey);
        }

        /// <summary>선택한 기판의 처리 이력(SubstrateProcessingHistory).</summary>
        public List<SubstrateProcessingHistoryItem> GetProcessingHistory(SubstrateSearchRow row)
        {
            var result = new List<SubstrateProcessingHistoryItem>();
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return result;

            RunOnSource(row.Source, row.ArchiveDbPath, conn =>
            {
                if (false == TableExists(conn, "SubstrateProcessingHistory"))
                    return;

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT SubstrateKey, EventTime, OldState, NewState, ControlJobId, ProcessJobId, LocationId, Description
FROM SubstrateProcessingHistory
WHERE SubstrateKey = $key
ORDER BY EventTime ASC, Id ASC;";
                    cmd.Parameters.Add("$key", DbType.String).Value = row.UniqueKey;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new SubstrateProcessingHistoryItem
                            {
                                SubstrateKey = GetStringOrEmpty(reader, 0),
                                EventTime = ParseIsoTime(GetStringOrEmpty(reader, 1)),
                                OldState = GetStringOrEmpty(reader, 2),
                                NewState = GetStringOrEmpty(reader, 3),
                                ControlJobId = GetStringOrEmpty(reader, 4),
                                ProcessJobId = GetStringOrEmpty(reader, 5),
                                LocationId = GetStringOrEmpty(reader, 6),
                                Description = GetStringOrEmpty(reader, 7)
                            });
                        }
                    }
                }
            });
            return result;
        }

        /// <summary>선택한 기판의 위치 이동 이력(SubstrateLocationHistory).</summary>
        public List<SubstrateLocationChangeItem> GetLocationHistory(SubstrateSearchRow row)
        {
            var result = new List<SubstrateLocationChangeItem>();
            if (row == null || string.IsNullOrEmpty(row.UniqueKey))
                return result;

            RunOnSource(row.Source, row.ArchiveDbPath, conn =>
            {
                if (false == TableExists(conn, "SubstrateLocationHistory"))
                    return;

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT SubstrateKey, FromLocationName, FromLocationKind, ToLocationName, ToLocationKind, ChangeTime, Reason
FROM SubstrateLocationHistory
WHERE SubstrateKey = $key
ORDER BY ChangeTime ASC, Id ASC;";
                    cmd.Parameters.Add("$key", DbType.String).Value = row.UniqueKey;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var key = GetStringOrEmpty(reader, 0);

                            string from = reader.IsDBNull(1) ? null : reader.GetString(1);
                            if (string.IsNullOrWhiteSpace(from)) from = null;
                            var fromKind = EnumPersistence.ParseNameOrDefault(
                                Convert.ToString(reader.GetValue(2)), EFEM.Defines.Common.ModuleType.Unknown);

                            string to = reader.IsDBNull(3) ? null : reader.GetString(3);
                            if (string.IsNullOrWhiteSpace(to)) to = null;
                            var toKind = EnumPersistence.ParseNameOrDefault(
                                Convert.ToString(reader.GetValue(4)), EFEM.Defines.Common.ModuleType.Unknown);

                            var time = ParseIsoTime(GetStringOrEmpty(reader, 5));
                            var reason = GetStringOrEmpty(reader, 6);

                            // from/to 둘 다 비면 DTO 생성자가 예외를 던지므로 방어(견고성).
                            if (from == null && to == null)
                                continue;

                            result.Add(new SubstrateLocationChangeItem(key, from, fromKind, to, toKind, time, reason));
                        }
                    }
                }
            });
            return result;
        }
        #endregion </기판 상세>

        #region <공용 로더>
        private List<HistoryRecord> LoadLotHistory(MaterialSource source, string archivePath, string keyColumn, string keyValue)
        {
            var result = new List<HistoryRecord>();
            RunOnSource(source, archivePath, conn =>
            {
                if (false == TableExists(conn, "LotHistoryEvent"))
                    return;

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT EventTime, Category, PortId, CarrierKey, CarrierId, LotId, " +
                        "SubstrateKey, SubstrateName, CarrierEventCode, SubstrateEventCode, Message " +
                        "FROM LotHistoryEvent WHERE " + keyColumn + " = $key ORDER BY EventTime;";
                    cmd.Parameters.Add("$key", DbType.String).Value = keyValue;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            result.Add(MapLotEvent(reader));
                    }
                }
            });
            return result;
        }

        private static string BuildCarrierSelect(string tail)
        {
            return
                "SELECT UniqueKey, LotId, CarrierId, PortId, AccessStatus, Capacity, LoadTime, UnloadTime " +
                "FROM Carrier " + tail + ";";
        }
        private static CarrierItem MapCarrierBasic(SQLiteDataReader reader)
        {
            return new CarrierItem
            {
                UniqueKey = GetStringOrEmpty(reader, 0),
                LotId = GetStringOrEmpty(reader, 1),
                CarrierId = GetStringOrEmpty(reader, 2),
                PortId = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3)),
                AccessStatus = reader.IsDBNull(4)
                    ? EFEM.Defines.LoadPort.CarrierAccessStates.NotAccessed
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(4)), EFEM.Defines.LoadPort.CarrierAccessStates.InAccessed),
                Capacity = reader.IsDBNull(5) ? 0 : Convert.ToInt32(reader.GetValue(5)),
                LoadTime = GetStringOrEmpty(reader, 6),
                UnloadTime = GetStringOrEmpty(reader, 7),
                SlotMaps = new Dictionary<int, EFEM.Defines.LoadPort.CarrierSlotMapStates>(),
                Extra = new Dictionary<string, string>(StringComparer.Ordinal)
            };
        }
        private static CarrierSearchRow ToSearchRow(CarrierItem item, MaterialSource source, string archivePath)
        {
            return new CarrierSearchRow
            {
                UniqueKey = item.UniqueKey,
                CarrierId = item.CarrierId,
                LotId = item.LotId,
                PortId = item.PortId,
                AccessStatus = item.AccessStatus,
                Capacity = item.Capacity,
                LoadTime = item.LoadTime,
                UnloadTime = item.UnloadTime,
                Source = source,
                ArchiveDbPath = archivePath
            };
        }
        private static SubstrateSearchRow ToSearchRow(SubstrateItem item, MaterialSource source, string archivePath)
        {
            return new SubstrateSearchRow
            {
                UniqueKey = item.UniqueKey,
                Name = item.Name,
                OriginName = item.OriginName,
                LocationId = item.LocationId,
                SourcePortId = item.SourcePortId,
                SourceSlot = item.SourceSlot,
                SourceCarrierId = item.SourceCarrierId,
                CurrentCarrierKey = item.CurrentCarrierKey,
                DestinationPortId = item.DestinationPortId,
                DestinationSlot = item.DestinationSlot,
                LotId = item.LotId,
                RecipeId = item.RecipeId,
                ProcessJobId = item.ProcessJobId,
                ControlJobId = item.ControlJobId,
                TransportStatus = item.TransportStatus,
                ProcessingStatus = item.ProcessingStatus,
                IdReadingStatus = item.IdReadingStatus,
                DoNotProcessFlag = item.DoNotProcessFlag,
                Usage = item.Usage,
                Source = source,
                ArchiveDbPath = archivePath
            };
        }
        private static string BuildSubstrateSelect(bool hasOrigin, string tail)
        {
            string origin = hasOrigin ? "OriginName" : "'' AS OriginName";
            return
                "SELECT UniqueKey, Name, " + origin + ", LocationId, " +
                "SourcePortId, SourceSlot, SourceCarrierId, CurrentCarrierKey, " +
                "DestinationPortId, DestinationSlot, " +
                "LotId, RecipeId, ProcessJobId, ControlJobId, " +
                "TransportStatus, ProcessingStatus, IdReadingStatus, " +
                "DoNotProcessFlag, Usage " +
                "FROM Substrate " + tail + ";";
        }
        private static SubstrateItem MapSubstrateFull(SQLiteDataReader reader)
        {
            return new SubstrateItem
            {
                UniqueKey = GetStringOrEmpty(reader, 0),
                Name = GetStringOrEmpty(reader, 1),
                OriginName = GetStringOrEmpty(reader, 2),
                LocationId = GetStringOrEmpty(reader, 3),
                SourcePortId = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4)),
                SourceSlot = reader.IsDBNull(5) ? 0 : Convert.ToInt32(reader.GetValue(5)),
                SourceCarrierId = GetStringOrEmpty(reader, 6),
                CurrentCarrierKey = GetStringOrEmpty(reader, 7),
                DestinationPortId = reader.IsDBNull(8) ? 0 : Convert.ToInt32(reader.GetValue(8)),
                DestinationSlot = reader.IsDBNull(9) ? 0 : Convert.ToInt32(reader.GetValue(9)),
                LotId = GetStringOrEmpty(reader, 10),
                RecipeId = GetStringOrEmpty(reader, 11),
                ProcessJobId = GetStringOrEmpty(reader, 12),
                ControlJobId = GetStringOrEmpty(reader, 13),
                TransportStatus = reader.IsDBNull(14)
                    ? EFEM.Defines.MaterialTracking.TransportStates.AtSource
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(14)), EFEM.Defines.MaterialTracking.TransportStates.AtSource),
                ProcessingStatus = reader.IsDBNull(15)
                    ? EFEM.Defines.MaterialTracking.ProcessingStates.NeedsProcessing
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(15)), EFEM.Defines.MaterialTracking.ProcessingStates.NeedsProcessing),
                IdReadingStatus = reader.IsDBNull(16)
                    ? EFEM.Defines.MaterialTracking.IdReadingStates.NotConfirmed
                    : EnumPersistence.ParseNameOrDefault(Convert.ToString(reader.GetValue(16)), EFEM.Defines.MaterialTracking.IdReadingStates.NotConfirmed),
                DoNotProcessFlag = DbUtil.IntToBool(reader.IsDBNull(17) ? 0L : Convert.ToInt64(reader.GetValue(17))),
                Usage = DbUtil.IntToBool(reader.IsDBNull(18) ? 0L : Convert.ToInt64(reader.GetValue(18))),
                Extra = new Dictionary<string, string>(StringComparer.Ordinal)
            };
        }
        private static HistoryRecord MapLotEvent(SQLiteDataReader reader)
        {
            return new HistoryRecord
            {
                Time = ParseLotEventTime(GetStringOrEmpty(reader, 0)),
                Category = GetStringOrEmpty(reader, 1),
                PortId = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)),
                CarrierKey = GetStringOrEmpty(reader, 3),
                CarrierId = GetStringOrEmpty(reader, 4),
                LotId = GetStringOrEmpty(reader, 5),
                SubstrateKey = GetStringOrEmpty(reader, 6),
                SubstrateName = GetStringOrEmpty(reader, 7),
                CarrierEventCode = GetStringOrEmpty(reader, 8),
                SubstrateEventCode = GetStringOrEmpty(reader, 9),
                Message = GetStringOrEmpty(reader, 10)
            };
        }

        /// <summary>Extra 와이드 테이블 로드. archive 구버전 파일에 없는 컬럼은 건너뛴다(PRAGMA로 존재 확인).</summary>
        private static Dictionary<string, string> LoadExtra(SQLiteConnection conn, string table, string keyColumn, string[] extraKeys, string keyValue)
        {
            var dict = new Dictionary<string, string>(StringComparer.Ordinal);
            if (false == TableExists(conn, table) || extraKeys.Length == 0)
                return dict;

            var existing = GetColumns(conn, table);
            var cols = new List<string>();
            for (int i = 0; i < extraKeys.Length; ++i)
            {
                if (existing.Contains(extraKeys[i]))
                    cols.Add(extraKeys[i]);
            }
            if (cols.Count == 0)
                return dict;

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT " + string.Join(", ", cols) + " FROM " + table + " WHERE " + keyColumn + " = $key;";
                cmd.Parameters.Add("$key", DbType.String).Value = keyValue;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < cols.Count; ++i)
                            dict[cols[i]] = reader.IsDBNull(i) ? string.Empty : reader.GetString(i);
                    }
                }
            }
            return dict;
        }
        private static Dictionary<int, EFEM.Defines.LoadPort.CarrierSlotMapStates> LoadCarrierSlotMap(SQLiteConnection conn, string key)
        {
            var dict = new Dictionary<int, EFEM.Defines.LoadPort.CarrierSlotMapStates>();
            if (false == TableExists(conn, "CarrierSlotMap"))
                return dict;

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT SlotNo, MapValue FROM CarrierSlotMap WHERE CarrierKey = $key ORDER BY SlotNo;";
                cmd.Parameters.Add("$key", DbType.String).Value = key;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var slot = Convert.ToInt32(reader.GetValue(0));
                        var mv = EnumPersistence.ParseNameOrDefault(
                            Convert.ToString(reader.GetValue(1)),
                            EFEM.Defines.LoadPort.CarrierSlotMapStates.Undefined);
                        dict[slot] = mv;
                    }
                }
            }
            return dict;
        }
        #endregion </공용 로더>

        #region <인프라>
        /// <summary>소스별 커넥션을 열어 body 를 실행한다. main=OpenConnection(WAL), archive=read-only 직접 커넥션. 실패는 삼킨다.</summary>
        private void RunOnSource(MaterialSource source, string archivePath, Action<SQLiteConnection> body)
        {
            try
            {
                if (source == MaterialSource.Main)
                {
                    using (var conn = _db.OpenConnection())
                        body(conn);
                }
                else
                {
                    if (string.IsNullOrEmpty(archivePath) || false == File.Exists(archivePath))
                        return;

                    using (var conn = new SQLiteConnection(string.Format("Data Source={0};Read Only=True;FailIfMissing=True;", archivePath)))
                    {
                        conn.Open();
                        body(conn);
                    }
                }
            }
            catch
            {
                // 조회 실패는 빈/부분 결과로 처리 (조회 UI 견고성)
            }
        }
        private static bool TableExists(SQLiteConnection conn, string tableName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = $name LIMIT 1;";
                cmd.Parameters.Add("$name", DbType.String).Value = tableName;
                var r = cmd.ExecuteScalar();
                return r != null && r != DBNull.Value;
            }
        }
        private static HashSet<string> GetColumns(SQLiteConnection conn, string tableName)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(\"" + tableName.Replace("\"", "\"\"") + "\");";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = Convert.ToString(reader["name"]);
                        if (false == string.IsNullOrEmpty(name))
                            set.Add(name);
                    }
                }
            }
            return set;
        }
        /// <summary>
        /// "그 외 조건"(사용자가 SelectionList 로 고른 기본속성/Extra 속성 필드 + 값)을 where 절에 추가한다.
        /// 필드명은 baseFieldNames/extraKeys 화이트리스트에 있는 것만 허용(식별자 정규식 검증 포함, SQL 인젝션 방지).
        /// Extra 속성은 CarrierExtra/SubstrateExtra 서브쿼리로 필터링하며, 해당 커넥션에 그 컬럼이 없으면
        /// (구버전 archive 등) 매칭 불가로 간주해 이 커넥션 검색 자체를 건너뛴다(빈 결과가 더 정확함).
        /// 반환값 false = 이 커넥션에서 검색을 중단해야 함(호출측이 즉시 return).
        /// </summary>
        private static bool TryAddOtherCondition(
            SQLiteConnection conn, SQLiteCommand cmd, List<string> where,
            string extraTableName, string extraKeyColumn,
            string[] baseFieldNames, string[] extraKeys,
            string otherFieldName, string otherFieldValue, bool exactMatch)
        {
            if (string.IsNullOrWhiteSpace(otherFieldName) || string.IsNullOrWhiteSpace(otherFieldValue))
                return true;

            if (false == IdentifierRegex.IsMatch(otherFieldName))
                return false;

            bool isBase = Array.IndexOf(baseFieldNames, otherFieldName) >= 0;
            bool isExtra = false == isBase && extraKeys != null && Array.IndexOf(extraKeys, otherFieldName) >= 0;

            if (false == isBase && false == isExtra)
                return false;

            string op = exactMatch ? "=" : "LIKE";
            string val = exactMatch ? otherFieldValue : Like(otherFieldValue);

            if (isBase)
            {
                where.Add("\"" + otherFieldName + "\" " + op + " $otherVal");
                cmd.Parameters.Add("$otherVal", DbType.String).Value = val;
                return true;
            }

            if (false == TableExists(conn, extraTableName))
                return false;
            if (false == GetColumns(conn, extraTableName).Contains(otherFieldName))
                return false;

            where.Add("UniqueKey IN (SELECT " + extraKeyColumn + " FROM " + extraTableName +
                " WHERE \"" + otherFieldName + "\" " + op + " $otherVal)");
            cmd.Parameters.Add("$otherVal", DbType.String).Value = val;
            return true;
        }
        private static string GetStringOrEmpty(IDataRecord reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal));
        }
        private static string Like(string value)
        {
            return "%" + value.Trim() + "%";
        }
        /// <summary>LotHistoryEvent.EventTime(로컬 "yyyy-MM-dd HH:mm:ss.fff") 파싱.</summary>
        private static DateTime ParseLotEventTime(string text)
        {
            if (string.IsNullOrEmpty(text))
                return DateTime.MinValue;
            if (DateTime.TryParseExact(text, LotEventTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;
            DateTime.TryParse(text, out result);
            return result;
        }
        /// <summary>처리/위치 이력 시각(DbUtil.ToIsoString = UTC round-trip) 파싱. 표시측에서 ToLocalTime 권장.</summary>
        private static DateTime ParseIsoTime(string text)
        {
            if (string.IsNullOrEmpty(text))
                return DateTime.MinValue;
            try { return DbUtil.FromIsoString(text); }
            catch
            {
                DateTime.TryParse(text, out DateTime result);
                return result;
            }
        }
        #endregion </인프라>
    }
}
