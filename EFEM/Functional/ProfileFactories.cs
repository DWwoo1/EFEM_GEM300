using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineEnumProject.AppConfig;

using EFEM.MaterialTracking;
using EFEM.MaterialTracking.LocationStorage;
using EFEM.MaterialTracking.LocationHistory.Storage;
using EFEM.MaterialTracking.CarrierStorage;
using EFEM.MaterialTracking.ProcessingHistory.Storage;
using EFEM.MaterialTracking.SubstrateStorage;
using EFEM.MaterialTracking.LocationState;
using EFEM.MaterialTracking.LocationHistory;
using EFEM.MaterialTracking.ProcessingHistory;

namespace FrameOfSystem3.Functional
{
    #region <Material Extra Attribute>
    public static class MaterialExtraAttributeFactory
    {
        public static (IMaterialExtraAttribute Substrate, IMaterialExtraAttribute Carrier) Create(EN_PROCESS_TYPE processType)
        {
            switch (processType)
            {
                case EN_PROCESS_TYPE.BIN_SORTER:
                    return (new EFEM.CustomizedByProcessType.PWA500BIN.PWA500BINSubstrateExtraAttributes(), new EFEM.CustomizedByProcessType.PWA500Common.PWA500CarrierExtraAttributes());
                case EN_PROCESS_TYPE.DIE_TRANSFER:
                case EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    return (new EFEM.CustomizedByProcessType.PWA500W.PWA500WSubstrateExtraAttributes(), new EFEM.CustomizedByProcessType.PWA500Common.PWA500CarrierExtraAttributes());
                case EN_PROCESS_TYPE.NONE:
                default:
                    return (new NullProfile(), new NullProfile());
            }
        }
    }
    public sealed class NullProfile : IMaterialExtraAttribute
    {
        public IEnumerable<string> GetExtraKeys()
        {
            return null;
        }
        public void CreateAttributes(Dictionary<string, string> extra) { }
        public void InitializeToPublish(Dictionary<string, string> extra, IMaterial material) { }
    }
    #endregion </Material Extra Attribute>

    #region <MaterialTrackingStorageFactory>
    public sealed class MaterialTrackingStorageContext
    {
        public MaterialTrackingStorageContext(
            ISubstrateLocationHistoryStorage locationHistory,
            ISubstrateProcessingHistoryStorage processingHistory,
            ISubstrateStorage substrate,
            ICarrierStorage carrier,
            ILocationStorage location,
            Func<DateTime> clock)
        {
            LocationHistory = locationHistory;
            ProcessingHistory = processingHistory;
            Substrate = substrate;
            Carrier = carrier;
            Clock = clock;
        }

        public ISubstrateLocationHistoryStorage LocationHistory { get; }
        public ISubstrateProcessingHistoryStorage ProcessingHistory { get; }
        public ISubstrateStorage Substrate { get; }
        public ICarrierStorage Carrier { get; }
        public Func<DateTime> Clock { get; }
        public ILocationStorage Location { get; }
    }

    public interface IMaterialTrackingStorageContextFactory
    {
        MaterialTrackingStorageContext Create(
            Func<IEnumerable<string>> funcForCarrier,
            Func<IEnumerable<string>> funcForSubstrate,
            IEnumerable<EFEM.Database.IDbMigrationStep> migrationSteps);
        void ShutDown();
    }
    public sealed class SqliteMaterialTrackingStorageContextFactory : IMaterialTrackingStorageContextFactory
    {
        private EFEM.Database.MaterialDbContext _databaseContext;
        private readonly string _dbFilePath;
        private readonly Func<DateTime> _clock;

        public SqliteMaterialTrackingStorageContextFactory(
            string dbFilePath,
            Func<DateTime> clock)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentNullException(nameof(dbFilePath));

            _dbFilePath = dbFilePath;
            _clock = clock ?? (() => DateTime.Now);
        }

