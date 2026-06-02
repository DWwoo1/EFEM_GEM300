using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.Modules;
using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking;
//using EFEM.MaterialTracking.Utilities;
using EFEM.Defines.ProcessModule;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainSummary.PWA500W
{
    public partial class SummaryProcessModuleInfo500W : UserControl
    {
        #region <Constructors>
        public SummaryProcessModuleInfo500W(int pmIndex)
        {
            InitializeComponent();

            ProcessModuleIndex = pmIndex;
            _processGroup = ProcessModuleGroup.Instance;
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;

            _communicationInfo = new NetworkInformation();

            CorePorts = new List<int>
            {
                (int)LoadPortType.Core_12 + 1,
                (int)LoadPortType.Core_8_1 + 1,
                (int)LoadPortType.Core_8_2 + 1
            };

            _substratesAtProcessModule = new List<Substrate>();
            _core_8_SubstratesAtProcessModule = new List<Substrate>();
            _core_12_SubstratesAtProcessModule = new List<Substrate>();
            _sortSubstratesAtProcessModule = new List<Substrate>();

            MappedServiceIndex = new Dictionary<int, int>();
            MappedClientIndex = new Dictionary<int, int>();

            //_temporaryLocations = new List<string>();
            _temporaryLoadingLocations = new List<string>();
            _temporaryUnloadingLocations = new List<string>();
            RequestedLocation = new Dictionary<string, int>();
            RequestedInputLocation = new Dictionary<string, int>();
            RequestedOutputLocation = new Dictionary<string, int>();

            InitGridControl();
            InitConnectionStatusGridViews();

            ProcessModuleName = _processGroup.GetProcessModuleName(ProcessModuleIndex);

            var style = gvCoreSubstrateList.DefaultCellStyle;
            style.SelectionBackColor = style.BackColor;
            style.SelectionForeColor = style.ForeColor;
            var style2 = gvSortSubstrateList.DefaultCellStyle;
            style2.SelectionBackColor = style.BackColor;
            style2.SelectionForeColor = style.ForeColor;

            this.Dock = DockStyle.Fill;
        }

        #endregion </Constructors>

        #region <Fields>
        private readonly int ProcessModuleIndex;
        private static ProcessModuleGroup _processGroup = null;
        private static CarrierManagementServer _carrierServer = null;
        private string _temporaryEquipmentState;
        private string _temporaryRecipeId;
        private static SubstrateManager _substrateManager = null;
        private readonly List<int> CorePorts = null;
        private List<Substrate> _substratesAtProcessModule = null;
        private List<Substrate> _core_8_SubstratesAtProcessModule = null;   // 2025.02.12 dwlim [MOD] Core 2가지 적용
        private List<Substrate> _core_12_SubstratesAtProcessModule = null;  // 2025.02.12 dwlim [MOD] Core 2가지 적용
        private List<Substrate> _sortSubstratesAtProcessModule = null;

        private const int ColumnSubstrateName = 0;

        private const int ColumnInputRequestEnabled = 0;
        private const int ColumnOutputRequestEnabled = 1;
        private const int ColumnRequestLocation = 2;

        private const int ColumnConnectionStatus = 0;
        private const int ColumnConnectionName = 1;

        private const string ColumnName_Input = "Input";
        private const string ColumnName_Output = "Output";

        private List<string> _temporaryLoadingLocations = null;
        private List<string> _temporaryUnloadingLocations = null;
        private readonly Dictionary<string, int> RequestedLocation = null;
        private readonly Dictionary<string, int> RequestedInputLocation = null;
        private readonly Dictionary<string, int> RequestedOutputLocation = null;
        //private Dictionary<int, int> _gvRequestLocation = null;

        private readonly Color EnabledColor = Color.LimeGreen;
        private readonly Color DisabledColor = Color.White;

        private NetworkInformation _communicationInfo;

        private readonly Dictionary<int, int> MappedServiceIndex = null;
        private readonly Dictionary<int, int> MappedClientIndex = null;
        private readonly string ProcessModuleName = string.Empty;

        private string _currentLotId = string.Empty;
        private int _LocationColumn;

        private const int RowIndexCore200mm = 0;
        private const int RowIndexCore300mm = 1;
        private const int RowIndexSort = 2;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        private void InitConnectionStatusGridViews()
        {
            if (_processGroup.GetCommunicationInfo(ProcessModuleIndex, ref _communicationInfo))
            {
                // Service
                gvServiceStatus.Rows.Clear();
                int index = 0;
                foreach (var item in _communicationInfo.ServiceInfo)
                {
                    gvServiceStatus.Rows.Add();
                    gvServiceStatus[ColumnConnectionName, index].Value = item.Value.Name;
                    MappedServiceIndex[item.Key] = index;
                    ++index;
                }

                gvClientStatus.Rows.Clear();
                index = 0;
                foreach (var item in _communicationInfo.ClientInfo)
                {
                    gvClientStatus.Rows.Add();
                    gvClientStatus[ColumnConnectionName, index].Value = item.Value.Name;
                    MappedClientIndex[item.Key] = index;
                    ++index;
                }
            }

        }
        private void InitGridControl()
        {
            string[] locations = _processGroup.GetEntrywayNames(ProcessModuleIndex);
            for (int i = 0; i < locations.Length; ++i)
            {
                string[] splitted = locations[i].Split('.');
                if (splitted.Length < 2)
                    continue;

                RequestedLocation[locations[i]] = i;
            }

            gvReceivedRequest.Rows.Clear();
            for (int i = 0; i < locations.Length / 2; ++i)
            {
                gvReceivedRequest.Rows.Add();
                if (i == RowIndexCore200mm)
                {
                    gvReceivedRequest[2, i].Value = "CORE (#8)";
                }
                else if (i == RowIndexCore300mm)
                {
                    gvReceivedRequest[2, i].Value = "CORE (#12)";
                }
                else if (i == RowIndexSort)
                {
                    gvReceivedRequest[2, i].Value = "BIN";
                }
            }
        }
        private void UpdateSortGridView()
        {
            bool isCleared = false;
            if (gvSortSubstrateList.Rows.Count != _sortSubstratesAtProcessModule.Count)
            {
                gvSortSubstrateList.Rows.Clear();
                isCleared = true;
            }

            for (int i = 0; i < _sortSubstratesAtProcessModule.Count; ++i)
            {
                if (isCleared || gvSortSubstrateList[ColumnSubstrateName, i].Value.ToString() != _sortSubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvSortSubstrateList.Rows.Add();
                        gvSortSubstrateList[ColumnSubstrateName, i].Value = string.Empty;
                    }
                }

                if (gvSortSubstrateList[ColumnSubstrateName, i] == null)
                    continue;

                var cell = gvSortSubstrateList[ColumnSubstrateName, i];
                var substrate = _sortSubstratesAtProcessModule[i];
                UpdateCellColor(substrate, ref cell);
            }
        }
        private void UpdateCoreGridView()
        {
            int coreSubstratesAtProcessModuleCount = _core_8_SubstratesAtProcessModule.Count + _core_12_SubstratesAtProcessModule.Count;
            int offset = _core_8_SubstratesAtProcessModule.Count;
            int gvIndexCount = gvCoreSubstrateList.Rows.Count;
            bool isCleared = false;

            if (gvIndexCount != coreSubstratesAtProcessModuleCount)
            {
                gvCoreSubstrateList.Rows.Clear();
                isCleared = true;
                gvIndexCount = 0;
            }

            for (int i = 0; i < _core_8_SubstratesAtProcessModule.Count; ++i)
            {
                int index = i;
                if (isCleared || gvCoreSubstrateList[ColumnSubstrateName, index].Value.ToString() != _core_8_SubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvCoreSubstrateList.Rows.Add();
                        gvCoreSubstrateList[ColumnSubstrateName, index].Value = string.Empty;
                        gvIndexCount++;
                    }
                }

                if (gvCoreSubstrateList[ColumnSubstrateName, index] == null)
                    continue;

                var cell = gvCoreSubstrateList[ColumnSubstrateName, index];
                var substrate = _core_8_SubstratesAtProcessModule[i];
                UpdateCellColor(substrate, ref cell);
            }
            for (int i = 0; i < _core_12_SubstratesAtProcessModule.Count; ++i)
            {
                int index = i + offset;
                if (isCleared || gvCoreSubstrateList[ColumnSubstrateName, index].Value.ToString() != _core_12_SubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvCoreSubstrateList.Rows.Add();
                        gvCoreSubstrateList[ColumnSubstrateName, index].Value = string.Empty;
                        gvIndexCount++;
                    }
                }

                if (gvCoreSubstrateList[ColumnSubstrateName, index] == null)
                    continue;

                var cell = gvCoreSubstrateList[ColumnSubstrateName, index];
                var substrate = _core_12_SubstratesAtProcessModule[i];
                UpdateCellColor(substrate, ref cell);
            }

            #region original
            //bool isCleared = false;
            //int gvRowsMaximumCount = 2;

            //int coreSubstratesAtProcessModuleCount = _core_8_SubstratesAtProcessModule.Count + _core_12_SubstratesAtProcessModule.Count;
            //if (gvCoreSubstrateList.Rows.Count > gvRowsMaximumCount ||
            //    gvCoreSubstrateList.Rows.Count != coreSubstratesAtProcessModuleCount)
            //{
            //    gvCoreSubstrateList.Rows.Clear();
            //    isCleared = true;
            //}
            //if (isCleared)
            //{
            //    for (int i = 0; i < coreSubstratesAtProcessModuleCount; i++)
            //    {
            //        gvCoreSubstrateList.Rows.Add();
            //    }
            //}
            //if (0 != _core_8_SubstratesAtProcessModule.Count)
            //{
            //    for (int i = 0; i < gvRowsMaximumCount; i++)
            //    {
            //        if (isCleared || gvCoreSubstrateList[0, i].Value.ToString() != _core_8_SubstratesAtProcessModule[i].Name)
            //        {
            //            gvCoreSubstrateList[ColumnSubstrateName, i].Value = _core_8_SubstratesAtProcessModule[i].Name;
            //        }
            //    }
            //}
            //if (0 != _core_12_SubstratesAtProcessModule.Count)
            //{
            //    for (int i = 0; i < gvRowsMaximumCount; i++)
            //    {
            //        if (isCleared || gvCoreSubstrateList[0, i].Value.ToString() != _core_12_SubstratesAtProcessModule[i].Name)
            //        {
            //            gvCoreSubstrateList[ColumnSubstrateName, i].Value = _core_12_SubstratesAtProcessModule[i].Name;
            //        }
            //    }
            //}
            #endregion original
        }
        private void UpdateCellColor(Substrate substrate, ref DataGridViewCell cell)
        {
            if (cell.Value == null)
                return;
            if (false == string.Equals(substrate.Name, cell.Value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                cell.Value = substrate.Name;
            }
            var backColor = SubstrateMapper.GetColorBySubstrateStatus(substrate.TransportStatus, substrate.ProcessingStatus);
            if (cell.Style.BackColor != backColor)
            {
                cell.Style.BackColor = backColor;

                Color foreColor;
                if (cell.Style.BackColor == Color.Silver ||
                cell.Style.BackColor == Color.White)
                {
                    foreColor = Color.Black;
                }
                else
                {
                    foreColor = Color.White;
                }

                if (cell.Style.ForeColor != foreColor)
                {
                    cell.Style.ForeColor = foreColor;
                }


                if (cell.Style.SelectionBackColor != cell.Style.BackColor)
                {
                    cell.Style.SelectionBackColor = cell.Style.BackColor;
                }
                if (cell.Style.SelectionForeColor != cell.Style.ForeColor)
                {
                    cell.Style.SelectionForeColor = cell.Style.ForeColor;
                }
            }
            //cell.Value = substrate.Name;
            //cell.Style.BackColor = SubstrateMapper.GetColorBySubstrateStatus(substrate.TransportStatus, substrate.ProcessingStatus);
            //if (cell.Style.BackColor == Color.Silver ||
            //    cell.Style.BackColor == Color.White)
            //{
            //    cell.Style.ForeColor = Color.Black;
            //}
            //else
            //{
            //    cell.Style.ForeColor = Color.White;
            //}

            //cell.Style.SelectionBackColor = cell.Style.BackColor;
            //cell.Style.SelectionForeColor = cell.Style.ForeColor;
        }
        private void UpdateRequestedCellColor(int columnIndex, int cellIndex, bool enabled)
        {           
            if (gvReceivedRequest.Rows.Count <= cellIndex)
                return;

            if(enabled)
            {
                gvReceivedRequest.Rows[cellIndex].Cells[columnIndex].Style.BackColor = EnabledColor;
                gvReceivedRequest.Rows[cellIndex].Cells[columnIndex].Style.SelectionBackColor = EnabledColor;
            }
            else
            {
                gvReceivedRequest.Rows[cellIndex].Cells[columnIndex].Style.BackColor = DisabledColor;
                gvReceivedRequest.Rows[cellIndex].Cells[columnIndex].Style.SelectionBackColor = DisabledColor;
            }
        }
        private void UpdateServiceStatus()
        {
            if (_processGroup.GetCommunicationInfo(ProcessModuleIndex, ref _communicationInfo))
            {
                foreach (var item in _communicationInfo.ServiceInfo)
                {
                    int row = MappedServiceIndex[item.Key];
                    if (row < gvClientStatus.Rows.Count)
                    {
                        if (item.Value.ConnectionStatus)
                        {
                            gvServiceStatus.Rows[row].Cells[ColumnConnectionStatus].Style.BackColor = EnabledColor;
                            gvServiceStatus.Rows[row].Cells[ColumnConnectionStatus].Style.SelectionBackColor = EnabledColor;
                        }
                        else
                        {
                            gvServiceStatus.Rows[row].Cells[ColumnConnectionStatus].Style.BackColor = DisabledColor;
                            gvServiceStatus.Rows[row].Cells[ColumnConnectionStatus].Style.SelectionBackColor = DisabledColor;
                        }
                    }
                }

                foreach (var item in _communicationInfo.ClientInfo)
                {
                    int row = MappedClientIndex[item.Key];
                    if (row < gvClientStatus.Rows.Count)
                    {
                        // gvServiceStatus[ColumnStatus, item.Key];

                        if (item.Value.ConnectionStatus)
                        {
                            gvClientStatus.Rows[row].Cells[ColumnConnectionStatus].Style.BackColor = EnabledColor;
                            gvClientStatus.Rows[row].Cells[ColumnConnectionStatus].Style.SelectionBackColor = EnabledColor;
                        }
                        else
                        {
                            gvClientStatus.Rows[row].Cells[ColumnConnectionStatus].Style.BackColor = DisabledColor;
                            gvClientStatus.Rows[row].Cells[ColumnConnectionStatus].Style.SelectionBackColor = DisabledColor;
                        }
                    }
                }
            }
        }
        public void RefreshSubstrateList()
        {
            if (_substrateManager.GetSubstratesAtProcessModule(ProcessModuleName, ref _substratesAtProcessModule))
            {
                _core_8_SubstratesAtProcessModule.Clear();
                _core_12_SubstratesAtProcessModule.Clear();
                _sortSubstratesAtProcessModule.Clear();
                for (int i = 0; i < _substratesAtProcessModule.Count; ++i)
                {
                    string subType = _substratesAtProcessModule[i].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                    if (false == Enum.TryParse(subType, out SubstrateType convertedSubType))
                        continue;

                    string subSize = _substratesAtProcessModule[i].GetAttribute(PWA500SubstrateAttributes.SubstrateSize);
                    if (false == Enum.TryParse(subSize, out SubstrateSize convertedSubSize))
                        continue;

                    switch (convertedSubType)
                    {
                        case SubstrateType.Core:
                            if (convertedSubSize == SubstrateSize.Inch_8)
                            {
                                _core_8_SubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                            }
                            else
                            {
                                _core_12_SubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                            }
                            break;

                        default:
                            {
                                _sortSubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                            }
                            break;
                    }
                }

                UpdateCoreGridView();
                UpdateSortGridView();
                UpdateServiceStatus();
                //for (int i = 0; i < _temporaryList.Count; ++i)
                //{
                //    if (needRedraw || Substrates[i].Name != _temporaryList[i].Name)
                //    {
                //        if (needRedraw)
                //        {
                //            Substrates.Add(_temporaryList[i]);
                //            gvSortSubstrateList.Rows.Add();
                //            gvSortSubstrateList[ColumnSubstrateIndex, i].Value = (i + 1).ToString();
                //        }
                //        else
                //        {
                //            Substrates[i] = _temporaryList[i];
                //        }

                //        gvSortSubstrateList[ColumnSubstrateName, i].Value = _temporaryList[i].Name;
                //    }
                //}
            }
        }
        public void RefreshModuleInfo()
        {
            #region <Status>
            _temporaryEquipmentState = _processGroup.GetEquipmentState(ProcessModuleIndex).ToString();
            if (false == lblEquipmentState.Text.Equals(_temporaryEquipmentState))
            {
                lblEquipmentState.Text = _temporaryEquipmentState;
            }

            _temporaryRecipeId = _processGroup.GetRecipeId(ProcessModuleIndex);
            if (false == lblRecipeId.Text.Equals(_temporaryRecipeId))
            {
                lblRecipeId.Text = _temporaryRecipeId;
            }

            _currentLotId = string.Empty;
            for (int i = 0; i < CorePorts.Count; ++i)
            {
                if (false == _carrierServer.HasCarrier(CorePorts[i]))
                    continue;

                if (_carrierServer.GetCarrierAccessingStatus(CorePorts[i]).Equals(Defines.LoadPort.CarrierAccessStates.InAccessed))
                {
                    _currentLotId = _carrierServer.GetCarrierLotId(CorePorts[i]);
                    break;
                }
            }
            lblLotId.Text = _currentLotId;
            #endregion </Status>

            #region <Requests>
            _temporaryLoadingLocations.Clear();
            _temporaryUnloadingLocations.Clear();
            _processGroup.IsLoadingRequested(ProcessModuleIndex, ref _temporaryLoadingLocations);
            _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref _temporaryUnloadingLocations);

            foreach (var item in RequestedLocation)
            {
                bool hasRequest = (_temporaryLoadingLocations.Contains(item.Key)
                    || _temporaryUnloadingLocations.Contains(item.Key));

                int gvColumnIndex = item.Key.Contains(ColumnName_Input) ? ColumnInputRequestEnabled :
                                    item.Key.Contains(ColumnName_Output) ? ColumnOutputRequestEnabled : -1;
                int gvRowIndex = item.Key.Contains(Constants.Core_8_Name) ? RowIndexCore200mm :
                                item.Key.Contains(Constants.Core_12_Name) ? RowIndexCore300mm :
                                item.Key.Contains(Constants.Sort_12_Name) ? RowIndexSort : -1;

                if (gvColumnIndex == -1 || gvRowIndex == -1)
                    continue;

                UpdateRequestedCellColor(gvColumnIndex, gvRowIndex, hasRequest);
            }
            #endregion </Requests>
        }
        #endregion </Methods>
    }
}
