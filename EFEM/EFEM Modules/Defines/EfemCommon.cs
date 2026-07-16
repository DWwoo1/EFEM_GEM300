using System;
using System.Threading;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Schema;
using System.IO.Compression;
using System.Threading.Tasks;

using FrameOfSystem3.Work;
using Define.DefineEnumProject.AppConfig;

namespace EFEM.Defines.Common
{
    #region <Class>   
    public class CommandResults
    {
        public CommandResults(string actionName,
            CommandResult result,
            string description = null)
        {
            ActionName = actionName;
            CommandResult = result;

            if (description == null)
                Description = string.Empty;
            else
                Description = description;
        }

        public string ActionName { get; set; }
        public CommandResult CommandResult { get; set; }
        public string Description { get; set; }
        public int AlarmCode { get; set; }
    }

    //public class LocationName
    //{
    //    #region <Constructors>
    //    private LocationName()
    //    {
    //        _locationNames = (string[])Enum.GetValues(GetLocationType());
    //    }
    //    #endregion </Constructors>

    //    #region <Fields>
    //    private static LocationName _inatance = null;
    //    private readonly string[] _locationNames = null;
    //    #endregion </Fields>

    //    #region <Properties>
    //    public static LocationName Instance
    //    {
    //        get
    //        {
    //            if (_inatance == null)
    //                _inatance = new LocationName();

    //            return _inatance;
    //        }
    //    }

    //    public string[] LocationNames
    //    {
    //        get
    //        {
    //            return _locationNames;
    //        }
    //    }
    //    #endregion </Properties>

    //    #region <Methods>
    //    private Type GetLocationType()
    //    {
    //        switch (FrameOfSystem3.AppConfig.AppConfigManager.Instance.ProcessType)
    //        {
    //            case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
    //                return typeof(PWA500BINLocations);
    //        }

    //        return null;
    //    }
    //    #endregion </Methods>
    //}
    public static class RecoveryFileDefines
    {
        public const string FileExtension = "xml";
        public const string FileRootName = "SubstrateInformation";
        public static readonly string RecoveryDatabasePath = Path.Combine(Environment.CurrentDirectory, @"..\Recovery\DB");
        public static readonly string RecoveryFilePath = Path.Combine(Environment.CurrentDirectory, @"..\Recovery\Substrates");
        public static readonly string CarrierRecoveryFilePath = Path.Combine(Environment.CurrentDirectory, @"..\Recovery\Carriers");
        public static readonly string LoadPortRecoveryFilePath = Path.Combine(Environment.CurrentDirectory, @"..\Recovery\LP");
        public static readonly string JobRecoveryFilePath = Path.Combine(Environment.CurrentDirectory, @"..\Recovery\Job");
        public static readonly string LocationHistoryPath = Path.Combine(RecoveryFilePath, "Location");
        public static readonly string ProcessingHistoryPath = Path.Combine(RecoveryFilePath, "Processing");
        public const string LocationTypeKey = "LocationType";
        public const string LocationTypeLoadPort = "LocationTypeLoadPort";
        public const string LocationTypeProcessModule = "LocationTypeProcessModule";
        public const string LocationTypeRobot = "LocationTypeRobot";
        public const string LocationTypeUnknown = "LocationTypeUnknown";

        public const string LoadPortLocationLoadPortName = "LoadPortLocationLoadPortName";
        public const string LoadPortLocationPortId = "LoadPortLocationPortId";
        public const string LoadPortLocationSlot = "LoadPortLocationSlot";

        public const string ProcessModuleLocationProcessModuleName = "ProcessModuleLocationProcessModuleName";

        public const string RobotLocationRobotName = "RobotLocationRobotName";
        public const string RobotLocationArm = "RobotLocationArm";
    }
    
    public sealed class MonitorKeyedLocker : IDisposable
    {
        private sealed class Entry { public readonly object Sync = new object(); public int RefCount; }

        private readonly ConcurrentDictionary<string, Entry> _locks =
            new ConcurrentDictionary<string, Entry>(StringComparer.Ordinal);
        private volatile bool _disposed;

        public IDisposable Acquire(string key)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(MonitorKeyedLocker));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var e = _locks.GetOrAdd(key, _ => new Entry());
            Interlocked.Increment(ref e.RefCount);

