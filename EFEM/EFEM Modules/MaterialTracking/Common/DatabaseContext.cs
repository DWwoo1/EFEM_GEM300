using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Data.SQLite;

namespace EFEM.Database
{
    public interface IDbMigrationStep
    {
        int Version { get; }
        string Name { get; }
        void Apply(SQLiteConnection conn, SQLiteTransaction tx, string schemaName);
    }

    public sealed class DelegateDbMigrationStep : IDbMigrationStep
    {
        public int Version { get; }
        public string Name { get; }
        private readonly Action<SQLiteConnection, SQLiteTransaction, string> _apply;

        public DelegateDbMigrationStep(
            int version,
            string name,
            Action<SQLiteConnection, SQLiteTransaction, string> apply)
        {
            Version = version;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _apply = apply ?? throw new ArgumentNullException(nameof(apply));
        }

        public void Apply(SQLiteConnection conn, SQLiteTransaction tx, string schemaName)
        {
            _apply(conn, tx, schemaName);
        }
    }

    /*
     * SELECT Substrate.UniqueKey
   FROM Substrate
   JOIN Carrier ON Carrier.UniqueKey = Substrate.CurrentCarrierKey
   WHERE Carrier.CarrierId = 'CARRIER04'
   --JOIN SubstrateExtra ON SubstrateExtra.SubstrateKey = Substrate.UniqueKey 
   --WHERE SubstrateExtra.ParentLotId = 'P5D383' AND SubstrateExtra.IsLastSubstrate = 'True';
     */
    public enum WriteExecutionMode
    {
        WaitForCompletion,
        QueueOnly
    }
    // 주의: 이 enum은 stale/신뢰 금지. 실제 마이그레이션 버전 번호는 MigrationSteps.GetMigrationSteps()가 소유하며
    // 과거 재배정 이력이 있다(초기 6.x에서 v2 = KeyLotQty->LotQty 였으나, 현재 v2 = Location.Name->Id, KeyLotQty->LotQty는 v4).
    // 그래서 마이그레이션 적용은 SchemaVersion 카운터가 아니라 각 스텝의 실제 스키마 검사(TableExists/ColumnExists)로 판단한다.
    public enum DataBaseVersion
    {
        BaseSchema = 1,
        CarrierExtraChanged = 2,    // (레거시 표기) 초기 6.x에서만 v2 == KeyLotQty->LotQty. 현재 의미와 불일치하므로 참조 금지.
    }
    public sealed class MaterialDbContext : IDisposable
    {
        public sealed class DbCommandLogEntry
        {
            public string CommandText { get; set; }
            public Dictionary<string, object> Parameters { get; set; }
        }
        private sealed class WriteJobWithoutTransaction
        {
            public bool UseTransaction { get; set; }
            public WriteExecutionMode Mode { get; }
            public Func<SQLiteConnection, Task> Work { get; }
            public TaskCompletionSource<bool> Tcs { get; }
            public List<DbCommandLogEntry> LoggedCommands { get; }

            public WriteJobWithoutTransaction(Func<SQLiteConnection, Task> work, WriteExecutionMode mode)
            {
                Mode = mode;
                Work = work ?? throw new ArgumentNullException(nameof(work));
                Tcs = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                LoggedCommands = new List<DbCommandLogEntry>();
            }
        }
        private sealed class WriteJob
        {
            public bool UseTransaction { get; set; }
            public WriteExecutionMode Mode { get; }
            public Func<SQLiteConnection, SQLiteTransaction, Task> Work { get; }
            public TaskCompletionSource<bool> Tcs { get; }
            public List<DbCommandLogEntry> LoggedCommands { get; }

            public WriteJob(Func<SQLiteConnection, SQLiteTransaction, Task> work, WriteExecutionMode mode)
            {
                Mode = mode;
                Work = work ?? throw new ArgumentNullException(nameof(work));
                Tcs = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                LoggedCommands = new List<DbCommandLogEntry>();
            }
        }

        internal static class ExtraSchemaCreator
        {
            private static readonly Regex ColumnNameRegex =
                new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$", RegexOptions.Compiled);
            private static string ValidateIdentifier(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Column name is required.", nameof(name));

                if (!ColumnNameRegex.IsMatch(name))
                    throw new ArgumentException($"Invalid column name for SQL identifier: '{name}'", nameof(name));

                return name;
            }
            public static string GetCarrierExtraTableCommand(IEnumerable<string> extraKeys)
            {
                if (extraKeys == null)
                    throw new ArgumentNullException(nameof(extraKeys));

                var cols = extraKeys
                    .Select(ValidateIdentifier)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();

                var sb = new StringBuilder();
                sb.AppendLine("-- Carrier Extra (wide columns)");
                sb.AppendLine("CREATE TABLE IF NOT EXISTS CarrierExtra (");
                sb.AppendLine("    CarrierKey TEXT NOT NULL,");

                foreach (var col in cols)
                {
                    sb.Append("    ");
                    sb.Append(col);
                    sb.AppendLine(" TEXT,");
                }

                sb.AppendLine("    PRIMARY KEY (CarrierKey),");
                sb.AppendLine("    FOREIGN KEY (CarrierKey)");
                sb.AppendLine("        REFERENCES Carrier(UniqueKey)");
                sb.AppendLine("        ON DELETE CASCADE");
                sb.AppendLine(");");

                return sb.ToString();
            }
            public static string GetSubstrateExtraTableCommand(IEnumerable<string> extraKeys)
            {
                if (extraKeys == null)
                    throw new ArgumentNullException(nameof(extraKeys));

                var cols = extraKeys
                    .Select(ValidateIdentifier)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();

                var sb = new StringBuilder();
                sb.AppendLine("-- Substrate Extra (wide columns)");
                sb.AppendLine("CREATE TABLE IF NOT EXISTS SubstrateExtra (");
                sb.AppendLine("    SubstrateKey TEXT NOT NULL,");

                foreach (var col in cols)
                {
                    sb.Append("    ");
                    sb.Append(col);
                    sb.AppendLine(" TEXT,");
                }

                sb.AppendLine("    PRIMARY KEY (SubstrateKey),");
                sb.AppendLine("    FOREIGN KEY (SubstrateKey)");
                sb.AppendLine("        REFERENCES Substrate(UniqueKey)");
                sb.AppendLine("        ON DELETE CASCADE");
                sb.AppendLine(");");

                return sb.ToString();
            }
            public static string GetArchiveCarrierExtraTableCommand(IEnumerable<string> extraKeys)
            {
                if (extraKeys == null)
                    throw new ArgumentNullException(nameof(extraKeys));

                var cols = extraKeys
                    .Select(ValidateIdentifier)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();

                var sb = new StringBuilder();
                sb.AppendLine("CREATE TABLE IF NOT EXISTS archive.CarrierExtra (");
                sb.AppendLine("    CarrierKey TEXT NOT NULL,");

                foreach (var col in cols)
                {
                    sb.Append("    ");
                    sb.Append(col);
                    sb.AppendLine(" TEXT,");
                }

                sb.AppendLine("    PRIMARY KEY (CarrierKey)");
                sb.AppendLine(");");

                return sb.ToString();
            }
            public static string GetArchiveSubstrateExtraTableCommand(IEnumerable<string> extraKeys)
            {
                if (extraKeys == null)
                    throw new ArgumentNullException(nameof(extraKeys));

                var cols = extraKeys
                    .Select(ValidateIdentifier)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();

                var sb = new StringBuilder();
                sb.AppendLine("CREATE TABLE IF NOT EXISTS archive.SubstrateExtra (");
                sb.AppendLine("    SubstrateKey TEXT NOT NULL,");

                foreach (var col in cols)
                {
                    sb.Append("    ");
                    sb.Append(col);
                    sb.AppendLine(" TEXT,");
                }

                sb.AppendLine("    PRIMARY KEY (SubstrateKey)");
                sb.AppendLine(");");

                return sb.ToString();
            }
        }
        private static class MaterialSchemaSql
        {
            // SchemaVersion
            const string SchemaVersion = @"
-- 스키마 버전
CREATE TABLE IF NOT EXISTS SchemaVersion (
    Version INTEGER NOT NULL
);

INSERT INTO SchemaVersion (Version)
SELECT 1
WHERE NOT EXISTS (SELECT 1 FROM SchemaVersion);
";

            // Location
            const string LocationTable = @"
-- Location
CREATE TABLE IF NOT EXISTS Location (
    Name         TEXT PRIMARY KEY,
    LocationKind TEXT NOT NULL,
    Capacity     INTEGER NOT NULL
                 CHECK (Capacity >= 0)
);
";

