using System;
using System.Collections.Generic;

using TickCounter_;

using Define.DefineEnumBase.Common;

using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using FrameOfSystem3.Task;

namespace EFEM.Modules.LoadPort.LoadPortControllers
{
    class LoadPortControllerSimulator : LoadPortController
    {
        #region <Constructors>
        public LoadPortControllerSimulator(int portId, string name, EN_CONNECTION_TYPE interfaceType, int commIndex) : base(portId, name, interfaceType, commIndex) 
        {
            _taskOperator = TaskOperator.GetInstance();
        }
        #endregion </Constructors>

        #region <Fields>
        //private LoadPortLoadingTypes _currentCarrierMode;

        private readonly TickCounter _delayForAction = new TickCounter();
        private static TaskOperator _taskOperator = null;

        #region <Status Fields>
        private bool _temporaryPresent = false;
        private bool _temporaryPlaced = false;
        private bool _temporaryClamped = false;
        private bool _temporaryDocked = false;
        private bool _temporaryDoorState = false;

        private LoadPortAccessMode _currentMode = LoadPortAccessMode.Manual;
        private LoadPortLoadingMode _loadingMode = LoadPortLoadingMode.Cassette;
        #endregion </Status Fields>

        #region <Event Fields>
        public bool _tpConnected = false;
        public bool _tpDisconnected = false;
        public bool _loadUnloadButtonPushed = false;
        public bool _podIn = false;
        public bool _podOut = false;
        public bool _startResetAll = false;
        public bool _endResetAll = false;
        public bool _foupStarted = false;
        public bool _foupIncorrectPos = false;
        public bool _estopPushed = false;
        #endregion </Event Fields>

        #endregion </Fields>

        #region <Methods>

        #region <Init/Close>
        public override bool InitController()
        {           
            return true;
            //throw new NotImplementedException();
        }

        public override bool CloseController()
        {
            throw new NotImplementedException();
        }        
        #endregion </Init/Close>

        #region <Actions>
        public override CommandResults DoInitialize()
        {
            return ExecuteCommand(LoadPortCommands.Initialize);
        }

        public override CommandResults DoLoad()
        {
            var result = CommandResult.Proceed;

            switch (_actionStep)
            {
                case 0:
                    {
                        _delayForAction.SetTickCount(500);
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        _delayForAction.SetTickCount(2000);
                        _temporaryClamped = true;
                        UpdateLogicalStates();
                        ++_actionStep;
                    }
                    break;
                case 2:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        _temporaryDocked = true;
                        UpdateLogicalStates();
                        uint delay = 5000 + (uint)PortId * 50;
                        _delayForAction.SetTickCount(delay);
                        ++_actionStep;
                    }
                    break;
                case 3:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        _temporaryDoorState = true;
                        UpdateLogicalStates();
                        UpdateSlotMap();
                        result = CommandResult.Completed;
                    }
                    break;

                default:
                    result = CommandResult.Invalid;
                    break;
            }

            if (false == result.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
            }

