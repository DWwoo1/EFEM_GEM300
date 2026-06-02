using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using EquipmentState_;

using FrameOfSystem3.Views;
using FrameOfSystem3.Views.Functional;
using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.Scenario;

using EFEM.Modules;
using EFEM.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500W
{
    public partial class SubViewOperationHistoryLotHistory500W : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationHistoryLotHistory500W()
        {
            InitializeComponent();

            _loadPortManager = LoadPortManager.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;
            _lotHistoryLog = LotHistoryLog.Instance;
            _selectionList = Form_SelectionList.GetInstance();
            _formSummary = FormProductionSummary.Instance;

            UserControlSelectedHistory = new MainDisplaySubPanelLotHistoryDisplayer(false)
            {
                Dock = DockStyle.Fill
            };
            tableLayoutPanel1.Controls.Add(UserControlSelectedHistory, 1, 0);

            _substrateType = SubType.Core;
            LotList = new List<string>();

            SubstrateTypes = new Dictionary<int, string>();
            var subTypes = Enum.GetNames(typeof(SubType));
            for(int i = 0; i < subTypes.Length; ++i)
            {
                SubstrateTypes[i] = subTypes[i].ToString();
            }
            
            Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Type>
        enum SubType
        {
            Core,
            Bin
        }
        #endregion </Type>

        #region <Fields>
        private static ProcessModuleGroup _processGroup = null;
        private static SubstrateManager _substrateManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static LoadPortManager _loadPortManager = null;
        private static FunctionsForPWA500W_NRD _functionsForPWA500 = null;
        private static LotHistoryLog _lotHistoryLog = null;
        private static Form_SelectionList _selectionList = null;
        private static FormProductionSummary _formSummary = null;

        #region <History Variables>
        private SubType _substrateType;
        private readonly List<string> LotList = null;

        private DateTime _selectedDate;
        private string _selectedLotId = string.Empty;
        private string _selectedCarrierId = string.Empty;

        private readonly Dictionary<int, string> SubstrateTypes = null;
        #endregion </History Variables>

        #region <User Control>
        private readonly MainDisplaySubPanelLotHistoryDisplayer UserControlSelectedHistory = null;
        #endregion </User Control>

        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                calander.MaxDate = DateTime.Today;
                calander.SelectionStart = DateTime.Today;
                calander.SelectionEnd = DateTime.Today;
            });

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            lblCarrierName.Text = _selectedCarrierId;
            lblSelectedSubstrateType.Text = _substrateType.ToString();

            if (LotList == null ||
                LotList.Count == 0)
                btnApply.Enabled = false;
            else
                btnApply.Enabled = true;

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void CalanderDateChanged(object sender, DateRangeEventArgs e)
        {
            _selectedDate = e.Start;

            UpdateLotList(_selectedDate);

            BeginInvoke((Action)(() =>
            {
                UpdateGridView();

            }));
        }
        private void CalanderSelected(object sender, DateRangeEventArgs e)
        {

        }
        private void LblClicked(object sender, EventArgs e)
        {
            if (_selectionList.CreateForm("Select Type", SubstrateTypes.Values.ToArray(), SubstrateTypes.Keys.ToArray(), (int)_substrateType))
            {
                string selectedType = string.Empty;
                _selectionList.GetResult(ref selectedType);
                Enum.TryParse(selectedType, out _substrateType);
            }
        }
        private void BtnClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnApply))
            {
                if (false == string.IsNullOrEmpty(_selectedLotId))
                {
                    bool isCore = _substrateType.Equals(SubType.Core) ? true : false;
                    var path = _lotHistoryLog.GetBackupHistoryPath(_selectedDate, isCore);
                    var filePath = Path.Combine(path, _selectedLotId);
                    if (false == Directory.Exists(filePath))
                        return;

                    var files = Directory.GetFiles(filePath, "*.log");
                    if (files == null || files.Length == 0)
                        return;

                    UserControlSelectedHistory.DisplayHistory(files[0]);
                    _selectedCarrierId = Path.GetFileNameWithoutExtension(files[0]);
                }
            }
            else if (sender.Equals(btnViewSummary))
            {
                bool isCore = _substrateType.Equals(SubType.Core) ? true : false;
                var path = _lotHistoryLog.GetBackupHistoryPath(_selectedDate, isCore);
                _formSummary.ShowMessage(isCore, _selectedDate, path);
            }
        }

        private void GvCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row >= 0)
            {
                _selectedLotId = gvLotList[0, row].Value.ToString();
            }
            else
            {
                _selectedLotId = string.Empty;
            }
        }
        #endregion </UI Events>

        #region <Internals>
        private void UpdateLotList(DateTime time)
        {
            bool isCore = _substrateType.Equals(SubType.Core) ? true : false;

            LotList.Clear();
            var path = _lotHistoryLog.GetBackupHistoryPath(time, isCore);
            if (false == Directory.Exists(path))
            {                
                return;
            }

            var directories = Directory.GetDirectories(path);
            for (int i = 0; directories != null && i < directories.Length; ++i)
            {
                var relative = GetRelativePath(path, directories[i]);
                LotList.Add(relative);
            }
        }
        private void UpdateGridView()
        {
            gvLotList.Rows.Clear();
            for (int i = 0; i < LotList.Count; ++i)
            {
                gvLotList.Rows.Add();
                gvLotList[0, i].Value = LotList[i];
            }

            gvLotList.ClearSelection();
        }

        private string GetRelativePath(string basePath, string targetPath)
        {
            DirectoryInfo baseDir = new DirectoryInfo(Path.GetFullPath(basePath));
            DirectoryInfo targetDir = new DirectoryInfo(Path.GetFullPath(targetPath));

            Uri baseUri = new Uri(baseDir.FullName + Path.DirectorySeparatorChar);
            Uri targetUri = new Uri(targetDir.FullName);

            Uri relativeUri = baseUri.MakeRelativeUri(targetUri);
            return relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar);
        }
        #endregion </Internals>

        #endregion </Methods>
    }
}