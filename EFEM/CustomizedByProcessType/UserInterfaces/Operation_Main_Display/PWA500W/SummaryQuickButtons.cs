using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DigitalIO_;
using FrameOfSystem3.Recipe;
using FrameOfSystem3.Component;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using Define.DefineEnumProject.DigitalIO;

using FrameOfSystem3.Views;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainSummary.PWA500W
{
    public partial class SummaryQuickButtons : UserControl
    {
        #region <Constructors>
        public SummaryQuickButtons()
        {
            InitializeComponent();

            _digitalIO = DigitalIO.GetInstance();
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            OutputLabels = new Dictionary<int, Sys3Controls.Sys3LedLabelWithText>();

            MappingIO();

            lblUseCapacityLimitBin.UnitText = "EA";

            _controlInterface = new ControlInterface();
            _controlInterface.AssignControls(this.Controls);

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Enums>
        #endregion </Enums>

        #region <Fields>
        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private static DigitalIO _digitalIO = null;
        private readonly Dictionary<int, Sys3Controls.Sys3LedLabelWithText> OutputLabels = null;
        private ControlInterface _controlInterface = null;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Display Methods>
        public void ProcessWhenActivation()
        {
            //PanelInterface.ProcessWhenActivation();            
        }

        public void ProcessWhenDeactivation()
        {
            //PanelInterface.ProcessWhenDeactivation();
        }

        public void CallFunctionByTimer()
        {
            _controlInterface.RefreshValueParameter();

            foreach (var item in OutputLabels)
            {
                item.Value.Active = _digitalIO.ReadOutput(item.Key);
            }

            lblUseSecsGem.Active = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false);
            //lblUseMapHandlingOnly.Active = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGemWithCoreWaferMapHandlingOnly.ToString(), false);


            lblUseCapacityLimitBin.Active =
                _recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.UseCapacityLimitBin1.ToString(), false);

            lblUseRecipeDownload.Active = _recipe.GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseDownloadingRecipe.ToString(), true);
            //lblCapacityLimitRightStage.Text = _recipe.GetValue("LoadPort3", Define.DefineEnumProject.Task.LoadPort.PARAM_PROCESS.AVAILABLE_CARRIER_CAPACITY.ToString(), 0).ToString();
        }
        #endregion </Display Methods>

        #region <Initialize>
        private void MappingIO()
        {
            if (int.TryParse(lblDoorLock.Tag.ToString(), out int index))
            {
                OutputLabels[index] = lblDoorLock;
            }
        }
        #endregion </Initialize>

        #region <UI Events>
        #endregion </UI Events>

        #endregion </Methods>

        private void BtnQuickButtonClicked(object sender, EventArgs e)
        {
            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE) &&
                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
                return;

            if (!(sender is Sys3Controls.Sys3LedLabelWithText control))
                return;

            if (sender.Equals(lblDoorLock))
            {
                if (false == int.TryParse(control.Tag.ToString(), out int index))
                    return;

                _digitalIO.WriteOutput(index, !control.Active);
            }
            else if (sender.Equals(lblUseSecsGem))
            {
                bool value = !lblUseSecsGem.Active;
                _recipe.SetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), value.ToString());
                FrameOfSystem3.SECSGEM.ScenarioOperator.Instance.SetUse(value);
            }
            //else if (sender.Equals(lblUseMapHandlingOnly))
            //{
            //    bool value = !lblUseMapHandlingOnly.Active;
            //    _recipe.SetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGemWithCoreWaferMapHandlingOnly.ToString(), value.ToString());
            //}
            else if (sender.Equals(lblUseRecipeDownload))
            {
                if (!(sender is Sys3Controls.Sys3LedLabelWithText label))
                    return;

                string paramName = label.Tag.ToString();
                if (false == Enum.TryParse(paramName, out PARAM_EQUIPMENT param))
                    return;

                bool value = !label.Active;
                _recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, paramName, value.ToString());
            }
            else if (sender.Equals(lblUseCapacityLimitBin))
            {
                if (!(sender is Sys3Controls.Sys3LedLabelWithText label))
                    return;

                string paramName = label.Tag.ToString();
                if (false == Enum.TryParse(paramName, out PARAM_EQUIPMENT param))
                    return;

                bool value = !label.Active;
                _recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, paramName, value.ToString());
            }
        }
    }
}