            _result.CommandResult = result;
            return _result;
        }

        public override CommandResults DoUnload()
        {
            var result = CommandResult.Proceed;

            switch (_actionStep)
            {
                case 0:
                    {
                        _delayForAction.SetTickCount(5000);
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        _temporaryDoorState = false;
                        UpdateLogicalStates();
                        _delayForAction.SetTickCount(2000);
                        ++_actionStep;
                    }
                    break;
                case 2:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        _temporaryDocked = false;
                        //_temporaryPlaced = false;
                        UpdateLogicalStates();

                        _delayForAction.SetTickCount(500);
                        ++_actionStep;
                    }
                    break;
                case 3:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        _temporaryClamped = false;
                        //_temporaryPlaced = true;
                        UpdateLogicalStates();
                        //UpdateSlotMap();
                        result = CommandResult.Completed;
                    }
                    break;

                default:
                    result = CommandResult.Invalid;
                    break;
            }

            if (false == result.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
            }

            _result.CommandResult = result;
            return _result;
        }

        public override CommandResults DoClamp()
        {
            return ExecuteCommand(LoadPortCommands.Clamp);
        }

        public override CommandResults DoUnClamp()
        {
            return ExecuteCommand(LoadPortCommands.Unclamp);
        }

        public override CommandResults DoDock()
        {
            return ExecuteCommand(LoadPortCommands.Dock);
        }

        public override CommandResults DoUnDock()
        {
            return ExecuteCommand(LoadPortCommands.Undock);
        }

        public override CommandResults DoOpenDoor()
        {
            return ExecuteCommand(LoadPortCommands.DoorOpen);
        }

        public override CommandResults DoCloseDoor()
        {
            return ExecuteCommand(LoadPortCommands.DoorClose);
        }
        public override CommandResults DoScan()
        {
            return ExecuteCommand(LoadPortCommands.ScanDown);
        }
        public override CommandResults DoGetSlotMap()
        {
            return ExecuteCommand(LoadPortCommands.GetMap);
        }
        public override CommandResults DoFindLoadingMode()
        {
            ChangeLoadingMode(LoadPortLoadingMode.ClosedCassette);
            return new CommandResults(LoadPortCommands.FindLoadingMode.ToString(), CommandResult.Completed);
        }
        public override CommandResults DoChangeLoadingMode(LoadPortLoadingMode mode)
        {
            if (mode == LoadPortLoadingMode.Cassette)
            {
                return ExecuteCommand(LoadPortCommands.ChangeToCassette);
            }
            else if (mode == LoadPortLoadingMode.ClosedCassette)
            {
                return ExecuteCommand(LoadPortCommands.ChangeToClosedCassette);
            }
            else
            {
                return ExecuteCommand(LoadPortCommands.ChangeToFoup);
            }
        }
        public override CommandResults DoChangeAccessMode(LoadPortAccessMode mode)
        {
            if (mode.Equals(LoadPortAccessMode.Auto))
                return ExecuteCommand(LoadPortCommands.ChangeAccessModeToAuto);
            else
                return ExecuteCommand(LoadPortCommands.ChangeAccessModeToManual);
        }
        public override CommandResults DoClearAlarm()
        {
            return ExecuteCommand(LoadPortCommands.Reset);
        }
        public override CommandResults DoAmpControl(bool enabled)
        {
            if (enabled)
                return ExecuteCommand(LoadPortCommands.AmpOn);
            else
                return ExecuteCommand(LoadPortCommands.AmpOff);
        }

        #endregion </Actions>

        #region <States>
        
        #region <Simul Only>
        //public void UpdateCarrierPresence(bool presence)
        //{
        //    _temporaryPlaced = presence;
        //    _temporaryPresent = presence;
        //    UpdateLogicalStates();
        //}

        //public void UpdateInitialization(bool enabled)
        //{
        //    UpdateInitializationState(enabled);
        //}
        #endregion </Simul Only>

        public override void OnIndicatorChanged(LoadPortIndicatorTypes indicator, LoadPortIndicatorStates state)
        {
            throw new NotImplementedException();
        }
        public override string GetTriggeredControllerAlarm()
        {
            return string.Empty;
        }
        #endregion </States>

        #region <Thread>
        protected override bool RemoveTokens(string receivedMessage, ref string newString)
        {
            return true;
        }

        // 받은 메시지를 파싱한다.
        protected override void ParseMessages(string receivedMessage)
        {
            // Simul을 위해
            if (_taskOperator.SimulLoadPortPlaced.ContainsKey(PortId))
            {
                _taskOperator.SimulLoadPortPlaced.TryRemove(PortId, out _);

                _temporaryPresent = true;
                _temporaryPlaced = true;
                
                UpdateLogicalStates();
            }

            if (_taskOperator.SimulLoadPortRemoved.ContainsKey(PortId))
            {
                _taskOperator.SimulLoadPortRemoved.TryRemove(PortId, out _);

                _temporaryPresent = false;
                _temporaryPlaced = false;

                UpdateLogicalStates();
            }

            if (_taskOperator.SimulLoadPortLoadClicked.ContainsKey(PortId))
            {
                _taskOperator.SimulLoadPortLoadClicked.TryRemove(PortId, out _);

                OnButtonPressed(LoadPortButtonTypes.Load);
            }

            if (_taskOperator.SimulLoadPortUnloadClicked.ContainsKey(PortId))
            {
                _taskOperator.SimulLoadPortUnloadClicked.TryRemove(PortId, out _);

                OnButtonPressed(LoadPortButtonTypes.Unload);
            }
        }
        #endregion </Thread>

        #region <Internals>
        private CommandResults ExecuteCommand(LoadPortCommands command)
        {
            var result = CommandResult.Proceed;
            switch (_actionStep)
            {
                case 0:
                    {
                        _delayForAction.SetTickCount(1000);
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        switch (command)
                        {
                            case LoadPortCommands.Load:
                                {
                                    _temporaryClamped = true;
                                    _temporaryDocked = true;
                                    _temporaryDoorState = true;
                                    UpdateLogicalStates();
                                    UpdateSlotMap();
                                }
                                break;
                            case LoadPortCommands.Unload:
                                {
                                    _temporaryClamped = false;
                                    _temporaryDocked =  false;
                                    _temporaryDoorState = false;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.Clamp:
                                {
                                    _temporaryClamped = true;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.Unclamp:
                                {
                                    _temporaryClamped = false;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.Dock:
                                {
                                    _temporaryDocked = true;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.Undock:
                                {
                                    _temporaryDocked = false;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.DoorOpen:
                                {
                                    _temporaryDoorState = true;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.DoorClose:
                                {
                                    _temporaryDoorState = false;
                                    UpdateLogicalStates();
                                }
                                break;
                            case LoadPortCommands.Initialize:
                                {
                                    _temporaryDoorState = false;
                                    _temporaryDocked = false;
                                    _temporaryClamped = false;
                                    if (CarrierManagementServer.Instance.HasCarrier(PortId))
                                    {
                                        _temporaryPresent = true;
                                        _temporaryPlaced = true;
                                    }
                                    
                                    UpdateLogicalStates();
                                    _delayForAction.SetTickCount(1000);

                                    ++_actionStep;                                   
                                    _result.CommandResult = CommandResult.Proceed;
                                    return _result;
                                    //UpdateInitializationState(true);
                                }
                                
                            case LoadPortCommands.Scan:
                            case LoadPortCommands.ScanDown:
                            case LoadPortCommands.GetMap:
                                {
                                    UpdateSlotMap();
                                }
                                break;
                            case LoadPortCommands.Reset:
                                break;
                            case LoadPortCommands.AmpOn:
                                break;
                            case LoadPortCommands.AmpOff:
                                break;
                            case LoadPortCommands.GetState:
                                break;
                            case LoadPortCommands.ChangeToClosedCassette:
                                _loadingMode = LoadPortLoadingMode.ClosedCassette;
                                ChangeLoadingTypeState(_loadingMode);
                                break;
                            case LoadPortCommands.ChangeToCassette:
                                _loadingMode = LoadPortLoadingMode.Cassette;
                                ChangeLoadingTypeState(_loadingMode);
                                break;
                            case LoadPortCommands.ChangeToFoup:
                                _loadingMode = LoadPortLoadingMode.Foup;
                                ChangeLoadingTypeState(_loadingMode);
                                break;
                            case LoadPortCommands.ChangeAccessModeToAuto:
                                _currentMode = LoadPortAccessMode.Auto;
                                ChangeAccessingState(_currentMode);
                                break;
                            case LoadPortCommands.ChangeAccessModeToManual:
                                _currentMode = LoadPortAccessMode.Manual;
                                ChangeAccessingState(_currentMode);
                                break;

                            default:
                                break;
                        }

                        result = CommandResult.Completed;
                    }
                    break;
                
                case 2:
                    {
                        if (false == _delayForAction.IsTickOver(true))
                            break;

                        ChangeInitializationState(true);
                        ChangeAccessingState(_currentMode);
                        ChangeLoadingTypeState(_loadingMode);
                        result = CommandResult.Completed;
                    }
                    break;
                default:
                    result = CommandResult.Invalid;
                    break;
            }

            if (false == result.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
            }

            _result.CommandResult = result;
            return _result;
        }

        private void UpdateLogicalStates()
        {
            ChangePresentState(_temporaryPresent);
            ChangePlacedState(_temporaryPlaced);
            ChangeClampState(_temporaryClamped);
            ChangeDockState(_temporaryDocked);
            ChangeDoorState(_temporaryDoorState);
        }
        private void UpdateSlotMap()
        {
            int available = 13;
            bool hasAnySubstrates = SubstrateManager.Instance.HasAnySubstrateAtLoadPort(PortId);
            Dictionary<int, CarrierSlotMapStates> slotState = new Dictionary<int, CarrierSlotMapStates>();
            if (false == hasAnySubstrates)
            {
                for (int i = 1; i <= available; ++i)
                {
                    if (_taskOperator.IsDryRunMode() || i == 1)
                    {
                        slotState[i] = CarrierSlotMapStates.Empty;
                    }
                    else
                    {
                        slotState[i] = CarrierSlotMapStates.CorrectlyOccupied;
                    }
                }
            }
            else
            {
                for (int i = 1; i <= available; ++i)
                {
                    bool hasSubstrateAtSlot = SubstrateManager.Instance.HasSubstrateAtLoadPort(PortId, i);
                    if (false == hasSubstrateAtSlot)
                        slotState[i] = CarrierSlotMapStates.Empty;
                    else
                        slotState[i] = CarrierSlotMapStates.CorrectlyOccupied;
                }
            }
            ChangeSlotMap(slotState);
        }
        #endregion </Internals>

        #endregion </Methods>
    }
}
