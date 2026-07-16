using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using EFEM.Database;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] SQLite 기반 이력 저장소.
    /// MaterialDbContext(자재 추적 DB)를 공용으로 사용하며 LotHistoryEvent 테이블에 기록한다.
    ///
    /// 설계 원칙:
    /// - 모든 쓰기는 ExecuteWriteAsync(QueueOnly) — 엔진 펌핑 루프(장비 스캔과 연동)를 블로킹하지 않는다.
    ///   파일 저장소가 주(primary)이고 이 저장소는 병행(best-effort)이므로 DB 실패는 LogJobFailure 로깅에 맡긴다.
    /// - CarrierKey/SubstrateKey는 Carrier/Substrate.UniqueKey와 논리 조인되는 키지만 FK 제약은 없다.
    ///   (자재 행의 archive 이동/삭제와 이력 수명주기 분리)
    /// - 캐리어 수명주기 연동: 캐리어 제거 시 SqliteCarrierStorage.PrepareToArchiveAsync가
    ///   해당 CarrierKey의 이력 행을 일자별 archive DB로 함께 이동한다.
    /// - 파일 저장소 실패로 엔진이 명령을 재시도해도 중복이 없도록 INSERT OR IGNORE + 자연 키 UNIQUE 인덱스 사용.
    /// </summary>
    public sealed class SqliteHistoryStore : IHistoryStore
    {
        #region <Constructors>
        public SqliteHistoryStore(MaterialDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("db");
        }
        #endregion </Constructors>

        #region <Fields>
        private const string EventTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private const int OrphanSweepDays = 7;

        private readonly MaterialDbContext _db;
        #endregion </Fields>

        #region <IHistoryStore>
        public void RegisterCarrierDirectory(int portId, string name)
        {
            // 파일 저장소 전용 개념(포트 작업 폴더) — DB는 준비할 채널이 없다.
        }
        public void AppendCarrierEvent(HistoryRecord record)
        {
            InsertEvent(record);
        }
        public void AppendSubstrateEvent(HistoryRecord record)
        {
            InsertEvent(record);
        }
        public void AppendSubstrateEventWithCarrier(HistoryRecord record)
        {
            // 파일 저장소는 기판/캐리어 2개 파일에 쓰지만, DB는 캐리어 컬럼이 채워진 행 하나로 표현된다.
            InsertEvent(record);
        }
        public void BindSubstrateToCarrier(DateTime time, int portId, string carrierKey, string carrierId, string substrateKey, string substrateName, string category)
        {
            _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    // 미귀속(CarrierKey='') 행만 귀속시킨다. 이미 다른 캐리어에 귀속된 행은 유지.
                    // SubstrateKey가 있으면 키로(개명 무관), 키가 빈 행은 이름+분류 폴백으로 매칭.
                    cmd.CommandText = @"
UPDATE LotHistoryEvent
SET CarrierKey = $carrierKey,
    CarrierId  = $carrierId,
    PortId     = $portId
WHERE CarrierKey = ''
  AND (($substrateKey <> '' AND SubstrateKey = $substrateKey)
    OR (SubstrateKey = '' AND SubstrateName = $substrateName AND Category = $category));
";
                    cmd.Parameters.Add("$carrierKey", DbType.String).Value = carrierKey ?? string.Empty;
                    cmd.Parameters.Add("$carrierId", DbType.String).Value = carrierId ?? string.Empty;
                    cmd.Parameters.Add("$portId", DbType.Int32).Value = portId;
                    cmd.Parameters.Add("$substrateKey", DbType.String).Value = substrateKey ?? string.Empty;
                    cmd.Parameters.Add("$substrateName", DbType.String).Value = substrateName ?? string.Empty;
                    cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
        public void RenameSubstrate(DateTime time, string substrateKey, string oldName, string newName, string category)
        {
            _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    // 파일 저장소의 파일 개명(과거 라인이 새 이름 파일로 모임)과 등가:
                    // 키 일치 행 + (키 빈 값 && 구 이름 && 미귀속) 행의 표시 이름을 새 이름으로 갱신해서
                    // 이후 BindSubstrateToCarrier의 이름 폴백이 새 이름으로 매칭되게 한다.
                    cmd.CommandText = @"
UPDATE LotHistoryEvent
SET SubstrateName = $newName
WHERE ($substrateKey <> '' AND SubstrateKey = $substrateKey)
   OR (SubstrateKey = '' AND SubstrateName = $oldName AND CarrierKey = '' AND Category = $category);
";
                    cmd.Parameters.Add("$newName", DbType.String).Value = newName ?? string.Empty;
                    cmd.Parameters.Add("$substrateKey", DbType.String).Value = substrateKey ?? string.Empty;
                    cmd.Parameters.Add("$oldName", DbType.String).Value = oldName ?? string.Empty;
                    cmd.Parameters.Add("$category", DbType.String).Value = category ?? string.Empty;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
        public void CompleteCarrier(DateTime time, int portId, string carrierKey, string carrierId, string lotId, List<string> substrateNames, string category)
        {
            // 행 이동은 캐리어 제거 시(SqliteCarrierStorage) 수행되므로 여기서는 랏 확정만 한다.
            _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
UPDATE LotHistoryEvent
SET LotId = $lotId
WHERE CarrierKey = $carrierKey
  AND CarrierKey <> ''
  AND LotId = '';
";
                    cmd.Parameters.Add("$lotId", DbType.String).Value = lotId ?? string.Empty;
                    cmd.Parameters.Add("$carrierKey", DbType.String).Value = carrierKey ?? string.Empty;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
        public void ClearPrevious(DateTime time, int portId, string carrierId, string loadPortName)
        {
            // 파일 저장소 전용 개념(포트 폴더의 이전 캐리어 잔여 파일 정리).
            // DB 행은 캐리어 단위로 귀속/이동되므로 정리할 잔여물이 없다.
        }
        /// <summary>
        /// 캐리어에 귀속되지 못한 채 오래 남은 이력 행을 당일 archive DB로 이동한다.
        /// (파일 저장소의 고아 기판 이력 스윕과 동일 정책)
        /// </summary>
        public void SweepOrphans()
        {
            DateTime now = DateTime.Now;
            string archiveDbPath = string.Format(@"{0}\Archive\{1:0000}{2:00}{3:00}.db", _db.DataBasePath, now.Year, now.Month, now.Day);
            string archiveDir = Path.GetDirectoryName(archiveDbPath);
            if (false == Directory.Exists(archiveDir))
                Directory.CreateDirectory(archiveDir);

            string cutoff = now.AddDays(-OrphanSweepDays).ToString(EventTimeFormat);

            // ATTACH/이동은 한 트랜잭션 작업으로, DETACH는 별도 작업으로 — SqliteCarrierStorage의 아카이브 관용구를 따른다.
            _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                var escapedPath = archiveDbPath.Replace("'", "''");
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = string.Format("ATTACH DATABASE '{0}' AS archive;", escapedPath);
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                _db.EnsureArchiveSchemaAndMigrate(conn, tx);

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = _db.GetArchiveCommand();
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO archive.LotHistoryEvent (
    EventTime, Category, PortId,
    CarrierKey, CarrierId, LotId,
    SubstrateKey, SubstrateName,
    CarrierEventCode, SubstrateEventCode, Message
)
SELECT
    EventTime, Category, PortId,
    CarrierKey, CarrierId, LotId,
    SubstrateKey, SubstrateName,
    CarrierEventCode, SubstrateEventCode, Message
FROM LotHistoryEvent
WHERE CarrierKey = '' AND EventTime < $cutoff;
";
                    cmd.Parameters.Add("$cutoff", DbType.String).Value = cutoff;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = "DELETE FROM LotHistoryEvent WHERE CarrierKey = '' AND EventTime < $cutoff;";
                    cmd.Parameters.Add("$cutoff", DbType.String).Value = cutoff;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });

            _db.ExecuteWriteAsync(async (conn) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DETACH DATABASE archive;";
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
        public void WriteDiagnostic(string message)
        {
            // 진단은 합성 저장소(ParallelHistoryStore)가 파일 저장소로 라우팅한다.
        }
        #endregion </IHistoryStore>

        #region <Internal>
        private void InsertEvent(HistoryRecord record)
        {
            _db.ExecuteWriteAsync(async (conn, tx) =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT OR IGNORE INTO LotHistoryEvent (
    EventTime, Category, PortId,
    CarrierKey, CarrierId, LotId,
    SubstrateKey, SubstrateName,
    CarrierEventCode, SubstrateEventCode, Message
)
VALUES (
    $eventTime, $category, $portId,
    $carrierKey, $carrierId, '',
    $substrateKey, $substrateName,
    $carrierEventCode, $substrateEventCode, $message
);
";
                    cmd.Parameters.Add("$eventTime", DbType.String).Value = record.Time.ToString(EventTimeFormat);
                    cmd.Parameters.Add("$category", DbType.String).Value = record.Category ?? string.Empty;
                    cmd.Parameters.Add("$portId", DbType.Int32).Value = record.PortId;
                    cmd.Parameters.Add("$carrierKey", DbType.String).Value = record.CarrierKey ?? string.Empty;
                    cmd.Parameters.Add("$carrierId", DbType.String).Value = record.CarrierId ?? string.Empty;
                    cmd.Parameters.Add("$substrateKey", DbType.String).Value = record.SubstrateKey ?? string.Empty;
                    cmd.Parameters.Add("$substrateName", DbType.String).Value = record.SubstrateName ?? string.Empty;
                    cmd.Parameters.Add("$carrierEventCode", DbType.String).Value = record.CarrierEventCode ?? string.Empty;
                    cmd.Parameters.Add("$substrateEventCode", DbType.String).Value = record.SubstrateEventCode ?? string.Empty;
                    cmd.Parameters.Add("$message", DbType.String).Value = record.Message ?? string.Empty;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
        #endregion </Internal>
    }
}