            bool taken = false;
            try
            {
                System.Threading.Monitor.Enter(e.Sync, ref taken);
                return new Releaser(this, key, e);
            }
            catch
            {
                if (taken) System.Threading.Monitor.Exit(e.Sync);
                if (Interlocked.Decrement(ref e.RefCount) == 0)
                    TryCleanup(key, e);
                throw;
            }
        }

        private sealed class Releaser : IDisposable
        {
            private readonly MonitorKeyedLocker _owner;
            private readonly string _key;
            private readonly Entry _e;
            private int _done;

            public Releaser(MonitorKeyedLocker owner, string key, Entry e)
            { _owner = owner; _key = key; _e = e; }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _done, 1) != 0) return;
                System.Threading.Monitor.Exit(_e.Sync);
                if (Interlocked.Decrement(ref _e.RefCount) == 0)
                    _owner.TryCleanup(_key, _e);
            }
        }

        private void TryCleanup(string key, Entry e)
        {
            Entry cur;
            if (!_locks.TryGetValue(key, out cur)) return;
            if (!object.ReferenceEquals(cur, e)) return;

            Entry removed;
            if (_locks.TryRemove(key, out removed))
            {
                if (!object.ReferenceEquals(removed, e))
                {
                    _locks.TryAdd(key, removed); // 되돌림
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
            _locks.Clear();
        }
    }
    public static class BaseLogTypes
    {
        public const string LogTypeDebug = "Debug";
        public const string LogTypeLoadPort = "LoadPort";
        public const string LogTypeAtmRobot = "AtmRobot";
        public const string LogTypeProcessModule = "ProcessModule";
        public const string LogTypeSecsGem = "SecsGem";
    }

    public class LogInfo
    {
        public StreamWriter StreamWriter;
        public string LogDirectory;
        public DateTime LastBackupDate;
        public bool UseCleanup;

        public string CurrentFilePath;
        public long MaxFileSizeBytes;
    }
    public class AsyncLoggerForEfem
    {
        #region <Constructors>
        private AsyncLoggerForEfem()
        {
            _basePath = Path.Combine(Define.DefineConstant.FilePath.FILEPATH_LOG, "EFEM");

            _logInfos = new ConcurrentDictionary<string, LogInfo>();
            _logQueues = new ConcurrentDictionary<string, ConcurrentQueue<Tuple<DateTime, string>>>();

            // 로그 파일 경로가 존재하지 않으면 생성
            if (false == Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }

            RegisterLogType(BaseLogTypes.LogTypeDebug, BaseLogTypes.LogTypeDebug, true);

            _writeLogTask = ProcessLogsAsync();
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly string _basePath;

        private volatile bool _isExiting = false;
        //private string _temporaryPath = string.Empty;
        //private string _temporaryDir = string.Empty;

        //private static AsyncLoggerForEFEM _instance = null;
        private static readonly Lazy<AsyncLoggerForEfem> _lazy = new Lazy<AsyncLoggerForEfem>(() => new AsyncLoggerForEfem());

        private readonly Task _writeLogTask;

        //private const int BackupHour = 22;
        
        private const string BackupFolderName = "Backup";

        // 보존 정책: 어제/오늘 유지 → 이틀 전까지 정리
        private const int RetentionKeepDays = 2; // today - 2일 이전은 정리
        // TODO : 임시
        private const long DefaultMaxFileSizeBytes = 10L * 1024 * 1024; // 50MB

        private readonly ConcurrentDictionary<string, LogInfo> _logInfos;
        //private readonly Dictionary<string, StreamWriter> StreamWriters;
        //private readonly Dictionary<string, string> LogDirectories;
        //private readonly Dictionary<string, DateTime> LastBackupDate;
        //private readonly Dictionary<string, bool> UseCleanUp;
        private readonly ConcurrentDictionary<string, ConcurrentQueue<Tuple<DateTime, string>>> _logQueues;
        #endregion </Fields>

        #region <Properties>
        public static AsyncLoggerForEfem Instance
        {
            get
            {
                return _lazy.Value;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <External>
        public void RegisterLogType(string logType, string filePath, bool useCleanup)
        {
            if (_logInfos.ContainsKey(logType))
                return;

            _logQueues.TryAdd(logType, new ConcurrentQueue<Tuple<DateTime, string>>());

            var info = new LogInfo
            {
                LogDirectory = Path.Combine(_basePath, filePath),
                LastBackupDate = DateTime.MinValue,
                UseCleanup = useCleanup,
                StreamWriter = null,
                MaxFileSizeBytes = DefaultMaxFileSizeBytes
            };

            Directory.CreateDirectory(info.LogDirectory);
            _logInfos[logType] = info; // ConcurrentDictionary
        }
        public void EnqueueLog(string logType, string message)
        {
            if (_isExiting) 
                return;

            var logEntry = Tuple.Create(DateTime.Now, message);

            if (false == _logQueues.TryGetValue(logType, out var q))
            {
                RegisterLogType(logType, logType, useCleanup: true);
                q = _logQueues[logType];
            }
            q.Enqueue(logEntry);
        }
        public void WriteDebugLog(string message)
        {
            EnqueueLog(BaseLogTypes.LogTypeDebug, message);
        }
        public async Task ExitAsync()
        {
            _isExiting = true;

            await WaitForCompletion();

            //CloseStreamWritersAll();
        }
        #endregion </External>

        #region <Internal>
        private void CreateLogFilePath(string logType, DateTime date, out string logFilePath)
        {
            if (false == _logInfos.TryGetValue(logType, out var info))
                info = _logInfos[BaseLogTypes.LogTypeDebug];

            var dir = info.LogDirectory;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            logFilePath = Path.Combine(dir, $"{date:yyyyMMdd}.txt");
        }

        private string BuildHourlyRotatedPath(string basePath, DateTime now)
        {
            var dir = Path.GetDirectoryName(basePath);
            var baseName = Path.GetFileNameWithoutExtension(basePath); // yyyyMMdd
            var ext = Path.GetExtension(basePath);                     // .txt

            var candidate = Path.Combine(dir, $"{baseName}_{now:HH}{ext}");
            if (false == File.Exists(candidate))
                return candidate;

            // 같은 시간대에 여러 번 넘칠 경우 충돌 방지
            int idx = 1;
            while (true)
            {
                var alt = Path.Combine(dir, $"{baseName}_{now:HH}_{idx}{ext}");
                if (false == File.Exists(alt)) return alt;
                idx++;
            }
        }
        private void CreateStreamWriter(string logType, string filePath)
        {
            if (false == _logInfos.TryGetValue(logType, out var info))
                return;

            var fs = new FileStream(
                filePath,
                FileMode.Append,
                FileAccess.Write,
                FileShare.Read // <- 중요: 외부에서 읽기 허용
            );

            info.StreamWriter = new StreamWriter(fs) { AutoFlush = true };
            info.CurrentFilePath = filePath; // 현재 파일 경로 기억
        }
        private void CloseStreamWritersAll()
        {
            foreach (var item in _logInfos)
            {
                CloseStreamWriter(item.Key);
            }
        }
        private void CloseStreamWriter(string logType)
        {
            if (false == _logInfos.TryGetValue(logType, out LogInfo info))
                return;

            if (info.StreamWriter != null)
            {
                info.StreamWriter.Close();
                info.StreamWriter.Dispose();
                info.StreamWriter = null;
                info.CurrentFilePath = null; // 리셋
            }
        }
        private async Task ProcessLogsAsync()
        {
            while (true)
            {
                try
                {
                    var didWork = false;

                    foreach (var kv in _logQueues)
                    {
                        var type = kv.Key;
                        var q = kv.Value;

                        while (q.TryDequeue(out var logEntry))
                        {
                            WriteLog(type, logEntry.Item1, logEntry.Item2);
                            didWork = true;
                        }

                        if (q.IsEmpty)
                        {
                            CloseStreamWriter(type);
                            CleanupLogs(type);
                        }
                    }

                    // 종료 신호면: 모든 큐가 비었을 때 실제 종료
                    if (_isExiting && AllQueuesEmpty())
                    {
                        CloseStreamWritersAll();
                        return;
                    }

                    if (false == didWork)
                        await Task.Delay(10);
                }
                catch (Exception ex)
                {
                    // 로그 자체가 막혔을 수 있으니 best-effort로 디버그 큐에 적재
                    EnqueueLog(BaseLogTypes.LogTypeDebug,
                        $"### Logger loop error: {ex.Message}, {ex.StackTrace}");
                    await Task.Delay(50);
                }
            }

            //while (true)
            //{
            //    await Task.Delay(1);

            //    foreach (var item in LogToWrite)
            //    {
            //        if (item.Value.Count > 0)
            //        {
            //            if (item.Value.TryDequeue(out Tuple<DateTime, string> logEntry))
            //            {
            //                WriteLog(item.Key, logEntry.Item1, logEntry.Item2);
            //            }
            //        }
            //        else
            //        {
            //            CloseStreamWriter(item.Key);

            //            // 이전 날짜 로그는 압축 및 제거
            //            CleanUpLogsAsync(item.Key);
            //        }
            //    }

            //    if (_exiting)
            //        return;
            //}
        }
        private bool AllQueuesEmpty()
        {
            foreach (var kv in _logQueues)
            {
                if (false == kv.Value.IsEmpty)
                    return false;
            }
            return true;
        }
        private void WriteLineToFile(string logType, string path, string message)
        {
            // 1) 해당 로그타입 정보가 없으면 반환
            if (false == _logInfos.TryGetValue(logType, out var info))
                return;

            // 2) 현 시점의 StreamWriter 스냅샷을 잡음
            var sw = info.StreamWriter;

            // 3) 없다면 생성 시도
            if (sw == null)
            {
                CreateStreamWriter(logType, path);

                // 생성 후 다시 참조를 얻어 안전하게 사용
                if (!_logInfos.TryGetValue(logType, out info))
                    return;

                sw = info.StreamWriter;
                if (sw == null) 
                    return;
            }

            try
            {
                sw.WriteLine(message);
            }
            catch (IOException)
            {
                CreateStreamWriter(logType, path);
                if (_logInfos.TryGetValue(logType, out info) && info.StreamWriter != null)
                    info.StreamWriter.WriteLine(message);
            }
        }
        private void WriteLog(string logType, DateTime logDate, string message)
        {
            try
            {
                // 1) 오늘 날짜 기준 기본 파일 (yyyyMMdd.txt)
                CreateLogFilePath(logType, logDate, out var basePath);

                // 2) 현재 사용 중인 파일이 있으면 우선 사용
                string currentPath = basePath;
                if (_logInfos.TryGetValue(logType, out var info) && !string.IsNullOrEmpty(info.CurrentFilePath))
                    currentPath = info.CurrentFilePath;

                // 3) 날짜가 바뀌었으면 basePath로 리셋(기존 writer 닫기)
                var nameNoExt = Path.GetFileNameWithoutExtension(currentPath); // yyyyMMdd or yyyyMMdd_HH
                var us = nameNoExt.IndexOf('_');
                var currentDatePart = (us > 0) ? nameNoExt.Substring(0, us) : nameNoExt;
                if (!string.Equals(currentDatePart, logDate.ToString("yyyyMMdd"), StringComparison.Ordinal))
                {
                    CloseStreamWriter(logType);
                    currentPath = basePath;
                }

                // 4) 용량 초과 시: 현재 파일을 시간 접미사로 rename → 다시 yyyyMMdd.txt로 기록
                if (_logInfos.TryGetValue(logType, out var info2)
                    && info2.MaxFileSizeBytes > 0
                    && File.Exists(currentPath))
                {
                    long len = new FileInfo(currentPath).Length;
                    if (len >= info2.MaxFileSizeBytes)
                    {
                        // writer 닫고 파일 rename (간헐적 잠금 대비 재시도)
                        CloseStreamWriter(logType);
                        var rotatedPath = BuildHourlyRotatedPath(basePath, DateTime.Now);

                        const int maxMoveRetries = 3;
                        const int moveRetryDelayMs = 50;
                        bool moved = false;

                        for (int attempt = 0; attempt < maxMoveRetries && !moved; attempt++)
                        {
                            try
                            {
                                File.Move(currentPath, rotatedPath);
                                moved = true;
                            }
                            catch (IOException)
                            {
                                // 잠금/경합 가능성 → 잠깐 대기 후 경로 갱신하며 재시도
                                System.Threading.Thread.Sleep(moveRetryDelayMs);
                                rotatedPath = BuildHourlyRotatedPath(basePath, DateTime.Now);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                // 권한/잠금 이슈도 재시도
                                System.Threading.Thread.Sleep(moveRetryDelayMs);
                                rotatedPath = BuildHourlyRotatedPath(basePath, DateTime.Now);
                            }
                        }

                        // rename 성공/실패와 무관하게, 다시 yyyyMMdd.txt로 이어서 기록
                        currentPath = basePath;
                    }
                }

                // 5) 경로가 바뀌었으면 writer 교체
                if (_logInfos.TryGetValue(logType, out var info3))
                {
                    if (!string.Equals(info3.CurrentFilePath, currentPath, StringComparison.OrdinalIgnoreCase))
                        CloseStreamWriter(logType);
                }

                // 6) 실제 쓰기
                var logEntry = $"[{logDate:HH:mm:ss.fff}] {message}";
                WriteLineToFile(logType, currentPath, logEntry);
            }
            catch (Exception ex)
            {
                // 로깅 중 오류는 Debug 로그에 best-effort로 남김
                CreateLogFilePath(BaseLogTypes.LogTypeDebug, logDate, out var debugPath);
                WriteLineToFile(BaseLogTypes.LogTypeDebug, debugPath,
                    $"##### {ex.Message} : {ex.StackTrace} #####");
            }
        }

        private void CleanupLogs(string logType)
        {
            if (!_logInfos.ContainsKey(logType))
                return;

            if (!_logInfos[logType].UseCleanup)
                return;

            DateTime today = DateTime.Today;
            // 어제(today-1), 오늘(today)는 유지 → (today - 2) 이하 정리
            DateTime threshold = today.AddDays(-RetentionKeepDays);

            // 실행 시각 기록(지금은 게이팅에 쓰지 않지만 추후 정책용으로 남겨둠)
            _logInfos[logType].LastBackupDate = DateTime.Now;

            try
            {
                string dir = _logInfos[logType].LogDirectory;
                string tempFolderPath = Path.Combine(dir, "Temp");
                bool tempCreated = false;     // Temp 폴더 실제 생성 여부

                // ▼ 백업 폴더: 현재 로그 폴더 기준 \Backup
                const string BackupFolderName = "Backup"; // 상단 상수로 빼도 됨
                string backupDir = Path.Combine(dir, BackupFolderName);
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                var logFiles = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly);
                //var logFiles = Directory.GetFiles(dir, "*.txt", SearchOption.TopDirectoryOnly);
                if (logFiles.Length == 0) return;

                foreach (var file in logFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string nameNoExt = Path.GetFileNameWithoutExtension(file);

                    // TODO : 기존 zip 파일 검색 후 보내기
                    string extension = Path.GetExtension(file);
                    if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            string dest = Path.Combine(backupDir, fileName);
                            File.Move(file, dest);
                        }
                        catch (Exception exDel)
                        {
                            EnqueueLog(
                                BaseLogTypes.LogTypeDebug,
                                $"### File Move has Failed : {exDel.Message}, {exDel.StackTrace}"
                            );
                        }

                        continue;
                    }
                    else if (false == extension.Equals(".txt"))
                        continue;

                    // 파일명에서 날짜 추출: yyyyMMdd 또는 yyyyMMdd_... (언더스코어 앞까지만 날짜로 간주)
                    int us = nameNoExt.IndexOf('_');
                    string datePart = (us > 0) ? nameNoExt.Substring(0, us) : nameNoExt;

                    if (DateTime.TryParseExact(
                            datePart,
                            "yyyyMMdd",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out DateTime parsedDate))
                    {
                        // ▶ 이틀 전까지(<= threshold)만 정리
                        if (parsedDate.Date <= threshold)
                        {
                            // 잠금/경합 대비: Temp 경유 후 zip에 추가
                            if (!tempCreated)  // 한 번만 생성
                            {
                                Directory.CreateDirectory(tempFolderPath);
                                tempCreated = true;
                            }
                            //if (!Directory.Exists(tempFolderPath))
                            //    Directory.CreateDirectory(tempFolderPath);

                            string tempPath = Path.Combine(tempFolderPath, fileName);
                            File.Copy(file, tempPath, true);

                            // 날짜별 ZIP 이름: Backup\yyyyMMdd.zip
                            string zipFileName = Path.Combine(backupDir, $"{datePart}.zip");

                            if (File.Exists(zipFileName))
                            {
                                using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
                                {
                                    // 중복 엔트리 있으면 삭제 후 다시 추가
                                    var existing = zip.GetEntry(fileName);
                                    if (existing != null) existing.Delete();

                                    zip.CreateEntryFromFile(
                                        tempPath,
                                        fileName,
                                        CompressionLevel.Optimal
                                    );
                                }
                            }
                            else
                            {
                                using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Create))
                                {
                                    zip.CreateEntryFromFile(
                                        tempPath,
                                        fileName,
                                        System.IO.Compression.CompressionLevel.Optimal
                                    );
                                }
                            }

                            // 원본 삭제 (실패는 로그로)
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception exDel)
                            {
                                EnqueueLog(
                                    BaseLogTypes.LogTypeDebug,
                                    $"### File Delete Failed : {exDel.Message}, {exDel.StackTrace}"
                                );
                            }
                        }
                    }
                }

                // 임시 폴더 정리(사용 중이면 조용히 무시)
                try 
                {
                    if (tempCreated && Directory.Exists(tempFolderPath))
                    {
                        Directory.Delete(tempFolderPath, true);
                    }
                } 
                catch { }
            }
            catch (Exception ex)
            {
                EnqueueLog(
                    BaseLogTypes.LogTypeDebug,
                    $"### Backup Failed : {ex.Message}, {ex.StackTrace}"
                );
            }
        }

        private Task WaitForCompletion() => _writeLogTask;
        #endregion </Internal>

        #endregion </Methods>
    }
    public class DebugLogger : ModuleLogger
    {
        private DebugLogger() : base(BaseLogTypes.LogTypeDebug, BaseLogTypes.LogTypeDebug, true) { }
        private static readonly Lazy<DebugLogger> _lazy = new Lazy<DebugLogger>(() => new DebugLogger());

        public static DebugLogger Instance
        {
            get
            {
                return _lazy.Value;
            }
        }

        public void WriteDebugLog(string message)
        {
            WriteLog(LogTitleTypes.ETC, message);
        }
    }
    public class ModuleLogger
    {
        public ModuleLogger(string logType, string filePath, bool useCleanup)
        {
            _logType = logType;

            _logger.RegisterLogType(logType, filePath, useCleanup);
            //FilePath = string.Format(@"EFEM\{0}\Log", name);
        }

        private readonly string _logType;

        //private readonly string FilePath = null;
        private static readonly AsyncLoggerForEfem _logger = AsyncLoggerForEfem.Instance;
        private string _messageToWrite = string.Empty;

        protected void WriteLog(LogTitleTypes titleType, string message)
        {
            _messageToWrite = string.Format("[{0}] {1}", titleType.ToString(), message);
            _logger.EnqueueLog(_logType, _messageToWrite);
        }

        public void WriteActionStartLog(string actionName, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                WriteLog(LogTitleTypes.ACT, $"########## Starting Action({actionName}) ##########");
            }
            else
            {
                WriteLog(LogTitleTypes.ACT, $"########## Starting Action({actionName}) : {message} ##########");
            }
        }
        public void WriteAlarmLog(string message)
        {
            WriteLog(LogTitleTypes.ACT, $"########## {message} ##########");
        }
        public void WriteActionEndLog(string actionName, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                WriteLog(LogTitleTypes.ACT, $"########## Ending Action({actionName}) ##########");
            }
            else
            {
                WriteLog(LogTitleTypes.ACT, $"########## Ending Action({actionName}) : {message} ##########");
            }
        }
    }
    #endregion </Class>

    #region <Interfaces>
    public interface IProcessModuleConfigure
    {
        void RegisterLocations(out List<string> locationNames, out List<string> entryWayNames);
        void MappingLocationAndEntryWay(List<string> locationNames, List<string> entryWayNames, out Dictionary<string, string> mappedEntryWays);
        void MappingCommunicatorPortByLocation(string[] locations, ref int[] ports);
    }
    #endregion </Interfaces>

    #region <Enumerations>
    // [영속화 enum] LocationKind로 저장됨. 저장은 이름으로. 멤버 재배치/삭제 금지 — 끝에만 추가.
    public enum ModuleType
    {
        Unknown = 0,
        LoadPort = 1,
        Robot = 2,
        ProcessModule = 3,
        Aligner = 4,
        Normal = 5,
    }
    public enum OccupancyChangeReason
    {
        Created = 0,
        PickedByRobot,
        PlacedByRobot,
        Edited,
        Recovery,
        Removed,
        Unknown = 99
    }
    public enum CommandResult
    {
        Proceed,
        Completed,
        Skipped,
        Timeout,
        Error,
        Invalid
    }
    public enum CommunicationResult
    {
        Ack,
        Nack,
        Proceed,
        Error,
    }
    //public enum LogTypes
    //{
    //    Temp,
    //    LoadPort,
    //    AtmRobot,
    //    ProcessModule,
    //    SecsGem
    //}
    public enum LogTitleTypes
    {
        ETC,    // 기타
        ACT,    // 액선 단위
        OPER,   // 단위 동작 단위
        SEND,   // 통신 송신 단위
        RECV,   // 통신 수신 단위
        IN,     // IN 신호
        OUT,    // OUT 신호
        CARR,   // 캐리어 단위
        RFID,   // RFID 읽기/쓰기
    }

    public enum MaterialFormat
    {
        Unknown = 0,
        Wafers = 1,     // 1 - wafers
        Cassettes,      // 2 - cassettes
        Die,            // 3 - die
        Boats,          // 4 - boats
        Ingots,         // 5 - ingots
        LeadFrames,     // 6 - leadframes
        Lots,           // 7 - lots
        Magazines,      // 8 - magazines
        Packages,       // 9 - packages
        Plates,         // 10 - plates
        Tubes,          // 11 - tubes
        WaterFrames,    // 12 - waterframes
        Carrier,        // 13 - carrier (FOUP, SMIF pod, cassette)
        Substrate,      // 14 - substrate
    }

    #region <Location Names>
    public enum PWA500BINLocations
    {
        ProcessModuleCoreInput,
        ProcessModuleSortingInput,
        ProcessModuleCoreOutput,
        ProcessModuleSortingOutput,
    }
    #endregion </Location Names>

    #endregion </Enumerations>

    public static class NewVersionChecker
    {
        public static bool IsOldEvents()
        {
            var path = Path.Combine(Define.DefineConstant.FilePath.FILEPATH_EXE, "OldEvents.txt");

            return File.Exists(path);
        }
    }
}

