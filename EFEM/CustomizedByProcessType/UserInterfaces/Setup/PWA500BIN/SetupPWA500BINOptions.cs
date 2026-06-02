using System;
using System.Collections.Generic;
using System.Windows.Forms;

using FrameOfSystem3.Views;

namespace EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN
{
    public partial class SetupPWA500BINOptions : ParameterPanel
    {
        #region <Constructors>
        public SetupPWA500BINOptions(string name)
        {
            //string toggleName = string.Format("UseLoadPort{0}", lpIndex + 1);
            InitializeComponent(/*toggleName*/);

            gbTitle.Text = name;
            _recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>
        
        #region <Type>
        #endregion </Type>

        #region <Fields>
        private static FrameOfSystem3.Recipe.Recipe _recipe = null;
        #endregion </Fields>

        #region <Methods>
        
        #region <UI Event>
        private void ToggleClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Controls.Sys3ToggleButton toggle))
                return;
        }
        #endregion </UI Event>

        #region <Override>
        #endregion </Override>

        #endregion </Methods>
    }
}
