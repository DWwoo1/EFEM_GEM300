using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Define.DefineEnumBase.Common;
using FrameOfSystem3.Functional;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;

namespace EFEM.Modules.LoadPort.LoadPortControllers
{
    // 2024.11.14. by dwlim [ADD] SELOP8 LED I/F 추가
    public enum LoadPortLEDTypes
    {
        // LoadPort 상단 LED
        //None = 0,
        Load_Top = 1,
        Unload_Top = 2,
        Manual = 3,
        Auto = 4,
        Reserved = 5,
        Alarm = 6,

        // LoadPort 중간 커버 LED
        Load_Middle = 11,
        Unload_Middle = 12,
        MAC = 13,
        Metal = 14,
    }
    public enum LEDLightingTypes
    {
        None = 0,
        On = 1,
        Off = 2,
        Blink = 3,
    }
    // 2024.11.14. by dwlim [END]
    class SELOP8Controller : LoadPortController
    {
        #region <Constructors>
        public SELOP8Controller(int portId, string name, EN_CONNECTION_TYPE interfaceType, int commIndex) : base(portId, name, interfaceType, commIndex)
        {
            // 2024.11.13. by dwlim [MOD] ACCESS MODE 저장
            _PortId = portId;
            //ReadAccessModeFile();
            ReadLoadingModeFile();
        }
        #endregion </Constructors>

        #region Status Types
        private const int FoupPlacementStatus = 6;
        private const int FoupClampStatus = 7;
        private const int FoupDockStatus = 13;
        private const int ZaxisPosition = 15;
        #endregion

        #region Event Types
        private const int ClampPositionSensorMask = 1 << 0;
        private const int FoupDetectSensorMask1 = 1 << 24;
        private const int FoupStabilizedSensorMask1 = 1 << 26;
        private const int FoupStabilizedSensorMask2 = 1 << 27;
        private const int FoupStabilizedSensorMask3 = 1 << 28;

        #endregion

        #region <Fields>
        private const char SendStartToken = 's';
        private const char EndToken = ';';
        private const string LoadPortADR = "00";

        private const string AckMessage = "ACK";
        private const string NakMessage = "NAK";
        private const string StatusMessage = "STATE";
        private const string MapStatusMessage = "MAPRD";
        private const string VerMessage = "VERSN";
        private const string ParameterMessage = "PARAM";
        private const string LEDStatusMessage = "LEDST";
     
        private const string NormalCompleteMessage = "INF";
        private const string RetransmissionINF = "RIF";
        private const string AbnormalCompleteMessage = "ABS";
        private const string RetransmissionABS = "RAS";
        private const string FINmessage = "FIN";
        private const string EventMessage = "INPUT";

        private const string CarrierExist = "PODON";
        private const string CarrierNotexist = "PODOF";
        private const string CarrierPresented = "SMTON";

        private const string LightOn = "LON";
        private const string LightOff = "LOF";
        private const string LightBlink = "LBL";

        private const char SlotStatusEmpty = '0';
        private const char SlotStatusCorrectlyOccupied = '1';
        private const char SlotStatusCross = '2';
        private const char SlotStatusUpDownTilted = '4';
        private const char SlotStatusThicknessErrorThick = 'W';
        private const char SlotStatusThicknessErrorThin = 'T';

        private const int StatusMessageLength = 20;
        private const int LEDStatusMessageLength = 14;

        private readonly ConcurrentDictionary<LoadPortCommands, CommandResults> _commandResults
            = new ConcurrentDictionary<LoadPortCommands, CommandResults>();

        private delegate CommandResults _callBackHandleLEDCommand(LoadPortCommands command);

        private LoadPortLoadingMode _currentCarrierMode;
        private LoadPortAccessMode _currentAccessMode;      // 2024.11.13. by dwlim [ADD] ACCESS MODE 저장
        private LoadPortLEDTypes _LEDType;                  // 2024.11.14. by dwlim [ADD] SELOP8 LED I/F 추가
        private LoadPortCommands _LEDLightingCommand;

        private int _capacity = 0;
        private string[] _doingActionData;

        private const uint TimeLong = 30000;
        private const uint TimeMiddle = 10000;
        private const uint TimeShort = 5000;

        private const int SlotMaxCount = 30;    // CYMECHS 26
        private const int SlotMinCount = 4;     // CYMECHS 1

        #region <Config>
        private readonly int _PortId;
        private readonly string SECTION_NAME = "LOADPORT";

        // 2024.11.13. by dwlim [ADD] ACCESS MODE 저장
        private const string KEY_ACCESS_MODE = "ACCESS_MODE";
        private int _accessMode;
        // 2024.11.13. by dwlim [END]

        // 2025.03.25. by dwlim [ADD] LOADING MODE 저장
        private const string KEY_LOADING_MODE = "LOADING_MODE";
        private int _loadingMode;
        // 2025.03.25. by dwlim [END]

        private int _callbackActionStep = 0;
        #endregion </Config>

        #region <Status Fields>
        private bool _temporaryPresent = false;
        private bool _temporaryPlaced = false;
        private bool _temporaryClamped = false;
        private bool _temporaryDocked = false;
        private bool _temporaryDoorState = false;