            // 3) Carrier Tables
            const string CarrierMainTable = @"
-- Carrier
CREATE TABLE IF NOT EXISTS Carrier (
    UniqueKey     TEXT PRIMARY KEY,
    LotId         TEXT,
    CarrierId     TEXT,
    PortId        INTEGER,
    AccessStatus  TEXT NOT NULL DEFAULT 'NotAccessed',
    Capacity      INTEGER NOT NULL DEFAULT 0,
    LoadTime      TEXT,
    UnloadTime    TEXT
);
";
            const string CarrierSlotMapTable = @"
CREATE TABLE IF NOT EXISTS CarrierSlotMap (
    CarrierKey  TEXT    NOT NULL,
    SlotNo      INTEGER NOT NULL,
    MapValue    TEXT NOT NULL,
    PRIMARY KEY (CarrierKey, SlotNo),
    FOREIGN KEY (CarrierKey)
        REFERENCES Carrier(UniqueKey)
        ON DELETE CASCADE
);
";
            //            const string CarrierExtraTable = @"
            //CREATE TABLE IF NOT EXISTS CarrierExtra (
            //    CarrierKey  TEXT NOT NULL,
            //    ExtraKey    TEXT NOT NULL,
            //    ExtraValue  TEXT,
            //    PRIMARY KEY (CarrierKey, ExtraKey),
            //    FOREIGN KEY (CarrierKey)
            //        REFERENCES Carrier(UniqueKey)
            //        ON DELETE CASCADE
            //);
            //";
            // Substrate Tables
            const string SubstrateMainTable = @"
-- Substrate
CREATE TABLE IF NOT EXISTS Substrate (
    UniqueKey           TEXT PRIMARY KEY,
    Name                TEXT,
    OriginName          TEXT,
    LocationId          TEXT,
    SourcePortId        INTEGER,
    SourceSlot          INTEGER,
    SourceCarrierId     TEXT,
    CurrentCarrierKey   TEXT,    -- ★ 지금 올라가 있는 캐리어
    DestinationPortId   INTEGER,
    DestinationSlot     INTEGER,
    LotId               TEXT,
    RecipeId            TEXT,
    ProcessJobId        TEXT,
    ControlJobId        TEXT,
    TransportStatus     TEXT,
    ProcessingStatus    TEXT,
    IdReadingStatus     TEXT,
    DoNotProcessFlag    INTEGER NOT NULL DEFAULT 0,
    Usage               INTEGER NOT NULL DEFAULT 0,

    FOREIGN KEY (LocationId)
        REFERENCES Location(Name),

    -- 현재 캐리어에 올려져 있는 자재만 Carrier 삭제 시 CASCADE
    FOREIGN KEY (CurrentCarrierKey)
        REFERENCES Carrier(UniqueKey)
        ON DELETE CASCADE
);
";
            //            const string SubstrateExtraTable = @"
            //-- Extra 테이블 (인덱스 없음)
            //CREATE TABLE IF NOT EXISTS SubstrateExtra (
            //    SubstrateKey    TEXT NOT NULL,
            //    ExtraKey        TEXT NOT NULL,
            //    ExtraValue      TEXT,
            //    PRIMARY KEY (SubstrateKey, ExtraKey),
            //    FOREIGN KEY (SubstrateKey)
            //        REFERENCES Substrate(UniqueKey)
            //        ON DELETE CASCADE
            //);
            //";

            // History
            //            const string LocationHistoryTable = @"
            //-- Stay History
            //CREATE TABLE IF NOT EXISTS SubstrateStayHistory (
            //    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
            //    SubstrateKey   TEXT    NOT NULL,
            //    LocationName   TEXT    NOT NULL,
            //    LocationType   TEXT    NOT NULL,
            //    StayStartTime  TEXT    NOT NULL,
            //    StayEndTime    TEXT    NOT NULL,
            //    StartAction    TEXT    NOT NULL,
            //    EndAction      TEXT    NOT NULL,

            //    FOREIGN KEY (SubstrateKey)
            //        REFERENCES Substrate(UniqueKey)
            //        ON DELETE CASCADE,

            //    FOREIGN KEY (LocationName)
            //        REFERENCES Location(Name)
            //);
            //";

            const string LocationChangeHistoryTable = @"
-- Location Change History
CREATE TABLE IF NOT EXISTS SubstrateLocationHistory (
    Id               INTEGER PRIMARY KEY AUTOINCREMENT,
    SubstrateKey     TEXT    NOT NULL,
    FromLocationName TEXT    NULL,
    FromLocationKind TEXT,
    ToLocationName   TEXT    NULL,
    ToLocationKind   TEXT,
    ChangeTime       TEXT    NOT NULL,
    Reason           TEXT    NOT NULL,

    FOREIGN KEY (SubstrateKey)
        REFERENCES Substrate(UniqueKey)
        ON DELETE CASCADE,

    FOREIGN KEY (FromLocationName)
        REFERENCES Location(Name),

    FOREIGN KEY (ToLocationName)
        REFERENCES Location(Name)
);
";

            // History
            const string ProcessingHistoryTable = @"
-- Processing History
CREATE TABLE IF NOT EXISTS SubstrateProcessingHistory (
    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
    SubstrateKey  TEXT    NOT NULL,
    EventTime     TEXT    NOT NULL,
    OldState      TEXT    NOT NULL,
    NewState      TEXT    NOT NULL,
    ControlJobId  TEXT,
    ProcessJobId  TEXT,
    LocationId    TEXT,
    Description   TEXT,

    FOREIGN KEY (SubstrateKey)
        REFERENCES Substrate(UniqueKey)
        ON DELETE CASCADE
);
";

            // 2026.07.06. jhlim [ADD] 랏 히스토리 이벤트 (EFEM.History.SqliteHistoryStore가 기록)
            // - CarrierKey/SubstrateKey는 Carrier/Substrate.UniqueKey와 논리적으로 조인되는 키지만
            //   FK 제약은 걸지 않는다: 자재 행의 archive 이동/삭제(캐스케이드)와 이력 수명주기를 분리하기 위함.
            //   이력 행의 archive 이동은 SqliteCarrierStorage.PrepareToArchiveAsync에서 캐리어 제거와 함께 수행된다.
            const string LotHistoryEventTable = @"
-- Lot History Event
CREATE TABLE IF NOT EXISTS LotHistoryEvent (
    Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
    EventTime           TEXT    NOT NULL,
    Category            TEXT    NOT NULL DEFAULT '',
    PortId              INTEGER NOT NULL DEFAULT -1,
    CarrierKey          TEXT    NOT NULL DEFAULT '',
    CarrierId           TEXT    NOT NULL DEFAULT '',
    LotId               TEXT    NOT NULL DEFAULT '',
    SubstrateKey        TEXT    NOT NULL DEFAULT '',
    SubstrateName       TEXT    NOT NULL DEFAULT '',
    CarrierEventCode    TEXT    NOT NULL DEFAULT '',
    SubstrateEventCode  TEXT    NOT NULL DEFAULT '',
    Message             TEXT    NOT NULL DEFAULT ''
);
";

            // 인덱스
            public const string Indexes = @"
-- ========= 인덱스 (Extra 제외) =========
CREATE INDEX IF NOT EXISTS IX_Location_LocationKind
    ON Location(LocationKind);

CREATE INDEX IF NOT EXISTS IX_Carrier_LotId
    ON Carrier(LotId);

CREATE INDEX IF NOT EXISTS IX_Carrier_PortId
    ON Carrier(PortId);

CREATE INDEX IF NOT EXISTS IX_Substrate_LocationId
    ON Substrate(LocationId);

CREATE INDEX IF NOT EXISTS IX_Substrate_LotId
    ON Substrate(LotId);

CREATE INDEX IF NOT EXISTS IX_Substrate_CurrentCarrier
    ON Substrate(CurrentCarrierKey);

CREATE INDEX IF NOT EXISTS IX_SubstrateLocationHistory_Key_Time
    ON SubstrateLocationHistory(SubstrateKey, ChangeTime);

CREATE INDEX IF NOT EXISTS IX_SubstrateLocationHistory_ToLocation_Time
    ON SubstrateLocationHistory(ToLocationName, ChangeTime);

CREATE INDEX IF NOT EXISTS IX_SubstrateProcHistory_Substrate
    ON SubstrateProcessingHistory(SubstrateKey, EventTime);

CREATE INDEX IF NOT EXISTS IX_SubstrateProcHistory_ControlJob
    ON SubstrateProcessingHistory(ControlJobId);

CREATE INDEX IF NOT EXISTS IX_SubstrateProcHistory_ProcessJob
    ON SubstrateProcessingHistory(ProcessJobId);

CREATE INDEX IF NOT EXISTS IX_LotHistoryEvent_CarrierKey
    ON LotHistoryEvent(CarrierKey);

CREATE INDEX IF NOT EXISTS IX_LotHistoryEvent_SubstrateKey
    ON LotHistoryEvent(SubstrateKey);

CREATE INDEX IF NOT EXISTS IX_LotHistoryEvent_LotId
    ON LotHistoryEvent(LotId);

CREATE INDEX IF NOT EXISTS IX_LotHistoryEvent_EventTime
    ON LotHistoryEvent(EventTime);

-- 파일 저장소 실패로 이력 명령이 재시도될 때 DB에 같은 이벤트가 중복 INSERT되는 것을
-- INSERT OR IGNORE로 막기 위한 자연 키 (ms 타임스탬프 + 내용 동일 = 같은 이벤트)
CREATE UNIQUE INDEX IF NOT EXISTS UQ_LotHistoryEvent_Natural
    ON LotHistoryEvent(EventTime, SubstrateName, CarrierEventCode, SubstrateEventCode, Message);
";

