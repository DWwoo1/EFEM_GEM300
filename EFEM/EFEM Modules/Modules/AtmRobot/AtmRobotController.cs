using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Linq;

using TickCounter_;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Common;
using EFEM.Defines.Communicator;
using EFEM.Defines.AtmRobot;

namespace EFEM.Modules.AtmRobot
{
    public abstract class AtmRobotController// : AtmRobot, IRobotStateHandler
    {
        #region <Constructors>
        public AtmRobotController(int robotIndex,
            Dictionary<string, string> stationNames,
            EN_CONNECTION_TYPE interfaceType,
            int commIndex)// : base(name)
        {
            Index = robotIndex;
            _comm = new Communicator(interfaceType, commIndex);
            TicksForConnection.SetTickCount(RetryInterval);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly Communicator _comm = null;
        private string _receivedMessage = string.Empty;
        private string _receivedTemporayMessage = string.Empty;

        protected int _actionStep = 0;
        protected TickCounter _timeChecker = new TickCounter();
        protected CommandResults _result = new CommandResults(RobotCommands.Idle.ToString() ,CommandResult.Error);
        protected AtmRobotLogger _logger = null;

        private readonly TickCounter TicksForConnection = new TickCounter();
        private readonly uint RetryInterval = 5000;
        private const int RetryCountLimit = 3;
        private int _retryCount;
        #endregion </Fields>

        #region <Properties>
        public int Index { get; private set; }
        public bool Initialized { get; protected set; }
        public RobotCommands DoingAction { get; protected set; }
        public bool WaferPresenceLowerArm
        {
            get; protected set;
            //get
            //{
            //    if (_myStateInformation == null)
            //        return false;

            //    return _myStateInformation.WaferPresenceLowerArm;
            //}
        }
        public bool WaferPresenceUpperArm 
        {
            get; protected set;
            //get
            //{
            //    if (_myStateInformation == null)
            //        return false;

            //    return _myStateInformation.WaferPresenceUpperArm;
            //}
        }
        #endregion </Properties>

        #region <Methods>

        #region <IRobot Interfaces>
        public void AssignLogger(ref AtmRobotLogger logger)
        {
            _logger = logger;
        }
        public void InitializeFlags()
        {
            InitAction();
        }
        public CommandResults Initialize()
        {
            return DoInitialize(); 
        }
        public CommandResults ApproachForPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            return DoApproachForPick(armType, targetLocation, slot);
        }
        public CommandResults Pick(RobotArmTypes armType, string targetLocation, int slot)
        {
            return DoPick(armType, targetLocation, slot);
        }
        public CommandResults MoveToPick(RobotArmTypes armType, string targetLocation, int slot)
        {
            return DoMoveToPick(armType, targetLocation, slot);
        }
        public CommandResults ApproachForPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            return DoApproachForPlace(armType, targetLocation, slot);
        }
        public CommandResults Place(RobotArmTypes armType, string targetLocation, int slot)
        {
            return DoPlace(armType, targetLocation, slot);
        }
        public CommandResults MoveToPlace(RobotArmTypes armType, string targetLocation, int slot)
        {
            return DoMoveToPlace(armType, targetLocation, slot);
        }
        public CommandResults SetLowSpeed(RobotArmTypes armType, double speed)
        {
            return DoSetLowSpeed(armType, speed);
        }
        public CommandResults SetHighSpeed(RobotArmTypes armType, double speed)
        {
            return DoSetHighSpeed(armType, speed);
        }
        public CommandResults GetLowSpeed(RobotArmTypes armType, ref double speed)
        {
            return DoGetLowSpeed(armType, ref speed);
        }
        public CommandResults GetHighSpeed(RobotArmTypes armType, ref double speed)
        {
            return DoGetHighSpeed(armType, ref speed);
        }
        public CommandResults GetWaferPresence()
        {
            return DoGetWaferPresence();
        }
        public CommandResults Clear()
        {
            return DoClear();
        }
        public CommandResults Grip(RobotArmTypes armType)
        {
            return DoGrip(armType);
        }
        public CommandResults Ungrip(RobotArmTypes armType)
        {
            return DoUngrip(armType);
        }
        public CommandResults ServoOn()
        {
            return DoServoOn();
        }
        public CommandResults ServoOff()
        {
            return DoServoOff();
        }
        #endregion </IRobot Interfaces>

