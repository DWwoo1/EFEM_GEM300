using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.Modules;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;

using FrameOfSystem3.Views;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainSummary.PWA500BIN
{ 
    public partial class SummaryRobotInfo : UserControl
    {
        #region <Constructors>
        public SummaryRobotInfo(int robotIndex)
        {
            InitializeComponent();
            
            RobotIndex = robotIndex;
            _robotManager = AtmRobotManager.Instance;
            _robotStateInformation = new RobotStateInformation();

            ControlList = new ConcurrentDictionary<RobotArmTypes, Sys3Controls.Sys3Label>();
            ControlList.TryAdd(RobotArmTypes.UpperArm, lblUpperArmSubstrateInfo);
            ControlList.TryAdd(RobotArmTypes.LowerArm, lblLowerArmSubstrateInfo);

            this.Dock = DockStyle.Fill;
        }

        #endregion </Constructors>

        #region <Fields>
        private readonly int RobotIndex;
        private static AtmRobotManager _robotManager = null;
        private RobotStateInformation _robotStateInformation = null;
        private readonly ConcurrentDictionary<RobotArmTypes, Sys3Controls.Sys3Label> ControlList = null;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public void RefreshRobotInfo()
        {
            _robotStateInformation = _robotManager.GetStateInformation(RobotIndex);
            if (_robotStateInformation == null)
                return;

            string robotName = _robotManager.GetRobotName(RobotIndex);
            foreach (var item in ControlList)
            {
                if (_robotManager.GetSubstrate(robotName, item.Key, out var s))
                {
                    if (false == ControlList[item.Key].Text.Equals(s.Name))
                    {
                        ControlList[item.Key].Text = s.Name;
                    }
                }
                else
                {
                    ControlList[item.Key].Text = "";
                }
            }
        }
        #endregion </Methods>

    }
}