            // 전체 스키마
            internal static readonly string MainSchema =
                SchemaVersion
                + LocationTable
                + CarrierMainTable
                + CarrierSlotMapTable
                + SubstrateMainTable
                //+ LocationHistoryTable
                + LocationChangeHistoryTable
                + ProcessingHistoryTable
                + LotHistoryEventTable;
            //+ CarrierExtraTable
            //+ SubstrateExtraTable
            //+ Indexes;
        }
        private static class ArchiveSchemaSql
        {
            const string CarrierMainTable = @"
CREATE TABLE IF NOT EXISTS archive.Carrier (
    UniqueKey     TEXT PRIMARY KEY,
    LotId         TEXT,
    CarrierId     TEXT,
    PortId        INTEGER,
    AccessStatus  TEXT,
    Capacity      INTEGER,
    LoadTime      TEXT,
    UnloadTime    TEXT
);
";
            const string CarrierSlotMapTable = @"
CREATE TABLE IF NOT EXISTS archive.CarrierSlotMap (
    CarrierKey  TEXT    NOT NULL,
    SlotNo      INTEGER NOT NULL,
    MapValue    TEXT NOT NULL,
    PRIMARY KEY (CarrierKey, SlotNo)
);
";
            //            const string CarrierExtraTable = @"
            //CREATE TABLE IF NOT EXISTS archive.CarrierExtra (
            //    CarrierKey  TEXT NOT NULL,
            //    ExtraKey    TEXT NOT NULL,
            //    ExtraValue  TEXT,
            //    PRIMARY KEY (CarrierKey, ExtraKey)
            //);
            //";

            const string SubstrateMainTable = @"
CREATE TABLE IF NOT EXISTS archive.Substrate (
    UniqueKey           TEXT PRIMARY KEY,
    Name                TEXT,
    OriginName          TEXT,
    LocationId          TEXT,
    SourcePortId        INTEGER,
    SourceSlot          INTEGER,
    SourceCarrierId     TEXT,
    CurrentCarrierKey   TEXT,
    DestinationPortId   INTEGER,
    DestinationSlot     INTEGER,
    LotId               TEXT,
    RecipeId            TEXT,
    ProcessJobId        TEXT,
    ControlJobId        TEXT,
    TransportStatus     TEXT,
    ProcessingStatus    TEXT,
    IdReadingStatus     TEXT,
    DoNotProcessFlag    INTEGER NOT NULL,
    Usage               INTEGER NOT NULL
);
";
            //            const string SubstrateExtraTable = @"
            //CREATE TABLE IF NOT EXISTS archive.SubstrateExtra (
            //    SubstrateKey    TEXT NOT NULL,
            //    ExtraKey        TEXT NOT NULL,
            //    ExtraValue      TEXT,
            //    PRIMARY KEY (SubstrateKey, ExtraKey)
            //);
            //";

            //            const string LocationHistoryTable = @"
            //CREATE TABLE IF NOT EXISTS archive.SubstrateStayHistory (
            //    Id             INTEGER PRIMARY KEY,
            //    SubstrateKey   TEXT    NOT NULL,
            //    LocationName   TEXT    NOT NULL,
            //    LocationType   TEXT    NOT NULL,
            //    StayStartTime  TEXT    NOT NULL,
            //    StayEndTime    TEXT    NOT NULL,
            //    StartAction    TEXT    NOT NULL,
            //    EndAction      TEXT    NOT NULL
            //);
            //";

            const string LocationChangeHistoryTable = @"
CREATE TABLE IF NOT EXISTS archive.SubstrateLocationHistory (
    Id               INTEGER PRIMARY KEY,
    SubstrateKey     TEXT    NOT NULL,
    FromLocationName TEXT    NULL,
    FromLocationKind TEXT,
    ToLocationName   TEXT    NULL,
    ToLocationKind   TEXT,
    ChangeTime       TEXT    NOT NULL,
    Reason           TEXT    NOT NULL
);
";

            const string ProcessingHistoryTable = @"
CREATE TABLE IF NOT EXISTS archive.SubstrateProcessingHistory (
    Id            INTEGER PRIMARY KEY,
    SubstrateKey  TEXT    NOT NULL,
    EventTime     TEXT    NOT NULL,
    OldState      TEXT    NOT NULL,
    NewState      TEXT    NOT NULL,
    ControlJobId  TEXT,
    ProcessJobId  TEXT,
    LocationId    TEXT,
    Description   TEXT
);
";

            // 2026.07.06. jhlim [ADD] 랏 히스토리 이벤트 아카이브 (캐리어 제거 시 main에서 이동)
            // Id는 main 값을 승계하지 않고 archive 파일 자체 AUTOINCREMENT 사용 (정렬은 EventTime 기준)
            const string LotHistoryEventTable = @"
CREATE TABLE IF NOT EXISTS archive.LotHistoryEvent (
    Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
    EventTime           TEXT    NOT NULL,
    Category            TEXT    NOT NULL DEFAULT '',
    PortId              INTEGER NOT NULL DEFAULT -1,
    CarrierKey          TEXT    NOT NULL DEFAULT '',
    CarrierId           TEXT    NOT NULL DEFAULT '',
    LotId               TEXT    NOT NULL DEFAULT '',
    SubstrateKey        TEXT    NOT NULL DEFAULT '',
    SubstrateName       TEXT    NOT NULL DEFAULT '',
    CarrierEventCode    TEXT    NOT NULL DEFAULT '',
    SubstrateEventCode  TEXT    NOT NULL DEFAULT '',
    Message             TEXT    NOT NULL DEFAULT ''
);

CREATE INDEX IF NOT EXISTS archive.IX_LotHistoryEvent_CarrierKey
    ON LotHistoryEvent(CarrierKey);

CREATE INDEX IF NOT EXISTS archive.IX_LotHistoryEvent_LotId
    ON LotHistoryEvent(LotId);

CREATE INDEX IF NOT EXISTS archive.IX_LotHistoryEvent_EventTime
    ON LotHistoryEvent(EventTime);
";

            public const string ArchiveAtTableAndIndexes = @"
CREATE TABLE IF NOT EXISTS archive.ArchiveAt (
    Id         INTEGER PRIMARY KEY AUTOINCREMENT,
    ItemKey    TEXT    NOT NULL,
    ItemKind   INTEGER NOT NULL, -- 0 = Carrier, 1 = Substrate
    ArchivedAt TEXT    NOT NULL
);

CREATE INDEX IF NOT EXISTS archive.IX_ArchiveAt_ItemKey
    ON ArchiveAt(ItemKey);

CREATE INDEX IF NOT EXISTS archive.IX_ArchiveAt_ArchivedAt
    ON ArchiveAt(ArchivedAt);
";

            public static readonly string ArchiveSchema =
                CarrierMainTable
                + CarrierSlotMapTable
                + SubstrateMainTable
                //+ LocationHistoryTable
                + LocationChangeHistoryTable
                + ProcessingHistoryTable
                + LotHistoryEventTable;
            //+ CarrierExtraTable
            //+ SubstrateExtraTable
            //+ ArchiveAtTableAndIndexes;
        }
        private static class WriteJobCommandLogContext
        {
            private static readonly AsyncLocal<WriteJob> _currentJob = new AsyncLocal<WriteJob>();
            public static WriteJob CurrentJob
            {
                get { return _currentJob.Value; }
                set { _currentJob.Value = value; }
            }
        }

        /// <summary>
        /// 현재 실행 중인 쓰기 잡(WriteJob)에 이 커맨드를 기록한다. 잡 컨텍스트 밖(읽기 경로 등)에서 호출되면
        /// 조용히 no-op — 그래서 저장소 클래스의 모든 커맨드 실행 지점(읽기 포함)에 구분 없이 넣어도 안전하다.
        /// 실행 직전에 호출해야 한다 — 그래야 ExecuteXxx 자체가 실패해도 이미 로그에 남는다.
        /// </summary>
        public static void LogCommand(SQLiteCommand cmd)
        {
            var job = WriteJobCommandLogContext.CurrentJob;
            if (job == null || cmd == null)
                return;

            var parameters = new Dictionary<string, object>();
            foreach (SQLiteParameter p in cmd.Parameters)
                parameters[p.ParameterName] = p.Value ?? DBNull.Value;

            job.LoggedCommands.Add(new DbCommandLogEntry
            {
                CommandText = cmd.CommandText,
                Parameters = parameters
            });
        }

