using System;
using System.Collections.Generic;

using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.Modules.AtmRobot;
using EFEM.MaterialTracking;

namespace EFEM.Modules
{
    public class AtmRobotManager
    {
        #region <Constructors>
        public AtmRobotManager() { }
        #endregion </Constructors>

        #region <Fields>
        private static AtmRobotManager _instance = null;

        private readonly Dictionary<int, AtmRobotOperator> Robots = new Dictionary<int, AtmRobotOperator>();
        #endregion </Fields>

        #region <Properties>
        public static AtmRobotManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AtmRobotManager();
                }

                return _instance;
            }
        }
        public int Count
        {
            get
            {
                return Robots.Count;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Assign, Object>
        public void AssignRobots(AtmRobotOperator loadPort)
        {
            int index = Robots.Count;
            Robots.Add(index, loadPort);
            
            Robots[index].InitOperator();
        }

        public string GetRobotName(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return string.Empty;

            return Robots[robotIndex].Name;
        }
        //public bool GetRobot(int index, ref AtmRobotController robot)
        //{
        //    if (false == Robots.ContainsKey(index))
        //        return false;

        //    robot = Robots[index];
            
        //    return true;
        //}

        public RobotStateInformation GetStateInformation(int index)
        {
            if (false == Robots.ContainsKey(index))
                return null;

            return Robots[index].GetStateInformation();
        }
        public bool GetSubstrate(string locationName, RobotArmTypes armType, out Substrate s)
        {
            s = null;
            foreach (var item in Robots)
            {
                if (item.Value.Name.Equals(locationName))
                {
                    return item.Value.GetSubstrate(armType, out s);
                }
            }

            return false;
        }
        public void AttachBusySignalByDigitalInput(int index, int signalIndex, Func<int, bool> functionToReadInput)
        {
            if (false == Robots.ContainsKey(index))
                return;

            if (signalIndex >= 0)
            {
                Robots[index].AttachBusySignalByDigitalInput(signalIndex, functionToReadInput);
            }
        }
        public void AttachAlarmSignalByDigitalInput(int index, int signalIndex, Func<int, bool> functionToReadInput)
        {
            if (false == Robots.ContainsKey(index))
                return;

            if (signalIndex >= 0)
            {
                Robots[index].AttachAlarmSignalByDigitalInput(signalIndex, functionToReadInput);
            }
        }
        public void AttachServoSignalByDigitalInput(int index, int signalIndex, Func<int, bool> functionToReadInput)
        {
            if (false == Robots.ContainsKey(index))
                return;

            if (signalIndex >= 0)
            {
                Robots[index].AttachServoStatusByDigitalInput(signalIndex, functionToReadInput);
            }
        }
        //public void AssignSubstrate(int robotIndex, RobotArmTypes armType, Substrate substrate)
        //{
        //    if (false == Robots.ContainsKey(robotIndex))
        //        return;

        //    Robots[robotIndex].AssignSubstrate(armType, substrate);
        //}
        //public void AssignSubstrate(int robotIndex, Dictionary<RobotArmTypes, Substrate> substrates)
        //{
        //    if (false == Robots.ContainsKey(robotIndex))
        //        return;

        //    foreach (var item in substrates)
        //    {
        //        Robots[robotIndex].AssignSubstrate(item.Key, item.Value);
        //    }
        //}
        public AtmRobotLogger GetLogger(int index)
        {
            if (false == Robots.ContainsKey(index))
                return null;

            return Robots[index].Logger;
        }
        #endregion </Assign, Object>

        #region <Execute>
        public void Execute()
        {
            foreach (var item in Robots)
            {
                item.Value.Monitoring();
            }
        }
        #endregion </Execute>

        #region <Actions>
        public void InitAtmRobotAction(int index)
        {
            if (false == Robots.ContainsKey(index))
                return;

            Robots[index].InitAction();
        }
        public CommandResults InitializeAtmRobot(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.Initialize.ToString(), CommandResult.Error);

            return Robots[robotIndex].Initialize();
        }
        public CommandResults ApproachForPick(int robotIndex, RobotArmTypes armType, string locationId)//RobotArmTypes armType, string targetLocation, int slot)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.ApproachForPick.ToString(), CommandResult.Error);

            return Robots[robotIndex].ApproachForPick(armType, locationId);
        }
        public CommandResults MoveToPick(int robotIndex, RobotArmTypes armType, string locationId)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.MoveToPick.ToString(), CommandResult.Error);

            return Robots[robotIndex].MoveToPick(armType, locationId);
        }
        public CommandResults Pick(int robotIndex, RobotArmTypes armType, string locationId, bool testMode, string substrateKey = "")
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.Pick.ToString(), CommandResult.Error);

            return Robots[robotIndex].Pick(armType, locationId, testMode, substrateKey);
        }
        public CommandResults ApproachForPlace(int robotIndex, RobotArmTypes armType, string locationId)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.ApproachForPlace.ToString(), CommandResult.Error);

            return Robots[robotIndex].ApproachForPlace(armType, locationId);
        }
        public CommandResults MoveToPlace(int robotIndex, RobotArmTypes armType, string locationId)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.MoveToPlace.ToString(), CommandResult.Error);

            return Robots[robotIndex].MoveToPlace(armType, locationId);
        }
        public CommandResults Place(int robotIndex, RobotArmTypes armType, string locationId, bool testMode, string substrateKey = "")
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.Place.ToString(), CommandResult.Error);

            return Robots[robotIndex].Place(armType, locationId, testMode, substrateKey);
        }
        public CommandResults GetWaferPresence(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.GetWaferPresence.ToString(), CommandResult.Error);

            return Robots[robotIndex].GetWaferPresence();
        }
        public CommandResults GetHighSpeed(int robotIndex, RobotArmTypes armType, ref double speed)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.GetHighSpeed.ToString(), CommandResult.Error);

            return Robots[robotIndex].GetHighSpeed(armType, ref speed);
        }
        public CommandResults GetLowSpeed(int robotIndex, RobotArmTypes armType, ref double speed)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.GetLowSpeed.ToString(), CommandResult.Error);

            return Robots[robotIndex].GetLowSpeed(armType, ref speed);
        }
        public CommandResults SetHighSpeed(int robotIndex, RobotArmTypes armType, double speed)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.SetHighSpeed.ToString(), CommandResult.Error);

            return Robots[robotIndex].SetHighSpeed(armType, speed);
        }
        public CommandResults SetLowSpeed(int robotIndex, RobotArmTypes armType, double speed)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.SetLowSpeed.ToString(), CommandResult.Error);

            return Robots[robotIndex].SetLowSpeed(armType, speed);
        }
        public CommandResults Clear(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.ClearError.ToString(), CommandResult.Error);

            return Robots[robotIndex].Clear();
        }
        public CommandResults Grip(int robotIndex, RobotArmTypes armType)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.Grip.ToString(), CommandResult.Error);

            return Robots[robotIndex].Grip(armType);
        }
        public CommandResults Ungrip(int robotIndex, RobotArmTypes armType)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.Ungrip.ToString(), CommandResult.Error);

            return Robots[robotIndex].Ungrip(armType);
        }
        public CommandResults ServoOn(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.AmpOn.ToString(), CommandResult.Error);

            return Robots[robotIndex].ServoOn();
        }
        public CommandResults ServoOff(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return new CommandResults(RobotCommands.AmpOff.ToString(), CommandResult.Error);

            return Robots[robotIndex].ServoOff();
        }
        #endregion </Actions>

        #region <States>
        public bool IsConnectedWithController(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].IsConnected;
        }
        public bool IsRobotBusy(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].IsRobotBusy;
        }
        public bool IsRobotAlarm(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].IsRobotAlarm;
        }
        public bool GetServoStatus(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].IsServoOn;
        }
        public bool GetAvailableArm(int robotIndex, bool picking, ref List<RobotArmTypes> availableArms)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            // 2024.09.02. jhlim [MOD] Arm Role 구현하여 아래는 필요없음
            return Robots[robotIndex].GetAvailableArm(picking, ref availableArms);
            //if (picking != Robots[robotIndex].HasSubstrateAtArm(RobotArmTypes.UpperArm))
            //    availableArms.Add(RobotArmTypes.UpperArm);

            //if (picking != Robots[robotIndex].HasSubstrateAtArm(RobotArmTypes.LowerArm))
            //    availableArms.Add(RobotArmTypes.LowerArm);

            //return availableArms.Count > 0;
            // 2024.09.02. jhlim [END]
        }
        public bool GetWaferPresenceUpperArm(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].WaferPresenceUpperArm;
        }
        public bool GetWaferPresenceLowerArm(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].WaferPresenceLowerArm;
        }

        public bool GetInitializationState(int robotIndex)
        {
            if (false == Robots.ContainsKey(robotIndex))
                return false;

            return Robots[robotIndex].Initialized;
        }

        #endregion <States>

        #endregion </Methods>
    }
}