//namespace SerializableDictionary
//{
//    [XmlRoot("dictionary")]
//    public class SerializableDictionary<TKey, TValue>
//        : Dictionary<TKey, TValue>, IXmlSerializable
//    {
//        #region <IXmlSerializable Members>
//        public System.Xml.Schema.XmlSchema GetSchema()
//        {
//            return null;
//        }

//        public void ReadXml(System.Xml.XmlReader reader)
//        {
//            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
//            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

//            bool wasEmpty = reader.IsEmptyElement;
//            reader.Read();

//            if (wasEmpty)
//                return;

//            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
//            {
//                reader.ReadStartElement("item");

//                reader.ReadStartElement("key");
//                TKey key = (TKey)keySerializer.Deserialize(reader);
//                reader.ReadEndElement();

//                reader.ReadStartElement("value");
//                TValue value = (TValue)valueSerializer.Deserialize(reader);
//                reader.ReadEndElement();

//                this.Add(key, value);

//                reader.ReadEndElement();
//                reader.MoveToContent();
//            }
//            reader.ReadEndElement();
//        }

//        public void WriteXml(System.Xml.XmlWriter writer)
//        {
//            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
//            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

//            foreach (TKey key in this.Keys)
//            {
//                writer.WriteStartElement("item");

//                writer.WriteStartElement("key");
//                keySerializer.Serialize(writer, key);
//                writer.WriteEndElement();

