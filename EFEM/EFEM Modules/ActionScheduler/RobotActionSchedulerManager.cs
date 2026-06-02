using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EquipmentState_;

using FrameOfSystem3.DynamicLink_;
using EFEM.Defines.Common;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;
using EFEM.ActionScheduler.RobotActionSchedulers;

namespace EFEM.ActionScheduler
{
    public class RobotActionSchedulerManager
    {
        #region <Constructors>        
        #endregion </Constructors>

        #region <Fields>
        private static RobotActionSchedulerManager _instance = null;

        private readonly Dictionary<int, BaseRobotActionScheduler> RobotSchedulers = new Dictionary<int, BaseRobotActionScheduler>();
        #endregion </Fields>

        #region <Properties>
        public static RobotActionSchedulerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RobotActionSchedulerManager();
                }

                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>
        
        #region <Manual Work Info>
        public void InitManualWorkingInformation(int robotIndex)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return;

            RobotSchedulers[robotIndex].InitWorkingInfo();
        }
        public void SetManualWorkingInformation(int robotIndex, RobotArmTypes arm, string substrateKey, string locationId, ModuleType locationType, bool additional)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return;

            RobotSchedulers[robotIndex].SetManualWorkingInformation(arm, substrateKey, locationId, locationType, additional);
        }
        public void RemoveCurrentManualWorkingInfo(int robotIndex)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return;

            RobotSchedulers[robotIndex].RemoveCurrentManualWorkingInfo();
        }
        public bool GetManualWorkingInformation(int robotIndex, ref RobotWorkingInfo workInfo)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return false;

            workInfo = RobotSchedulers[robotIndex].GetWorkingInformation(true);
            return (workInfo != null);
        }
        #endregion </Manual Work Info>

        #region <Scheduler>
        public bool CreateScheduler(int robotIndex, BaseRobotActionScheduler scheduler)
        {
            RobotSchedulers[robotIndex] = scheduler;
            return true;
        }
        public bool GetWorkingInformation(int robotIndex, ref RobotWorkingInfo workInfo)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return false;

            workInfo = RobotSchedulers[robotIndex].GetWorkingInformation(false);
            return true;
        }
        public void InitSchedulers(int robotIndex)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return;

            RobotSchedulers[robotIndex].InitScheduler();
        }
        public RobotScheduleType ExecuteSchedulers(int robotIndex)
        {
            if (RobotSchedulers == null || false == RobotSchedulers.ContainsKey(robotIndex))
                return RobotScheduleType.Selection;

            return RobotSchedulers[robotIndex].ExecuteSchedulers();
        }
        public EN_PORT_STATUS ConvertPortStatusFromRobotPortType(RobotScheduleType portType)
        {
            switch (portType)
            {
                case RobotScheduleType.Selection:
                    return EN_PORT_STATUS.SELECTION;

                case RobotScheduleType.Pick:
                    return EN_PORT_STATUS.EMPTY;

                case RobotScheduleType.Place:
                    return EN_PORT_STATUS.EXIST;

                default:
                    return EN_PORT_STATUS.UNABLE;
            }
        }
        #endregion </Scheduler>

        #endregion </Methods>

    }
}
