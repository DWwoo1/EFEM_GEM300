using System;
using System.Collections.Generic;

namespace EFEM.History
{
    /// <summary>날짜별 랏 목록의 항목. (조회 화면의 랏 리스트용)</summary>
    public sealed class LotSummary
    {
        public string LotId = string.Empty;
        public DateTime CreatedTime;
    }

    /// <summary>
    /// 2026.07.06. jhlim [ADD] 랏 히스토리 조회 추상화. (쓰기 측 IHistoryStore와 분리된 읽기 전용 계약)
    ///
    /// 구현체:
    /// - FileHistoryQuery  : 현행 파일 구조(백업 날짜 폴더 + CurrentWorking 파일) 조회
    /// - SqliteHistoryQuery: main LotHistoryEvent(미제거 캐리어) + 일자별 archive DB(제거된 캐리어) 통합 조회
    /// 조회 소스 선택은 파사드(LotHistoryLog.GetQuery)가 옵션(UseDatabaseForLotHistoryQuery)으로 결정한다.
    ///
    /// 정합 주의: 파일은 백업(완료) 날짜 기준으로 랏이 묶이고, DB는 main=이벤트 날짜/archive=캐리어 제거 날짜
    /// 기준이라 자정에 걸친 랏은 두 소스에서 하루 어긋나 보일 수 있다.
    /// 모든 메서드는 실패 시 던지지 않고 빈 결과를 반환한다. (조회 UI 견고성)
    /// </summary>
    public interface IHistoryQuery
    {
        /// <summary>선택 날짜/분류의 랏 목록 (생성 시각 오름차순)</summary>
        List<LotSummary> GetLots(DateTime date, string category);

        /// <summary>랏 상세 타임라인 (캐리어 이벤트 + 귀속 기판 이벤트)</summary>
        List<HistoryRecord> GetLotHistory(DateTime date, string category, string lotId);

        /// <summary>작업 중인 캐리어의 이력 (현재작업 화면)</summary>
        List<HistoryRecord> GetWorkingCarrierHistory(int portId, string carrierId);

        /// <summary>작업 중인 기판의 이력 (현재작업 화면)</summary>
        List<HistoryRecord> GetWorkingSubstrateHistory(string substrateName, string category);

        /// <summary>선택 날짜/분류의 랏별 기판 이름 목록 (생산 집계 화면)</summary>
        Dictionary<string, List<string>> GetLotSubstrates(DateTime date, string category);
    }
}
