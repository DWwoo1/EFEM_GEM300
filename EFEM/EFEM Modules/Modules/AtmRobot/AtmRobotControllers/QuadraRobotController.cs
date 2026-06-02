using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;

namespace EFEM.Modules.AtmRobot.AtmRobotControllers
{
    public class QuadraRobotController : AtmRobotController
    {
        #region <Constructors>
        public QuadraRobotController(int robotIndex,
            Dictionary<string, string> stationNames,
            EN_CONNECTION_TYPE interfaceType,
            int commIndex) : base(robotIndex, stationNames, interfaceType, commIndex) { }
        #endregion </Constructors>

        #region <Fields>
        private const uint TimeLong = 30000;
        private const uint TimeMiddle = 10000;
        private const uint TimeShort = 5000;

        private const string CarriageReturnToken = "\r";
        //private string _receivedMessage = string.Empty;
        private const string CompleteMessage = "_RDY";
        private const string NackMessage = "_NAK";
        private const string ErrorMessage = "_ERR ";
        private const string CompleteHispdMessage = "HISPD ALL";
        private const string CompleteLospdMessage = "LOSPD ALL";
        private const string CompleteWaferPresence = "WAFER";
        private readonly ConcurrentDictionary<RobotCommands, CommandResults> _commandResults
            = new ConcurrentDictionary<RobotCommands, CommandResults>();

        private double _completeHispdValue = 0.0;
        private double _completeLospdValue = 0.0;

