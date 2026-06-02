using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Recipe;

using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking;

namespace EFEM.Modules.AtmRobot
{
    public class AtmRobotOperator
    {
        #region <Constructors>
        public AtmRobotOperator(int robotIndex,
            string name,
            AtmRobotController controller,
            Dictionary<string, string> stationNames)
        {
            Name = name;
            Index = robotIndex;
            _logger = new AtmRobotLogger(Name);

            Controller = controller;
            Controller.AssignLogger(ref _logger);
            StationInformation = new ReadOnlyDictionary<string, string>(stationNames);
            MyStateInformation = new RobotStateInformation();

            _recipe = Recipe.GetInstance();

            UseArms = new Dictionary<RobotArmTypes, PARAM_EQUIPMENT>
            {
                [RobotArmTypes.UpperArm] = PARAM_EQUIPMENT.UseRobotUpperArm,
                [RobotArmTypes.LowerArm] = PARAM_EQUIPMENT.UseRobotLowerArm,
            };
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ReadOnlyDictionary<string, string> StationInformation = null;
        private readonly AtmRobotController Controller = null;
        private readonly RobotStateInformation MyStateInformation = null;
        private readonly AtmRobotLogger _logger = null;

        //private static SubstrateManager SubstrateManager = null;
        private static Recipe _recipe = null;

        private const string InvalidStation = "Invalid location or station name";
        private const string HasNoSubstrates = "There are no substrates at the location";
        private const string HasSubstratesAlready = "There are already substrates at the location";
        private const string HasSubstrateAlready = "The arm has substrates already";
        private const string InvalidActionStep = "Invalid Action Step";

        private int _actionStep = 0;
        private string _temporaryStationName = string.Empty;
        private int _temporarySlot = 0;
        private string _currentAction;

        private readonly Dictionary<RobotArmTypes, PARAM_EQUIPMENT> UseArms = null;

        private int _digitalInputIndexOfBusy = -1;
        private int _digitalInputIndexOfAlarm = -1;
        private int _digitalInputIndexOfServoStatus = -1;
        private Func<int, bool> _functionToReadInput = null;
        #endregion </Fields>

        #region <Properties>
        public string Name { get; private set; }
        public int Index { get; private set; }
        public bool IsConnected
        {
            get
            {
                if (Controller == null)
                    return false;

                return Controller.IsConnected();
            }
        }
        public bool IsRobotBusy
        {
            get
            {
                if (_functionToReadInput == null || _digitalInputIndexOfBusy < 0 ||
                    Controller is AtmRobotControllers.RobotControllerSimulator)
                {
                    return (false == Controller.DoingAction.Equals(RobotCommands.Idle));
                }

                return _functionToReadInput(_digitalInputIndexOfBusy);
            }
        }
        public bool IsRobotAlarm
        {
            get
            {
                if (_functionToReadInput == null || _digitalInputIndexOfAlarm < 0 ||
                    Controller is AtmRobotControllers.RobotControllerSimulator)
                {
                    return false;
                }

                return _functionToReadInput(_digitalInputIndexOfAlarm);
            }
        }
        public bool IsServoOn
        {
            get
            {
                if (_functionToReadInput == null || _digitalInputIndexOfServoStatus < 0 ||
                    Controller is AtmRobotControllers.RobotControllerSimulator)
                {
                    return false;
                }

                return _functionToReadInput(_digitalInputIndexOfServoStatus);
            }
        }
        public bool Initialized
        {
            get
            {
                if (Controller == null)
                    return false;

                return Controller.Initialized;
            }
        }
        public bool WaferPresenceLowerArm
        {
            get
            {
                if (Controller == null)
                    return false;

                return Controller.WaferPresenceLowerArm;
            }
        }
        public bool WaferPresenceUpperArm
        {
            get
            {
                if (Controller == null)
                    return false;

                return Controller.WaferPresenceUpperArm;
            }
        }
        public AtmRobotLogger Logger
        {
            get
            {
                return _logger;
            }
        }
        protected static SubstrateManager SubstrateManager
        {
            get
            {
                return SubstrateManager.Instance;
            }
        }
        #endregion </Properties>

        #region <Methods>
        public void InitOperator()
        {
            Controller.InitController();
        }
        public RobotStateInformation GetStateInformation()
        {
            return MyStateInformation;
        }
        public bool GetAvailableArm(bool picking, ref List<RobotArmTypes> availableArms)
        {
            availableArms.Clear();
            int countOfAvailableArms = 0;
            foreach (var item in UseArms)
            {
                bool useArm = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, item.Value.ToString(), true);
                if (false == useArm)
                    continue;

                bool hasSubstrate = HasSubstrateAtArm(item.Key);
                if (false == FrameOfSystem3.Task.TaskOperator.GetInstance().IsSimulationMode())
                {
                    switch (item.Key)
                    {
                        case RobotArmTypes.UpperArm:
                            hasSubstrate |= WaferPresenceUpperArm;
                            break;
                        case RobotArmTypes.LowerArm:
                            hasSubstrate |= WaferPresenceLowerArm;
                            break;
                        default:
                            break;
                    }
                }

                if (picking != hasSubstrate)
                {
                    availableArms.Add(item.Key);
                    ++countOfAvailableArms;
                }
                else
                {
                    if (picking && hasSubstrate)
                    {
                        // Unload 해야할 자재면 수를 증가시킨다.
                        if (GetSubstrate(item.Key, out var temporarySubstrate) && NeedToUnload(temporarySubstrate.ProcessingStatus))
                        {
                            ++countOfAvailableArms;
                        }
                    }
                }
            }

            return countOfAvailableArms > 0;
            //if (picking)
            //{
            //}
            //else
            //{
            //    return countOfAvailableArms > 0;
            //}


            // 둘 중 하나라도 정보가 있으면 True, 자재 감지는 되는데 정보가 없으면 에러처리 필요
            //bool isAvailableUpperArm = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.UseRobotUpperArm.ToString(), true);
            //bool isAvailableLowerArm = _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.UseRobotLowerArm.ToString(), true);

            //bool hasSubstrateAtUpperArm, hasSubstrateAtLowerArm;
            //if (isAvailableUpperArm)
            //{
            //    hasSubstrateAtUpperArm = HasSubstrateAtArm(RobotArmTypes.UpperArm) || WaferPresenceUpperArm;
            //}
            //else
            //{
            //    hasSubstrateAtUpperArm = picking;
            //}

            //if (isAvailableLowerArm)
            //{
            //    hasSubstrateAtLowerArm = HasSubstrateAtArm(RobotArmTypes.LowerArm) || WaferPresenceLowerArm;
            //}
            //else
            //{
            //    hasSubstrateAtLowerArm = picking;
            //}

            //if (picking)
            //{
            //    // 하나는 비워야한다. 둘 중 하나라도 있으면 False
            //    if (hasSubstrateAtUpperArm || hasSubstrateAtLowerArm)
            //        return false;
            //}

            //if (picking != hasSubstrateAtUpperArm)
            //    availableArms.Add(RobotArmTypes.UpperArm);

            //if (picking != hasSubstrateAtLowerArm)
            //    availableArms.Add(RobotArmTypes.LowerArm);

            //return availableArms.Count > 0;
        }
        public bool HasSubstrateAtArm(RobotArmTypes armType)
        {
            return SubstrateManager.GetSubstrateAtRobot(Name, armType, out _);
        }
        public bool GetSubstrate(RobotArmTypes armType, out Substrate substrate)
        {
            return SubstrateManager.GetSubstrateAtRobot(Name, armType, out substrate);
        }

