using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using FrameOfSystem3.Component;
using FrameOfSystem3.Functional;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using Define.DefineEnumProject.Map;
using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Task.Global;
using Define.DefineConstant;

namespace FrameOfSystem3.Views.Operation
{
    public partial class SubViewOperationSecsGemCommon : UserControlForMainView.CustomView
    {
        #region constructor
        public SubViewOperationSecsGemCommon()
        {
            InitializeComponent();

            _selectionList = Functional.Form_SelectionList.GetInstance();
            _messageBox = Functional.Form_MessageBox.GetInstance();
            _postOffice = PostOffice.GetInstance();
            _keyboard = Functional.Form_Keyboard.GetInstance();

            _scenarioOperator = ScenarioOperator.Instance;
            _scenarioList = new List<string>();

            this.Dock = DockStyle.Fill;
        }
        #endregion /constructor

        #region field
        Functional.Form_MessageBox _messageBox = null;
        Functional.Form_SelectionList _selectionList = null;
        Functional.Form_Keyboard _keyboard = null;
        PostOffice _postOffice = null;
        private string _selectScenario;

        private readonly Dictionary<string, string> DataToSend = new Dictionary<string, string>();

        private List<string> _scenarioList = null;

        private static ScenarioOperator _scenarioOperator = null;
        #endregion /field

        #region inherit
        protected override void ProcessWhenActivation()
        {
            _scenarioList = _scenarioOperator.GetScenarioList();
            if (string.IsNullOrEmpty(_selectScenario) && _scenarioList != null && _scenarioList.Count > 0)
            {
                _selectScenario = _scenarioList[0];
            }
            lblScenarioToExecute.Text = _selectScenario;
            tglUseSecsgem.Active = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON
                , FrameOfSystem3.Recipe.PARAM_COMMON.UseSecsGem.ToString(), false);

            lblScenarioToExecute.Enabled = _scenarioList != null;
            btnExecuteScenario.Enabled = _scenarioList != null;
            sys3button1.Enabled = _scenarioList != null;
            sys3button2.Enabled = _scenarioList != null;

            UpdateDataToSend();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        public override void CallFunctionByTimer()
        {
        }
        #endregion /inherit

        private void Click_ScenarioRun(object sender, EventArgs e)
        {
            if (false == _messageBox.ShowMessage(string.Format("{0} scenario run?", _selectScenario)))
                return;

            DataToSend.Clear();
            for (int row = 0; row < gvMessageToSend.Rows.Count; ++row)
            {
                string key = gvMessageToSend[0, row].Value.ToString();
                string value = string.Empty;
                if (gvMessageToSend[1, row].Value != null)
                    value = gvMessageToSend[1, row].Value.ToString();
                DataToSend[key] = value;
            }

            //Enum convertedScenarioName = _scenarioOperator.ConvertScenarioByString(_selectScenario);
            //if (convertedScenarioName == null)
            //    return;
            if (false == Enum.TryParse(_selectScenario, out EN_SCENARIO scenario))
                return;

            _postOffice.SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.ScenarioCirculator
                , Define.DefineEnumProject.Mail.EN_MAIL.SendScenario
                , scenario
                , DataToSend);
        }
        private void Click_ScenarioSelect(object sender, EventArgs e)
        {
            if (_scenarioList == null)
                return;

            if (false == _selectionList.CreateForm("SCENARIO", _scenarioList.ToArray(), _selectScenario))
                return;

            _selectionList.GetResult(ref _selectScenario);

            lblScenarioToExecute.Text = _selectScenario.ToString();

            UpdateDataToSend();
            //UpdateSendDataDisplay();
        }

        private void Click_SendDataKey(object sender, EventArgs e)
        {
            if (false == _keyboard.CreateForm(lbl_SendDataKey.Text))
                return;

            string selectedKey = string.Empty;
            _selectionList.GetResult(ref selectedKey);

            lbl_SendDataKey.Text = selectedKey;
        }
        private void Click_SendDataAdd(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbl_SendDataKey.Text))
                return;

            DataToSend[lbl_SendDataKey.Text] = string.Empty;

            AddGridViewControl(lbl_SendDataKey.Text);
        }
        private void Click_SendDataClear(object sender, EventArgs e)
        {
            DataToSend.Clear();
            gvMessageToSend.Rows.Clear();
            //UpdateSendDataDisplay();
        }
        private void UpdateDataToSend()
        {
            if (false == Enum.TryParse(_selectScenario, out EN_SCENARIO scenario))
                return;

            var dataToSend = _scenarioOperator.GetScenarioParameterList(scenario);

            DataToSend.Clear();
            gvMessageToSend.Rows.Clear();

            if (dataToSend == null)
                return;

            for(int i = 0; i < dataToSend.Count; ++i)
            {
                DataToSend[dataToSend[i]] = string.Empty;

                gvMessageToSend.Rows.Add();
                gvMessageToSend[0, i].Value = dataToSend[i];
                gvMessageToSend[1, i].Value = string.Empty;
            }
        }
        private void AddGridViewControl(string key)
        {
            int row = gvMessageToSend.Rows.Count;
            gvMessageToSend.Rows.Add();
            gvMessageToSend[0, row].Value = key;
            gvMessageToSend[1, row].Value = string.Empty;
        }

        private void toggleUseSecsGemClicked(object sender, EventArgs e)
        {
            bool use = tglUseSecsgem.Active;

            FrameOfSystem3.Recipe.Recipe.GetInstance().SetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON
                , FrameOfSystem3.Recipe.PARAM_COMMON.UseSecsGem.ToString(), use.ToString());

            PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.ScenarioCirculator,
                Define.DefineEnumProject.Mail.EN_MAIL.ScenarioCurculatorUse, use);
        }
    }
}