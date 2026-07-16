using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 파일 기반 이력 저장소. (LotHistoryEngine에서 영속화 로직만 분리)
    /// - 캐리어 이력 : {base}\CurrentWorking\{포트폴더}\{CarrierId}.log
    /// - 기판 이력   : {base}\CurrentWorking\{카테고리}\{SubstrateName}.log
    /// - 백업        : {base}\Backup\{yyyy}\{MM}\{dd}\{카테고리}\{LotId}\
    ///
    /// 파일명은 운영자 열람성을 위해 표시 이름(CarrierId/SubstrateName)을 사용한다.
    /// 불변 키(CarrierKey/SubstrateKey)는 이 저장소에서는 사용하지 않는다. (DB 저장소의 키 컬럼용)
    /// </summary>
    public sealed class FileHistoryStore : IHistoryStore
    {
        #region <Constructors>
        /// <param name="basePath">이력 루트 폴더 (하위에 CurrentWorking/, Backup/ 생성)</param>
        /// <param name="substrateCategories">기판 분류 폴더명 목록 (예: Core, Bin)</param>
        /// <param name="diagnosticFileName">진단 로그 파일명 (확장자 제외, basePath 바로 아래 생성)</param>
        public FileHistoryStore(string basePath, IEnumerable<string> substrateCategories, string diagnosticFileName)
        {
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentNullException("basePath");
            if (substrateCategories == null)
                throw new ArgumentNullException("substrateCategories");
            if (string.IsNullOrEmpty(diagnosticFileName))
                throw new ArgumentNullException("diagnosticFileName");

            BasePath = basePath;
            BasePathForSubstrate = string.Format(@"{0}\CurrentWorking", BasePath);
            Categories = substrateCategories.ToArray();
            DiagnosticFilePath = string.Format(@"{0}\{1}{2}", BasePath, diagnosticFileName, LogFileExtension);

            CurrentWorkingPath = new Dictionary<int, string>();
        }
        #endregion </Constructors>

        #region <Fields>
        private const string LogFileExtension = ".log";
        private const int OrphanSweepDays = 7;

        private readonly string BasePath;
        private readonly string BasePathForSubstrate;
        private readonly string[] Categories;
        private readonly string DiagnosticFilePath;
        private readonly Dictionary<int, string> CurrentWorkingPath;
        #endregion </Fields>

        #region <Methods>

        #region <Paths>
        // 파일 저장소 고유 API : UI가 이력 파일을 직접 열람할 때도 사용한다.
        public string GetBackupPath(DateTime time, string category)
        {
            return string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\{4}", BasePath, time.Year, time.Month, time.Day, category);
        }
        public string GetCarrierHistoryPath(int portId, string carrierId)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return string.Empty;

            return string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
        }
        public string GetSubstratePath(string substrateName, string category)
        {
            return string.Format(@"{0}\{1}\{2}{3}", BasePathForSubstrate, category, substrateName, LogFileExtension);
        }
        #endregion </Paths>

        #region <IHistoryStore>
        public void RegisterCarrierDirectory(int portId, string name)
        {
            string dir = string.Format(@"{0}\{1}", BasePathForSubstrate, name);
            CurrentWorkingPath[portId] = dir;
            if (false == Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        public void AppendCarrierEvent(HistoryRecord record)
        {
            var filePath = GetCarrierHistoryPath(record.PortId, record.CarrierId);
            if (string.IsNullOrEmpty(filePath))
                return;

            WriteLog(filePath, HistoryLineFormat.Compose(record));
        }
        public void AppendSubstrateEvent(HistoryRecord record)
        {
            WriteLog(GetSubstratePath(record.SubstrateName, record.Category), HistoryLineFormat.Compose(record));
        }
        public void AppendSubstrateEventWithCarrier(HistoryRecord record)
        {
            var line = HistoryLineFormat.Compose(record);

            // Substrate History 기록
            WriteLog(GetSubstratePath(record.SubstrateName, record.Category), line);

            // Carrier History 에도 기록 (포트 미등록이면 캐리어 기록만 생략)
            var carrierFilePath = GetCarrierHistoryPath(record.PortId, record.CarrierId);
            if (string.IsNullOrEmpty(carrierFilePath))
                return;

            WriteLog(carrierFilePath, line);
        }
        public void BindSubstrateToCarrier(DateTime time, int portId, string carrierKey, string carrierId, string substrateKey, string substrateName, string category)
        {
            // 이력이 없는 기판(작업 없이 반환되는 공테이프 등)은 병합할 것이 없으므로 정상 스킵
            var substrateHistoryFullPath = GetSubstratePath(substrateName, category);
            if (false == File.Exists(substrateHistoryFullPath))
                return;

            var carrierHistoryFullPath = GetCarrierHistoryPath(portId, carrierId);
            if (string.IsNullOrEmpty(carrierHistoryFullPath) ||
                false == File.Exists(carrierHistoryFullPath))
            {
                // 기판 이력은 있는데 받을 캐리어 파일이 없으면
                // 해당 작업이력이 캐리어 이력에서 유실되므로 조용히 삼키지 않고 흔적을 남긴다.
                WriteDiagnostic(string.Format("병합 스킵 (캐리어 이력 파일 없음) : [포트:{0}], [캐리어:{1}], [기판:{2}]",
                    portId, carrierId, substrateName));
                return;
            }

            string[] lines;
            using (FileStream fs = new FileStream(substrateHistoryFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                var tempList = new List<string>();
                while (false == sr.EndOfStream)
                {
                    tempList.Add(sr.ReadLine());
                }
                lines = tempList.ToArray();
            }

            if (lines == null || lines.Length <= 0)
                return;

            RewriteSubstrateNameField(substrateName, ref lines);

            // 기판이 캐리어에서 회수됐다가 재안착하면 병합이 다시 실행되므로
            // 캐리어 파일에 이미 있는 라인은 건너뛰어 중복 기록을 막는다.
            // (라인에 ms 단위 타임스탬프와 기판명이 포함되어 서로 다른 이벤트가 같은 라인이 될 가능성은 무시할 수준)
            var existingLines = new HashSet<string>();
            using (FileStream fs = new FileStream(carrierHistoryFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (false == sr.EndOfStream)
                {
                    existingLines.Add(sr.ReadLine());
                }
            }

            using (FileStream fs = new FileStream(carrierHistoryFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (existingLines.Contains(lines[i]))
                        continue;

                    sw.WriteLine(lines[i]);
                }
            }
        }
        public void RenameSubstrate(DateTime time, string substrateKey, string oldName, string newName, string category)
        {
            string sourceFilePath = GetSubstratePath(oldName, category);
            string destFilePath = GetSubstratePath(newName, category);

            if (false == File.Exists(sourceFilePath))
                return;

            string destPath = Path.GetDirectoryName(destFilePath);
            if (false == Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            MoveFileWithMerge(sourceFilePath, destFilePath);
        }
        public void CompleteCarrier(DateTime time, int portId, string carrierKey, string carrierId, string lotId, List<string> substrateNames, string category)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return;

            string backupPath = string.Format(@"{0}\{1}", GetBackupPath(time, category), lotId);
            if (false == Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);

            // Substrate Lists : 개별 파일 실패는 격리하고 나머지와 캐리어 파일 이동은 계속 진행한다.
            List<string> failedFiles = null;
            if (substrateNames != null)
            {
                string backupSubstratePath = string.Format(@"{0}\Wafers", backupPath);
                for (int i = 0; i < substrateNames.Count; ++i)
                {
                    try
                    {
                        MoveSubstrateHistoryFile(category, substrateNames[i], backupSubstratePath);
                    }
                    catch
                    {
                        if (failedFiles == null)
                            failedFiles = new List<string>();
                        failedFiles.Add(substrateNames[i]);
                    }
                }
            }

            // Carrier History
            try
            {
                string sourceFilePath = string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
                string backupFullPath = string.Format(@"{0}\{1}{2}", backupPath, carrierId, LogFileExtension);
                if (File.Exists(sourceFilePath))
                {
                    MoveFileWithMerge(sourceFilePath, backupFullPath);
                }
            }
            catch
            {
                if (failedFiles == null)
                    failedFiles = new List<string>();
                failedFiles.Add(carrierId);
            }

            // 실패분이 있으면 예외를 던져 명령 단위 재시도를 유도한다. (성공분은 재실행 시 건너뜀)
            if (failedFiles != null && 0 < failedFiles.Count)
                throw new IOException(string.Format("백업 이동 실패 : {0}", string.Join(", ", failedFiles)));
        }
        public void ClearPrevious(DateTime time, int portId, string carrierId, string loadPortName)
        {
            if (false == CurrentWorkingPath.TryGetValue(portId, out string basePath))
                return;

            if (false == Directory.Exists(basePath))
                return;

            string backupPath = string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\NotCompleted\{4}", BasePath, time.Year, time.Month, time.Day, loadPortName);
            if (false == Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);

            string[] files = Directory.GetFiles(basePath);
            string sourceFilePath = string.Format(@"{0}\{1}{2}", basePath, carrierId, LogFileExtension);
            List<string> failedFiles = null;
            for (int i = 0; files != null && i < files.Length; ++i)
            {
                var file = files[i];
                if (file.Equals(sourceFilePath))
                    continue;

                try
                {
                    string destinationPath = Path.Combine(backupPath, Path.GetFileName(file));
                    MoveFileWithMerge(file, destinationPath);
                }
                catch
                {
                    if (failedFiles == null)
                        failedFiles = new List<string>();
                    failedFiles.Add(file);
                }
            }

            if (failedFiles != null && 0 < failedFiles.Count)
                throw new IOException(string.Format("이전 이력 정리 실패 : {0}", string.Join(", ", failedFiles)));
        }
        /// <summary>
        /// 백업 목록에 포함되지 못하고 남은 기판 이력 파일을 NotCompleted 백업으로 옮긴다.
        /// </summary>
        public void SweepOrphans()
        {
            try
            {
                DateTime now = DateTime.Now;

                foreach (string category in Categories)
                {
                    string dir = string.Format(@"{0}\{1}", BasePathForSubstrate, category);
                    if (false == Directory.Exists(dir))
                        continue;

                    string[] files = Directory.GetFiles(dir);
                    for (int i = 0; files != null && i < files.Length; ++i)
                    {
                        try
                        {
                            if ((now - File.GetLastWriteTime(files[i])).TotalDays < OrphanSweepDays)
                                continue;

                            string destPath = string.Format(@"{0}\Backup\{1:0000}\{2:00}\{3:00}\NotCompleted\Substrates\{4}", BasePath, now.Year, now.Month, now.Day, category);
                            if (false == Directory.Exists(destPath))
                                Directory.CreateDirectory(destPath);

                            MoveFileWithMerge(files[i], Path.Combine(destPath, Path.GetFileName(files[i])));
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteDiagnostic(string.Format("고아 기판 이력 정리 실패 : {0}", ex.Message));
            }
        }
        /// <summary>이력 처리 실패를 조용히 삼키지 않도록 진단 로그를 남긴다.</summary>
        public void WriteDiagnostic(string message)
        {
            try
            {
                string dirName = Path.GetDirectoryName(DiagnosticFilePath);
                if (false == Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                using (FileStream fs = new FileStream(DiagnosticFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(string.Format("{0:yyyy/MM/dd-HH:mm:ss.fff}\t{1}", DateTime.Now, message));
                }
            }
            catch
            {
            }
        }
        #endregion </IHistoryStore>

        #region <Internal>
        /// <summary>병합되는 라인들의 기판이름 필드(3번째)를 최종 확정 이름으로 치환한다. (링 ID로 기록된 초기 이력 대응)</summary>
        private static void RewriteSubstrateNameField(string substrateName, ref string[] linesToChange)
        {
            for (int i = 0; i < linesToChange.Length; ++i)
            {
                var parts = linesToChange[i].Split(new char[] { '\t' }, StringSplitOptions.None);
                // parts[2] 접근이므로 최소 3개 필드 필요
                if (parts.Length < 3)
                    continue;

                parts[2] = substrateName;
                linesToChange[i] = string.Join("\t", parts);
            }
        }
        private void MoveSubstrateHistoryFile(string category, string substrateName, string newPath)
        {
            string sourceFilePath = GetSubstratePath(substrateName, category);
            string destFilePath = string.Format(@"{0}\{1}{2}", newPath, substrateName, LogFileExtension);

            if (false == File.Exists(sourceFilePath))
                return;

            string destPath = Path.GetDirectoryName(destFilePath);
            if (false == Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            MoveFileWithMerge(sourceFilePath, destFilePath);
        }
        /// <summary>
        /// 목적지에 같은 파일이 있으면 지우지 않고 내용을 이어붙인다.
        /// (같은 캐리어를 같은 날 재작업 시 기존 백업 이력 유실 방지)
        /// </summary>
        private void MoveFileWithMerge(string sourceFilePath, string destFilePath)
        {
            if (File.Exists(destFilePath))
            {
                using (FileStream source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream dest = new FileStream(destFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    source.CopyTo(dest);
                }

                File.Delete(sourceFilePath);
            }
            else
            {
                File.Move(sourceFilePath, destFilePath);
            }
        }
        private void WriteLog(string filePath, string logEntry)
        {
            // 예외를 삼키지 않고 엔진으로 전파하여 재시도/진단 처리한다.
            string dirName = Path.GetDirectoryName(filePath);
            if (false == Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(logEntry);
            }
        }
        #endregion </Internal>

        #endregion </Methods>
    }
}
