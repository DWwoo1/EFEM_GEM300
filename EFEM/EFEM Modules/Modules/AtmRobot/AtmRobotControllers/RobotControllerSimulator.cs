using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;

namespace EFEM.Modules.AtmRobot.AtmRobotControllers
{
    public class RobotControllerSimulator : AtmRobotController
    {
        #region <Constructors>
        public RobotControllerSimulator(int robotIndex,
            Dictionary<string, string> stationNames,
            EN_CONNECTION_TYPE interfaceType, 
            int commIndex) : base(robotIndex, stationNames, interfaceType, commIndex) 
        {
            CurrentSpeedLow = new Dictionary<RobotArmTypes, double>();
            CurrentSpeedHigh = new Dictionary<RobotArmTypes, double>();
            var armTypes = (RobotArmTypes[])Enum.GetValues(typeof(RobotArmTypes));
            foreach (var item in armTypes)
            {
                if (item == RobotArmTypes.LowerArm || item == RobotArmTypes.UpperArm)
                {
                    CurrentSpeedLow.Add(item, 50);
                    CurrentSpeedHigh.Add(item, 100);
                }
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly Dictionary<RobotArmTypes, double> CurrentSpeedLow = null;
        private readonly Dictionary<RobotArmTypes, double> CurrentSpeedHigh = null;
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
            return ExecuteAction(RobotCommands.Initialize, RobotArmTypes.All);
        }

        protected override CommandResults DoApproachForPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.ApproachForPick, armType);
        }