//                writer.WriteStartElement("value");
//                TValue value = this[key];
//                valueSerializer.Serialize(writer, value);
//                writer.WriteEndElement();

//                writer.WriteEndElement();
//            }
//        }
//        #endregion </IXmlSerializable Members>
//    }
//}

//namespace SerializableDictionary
//{
//    public class MyKeyValuePair<TKey, TValue>
//    {
//        public TKey Key { get; set; }
//        public TValue Value { get; set; }
//    }

//    public class SerializableDictionary<TKey, TValue>
//    {
//        public MyKeyValuePair<TKey, TValue>[] NewDictionary { get; set; }

//        public void UpdateKeyValues(Dictionary<TKey, TValue> dictionary)
//        {
//            NewDictionary = dictionary.Select(kvp => new MyKeyValuePair<TKey, TValue> { Key = kvp.Key, Value = kvp.Value }).ToArray();
//        }
//    }
//}

//namespace SerializableDictionary
//{
//    [XmlRoot("dictionary")]
//    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IXmlSerializable
//    {
//        #region <Fields>
//        #endregion </Fields>
//        private readonly Dictionary<TKey, TValue> MyDictionary = new Dictionary<TKey, TValue>();

//        #region <Properties>
//        public ICollection<TValue> Values => MyDictionary.Values;

