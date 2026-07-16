using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 파일↔DB 병행 기록의 정합성 자동 대조. (병행 검증 기간 전용)
    ///
    /// 화면 조회는 파일 우선이라 DB 내용이 정상인지 화면으로는 알 수 없으므로,
    /// 두 조회 구현(FileHistoryQuery/SqliteHistoryQuery)을 나란히 실행해 결과를 비교한다.
    /// - 대상: 랏 목록 / 랏별 상세 레코드(시간순 정렬 후 필드 비교) / 랏별 기판 목록
    /// - 결과: {리포트 폴더}\yyyyMMdd.txt 저장, 불일치 시 진단 로그에 요약 1줄
    /// - 자정에 걸친 랏은 파일(백업 날짜)과 DB(이벤트/제거 날짜) 기준 차이로 한쪽에만 보일 수 있어
    ///   경계 가능성을 표기한다.
    /// 파일 저장소 은퇴(기록 중단) 시 비교 대상이 사라지므로 이 클래스도 함께 은퇴한다.
    /// </summary>
    public sealed class HistoryConsistencyChecker
    {
        #region <Constructors>
        public HistoryConsistencyChecker(IHistoryQuery file, IHistoryQuery database, string reportDirectory, Action<string> writeDiagnostic)
        {
            _file = file ?? throw new ArgumentNullException("file");
            _database = database ?? throw new ArgumentNullException("database");
            _reportDirectory = reportDirectory ?? throw new ArgumentNullException("reportDirectory");
            _writeDiagnostic = writeDiagnostic;
        }
        #endregion </Constructors>

        #region <Fields>
        private const int MaxDetailSamples = 5;

        private readonly IHistoryQuery _file;
        private readonly IHistoryQuery _database;
        private readonly string _reportDirectory;
        private readonly Action<string> _writeDiagnostic;
        #endregion </Fields>

        #region <Methods>
        /// <summary>지정 날짜의 파일/DB 이력을 대조하고 리포트를 남긴다. 전체 일치 여부를 반환.</summary>
        public bool VerifyDate(DateTime date, IEnumerable<string> categories)
        {
            bool allMatched = true;
            var report = new StringBuilder();
            report.AppendLine(string.Format("랏 히스토리 정합 대조 리포트 - 대상일 {0:yyyy-MM-dd} (생성 {1:yyyy-MM-dd HH:mm:ss})", date, DateTime.Now));
            report.AppendLine("주의: 자정에 걸친 랏은 파일(백업 날짜)/DB(이벤트·제거 날짜) 기준 차이로 한쪽에만 보일 수 있음 - '(날짜 경계 가능)' 표기 참고");
            report.AppendLine();

            foreach (var category in categories)
            {
                if (false == VerifyCategory(date, category, report))
                    allMatched = false;
            }

            report.AppendLine(allMatched ? "결과: OK (전체 일치)" : "결과: 불일치 발견");

            try
            {
                if (false == Directory.Exists(_reportDirectory))
                    Directory.CreateDirectory(_reportDirectory);

                File.WriteAllText(
                    Path.Combine(_reportDirectory, string.Format("{0:yyyyMMdd}.txt", date)),
                    report.ToString(),
                    new UTF8Encoding(true));
            }
            catch
            {
            }

            if (false == allMatched && _writeDiagnostic != null)
            {
                _writeDiagnostic(string.Format("이력 정합 불일치 : 대상일 {0:yyyy-MM-dd} - 상세는 Verify\\{0:yyyyMMdd}.txt 참고", date));
            }

            return allMatched;
        }

        private bool VerifyCategory(DateTime date, string category, StringBuilder report)
        {
            bool matched = true;
            report.AppendLine(string.Format("=== 분류 [{0}] ===", category));

            var fileLots = _file.GetLots(date, category).ToDictionary(x => x.LotId, x => x.CreatedTime);
            var dbLots = _database.GetLots(date, category).ToDictionary(x => x.LotId, x => x.CreatedTime);
            report.AppendLine(string.Format("랏 수 : 파일 {0} / DB {1}", fileLots.Count, dbLots.Count));

            foreach (var lot in fileLots)
            {
                if (false == dbLots.ContainsKey(lot.Key))
                {
                    matched = false;
                    report.AppendLine(string.Format("  [불일치] 파일에만 존재 : {0} (생성 {1:MM-dd HH:mm:ss}){2}",
                        lot.Key, lot.Value, IsDateBoundary(lot.Value, date) ? " (날짜 경계 가능)" : string.Empty));
                }
            }
            foreach (var lot in dbLots)
            {
                if (false == fileLots.ContainsKey(lot.Key))
                {
                    matched = false;
                    report.AppendLine(string.Format("  [불일치] DB에만 존재 : {0} (생성 {1:MM-dd HH:mm:ss}){2}",
                        lot.Key, lot.Value, IsDateBoundary(lot.Value, date) ? " (날짜 경계 가능)" : string.Empty));
                }
            }

            // 양쪽에 있는 랏의 상세/기판 목록 대조
            var fileSubstrates = _file.GetLotSubstrates(date, category);
            var dbSubstrates = _database.GetLotSubstrates(date, category);
            foreach (var lotId in fileLots.Keys.Where(dbLots.ContainsKey))
            {
                if (false == VerifyLotDetail(date, category, lotId, report))
                    matched = false;

                if (false == VerifyLotSubstrates(lotId, fileSubstrates, dbSubstrates, report))
                    matched = false;
            }

            if (matched)
                report.AppendLine("  일치");
            report.AppendLine();

            return matched;
        }

        private bool VerifyLotDetail(DateTime date, string category, string lotId, StringBuilder report)
        {
            var fileRecords = _file.GetLotHistory(date, category, lotId);
            var dbRecords = _database.GetLotHistory(date, category, lotId);

            // 파일은 안착 블록 순, DB는 시간순이므로 동일 키로 정렬 후 비교한다.
            var fileKeys = fileRecords.Select(CompareKey).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var dbKeys = dbRecords.Select(CompareKey).OrderBy(x => x, StringComparer.Ordinal).ToList();

            if (fileKeys.Count != dbKeys.Count)
            {
                report.AppendLine(string.Format("  [불일치] 랏 {0} 레코드 수 : 파일 {1} / DB {2}", lotId, fileKeys.Count, dbKeys.Count));
            }

            var onlyFile = fileKeys.Except(dbKeys).ToList();
            var onlyDb = dbKeys.Except(fileKeys).ToList();
            if (onlyFile.Count == 0 && onlyDb.Count == 0)
                return fileKeys.Count == dbKeys.Count;

            report.AppendLine(string.Format("  [불일치] 랏 {0} 내용 차이 : 파일에만 {1}건, DB에만 {2}건", lotId, onlyFile.Count, onlyDb.Count));
            for (int i = 0; i < onlyFile.Count && i < MaxDetailSamples; ++i)
                report.AppendLine("    파일측: " + onlyFile[i]);
            for (int i = 0; i < onlyDb.Count && i < MaxDetailSamples; ++i)
                report.AppendLine("    DB측  : " + onlyDb[i]);

            return false;
        }

        private static bool VerifyLotSubstrates(string lotId,
            Dictionary<string, List<string>> fileSubstrates,
            Dictionary<string, List<string>> dbSubstrates,
            StringBuilder report)
        {
            fileSubstrates.TryGetValue(lotId, out List<string> fromFile);
            dbSubstrates.TryGetValue(lotId, out List<string> fromDb);

            var fileSet = new HashSet<string>(fromFile ?? new List<string>());
            var dbSet = new HashSet<string>(fromDb ?? new List<string>());
            if (fileSet.SetEquals(dbSet))
                return true;

            report.AppendLine(string.Format("  [불일치] 랏 {0} 기판 목록 : 파일 {1}개 / DB {2}개 (파일에만: {3} / DB에만: {4})",
                lotId, fileSet.Count, dbSet.Count,
                string.Join(",", fileSet.Except(dbSet)),
                string.Join(",", dbSet.Except(fileSet))));
            return false;
        }

        private static string CompareKey(HistoryRecord record)
        {
            // 연도 힌트 차이의 영향을 받지 않도록 파일 라인과 같은 MM/dd 타임스탬프로 비교한다.
            return string.Join("\t",
                HistoryLineFormat.ComposeTimestamp(record.Time),
                record.CarrierEventCode,
                record.SubstrateName,
                record.SubstrateEventCode,
                record.Message);
        }

        private static bool IsDateBoundary(DateTime createdTime, DateTime targetDate)
        {
            return createdTime.Date != targetDate.Date;
        }
        #endregion </Methods>
    }
}
