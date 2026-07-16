using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] PWA500Common의 LotHistoryLog에서 저장 메커니즘만 분리한 공용 엔진.
    /// (2단계: 영속화 로직을 IHistoryStore로 재분리 - 엔진은 큐잉/순서 보장/재시도만 담당)
    ///
    /// 프로젝트/고객사별로 달라지는 것(이벤트 어휘, 분류 체계, 메시지 문구)은 알지 못한다:
    /// - 이벤트 코드/분류는 HistoryRecord의 문자열 필드로 받는다.
    /// - 프로젝트별 파사드(예: LotHistoryLog)가 어휘와 문구를 정의하고 이 엔진을 호출한다.
    ///
    /// 로그 쓰기와 바인딩/개명/백업/정리의 순서 보장을 위해 모든 조작을 명령으로 큐잉하고
    /// 단일 스레드(ExecuteWriteAsync)에서 순서대로 저장소(IHistoryStore)에 반영한다.
    /// </summary>
    public sealed class LotHistoryEngine
    {
        #region <Constructors>
        public LotHistoryEngine(IHistoryStore store)
        {
            _store = store ?? throw new ArgumentNullException("store");

            _registeredPorts = new HashSet<int>();
        }
        #endregion </Constructors>

        #region <Types>
        enum HistoryCommandType
        {
            AppendCarrierEvent,
            AppendSubstrateEvent,
            AppendSubstrateEventWithCarrier,
            BindSubstrateToCarrier,
            RenameSubstrate,
            CompleteCarrier,
            ClearPrevious,
        }

        sealed class HistoryCommand
        {
            public HistoryCommandType Type;
            public DateTime Time;               // enqueue 시점(백업 날짜 폴더 기준)
            public int RetryCount;

            // Append*
            public HistoryRecord Record;

            // BindSubstrateToCarrier / RenameSubstrate / CompleteCarrier / ClearPrevious
            public int PortId;
            public string CarrierKey;
            public string CarrierId;
            public string LotId;
            public string LoadPortName;
            public List<string> Substrates;
            public string Category;
            public string SubstrateKey;
            public string SubstrateName;
            public string OldName;
            public string NewName;
        }
        #endregion </Types>

        #region <Fields>
        private const int MaxRetryCount = 3;

        private readonly IHistoryStore _store;
        private readonly HashSet<int> _registeredPorts;
        private readonly ConcurrentQueue<HistoryCommand> QueueToWrite = new ConcurrentQueue<HistoryCommand>();
        private readonly object _processLock = new object();
        private bool _orphanSweepDone = false;

        private Action<int, string> _logMessageToDisplay = null;
        #endregion </Fields>

        #region <Methods>

        #region <Appending>
        public void RegisterCarrierDirectory(int portId, string name)
        {
            _registeredPorts.Add(portId);
            _store.RegisterCarrierDirectory(portId, name);
        }
        public void AttachDisplayLogAction(Action<int, string> action)
        {
            _logMessageToDisplay = action;
        }
        /// <summary>캐리어 단위 이벤트를 캐리어 이력에 기록한다.</summary>
        public void AppendCarrierEvent(HistoryRecord record)
        {
            if (false == _registeredPorts.Contains(record.PortId))
                return;

            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.AppendCarrierEvent,
                Record = record,
            });
        }
        /// <summary>기판 단위 이벤트를 기판 이력에만 기록한다. (소속 캐리어 미확정 단계)</summary>
        public void AppendSubstrateEvent(HistoryRecord record)
        {
            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.AppendSubstrateEvent,
                Record = record,
            });
        }
        /// <summary>기판 단위 이벤트를 기판 이력과 소속 캐리어 이력에 동시 기록한다.</summary>
        public void AppendSubstrateEventWithCarrier(HistoryRecord record)
        {
            // 포트 미등록이면 기판 이력에만 기록 (기존 동작 유지)
            if (false == _registeredPorts.Contains(record.PortId))
            {
                AppendSubstrateEvent(record);
                return;
            }

            if (_logMessageToDisplay != null)
            {
                _logMessageToDisplay(record.PortId, HistoryLineFormat.Compose(record));
            }

            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.AppendSubstrateEventWithCarrier,
                Record = record,
            });
        }
        #endregion </Appending>

        #region <Operations>
        /// <summary>기판 이력 전체를 캐리어 이력에 귀속시킨다. (지연 바인딩: 안착 시점에 소속 확정)</summary>
        public void BindSubstrateToCarrier(int portId, string carrierKey, string carrierId, string substrateKey, string substrateName, string category)
        {
            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.BindSubstrateToCarrier,
                PortId = portId,
                CarrierKey = carrierKey,
                CarrierId = carrierId,
                SubstrateKey = substrateKey,
                SubstrateName = substrateName,
                Category = category,
            });
        }
        /// <summary>기판 이력의 표시 이름을 변경한다. (큐 FIFO이므로 구 이름 로그 -> 개명 -> 신 이름 로그 순서 보장)</summary>
        public void RenameSubstrate(string substrateKey, string oldName, string newName, string category)
        {
            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.RenameSubstrate,
                SubstrateKey = substrateKey,
                OldName = oldName,
                NewName = newName,
                Category = category,
            });
        }
        /// <summary>완료된 캐리어 이력과 기판 이력들을 랏 단위로 확정한다.</summary>
        public void CompleteCarrier(int portId, string carrierKey, string carrierId, string lotId, List<string> substrateNames, string category)
        {
            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.CompleteCarrier,
                PortId = portId,
                CarrierKey = carrierKey,
                CarrierId = carrierId,
                LotId = lotId,
                Substrates = substrateNames != null ? new List<string>(substrateNames) : null,
                Category = category,
            });
        }
        /// <summary>이전 작업의 잔여 캐리어 이력을 정리한다.</summary>
        public void ClearPreviousHistory(int portId, string carrierId, string loadportName)
        {
            Enqueue(new HistoryCommand
            {
                Type = HistoryCommandType.ClearPrevious,
                PortId = portId,
                CarrierId = carrierId,
                LoadPortName = loadportName,
            });
        }
        #endregion </Operations>

        #region <Executing>
        // 스캔당 진입 시점까지 쌓인 명령 전량 처리
        public void ExecuteWriteAsync()
        {
            if (false == _orphanSweepDone)
            {
                _orphanSweepDone = true;
                _store.SweepOrphans();
            }

            if (QueueToWrite.Count <= 0)
                return;

            lock (_processLock)
            {
                // 처리 중 재시도로 다시 큐에 들어간 명령은 다음 스캔에 처리되도록
                // 진입 시점의 개수만큼만 처리한다.
                int countToProcess = QueueToWrite.Count;
                for (int i = 0; i < countToProcess; ++i)
                {
                    if (false == QueueToWrite.TryDequeue(out HistoryCommand command))
                        break;

                    ProcessCommand(command, true);
                }
            }
        }

        /// <summary>프로그램 종료 시 큐에 남은 이력을 모두 기록한다. (재시도 없이 즉시 처리)</summary>
        public void FlushAll()
        {
            lock (_processLock)
            {
                while (QueueToWrite.TryDequeue(out HistoryCommand command))
                {
                    ProcessCommand(command, false);
                }
            }
        }

        private void Enqueue(HistoryCommand command)
        {
            command.Time = DateTime.Now;
            QueueToWrite.Enqueue(command);
        }

        private void ProcessCommand(HistoryCommand command, bool allowRetry)
        {
            try
            {
                switch (command.Type)
                {
                    case HistoryCommandType.AppendCarrierEvent:
                        _store.AppendCarrierEvent(command.Record);
                        break;
                    case HistoryCommandType.AppendSubstrateEvent:
                        _store.AppendSubstrateEvent(command.Record);
                        break;
                    case HistoryCommandType.AppendSubstrateEventWithCarrier:
                        _store.AppendSubstrateEventWithCarrier(command.Record);
                        break;
                    case HistoryCommandType.BindSubstrateToCarrier:
                        _store.BindSubstrateToCarrier(command.Time, command.PortId, command.CarrierKey, command.CarrierId, command.SubstrateKey, command.SubstrateName, command.Category);
                        break;
                    case HistoryCommandType.RenameSubstrate:
                        _store.RenameSubstrate(command.Time, command.SubstrateKey, command.OldName, command.NewName, command.Category);
                        break;
                    case HistoryCommandType.CompleteCarrier:
                        _store.CompleteCarrier(command.Time, command.PortId, command.CarrierKey, command.CarrierId, command.LotId, command.Substrates, command.Category);
                        break;
                    case HistoryCommandType.ClearPrevious:
                        _store.ClearPrevious(command.Time, command.PortId, command.CarrierId, command.LoadPortName);
                        break;
                }
            }
            catch (Exception ex)
            {
                // 일시적 파일 잠금 등은 다음 스캔에 재시도한다.
                // (저장소 구현은 이미 반영된 항목을 건너뛰므로 재실행해도 안전)
                if (allowRetry && command.RetryCount < MaxRetryCount)
                {
                    command.RetryCount += 1;
                    QueueToWrite.Enqueue(command);
                }
                else
                {
                    _store.WriteDiagnostic(string.Format("{0} 처리 실패 (재시도 {1}회 소진) : {2}", command.Type, command.RetryCount, ex.Message));
                }
            }
        }
        #endregion </Executing>

        #endregion </Methods>
    }
}
