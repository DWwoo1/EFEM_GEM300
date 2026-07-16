using System;
using System.Collections.Generic;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 자동 소스 선택 조회. (사용자 결정: 설정 토글 대신 데이터 존재 기준 자동 선택)
    /// - 파일이 주 저장소인 병행 기간에는 파일 결과가 항상 존재하므로 파일이 우선한다.
    /// - 파일 기록이 중단된 이후(또는 파일이 지워진 과거 날짜)에는 DB 결과로 자동 폴백한다.
    /// - 랏 목록/집계는 두 소스의 합집합 - 파일 기록 중단 당일처럼 소스별로 랏이 반쪽씩 존재하는
    ///   경계 상황에서도 목록이 완전하도록 한다. (같은 랏은 파일 항목 우선)
    /// database는 미장착(null)일 수 있으며 그 경우 파일 단독으로 동작한다.
    /// </summary>
    public sealed class CompositeHistoryQuery : IHistoryQuery
    {
        #region <Constructors>
        public CompositeHistoryQuery(IHistoryQuery file)
        {
            _file = file ?? throw new ArgumentNullException("file");
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly IHistoryQuery _file;
        // 초기화 순서상 DB 조회는 나중에 장착된다. (UI 스레드에서 읽으므로 volatile)
        private volatile IHistoryQuery _database = null;
        #endregion </Fields>

        #region <Methods>
        public void SetDatabase(IHistoryQuery database)
        {
            _database = database;
        }
        #endregion </Methods>

        #region <IHistoryQuery>
        public List<LotSummary> GetLots(DateTime date, string category)
        {
            var result = _file.GetLots(date, category);
            var database = _database;
            if (database == null)
                return result;

            var knownLots = new HashSet<string>();
            for (int i = 0; i < result.Count; ++i)
                knownLots.Add(result[i].LotId);

            // 파일에 없는 랏만 DB에서 보충 (합집합)
            var fromDatabase = database.GetLots(date, category);
            bool appended = false;
            for (int i = 0; i < fromDatabase.Count; ++i)
            {
                if (knownLots.Contains(fromDatabase[i].LotId))
                    continue;

                result.Add(fromDatabase[i]);
                appended = true;
            }

            if (appended)
                result.Sort((a, b) => a.CreatedTime.CompareTo(b.CreatedTime));

            return result;
        }
        public List<HistoryRecord> GetLotHistory(DateTime date, string category, string lotId)
        {
            var result = _file.GetLotHistory(date, category, lotId);
            var database = _database;
            if (result.Count > 0 || database == null)
                return result;

            return database.GetLotHistory(date, category, lotId);
        }
        public List<HistoryRecord> GetWorkingCarrierHistory(int portId, string carrierId)
        {
            var result = _file.GetWorkingCarrierHistory(portId, carrierId);
            var database = _database;
            if (result.Count > 0 || database == null)
                return result;

            return database.GetWorkingCarrierHistory(portId, carrierId);
        }
        public List<HistoryRecord> GetWorkingSubstrateHistory(string substrateName, string category)
        {
            var result = _file.GetWorkingSubstrateHistory(substrateName, category);
            var database = _database;
            if (result.Count > 0 || database == null)
                return result;

            return database.GetWorkingSubstrateHistory(substrateName, category);
        }
        public Dictionary<string, List<string>> GetLotSubstrates(DateTime date, string category)
        {
            var result = _file.GetLotSubstrates(date, category);
            var database = _database;
            if (database == null)
                return result;

            // 파일에 없는 랏만 DB에서 보충 (합집합, 같은 랏은 파일 우선)
            var fromDatabase = database.GetLotSubstrates(date, category);
            foreach (var item in fromDatabase)
            {
                if (false == result.ContainsKey(item.Key))
                    result[item.Key] = item.Value;
            }

            return result;
        }
        #endregion </IHistoryQuery>
    }
}
