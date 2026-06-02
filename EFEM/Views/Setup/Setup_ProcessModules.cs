using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumProject.Task;

using EFEM.Defines.ProcessModule;
using EFEM.Modules;
using EFEM.Modules.ProcessModule;
using FrameOfSystem3.Component;
using FrameOfSystem3.Work;

namespace FrameOfSystem3.Views.Setup
{
	public partial class Setup_ProcessModules : UserControlForMainView.CustomView
	{
        #region <Constructors>
        public Setup_ProcessModules()
        {
            InitializeComponent();

            _processGroup = ProcessModuleGroup.Instance;
            _appConfigManager = AppConfigManager.Instance;

            switch (_appConfigManager.ProcessType)
            {
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                    {
                        mainView = new EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500BIN.SetupProcessModulePWA500BIN();
                    }
                    break;
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                    {
                        mainView = new EFEM.CustomizedByProcessType.UserInterface.Setup.PWA500W.SetupProcessModulePWA500W();
                    }
                    break;
                default:
                    return;
            }

            pnMainView.Controls.Add(mainView);
        }
        #endregion </Constructors>

        #region <Fields>
        private Functional.Form_MessageBox _messageBox = Functional.Form_MessageBox.GetInstance();

        private static ProcessModuleGroup _processGroup = null;
        private static AppConfigManager _appConfigManager = null;

        private readonly UserControlForMainView.CustomView mainView = null;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        #region <Override Methods>
        public override void CallFunctionByTimer()
        {
            base.CallFunctionByTimer();

            if (mainView != null)
            {
                mainView.CallFunctionByTimer();
            }
        }
        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();

            if (mainView != null)
            {
                mainView.ActivateView();
            }
        }
        protected override void ProcessWhenDeactivation()
        {
            if (mainView != null)
            {
                mainView.DeactivateView();
            }

            base.ProcessWhenDeactivation();
        }
        #endregion </Override Methods>

        #endregion </Methods>
    }
}