        //private bool _temporaryInitialized = false;
        private bool _temporaryAutoMode = false;
        private bool _temporaryInitialized = false;

        // 2024.10.11. by dwlim [ADD] _temporaryMode값 '1'로 초기화
        //private char _temporaryMode = '1';                      // 0 : Host Mode            1 : Manual Mode
        #endregion </Status Fields>

        #region <Event Fields>
        private const string LoadButtonPushed = "LODSW";
        private const string UnloadButtonPushed = "ULDSW";
        private const string FoupModeButtonPushed = "MACSW";
        private const string CassetteModeButtonPushed = "MTLSW";

        private const int FoupPresence1 = 1 << 24;
        private const int FoupPlacementMask1 = 1 << 26;
        private const int FoupPlacementMask2 = 1 << 27;
        private const int FoupPlacementMask3 = 1 << 28;
        #endregion </Event Fields>

        #endregion </Fields>

        #region <Enumerations>
        #endregion </Enumerations>

        #region <Methods>

        #region <Init/Close>
        public override bool InitController()
        {
            //System.Threading.Tasks.Task.Run(() => DoInitController());

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
            return ExecuteInitialization(HandleSelectedLED);
        }

        public override CommandResults DoLoad()
        {
            return ExecuteCommandWithMap(LoadPortCommands.Load, HandleSelectedLED);
        }

        public override CommandResults DoUnload()
        {
            return ExecuteCommandWithMap(LoadPortCommands.Unload, HandleSelectedLED);
        }

        public override CommandResults DoClamp()
        {
            return ExecuteCommand(LoadPortCommands.Clamp, null);
        }

        public override CommandResults DoUnClamp()
        {
            return ExecuteCommand(LoadPortCommands.Unclamp, null);
        }

        public override CommandResults DoDock()
        {
            return ExecuteCommand(LoadPortCommands.Dock, null);
        }

        public override CommandResults DoUnDock()
        {
            return ExecuteCommand(LoadPortCommands.Undock, null);
        }

        public override CommandResults DoOpenDoor()
        {
            return ExecuteCommandWithMap(LoadPortCommands.DoorOpen, null);
        }

        public override CommandResults DoCloseDoor()
        {
            return ExecuteCommandWithMap(LoadPortCommands.DoorClose, null);
        }
        public override CommandResults DoScan()
        {
            return ExecuteCommandWithMap(LoadPortCommands.ScanDown, null);
        }
        public override CommandResults DoGetSlotMap()
        {
            return ExecuteCommandWithMap(LoadPortCommands.GetMap, null);
        }
        public override CommandResults DoFindLoadingMode()
        {
            return ExecuteFindingLoadingMode(LoadPortCommands.FindLoadingMode);
        }
        public override CommandResults DoChangeLoadingMode(LoadPortLoadingMode mode)
        {
            return ExecuteChangingMode(mode);
        }
        public override CommandResults DoClearAlarm()
        {
            return ExecuteResetOrStateCommand();
        }
        public override CommandResults DoAmpControl(bool enabled)
        {
            if (enabled)
                return ExecuteCommand(LoadPortCommands.AmpOn, null);
            else
                return ExecuteCommand(LoadPortCommands.AmpOff, null);
        }
        public override CommandResults DoChangeAccessMode(LoadPortAccessMode mode)
        {
            return ExecuteChangingAccessMode(mode);
        }
        #endregion </Actions>

        #region <States>
        public override void OnIndicatorChanged(LoadPortIndicatorTypes indicator, LoadPortIndicatorStates state)
        {
            throw new NotImplementedException();
        }
        public override string GetTriggeredControllerAlarm()
        {
            return _triggeredAlarm;
        }
        #endregion </States>

        #region <Thread>
        protected override bool RemoveTokens(string receivedMessage, ref string newString)
        {
            int index = receivedMessage.IndexOf(EndToken);
            if (index < 0)
                return false;

            string strMessage = receivedMessage.Remove(index);
            newString = strMessage.Remove(strMessage.IndexOf(SendStartToken), (SendStartToken + LoadPortADR).Length);

            return true;
        }

        // 받은 메시지를 파싱한다.
        protected override void ParseMessages(string receivedMessage)
        {
            // 1. 받은 메시지를 이용하여 파싱
            // 일단 Ack는 파싱할 필요가 없을 것 같다.
            if (false == ParseDatas(receivedMessage))
            {
                return;
            }
            if (receivedMessage.StartsWith(NormalCompleteMessage) || receivedMessage.StartsWith(RetransmissionINF))
            {
                if (_doingActionData[1].Equals(LoadButtonPushed))
                {
                    OnButtonPressed(LoadPortButtonTypes.Load);
                }
                else if (_doingActionData[1].Equals(UnloadButtonPushed))
                {
                    OnButtonPressed(LoadPortButtonTypes.Unload);
                }
                else if(_doingActionData[1].Equals(CarrierNotexist))
                {
                    ChangePlacedState(false);
                    ChangePresentState(false);
                }
                else if (_doingActionData[1].Equals(CarrierExist))
                {
                    ChangePlacedState(true);
                    ChangePresentState(true);
                }
                // 2024.12.19. dwlim [ADD] Carrier를 제거하면서 마지막에 Present가 감지됨. Place와 같이 봐야할듯
                else if (_doingActionData[1].Equals(CarrierPresented))
                {
                    //ChangePresentState(true);
                }
                // PODON : PRESENT, PLACE 감지
                // END

                _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Completed));