//        public TValue this[TKey key]
//        {
//            get => MyDictionary[key];
//            set => MyDictionary[key] = value;
//        }
//        public ICollection<TKey> Keys => MyDictionary.Keys;
//        public int Count => MyDictionary.Count;
//        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)MyDictionary).IsReadOnly;
//        #endregion </Properties>

//        #region <Methods>
//        public void Add(TKey key, TValue value)
//        {
//            MyDictionary.Add(key, value);
//        }
//        public void Add(KeyValuePair<TKey, TValue> item)
//        {
//            MyDictionary.Add(item.Key, item.Value);
//        }
//        public bool Remove(TKey key)
//        {
//            return MyDictionary.Remove(key);
//        }
//        public bool ContainsKey(TKey key)
//        {
//            return MyDictionary.ContainsKey(key);
//        }
//        public bool TryGetValue(TKey key, out TValue value)
//        {
//            return MyDictionary.TryGetValue(key, out value);
//        }
//        public void Clear()
//        {
//            MyDictionary.Clear();
//        }
//        public bool Contains(KeyValuePair<TKey, TValue> item)
//        {
//            return MyDictionary.Contains(item);
//        }
//        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
//        {
//            ((ICollection<KeyValuePair<TKey, TValue>>)MyDictionary).CopyTo(array, arrayIndex);
//        }
//        public bool Remove(KeyValuePair<TKey, TValue> item)
//        {
//            return MyDictionary.Remove(item.Key);
//        }

