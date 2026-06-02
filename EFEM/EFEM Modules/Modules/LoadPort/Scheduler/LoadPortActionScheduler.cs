using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Recipe;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.Defines.AtmRobot;
using EFEM.Modules.LoadPort;
using EFEM.Modules.AtmRobot;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking;

namespace EFEM.Modules.LoadPort.Scheduler
{
    public sealed class LoadPortActionScheduler
    {
        #region <Constructors>
        public LoadPortActionScheduler(int lpIndex, int portId)
        {
            _loadPortManager = LoadPortManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _substrateManager = SubstrateManager.Instance;

            Index = lpIndex;
            PortId = portId;

            _loadPortInformation = new LoadPortStateInformation();

            _carrierCompletionCondition = new DefaultCarrierCompletionCondition();
            _carrierCompletionHandlingPolicy = new DefaultCarrierCompletionHandlingPolicy();
        }
        #endregion </Constructors>

        #region <Fields>
        protected static LoadPortManager _loadPortManager = null;
        protected static CarrierManagementServer _carrierServer = null;
        protected static SubstrateManager _substrateManager = null;

        protected LoadPortStateInformation _loadPortInformation = null;

        private readonly string ParamSecsGem = PARAM_COMMON.UseSecsGem.ToString();
        
        private Recipe _recipe = Recipe.GetInstance();

        private ICarrierCompletionCondition _carrierCompletionCondition;
        private ICarrierCompletionHandlingPolicy _carrierCompletionHandlingPolicy;
        #endregion </Fields>

        #region <Properties>
        public int Index { get; private set; }
        public int PortId { get; private set; }
        protected Recipe RecipeInstance
        {
            get
            {
                return _recipe;
            }
        }
        protected bool UseSecsGem
        {
            get
            {
                return _recipe.GetValue(EN_RECIPE_TYPE.COMMON, ParamSecsGem, true);
            }
        }
        #endregion </Properties>

        #region <Methods>
        public void RegisterCompletionCondition(ICarrierCompletionCondition condition)
        {
            _carrierCompletionCondition = condition;
        }
        public void RegisterCompletionHandlingPolicy(ICarrierCompletionHandlingPolicy policy)
        {
            _carrierCompletionHandlingPolicy = policy;
        }
        public void TryFinalizePendingCarrierCompletion(
            int portId,
            LoadPortStateInformation loadPortInformation)
        {
            if (portId != PortId)
                return;

            if (_carrierCompletionHandlingPolicy == null)
                return;

            // action 결정 없이 pending 완료 확정만 시도한다.
            _carrierCompletionHandlingPolicy.TryFinalizeCompletion(
                portId,
                loadPortInformation);
        }
        public CARRIER_PORT_TYPE ExecuteSchedulers()
        {
            LoadPortStateInformation currentState =
                _loadPortManager.GetLoadPortState(Index);

            if (currentState == null)
                return CARRIER_PORT_TYPE.SELECTION;

            _loadPortInformation = new LoadPortStateInformation();
            currentState.CopyTo(ref _loadPortInformation);

            bool isCarrierCompleted = IsCarrierCompleted();

            if (_carrierCompletionHandlingPolicy != null)
            {
                if (isCarrierCompleted)
                {
                    _carrierCompletionHandlingPolicy.RequestCompletion(PortId);
                }

                _carrierCompletionHandlingPolicy.TryFinalizeCompletion(
                    PortId,
                    _loadPortInformation);
            }

            return DecideNextAction();
        }
        //public CARRIER_PORT_TYPE ExecuteSchedulers()
        //{
        //    _loadPortInformation = _loadPortManager.GetLoadPortState(Index);

        //    bool isCarrierCompleted = IsCarrierCompleted();

        //    if (_carrierCompletionHandlingPolicy != null)
        //    {
        //        if (isCarrierCompleted)
        //        {
        //            // 완료 조건 만족 사실만 policy에 기록한다.
        //            _carrierCompletionHandlingPolicy.RequestCompletion(PortId);
        //        }

