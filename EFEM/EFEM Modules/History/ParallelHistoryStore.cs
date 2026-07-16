using System;
using System.Collections.Generic;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 병행 기록용 합성 저장소.
    /// - primary(파일): 주 저장소. 예외를 그대로 전파해 엔진의 명령 단위 재시도를 유지한다.
    /// - secondary(DB): 병행 저장소(best-effort). 예외는 삼키고 primary 진단 로그로만 남긴다.
    ///   (secondary 실패가 파일 이력 기록을 막으면 안 됨)
    /// primary 실패로 엔진이 명령을 재시도하면 secondary가 재실행되는데,
    /// SqliteHistoryStore는 INSERT OR IGNORE(자연 키 UNIQUE)로 중복을 막으므로 재실행에 안전하다.
    /// secondary는 초기화 순서상 나중에 장착된다. (DB 컨텍스트가 파사드 생성보다 늦게 구성됨)
    /// </summary>
    public sealed class ParallelHistoryStore : IHistoryStore
    {
        #region <Constructors>
        public ParallelHistoryStore(IHistoryStore primary)
        {
            _primary = primary ?? throw new ArgumentNullException("primary");
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly IHistoryStore _primary;
        // 엔진 처리 스레드에서 읽고 초기화 스레드에서 장착되므로 volatile (참조 대입은 원자적)
        private volatile IHistoryStore _secondary = null;
        #endregion </Fields>

        #region <Methods>
        public void SetSecondary(IHistoryStore secondary)
        {
            _secondary = secondary;
        }

        public void RegisterCarrierDirectory(int portId, string name)
        {
            _primary.RegisterCarrierDirectory(portId, name);
            RunSecondary("RegisterCarrierDirectory", s => s.RegisterCarrierDirectory(portId, name));
        }
        public void AppendCarrierEvent(HistoryRecord record)
        {
            _primary.AppendCarrierEvent(record);
            RunSecondary("AppendCarrierEvent", s => s.AppendCarrierEvent(record));
        }
        public void AppendSubstrateEvent(HistoryRecord record)
        {
            _primary.AppendSubstrateEvent(record);
            RunSecondary("AppendSubstrateEvent", s => s.AppendSubstrateEvent(record));
        }
        public void AppendSubstrateEventWithCarrier(HistoryRecord record)
        {
            _primary.AppendSubstrateEventWithCarrier(record);
            RunSecondary("AppendSubstrateEventWithCarrier", s => s.AppendSubstrateEventWithCarrier(record));
        }
        public void BindSubstrateToCarrier(DateTime time, int portId, string carrierKey, string carrierId, string substrateKey, string substrateName, string category)
        {
            _primary.BindSubstrateToCarrier(time, portId, carrierKey, carrierId, substrateKey, substrateName, category);
            RunSecondary("BindSubstrateToCarrier", s => s.BindSubstrateToCarrier(time, portId, carrierKey, carrierId, substrateKey, substrateName, category));
        }
        public void RenameSubstrate(DateTime time, string substrateKey, string oldName, string newName, string category)
        {
            _primary.RenameSubstrate(time, substrateKey, oldName, newName, category);
            RunSecondary("RenameSubstrate", s => s.RenameSubstrate(time, substrateKey, oldName, newName, category));
        }
        public void CompleteCarrier(DateTime time, int portId, string carrierKey, string carrierId, string lotId, List<string> substrateNames, string category)
        {
            _primary.CompleteCarrier(time, portId, carrierKey, carrierId, lotId, substrateNames, category);
            RunSecondary("CompleteCarrier", s => s.CompleteCarrier(time, portId, carrierKey, carrierId, lotId, substrateNames, category));
        }
        public void ClearPrevious(DateTime time, int portId, string carrierId, string loadPortName)
        {
            _primary.ClearPrevious(time, portId, carrierId, loadPortName);
            RunSecondary("ClearPrevious", s => s.ClearPrevious(time, portId, carrierId, loadPortName));
        }
        public void SweepOrphans()
        {
            _primary.SweepOrphans();
            RunSecondary("SweepOrphans", s => s.SweepOrphans());
        }
        public void WriteDiagnostic(string message)
        {
            _primary.WriteDiagnostic(message);
        }

        private void RunSecondary(string operationName, Action<IHistoryStore> action)
        {
            var secondary = _secondary;
            if (secondary == null)
                return;

            try
            {
                action(secondary);
            }
            catch (Exception ex)
            {
                _primary.WriteDiagnostic(string.Format("DB 이력 기록 실패 ({0}) : {1}", operationName, ex.Message));
            }
        }
        #endregion </Methods>
    }
}
