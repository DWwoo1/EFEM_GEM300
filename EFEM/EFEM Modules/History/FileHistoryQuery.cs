using System;
using System.Collections.Generic;
using System.IO;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 파일 기반 이력 조회. (기존 조회 화면들의 폴더 스캔/파일 파싱 로직을 이관)
    /// - 랏 목록  : 백업 날짜 폴더의 하위 랏 폴더 스캔 (생성 시각 = 첫 .log의 CreationTime)
    /// - 랏 상세  : 랏 폴더의 첫 .log 파싱 (기존 화면과 동일하게 첫 파일만)
    /// - 현재작업 : CurrentWorking의 캐리어/기판 파일 파싱
    /// - 집계     : 랏 폴더 하위 Wafers\ 파일명 수집
    /// </summary>
    public sealed class FileHistoryQuery : IHistoryQuery
    {
        #region <Constructors>
        public FileHistoryQuery(FileHistoryStore fileStore)
        {
            _fileStore = fileStore ?? throw new ArgumentNullException("fileStore");
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly FileHistoryStore _fileStore;
        #endregion </Fields>

        #region <IHistoryQuery>
        public List<LotSummary> GetLots(DateTime date, string category)
        {
            var result = new List<LotSummary>();
            try
            {
                var path = _fileStore.GetBackupPath(date, category);
                if (false == Directory.Exists(path))
                    return result;

                var directories = Directory.GetDirectories(path);
                for (int i = 0; directories != null && i < directories.Length; ++i)
                {
                    result.Add(new LotSummary
                    {
                        LotId = Path.GetFileName(directories[i]),
                        CreatedTime = GetLotCreatedTime(directories[i]),
                    });
                }

                result.Sort((a, b) => a.CreatedTime.CompareTo(b.CreatedTime));
            }
            catch
            {
            }
            return result;
        }
        public List<HistoryRecord> GetLotHistory(DateTime date, string category, string lotId)
        {
            try
            {
                var lotPath = Path.Combine(_fileStore.GetBackupPath(date, category), lotId);
                if (false == Directory.Exists(lotPath))
                    return new List<HistoryRecord>();

                var files = Directory.GetFiles(lotPath, "*.log");
                if (files == null || files.Length == 0)
                    return new List<HistoryRecord>();

                // 기존 화면과 동일하게 폴더의 첫 캐리어 로그만 표시 대상
                var records = ReadRecordsFromFile(files[0], date);

                // 라인에는 캐리어 ID가 없으므로 파일명(=캐리어 ID)으로 보충 (화면의 CARRIER NAME 표시용)
                var carrierId = Path.GetFileNameWithoutExtension(files[0]);
                for (int i = 0; i < records.Count; ++i)
                {
                    records[i].CarrierId = carrierId;
                    records[i].LotId = lotId;
                }

                return records;
            }
            catch
            {
                return new List<HistoryRecord>();
            }
        }
        public List<HistoryRecord> GetWorkingCarrierHistory(int portId, string carrierId)
        {
            try
            {
                var records = ReadRecordsFromFile(_fileStore.GetCarrierHistoryPath(portId, carrierId), DateTime.Now);
                for (int i = 0; i < records.Count; ++i)
                {
                    records[i].PortId = portId;
                    records[i].CarrierId = carrierId;
                }
                return records;
            }
            catch
            {
                return new List<HistoryRecord>();
            }
        }
        public List<HistoryRecord> GetWorkingSubstrateHistory(string substrateName, string category)
        {
            try
            {
                return ReadRecordsFromFile(_fileStore.GetSubstratePath(substrateName, category), DateTime.Now);
            }
            catch
            {
                return new List<HistoryRecord>();
            }
        }
        public Dictionary<string, List<string>> GetLotSubstrates(DateTime date, string category)
        {
            var result = new Dictionary<string, List<string>>();
            try
            {
                var path = _fileStore.GetBackupPath(date, category);
                if (false == Directory.Exists(path))
                    return result;

                var directories = Directory.GetDirectories(path);
                for (int i = 0; directories != null && i < directories.Length; ++i)
                {
                    string wafersPath = Path.Combine(directories[i], "Wafers");
                    if (false == Directory.Exists(wafersPath))
                        continue;

                    var names = new List<string>();
                    var wafers = Directory.GetFiles(wafersPath);
                    for (int j = 0; wafers != null && j < wafers.Length; ++j)
                    {
                        names.Add(Path.GetFileNameWithoutExtension(wafers[j]));
                    }

                    result[Path.GetFileName(directories[i])] = names;
                }
            }
            catch
            {
            }
            return result;
        }
        #endregion </IHistoryQuery>

        #region <Internal>
        /// <summary>캐리어 이력 파일은 File.Move로 백업되어 생성 시간이 보존되므로 랏 생성 시간으로 사용. (기존 화면 로직 이관)</summary>
        private static DateTime GetLotCreatedTime(string lotDirectory)
        {
            var carrierLogs = Directory.GetFiles(lotDirectory, "*.log");
            if (carrierLogs != null && carrierLogs.Length > 0)
                return File.GetCreationTime(carrierLogs[0]);

            return Directory.GetCreationTime(lotDirectory);
        }
        private static List<HistoryRecord> ReadRecordsFromFile(string filePath, DateTime referenceDate)
        {
            var result = new List<HistoryRecord>();
            if (string.IsNullOrEmpty(filePath) || false == File.Exists(filePath))
                return result;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (false == sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (false == HistoryLineFormat.TryParse(line, referenceDate.Year, out HistoryRecord record))
                        continue;

                    // 라인에 연도가 없으므로 연말/연초에 걸친 이력 보정 (1월 파일 안의 12월 라인)
                    if (record.Time.Month == 12 && referenceDate.Month == 1)
                        record.Time = record.Time.AddYears(-1);

                    result.Add(record);
                }
            }
            return result;
        }
        #endregion </Internal>
    }
}