        //        // 현재 scheduler cycle에서도 확정 가능하면 즉시 처리한다.
        //        _carrierCompletionHandlingPolicy.TryFinalizeCompletion(
        //            PortId,
        //            _loadPortInformation);
        //    }

        //    return DecideNextAction();
        //}
        public void ChangeSlotMapForDryRun()
        {
            //if (false == _carrierServer.HasCarrier(PortId))
            //    return;

            //var slotMaps = _carrierServer.GetCarrierSlotMap(PortId);
            //if (slotMaps == null)
            //    return;

            //Dictionary<int, CarrierSlotMapStates> newMap = new Dictionary<int, CarrierSlotMapStates>();
            //foreach (var item in slotMaps)
            //{
            //    LoadPortLocation location = new LoadPortLocation(PortId, item.Key, "");
            //    if (_locationServer.GetLoadPortSlotLocation(PortId, item.Key, ref location))
            //    {
            //        bool hasSubstrate = _substrateManager.HasSubstrateAtLoadPort(PortId, item.Key);
            //        switch (item.Value)
            //        {
            //            case CarrierSlotMapStates.Empty:
            //                {
            //                    newMap[item.Key] = CarrierSlotMapStates.CorrectlyOccupied;
            //                    if (false == hasSubstrate)
            //                    {
            //                        _substrateManager.CreateSubstrate(location.Name, location.Name, location);
            //                    }
            //                }
            //                break;
            //            case CarrierSlotMapStates.NotEmpty:
            //            case CarrierSlotMapStates.CorrectlyOccupied:
            //            case CarrierSlotMapStates.DoubleSlotted:
            //            case CarrierSlotMapStates.CrossSlotted:
            //                {
            //                    newMap[item.Key] = CarrierSlotMapStates.Empty;
            //                    if (hasSubstrate)
            //                    {
            //                        string key = _substrateManager.GetSubstrateKeyAtLoadPort(PortId, item.Key);
            //                        _substrateManager.RemoveSubstrateByKey(key);
            //                    }
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}

            //_carrierServer.SetCarrierSlotMap(PortId, newMap);
            ////_substrateManager.SaveRecoveryDataAll();
        }
        private CARRIER_PORT_TYPE DecideNextAction()
        {
            switch (_loadPortInformation.TransferState)
            {
                case LoadPortTransferStates.TransferBlocked:
                    {
                        if (ShouldUnloadCarrier())
                        {
                            if (_loadPortInformation.DoorState ||
                                _loadPortInformation.DockState ||
                                _loadPortInformation.ClampState)
                            {
                                return CARRIER_PORT_TYPE.ACTION_UNLOAD;
                            }
                            else
                            {
                                return CARRIER_PORT_TYPE.READY_TO_UNLOAD;
                            }
                        }
                        else
                        {
                            if (false == _loadPortInformation.DoorState)
                            {
                                return CARRIER_PORT_TYPE.ACTION_LOAD;
                            }
                        }
                    }
                    break;

                case LoadPortTransferStates.ReadyToLoad:
                    return CARRIER_PORT_TYPE.READY_TO_LOAD;

                case LoadPortTransferStates.ReadyToUnload:
                    return CARRIER_PORT_TYPE.READY_TO_UNLOAD;

            }

            return CARRIER_PORT_TYPE.SELECTION;
        }
        private bool ShouldUnloadCarrier()
        {
            if (_carrierCompletionHandlingPolicy == null)
                return false;

            return _carrierCompletionHandlingPolicy.ShouldUnloadCarrier(PortId);
        }
        private bool IsCarrierCompleted()
        {
            if (_carrierCompletionCondition == null)
                return false;

            return _carrierCompletionCondition.IsCarrierCompleted(
                PortId,
                _loadPortInformation.TransferState);
        }
        #endregion </Methods>
    }
}