        public MaterialTrackingStorageContext Create(
            Func<IEnumerable<string>> funcForCarrier,
            Func<IEnumerable<string>> funcForSubstrate,
            IEnumerable<EFEM.Database.IDbMigrationStep> migrationSteps)
        {
            // 1) DB 컨텍스트 생성 (스키마/트리거 포함)
            _databaseContext = new EFEM.Database.MaterialDbContext(
                _dbFilePath, EFEM.Defines.Common.AsyncLoggerForEfem.Instance.WriteDebugLog, funcForCarrier, funcForSubstrate, migrationSteps);

            // 2) 저장소들 생성
            var stayHistory = new SqliteSubstrateLocationHistoryStorage(_databaseContext);
            var procHistory = new SqliteSubstrateProcessingHistoryStorage(_databaseContext);
            var substrate = new SqliteSubstrateStorage(_databaseContext);
            var carrier = new SqliteCarrierStorage(_databaseContext);
            var location = new SqliteLocationStorage(_databaseContext);
            LocationService.ConfigureService(location);

            // 4) 컨텍스트 조립
            return new MaterialTrackingStorageContext(
                stayHistory,
                procHistory,
                substrate,
                carrier,
                location,
                _clock);
        }

        public async void ShutDown()
        {
            await _databaseContext.ShutdownAsync();
        }
    }

    public sealed class JsonMaterialTrackingStorageContextFactory : IMaterialTrackingStorageContextFactory
    {
        private readonly string _pathForLocationHistory;
        private readonly string _pathForProcessingHistory;
        private readonly string _pathForSubstrate;
        private readonly string _pathForCarrier;
        private readonly string _pathForLocation;

        public JsonMaterialTrackingStorageContextFactory(
            string pathForLocationHistory,
            string pathForProcessingHistory,
            string pathForSubstrate,
            string pathForCarrier,
            string pathForLocation)
        {
            _pathForLocationHistory = pathForLocationHistory;
            _pathForProcessingHistory = pathForProcessingHistory;
            _pathForSubstrate = pathForSubstrate;
            _pathForCarrier = pathForCarrier;
            _pathForLocation = pathForLocation;
        }

        public MaterialTrackingStorageContext Create(
            Func<IEnumerable<string>> funcForCarrier,
            Func<IEnumerable<string>> funcForSubstrate,
            IEnumerable<EFEM.Database.IDbMigrationStep> migrationSteps)
        {
            var location = new JsonLocationStorage(_pathForLocation);
            LocationService.ConfigureService(location);

            return new MaterialTrackingStorageContext(
                new JsonSubstrateLocationHistoryStorage(_pathForLocationHistory),
                new JsonSubstrateProcessingHistoryStorage(_pathForProcessingHistory),
                new JsonSubstrateStorage(_pathForSubstrate),
                new JsonCarrierStorage(_pathForCarrier),
                location,
                () => DateTime.Now);
        }
        public void ShutDown() { }
    }

    public sealed class JsonAndSqliteMaterialTrackingStorageContextFactory : IMaterialTrackingStorageContextFactory
    {
        private EFEM.Database.MaterialDbContext _databaseContext;
        private readonly string _dbFilePath;
        private readonly Func<DateTime> _clock;
        private readonly string _pathForLocationHistory;
        private readonly string _pathForProcessingHistory;
        private readonly string _pathForSubstrate;
        private readonly string _pathForCarrier;

        public JsonAndSqliteMaterialTrackingStorageContextFactory(
            string pathForLocationHistory,
            string pathForProcessingHistory,
            string pathForSubstrate,
            string pathForCarrier,
            string dbFilePath,
            Func<DateTime> clock)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentNullException(nameof(dbFilePath));

            _pathForLocationHistory = pathForLocationHistory;
            _pathForProcessingHistory = pathForProcessingHistory;
            _pathForSubstrate = pathForSubstrate;
            _pathForCarrier = pathForCarrier;

            _dbFilePath = dbFilePath;
            _clock = clock ?? (() => DateTime.Now);
        }

        public MaterialTrackingStorageContext Create(
            Func<IEnumerable<string>> funcForCarrier,
            Func<IEnumerable<string>> funcForSubstrate,
            IEnumerable<EFEM.Database.IDbMigrationStep> migrationSteps)
        {
            // 외부에서 주입되도록 변경
            //var extraMigrations = new List<EFEM.Database.IDbMigrationStep>
            //{
            //    new EFEM.Database.DelegateDbMigrationStep(
            //        2,
            //        "Rename CarrierExtra.KeyLotQty to LotQty",
            //        (conn, tx, schemaName) =>
            //        {
            //            const string tableNameOnly = "CarrierExtra";
            //            const string oldColumnName = "KeyLotQty";
            //            const string newColumnName = "LotQty";

            //            string qualifiedTableName = string.IsNullOrWhiteSpace(schemaName)
            //                ? "\"" + tableNameOnly + "\""
            //                : "\"" + schemaName + "\".\"" + tableNameOnly + "\"";

            //            bool tableExists;
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.Transaction = tx;
            //                cmd.CommandText =
            //                    "SELECT 1 FROM "
            //                    + (string.IsNullOrWhiteSpace(schemaName)
            //                        ? "sqlite_master"
            //                        : "\"" + schemaName + "\".sqlite_master")
            //                    + " WHERE type = 'table' AND name = $name LIMIT 1;";
            //                cmd.Parameters.AddWithValue("$name", tableNameOnly);

            //                var result = cmd.ExecuteScalar();
            //                tableExists = result != null && result != DBNull.Value;
            //            }

            //            if (!tableExists)
            //                return;

            //            bool hasOldColumn = false;
            //            bool hasNewColumn = false;

            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.Transaction = tx;
            //                cmd.CommandText = string.IsNullOrWhiteSpace(schemaName)
            //                    ? "PRAGMA table_info(\"" + tableNameOnly + "\");"
            //                    : "PRAGMA \"" + schemaName + "\".table_info(\"" + tableNameOnly + "\");";

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                    {
            //                        var currentName = Convert.ToString(reader["name"]);

            //                        if (string.Equals(currentName, oldColumnName, StringComparison.OrdinalIgnoreCase))
            //                            hasOldColumn = true;

            //                        if (string.Equals(currentName, newColumnName, StringComparison.OrdinalIgnoreCase))
            //                            hasNewColumn = true;
            //                    }
            //                }
            //            }

            //            if (hasOldColumn && !hasNewColumn)
            //            {
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText =
            //                        "ALTER TABLE " + qualifiedTableName +
            //                        " RENAME COLUMN " + oldColumnName + " TO " + newColumnName + ";";
            //                    cmd.ExecuteNonQuery();
            //                }

            //                return;
            //            }

            //            if (hasOldColumn && hasNewColumn)
            //            {
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.Transaction = tx;
            //                    cmd.CommandText =
            //                        "UPDATE " + qualifiedTableName + " " +
            //                        "SET LotQty = COALESCE(LotQty, KeyLotQty) " +
            //                        "WHERE LotQty IS NULL;";
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //        })
            //};

            // 1) DB 컨텍스트 생성 (스키마/트리거 포함)
            _databaseContext = new EFEM.Database.MaterialDbContext(
                _dbFilePath,
                EFEM.Defines.Common.AsyncLoggerForEfem.Instance.WriteDebugLog,
                funcForCarrier,
                funcForSubstrate,
                migrationSteps);

            // 2) 저장소들 생성
            var stayHistory = new JsonAndSqliteSubstrateLocationHistoryStorage(
                _pathForLocationHistory, 6,
                _databaseContext);
            var procHistory = new JsonAndSqliteSubstrateProcessingHistoryStorage(
                _pathForProcessingHistory, 6,
                _databaseContext);
            var substrate = new JsonAndSqliteSubstrateStorage(
                _pathForSubstrate, 6,
                _databaseContext);
            var carrier = new JsonAndSqliteCarrierStorage(
                _pathForCarrier, 6,
                _databaseContext);
            var location = new SqliteLocationStorage(_databaseContext);
            LocationService.ConfigureService(location);

            // 4) 컨텍스트 조립
            return new MaterialTrackingStorageContext(
                stayHistory,
                procHistory,
                substrate,
                carrier,
                location,
                _clock);
        }

        public async void ShutDown()
        {
            await _databaseContext.ShutdownAsync();
        }
    }
    #endregion </MaterialTrackingStorageFactory>

    #region <SubstrateHistoryServicesFactory>
    public static class SubstrateHistoryServicesFactory
    {
        private static SubstrateHistoryTracker _locationHistoryTracker;
        private static LocationStateService _locationStateService;
        private static SubstrateProcessingService _substrateProcessingService;
        public static (LocationStateService locationStateService, SubstrateProcessingService substrateProcessingService) Create(
            ISubstrateLocationHistoryStorage historyStorage,
            ISubstrateProcessingHistoryStorage processingStorage,
            Func<DateTime> clock,
            ILocationEvent locationEvent)
        {
            if (historyStorage == null) throw new ArgumentNullException(nameof(historyStorage));
            if (processingStorage == null) throw new ArgumentNullException(nameof(processingStorage));
            if (clock == null) throw new ArgumentNullException(nameof(clock));

            _locationHistoryTracker = new SubstrateHistoryTracker(historyStorage);
            var locEvent = locationEvent ?? new NullLocationEvent();

            _locationStateService = new LocationStateService(_locationHistoryTracker, locEvent, clock);
            _substrateProcessingService = new SubstrateProcessingService(processingStorage, clock);

            return (_locationStateService, _substrateProcessingService);
        }

        // TODO : 변경 필요
        public static void AddOrUpdateLocations(IEnumerable<LocationItem> items)
        {
            //_locationStateService.AddOrUpdateLocations(items);
        }
        public static ISubstrateEventObserver HistoryTracker => _locationHistoryTracker;
    }
    #endregion </SubstrateHistoryServicesFactory>
}

