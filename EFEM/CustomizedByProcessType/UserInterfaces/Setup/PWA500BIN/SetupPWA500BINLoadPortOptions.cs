using System;
using System.Collections.Generic;
using System.Windows.Forms;

using FrameOfSystem3.Views;

namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN
{
    public partial class SetupPWA500BINLoadPortOptions : ParameterPanel
    {
        #region <Constructors>
        public SetupPWA500BINLoadPortOptions(int lpIndex)
        {
            //string toggleName = string.Format("UseLoadPort{0}", lpIndex + 1);
            InitializeComponent(/*toggleName*/);

            LoadPortIndex = lpIndex;           
            gbTitle.Text = string.Format("LoadPort{0}", lpIndex + 1);
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            var paramUsable = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.UseLoadPort1 + lpIndex;
            toggleUseLoadPort.ParameterName = paramUsable.ToString(); //string.Format("UseLoadPort{0}", lpIndex + 1);

            var paramType = FrameOfSystem3.Recipe.PARAM_EQUIPMENT.LoadPortType1 + lpIndex;
            lblWaferType.ParameterName = paramType.ToString();// string.Format("LoadPortType{0}", lpIndex + 1);

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