        #region <Actions>
        public virtual void InitAction()
        {
            _actionStep = 0;
            DoingAction = RobotCommands.Idle;
        }
        public abstract bool InitController();
        public abstract bool CloseController();
        protected abstract CommandResults DoInitialize();
        protected abstract CommandResults DoApproachForPick(RobotArmTypes armType, string targetLocation, int slot);
		protected abstract CommandResults DoMoveToPick(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoPick(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoPickStepOne(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoPickStepTwo(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoApproachForPlace(RobotArmTypes armType, string targetLocation, int slot);
		protected abstract CommandResults DoMoveToPlace(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoPlace(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoPlaceStepOne(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoPlaceStepTwo(RobotArmTypes armType, string targetLocation, int slot);
        protected abstract CommandResults DoSetLowSpeed(RobotArmTypes armType, double speed);
        protected abstract CommandResults DoGetLowSpeed(RobotArmTypes armType, ref double speed);
        protected abstract CommandResults DoSetHighSpeed(RobotArmTypes armType, double speed);
        protected abstract CommandResults DoGetHighSpeed(RobotArmTypes armType, ref double speed);
        protected abstract CommandResults DoGetWaferPresence();
        protected abstract CommandResults DoClear();
        protected abstract CommandResults DoGrip(RobotArmTypes armType);
        protected abstract CommandResults DoUngrip(RobotArmTypes armType);
        protected abstract CommandResults DoServoOn();
        protected abstract CommandResults DoServoOff();
        #endregion </Actions>

        #region <Communication>
        public bool IsConnected()
        {
            if (this is AtmRobotControllers.RobotControllerSimulator)
                return true;

            if (_comm == null)
                return false;

            return _comm.IsConnected;
        }
        protected bool DoAction(RobotCommands command, byte[] messageToSend)
        {
            if (false == IsConnected())
                return false;

            if (false == DoingAction.Equals(RobotCommands.Idle))
                return false;

            DoingAction = command;

            return _comm.WriteByteData(messageToSend);
        }

        protected bool DoAction(RobotCommands command, string messageToSend)
        {
            if (false == IsConnected())
                return false;

            if (false == DoingAction.Equals(RobotCommands.Idle))
                return false;

            DoingAction = command;

            _logger.WriteCommLog(messageToSend, false);

            return _comm.WriteStringData(messageToSend);
        }

        protected bool ReceiveByteData(ref byte[] receivedMessage)
        {
            if (false == IsConnected())
                return false;

            return _comm.ReadByteData(ref receivedMessage);
        }

        protected bool ReceiveStringData(ref string receivedMessage)
        {
            if (false == IsConnected())
                return false;

            return _comm.ReadStringData(ref receivedMessage);
        }

        // 2024.12.21. jhlim [ADD] 연결되지 않는 경우 무한으로 익셉션이 발생해 로그가 계속 쌓이게된다..
        private bool RetryConnectIfNeed()
        {
            if (false == IsConnected())
            {
                if (_retryCount < RetryCountLimit && TicksForConnection.IsTickOver(false))
                {
                    _comm.OpenPort();

                    TicksForConnection.SetTickCount(RetryInterval);
                    ++_retryCount;
                }

                return false;
            }
            else
            {
                // Flag Reset
                _retryCount = 0;
                TicksForConnection.SetTickCount(RetryInterval);

                return true;
            }
        }
        // 2024.12.21. jhlim [END]
        #endregion </Communication>

        #region <Thread>
        protected abstract bool RemoveTokens(string receivedMessage, ref string newString);
        protected abstract void ParseMessages(string receivedMessage);

        public void Monitoring()
        {
            if (_comm == null)
                return;

            // 2024.12.21. jhlim [MOD] 내용이 길어져 메서드로 변경
            if (false == RetryConnectIfNeed())
                return;

            //if (false == _comm.IsConnected)
            //{
            //    _comm.OpenPort();

            //    return;
            //}
            // 2024.12.21. jhlim [END]

            _receivedTemporayMessage = string.Empty; _receivedMessage = string.Empty;
            if (false == _comm.ReadStringData(ref _receivedTemporayMessage))
                return;

            if (false == RemoveTokens(_receivedTemporayMessage, ref _receivedMessage))
                return;

            _logger.WriteCommLog(_receivedMessage, true);

            ParseMessages(_receivedMessage);

            Execute();
        }

        private void Execute()
        {

        }
        #endregion </Thread>

        #endregion </Methods>
    }
}