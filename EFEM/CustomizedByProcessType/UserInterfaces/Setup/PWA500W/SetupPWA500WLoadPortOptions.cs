using System;
using System.Collections.Generic;
using System.Windows.Forms;

using FrameOfSystem3.Views;

namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500W
{
    public partial class SetupPWA500WLoadPortOptions : ParameterPanel
    {
        #region <Constructors>
        public SetupPWA500WLoadPortOptions(int lpIndex)
        {
            //string toggleName = string.Format("UseLoadPort{0}", lpIndex + 1);
            InitializeComponent(/*toggleName*/);

            LoadPortIndex = lpIndex;           
            gbTitle.Text = string.Format("LoadPort{0}", lpIndex + 1);
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            var paramUsable = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.UseLoadPort1 + lpIndex;
            toggleUseLoadPort.ParameterName = paramUsable.ToString();

            var paramType = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + lpIndex;
            lblWaferType.ParameterName = paramType.ToString();

            var paramType2 = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortSize1 + lpIndex;
            lblWaferSize.ParameterName = paramType2.ToString();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>
        
        #region <Type>
        #endregion </Type>

        #region <Fields>
        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        private readonly int LoadPortIndex;
        #endregion </Fields>

        #region <Methods>
        
        #region <UI Event>
        private void ToggleClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3ToggleButton toggle))
                return;
        }

        private void BtnClicked(object sender, EventArgs e)
        {

        }
        #endregion </UI Event>

        #endregion </Methods>
    }
}