        TickCounter_.TickCounter tickCounter = new TickCounter_.TickCounter();
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

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
        protected override CommandResults DoInitialize()
        {
            RobotCommands currentAction = RobotCommands.Initialize;
            switch (_actionStep)
            {
                case 0:
                    {
                        // Reset
                        _timeChecker.SetTickCount(5000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(RobotCommands.ClearError, RobotArmTypes.All, "", 0))
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
                            if (false == HasCommandResultData(RobotCommands.ClearError))
                                break;

                            _result = GetCommandResult(RobotCommands.ClearError);
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
                        _timeChecker.SetTickCount(45000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 11:
                    {
                        if (SendMessage(currentAction, RobotArmTypes.All, "", 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 12:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    if (_result.CommandResult.Equals(CommandResult.Completed))
                    {
                        Initialized = true;
                    }
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }

        protected override CommandResults DoApproachForPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.ApproachForPick;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }

        protected override CommandResults DoMoveToPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.MoveToPick;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }

        protected override CommandResults DoApproachForPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.ApproachForPlace;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoMoveToPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.MoveToPlace;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }

        protected override CommandResults DoPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.Pick;
            switch (_actionStep)
            {
                case 0:
                    {
                        //// Reset
                        //_timeChecker.SetTickCount(5000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        //if (SendMessage(RobotCommands.ClearError, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 1:
                    {
                        //if (IsTimeOver())
                        //{
                        //    _result.CommandResult = CommandResult.Timeout;
                        //    break;
                        //}
                        //else
                        {
                            //if (false == HasCommandResultData(RobotCommands.ClearError))
                            //    break;
                            
                            //_result  = GetCommandResult(RobotCommands.ClearError);
                            //if (_result.CommandResult.Equals(CommandResult.Completed))
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
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 11:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 12:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoPickStepOne(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.PickStepOne;
            switch (_actionStep)
            {
                case 0:
                    {
                        //// Reset
                        //_timeChecker.SetTickCount(5000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        //if (SendMessage(RobotCommands.ClearError, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 1:
                    {
                        //if (IsTimeOver())
                        //{
                        //    _result.CommandResult = CommandResult.Timeout;
                        //    break;
                        //}
                        //else
                        {
                            //if (false == HasCommandResultData(RobotCommands.ClearError))
                            //    break;

                            //_result  = GetCommandResult(RobotCommands.ClearError);
                            //if (_result.CommandResult.Equals(CommandResult.Completed))
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
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 11:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 12:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoPickStepTwo(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.PickStepTwo;
            switch (_actionStep)
            {
                case 0:
                    {
                        //// Reset
                        //_timeChecker.SetTickCount(5000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        //if (SendMessage(RobotCommands.ClearError, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 1:
                    {
                        //if (IsTimeOver())
                        //{
                        //    _result.CommandResult = CommandResult.Timeout;
                        //    break;
                        //}
                        //else
                        {
                            //if (false == HasCommandResultData(RobotCommands.ClearError))
                            //    break;

                            //_result  = GetCommandResult(RobotCommands.ClearError);
                            //if (_result.CommandResult.Equals(CommandResult.Completed))
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
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 11:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 12:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoGetWaferPresence()
        {
            RobotCommands currentAction = RobotCommands.GetWaferPresence;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, RobotArmTypes.All, null, 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.Place;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        // 2024.09.25. by dwlim. [ADD] Place 구분동작 추가
        protected override CommandResults DoPlaceStepOne(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.PlaceStepOne;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoPlaceStepTwo(RobotArmTypes armType, string targetLocation, int slot)
        {
            RobotCommands currentAction = RobotCommands.PlaceStepTwo;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }


                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoSetLowSpeed(RobotArmTypes armType, double speed)
        {
            RobotCommands currentAction = RobotCommands.SetLowSpeed;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendSpeedSettingMessage(currentAction, armType, speed))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoGetLowSpeed(RobotArmTypes armType, ref double speed)
        {
            RobotCommands currentAction = RobotCommands.GetLowSpeed;
            switch (_actionStep)
            {
                case 0:
                    {
                        // 참조로 받은 변수 값 초기화 -> 명령에 실패하는 경우 대비
                        speed = 0;
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, null, 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                // 응답을 저장한 값을 참조로 받은 변수에 대입
                speed = _completeLospdValue;

                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoSetHighSpeed(RobotArmTypes armType, double speed)
        {
            RobotCommands currentAction = RobotCommands.SetHighSpeed;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendSpeedSettingMessage(currentAction, armType, speed))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoGetHighSpeed(RobotArmTypes armType, ref double speed)
        {
            RobotCommands currentAction = RobotCommands.GetHighSpeed;
            switch (_actionStep)
            {
                case 0:
                    {
                        // 참조로 받은 변수 값 초기화 -> 명령에 실패하는 경우 대비
                        speed = 0;
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, armType, null, 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                // 응답을 저장한 값을 참조로 받은 변수에 대입
                speed = _completeHispdValue;

                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoClear()
        {
            RobotCommands currentAction = RobotCommands.ClearError;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, RobotArmTypes.All, null, 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoGrip(RobotArmTypes armType)
        {
            RobotCommands currentAction = RobotCommands.Grip;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendGripSettingMessage(currentAction, armType))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoUngrip(RobotArmTypes armType)
        {
            RobotCommands currentAction = RobotCommands.Ungrip;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendGripSettingMessage(currentAction, armType))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoServoOn()
        {
            RobotCommands currentAction = RobotCommands.AmpOn;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, RobotArmTypes.All, null, 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        protected override CommandResults DoServoOff()
        {
            RobotCommands currentAction = RobotCommands.AmpOff;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(30000);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        ++_actionStep;
                    }
                    break;
                case 1:
                    {
                        if (SendMessage(currentAction, RobotArmTypes.All, null, 0))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 2:
                    if (IsTimeOver())
                    {
                        _result.CommandResult = CommandResult.Timeout;
                        break;
                    }

                    if (false == HasCommandResultData(currentAction))
                        break;

                    _result = GetCommandResult(currentAction);
                    break;

                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        #endregion </Actions>

        #region <Thread>
        protected override bool RemoveTokens(string receivedMessage, ref string newString)
        {
            int index = receivedMessage.IndexOf(CarriageReturnToken);
            if (index < 0)
                return false;

            newString = receivedMessage.Remove(index);

            return true;
        }

        // 받은 메시지를 파싱한다.
        protected override void ParseMessages(string receivedMessage)
        {
            // 1. 토큰 제거
            //if (false == RemoveTokens(receivedMessage, ref _receivedMessage))
            //    return;

            //if (receivedMessage.Equals(CompleteMessage))
            //{
            //    _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Completed));
            //    DoingAction = RobotCommands.Idle;
            //}

            // 2024.09.25. by dwlim. [MOD] Place 구분동작 추가로 인한 수정
            if (receivedMessage.Equals(CompleteMessage))
            {
                _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Completed));
                DoingAction = RobotCommands.Idle;
            }
            if (receivedMessage.Equals(NackMessage))
            {
                _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Error));
                DoingAction = RobotCommands.Idle;
            }
            if (receivedMessage.StartsWith(ErrorMessage))
            {
                string errorCode = receivedMessage.Replace(ErrorMessage, "");
                _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Error, errorCode));
              
                if (errorCode == "00009")
                {
                    Initialized = false;
                }
                DoingAction = RobotCommands.Idle;
            }
            if (receivedMessage.StartsWith(CompleteWaferPresence))
            {
                _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Completed));
                DoingAction = RobotCommands.Idle;

                string[] parts = receivedMessage.Split(' '); // WAFER A Y B N (5ea)
                WaferPresenceUpperArm = parts[4] == "Y";
                WaferPresenceLowerArm = parts[2] == "Y";
            }
            if (receivedMessage.StartsWith(CompleteHispdMessage))
            {
                string[] parts = receivedMessage.Split(' '); // HISPD ALL 000 (3ea)
                if (false == double.TryParse(parts[2], out _completeHispdValue))
                {
                    _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Error));
                }
                else
                {
                    // 결과를 변수에 저장 후 완료 처리
                    _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Completed));
                }
                DoingAction = RobotCommands.Idle;
            }
            if (receivedMessage.StartsWith(CompleteLospdMessage))
            {
                string[] parts = receivedMessage.Split(' '); // LOSPD ALL 000 (3ea)
                if (false == double.TryParse(parts[2], out _completeLospdValue))
                {
                    _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Error));
                }
                else
                {
                    // 결과를 변수에 저장 후 완료 처리
                    _commandResults.TryAdd(DoingAction, new CommandResults(DoingAction.ToString(), CommandResult.Completed));
                }
                DoingAction = RobotCommands.Idle;
            }
        }
        #endregion </Thread>

        #region <Internals>
        private bool SendSpeedSettingMessage(RobotCommands command, RobotArmTypes arm, double speed)
        {
            if (_commandResults.ContainsKey(command))
                _commandResults.TryRemove(command, out _);
            string messageToSend;
            switch (command)
            {
                case RobotCommands.SetLowSpeed:
                    messageToSend = string.Format("SET LOSPD ALL {0}", speed);
                    break;
                case RobotCommands.SetHighSpeed:
                    messageToSend = string.Format("SET HISPD ALL {0}", speed);
                    break;
                default:
                    return false;
            }

            return DoAction(command, messageToSend);
        }
        private bool SendGripSettingMessage(RobotCommands command, RobotArmTypes arm)
        {
            if (_commandResults.ContainsKey(command))
                _commandResults.TryRemove(command, out _);
            string messageToSend = string.Empty;
            string armId = string.Empty;

            switch (command)
            {
                case RobotCommands.Grip:
                    if (false == GetArmId(arm, ref armId))
                        return false;
                    messageToSend = string.Format("GRIP ON ARM {0}", armId);
                    break;
                case RobotCommands.Ungrip:
                    if (false == GetArmId(arm, ref armId))
                        return false;
                    messageToSend = string.Format("GRIP OFF ARM {0}", armId);
                    break;
                default:
                    return false;
            }

            return DoAction(command, messageToSend);
        }
        private bool SendMessage(RobotCommands command, RobotArmTypes arm, string location, int slot)
        {
            if (_commandResults.ContainsKey(command))
                _commandResults.TryRemove(command, out _);
            string messageToSend = string.Empty;
            string armId = string.Empty;
            //slot = slot + 1;
            switch (command)
            {
                case RobotCommands.Initialize:
                    messageToSend = "HOME ALL";
                    break;
                case RobotCommands.ApproachForPick:

                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("GOTO N {0} R RE Z DN SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                case RobotCommands.MoveToPick:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("GOTO N {0} R EX Z DN SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                case RobotCommands.Pick:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("PICK {0} SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.10.10. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.PickStepOne:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("EXTENDG {0} Z DN SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.10.10. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.PickStepTwo:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("RETRACTP {0} Z UP SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                case RobotCommands.ApproachForPlace:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("GOTO N {0} R RE Z UP SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                case RobotCommands.MoveToPlace:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("GOTO N {0} R EX Z UP SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                case RobotCommands.Place:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("PLACE {0} SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.10.10. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.PlaceStepOne:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("EXTENDG {0} Z UP SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.10.10. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.PlaceStepTwo:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("RETRACTP {0} Z DN SLOT {1} ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.10.10. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.ZaxisUp:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("ZAXIS {0} SLOT {1} UP ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.09.25. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.ZaxisMid:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("ZAXIS {0} SLOT {1} MID ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                // 2024.09.25. by dwlim. [ADD] Place 구분동작 추가로 인한 Command 추가
                case RobotCommands.ZaxisDown:
                    {
                        if (false == GetArmId(arm, ref armId))
                            return false;

                        messageToSend = string.Format("ZAXIS {0} SLOT {1} DN ARM {2}", location, slot.ToString(), armId);
                    }
                    break;
                case RobotCommands.GetWaferPresence:
                    messageToSend = string.Format("RQ WAFER ARM ALL");
                    break;
                case RobotCommands.AmpOn:
                    messageToSend = string.Format("SERVO ON ALL");
                    break;
                case RobotCommands.AmpOff:
                    messageToSend = string.Format("SERVO OFF ALL");
                    break;
                case RobotCommands.ClearError:
                    messageToSend = string.Format("CLEAR");
                    break;
                case RobotCommands.GetError:
                    messageToSend = string.Format("RQ ERR");
                    break;
                case RobotCommands.Hello:
                    messageToSend = string.Format("HLLO");
                    break;
                case RobotCommands.GetHighSpeed:
                    messageToSend = string.Format("RQ HISPD ALL");
                    break;
                case RobotCommands.GetLowSpeed:
                    messageToSend = string.Format("RQ LOSPD ALL");
                    break;
                default:
                    return false;
            }

            return DoAction(command, messageToSend);
        }
        private bool GetArmId(RobotArmTypes arm, ref string armId)
        {
            switch (arm)
            {
                case RobotArmTypes.UpperArm:
                    armId = "B";
                    return true;

                case RobotArmTypes.LowerArm:
                    armId = "A";
                    return true;

                //case ROBOT_ARM_TYPES.ALL:
                //    return false;
                default:
                    return false;
            }
        }

        private bool HasCommandResultData(RobotCommands command)
        {
            return _commandResults.ContainsKey(command);
        }

        private CommandResults GetCommandResult(RobotCommands command)
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

        // 2024.09.25. by dwlim. [ADD] 구분동작할 때, Wafer 유무를 확인할 필요가 있어서 추가
        private bool IsPresenceWafer(RobotArmTypes armType)
        {
            if (armType == RobotArmTypes.LowerArm)
            {
                return WaferPresenceLowerArm;
            }
            else if (armType == RobotArmTypes.UpperArm)
            {
                return WaferPresenceUpperArm;
            }

            return false;
        }
        // 2024.10.07. by dwlim. [ADD] Place Step 위한 함수 추가
        private void SetTimeOverByCommand(RobotCommands command)
        {
            uint time;
            switch (command)
            {
                case RobotCommands.Initialize:
                case RobotCommands.ApproachForPick:
                case RobotCommands.MoveToPick:
                case RobotCommands.Pick:
                case RobotCommands.PickStepOne:
                case RobotCommands.PickStepTwo:
                case RobotCommands.PickTest:
                case RobotCommands.ApproachForPlace:
                case RobotCommands.MoveToPlace:
                case RobotCommands.Place:
                case RobotCommands.PlaceStepOne:
                case RobotCommands.PlaceStepTwo:
                case RobotCommands.PlaceTest:
                    time = TimeLong;
                    break;

                case RobotCommands.AmpOn:
                case RobotCommands.AmpOff:
                case RobotCommands.GetWaferPresence:
                case RobotCommands.SetLowSpeed:
                case RobotCommands.SetHighSpeed:
                case RobotCommands.GetLowSpeed:
                case RobotCommands.GetHighSpeed:
                case RobotCommands.ClearError:
                case RobotCommands.GetError:
                case RobotCommands.Hello:
                case RobotCommands.Status:
                    time = TimeShort;
                    break;

                case RobotCommands.ZaxisDown:
                case RobotCommands.ZaxisMid:
                case RobotCommands.DoorLock:
                case RobotCommands.DoorUnlock:
                case RobotCommands.Grip:
                case RobotCommands.Ungrip:
                    time = TimeMiddle;
                    break;

                default:
                    time = TimeShort;
                    break;
            }

            _timeChecker.SetTickCount(time);
        }
        // 2024.10.10. by dwlim. [ADD] Place Step 개별함수
        private CommandResults PickStepOne(RobotArmTypes armType, string targetLocation, int slot)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        SetTimeOverByCommand(RobotCommands.PickStepOne);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(RobotCommands.Grip, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.Grip))
                                break;

                            _result = GetCommandResult(RobotCommands.Grip);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                tickCounter.SetTickCount(1000);
                                _actionStep = 2;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        // 1초 Delay 
                        if (tickCounter.IsTickOver(true))
                        {
                            _actionStep = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (SendMessage(RobotCommands.Ungrip, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.Ungrip))
                                break;

                            _result = GetCommandResult(RobotCommands.Ungrip);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 20;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (SendMessage(RobotCommands.ApproachForPick, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.ApproachForPick))
                                break;

                            _result = GetCommandResult(RobotCommands.ApproachForPick);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 30;
                            }
                        }
                    }
                    break;
                case 30:
                    {
                        if (SendMessage(RobotCommands.MoveToPick, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.MoveToPick))
                                break;

                            _result = GetCommandResult(RobotCommands.MoveToPick);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 40;
                            }
                        }
                    }
                    break;
                case 40:
                    {
                        if (SendMessage(RobotCommands.ZaxisMid, armType, targetLocation, slot))
                        {
                            ++_actionStep;
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
                        else
                        {
                            if (false == _commandResults.ContainsKey(RobotCommands.ZaxisMid))
                                break;

                            _result = GetCommandResult(RobotCommands.ZaxisMid);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 50;
                            }
                        }
                    }
                    break;
                case 50:
                    {
                        if (SendMessage(RobotCommands.Grip, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 51:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        if (false == _commandResults.ContainsKey(RobotCommands.Grip))
                            break;

                        _result = GetCommandResult(RobotCommands.Grip);
                        break;
                    }
                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        // 2024.10.10. by dwlim. [ADD] Place Step 개별함수
        private CommandResults PickStepTwo(RobotArmTypes armType, string targetLocation, int slot)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        SetTimeOverByCommand(RobotCommands.PickStepTwo);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(RobotCommands.ZaxisUp, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.ZaxisUp))
                                break;

                            _result = GetCommandResult(RobotCommands.ZaxisUp);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                tickCounter.SetTickCount(1000);
                                _actionStep = 10;
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        if (SendMessage(RobotCommands.GetWaferPresence, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.GetWaferPresence))
                                break;

                            _result = GetCommandResult(RobotCommands.GetWaferPresence);

                            if (false == IsPresenceWafer(armType))
                                break;

                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 20;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (SendMessage(RobotCommands.ApproachForPick, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.ApproachForPick))
                                break;

                            _result = GetCommandResult(RobotCommands.ApproachForPick);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 30;
                            }
                        }
                    }
                    break;
                case 30:
                    {
                        if (SendMessage(RobotCommands.GetWaferPresence, armType, targetLocation, slot))
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
                        if (false == _commandResults.ContainsKey(RobotCommands.Grip))
                            break;

                        _result = GetCommandResult(RobotCommands.Grip);

                        if (false == IsPresenceWafer(armType))
                            break;

                        break;
                    }
                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        // 2024.10.10. by dwlim. [ADD] Place Step 개별함수
        private CommandResults PlaceStepOne(RobotArmTypes armType, string targetLocation, int slot)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        SetTimeOverByCommand(RobotCommands.PlaceStepOne);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(RobotCommands.Grip, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.Grip))
                                break;

                            _result = GetCommandResult(RobotCommands.Grip);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                tickCounter.SetTickCount(1000);
                                _actionStep = 2;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        // 1초 Delay 
                        if (tickCounter.IsTickOver(true))
                        {
                            _actionStep = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (SendMessage(RobotCommands.ApproachForPlace, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.ApproachForPlace))
                                break;

                            _result = GetCommandResult(RobotCommands.ApproachForPlace);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 20;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (SendMessage(RobotCommands.GetWaferPresence, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.GetWaferPresence))
                                break;

                            _result = GetCommandResult(RobotCommands.GetWaferPresence);

                            if (false == IsPresenceWafer(armType))
                                break;

                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 30;
                            }
                        }
                    }
                    break;
                case 30:
                    {
                        if (SendMessage(RobotCommands.MoveToPlace, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.MoveToPlace))
                                break;

                            _result = GetCommandResult(RobotCommands.MoveToPlace);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 40;
                            }
                        }
                    }
                    break;
                case 40:
                    {
                        if (SendMessage(RobotCommands.GetWaferPresence, armType, targetLocation, slot))
                        {
                            ++_actionStep;
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
                        else
                        {
                            if (false == _commandResults.ContainsKey(RobotCommands.GetWaferPresence))
                                break;

                            _result = GetCommandResult(RobotCommands.GetWaferPresence);

                            if (false == IsPresenceWafer(armType))
                                break;

                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 50;
                            }
                        }
                    }
                    break;
                case 50:
                    {
                        if (SendMessage(RobotCommands.ZaxisMid, armType, targetLocation, slot))
                        {
                            ++_actionStep;
                        }
                    }
                    break;
                case 51:
                    {
                        if (IsTimeOver())
                        {
                            _result.CommandResult = CommandResult.Timeout;
                            break;
                        }
                        if (false == _commandResults.ContainsKey(RobotCommands.ZaxisMid))
                            break;

                        _result = GetCommandResult(RobotCommands.ZaxisMid);
                        break;
                    }
                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        // 2024.10.10. by dwlim. [ADD] Place Step 개별함수
        private CommandResults PlaceStepTwo(RobotArmTypes armType, string targetLocation, int slot)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        SetTimeOverByCommand(RobotCommands.PlaceStepTwo);
                        _result.CommandResult = CommandResult.Proceed;
                        _result.Description = string.Empty;
                        if (SendMessage(RobotCommands.Ungrip, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.Ungrip))
                                break;

                            _result = GetCommandResult(RobotCommands.Ungrip);
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
                        if (SendMessage(RobotCommands.ZaxisDown, armType, targetLocation, slot))
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
                            if (false == _commandResults.ContainsKey(RobotCommands.ZaxisDown))
                                break;

                            _result = GetCommandResult(RobotCommands.ZaxisDown);
                            if (_result.CommandResult.Equals(CommandResult.Completed))
                            {
                                _result.CommandResult = CommandResult.Proceed;
                                _result.Description = string.Empty;
                                _actionStep = 20;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (SendMessage(RobotCommands.ApproachForPick, armType, targetLocation, slot))
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
                        if (false == _commandResults.ContainsKey(RobotCommands.ApproachForPick))
                            break;

                        _result = GetCommandResult(RobotCommands.ApproachForPick);
                        break;
                    }
                default:
                    _result.CommandResult = CommandResult.Invalid;
                    _result.Description = string.Format("Invalid Seq Num : {0}", _actionStep);
                    break;
            }

            if (false == _result.CommandResult.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            return _result;
        }
        #endregion </Internals>

        #endregion </Methods>
    }
}