        private bool _isShuttingDown;
        private readonly ConcurrentQueue<WriteJob> _writeQueue = new ConcurrentQueue<WriteJob>();
        private readonly SemaphoreSlim _queueSignal = new SemaphoreSlim(0);
        private readonly Task _writerTask;
        private readonly string _connectionString;
        private readonly Action<string> _funcForLog;
        private bool _disposed;
        private const string ArchiveSchemaName = "archive";
        private readonly IReadOnlyList<IDbMigrationStep> _migrationSteps;

        public string DataBaseFilePath { get; }
        public string DataBasePath { get; }
        public string[] CarrierExtraKeys { get; }
        public string[] SubstrateExtraKeys { get; }

        public MaterialDbContext(string dbFilePath,
            Action<string> funcForLog,
            Func<IEnumerable<string>> funcsForCarrier,
            Func<IEnumerable<string>> funcsForSubstrate,
            IEnumerable<IDbMigrationStep> extraMigrationSteps)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentNullException(nameof(dbFilePath));

            DataBaseFilePath = dbFilePath;
            DataBasePath = Path.GetDirectoryName(dbFilePath);

            CarrierExtraKeys = funcsForCarrier().OrderBy(x => x).ToArray();
            SubstrateExtraKeys = funcsForSubstrate().OrderBy(x => x).ToArray();

            _funcForLog = funcForLog;

            #region <Migration>
            //var combinedMigrationSteps = CreateDefaultMigrationSteps();

            //if (extraMigrationSteps != null)
            //{
            //    combinedMigrationSteps.AddRange(extraMigrationSteps);
            //}

            //_migrationSteps = combinedMigrationSteps
            //    .OrderBy(x => x.Version)
            //    .ToList();
            _migrationSteps = extraMigrationSteps.ToList();
            #endregion <Migration>

            var dir = Path.GetDirectoryName(dbFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = dbFilePath,
                ForeignKeys = true,
                JournalMode = SQLiteJournalModeEnum.Wal
            };

            _connectionString = builder.ToString();

            ExecuteWorkingTables();

            _writerTask = Task.Run(() => WriteWorkerLoopAsync());
        }

