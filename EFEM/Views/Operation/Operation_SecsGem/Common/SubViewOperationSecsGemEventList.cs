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
    public partial class SubViewOperationSecsGemEventList : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationSecsGemEventList()
        {
            InitializeComponent();

            _scenarioOperator = ScenarioOperator.Instance;
            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private static ScenarioOperator _scenarioOperator = null;

        private Dictionary<long, List<StatusVariable>> _reportList = new Dictionary<long, List<StatusVariable>>();
        private Dictionary<string, StatusVariable> _statusVariableList = new Dictionary<string, StatusVariable>();
        private Dictionary<string, CollectionEvent> _collectionEventList = new Dictionary<string, CollectionEvent>();
        private string _selectedCollectionEventName;        
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        protected override void ProcessWhenActivation()
        {
            InitializeCollectionEventGrid();
            UpdateVariableGridView();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        public override void CallFunctionByTimer()
        {
        }
        private void InitializeCollectionEventGrid()
        {            
            gvCollectionEvent.Rows.Clear();
            _reportList = _scenarioOperator.GetReportList();
            if (_reportList == null)
                return;

            _collectionEventList = _scenarioOperator.GetCollectionEventList();
            if (_collectionEventList == null)
                return;

            _statusVariableList = _scenarioOperator.GetStatusVariableList();
            if (_statusVariableList == null)
                return;

            int index = 0;
            foreach (var item in _collectionEventList)
            {
                gvCollectionEvent.Rows.Add();
                gvCollectionEvent[0, index].Value = item.Value.Id;
                gvCollectionEvent[1, index].Value = item.Key;
                ++index;
            }

            _selectedCollectionEventName = string.Empty;
            gvCollectionEvent.ClearSelection();
        }
        private void UpdateVariableGridView()
        {
            gvStatusVariableList.Rows.Clear();

            int index = 0;
            if (string.IsNullOrEmpty(_selectedCollectionEventName))
            {
                foreach (var item in _statusVariableList)
                {
                    gvStatusVariableList.Rows.Add();
                    UpdateVariableInfo(index, item.Value);
                    ++index;
                }
            }
            else
            {
                if (false == _collectionEventList.TryGetValue(_selectedCollectionEventName, out CollectionEvent eventInfo))
                    return;

                foreach (var item in eventInfo.Variables)
                {
                    gvStatusVariableList.Rows.Add();
                    UpdateVariableInfo(index, item.Value);
                    ++index;                    
                }
            }
            gvStatusVariableList.ClearSelection();
        }
        private void UpdateVariableInfo(int index, StatusVariable variableInfo)
        {
            gvStatusVariableList[0, index].Value = variableInfo.Id;
            gvStatusVariableList[1, index].Value = variableInfo.Name;
        }
        private void GvCollectionEventSelectionChanged(object sender, EventArgs e)
        {
            var selected = gvCollectionEvent.SelectedRows;
            if (selected == null || selected.Count <= 0)
                return;

            if (gvCollectionEvent[1, selected[0].Index].Value == null)
                return;

            _selectedCollectionEventName = gvCollectionEvent[1, selected[0].Index].Value.ToString();
            UpdateVariableGridView();
        }       
        private void BtnClearSelectionClicked(object sender, EventArgs e)
        {
            _selectedCollectionEventName = string.Empty;
            UpdateVariableGridView();
            gvCollectionEvent.ClearSelection();
            gvStatusVariableList.ClearSelection();
        }
        #endregion </Methods>
    }
}