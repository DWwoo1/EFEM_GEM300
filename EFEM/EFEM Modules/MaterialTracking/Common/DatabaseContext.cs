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
    public enum DataBaseVersion
    {
        BaseSchema = 1,
        CarrierExtraChanged = 2,    // ExtraAttribute 중 "KeyLotQty" -> "LotQty"로 변경
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
    LocationKind INTEGER NOT NULL,
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
    AccessStatus  INTEGER NOT NULL DEFAULT 0,
    Capacity      INTEGER NOT NULL DEFAULT 0,
    LoadTime      TEXT,
    UnloadTime    TEXT
);
";
            const string CarrierSlotMapTable = @"
CREATE TABLE IF NOT EXISTS CarrierSlotMap (
    CarrierKey  TEXT    NOT NULL,
    SlotNo      INTEGER NOT NULL,
    MapValue    INTEGER NOT NULL,
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
    TransportStatus     INTEGER,
    ProcessingStatus    INTEGER,
    IdReadingStatus     INTEGER,
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
    FromLocationKind INTEGER,
    ToLocationName   TEXT    NULL,
    ToLocationKind   INTEGER,
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
                + ProcessingHistoryTable;
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
    AccessStatus  INTEGER,
    Capacity      INTEGER,
    LoadTime      TEXT,
    UnloadTime    TEXT
);
";
            const string CarrierSlotMapTable = @"
CREATE TABLE IF NOT EXISTS archive.CarrierSlotMap (
    CarrierKey  TEXT    NOT NULL,
    SlotNo      INTEGER NOT NULL,
    MapValue    INTEGER NOT NULL,
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
    TransportStatus     INTEGER,
    ProcessingStatus    INTEGER,
    IdReadingStatus     INTEGER,
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
    FromLocationKind INTEGER,
    ToLocationName   TEXT    NULL,
    ToLocationKind   INTEGER,
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
                + ProcessingHistoryTable;
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

            foreach (var migration in _migrationSteps)
            {
                if (currentVersion >= migration.Version)
                    continue;

                migration.Apply(conn, tx, schemaName);

                currentVersion = migration.Version;
                setVersion(conn, tx, currentVersion);

                WriteLog("[DB Migration] " + GetDisplayTableName(schemaName, "Schema")
                    + " upgraded to version " + currentVersion
                    + " (" + migration.Name + ").");
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

        #region <Migration>
        internal void EnsureArchiveSchemaAndMigrate(SQLiteConnection conn, SQLiteTransaction tx)
        {
            ExecuteNonQuery(conn, tx, GetArchiveCommand());
            ApplyArchiveSchemaMigrations(conn, tx);
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
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                {
                    EnsureMainSchema(conn, tx);

                    ApplySchemaMigrations(conn, tx);

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