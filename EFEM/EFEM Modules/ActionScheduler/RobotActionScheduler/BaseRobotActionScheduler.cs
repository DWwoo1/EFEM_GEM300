using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Modules;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;

namespace EFEM.ActionScheduler.RobotActionSchedulers
{
    #region <Enumerations>
    public enum RobotScheduleType
    {
        Selection,
        Pick,
        Place
    }
    #endregion <Enumerations>

    public abstract class BaseRobotActionScheduler
    {
        #region <Constructors>
        public BaseRobotActionScheduler(int index)
        {
            Index = index;

            _loadPortManager = LoadPortManager.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _robotManager = AtmRobotManager.Instance;
            _substrateManager = SubstrateManager.Instance;

            _workingInfo = new RobotWorkingInfo();
            ManualWorkingInfo = new ConcurrentDictionary<int, RobotWorkingInfo>();
            LoadPortInformations = new ConcurrentDictionary<int, LoadPortStateInformation>();
            for(int i = 0; i < _loadPortManager.Count; ++i)
            {
                LoadPortInformations.TryAdd(i, null);
            }
            //EFEM.Modules.ProcessModule.ProcessModule
        }

        #endregion </Constructors>

        #region <Fields>
        protected static LoadPortManager _loadPortManager = null;
        protected static ProcessModuleGroup _processGroup = null;
        protected static CarrierManagementServer _carrierServer = null;
        protected static AtmRobotManager _robotManager = null;
        protected static SubstrateManager _substrateManager = null;
        protected readonly ConcurrentDictionary<int, LoadPortStateInformation> LoadPortInformations = null;
        
        protected RobotWorkingInfo _workingInfo;
        protected readonly ConcurrentDictionary<int, RobotWorkingInfo> ManualWorkingInfo;
        protected Dictionary<RobotArmTypes, Substrate> _substrates = new Dictionary<RobotArmTypes, Substrate>();
        protected RobotStateInformation _robotStateInformation = null;
        protected int _seqNum;
        #endregion </Fields>

        #region <Properties>
        public int Index { get; private set; }
        #endregion </Properties>

        #region <Methods>
        #region <Scheduling>
        public virtual void InitScheduler()
        {
            _seqNum = 0;
        }

        public RobotScheduleType ExecuteSchedulers()
        {
            foreach (var key in LoadPortInformations.Keys)
            {
                LoadPortInformations[key] = _loadPortManager.GetLoadPortState(key);
            }

            _robotStateInformation = AtmRobotManager.Instance.GetStateInformation(Index);

            return DecideNextAction();
        }
        #endregion </Scheduling>

        #region <Working Info>
        public void InitWorkingInfo()
        {
            _workingInfo.Init();
            ManualWorkingInfo.Clear();
        }
        public void SetManualWorkingInformation(RobotArmTypes arm, string substrateKey, string locationId, ModuleType locationType, bool additional)
        {
            if (false == additional)
                ManualWorkingInfo.Clear();

            int key = ManualWorkingInfo.Count;
            var workInfo = new RobotWorkingInfo();

            workInfo.Init();
            workInfo.ActionArm = arm;
            workInfo.SubstrateKey = substrateKey;
            workInfo.LocationId = locationId;
            workInfo.LocationType = locationType;

            ManualWorkingInfo[key] = workInfo;
        }
        public void RemoveCurrentManualWorkingInfo()
        {
            if (ManualWorkingInfo.Count <= 0)
                return;

            int key = ManualWorkingInfo.First().Key;
            ManualWorkingInfo.TryRemove(key, out _);
        }
        public RobotWorkingInfo GetWorkingInformation(bool manualAction)
        {
            if (false == manualAction)
            {
                return _workingInfo;
            }
            else
            {
                if (ManualWorkingInfo.Count <= 0)
                    return null;

                return ManualWorkingInfo.First().Value;
            }
        }
        #endregion </Working Info>

        #region <Inherit>

        #region <Execute>
        protected abstract RobotScheduleType DecideNextAction();
        #endregion </Execute>

        #endregion </Inherit>

        #endregion </Methods>
    }
}