        #region <Assigns>
        public void AttachBusySignalByDigitalInput(int indexOFInput, Func<int, bool> functionToReadInput)
        {
            _digitalInputIndexOfBusy = indexOFInput;
            if (_functionToReadInput == null)
            {
                _functionToReadInput = functionToReadInput;
            }
        }
        public void AttachAlarmSignalByDigitalInput(int indexOFInput, Func<int, bool> functionToReadInput)
        {
            _digitalInputIndexOfAlarm = indexOFInput;
            if (_functionToReadInput == null)
            {
                _functionToReadInput = functionToReadInput;
            }
        }
        public void AttachServoStatusByDigitalInput(int indexOFInput, Func<int, bool> functionToReadInput)
        {
            _digitalInputIndexOfServoStatus = indexOFInput;
            if (_functionToReadInput == null)
            {
                _functionToReadInput = functionToReadInput;
            }
        }
        #endregion </Assigns>

        #region <Action>
        public void InitAction()
        {
            if (Controller == null)
                return;

            _actionStep = 0;
            _temporaryStationName = string.Empty;

            Controller.InitAction();
        }
        public CommandResults Initialize()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.Initialize);
                    ++_actionStep;
                    break;
            }

            var result = Controller.Initialize();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.Initialize, result);
                    break;

            }

            return result;
        }
        public CommandResults ApproachForPick(RobotArmTypes armType, string locationId)
        {
            if (false == ConvertStationByLocation(locationId, ref _temporaryStationName, ref _temporarySlot))
                return new CommandResults(RobotCommands.ApproachForPick.ToString(), CommandResult.Error, InvalidStation);

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.ApproachForPick, armType, locationId);
                    ++_actionStep;
                    break;
            }

            var result = Controller.ApproachForPick(armType, _temporaryStationName, _temporarySlot);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.ApproachForPick, result);
                    break;

            }

            return result;
        }
        public CommandResults MoveToPick(RobotArmTypes armType, string locationId)
        {
            if (false == ConvertStationByLocation(locationId, ref _temporaryStationName, ref _temporarySlot))
                return new CommandResults(RobotCommands.MoveToPick.ToString(), CommandResult.Error, InvalidStation);

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.MoveToPick, armType, locationId);
                    ++_actionStep;
                    break;
            }

            var result = Controller.MoveToPick(armType, _temporaryStationName, _temporarySlot);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.MoveToPick, result);
                    break;

            }

            return result;
        }
        public CommandResults Pick(RobotArmTypes armType, string locationId, bool testMode, string substrateKey)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _currentAction = RobotCommands.Pick.ToString();

                        // 장소의 유효성을 검사한다.
                        if (false == ConvertStationByLocation(locationId, ref _temporaryStationName, ref _temporarySlot))
                            return new CommandResults(_currentAction, CommandResult.Error, InvalidStation);
                        
                        if (false == testMode)
                        {
                            // 1) 암에 자재가 있는지
                            if (GetSubstrate(armType, out _))
                            {
                                return new CommandResults(_currentAction,
                                    CommandResult.Error, HasSubstrateAlready);
                            }

                            LocationServer.FindLocationById(locationId, out var location);
                            if (false == SubstrateManager.GetSubstrateByLocationAndKey(location, substrateKey, out _))
                            {
                                return new CommandResults(_currentAction,
                                    CommandResult.Error, HasNoSubstrates);
                            }
                        }

                        _logger.WriteOperationStartLog(RobotCommands.Pick, armType, locationId);
                        
                        ++_actionStep;
                        return new CommandResults(_currentAction, CommandResult.Proceed);
                    }

                case 1:
                    {
                        CommandResults result;

                        if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsDryRunMode())
                            result = Controller.ApproachForPlace(armType, _temporaryStationName, _temporarySlot);
                        else
                            result = Controller.Pick(armType, _temporaryStationName, _temporarySlot);

                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            default:
                                _actionStep = 0;
                                _logger.WriteOperationEndLog(RobotCommands.Pick, result);
                                if (false == testMode)
                                {
                                    if (result.CommandResult.Equals(CommandResult.Completed))
                                    {
                                        if (false == LocationServer.FindLocationById(locationId, out var location))
                                        {

                                        }

                                        ExecuteAfterPick(location);

                                        if (false == SubstrateManager.GetSubstrateByLocationAndKey(location, substrateKey, out var toTransfer))
                                        {
                                            return new CommandResults(_currentAction, CommandResult.Error, HasNoSubstrates);
                                        }

                                        var key = toTransfer.UniqueKey;
                                        if (LocationServer.GetRobotLocation(Name, armType, out var loc))
                                        {
                                            var transport = toTransfer.TransportStatus;
                                            if (location.LocationKind == ModuleType.LoadPort)
                                            {
                                                transport = TransportStates.AtWork;
                                                //SubstrateManager.SetTransferStatusByKey(key, TransportStates.AtWork);
                                            }

                                            SubstrateManager.TransferSubstrate(
                                                key, 
                                                location, 
                                                loc, 
                                                transport,
                                                OccupancyChangeReason.PickedByRobot,
                                                OccupancyChangeReason.PickedByRobot);

                                            SubstrateManager.SetCurrentCarrierKeyByKey(key, string.Empty);
                                            SubstrateManager.SaveDataByKey(key);
                                        }
                                        //SubstrateManager.MoveMaterialToRobot(targetLocation, Name, armType, _substrateToTransfer);
                                    }
                                }
                                break;

                        }

                        return result;
                    }

                default:
                    return new CommandResults(_currentAction, CommandResult.Invalid, InvalidActionStep);
            }
        }
        public CommandResults ApproachForPlace(RobotArmTypes armType, string locationId)
        {
            if (false == ConvertStationByLocation(locationId, ref _temporaryStationName, ref _temporarySlot))
                return new CommandResults(RobotCommands.ApproachForPlace.ToString(), CommandResult.Error, InvalidStation);

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.ApproachForPlace, armType, locationId);
                    ++_actionStep;
                    break;
            }

            var result = Controller.ApproachForPlace(armType, _temporaryStationName, _temporarySlot);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.ApproachForPlace, result);
                    break;

            }

            return result;
        }
        public CommandResults MoveToPlace(RobotArmTypes armType, string locationId)
        {
            if (false == ConvertStationByLocation(locationId, ref _temporaryStationName, ref _temporarySlot))
                return new CommandResults(RobotCommands.MoveToPlace.ToString(), CommandResult.Error, InvalidStation);

            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.MoveToPlace, armType, locationId);
                    ++_actionStep;
                    break;
            }

            var result = Controller.MoveToPlace(armType, _temporaryStationName, _temporarySlot);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.MoveToPlace, result);
                    break;

            }

            return result;
        }

        public CommandResults Place(RobotArmTypes armType, string locationId, bool testMode, string substrateKey)
        {
            switch (_actionStep)
            {
                case 0:
                    {
                        _currentAction = RobotCommands.Place.ToString();

                        // 장소의 유효성을 검사한다.
                        if (false == ConvertStationByLocation(locationId, ref _temporaryStationName, ref _temporarySlot))
                            return new CommandResults(_currentAction, CommandResult.Error, InvalidStation);
                        
                        if (false == testMode)
                        {
                            // 내 장소에서 Sub 정보를 찾는다.
                            if (false == GetSubstrate(armType, out _))
                            {
                                return new CommandResults(_currentAction,
                                    CommandResult.Error, HasNoSubstrates);
                            }

                            if (LocationServer.FindLocationById(locationId, out var location) &&
                                location is LoadPortLocation)
                            {
                                var lpLocation = location as LoadPortLocation;
                                if (lpLocation == null)
                                    return new CommandResults(_currentAction, CommandResult.Error, InvalidStation);

                                // 장소에 Sub이 있으면 에러다.
                                if (SubstrateManager.HasSubstrateAtLoadPort(lpLocation.PortId, lpLocation.Slot))
                                {
                                    return new CommandResults(_currentAction, CommandResult.Error, HasSubstratesAlready);
                                }
                            }
                        }
                        _logger.WriteOperationStartLog(RobotCommands.Place, armType, locationId);

                        ++_actionStep;
                        return new CommandResults(_currentAction, CommandResult.Proceed);
                    }

                case 1:
                    {
                        CommandResults result;

                        if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsDryRunMode())
                            result = Controller.ApproachForPlace(armType, _temporaryStationName, _temporarySlot);
                        else
                            result = Controller.Place(armType, _temporaryStationName, _temporarySlot);

                        switch (result.CommandResult)
                        {
                            case CommandResult.Proceed:
                                break;
                            default:
                                _actionStep = 0;
                                _logger.WriteOperationEndLog(RobotCommands.Place, result);
                                if (false == testMode)
                                {
                                    if (result.CommandResult.Equals(CommandResult.Completed))
                                    {
                                        if (false == GetSubstrate(armType, out var toTransfer))
                                        {
                                            return new CommandResults(_currentAction,
                                                CommandResult.Error, HasNoSubstrates);
                                        }

                                        if (false == LocationServer.FindLocationById(locationId, out var location))
                                        {

                                        }

                                        var key = toTransfer.UniqueKey;
                                        ExecuteAfterPlace(key, location, toTransfer.TransportStatus, out var state);

                                        if (LocationServer.GetRobotLocation(Name, armType, out var loc))
                                        {
                                            SubstrateManager.TransferSubstrate(
                                                key, 
                                                loc, 
                                                location, 
                                                state,
                                                OccupancyChangeReason.PlacedByRobot, 
                                                OccupancyChangeReason.PlacedByRobot);

                                            if (location.LocationKind == ModuleType.LoadPort)
                                            {
                                                var lpLocation = location as LoadPortLocation;
                                                if (lpLocation != null)
                                                {
                                                    var carrierKey = CarrierManagementServer.Instance.GetCarrierKey(lpLocation.PortId);
                                                    SubstrateManager.SetCurrentCarrierKeyByKey(key, carrierKey);
                                                    SubstrateManager.SaveDataByKey(key);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                        }                      

                        return result;
                    }

                default:
                    return new CommandResults(_currentAction, CommandResult.Invalid, InvalidActionStep);
            }
        }

        //public CommandResults Place(RobotArmTypes armType, string targetLocation, int slot)
        //{
        //    string stationName = string.Empty;
        //    if (false == ConvertStationByLocation(targetLocation, ref stationName))
        //        return new CommandResults(RobotCommands.Place.ToString(), CommandResult.Error, InvalidStation);

        //    UpdateLocationInfo(false, targetLocation, slot);

        //    CommandResults result = Controller.Place(armType, stationName, slot);
        //    if (result.CommandResult.Equals(CommandResult.Completed))
        //    {

        //    }

        //    return result;
        //}
        public CommandResults GetWaferPresence()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.GetWaferPresence);
                    ++_actionStep;
                    break;
            }

            var result = Controller.GetWaferPresence();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.GetWaferPresence, result);
                    break;

            }

            return result;
        }
        public CommandResults GetHighSpeed(RobotArmTypes armType, ref double speed)
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.GetHighSpeed);
                    ++_actionStep;
                    break;
            }

            var result = Controller.GetHighSpeed(armType, ref speed);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.GetHighSpeed, result);
                    break;

            }

            return result;
        }
        public CommandResults GetLowSpeed(RobotArmTypes armType, ref double speed)
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.GetLowSpeed);
                    ++_actionStep;
                    break;
            }

            var result = Controller.GetLowSpeed(armType, ref speed);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.GetLowSpeed, result);
                    break;

            }

            return result;
        }
        public CommandResults SetHighSpeed(RobotArmTypes armType, double speed)
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.SetHighSpeed);
                    ++_actionStep;
                    break;
            }

            var result = Controller.SetHighSpeed(armType, speed);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.SetHighSpeed, result);
                    break;

            }

            return result;
        }
        public CommandResults SetLowSpeed(RobotArmTypes armType, double speed)
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.SetLowSpeed);
                    ++_actionStep;
                    break;
            }

            var result = Controller.SetLowSpeed(armType, speed);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.SetLowSpeed, result);
                    break;

            }

            return result;
        }
        public CommandResults Clear()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.ClearError);
                    ++_actionStep;
                    break;
            }

            var result = Controller.Clear();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.ClearError, result);
                    break;

            }

            return result;
        }
        public CommandResults Grip(RobotArmTypes armType)
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.Grip);
                    ++_actionStep;
                    break;
            }

            var result = Controller.Grip(armType);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.Grip, result);
                    break;

            }

            return result;
        }
        public CommandResults Ungrip(RobotArmTypes armType)
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.Ungrip);
                    ++_actionStep;
                    break;
            }

            var result = Controller.Ungrip(armType);
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.Ungrip, result);
                    break;

            }

            return result;
        }
        public CommandResults ServoOn()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.AmpOn);
                    ++_actionStep;
                    break;
            }

            var result = Controller.ServoOn();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.AmpOn, result);
                    break;

            }

            return result;
        }
        public CommandResults ServoOff()
        {
            switch (_actionStep)
            {
                case 0:
                    _logger.WriteOperationStartLog(RobotCommands.AmpOff);
                    ++_actionStep;
                    break;
            }

            var result = Controller.ServoOff();
            switch (result.CommandResult)
            {
                case CommandResult.Proceed:
                    break;
                default:
                    _actionStep = 0;
                    _logger.WriteOperationEndLog(RobotCommands.AmpOff, result);
                    break;

            }

            return result;
        }
        #endregion </Action>

        #region <Execute>
        public void Monitoring()
        {
            if (Controller == null)
                return;

            Controller.Monitoring();
        }
        #endregion </Execute>

        #region <Location>
        private bool ConvertStationByLocation(string locationId, ref string station, ref int slot)
        {
            if (false == LocationServer.FindLocationById(locationId, out var targetLocation))
                return false;

            string stationName = string.Empty;
            if (targetLocation is LoadPortLocation)
            {
                var location = targetLocation as LoadPortLocation;

                int portId = location.PortId;
                stationName = LocationServer.GetLoadPortLocationId(portId);
                slot = location.Slot;
            }
            else if (targetLocation is ProcessModuleLocation)
            {
                var location = targetLocation as ProcessModuleLocation;

                stationName = locationId;
                slot = 1;
                
            }
            else if (targetLocation is RobotLocation)
            {
                return false;
            }

            return StationInformation.TryGetValue(stationName, out station);
        }
        #endregion </Location>

        #region <ETC>
        private void ExecuteAfterPick(Location loc)
        {
            if (loc is LoadPortLocation)
            {
                var lpLoc = loc as LoadPortLocation;
                int portId = lpLoc.PortId;
                if (CarrierManagementServer.Instance.GetCarrierAccessingStatus(portId).Equals(EFEM.Defines.LoadPort.CarrierAccessStates.NotAccessed))
                {
                    CarrierManagementServer.Instance.SetCarrierAccessingStatus(portId, EFEM.Defines.LoadPort.CarrierAccessStates.InAccessed);
                    CarrierManagementServer.Instance.SaveCarrierData(portId);
                }
            }
        }
        private void ExecuteAfterPlace(string key, Location loc, TransportStates initState, out TransportStates state)
        {
            state = initState;
            if (loc is LoadPortLocation)
            {
                SubstrateManager.TryGetTransportStatusByProcessingStatus(key, out state);

                // Destination
                var lpLoc = loc as LoadPortLocation;
                int destPort = lpLoc.PortId;
                if (CarrierManagementServer.Instance.GetCarrierAccessingStatus(destPort).Equals(EFEM.Defines.LoadPort.CarrierAccessStates.NotAccessed))
                {
                    CarrierManagementServer.Instance.SetCarrierAccessingStatus(destPort, EFEM.Defines.LoadPort.CarrierAccessStates.InAccessed);
                    CarrierManagementServer.Instance.SaveCarrierData(destPort);
                }
            }
        }

        private bool NeedToUnload(ProcessingStates status)
        {
            switch (status)
            {
                case ProcessingStates.NeedsProcessing:
                case ProcessingStates.InProcess:
                case ProcessingStates.Lost:
                    return false;
                case ProcessingStates.Processed:
                case ProcessingStates.Rejected:
                case ProcessingStates.Stopped:
                case ProcessingStates.Aborted:
                case ProcessingStates.Skipped:
                    return true;

                default:
                    return false;
            }
        }
        #endregion </ETC>
        #endregion </Methods>
    }
}
