using System;
using EFEM.Defines.LoadPort;

namespace EFEM.Modules.LoadPort.State
{
    /// <summary>
    /// E87 Carrier State Model의 최소 골격.
    ///
    /// 현재 역할:
    /// - CarrierManagementServer가 가진 raw CarrierAccessingStatus observation을 받아온다.
    /// - 이전 상태와 현재 상태를 비교해 변화 여부와 이벤트 데이터를 계산한다.
    /// - 계산된 현재 상태를 LoadPortStateInformation으로 복사할 수 있게 한다.
    ///
    /// 주의:
    /// - 이 객체는 raw truth source가 아니다.
    /// - raw truth는 CarrierManagementServer / Carrier object 쪽에 있고,
    ///   여기서는 E87 문맥의 carrier-facing state mirror / compare 역할만 맡는다.
    /// - 실제 CarrierAccessingStateChanged 이벤트 발행은
    ///   상위 E87LoadPortStateModel이 담당한다.
    /// </summary>
    public sealed class E87CarrierStateModel
    {
        private readonly int _portId;
        private readonly object _sync = new object();

        // 현재 Evaluate cycle에 반영할 raw observation 값.
        // source of truth는 CarrierManagementServer 쪽에 있다.
        private CarrierAccessStates _observedCarrierAccessingState;

        // E87 carrier state model이 현재 보유하는 해석 결과.
        // LoadPortStateInformation으로 projection 되는 값이다.
        private CarrierAccessStates _carrierAccessingState;

        public E87CarrierStateModel(int portId)
        {
            _portId = portId;
            _observedCarrierAccessingState = CarrierAccessStates.NotAccessed;
            _carrierAccessingState = CarrierAccessStates.NotAccessed;
        }

        public int PortId
        {
            get { return _portId; }
        }

        public CarrierAccessStates CarrierAccessingState
        {
            get
            {
                lock (_sync)
                {
                    return _carrierAccessingState;
                }
            }
        }

        public void Initialize()
        {
            lock (_sync)
            {
                _observedCarrierAccessingState = CarrierAccessStates.NotAccessed;
                _carrierAccessingState = CarrierAccessStates.NotAccessed;
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                _observedCarrierAccessingState = CarrierAccessStates.NotAccessed;
                _carrierAccessingState = CarrierAccessStates.NotAccessed;
            }
        }
        
        // 복구 시점에 호출하여 마지막 값을 갱신한다.
        public void Synchronize(CarrierAccessStates currentState)
        {
            lock (_sync)
            {
                _observedCarrierAccessingState = currentState;
                _carrierAccessingState = currentState;
            }
        }

        /// <summary>
        /// CarrierManagementServer에서 읽어온 raw CarrierAccessingStatus observation을 저장한다.
        /// 이 메서드는 상태를 즉시 바꾸지 않고,
        /// Evaluate() 시점에 반영될 입력만 갱신한다.
        /// </summary>
        public void UpdateObservation(CarrierAccessStates observedState)
        {
            lock (_sync)
            {
                _observedCarrierAccessingState = observedState;
            }
        }

        /// <summary>
        /// 이번 cycle의 observation을 현재 상태로 반영한다.
        /// 상태 변화가 있으면 이전/현재 상태 정보를 changedEvent로 반환한다.
        /// 실제 CarrierAccessingStateChanged 이벤트 발행은
        /// 상위 E87LoadPortStateModel이 Evaluate() 마지막(lock 밖)에서 담당한다.
        /// </summary>
        public bool Evaluate(out CarrierAccessingStateChangedEvent changedEvent)
        {
            changedEvent = default(CarrierAccessingStateChangedEvent);
            bool changed = false;

            lock (_sync)
            {
                var prev = _carrierAccessingState;
                var next = _observedCarrierAccessingState;

                if (prev != next)
                {
                    _carrierAccessingState = next;
                    changed = true;

                    changedEvent = new CarrierAccessingStateChangedEvent
                    {
                        PortId = _portId,
                        PreviousState = prev,
                        CurrentState = next
                    };
                }
            }

            return changed;
        }

        /// <summary>
        /// 현재 carrier state model의 결과를 LoadPortStateInformation에 반영한다.
        /// LoadPort transfer 상태 해석은 이 projection된 값을 참조한다.
        /// </summary>
        public void CopyTo(LoadPortStateInformation state)
        {
            lock (_sync)
            {
                state.CarrierAccessingState = _carrierAccessingState;
            }
        }
    }
}