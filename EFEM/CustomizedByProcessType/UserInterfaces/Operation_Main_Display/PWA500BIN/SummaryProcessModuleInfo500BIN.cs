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
using EFEM.Defines.ProcessModule;
//using EFEM.MaterialTracking.Utilities;
using EFEM.CustomizedByProcessType.PWA500BIN;
using EFEM.CustomizedByProcessType.PWA500Common;

using FrameOfSystem3.Views;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainSummary.PWA500BIN
{
    public partial class SummaryProcessModuleInfo500BIN : UserControl
    {
        #region <Constructors>
        public SummaryProcessModuleInfo500BIN(int pmIndex)
        {
            InitializeComponent();

            ProcessModuleIndex = pmIndex;
            _processGroup = ProcessModuleGroup.Instance;
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;

            _communicationInfo = new NetworkInformation();

            CorePorts = new List<int>
            {
                (int)LoadPortType.Core_1 + 1,
                (int)LoadPortType.Core_2 + 1
            };

            _substratesAtProcessModule = new List<Substrate>();
            _coreSubstratesAtProcessModule = new List<Substrate>();
            _sortSubstratesAtProcessModule = new List<Substrate>();

            MappedServiceIndex = new Dictionary<int, int>();
            MappedClientIndex = new Dictionary<int, int>();

            //_temporaryLocations = new List<string>();
            _temporaryLoadingLocations = new List<string>();
            _temporaryUnloadingLocations = new List<string>();
            RequestedLocation = new Dictionary<string, int>();

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
        private List<Substrate> _coreSubstratesAtProcessModule = null;
        private List<Substrate> _sortSubstratesAtProcessModule = null;

        private const int ColumnSubstrateName = 0;

        private const int ColumnRequestEnabled = 0;
        private const int ColumnRequestLocation = 1;

        private const int ColumnConnectionStatus = 0;
        private const int ColumnConnectionName = 1;

        private List<string> _temporaryLoadingLocations = null;
        private List<string> _temporaryUnloadingLocations = null;
        private readonly Dictionary<string, int> RequestedLocation = null;

        private readonly Color EnabledColor = Color.LimeGreen;
        private readonly Color DisabledColor = Color.White;

        private NetworkInformation _communicationInfo;

        private readonly Dictionary<int, int> MappedServiceIndex = null;
        private readonly Dictionary<int, int> MappedClientIndex = null;
        private readonly string ProcessModuleName = string.Empty;

        private string _currentLotId = string.Empty;
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
            if (gvReceivedRequest == null)
                return;

            string[] locations = _processGroup.GetEntrywayNames(ProcessModuleIndex);

            gvReceivedRequest.Rows.Clear();
            for (int i = 0; i < locations.Length; ++i)
            {
                string[] splitted = locations[i].Split('.');
                if (splitted.Length < 2)
                    continue;

                string loc = splitted[splitted.Length - 1];
                gvReceivedRequest.Rows.Add();
                gvReceivedRequest[ColumnRequestLocation, i].Value = loc;

                RequestedLocation[locations[i]] = i;               
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
            bool isCleared = false;
            if (gvCoreSubstrateList.Rows.Count != _coreSubstratesAtProcessModule.Count)
            {
                gvCoreSubstrateList.Rows.Clear();
                isCleared = true;
            }

            for (int i = 0; i < _coreSubstratesAtProcessModule.Count; ++i)
            {
                if (isCleared || gvCoreSubstrateList[ColumnSubstrateName, i].Value.ToString() != _coreSubstratesAtProcessModule[i].Name)
                {
                    if (isCleared)
                    {
                        gvCoreSubstrateList.Rows.Add();
                        gvCoreSubstrateList[ColumnSubstrateName, i].Value = string.Empty;
                    }
                }

                if (gvCoreSubstrateList[ColumnSubstrateName, i] == null)
                    continue;

                var cell = gvCoreSubstrateList[ColumnSubstrateName, i];
                var substrate = _coreSubstratesAtProcessModule[i];
                UpdateCellColor(substrate, ref cell);
            }
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

        private void UpdateRequestedCellColor(int cellIndex, bool enabled)
        {
            if (gvReceivedRequest.Rows.Count <= cellIndex)
                return;
            if(enabled)
            {
                gvReceivedRequest.Rows[cellIndex].Cells[ColumnRequestEnabled].Style.BackColor = EnabledColor;
                gvReceivedRequest.Rows[cellIndex].Cells[ColumnRequestEnabled].Style.SelectionBackColor = EnabledColor;
            }
            else
            {
                gvReceivedRequest.Rows[cellIndex].Cells[ColumnRequestEnabled].Style.BackColor = DisabledColor;
                gvReceivedRequest.Rows[cellIndex].Cells[ColumnRequestEnabled].Style.SelectionBackColor = DisabledColor;
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
                _coreSubstratesAtProcessModule.Clear();
                _sortSubstratesAtProcessModule.Clear();
                for (int i = 0; i < _substratesAtProcessModule.Count; ++i)
                {
                    string subType = _substratesAtProcessModule[i].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                    if (false == Enum.TryParse(subType, out SubstrateType convertedSubType))
                        continue;

                    switch (convertedSubType)
                    {
                        case SubstrateType.Core:
                            _coreSubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                            break;
                        case SubstrateType.Empty:
                        case SubstrateType.Bin1:
                        case SubstrateType.Bin2:
                        case SubstrateType.Bin3:
                            _sortSubstratesAtProcessModule.Add(_substratesAtProcessModule[i]);
                            break;
                        default:
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
            //_temporaryLocations.Clear();
            _temporaryLoadingLocations.Clear();
            _temporaryUnloadingLocations.Clear();
            _processGroup.IsLoadingRequested(ProcessModuleIndex, ref _temporaryLoadingLocations);
            _processGroup.IsUnloadingRequested(ProcessModuleIndex, ref _temporaryUnloadingLocations);

            //_temporaryLocations.AddRange(_temporaryLoadingLocations);
            //_temporaryLocations.AddRange(_temporaryUnloadingLocations);


            foreach (var item in RequestedLocation)
            {
                bool hasRequest = (_temporaryLoadingLocations.Contains(item.Key)
                    || _temporaryUnloadingLocations.Contains(item.Key));

                UpdateRequestedCellColor(item.Value, hasRequest);
            }




            //for (int i = 0; i < _temporaryLocations.Count; ++i)
            //{
            //    _temporaryLocation = _temporaryLocations[i];
            //    if (RequestedLocation.ContainsKey(_temporaryLocation))
            //    {
            //        _temporaryIndex = RequestedLocation[_temporaryLocation];
            //        gvReceivedRequest.Rows[_temporaryIndex].Cells[ColumnRequestEnabled].Style.BackColor = EnabledColor;
            //        gvReceivedRequest.Rows[_temporaryIndex].Cells[ColumnRequestEnabled].Style.SelectionBackColor = EnabledColor;
            //    }
            //    else
            //    {
            //        _temporaryIndex = RequestedLocation[_temporaryLocation];
            //        gvReceivedRequest.Rows[_temporaryIndex].Cells[ColumnRequestEnabled].Style.BackColor = DisabledColor;
            //        gvReceivedRequest.Rows[_temporaryIndex].Cells[ColumnRequestEnabled].Style.SelectionBackColor = DisabledColor;
            //    }
            //}

            #endregion </Requests>
        }
        #endregion </Methods>
    }
}