        protected override CommandResults DoApproachForPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.ApproachForPlace, armType);
        }
        protected override CommandResults DoMoveToPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.MoveToPick, armType);
        }
        protected override CommandResults DoPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.Pick, armType);
        }
        protected override CommandResults DoPickStepOne(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.Pick, armType);
        }
        protected override CommandResults DoPickStepTwo(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.Pick, armType);
        }
        protected override CommandResults DoMoveToPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.MoveToPlace, armType);
        }
        protected override CommandResults DoPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.Place, armType);
        }
        protected override CommandResults DoPlaceStepOne(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.PlaceStepOne, armType);
        }
        protected override CommandResults DoPlaceStepTwo(RobotArmTypes armType, string targetLocation, int slot)
        {
            return ExecuteAction(RobotCommands.PlaceStepTwo, armType);
        }
        protected override CommandResults DoSetLowSpeed(RobotArmTypes armType, double speed)
        {
            return ExecuteSpeedAction(RobotCommands.SetLowSpeed, armType, ref speed);
        }
        protected override CommandResults DoSetHighSpeed(RobotArmTypes armType, double speed)
        {
            return ExecuteSpeedAction(RobotCommands.SetHighSpeed, armType, ref speed);
        }
        protected override CommandResults DoGetLowSpeed(RobotArmTypes armType, ref double speed)
        {
            return ExecuteSpeedAction(RobotCommands.GetLowSpeed, armType, ref speed);
        }
        protected override CommandResults DoGetHighSpeed(RobotArmTypes armType, ref double speed)
        {
            return ExecuteSpeedAction(RobotCommands.GetHighSpeed, armType, ref speed);
        }
        protected override CommandResults DoGetWaferPresence()
        {
            return ExecuteAction(RobotCommands.GetWaferPresence, RobotArmTypes.All);
        }
        protected override CommandResults DoClear()
        {
            return ExecuteAction(RobotCommands.GetWaferPresence, RobotArmTypes.All);
        }
        protected override CommandResults DoGrip(RobotArmTypes armType)
        {
            return ExecuteAction(RobotCommands.GetWaferPresence, RobotArmTypes.All);
        }
        protected override CommandResults DoUngrip(RobotArmTypes armType)
        {
            return ExecuteAction(RobotCommands.GetWaferPresence, RobotArmTypes.All);
        }
        protected override CommandResults DoServoOn()
        {
            return ExecuteAction(RobotCommands.GetWaferPresence, RobotArmTypes.All);
        }
        protected override CommandResults DoServoOff()
        {
            return ExecuteAction(RobotCommands.GetWaferPresence, RobotArmTypes.All);
        }
        #endregion </Actions>

        #region <Thread>
        protected override bool RemoveTokens(string receivedMessage, ref string newString)
        {
            return true;
        }

        protected override void ParseMessages(string receivedMessage) { }
        #endregion </Thread>

        #region <Internals>
        private CommandResults ExecuteAction(RobotCommands command, RobotArmTypes armType)
        {
            var result = CommandResult.Proceed;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(1000);
                        ++_actionStep;
                    }
                    break;
                case 1:
                    if (IsTimeOver())
                    {
                        switch (command)
                        {
                            case RobotCommands.Initialize:
                                Initialized = true;
                                break;
                            case RobotCommands.Pick:
                                {
                                    switch (armType)
                                    {
                                        case RobotArmTypes.UpperArm:
                                            WaferPresenceUpperArm = true;
                                            break;
                                        case RobotArmTypes.LowerArm:
                                            WaferPresenceLowerArm = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case RobotCommands.Place:
                                {
                                    switch (armType)
                                    {
                                        case RobotArmTypes.UpperArm:
                                            WaferPresenceUpperArm = false;
                                            break;
                                        case RobotArmTypes.LowerArm:
                                            WaferPresenceLowerArm = false;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case RobotCommands.PlaceStepOne:
                                break;
                            case RobotCommands.PlaceStepTwo:
                                switch (armType)
                                {
                                    case RobotArmTypes.UpperArm:
                                        WaferPresenceUpperArm = false;
                                        break;
                                    case RobotArmTypes.LowerArm:
                                        WaferPresenceLowerArm = false;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            //case RobotCommands.ApproachForPick:
                            //    break;
                            case RobotCommands.MoveToPick:
                                break;
                            //case RobotCommands.ApproachForPlace:
                            //    break;
                            case RobotCommands.MoveToPlace:
                                break;
                            case RobotCommands.GetWaferPresence:
                                break;
                            case RobotCommands.AmpOn:
                                break;
                            case RobotCommands.AmpOff:
                                break;
                            case RobotCommands.ClearError:
                                break;
                            //case RobotCommands.GetError:
                            //    break;
                            //case RobotCommands.Hello:
                            //    break;
                            default:
                                break;
                        }
                        
                        result = CommandResult.Completed;
                    }
                    break;

                default:
                    result = CommandResult.Invalid;
                    break;
            }

            if (false == result.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            _result.CommandResult = result;
            return _result;
        }
        private CommandResults ExecuteSpeedAction(RobotCommands command, RobotArmTypes armType, ref double speed)
        {
            var result = CommandResult.Proceed;
            switch (_actionStep)
            {
                case 0:
                    {
                        _timeChecker.SetTickCount(5000);
                        ++_actionStep;
                    }
                    break;
                case 1:
                    if (IsTimeOver())
                    {
                        switch (command)
                        {
                            case RobotCommands.SetHighSpeed:
                                CurrentSpeedHigh[armType] = speed;
                                break;
                            case RobotCommands.SetLowSpeed:
                                CurrentSpeedLow[armType] = speed;
                                break;
                            case RobotCommands.GetHighSpeed:
                                speed = CurrentSpeedHigh[armType];
                                break;
                            case RobotCommands.GetLowSpeed:
                                speed = CurrentSpeedLow[armType];
                                break;
                            //case RobotCommands.Initialize:
                            //    Initialized = true;
                            //    break;
                            //case RobotCommands.Pick:
                            //    break;
                            //case RobotCommands.Place:
                            //    break;
                            //case RobotCommands.ApproachForPick:
                            //    break;
                            //case RobotCommands.MoveToPick:
                            //    break;
                            //case RobotCommands.ApproachForPlace:
                            //    break;
                            //case RobotCommands.MoveToPlace:
                            //    break;
                            //case RobotCommands.GetWaferPresence:
                            //    break;
                            //case RobotCommands.AmpOn:
                            //    break;
                            //case RobotCommands.AmpOff:
                            //    break;
                            //case RobotCommands.ClearError:
                            //    break;
                            //case RobotCommands.GetError:
                            //    break;
                            //case RobotCommands.Hello:
                            //    break;
                            default:
                                break;
                        }

                        result = CommandResult.Completed;
                    }
                    break;

                default:
                    result = CommandResult.Invalid;
                    break;
            }

            if (false == result.Equals(CommandResult.Proceed))
            {
                DoingAction = RobotCommands.Idle;
                _actionStep = 0;
            }

            _result.CommandResult = result;
            return _result;
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
        #endregion </Internals>

        #endregion </Methods>
    }
}