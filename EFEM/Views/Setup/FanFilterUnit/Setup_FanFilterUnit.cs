using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit;

namespace FrameOfSystem3.Views.Setup
{
	public partial class Setup_FanFilterUnit : UserControlForMainView.CustomView
	{
        #region <Constructors>
        public Setup_FanFilterUnit()
        {
            InitializeComponent();

            _fanFilterUnit = FanFilterUnitManager.Instance;

            InitGrid();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private static FanFilterUnitManager _fanFilterUnit = null;

        private readonly Functional.Form_Calculator m_InstanceOfCalculator = Functional.Form_Calculator.GetInstance();

        private readonly string MIN_OF_PARAM = "0";
        private readonly string MAX_OF_PARAM_SPEED = "2000";
        private readonly string MAX_OF_PARAM_PRESSURE = "1.2";

        private const int ColIndex = 0;
        private const int ColName = 1;
        private const int ColSettingSpeed = 2;
        private const int ColCurrentSpeed = 3;
        //private const int ColSettingPressure = 4;
        private const int ColCurrentPressure = 4;
        private const int ColStatus = 5;

        private UnitItem _temporaryItem = new UnitItem();
        //private readonly Dictionary<>
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region override
        protected override void ProcessWhenActivation()
        {
            //lbl_FFU_unit_count.Enabled = System.Diagnostics.Debugger.IsAttached;

            //int ffuUnitCount = _fanFilterUnit.Count;
            //Sys3Controls.Sys3Label[] labelTarget = { lblTarget_FFU_1, lblTarget_FFU_2, lblTarget_FFU_3 };
            //Sys3Controls.Sys3button[] buttonSet = { btnSetSpeed_1, btnSetSpeed_2, btnSetSpeed_3 };

            //for (int i = 0; i < ffuUnitCount; ++i)
            //{
            //    labelTarget[i].Enabled = (i < ffuUnitCount);
            //    buttonSet[i].Enabled = (i < ffuUnitCount);
            //}
            lbl_FFU_UnitCount.Text = _fanFilterUnit.Count.ToString();
            toggleUseFFU.Active = !_fanFilterUnit.Skipped;
            toggleDifferentialPressureMode.Active = _fanFilterUnit.UseDifferentialPressureMode;
            EnableControls();

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            UpdateGrid();
            UpdateAlarm();
            base.CallFunctionByTimer();
        }
        #endregion /override

        #region method
        private void InitGrid()
        {
            gvFanFilterUnitStatus.Rows.Clear();
            for (int i = 0; i < _fanFilterUnit.Count; ++i)
            {
                gvFanFilterUnitStatus.Rows.Add();
                gvFanFilterUnitStatus[ColIndex, i].Value = i + 1;
                gvFanFilterUnitStatus[ColName, i].Value = string.Format("FFU{0}", i + 1);
            }
        }
        private void UpdateGrid()
        {
            for (int i = 0; i < _fanFilterUnit.Count; ++i)
            {
                if (false == _fanFilterUnit.GetInformation(i, ref _temporaryItem))
                    continue;

                gvFanFilterUnitStatus[ColSettingSpeed, i].Value     = _temporaryItem.SettingSpeed;
                gvFanFilterUnitStatus[ColCurrentSpeed, i].Value     = _temporaryItem.CurrentSpeed;
                //gvFanFilterUnitStatus[ColSettingPressure, i].Value  = _temporaryItem.SettingDifferentialPressure;
                gvFanFilterUnitStatus[ColCurrentPressure, i].Value  = _temporaryItem.CurrentDifferentialPressure;
                gvFanFilterUnitStatus[ColStatus, i].Value           = _temporaryItem.Status;
            }
        }
        private void UpdateAlarm()
        {
            lbl_FFU_AlarmCode.Text = _temporaryItem.AlarmCode;
        }
        private void EnableControls()
        {
            bool enabled = !_fanFilterUnit.UseDifferentialPressureMode;
            btnSetSpeedAll.Enabled = enabled;
            btnStop.Enabled = enabled;
            lbl_FFU_Speed.Enabled = enabled;

            // 2025.01.08. by dwlim [ADD] 현재 모드변경은 구현 불가능하나, Pressure Set은 가능하여 만듦
            lbl_FFU_Pressure.Enabled = !enabled;
            btnSetPressure.Enabled = !enabled;
            // 2025.01.08. by dwlim [END]
        }
        #endregion /method

        #region ui event
        private void Click_FullSpeed(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3Label label))
                return;
            string strResult = string.Empty;

            if (label.Equals(lbl_FFU_Speed))
            {
                if (m_InstanceOfCalculator.CreateForm(lbl_FFU_Speed.Text, MIN_OF_PARAM, MAX_OF_PARAM_SPEED))
                {
                    m_InstanceOfCalculator.GetResult(ref strResult);
                    lbl_FFU_Speed.Text = strResult;
                }
            }
        }
        private void Click_Pressure(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3Label label))
                return;
            string strResult = string.Empty;

            if (label.Equals(lbl_FFU_Pressure))
            {
                if (m_InstanceOfCalculator.CreateForm(lbl_FFU_Pressure.Text, MIN_OF_PARAM, MAX_OF_PARAM_PRESSURE))
                {
                    m_InstanceOfCalculator.GetResult(ref strResult);
                    lbl_FFU_Pressure.Text = strResult;
                }
            }
        }
        private void Click_SetTarget(object sender, EventArgs e)
        {
            if (_fanFilterUnit.Skipped || _fanFilterUnit.UseDifferentialPressureMode)
                return;

            //int ffu = -1;
            //int mcuNo = 1;  // 1번만 사용

            //if (sender == btnSetSpeed_1) ffu = 0;
            //else if (sender == btnSetSpeed_2) ffu = 1;
            //else if (sender == btnSetSpeed_3) ffu = 2;
            //else return;

            //_fanFilterUnit.RequestNormalSpeed(mcuNo, ffu);
        }
        private void Click_SetTargetAll(object sender, EventArgs e)
        {
            if (_fanFilterUnit.Skipped || _fanFilterUnit.UseDifferentialPressureMode)
                return;

            double setSpeed = double.Parse(lbl_FFU_Speed.Text);

            _fanFilterUnit.SetSpeedAll(setSpeed);
        }

        private void Click_Stop(object sender, EventArgs e)
        {
            if (_fanFilterUnit.Skipped || _fanFilterUnit.UseDifferentialPressureMode)
                return;

            double getSpeed = 0;

            _fanFilterUnit.SetSpeedAll(getSpeed);
        }
        private void Click_SetPressure(object sender, EventArgs e)
        {
            if (_fanFilterUnit.Skipped || !(_fanFilterUnit.UseDifferentialPressureMode))
                return;

            double setPressure = double.Parse(lbl_FFU_Pressure.Text);

            _fanFilterUnit.SetPressure(setPressure);
        }
        #endregion /ui event

        #endregion </Methods>

        private void ToggleClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3ToggleButton toggle))
                return;

            if (toggle.Equals(toggleUseFFU))
            {
                _fanFilterUnit.Skipped = !toggle.Active;
            }
            if (toggle.Equals(toggleDifferentialPressureMode))
            {
                _fanFilterUnit.UseDifferentialPressureMode = toggle.Active;
                EnableControls();
            }
        }
    }
}