                if (_doingAction == LoadPortCommands.Initialize)
                {
                    ChangeInitializationState(true);
                }

                _doingAction = LoadPortCommands.Idle;
            }
            else if (receivedMessage.StartsWith(NakMessage))
            {
                _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Error));
                _doingAction = LoadPortCommands.Idle;
            }
            else if (receivedMessage.StartsWith(AckMessage))
            {
                if (_doingActionData[1].StartsWith(StatusMessage))
                {
                    UpdateLogicalStates(_doingActionData[2]);

                    if (_doingAction.Equals(LoadPortCommands.GetState) ||
                        _doingAction.Equals(LoadPortCommands.FindLoadingMode))
                    {
                        _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Completed));
                        _doingAction = LoadPortCommands.Idle;
                    }
                }

                else if (receivedMessage.Contains(MapStatusMessage))
                {
                    // Map
                    UpdateSlotMap(_doingActionData[2]);

                    if (_doingAction.Equals(LoadPortCommands.GetMap) ||
                        _doingAction.Equals(LoadPortCommands.Load) ||
                        _doingAction.Equals(LoadPortCommands.Unload))
                    {
                        _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Completed));
                        _doingAction = LoadPortCommands.Idle;
                    }
                }

                else if (receivedMessage.Contains(VerMessage))
                {
                    _doingAction = LoadPortCommands.Idle;
                }
                else if (receivedMessage.Contains(ParameterMessage))
                {
                    int nIndexCount;

                    if (_doingActionData[2].StartsWith("SLN"))
                    {
                        _capacity = 0;
                        if (false == string.IsNullOrEmpty(_doingActionData[2]))
                        {
                            nIndexCount = _doingActionData[2].Length;
                            _capacity = int.Parse(_doingActionData[2].Substring(nIndexCount - 2));
                        }
                    }

                    _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Completed));
                    _doingAction = LoadPortCommands.Idle;
                }
                else if (receivedMessage.Contains(LightOn) || receivedMessage.Contains(LightBlink) || receivedMessage.Contains(LightOff))
                {
                    //if ((int)_LEDType == int.Parse(_doingActionData[1].Substring(3,2)))
                    //{
                    //    _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Completed));
                    //    _doingAction = LoadPortCommands.Idle;
                    //}
                    _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Completed));
                    _doingAction = LoadPortCommands.Idle;
                }
            }
            else
            {
                if (receivedMessage.StartsWith(AbnormalCompleteMessage) || receivedMessage.StartsWith(RetransmissionABS))
                {
                    // Error
                    if (false == _doingAction.Equals(LoadPortCommands.Idle))
                    {
                        _commandResults.TryAdd(_doingAction, new CommandResults(_doingAction.ToString(), CommandResult.Error, receivedMessage));
                        _doingAction = LoadPortCommands.Idle;
                    }
                    else
                    {
                        _triggeredAlarm = receivedMessage;
                    }
                }
            }
        }
        #endregion </Thread>

        #region <Internals>
        private CommandResults ExecuteFindingLoadingMode(LoadPortCommands command)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        SetTimeOverByCommand(command);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(LoadPortCommands.Reset))
                        {
                            ++_actionStep;
                        }
                    }
                    break;

                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Reset))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Reset);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                if (false == command.Equals(LoadPortCommands.Reset))
                                {
                                    _result.CommandResult = CommandResult.Proceed;
                                    _result.Description = string.Empty;
                                    _actionStep = 10;
                                }
                            }
                        }
                    }
                    break;

                case 10:
                    {
                        // Command
                        if (SendMessage(command))
                        {
                            ++_actionStep;
                        }
                    }
                    break;

                case 11:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(command))
                                break;

                            _result = GetCommandResult(command);
                        }
                    }
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        private CommandResults ExecuteChangingMode(LoadPortLoadingMode mode)
        {
            LoadPortCommands command = mode.Equals(LoadPortLoadingMode.Cassette) ?
                LoadPortCommands.ChangeToCassette : LoadPortCommands.ChangeToFoup;

            if (false == LoadPortModeChanger.ContainsKey(mode))
                return new CommandResults(command.ToString(), CommandResult.Error);


            var result = ExecuteCommand(command, HandleSelectedLED);
            if (result.CommandResult.Equals(CommandResult.Completed))
            {
                _currentCarrierMode = mode;
                ChangeLoadingTypeState(mode);
                WriteLoadingModeFile(mode);
            }

            return result;
        }
        // 2024.11.13. by dwlim [MOD] ACCESS MODE 저장
        private CommandResults ExecuteChangingAccessMode(LoadPortAccessMode mode)
        {
            LoadPortCommands command = mode.Equals(LoadPortAccessMode.Auto) ?
                LoadPortCommands.ChangeAccessModeToAuto : LoadPortCommands.ChangeAccessModeToManual;

            var result = ExecuteOnlyLEDCommand(command, HandleSelectedLED);
            if (_currentAccessMode != mode && result.CommandResult == CommandResult.Completed)
            {
                WriteAccessModeFile(mode);
            }

            return result;
        }

        private CommandResults ExecuteOnlyLEDCommand(LoadPortCommands command, _callBackHandleLEDCommand callback)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        SetTimeOverByCommand(command);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(LoadPortCommands.Reset))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Reset))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Reset);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                if (false == command.Equals(LoadPortCommands.Reset))
                                {
                                    _result.CommandResult = CommandResult.Proceed;
                                    _result.Description = string.Empty;
                                    _actionStep = 10;
                                    _callbackActionStep = 0;
                                }
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        if (null == callback)
                        {
                            _actionStep = 20;
                            break;
                        }
                        SelectConflictingLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedOff);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                            _actionStep = 20;
                            _callbackActionStep = 0;
                        }
                    }
                    break;
                case 20:
                    {
                        if (null == callback)
                        {
                            _actionStep = 30;
                            break;
                        }
                        SelectLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedOn);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                            _actionStep = 30;
                            _callbackActionStep = 0;
                        }
                    }
                    break;
                case 30:
                    {
                        // GetState
                        if (SendMessage(LoadPortCommands.GetState))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 31:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == _commandResults.ContainsKey(LoadPortCommands.GetState))
                        break;

                    _result = GetCommandResult(LoadPortCommands.GetState);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
                _callbackActionStep = 0;
            }

            return _result;
        }

        private CommandResults ExecuteCommandWithMap(LoadPortCommands command, _callBackHandleLEDCommand callback)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        SetTimeOverByCommand(command);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(LoadPortCommands.Reset))
                        {
                            ++_actionStep;
                        }
                    }
                    break;

                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Reset))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Reset);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 10;
                            }
                        }
                    }
                    break;

                case 10:
                    {
                        // Get Capacity
                        if (SendMessage(LoadPortCommands.GetCapacity))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 11:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.GetCapacity))
                                break;

                            _result = GetCommandResult(LoadPortCommands.GetCapacity);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 20;
                                _callbackActionStep = 0;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (null == callback)
                        {
                            _actionStep = 30;
                            break;
                        }
                        SelectLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedBlink);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                            _actionStep = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        // Command
                        if (SendMessage(command))
                        {
                            ++_actionStep;
                        }
                    }
                    break;

                case 31:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(command))
                                break;

                            _result = GetCommandResult(command);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 40;
                                _callbackActionStep = 0;
                            }
                        }
                    }
                    break;
                case 40:
                    {
                        if (null == callback)
                        {
                            _actionStep = 50;
                            break;
                        }
                        SelectConflictingLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedOff);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _actionStep = 50;
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                        }
                        break;
                    }
                case 50:
                    {
                        // GetState
                        if (SendMessage(LoadPortCommands.GetState))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 51:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == _commandResults.ContainsKey(LoadPortCommands.GetState))
                        break;

                    _result = GetCommandResult(LoadPortCommands.GetState);
                    if (_result.CommandResult.Equals(CommandResult.Completed))
                    {
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        _actionStep = 60;
                    }
                    break;

                case 60:
                    if (SendMessage(LoadPortCommands.GetMap))
                    {
                        ++_actionStep;
                    }
                    break;

                case 61:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.GetMap))
                                break;

                            _result = GetCommandResult(LoadPortCommands.GetMap);
                        }
                    }
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
                _callbackActionStep = 0;
            }

            return _result;
        }

        private CommandResults ExecuteInitialization(_callBackHandleLEDCommand callback)
        {
            
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        SetTimeOverByCommand(LoadPortCommands.Initialize);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(LoadPortCommands.Reset))
                        {
                            ++_actionStep;
                            if (PortId == 4)
                            {
                                Console.WriteLine(_actionStep);
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Reset))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Reset);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 10;
                                if (PortId == 4)
                                {
                                    Console.WriteLine(_actionStep);
                                }
                            }
                        }
                    }
                    break;
                case 10:
                    // Command
                    if (SendMessage(LoadPortCommands.Initialize))
                    {
                        ++_actionStep;
                        if (PortId == 4)
                        {
                            Console.WriteLine(_actionStep);
                        }
                    }
                    break;
                case 11:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Initialize))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Initialize);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                ChangeInitializationState(true);

                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 20;
                                _callbackActionStep = 0;
                            }
                        }
                    }
                    break;

                case 20:
                    {
                        if (null == callback)
                        {
                            _actionStep = 30;
                            break;
                        }
                        
                        LoadPortCommands command = LoadPortCommands.Idle;
                        switch (AccessMode)
                        {
                            case LoadPortAccessMode.Auto:
                                command = LoadPortCommands.ChangeAccessModeToAuto;
                                break;
                            case LoadPortAccessMode.Manual:
                                command = LoadPortCommands.ChangeAccessModeToManual;
                                break;
                        }

                        SelectConflictingLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedOff);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                            _actionStep = 30;
                            _callbackActionStep = 0;
                            if (PortId == 4)
                            {
                                Console.WriteLine(_actionStep);
                            }
                        }
                        break;
                    }
                case 30:
                    {
                        if (null == callback)
                        {
                            _actionStep = 40;
                            break;
                        }

                        LoadPortCommands command = LoadPortCommands.Idle;
                        switch (AccessMode)
                        {
                            case LoadPortAccessMode.Auto:
                                command = LoadPortCommands.ChangeAccessModeToAuto;
                                break;
                            case LoadPortAccessMode.Manual:
                                command = LoadPortCommands.ChangeAccessModeToManual;
                                break;
                        }

                        SelectLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedOn);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _actionStep = 40;
                            _callbackActionStep = 0;
                            if (PortId == 4)
                            {
                                Console.WriteLine(_actionStep);
                            }
                        }
                        break;
                    }
                case 40:
                    {
                        // GetState
                        if (SendMessage(LoadPortCommands.GetState))
                        {
                            ++_actionStep;
                            if (PortId == 4)
                            {
                                Console.WriteLine(_actionStep);
                            }
                        }
                    }
                    break;
                case 41:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }

                        if (false == _commandResults.ContainsKey(LoadPortCommands.GetState))
                            break;

                        return GetCommandResult(LoadPortCommands.GetState);
                    }

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
                _callbackActionStep = 0;
            }

            return _result;
        }

        private CommandResults ExecuteCommand(LoadPortCommands command, _callBackHandleLEDCommand callback)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        SetTimeOverByCommand(command);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(LoadPortCommands.Reset))
                        {
                            ++_actionStep;
                        }
                    }
                    break;

                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Reset))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Reset);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                if (false == command.Equals(LoadPortCommands.Reset))
                                {
                                    _result.CommandResult = CommandResult.Proceed;
                                    _result.Description = string.Empty;
                                    _actionStep = 10;
                                    _callbackActionStep = 0;
                                }
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        if (null == callback)
                        {
                            _actionStep = 20;
                            break;
                        }
                        SelectLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedBlink);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                            _actionStep = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        // Command
                        if (SendMessage(command))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 21:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(command))
                                break;

                            _result = GetCommandResult(command);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 30;
                                _callbackActionStep = 0;
                            }
                        }
                    }
                    break;
                case 30:
                    {
                        if (null == callback)
                        {
                            _actionStep = 40;
                            break;
                        }
                        SelectLED(command);
                        SelectLEDLightingCommand(LoadPortCommands.LedOn);
                        _result = callback(command);
                        if (_result.CommandResult.Equals(CommandResult.Completed))
                        {
                            _result.CommandResult = CommandResult.Proceed;
                            _result.Description = string.Empty;
                            _actionStep = 40;
                        }
                    }
                    break;
                case 40:
                    {
                        // GetState
                        if (SendMessage(LoadPortCommands.GetState))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 41:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == _commandResults.ContainsKey(LoadPortCommands.GetState))
                        break;

                    _result = GetCommandResult(LoadPortCommands.GetState);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
                _callbackActionStep = 0;
            }

            return _result;
        }

        private CommandResults ExecuteResetOrStateCommand()
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        SetTimeOverByCommand(LoadPortCommands.Reset);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(LoadPortCommands.Reset))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(LoadPortCommands.Reset))
                                break;

                            _result = GetCommandResult(LoadPortCommands.Reset);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 10;
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        // GetState
                        if (SendMessage(LoadPortCommands.GetState))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 11:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == _commandResults.ContainsKey(LoadPortCommands.GetState))
                        break;

                    _result = GetCommandResult(LoadPortCommands.GetState);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                _doingAction = LoadPortCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }

        private void SetTimeOverByCommand(LoadPortCommands command)
        {
            uint time;
            switch (command)
            {
                case LoadPortCommands.Load:
                case LoadPortCommands.Unload:
                case LoadPortCommands.DoorOpen:
                case LoadPortCommands.DoorClose:
                case LoadPortCommands.Initialize:
                case LoadPortCommands.Scan:
                    time = TimeLong;
                    break;

                case LoadPortCommands.Clamp:
                case LoadPortCommands.Unclamp:
                case LoadPortCommands.Reset:
                case LoadPortCommands.AmpOn:
                case LoadPortCommands.AmpOff:
                case LoadPortCommands.GetState:
                case LoadPortCommands.GetMap:
                case LoadPortCommands.GetCapacity:
                case LoadPortCommands.LedOn:
                case LoadPortCommands.LedOff:
                case LoadPortCommands.LedBlink:
                    time = TimeShort;
                    break;

                case LoadPortCommands.Dock:
                case LoadPortCommands.Undock:
                    time = TimeMiddle;
                    break;

                case LoadPortCommands.ChangeToCassette:
                case LoadPortCommands.ChangeToFoup:
                    time = TimeMiddle;
                    break;

                default:
                    time = TimeShort;
                    break;
            }

            _timeChecker.SetTickCount(time);
        }

        private bool SendMessage(LoadPortCommands command)
        {
            // SET : [장비 설정] 오류 재설정, 초기화 등의 설정 처리를 요청하는 명령
            // GET : [상태 획득 요청] 상태 또는 매핑 결과 보고를 요청하는 명령
            // MOV : [동작 요청] 동작 실행 및 동작 제어를 요청하는 명령
            // MOD : [모드 설정] SELOP 통신 모드 전환을 요청하는 명령
            // EVT : [이벤트 설정] 이벤트 활성화/비활성화를 요청하는 명령

            if (_commandResults.ContainsKey(command))
                _commandResults.TryRemove(command, out _);

            string commandMessage = string.Empty;
            string messageToSend = string.Empty;
            switch (command)
            {
                case LoadPortCommands.Load:
                    if (_currentCarrierMode == LoadPortLoadingMode.Cassette)
                    {
                        commandMessage = "MOV:CLDMP";
                    }
                    else
                    {
                        commandMessage = "MOV:CLDMP";
                    }
                    break;
                case LoadPortCommands.Unload:
                    if (_currentCarrierMode == LoadPortLoadingMode.Cassette)
                    {
                        commandMessage = "MOV:CULOD";
                    }
                    else
                    {
                        commandMessage = "MOV:CULOD";    // 확인 필요
                    }
                    break;
                case LoadPortCommands.Clamp:
                    commandMessage = "MOV:PODCL";
                    break;
                case LoadPortCommands.Unclamp:
                    commandMessage = "MOV:PODOP";
                    break;
                case LoadPortCommands.Dock:
                    commandMessage = "MOV:YDDOR";
                    break;
                case LoadPortCommands.Undock:
                    commandMessage = "MOV:YWAIT";
                    break;
                case LoadPortCommands.DoorOpen:
                    commandMessage = "MOV:ZMPDW";
                    break;
                case LoadPortCommands.DoorClose:
                    commandMessage = "MOV:ZMPUP";
                    break;
                case LoadPortCommands.Hello:
                    //messageToSend = "LOAD";
                    commandMessage = "";
                    break;
                case LoadPortCommands.Initialize:
                    commandMessage = "MOV:ABORG";
                    break;
                case LoadPortCommands.Scan:
                    //messageToSend = "SCAN UP";
                    commandMessage = "";
                    break;
                case LoadPortCommands.ScanDown:
                    commandMessage = "MOV:MAPD1";
                    break;
                case LoadPortCommands.GetMap:
                    //messageToSend = "GET:MAPDT";
                    commandMessage = "GET:MAPRD";
                    break;
                case LoadPortCommands.GetCapacity:
                    commandMessage = "GET:PARAM/SLN";
                    break;
                case LoadPortCommands.FindLoadingMode:
                    commandMessage = "GET:STATE";
                    break;
                //case LoadPortCommands.SetCapacity:      // +4 ~ +30
                //    messageToSend = string.Format("SET:PARAM/SLN=+0000{0}", _setCapacity);
                //    break;
                case LoadPortCommands.GetState:
                    commandMessage = "GET:STATE";
                    break;
                case LoadPortCommands.Reset:
                    commandMessage = "SET:RESET";
                    break;
                case LoadPortCommands.ChangeToCassette:
                    commandMessage = "MOV:PINUP";
                    break;
                case LoadPortCommands.ChangeToFoup:
                    commandMessage = "MOV:PINDW";
                    break;
                //case LoadPortCommands.GetAcceessingMode:
                //    //messageToSend = "AUTO_MODE";
                //    messageToSend = "";
                //    break;
                case LoadPortCommands.ChangeAccessModeToAuto:
                    commandMessage = "";
                    break;
                case LoadPortCommands.ChangeAccessModeToManual:
                    commandMessage = "";
                    break;
                // 2024.11.14. by dwlim [ADD] SELOP8 LED I/F 추가
                case LoadPortCommands.LedOn:
                    commandMessage = string.Format("SET:{0}{1}", LightOn, ((int)_LEDType).ToString("D2"));
                    break;
                case LoadPortCommands.LedOff:
                    commandMessage = string.Format("SET:{0}{1}", LightOff, ((int)_LEDType).ToString("D2"));
                    break;
                case LoadPortCommands.LedBlink:
                    commandMessage = string.Format("SET:{0}{1}", LightBlink, ((int)_LEDType).ToString("D2"));
                    break;
                // 2024.11.14. by dwlim [END]

                // 2024.11.18. by dwlim [ADD] SELOP8 LED Status 추가
                case LoadPortCommands.LedStatus:
                    commandMessage = "GET:LEDST";
                    break;
                default:
                    commandMessage = string.Empty;
                    break;
            }
            messageToSend = string.Format("{0}{1}{2}{3}", SendStartToken, LoadPortADR, commandMessage, EndToken);

            return DoAction(command, messageToSend);
        }

        private CommandResults GetCommandResult(LoadPortCommands command)
        {
            if (false == _commandResults.TryRemove(command, out CommandResults commandResult))
                return new CommandResults(command.ToString(), CommandResult.Proceed);

            return commandResult;
        }

        private bool IsTimeOver()
        {
            if (_timeChecker.IsTickOver(true))
            {
                _actionStep = 0;

                return true;
            }

            return false;
        }

        private void UpdateLogicalStates(string receivedMessage)
        {
            if (receivedMessage.Length != StatusMessageLength)
                return;

            // Place & Present
            ChangePlacedState(receivedMessage[6] == '1' ? true : false);
            ChangePresentState(receivedMessage[6] == '1' ? true : false);   // 2024.12.19. by dwlim [ADD] 처음에 Present정보가 없어서 추가

            // Clamp
            ChangeClampState(receivedMessage[7] == '1' ? true : false);

            // Dock
            ChangeDockState(receivedMessage[13] == '1' ? true : false);

            // Door Open
            ChangeDoorState(receivedMessage[15] == '1' ? true : false);

            // Loading Mode
            switch (receivedMessage[16])
            {
                case '0':
                    ChangeLoadingTypeState(LoadPortLoadingMode.Foup);
                    break;
                case '1':
                    ChangeLoadingTypeState(LoadPortLoadingMode.Cassette);
                    break;
                default:
                    ChangeLoadingTypeState(LoadPortLoadingMode.Unknown);
                    break;
            }

            // Mode

            // MappedStatus
        }

        private void UpdateMechanicalStates(string receivedMessage)
        {
            int states = int.Parse(receivedMessage, System.Globalization.NumberStyles.HexNumber);

            // Present
            _temporaryPresent = (states & FoupDetectSensorMask1) != 0 ? true : false;
            ChangePresentState(_temporaryPresent);

            // Placement
            _temporaryPlaced = ((states & FoupStabilizedSensorMask1) != 0) && ((states & FoupStabilizedSensorMask2) != 0)
                                            && ((states & FoupStabilizedSensorMask3) != 0) ? true : false;
            ChangePlacedState(_temporaryPlaced);

            // FoupClampStatus
            _temporaryClamped = (states & ClampPositionSensorMask) != 0 ? true : false;
            ChangeClampState(_temporaryClamped);
        }

        private void UpdateSlotMap(string receivedMessage)
        {
            Dictionary<int, CarrierSlotMapStates> slotState = new Dictionary<int, CarrierSlotMapStates>();

            if (_capacity == 0)
            {
                for (int i = SlotMaxCount - 1; i >= SlotMinCount - 1; i--)
                {
                    if (receivedMessage[i] != '0')
                    {
                        _capacity = i + 1;
                        break;
                    }
                }
            }

            for (int i = 1; i <= _capacity; ++i)
            {
                var status = receivedMessage[i - 1];
                switch (status)
                {
                    case SlotStatusEmpty:
                        slotState[i] = CarrierSlotMapStates.Empty;
                        break;
                    case SlotStatusCorrectlyOccupied:
                        slotState[i] = CarrierSlotMapStates.CorrectlyOccupied;
                        break;
                    case SlotStatusCross:
                        slotState[i] = CarrierSlotMapStates.CrossSlotted;
                        break;
                    case SlotStatusUpDownTilted:
                        slotState[i] = CarrierSlotMapStates.NotEmpty;
                        break;
                    case SlotStatusThicknessErrorThick:
                        slotState[i] = CarrierSlotMapStates.DoubleSlotted;
                        break;
                    case SlotStatusThicknessErrorThin:
                        slotState[i] = CarrierSlotMapStates.NotEmpty;
                        break;

                    default:
                        slotState[i] = CarrierSlotMapStates.Undefined;
                        break;
                }

                //else if (status =='4')
                //{
                //    slotstate[i] = Defines.LoadPort.CarrierSlotMapStates.UpDownTilte;
                //}
                //else if (status =='W')
                //{
                //    slotstate[i] = Defines.LoadPort.CarrierSlotMapStates.WaferThicknessError_Thick;
                //}
                //else if (status =='T')
                //{
                //    slotstate[i] = Defines.LoadPort.CarrierSlotMapStates.WaferThicknessError_Thin;
                //}
                // 2024.10.04. jhlim [END]
            }

            ChangeSlotMap(slotState);
        }
        private bool ParseDatas(string receivedMessage)
        {
            //int index = receivedMessage.IndexOf(":");
            //if (index < 0)
            //    return false;
            _doingActionData = null;

            if (string.IsNullOrEmpty(receivedMessage))
            {
                return false;
            }

            if (receivedMessage.Contains(":"))
            {
                if (receivedMessage.Contains("/"))
                {
                    _doingActionData = receivedMessage.Split(':', '/');
                    return true;
                }
                _doingActionData = receivedMessage.Split(':');
                return true;
            }

            return false;
        }
        // 2024.12.18. by dwlim [END]
        // 2024.11.13. by dwlim [ADD] ACCESS MODE 저장
        private void ReadAccessModeFile()
        {
            string path = string.Format(@"{0}\AccessMode", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (false == Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullName = string.Format(@"{0}\LoadPort{1}.ini", path, _PortId);
            if (false == File.Exists(fullName))
            {
                WriteAccessModeFile(_currentAccessMode);
                return;
            }

            IniControl ini = new IniControl(fullName);

            _accessMode = ini.GetInt(SECTION_NAME, KEY_ACCESS_MODE, -1);
            if (_accessMode > -1)
            {
                _currentAccessMode = _accessMode == 0 ? LoadPortAccessMode.Auto : LoadPortAccessMode.Manual;
                ChangeAccessingState(_currentAccessMode);
            }
        }
        private void WriteAccessModeFile(LoadPortAccessMode mode)
        {
            string path = string.Format(@"{0}\AccessMode", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (false == Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullName = string.Format(@"{0}\LoadPort{1}.ini", path, _PortId);
            IniControl ini = new IniControl(fullName);

            if (_currentAccessMode != mode)
            {
                _currentAccessMode = mode;
                ChangeAccessingState(_currentAccessMode);
            }

            _accessMode = mode == LoadPortAccessMode.Auto ? 0 : 1;
            ini.WriteInt(SECTION_NAME, KEY_ACCESS_MODE, _accessMode);
        }
        // 2024.11.13. by dwlim [END]
        // 2025.03.25. by dwlim [ADD] LOADING MODE 저장
        private void ReadLoadingModeFile()
        {
            string path = string.Format(@"{0}\LoadingMode", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (false == Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullName = string.Format(@"{0}\LoadPort{1}.ini", path, _PortId);
            if (false == File.Exists(fullName))
            {
                WriteLoadingModeFile(_currentCarrierMode);
                return;
            }

            IniControl ini = new IniControl(fullName);

            _loadingMode = ini.GetInt(SECTION_NAME, KEY_LOADING_MODE, -1);
            if (_loadingMode > -1)
            {
                _currentCarrierMode = _loadingMode == 0 ? LoadPortLoadingMode.Foup : LoadPortLoadingMode.Cassette;
                ChangeLoadingTypeState(_currentCarrierMode);
            }
        }
        private void WriteLoadingModeFile(LoadPortLoadingMode mode)
        {
            string path = string.Format(@"{0}\LoadingMode", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (false == Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullName = string.Format(@"{0}\LoadPort{1}.ini", path, _PortId);
            IniControl ini = new IniControl(fullName);

            if (_currentCarrierMode != mode)
            {
                _currentCarrierMode = mode;
                ChangeLoadingTypeState(_currentCarrierMode);
            }

            _loadingMode = mode == LoadPortLoadingMode.Foup ? 0 : 1;
            ini.WriteInt(SECTION_NAME, KEY_LOADING_MODE, _loadingMode);
        }
        // 2025.03.25. by dwlim [END]
        private void SelectLED(LoadPortCommands command)
        {
            switch (command)
            {
                // LOAD, UNLOAD, ALARM 은 Parameter 변경으로 HOST에서 컨트롤하도록 변경 가능.
                case LoadPortCommands.Load:
                    _LEDType = LoadPortLEDTypes.Load_Middle;    // Load_Top은 HOST에서 컨트롤
                    break;
                case LoadPortCommands.Unload:
                    _LEDType = LoadPortLEDTypes.Unload_Middle;  // Unload_Top는 HOST에서 컨트롤
                    break;
                case LoadPortCommands.ChangeToCassette:
                    _LEDType = LoadPortLEDTypes.Metal;
                    break;
                case LoadPortCommands.ChangeToFoup:
                    _LEDType = LoadPortLEDTypes.MAC;
                    break;
                case LoadPortCommands.ChangeAccessModeToAuto:
                    _LEDType = LoadPortLEDTypes.Auto;
                    break;
                case LoadPortCommands.ChangeAccessModeToManual:
                    _LEDType = LoadPortLEDTypes.Manual;
                    break;
                default:
                    break;
            }
        }
        private void SelectConflictingLED(LoadPortCommands command)
        {
            switch (command)
            {
                // LOAD, UNLOAD, ALARM 은 Parameter 변경으로 HOST에서 컨트롤하도록 변경 가능.
                case LoadPortCommands.Load:
                    _LEDType = LoadPortLEDTypes.Unload_Middle;    // Load_Top은 HOST에서 컨트롤
                    break;
                case LoadPortCommands.Unload:
                    _LEDType = LoadPortLEDTypes.Load_Middle;  // Unload_Top는 HOST에서 컨트롤
                    break;
                case LoadPortCommands.ChangeToCassette:
                    _LEDType = LoadPortLEDTypes.MAC;
                    break;
                case LoadPortCommands.ChangeToFoup:
                    _LEDType = LoadPortLEDTypes.Metal;
                    break;
                case LoadPortCommands.ChangeAccessModeToAuto:
                    _LEDType = LoadPortLEDTypes.Manual;
                    break;
                case LoadPortCommands.ChangeAccessModeToManual:
                    _LEDType = LoadPortLEDTypes.Auto;
                    break;
                default:
                    break;
            }
        }
        private void SelectLEDLightingCommand(LoadPortCommands ledCommand)
        {
            switch (ledCommand)
            {
                case LoadPortCommands.LedOn:
                case LoadPortCommands.LedBlink:
                case LoadPortCommands.LedOff:
                    _LEDLightingCommand = ledCommand;
                    return;
                default:
                    return;
            }
        }
        private CommandResults HandleSelectedLED(LoadPortCommands command)
        {
            switch (_callbackActionStep)
            {
                case 0:
                    {
                        //SelectLED(command);
                        if (SendMessage(_LEDLightingCommand))
                        {
                            ++_callbackActionStep;
                        }
                    }
                    break;
                case 1:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        else
                        {
                            if (false == _commandResults.ContainsKey(_LEDLightingCommand))
                                break;

                            _result = GetCommandResult(_LEDLightingCommand);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                                break;
                        }
                    }
                    break;
            }
            return _result;
        }
        #endregion </Internals>

        #endregion </Methods>
    }
}