namespace EFEM.Migrations
{
    public static class MigrationSteps
    {
        public static IEnumerable<Database.IDbMigrationStep> GetMigrationSteps()
        {
            var extraMigrations = new List<EFEM.Database.IDbMigrationStep>
            {
                new Database.DelegateDbMigrationStep(
                    2,
                    "Rename Location.Name to Id and add Location.Name",
                    (conn, tx, schemaName) =>
                    {
                        const string tableNameOnly = "Location";

                        const string oldColumnName = "Name";
                        const string newColumnName = "Id";

                        string qualifiedTableName = string.IsNullOrWhiteSpace(schemaName)
                            ? "\"" + tableNameOnly + "\""
                            : "\"" + schemaName + "\".\"" + tableNameOnly + "\"";

                        bool tableExists;
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText =
                                "SELECT 1 FROM "
                                + (string.IsNullOrWhiteSpace(schemaName)
                                    ? "sqlite_master"
                                    : "\"" + schemaName + "\".sqlite_master")
                                + " WHERE type = 'table' AND name = $name LIMIT 1;";
                            cmd.Parameters.AddWithValue("$name", tableNameOnly);

                            var result = cmd.ExecuteScalar();
                            tableExists = result != null && result != DBNull.Value;
                        }

                        if (!tableExists)
                            return;

                        bool hasOldNameColumn = false;
                        bool hasIdColumn = false;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = string.IsNullOrWhiteSpace(schemaName)
                                ? "PRAGMA table_info(\"" + tableNameOnly + "\");"
                                : "PRAGMA \"" + schemaName + "\".table_info(\"" + tableNameOnly + "\");";

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var currentName = Convert.ToString(reader["name"]);

                                    if (string.Equals(currentName, oldColumnName, StringComparison.OrdinalIgnoreCase))
                                        hasOldNameColumn = true;

                                    if (string.Equals(currentName, newColumnName, StringComparison.OrdinalIgnoreCase))
                                        hasIdColumn = true;
                                }
                            }
                        }
                    