//        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
//        {
//            return MyDictionary.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        public XmlSchema GetSchema()
//        {
//            return null;
//        }

//        public void ReadXml(XmlReader reader)
//        {
//            if (reader.IsEmptyElement)
//            {
//                reader.ReadStartElement();
//                return;
//            }

//            reader.ReadStartElement();
//            while (reader.NodeType != XmlNodeType.EndElement)
//            {
//                reader.ReadStartElement("item");
//                TKey key = (TKey)new XmlSerializer(typeof(TKey)).Deserialize(reader);
//                TValue value = (TValue)new XmlSerializer(typeof(TValue)).Deserialize(reader);
//                MyDictionary.Add(key, value);
//                reader.ReadEndElement();
//                reader.MoveToContent();
//            }
//            reader.ReadEndElement();
//        }

//        public void WriteXml(XmlWriter writer)
//        {
//            foreach (var keyValuePair in MyDictionary)
//            {
//                writer.WriteStartElement("item");
//                new XmlSerializer(typeof(TKey)).Serialize(writer, keyValuePair.Key);
//                new XmlSerializer(typeof(TValue)).Serialize(writer, keyValuePair.Value);
//                writer.WriteEndElement();
//            }
//        }
//        #endregion </Methods>
//    }

//}

namespace Serializabler
{
    [Serializable]
    public class SerializableDictionary
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }
    }
}