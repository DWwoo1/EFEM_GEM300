using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using FrameOfSystem3.Task;
using Define.DefineEnumProject.Task;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace FrameOfSystem3.Views.EFEM_Simulator
{
    public partial class EFEMSimulator_AtmRobot : ParameterPanel
    {
        #region <Constructors>
        public EFEMSimulator_AtmRobot()
        {
            InitializeComponent();

            _taskOperator = Task.TaskOperator.GetInstance();
            _loadPortManager = LoadPortManager.Instance;
        }
        #endregion </Constructors>

        #region <Fields>
        private static Task.TaskOperator _taskOperator = null;
        private static LoadPortManager _loadPortManager = null;
        //private readonly int _selectedIndex = 0;       
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        
        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();
        }

        public override void CallFunctionByTimer()
        {
            UpdateUI();
            DisplayStates();

            base.CallFunctionByTimer();
        }
        #endregion </Override Methods>

        #region <Display>
        private void DisplayStates()
        {
        }
        private void UpdateUI()
        {
        }
        #endregion </Display>

        #region <UI Events>
        private void BtnPickWaferClicked(object sender, EventArgs e)
        {
        }

        private void BtnPlaceWaferClicked(object sender, EventArgs e)
        {
            EFEM.Defines.AtmRobot.RobotStateInformation state = AtmRobotManager.Instance.GetStateInformation(0);  
        }

        #endregion </UI Events>

        #endregion </Methods>
    }
}