                        /*
                            * 기존 Location.Name은 식별자 역할이므로 Id로 변경한다.
                            *
                            * 기존:
                            *   Name TEXT PRIMARY KEY
                            *
                            * 변경:
                            *   Id   TEXT PRIMARY KEY
                            */
                        if (hasOldNameColumn && !hasIdColumn)
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "ALTER TABLE " + qualifiedTableName +
                                    " RENAME COLUMN \"" + oldColumnName + "\" TO \"" + newColumnName + "\";";
                                cmd.ExecuteNonQuery();
                            }

                            hasOldNameColumn = false;
                            hasIdColumn = true;
                        }
                    
                        /*
                            * Id 변경 후, 표시용 Name 컬럼을 새로 추가한다.
                            * TEXT만 지정하므로 NULL 허용 컬럼이다.
                            */
                        if (!hasOldNameColumn)
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "ALTER TABLE " + qualifiedTableName +
                                    " ADD COLUMN \"" + oldColumnName + "\" TEXT;";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }),
                new Database.DelegateDbMigrationStep(
                    3,
                    "Add Substrate.OriginName and SubstrateExtra.ScrapInfo",
                    (conn, tx, schemaName) =>
                    {
                        Func<string, string, bool> addTextColumnIfMissing = (tableNameOnly, columnName) =>
                        {
                            string qualifiedTableName = string.IsNullOrWhiteSpace(schemaName)
                                ? "\"" + tableNameOnly + "\""
                                : "\"" + schemaName + "\".\"" + tableNameOnly + "\"";

                            bool tableExists;
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "SELECT 1 FROM "
                                    + (string.IsNullOrWhiteSpace(schemaName)
                                        ? "sqlite_master"
                                        : "\"" + schemaName + "\".sqlite_master")
                                    + " WHERE type = 'table' AND name = $name LIMIT 1;";
                                cmd.Parameters.AddWithValue("$name", tableNameOnly);

                                var result = cmd.ExecuteScalar();
                                tableExists = result != null && result != DBNull.Value;
                            }

                            if (!tableExists)
                                return false;

                            bool hasColumn = false;
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText = string.IsNullOrWhiteSpace(schemaName)
                                    ? "PRAGMA table_info(\"" + tableNameOnly + "\");"
                                    : "PRAGMA \"" + schemaName + "\".table_info(\"" + tableNameOnly + "\");";

                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var currentName = Convert.ToString(reader["name"]);
                                        if (string.Equals(currentName, columnName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            hasColumn = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!hasColumn)
                            {
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.Transaction = tx;
                                    cmd.CommandText =
                                        "ALTER TABLE " + qualifiedTableName +
                                        " ADD COLUMN \"" + columnName + "\" TEXT;";
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            return true;
                        };

                        if (addTextColumnIfMissing("Substrate", "OriginName"))
                        {
                            string qualifiedTableName = string.IsNullOrWhiteSpace(schemaName)
                                ? "\"Substrate\""
                                : "\"" + schemaName + "\".\"Substrate\"";

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "UPDATE " + qualifiedTableName + " " +
                                    "SET \"OriginName\" = COALESCE(\"OriginName\", \"Name\") " +
                                    "WHERE \"OriginName\" IS NULL;";
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (addTextColumnIfMissing("SubstrateExtra", "ScrapInfo"))
                        {
                            string qualifiedTableName = string.IsNullOrWhiteSpace(schemaName)
                                ? "\"SubstrateExtra\""
                                : "\"" + schemaName + "\".\"SubstrateExtra\"";

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "UPDATE " + qualifiedTableName + " " +
                                    "SET \"ScrapInfo\" = '' " +
                                    "WHERE \"ScrapInfo\" IS NULL;";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }),
                new Database.DelegateDbMigrationStep(
                    4,
                    "Rename CarrierExtra.KeyLotQty to LotQty",
                    (conn, tx, schemaName) =>
                    {
                        const string tableNameOnly = "CarrierExtra";
                        const string oldColumnName = "KeyLotQty";
                        const string newColumnName = "LotQty";

                        string qualifiedTableName = string.IsNullOrWhiteSpace(schemaName)
                            ? "\"" + tableNameOnly + "\""
                            : "\"" + schemaName + "\".\"" + tableNameOnly + "\"";

                        bool tableExists;
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText =
                                "SELECT 1 FROM "
                                + (string.IsNullOrWhiteSpace(schemaName)
                                    ? "sqlite_master"
                                    : "\"" + schemaName + "\".sqlite_master")
                                + " WHERE type = 'table' AND name = $name LIMIT 1;";
                            cmd.Parameters.AddWithValue("$name", tableNameOnly);

                            var result = cmd.ExecuteScalar();
                            tableExists = result != null && result != DBNull.Value;
                        }

                        if (!tableExists)
                            return;

                        bool hasOldColumn = false;
                        bool hasNewColumn = false;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = string.IsNullOrWhiteSpace(schemaName)
                                ? "PRAGMA table_info(\"" + tableNameOnly + "\");"
                                : "PRAGMA \"" + schemaName + "\".table_info(\"" + tableNameOnly + "\");";

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var currentName = Convert.ToString(reader["name"]);

                                    if (string.Equals(currentName, oldColumnName, StringComparison.OrdinalIgnoreCase))
                                        hasOldColumn = true;

                                    if (string.Equals(currentName, newColumnName, StringComparison.OrdinalIgnoreCase))
                                        hasNewColumn = true;
                                }
                            }
                        }

                        if (hasOldColumn && !hasNewColumn)
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "ALTER TABLE " + qualifiedTableName +
                                    " RENAME COLUMN " + oldColumnName + " TO " + newColumnName + ";";
                                cmd.ExecuteNonQuery();
                            }

                            return;
                        }

                        if (hasOldColumn && hasNewColumn)
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandText =
                                    "UPDATE " + qualifiedTableName + " " +
                                    "SET LotQty = COALESCE(LotQty, KeyLotQty) " +
                                    "WHERE LotQty IS NULL;";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }),
                };

            return extraMigrations;
        }
    }
}