        public async Task ShutdownAsync()
        {
            _isShuttingDown = true;

            _queueSignal.Release();

            await _writerTask.ConfigureAwait(false);
        }
        public SQLiteConnection OpenConnection()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MaterialDbContext));

            var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }

            return conn;
        }
        private void WriteLog(string message)
        {
            _funcForLog?.Invoke(message);
        }
        private async Task WriteWorkerLoopAsync()
        {
            SQLiteConnection conn = null;

            try
            {
                conn = new SQLiteConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                while (true)
                {
                    // 큐에 뭐가 올 때까지 대기
                    await _queueSignal.WaitAsync().ConfigureAwait(false);

                    // 같은 signal로 여러 job이 쌓여있을 수 있으니 다 비우기
                    while (_writeQueue.TryDequeue(out var job))
                    {
                        if (job.UseTransaction)
                        {
                            await ProcessWriteJobAsync(conn, job).ConfigureAwait(false);
                        }
                        else
                        {
                            await ProcessWriteJobWithoutTransactionAsync(conn, job).ConfigureAwait(false);
                        }
                    }

                    if (_isShuttingDown && _writeQueue.IsEmpty)
                        break;
                }
            }
            catch (Exception ex)
            {
                // 여기까지 터졌다는 건 worker 자체가 죽을 상황.
                // 반드시 로그 남기기.
                WriteLog($"WriteWorkerLoopAsync fatal error: {ex}");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        private async Task ProcessWriteJobAsync(SQLiteConnection conn, WriteJob job)
        {
            try
            {
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        WriteJobCommandLogContext.CurrentJob = job;

                        await job.Work(conn, tx).ConfigureAwait(false);

                        tx.Commit();
                        job.Tcs.TrySetResult(true);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            tx.Rollback();
                        }
                        catch
                        {
                        }

                        LogJobFailure(job, ex);
                        job.Tcs.TrySetException(ex);
                    }
                    finally
                    {
                        WriteJobCommandLogContext.CurrentJob = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogJobFailure(job, ex);
                job.Tcs.TrySetException(ex);
            }
        }
        private async Task ProcessWriteJobWithoutTransactionAsync(SQLiteConnection conn, WriteJob job)
        {
            try
            {
                WriteJobCommandLogContext.CurrentJob = job;

                await job.Work(conn, null).ConfigureAwait(false);

                job.Tcs.TrySetResult(true);
            }
            catch (Exception ex)
            {
                LogJobFailure(job, ex);
                job.Tcs.TrySetException(ex);
            }
            finally
            {
                WriteJobCommandLogContext.CurrentJob = null;
            }
        }

        private void LogJobFailure(WriteJob job, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- DB write job failed.");
            sb.AppendLine(ex.ToString());

            if (job?.LoggedCommands == null || job.LoggedCommands.Count == 0)
            {
                sb.AppendLine("No command was logged for this job.");
            }
            else
            {
                for (int i = 0; i < job.LoggedCommands.Count; i++)
                {
                    var entry = job.LoggedCommands[i];

                    sb.AppendLine($"Command #{i + 1}:");
                    sb.AppendLine(entry.CommandText ?? "<null>");

                    if (entry.Parameters != null && entry.Parameters.Count > 0)
                    {
                        sb.AppendLine("Parameters:");
                        foreach (var kv in entry.Parameters)
                        {
                            sb.Append("  ");
                            sb.Append(kv.Key);
                            sb.Append(" = ");
                            sb.Append(kv.Value ?? "NULL");
                            sb.AppendLine();
                        }
                    }
                }
            }

            WriteLog(sb.ToString());
        }

        private int GetMainSchemaVersion(SQLiteConnection conn, SQLiteTransaction tx)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "SELECT Version FROM SchemaVersion LIMIT 1;";

                var value = cmd.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                    return (int)DataBaseVersion.BaseSchema;

                return Convert.ToInt32(value);
            }
        }

        private void SetMainSchemaVersion(SQLiteConnection conn, SQLiteTransaction tx, int version)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "UPDATE SchemaVersion SET Version = $version;";
                cmd.Parameters.AddWithValue("$version", version);
                cmd.ExecuteNonQuery();
            }
        }

        private int GetArchiveSchemaVersion(SQLiteConnection conn, SQLiteTransaction tx)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "PRAGMA archive.user_version;";

                var value = cmd.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                    return 0;

                return Convert.ToInt32(value);
            }
        }

        private void SetArchiveSchemaVersion(SQLiteConnection conn, SQLiteTransaction tx, int version)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "PRAGMA archive.user_version = " + version + ";";
                cmd.ExecuteNonQuery();
            }
        }

        private static string QuoteIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Identifier is required.", nameof(name));

            if (!Regex.IsMatch(name, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                throw new ArgumentException("Invalid identifier: " + name, nameof(name));

            return "\"" + name + "\"";
        }

        private static string GetQualifiedTableName(string schemaName, string tableName)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                return QuoteIdentifier(tableName);

            return QuoteIdentifier(schemaName) + "." + QuoteIdentifier(tableName);
        }

        private static string GetQualifiedMasterTableName(string schemaName)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                return "sqlite_master";

            return QuoteIdentifier(schemaName) + ".sqlite_master";
        }

        private static string GetDisplayTableName(string schemaName, string tableName)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                return tableName;

            return schemaName + "." + tableName;
        }

        private bool TableExists(SQLiteConnection conn, SQLiteTransaction tx, string schemaName, string tableName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText =
                    "SELECT 1 FROM " + GetQualifiedMasterTableName(schemaName) +
                    " WHERE type = 'table' AND name = $name LIMIT 1;";
                cmd.Parameters.AddWithValue("$name", tableName);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value;
            }
        }

        private bool ColumnExists(SQLiteConnection conn, SQLiteTransaction tx, string schemaName, string tableName, string columnName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;

                if (string.IsNullOrWhiteSpace(schemaName))
                {
                    cmd.CommandText = "PRAGMA table_info(" + QuoteIdentifier(tableName) + ");";
                }
                else
                {
                    cmd.CommandText = "PRAGMA " + QuoteIdentifier(schemaName) +
                                      ".table_info(" + QuoteIdentifier(tableName) + ");";
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var currentName = Convert.ToString(reader["name"]);
                        if (string.Equals(currentName, columnName, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                }
            }

            return false;
        }

        private void ApplyMigrations(
            SQLiteConnection conn,
            SQLiteTransaction tx,
            string schemaName,
            Func<SQLiteConnection, SQLiteTransaction, int> getVersion,
            Action<SQLiteConnection, SQLiteTransaction, int> setVersion)
        {
            var currentVersion = getVersion(conn, tx);
            var maxVersion = currentVersion;

            // 카운터로 스킵하지 않는다: 마이그레이션 버전 번호가 과거 재배정된 이력이 있어(SchemaVersion 정수는
            // "무엇이 실제로 적용됐는가"의 신뢰 지표가 아님), 카운터로 스킵하면 미적용 스텝(예: Location.Name->Id)이
            // 잘못 건너뛰어질 수 있다. 각 스텝은 실제 스키마 상태(TableExists/ColumnExists)를 검사하는 멱등 연산이므로
            // 항상 Apply를 호출해 실제 상태 기준으로 자가 치유하게 한다. (버전 재배정/롤백된 DB에서도 정확)
            foreach (var migration in _migrationSteps)
            {
                migration.Apply(conn, tx, schemaName);

                if (migration.Version > maxVersion)
                    maxVersion = migration.Version;
            }

            // SchemaVersion은 게이트가 아니라 정보성 스탬프로만 갱신한다(최신 스텝 번호 기록).
            if (maxVersion != currentVersion)
            {
                setVersion(conn, tx, maxVersion);
                WriteLog("[DB Migration] " + GetDisplayTableName(schemaName, "Schema")
                    + " version stamp updated to " + maxVersion + " (state-based apply).");
            }
        }

        private void ApplySchemaMigrations(SQLiteConnection conn, SQLiteTransaction tx)
        {
            ApplyMigrations(
                conn,
                tx,
                null,
                GetMainSchemaVersion,
                SetMainSchemaVersion);
        }

        private void ApplyArchiveSchemaMigrations(SQLiteConnection conn, SQLiteTransaction tx)
        {
            ApplyMigrations(
                conn,
                tx,
                ArchiveSchemaName,
                GetArchiveSchemaVersion,
                SetArchiveSchemaVersion);
        }

        #region <Enum Column Affinity Rebuild>
        // 2026.07.13. jhlim [ADD] enum 저장을 정수 ordinal -> 이름(TEXT)으로 전환했는데,
        // 이 장비의 SQLite 스택(3.32.1 + 커스텀 인터롭)은 INTEGER affinity 컬럼에 비숫자 TEXT를 저장하면
        // 값이 '0'(text)으로 훼손되는 비표준 동작을 보인다(검증: 리터럴/파라미터/CAST 모두 동일, TEXT 컬럼은 정상).
        // 따라서 레거시 DB(enum 컬럼이 INTEGER 로 선언된)는 UPDATE 백필이 불가능하며,
        // 테이블 재구축(새 TEXT 테이블 생성 -> CASE 매핑 복사 -> DROP -> RENAME)이 필요하다.
        // - main DB: FK(ON DELETE CASCADE) 때문에 DROP 시 암묵 DELETE 가 연쇄삭제를 유발하므로
        //   반드시 ForeignKeys=false 별도 연결에서 수행한다(PRAGMA foreign_keys 는 트랜잭션 안에서 변경 불가).
        // - archive: FK 정의가 없어 호출측 트랜잭션 안에서 안전하게 수행 가능.
        // AccessStatus 만 5.18(Unknown=0 존재)↔6.18 ordinal 시프트가 있어 scheme-aware 매핑을 쓴다.

        private sealed class EnumTableRebuildDef
        {
            public string Table;
            public string CreateBody;                          // CREATE TABLE {name} ( ... ) 괄호 안
            public string[] Columns;                           // canonical 컬럼(복사 대상, 존재하는 것만 복사)
            public Dictionary<string, string> EnumCaseByColumn; // enum 컬럼 -> CASE WHEN body
        }

        private static string GetAccessStatusCase(bool legacy518)
        {
            // 5.18: Unknown=0, NotAccessed=1, InAccessed=2, CarrierCompleted=3, CarrierStopped=4
            // 6.18: NotAccessed=0, InAccessed=1, CarrierCompleted=2, CarrierStopped=3
            // 미지 값은 절대 CarrierCompleted 로 두지 않는다(미처리 조기 배출 방지) -> InAccessed.
            return legacy518
                ? "WHEN 0 THEN 'NotAccessed' WHEN 1 THEN 'NotAccessed' WHEN 2 THEN 'InAccessed' WHEN 3 THEN 'CarrierCompleted' WHEN 4 THEN 'CarrierStopped' ELSE 'InAccessed'"
                : "WHEN 0 THEN 'NotAccessed' WHEN 1 THEN 'InAccessed' WHEN 2 THEN 'CarrierCompleted' WHEN 3 THEN 'CarrierStopped' ELSE 'InAccessed'";
        }

        private const string SlotMapCase = "WHEN 0 THEN 'Undefined' WHEN 1 THEN 'Empty' WHEN 2 THEN 'NotEmpty' WHEN 3 THEN 'CorrectlyOccupied' WHEN 4 THEN 'DoubleSlotted' WHEN 5 THEN 'CrossSlotted' ELSE 'Undefined'";
        private const string TransportCase = "WHEN 0 THEN 'AtSource' WHEN 1 THEN 'AtWork' WHEN 2 THEN 'AtDestination' ELSE 'AtSource'";
        private const string ProcessingCase = "WHEN 0 THEN 'NeedsProcessing' WHEN 1 THEN 'InProcess' WHEN 2 THEN 'Processed' WHEN 3 THEN 'Aborted' WHEN 4 THEN 'Stopped' WHEN 5 THEN 'Rejected' WHEN 6 THEN 'Lost' WHEN 7 THEN 'Skipped' ELSE 'NeedsProcessing'";
        private const string IdReadingCase = "WHEN 0 THEN 'NotConfirmed' WHEN 1 THEN 'WaitingForHost' WHEN 2 THEN 'Confirmed' WHEN 3 THEN 'ConfirmationFailed' ELSE 'NotConfirmed'";
        private const string ModuleTypeCase = "WHEN 0 THEN 'Unknown' WHEN 1 THEN 'LoadPort' WHEN 2 THEN 'Robot' WHEN 3 THEN 'ProcessModule' WHEN 4 THEN 'Aligner' WHEN 5 THEN 'Normal' ELSE 'Unknown'";

        // main 스키마용(마이그레이션 v2~v5 적용 이후 canonical). Location.Id/Name, Substrate.OriginName 존재 전제.
        private static List<EnumTableRebuildDef> GetMainRebuildDefs(bool legacy518)
        {
            return new List<EnumTableRebuildDef>
            {
                new EnumTableRebuildDef
                {
                    Table = "Location",
                    CreateBody = "Id TEXT PRIMARY KEY, LocationKind TEXT NOT NULL, Capacity INTEGER NOT NULL CHECK (Capacity >= 0), Name TEXT",
                    Columns = new[] { "Id", "LocationKind", "Capacity", "Name" },
                    EnumCaseByColumn = new Dictionary<string, string> { ["LocationKind"] = ModuleTypeCase },
                },
                new EnumTableRebuildDef
                {
                    Table = "Carrier",
                    CreateBody = "UniqueKey TEXT PRIMARY KEY, LotId TEXT, CarrierId TEXT, PortId INTEGER, AccessStatus TEXT NOT NULL DEFAULT 'NotAccessed', Capacity INTEGER NOT NULL DEFAULT 0, LoadTime TEXT, UnloadTime TEXT",
                    Columns = new[] { "UniqueKey", "LotId", "CarrierId", "PortId", "AccessStatus", "Capacity", "LoadTime", "UnloadTime" },
                    EnumCaseByColumn = new Dictionary<string, string> { ["AccessStatus"] = GetAccessStatusCase(legacy518) },
                },
                new EnumTableRebuildDef
                {
                    Table = "CarrierSlotMap",
                    CreateBody = "CarrierKey TEXT NOT NULL, SlotNo INTEGER NOT NULL, MapValue TEXT NOT NULL, PRIMARY KEY (CarrierKey, SlotNo), FOREIGN KEY (CarrierKey) REFERENCES Carrier(UniqueKey) ON DELETE CASCADE",
                    Columns = new[] { "CarrierKey", "SlotNo", "MapValue" },
                    EnumCaseByColumn = new Dictionary<string, string> { ["MapValue"] = SlotMapCase },
                },
                new EnumTableRebuildDef
                {
                    Table = "Substrate",
                    CreateBody = "UniqueKey TEXT PRIMARY KEY, Name TEXT, OriginName TEXT, LocationId TEXT, SourcePortId INTEGER, SourceSlot INTEGER, SourceCarrierId TEXT, CurrentCarrierKey TEXT, DestinationPortId INTEGER, DestinationSlot INTEGER, LotId TEXT, RecipeId TEXT, ProcessJobId TEXT, ControlJobId TEXT, TransportStatus TEXT, ProcessingStatus TEXT, IdReadingStatus TEXT, DoNotProcessFlag INTEGER NOT NULL DEFAULT 0, Usage INTEGER NOT NULL DEFAULT 0, FOREIGN KEY (LocationId) REFERENCES Location(Id), FOREIGN KEY (CurrentCarrierKey) REFERENCES Carrier(UniqueKey) ON DELETE CASCADE",
                    Columns = new[] { "UniqueKey", "Name", "OriginName", "LocationId", "SourcePortId", "SourceSlot", "SourceCarrierId", "CurrentCarrierKey", "DestinationPortId", "DestinationSlot", "LotId", "RecipeId", "ProcessJobId", "ControlJobId", "TransportStatus", "ProcessingStatus", "IdReadingStatus", "DoNotProcessFlag", "Usage" },
                    EnumCaseByColumn = new Dictionary<string, string>
                    {
                        ["TransportStatus"] = TransportCase,
                        ["ProcessingStatus"] = ProcessingCase,
                        ["IdReadingStatus"] = IdReadingCase,
                    },
                },
                new EnumTableRebuildDef
                {
                    Table = "SubstrateLocationHistory",
                    CreateBody = "Id INTEGER PRIMARY KEY AUTOINCREMENT, SubstrateKey TEXT NOT NULL, FromLocationName TEXT NULL, FromLocationKind TEXT, ToLocationName TEXT NULL, ToLocationKind TEXT, ChangeTime TEXT NOT NULL, Reason TEXT NOT NULL, FOREIGN KEY (SubstrateKey) REFERENCES Substrate(UniqueKey) ON DELETE CASCADE, FOREIGN KEY (FromLocationName) REFERENCES Location(Id), FOREIGN KEY (ToLocationName) REFERENCES Location(Id)",
                    Columns = new[] { "Id", "SubstrateKey", "FromLocationName", "FromLocationKind", "ToLocationName", "ToLocationKind", "ChangeTime", "Reason" },
                    EnumCaseByColumn = new Dictionary<string, string>
                    {
                        ["FromLocationKind"] = ModuleTypeCase,
                        ["ToLocationKind"] = ModuleTypeCase,
                    },
                },
            };
        }

        // archive 스키마용(FK/AUTOINCREMENT 없음)
        private static List<EnumTableRebuildDef> GetArchiveRebuildDefs(bool legacy518)
        {
            return new List<EnumTableRebuildDef>
            {
                new EnumTableRebuildDef
                {
                    Table = "Carrier",
                    CreateBody = "UniqueKey TEXT PRIMARY KEY, LotId TEXT, CarrierId TEXT, PortId INTEGER, AccessStatus TEXT, Capacity INTEGER, LoadTime TEXT, UnloadTime TEXT",
                    Columns = new[] { "UniqueKey", "LotId", "CarrierId", "PortId", "AccessStatus", "Capacity", "LoadTime", "UnloadTime" },
                    EnumCaseByColumn = new Dictionary<string, string> { ["AccessStatus"] = GetAccessStatusCase(legacy518) },
                },
                new EnumTableRebuildDef
                {
                    Table = "CarrierSlotMap",
                    CreateBody = "CarrierKey TEXT NOT NULL, SlotNo INTEGER NOT NULL, MapValue TEXT NOT NULL, PRIMARY KEY (CarrierKey, SlotNo)",
                    Columns = new[] { "CarrierKey", "SlotNo", "MapValue" },
                    EnumCaseByColumn = new Dictionary<string, string> { ["MapValue"] = SlotMapCase },
                },
                new EnumTableRebuildDef
                {
                    Table = "Substrate",
                    CreateBody = "UniqueKey TEXT PRIMARY KEY, Name TEXT, OriginName TEXT, LocationId TEXT, SourcePortId INTEGER, SourceSlot INTEGER, SourceCarrierId TEXT, CurrentCarrierKey TEXT, DestinationPortId INTEGER, DestinationSlot INTEGER, LotId TEXT, RecipeId TEXT, ProcessJobId TEXT, ControlJobId TEXT, TransportStatus TEXT, ProcessingStatus TEXT, IdReadingStatus TEXT, DoNotProcessFlag INTEGER NOT NULL DEFAULT 0, Usage INTEGER NOT NULL DEFAULT 0",
                    Columns = new[] { "UniqueKey", "Name", "OriginName", "LocationId", "SourcePortId", "SourceSlot", "SourceCarrierId", "CurrentCarrierKey", "DestinationPortId", "DestinationSlot", "LotId", "RecipeId", "ProcessJobId", "ControlJobId", "TransportStatus", "ProcessingStatus", "IdReadingStatus", "DoNotProcessFlag", "Usage" },
                    EnumCaseByColumn = new Dictionary<string, string>
                    {
                        ["TransportStatus"] = TransportCase,
                        ["ProcessingStatus"] = ProcessingCase,
                        ["IdReadingStatus"] = IdReadingCase,
                    },
                },
                new EnumTableRebuildDef
                {
                    Table = "SubstrateLocationHistory",
                    CreateBody = "Id INTEGER PRIMARY KEY, SubstrateKey TEXT NOT NULL, FromLocationName TEXT NULL, FromLocationKind TEXT, ToLocationName TEXT NULL, ToLocationKind TEXT, ChangeTime TEXT NOT NULL, Reason TEXT NOT NULL",
                    Columns = new[] { "Id", "SubstrateKey", "FromLocationName", "FromLocationKind", "ToLocationName", "ToLocationKind", "ChangeTime", "Reason" },
                    EnumCaseByColumn = new Dictionary<string, string>
                    {
                        ["FromLocationKind"] = ModuleTypeCase,
                        ["ToLocationKind"] = ModuleTypeCase,
                    },
                },
            };
        }

        private List<string> GetExistingColumns(SQLiteConnection conn, SQLiteTransaction tx, string schemaName, string tableName)
        {
            var cols = new List<string>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = string.IsNullOrWhiteSpace(schemaName)
                    ? "PRAGMA table_info(" + QuoteIdentifier(tableName) + ");"
                    : "PRAGMA " + QuoteIdentifier(schemaName) + ".table_info(" + QuoteIdentifier(tableName) + ");";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        cols.Add(Convert.ToString(reader["name"]));
                }
            }
            return cols;
        }

        private string GetColumnDeclType(SQLiteConnection conn, SQLiteTransaction tx, string schemaName, string tableName, string columnName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = string.IsNullOrWhiteSpace(schemaName)
                    ? "PRAGMA table_info(" + QuoteIdentifier(tableName) + ");"
                    : "PRAGMA " + QuoteIdentifier(schemaName) + ".table_info(" + QuoteIdentifier(tableName) + ");";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (string.Equals(Convert.ToString(reader["name"]), columnName, StringComparison.OrdinalIgnoreCase))
                            return Convert.ToString(reader["type"]) ?? string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// enum 컬럼이 INTEGER 로 선언된 레거시 테이블을 TEXT 스키마로 재구축한다(멱등: 이미 TEXT 면 no-op).
        /// 값 매핑은 typeof(값)='integer' 인 것만 CASE 로 이름 변환하고, 이미 TEXT 인 값은 그대로 복사한다.
        /// </summary>
        private void RebuildIntegerEnumTables(SQLiteConnection conn, SQLiteTransaction tx, string schemaName, List<EnumTableRebuildDef> defs)
        {
            foreach (var def in defs)
            {
                if (!TableExists(conn, tx, schemaName, def.Table))
                    continue;

                // 첫 enum 컬럼의 선언 타입으로 레거시 여부 판정
                string firstEnumCol = null;
                foreach (var kv in def.EnumCaseByColumn) { firstEnumCol = kv.Key; break; }
                if (firstEnumCol == null || !ColumnExists(conn, tx, schemaName, def.Table, firstEnumCol))
                    continue;

                var declType = GetColumnDeclType(conn, tx, schemaName, def.Table, firstEnumCol);
                if (declType.IndexOf("INT", StringComparison.OrdinalIgnoreCase) < 0)
                    continue;   // 이미 TEXT(신규/재구축 완료)

                var existing = GetExistingColumns(conn, tx, schemaName, def.Table);
                var copyCols = new List<string>();
                foreach (var c in def.Columns)
                {
                    foreach (var e in existing)
                    {
                        if (string.Equals(e, c, StringComparison.OrdinalIgnoreCase)) { copyCols.Add(c); break; }
                    }
                }

                var selectExprs = new List<string>();
                foreach (var c in copyCols)
                {
                    string caseBody;
                    if (def.EnumCaseByColumn.TryGetValue(c, out caseBody))
                    {
                        selectExprs.Add(
                            "CASE WHEN typeof(" + QuoteIdentifier(c) + ")='integer' THEN (CASE " + QuoteIdentifier(c) + " " + caseBody + " END) ELSE " + QuoteIdentifier(c) + " END");
                    }
                    else
                    {
                        selectExprs.Add(QuoteIdentifier(c));
                    }
                }

                var oldName = GetQualifiedTableName(schemaName, def.Table);
                var newName = GetQualifiedTableName(schemaName, def.Table + "_rebuild");

                ExecuteNonQuery(conn, tx, "DROP TABLE IF EXISTS " + newName + ";");
                ExecuteNonQuery(conn, tx, "CREATE TABLE " + newName + " (" + def.CreateBody + ");");
                ExecuteNonQuery(conn, tx,
                    "INSERT INTO " + newName + " (" + string.Join(", ", copyCols.ConvertAll(QuoteIdentifier)) + ") " +
                    "SELECT " + string.Join(", ", selectExprs) + " FROM " + oldName + ";");
                ExecuteNonQuery(conn, tx, "DROP TABLE " + oldName + ";");
                ExecuteNonQuery(conn, tx, "ALTER TABLE " + newName + " RENAME TO " + QuoteIdentifier(def.Table) + ";");

                WriteLog("[DB Migration] " + GetDisplayTableName(schemaName, def.Table)
                    + " rebuilt with TEXT enum columns (legacy INTEGER affinity).");
            }
        }

        /// <summary>
        /// 5.18(AccessStatus에 Unknown=0 이 존재하던 체계) DB 인지 판정.
        /// 지문: CarrierExtra.KeyLotQty 컬럼 존재(5.18 전용) 또는 스키마 버전 스탬프 &lt; 2 (6.18 계열 코드가 손대지 않음).
        /// (마이그레이션 v4 가 KeyLotQty 를 rename 하기 전에 호출해야 정확하다.)
        /// </summary>
        private bool DetectLegacy518Scheme(SQLiteConnection conn, SQLiteTransaction tx, string schemaName, Func<SQLiteConnection, SQLiteTransaction, int> getVersion)
        {
            if (TableExists(conn, tx, schemaName, "CarrierExtra") &&
                ColumnExists(conn, tx, schemaName, "CarrierExtra", "KeyLotQty"))
                return true;

            try
            {
                return getVersion(conn, tx) < 2;
            }
            catch
            {
                // SchemaVersion 테이블조차 없는 신규 DB: 어차피 재구축 대상 데이터가 없어 무해.
                return true;
            }
        }
        #endregion </Enum Column Affinity Rebuild>

        #region <Migration>
        internal void EnsureArchiveSchemaAndMigrate(SQLiteConnection conn, SQLiteTransaction tx)
        {
            // 주의: 판정(KeyLotQty)은 마이그레이션(v4 rename)보다 먼저 해야 한다.
            bool archiveLegacy518 = DetectLegacy518Scheme(conn, tx, ArchiveSchemaName, GetArchiveSchemaVersion);

            ExecuteNonQuery(conn, tx, GetArchiveCommand());
            ApplyArchiveSchemaMigrations(conn, tx);

            // archive 테이블은 FK 정의가 없어 트랜잭션 안 재구축이 안전하다(DROP 시 연쇄삭제 없음).
            RebuildIntegerEnumTables(conn, tx, ArchiveSchemaName, GetArchiveRebuildDefs(archiveLegacy518));
        }
        private void MigrateCarrierExtraKeyLotQtyToLotQty(
    SQLiteConnection conn,
    SQLiteTransaction tx,
    string schemaName)
        {
            const string tableName = "CarrierExtra";
            const string oldColumnName = "KeyLotQty";
            const string newColumnName = "LotQty";

            if (!TableExists(conn, tx, schemaName, tableName))
                return;

            var hasOldColumn = ColumnExists(conn, tx, schemaName, tableName, oldColumnName);
            var hasNewColumn = ColumnExists(conn, tx, schemaName, tableName, newColumnName);

            if (hasOldColumn && !hasNewColumn)
            {
                ExecuteNonQuery(
                    conn,
                    tx,
                    "ALTER TABLE " + GetQualifiedTableName(schemaName, tableName) +
                    " RENAME COLUMN " + oldColumnName + " TO " + newColumnName + ";");

                WriteLog("[DB Migration] " + GetDisplayTableName(schemaName, tableName)
                    + "." + oldColumnName + " renamed to " + newColumnName + ".");

                return;
            }

            if (hasOldColumn && hasNewColumn)
            {
                ExecuteNonQuery(
                    conn,
                    tx,
                    @"UPDATE " + GetQualifiedTableName(schemaName, tableName) + @"
              SET LotQty = COALESCE(LotQty, KeyLotQty)
              WHERE LotQty IS NULL;");

                WriteLog("[DB Migration] " + GetDisplayTableName(schemaName, tableName)
                    + "." + oldColumnName + " merged into " + newColumnName + ".");
            }
        }

        //private void MigrateToV2_RenameCarrierExtraKeyLotQtyToLotQty(SQLiteConnection conn, SQLiteTransaction tx)
        //{
        //    if (!TableExists(conn, tx, "CarrierExtra"))
        //        return;

        //    var hasOldColumn = ColumnExists(conn, tx, "CarrierExtra", "KeyLotQty");
        //    var hasNewColumn = ColumnExists(conn, tx, "CarrierExtra", "LotQty");

        //    if (hasOldColumn && !hasNewColumn)
        //    {
        //        ExecuteNonQuery(conn, tx,
        //            "ALTER TABLE CarrierExtra RENAME COLUMN KeyLotQty TO LotQty;");

        //        WriteLog("[DB Migration] CarrierExtra.KeyLotQty renamed to LotQty.");
        //        return;
        //    }

        //    if (hasOldColumn && hasNewColumn)
        //    {
        //        ExecuteNonQuery(conn, tx,
        //            @"UPDATE CarrierExtra
        //      SET LotQty = COALESCE(LotQty, KeyLotQty)
        //      WHERE LotQty IS NULL;");

        //        WriteLog("[DB Migration] CarrierExtra.KeyLotQty merged into LotQty.");
        //    }
        //}
        #endregion </Migration>

        private static void ExecuteNonQuery(SQLiteConnection conn, SQLiteTransaction tx, string commandText)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = commandText;
                cmd.ExecuteNonQuery();
            }
        }

        private void EnsureMainSchema(SQLiteConnection conn, SQLiteTransaction tx)
        {
            ExecuteNonQuery(conn, tx, MaterialSchemaSql.MainSchema);
            ExecuteNonQuery(conn, tx, ExtraSchemaCreator.GetCarrierExtraTableCommand(CarrierExtraKeys));
            ExecuteNonQuery(conn, tx, ExtraSchemaCreator.GetSubstrateExtraTableCommand(SubstrateExtraKeys));
        }

        private void ExecuteWorkingTables()
        {
            bool mainLegacy518;

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // 5.18 체계 판정은 v4(KeyLotQty rename) 실행 전에 해야 정확하다.
                mainLegacy518 = DetectLegacy518Scheme(conn, null, null, GetMainSchemaVersion);

                using (var tx = conn.BeginTransaction())
                {
                    EnsureMainSchema(conn, tx);

                    ApplySchemaMigrations(conn, tx);

                    tx.Commit();
                }
            }

            // 레거시 enum(INTEGER) 테이블 재구축: main 은 FK CASCADE 위험(DROP 시 암묵 DELETE) 때문에
            // 반드시 ForeignKeys=false 별도 연결에서 수행한다. 인덱스는 재구축으로 테이블과 함께
            // 삭제될 수 있으므로 재구축 이후에 생성한다. (이미 TEXT 인 신규 DB 에서는 전부 no-op)
            var builderNoFk = new SQLiteConnectionStringBuilder
            {
                DataSource = DataBaseFilePath,
                ForeignKeys = false,
                JournalMode = SQLiteJournalModeEnum.Wal
            };

            using (var conn = new SQLiteConnection(builderNoFk.ToString()))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                {
                    RebuildIntegerEnumTables(conn, tx, null, GetMainRebuildDefs(mainLegacy518));

                    ExecuteNonQuery(conn, tx, MaterialSchemaSql.Indexes);

                    tx.Commit();
                }

                //                using (var cmd = conn.CreateCommand())
                //                {
                //                    cmd.CommandText = $@"{MaterialSchemaSql.MainSchema} {ExtraSchemaCreator.GetCarrierExtraTableCommand(CarrierExtraKeys)}
                //{ExtraSchemaCreator.GetSubstrateExtraTableCommand(SubstrateExtraKeys)}
                //{MaterialSchemaSql.Indexes}";

                //                    cmd.ExecuteNonQuery();
                //                }
            }
        }
        public string GetArchiveCommand()
        {
            return $@"{ArchiveSchemaSql.ArchiveSchema} {ExtraSchemaCreator.GetArchiveCarrierExtraTableCommand(CarrierExtraKeys)}
{ExtraSchemaCreator.GetArchiveSubstrateExtraTableCommand(SubstrateExtraKeys)}
{ArchiveSchemaSql.ArchiveAtTableAndIndexes}";
        }
        //        public void EnsureArchiveSchemaWithSameTablesAndArchiveAt(SQLiteConnection conn, SQLiteTransaction tx)
        //        {
        //            using (var cmd = conn.CreateCommand())
        //            {
        //                cmd.Transaction = tx;
        //                //cmd.CommandText = ArchiveSchemaSql.ArchiveSchema;

        //                cmd.CommandText = $@"{ArchiveSchemaSql.ArchiveSchema} {ExtraSchemaBuilder.BuildArchiveCarrierExtraTable(-())}
        //{ExtraSchemaBuilder.BuildArchiveSubstrateExtraTable(GetSubstrateExtraKeys())}
        //{ArchiveSchemaSql.ArchiveAtTableAndIndexes}";

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        public string GetArchiveSubstrateExtraCommand()
        {
            var keys = string.Join(" ,", SubstrateExtraKeys);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SubstrateExtraKeys.Length; ++i)
            {
                sb.Append($"se.{SubstrateExtraKeys[i]}");
                if (i < SubstrateExtraKeys.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            var command = $@"INSERT INTO archive.SubstrateExtra( 
SubstrateKey, {keys} ) 
SELECT 
    se.SubstrateKey, {sb}
FROM SubstrateExtra AS se
JOIN Substrate AS s
    ON s.UniqueKey = se.SubstrateKey
WHERE s.CurrentCarrierKey = $key;";

            return command;
        }
        public string GetArchiveCarrierExtraCommand()
        {
            var keys = string.Join(" ,", CarrierExtraKeys);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CarrierExtraKeys.Length; ++i)
            {
                sb.Append($"c.{CarrierExtraKeys[i]}");
                if (i < CarrierExtraKeys.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            var command = $@"INSERT INTO archive.CarrierExtra ( CarrierKey, {keys} ) SELECT c.CarrierKey, {sb} FROM CarrierExtra AS c WHERE c.CarrierKey = $key;";

            return command;
        }

        //public async Task ExecuteWriteAsync(
        //    IEnumerable<Func<SQLiteConnection, SQLiteTransaction, Task>> works,
        //    CancellationToken ct)
        //{
        //    await _writeGate.WaitAsync(ct).ConfigureAwait(false);

        //    try
        //    {
        //        using (var conn = new SQLiteConnection(_connectionString))
        //        {
        //            await conn.OpenAsync(ct).ConfigureAwait(false);
        //            using (var tx = conn.BeginTransaction())
        //            {
        //                try
        //                {
        //                    foreach (var item in works)
        //                    {
        //                        await item(conn, tx).ConfigureAwait(false);
        //                    }

        //                    tx.Commit();
        //                }
        //                catch
        //                {
        //                    tx.Rollback();
        //                }
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        _writeGate.Release();
        //    }
        //}

        //public async Task ExecuteWriteAsync(
        //    Func<SQLiteConnection, SQLiteTransaction, Task> work,
        //    CancellationToken ct)
        //{
        //    await _writeGate.WaitAsync(ct).ConfigureAwait(false);
        //    try
        //    {
        //        using (var conn = new SQLiteConnection(_connectionString))
        //        {
        //            await conn.OpenAsync(ct).ConfigureAwait(false);
        //            using (var tx = conn.BeginTransaction())
        //            {
        //                await work(conn, tx).ConfigureAwait(false);
        //                tx.Commit();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        _writeGate.Release();
        //    }
        //}

        public Task ExecuteWriteAsync(Func<SQLiteConnection, SQLiteTransaction, Task> work,
            WriteExecutionMode mode = WriteExecutionMode.QueueOnly)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));

            // job.Work는 CancellationToken까지 받는 시그니처
            var job = new WriteJob(
                async (conn, tx) =>
                {
                    await work(conn, tx).ConfigureAwait(false);
                }, mode);
            job.UseTransaction = true;

            _writeQueue.Enqueue(job);
            _queueSignal.Release();

            if (mode == WriteExecutionMode.QueueOnly)
                return Task.CompletedTask;

            return job.Tcs.Task;
        }

        public Task ExecuteWriteAsync(Func<SQLiteConnection, Task> work,
            WriteExecutionMode mode = WriteExecutionMode.QueueOnly)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));

            // job.Work는 CancellationToken까지 받는 시그니처
            var job = new WriteJob(
                async (conn, tx) =>
                {
                    await work(conn).ConfigureAwait(false);
                }, mode);
            job.UseTransaction = false;

            _writeQueue.Enqueue(job);
            _queueSignal.Release();

            if (mode == WriteExecutionMode.QueueOnly)
                return Task.CompletedTask;

            return job.Tcs.Task;
        }

        public Task ExecuteWriteAsync(IEnumerable<Func<SQLiteConnection, SQLiteTransaction, Task>> works,
            WriteExecutionMode mode = WriteExecutionMode.QueueOnly)
        {
            if (works == null) throw new ArgumentNullException(nameof(works));

            var workList = works.ToList();

            var job = new WriteJob(
                async (conn, tx) =>
                {
                    foreach (var w in workList)
                    {
                        await w(conn, tx).ConfigureAwait(false);
                    }
                }, mode);
            job.UseTransaction = true;

            _writeQueue.Enqueue(job);
            _queueSignal.Release();

            if (mode == WriteExecutionMode.QueueOnly)
                return Task.CompletedTask;

            return job.Tcs.Task;
        }

        // 공통 읽기 헬퍼
        public async Task<TResult> ExecuteReadAsync<TResult>(
            Func<SQLiteConnection, Task<TResult>> work)
        {
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    await conn.OpenAsync().ConfigureAwait(false);

                    return await work(conn).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return default(TResult);
            }
        }

        //public async Task AddLocations(
        //    IEnumerable<Tuple<string, string, List<string>, List<int>>> data)
        //{
        //    using (var conn = new SQLiteConnection(_connectionString))
        //    {
        //        conn.Open();
        //        using (var tx = conn.BeginTransaction())
        //        {
        //            foreach (var item in data)
        //            {
        //                var txt = item.Item1;
        //                var name = item.Item2;
        //                var keys = item.Item3;
        //                var values = item.Item4;

        //                using (var cmd = conn.CreateCommand())
        //                {
        //                    cmd.Transaction = tx;
        //                    cmd.CommandText = txt;
        //                    cmd.Parameters.AddWithValue("$name", name);

        //                    for (int i = 0; i < keys.Count; ++i)
        //                    {
        //                        var key = keys[i];
        //                        var value = values[i];

        //                        cmd.Parameters.AddWithValue(key, value);
        //                    }

        //                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        //                }
        //            }

        //            tx.Commit();
        //        }
        //    }
        //}

        //public async Task ExecuteRecordingHistory(string command,
        //    IReadOnlyList<string> keys, IReadOnlyList<string> values)
        //{
        //    using (var conn = new SQLiteConnection(_connectionString))
        //    {
        //        conn.Open();

        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = command;

        //            for (int i = 0; i < keys.Count; ++i)
        //            {
        //                var key = keys[i];
        //                object value;
        //                if (string.IsNullOrWhiteSpace(values[i]))
        //                {
        //                    value = DBNull.Value;
        //                }
        //                else
        //                {
        //                    value = (object)values[i];
        //                }

        //                cmd.Parameters.AddWithValue(key, value);
        //            }

        //            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        //        }
        //    }
        //}

        public void Dispose()
        {
            _disposed = true;
        }
    }

    namespace DatabaseOnly
    {
        using System;
        using System.Globalization;
        using System.IO;

        static class DbUtil
        {
            public static string ToIsoString(DateTime dt)
                => dt.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
            //=> dt.ToString("o", CultureInfo.InvariantCulture);

            public static DateTime FromIsoString(string s)
                => DateTime.Parse(
                    s,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

            //=> DateTime.Parse(s, null,
            //    DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal);

            public static int BoolToInt(bool v) => v ? 1 : 0;

            public static bool IntToBool(long v) => v != 0;

            public static void ValidateKey(string key, string paramName)
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Key is required.", paramName);
                if (key.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    throw new ArgumentException($"Invalid key: '{key}'", paramName);
            }
        }
    }
}