using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;

using EFEM.Database;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] SQLite 기반 이력 조회.
    /// - main LotHistoryEvent : 아직 제거되지 않은 캐리어의 이벤트 (선택 날짜의 이벤트만 필터)
    /// - 일자별 archive DB    : 캐리어 제거 시 이동된 이벤트 (파일 자체가 날짜 버킷이므로 전체 사용)
    /// 두 스코프를 합쳐 파일 조회(백업 날짜 폴더)와 같은 화면 의미를 만든다.
    /// main 읽기는 ExecuteReadAsync(별도 커넥션, WAL), archive 일자 파일은 read-only 직접 커넥션.
    /// 과거 archive 파일에 LotHistoryEvent 테이블이 없을 수 있으므로(기능 도입 전 생성분) archive 조회는 실패 시 빈 결과.
    /// </summary>
    public sealed class SqliteHistoryQuery : IHistoryQuery
    {
        #region <Constructors>
        public SqliteHistoryQuery(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("db");
        }
        #endregion </Constructors>

        #region <Fields>
        private const string EventTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        private readonly MaterialDbContext _db;
        #endregion </Fields>

        #region <IHistoryQuery>
        public List<LotSummary> GetLots(DateTime date, string category)
        {
            // LotId -> 최초 이벤트 시각 (main/archive 병합 시 이른 쪽 우선)
            var merged = new Dictionary<string, DateTime>();

            const string sql = @"
SELECT LotId, MIN(EventTime)
FROM LotHistoryEvent
WHERE LotId <> '' {0}
GROUP BY LotId
HAVING SUM(CASE WHEN Category = $category THEN 1 ELSE 0 END) > 0;
";
            Action<SQLiteDataReader> collect = reader =>
            {
                while (reader.Read())
                {
                    var lotId = reader.GetString(0);
                    var created = ParseEventTime(reader.GetString(1));
                    if (false == merged.TryGetValue(lotId, out DateTime exist) || created < exist)
                        merged[lotId] = created;
                }
            };

            QueryMain(string.Format(sql, "AND EventTime >= $start AND EventTime < $end"), cmd =>
            {
                AddDateRangeParameters(cmd, date);
                cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
            }, collect);

            QueryArchive(date, string.Format(sql, string.Empty), cmd =>
            {
                cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
            }, collect);

            return merged
                .Select(kv => new LotSummary { LotId = kv.Key, CreatedTime = kv.Value })
                .OrderBy(x => x.CreatedTime)
                .ToList();
        }
        public List<HistoryRecord> GetLotHistory(DateTime date, string category, string lotId)
        {
            var result = new List<HistoryRecord>();

            const string sql = @"
SELECT EventTime, Category, PortId, CarrierKey, CarrierId, LotId,
       SubstrateKey, SubstrateName, CarrierEventCode, SubstrateEventCode, Message
FROM LotHistoryEvent
WHERE LotId = $lotId {0}
ORDER BY EventTime;
";
            Action<SQLiteDataReader> collect = reader =>
            {
                while (reader.Read())
                    result.Add(MapRecord(reader));
            };

            QueryMain(string.Format(sql, "AND EventTime >= $start AND EventTime < $end"), cmd =>
            {
                AddDateRangeParameters(cmd, date);
                cmd.Parameters.Add("$lotId", DbType.String).Value = lotId ?? string.Empty;
            }, collect);

            QueryArchive(date, string.Format(sql, string.Empty), cmd =>
            {
                cmd.Parameters.Add("$lotId", DbType.String).Value = lotId ?? string.Empty;
            }, collect);

            return result.OrderBy(x => x.Time).ToList();
        }
        public List<HistoryRecord> GetWorkingCarrierHistory(int portId, string carrierId)
        {
            // 작업 중 = 아직 캐리어가 제거되지 않음 = main에만 존재
            var result = new List<HistoryRecord>();
            QueryMain(@"
SELECT EventTime, Category, PortId, CarrierKey, CarrierId, LotId,
       SubstrateKey, SubstrateName, CarrierEventCode, SubstrateEventCode, Message
FROM LotHistoryEvent
WHERE CarrierId = $carrierId AND PortId = $portId AND CarrierKey <> ''
ORDER BY EventTime;
", cmd =>
            {
                cmd.Parameters.Add("$carrierId", DbType.String).Value = carrierId ?? string.Empty;
                cmd.Parameters.Add("$portId", DbType.Int32).Value = portId;
            }, reader =>
            {
                while (reader.Read())
                    result.Add(MapRecord(reader));
            });
            return result;
        }
        public List<HistoryRecord> GetWorkingSubstrateHistory(string substrateName, string category)
        {
            var result = new List<HistoryRecord>();
            QueryMain(@"
SELECT EventTime, Category, PortId, CarrierKey, CarrierId, LotId,
       SubstrateKey, SubstrateName, CarrierEventCode, SubstrateEventCode, Message
FROM LotHistoryEvent
WHERE SubstrateName = $substrateName AND Category = $category
ORDER BY EventTime;
", cmd =>
            {
                cmd.Parameters.Add("$substrateName", DbType.String).Value = substrateName ?? string.Empty;
                cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
            }, reader =>
            {
                while (reader.Read())
                    result.Add(MapRecord(reader));
            });
            return result;
        }
        public Dictionary<string, List<string>> GetLotSubstrates(DateTime date, string category)
        {
            var result = new Dictionary<string, List<string>>();

            const string sql = @"
SELECT DISTINCT LotId, SubstrateName
FROM LotHistoryEvent
WHERE LotId <> '' AND SubstrateName <> '' AND Category = $category {0};
";
            Action<SQLiteDataReader> collect = reader =>
            {
                while (reader.Read())
                {
                    var lotId = reader.GetString(0);
                    var name = reader.GetString(1);
                    if (false == result.TryGetValue(lotId, out List<string> names))
                    {
                        names = new List<string>();
                        result[lotId] = names;
                    }
                    if (false == names.Contains(name))
                        names.Add(name);
                }
            };

            QueryMain(string.Format(sql, "AND EventTime >= $start AND EventTime < $end"), cmd =>
            {
                AddDateRangeParameters(cmd, date);
                cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
            }, collect);

            QueryArchive(date, string.Format(sql, string.Empty), cmd =>
            {
                cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
            }, collect);

            return result;
        }
        #endregion </IHistoryQuery>

        #region <Internal>
        private void QueryMain(string sql, Action<SQLiteCommand> bind, Action<SQLiteDataReader> collect)
        {
            try
            {
                _db.ExecuteReadAsync<bool>(async conn =>
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        bind(cmd);
                        using (var reader = (SQLiteDataReader)await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            collect(reader);
                        }
                    }
                    return true;
                }).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // 조회 실패는 빈 결과로 처리 (조회 UI 견고성)
            }
        }
        private void QueryArchive(DateTime date, string sql, Action<SQLiteCommand> bind, Action<SQLiteDataReader> collect)
        {
            try
            {
                string archiveDbPath = string.Format(@"{0}\Archive\{1:0000}{2:00}{3:00}.db", _db.DataBasePath, date.Year, date.Month, date.Day);
                if (false == File.Exists(archiveDbPath))
                    return;

                using (var conn = new SQLiteConnection(string.Format("Data Source={0};Read Only=True;FailIfMissing=True;", archiveDbPath)))
                {
                    conn.Open();

                    // 기능 도입 전에 생성된 archive 파일(자재만 아카이브됨)에는 LotHistoryEvent 테이블이 없다.
                    // 예외로 처리하면 디버거/SQLite 오류 로그에 계속 찍히므로 테이블 존재를 먼저 확인한다.
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'LotHistoryEvent';";
                        if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
                            return;
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        bind(cmd);
                        using (var reader = cmd.ExecuteReader())
                        {
                            collect((SQLiteDataReader)reader);
                        }
                    }
                }
            }
            catch
            {
                // 조회 실패는 빈 결과로 처리 (조회 UI 견고성)
            }
        }
        private static void AddDateRangeParameters(SQLiteCommand cmd, DateTime date)
        {
            cmd.Parameters.Add("$start", DbType.String).Value = date.Date.ToString(EventTimeFormat);
            cmd.Parameters.Add("$end", DbType.String).Value = date.Date.AddDays(1).ToString(EventTimeFormat);
        }
        private static HistoryRecord MapRecord(SQLiteDataReader reader)
        {
            return new HistoryRecord
            {
                Time = ParseEventTime(reader.GetString(0)),
                Category = reader.GetString(1),
                PortId = reader.GetInt32(2),
                CarrierKey = reader.GetString(3),
                CarrierId = reader.GetString(4),
                LotId = reader.GetString(5),
                SubstrateKey = reader.GetString(6),
                SubstrateName = reader.GetString(7),
                CarrierEventCode = reader.GetString(8),
                SubstrateEventCode = reader.GetString(9),
                Message = reader.GetString(10),
            };
        }
        private static DateTime ParseEventTime(string text)
        {
            if (DateTime.TryParseExact(text, EventTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            DateTime.TryParse(text, out result);
            return result;
        }
        #endregion </Internal>
    }
}
