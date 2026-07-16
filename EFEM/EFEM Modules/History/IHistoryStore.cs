using System;
using System.Collections.Generic;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 랏 히스토리 저장소 추상화.
    ///
    /// LotHistoryEngine이 큐잉/재시도/순서 보장을 담당하고, 실제 영속화는 이 인터페이스 구현이 담당한다.
    /// 구현체: FileHistoryStore(현행 파일 기반). DB 저장소(SQLite)를 추가하면 병행 기록/교체가 가능하다.
    ///
    /// 규약:
    /// - 모든 메서드는 엔진의 단일 처리 스레드에서만 호출된다. (구현은 스레드 안전을 가정하지 않아도 됨)
    /// - 일시 오류는 예외로 던진다. 엔진이 명령 단위로 재시도하므로 구현은 재실행에 안전해야 한다.
    ///   (이미 반영된 항목은 건너뛰는 식 - 예: 이동 완료 파일 스킵, 중복 라인 스킵)
    /// - 불변 키(CarrierKey/SubstrateKey)는 비어 있을 수 있다.
    /// </summary>
    public interface IHistoryStore
    {
        /// <summary>포트별 캐리어 이력 채널을 준비한다. (파일 저장소: 포트 작업 폴더 생성)</summary>
        void RegisterCarrierDirectory(int portId, string name);

        /// <summary>캐리어 단위 이벤트를 캐리어 이력에 기록한다.</summary>
        void AppendCarrierEvent(HistoryRecord record);

        /// <summary>기판 단위 이벤트를 기판 이력에만 기록한다. (소속 캐리어 미확정 단계)</summary>
        void AppendSubstrateEvent(HistoryRecord record);

        /// <summary>기판 단위 이벤트를 기판 이력과 소속 캐리어 이력에 함께 기록한다.</summary>
        void AppendSubstrateEventWithCarrier(HistoryRecord record);

        /// <summary>
        /// 기판 이력 전체를 캐리어 이력에 귀속시킨다. (지연 바인딩: 안착 시점에 소속 확정)
        /// 파일 저장소: 기판 파일을 캐리어 파일에 병합 / DB 저장소: 키 기반 소속 UPDATE.
        /// </summary>
        void BindSubstrateToCarrier(DateTime time, int portId, string carrierKey, string carrierId, string substrateKey, string substrateName, string category);

        /// <summary>기판의 표시 이름을 변경한다. (파일 저장소: 파일 개명 / DB 저장소: 표시명 UPDATE)</summary>
        void RenameSubstrate(DateTime time, string substrateKey, string oldName, string newName, string category);

        /// <summary>완료된 캐리어 이력과 기판 이력들을 랏 단위로 확정한다. (파일 저장소: 날짜/분류/랏 백업 폴더로 이동)</summary>
        void CompleteCarrier(DateTime time, int portId, string carrierKey, string carrierId, string lotId, List<string> substrateNames, string category);

        /// <summary>이전 작업의 잔여 캐리어 이력을 정리한다. (파일 저장소: NotCompleted 백업으로 이동)</summary>
        void ClearPrevious(DateTime time, int portId, string carrierId, string loadPortName);

        /// <summary>어디에도 귀속되지 못하고 남은 기판 이력을 정리한다. (기동 후 첫 처리 시 1회 호출)</summary>
        void SweepOrphans();

        /// <summary>이력 처리 실패 진단을 남긴다. (엔진의 재시도 소진 보고에도 사용)</summary>
        void WriteDiagnostic(string message);
    }
}
