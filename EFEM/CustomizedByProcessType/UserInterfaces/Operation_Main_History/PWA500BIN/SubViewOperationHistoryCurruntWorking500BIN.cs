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
using EFEM.CustomizedByProcessType.PWA500BIN;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500BIN
{
    public partial class SubViewOperationHistoryCurruntWorking500BIN : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public SubViewOperationHistoryCurruntWorking500BIN()
        {
            InitializeComponent();

            _loadPortManager = LoadPortManager.Instance;
            _processGroup = ProcessModuleGroup.Instance;
            _substrateManager = SubstrateManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _functionsForPWA500 = FunctionsForPWA500BIN_TP.Instance;
            _lotHistoryLog = LotHistoryLog.Instance;
            _selectionList = Form_SelectionList.GetInstance();
            _equipmentState = EquipmentState.GetInstance();

            _temporarySubstrates = new List<Substrate>();
            //_coreSubstrates = new List<Substrate>();
            _binSubstrates = new List<Substrate>();

            CurrentCoreCarriers = new Dictionary<int, string>();

            UserControlCoreCarrierHistory = new MainDisplaySubPanelLotHistoryDisplayer(true)
            {
                Dock = DockStyle.Fill
            };            
            tableLayoutPanel1.Controls.Add(UserControlCoreCarrierHistory, 1, 1);

            CurrentBinSubstrates = new Dictionary<string, string>();

            UserControlBinWaferHistory = new MainDisplaySubPanelLotHistoryDisplayer(true)
            {
                Dock = DockStyle.Fill
            };
            tableLayoutPanel1.Controls.Add(UserControlBinWaferHistory, 1, 0);

            // 자동 업데이트는 일단 막자.
            //_lotHistoryLog.AttachDisplayLogAction(AddCurrentPortHistory);

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private static ProcessModuleGroup _processGroup = null;
        private static SubstrateManager _substrateManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static LoadPortManager _loadPortManager = null;
        private static FunctionsForPWA500BIN_TP _functionsForPWA500 = null;
        private static LotHistoryLog _lotHistoryLog = null;
        private static Form_SelectionList _selectionList = null;
        private static EquipmentState _equipmentState = null;

        private const int ProcessModuleIndex = 0;

        #region <Core Carrier>
        private List<Substrate> _temporarySubstrates = null;
        //private List<Substrate> _coreSubstrates = null;
        private List<Substrate> _binSubstrates = null;
        private readonly Dictionary<int, string> CurrentCoreCarriers = null;
        private int _selectedCoreCarrierPort = -1;
        private string _selectedCarrierId = string.Empty;
        private bool _isCoreSelectionChanged = true;
        #endregion </Core Carrier>

        #region <Bin Wafer>
        private readonly Dictionary<string, string> CurrentBinSubstrates = null;
        private string _selectedBinWafer = string.Empty;
        #endregion </Bin Wafer>

        #region <User Control>
        private readonly MainDisplaySubPanelLotHistoryDisplayer UserControlCoreCarrierHistory = null;
        private readonly MainDisplaySubPanelLotHistoryDisplayer UserControlBinWaferHistory = null;
        #endregion </User Control>

        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            // 모든 자재 리스트는 아래 조건에서 다시 갱신한다.
            // 1. 화면전환 시
            // 2. Label 클릭

            // 코어 리스트는 작업 중인 캐리어 리스트만 포함한다.
            // BinList는 설비내(EFEM, 본설비) 모든 자재 중 Empty, Bin1~3 자재 중 Processing, Processed 상태만 리스트로 가져온다.

            // 리프레시는 현재 선택된 자재의 로그파일만 다시 읽는다.


            // Core 리스트 갱신
            RefreshCoreCarrierList();
            RefreshCoreCarrierHistory();

            // Bin 리스트 갱신
            RefreshBinSubstrateList();
            RefreshBinSubstrateHistory();

            //RefreshCoreCarrierList(true, ref _isCoreSelectionChanged);
            //RefreshCoreCarrierHistory();

            //RefreshBinSubstrateList();
            //RefreshBinSubstrateHistory();

            //ChangeSelectedMaterialByProcessingStatus();
            //ChangeSelectedCarrierByAccessingStatus();            

            base.ProcessWhenActivation();
        }
        public override void CallFunctionByTimer()
        {
            //if (_equipmentState.GetState().Equals(EQUIPMENT_STATE.EXECUTING) || System.Diagnostics.Debugger.IsAttached)
            //{
            //    // Core
            //    _isCoreSelectionChanged = false;
            //    RefreshCoreCarrierList(true, ref _isCoreSelectionChanged);
            //    if (_isCoreSelectionChanged)
            //    {
            //        // 선택된 포트가 변경되면 파일을 다시 로드한다.
            //        RefreshCoreCarrierHistory();
            //    }
            //    // Bin

            //}
            //if (_equipmentState.GetState().Equals(EQUIPMENT_STATE.EXECUTING) || System.Diagnostics.Debugger.IsAttached)
            //{
            //    Enabled = false;
            //}

            lblSelectedCoreCarrier.Text = _selectedCarrierId;
            lblSelectedBinWafer.Text = _selectedBinWafer;

            base.CallFunctionByTimer();
        }
        protected override void ProcessWhenDeactivation()
        {
        }
        #endregion </Override Methods>

        #region <UI Events>
        private void AddCurrentPortHistory(int port, string messageToAdd)
        {
            if (false == Visible)
                return;

            if (port.Equals(_selectedCoreCarrierPort))
            {
                UserControlCoreCarrierHistory.AddCurrentHistory(messageToAdd);
            }
        }

        private void BtnRefreshClicked(object sender, EventArgs e)
        {
            if (sender.Equals(btnRefreshCoreCarrierHistory))
            {
                //RefreshCoreCarrierList();
                if (false == RefreshCoreCarrierHistory())
                {
                    _selectedCoreCarrierPort = -1;
                    _selectedCarrierId = string.Empty;
                }
            }
            else if (sender.Equals(btnRefreshBinWaferHistory))
            {
                //RefreshBinSubstrateList();
                if (false == RefreshBinSubstrateHistory())
                {
                    _selectedBinWafer = string.Empty;
                }
            }
        }
        private void HistorySelectionLabelClicked(object sender, EventArgs e)
        {
            if (sender.Equals(lblSelectedCoreCarrier))
            {
                RefreshCoreCarrierList();
                if (_selectionList.CreateForm("Select Core Carrier", CurrentCoreCarriers.Values.ToArray(), CurrentCoreCarriers.Keys.ToArray(), _selectedCoreCarrierPort))
                {
                    _selectionList.GetResult(ref _selectedCoreCarrierPort);

                    RefreshCoreCarrierHistory();
                }
            }
            else if (sender.Equals(lblSelectedBinWafer))
            {
                RefreshBinSubstrateList();
                List<int> indexOfSubstrates = new List<int>();
                int index = 0;
                foreach (var item in CurrentBinSubstrates)
                {
                    indexOfSubstrates.Add(index++);
                }

                if (indexOfSubstrates.Count <= 0)
                    return;

                if (_selectionList.CreateForm("Select Bin Substrate", CurrentBinSubstrates.Keys.ToArray(), indexOfSubstrates.ToArray(), 0))
                {
                    _selectionList.GetResult(ref _selectedBinWafer);

                    RefreshBinSubstrateHistory();
                }
            }
        }
        #endregion </UI Events>

        #region <Internals>
        //private void ChangeSelectedCarrierByAccessingStatus()
        //{
        //    if (CurrentCoreCarriers == null ||
        //        CurrentCoreCarriers.Count <= 0)
        //        return;

        //    foreach (var item in CurrentCoreCarriers)
        //    {
        //        var accessStatus = _carrierServer.GetCarrierAccessingStatus(item.Key);
        //        if (accessStatus.Equals(Defines.LoadPort.CarrierAccessStates.InAccessed))
        //        {
        //            _selectedCarrierId = item.Value;
        //            break;
        //        }
        //    }
        //}
        //private void ChangeSelectedMaterialByProcessingStatus()
        //{
        //    if (_equipmentState.GetState().Equals(EQUIPMENT_STATE.EXECUTING) || System.Diagnostics.Debugger.IsAttached)
        //    {
        //        //_coreSubstrates.Clear();
        //        _binSubstrates.Clear();

        //        _temporarySubstrates.Clear();
        //        if (_substrateManager.GetSubstratesAll(ref _temporarySubstrates))
        //        {
        //            for (int i = 0; i < _temporarySubstrates.Count; ++i)
        //            {
        //                var status = _temporarySubstrates[i].ProcessingStatus;
        //                if (false == status.Equals(Defines.MaterialTracking.ProcessingStates.InProcess) &&
        //                    false == status.Equals(Defines.MaterialTracking.ProcessingStates.Processed))
        //                    continue;

        //                var substrateTypeString = _temporarySubstrates[i].GetAttribute(PWA500BINSubstrateAttributes.SubstrateType);
        //                if (false == Enum.TryParse(substrateTypeString, out SubstrateType substrateType))
        //                    continue;

        //                switch (substrateType)
        //                {
        //                    //case SubstrateType.Core:
        //                    //    _coreSubstrates.Add(_temporarySubstrates[i]);
        //                    //    break;
        //                    case SubstrateType.Empty:
        //                    case SubstrateType.Bin1:
        //                    case SubstrateType.Bin2:
        //                    case SubstrateType.Bin3:
        //                        _binSubstrates.Add(_temporarySubstrates[i]);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }

        //        //if (_coreSubstrates != null && _coreSubstrates.Count > 0)
        //        //{
        //        //    var resultCore = _coreSubstrates.FirstOrDefault(x => x.ProcessingStatus.Equals(Defines.MaterialTracking.ProcessingStates.Processed))
        //        //        ?? _coreSubstrates.FirstOrDefault(x => x.ProcessingStatus.Equals(Defines.MaterialTracking.ProcessingStates.InProcess));
        //        //    if (resultCore != null)
        //        //    {
        //        //        _selectedCoreCarrierPort = resultCore.SourcePortId;
        //        //    }
        //        //}

        //        if (_binSubstrates != null && _binSubstrates.Count > 0)
        //        {
        //            var resultBin = _binSubstrates.FirstOrDefault(x => x.ProcessingStatus.Equals(Defines.MaterialTracking.ProcessingStates.Processed))
        //                ?? _binSubstrates.FirstOrDefault(x => x.ProcessingStatus.Equals(Defines.MaterialTracking.ProcessingStates.InProcess));
        //            if (resultBin != null)
        //            {
        //                _selectedBinWafer = resultBin.Name;
        //            }
        //        }
        //    }
        //}

        private void RefreshBinSubstrateList()
        {
            CurrentBinSubstrates.Clear();

            _temporarySubstrates.Clear();
            if (_substrateManager.GetSubstratesAll(ref _temporarySubstrates))
            {
                for (int i = 0; i < _temporarySubstrates.Count; ++i)
                {
                    var status = _temporarySubstrates[i].ProcessingStatus;
                    if (false == status.Equals(Defines.MaterialTracking.ProcessingStates.InProcess) &&
                        false == status.Equals(Defines.MaterialTracking.ProcessingStates.Processed))
                        continue;

                    var substrateTypeString = _temporarySubstrates[i].GetAttribute(PWA500SubstrateAttributes.SubstrateType);
                    if (false == Enum.TryParse(substrateTypeString, out SubstrateType substrateType))
                        continue;

                    switch (substrateType)
                    {
                        case SubstrateType.Empty:
                        case SubstrateType.Bin1:
                        case SubstrateType.Bin2:
                        case SubstrateType.Bin3:
                            {
                                string name = _temporarySubstrates[i].Name;
                                CurrentBinSubstrates[name] = _lotHistoryLog.GetSubstratePath(name, false);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            //var filePath = Path.GetDirectoryName(_lotHistoryLog.GetSubstratePath("Temporary", false));
            //string[] files = Directory.GetFiles(filePath);
            //for (int i = 0; files != null && i < files.Length; ++i)
            //{
            //    var name = Path.GetFileNameWithoutExtension(files[i]);
            //    CurrentBinSubstrates[name] = files[i];
            //}
        }
        private string GetCurrentBinSubstratePath()
        {
            if (string.IsNullOrEmpty(_selectedBinWafer) ||
                false == CurrentBinSubstrates.ContainsKey(_selectedBinWafer))
            {
                _selectedBinWafer = string.Empty;
                return string.Empty;
            }

            return _lotHistoryLog.GetSubstratePath(_selectedBinWafer, false);
        }
        private bool RefreshBinSubstrateHistory()
        {
            var filePath = GetCurrentBinSubstratePath();
            UserControlBinWaferHistory.DisplayHistory(filePath);

            if (string.IsNullOrEmpty(filePath))
                return false;

            var dirPath = Path.GetDirectoryName(filePath);
            if (false == Directory.Exists(dirPath))
                return false;

            return File.Exists(filePath);
        }

        private void RefreshCoreCarrierList()
        {
            CurrentCoreCarriers.Clear();

            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                var substrateType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
                if (false == substrateType.Equals(SubstrateType.Core))
                    continue;

                if (false == _loadPortManager.IsLoadPortEnabled(i))
                    continue;

                int portId = _loadPortManager.GetLoadPortPortId(i);
                if (false == _carrierServer.HasCarrier(portId))
                    continue;

                CurrentCoreCarriers[portId] = _carrierServer.GetCarrierId(portId);
            }
        }

        //private void RefreshCoreCarrierList(bool useAutoSelection, ref bool isChanged)
        //{
        //    CurrentCoreCarriers.Clear();

        //    for (int i = 0; i < _loadPortManager.Count; ++i)
        //    {
        //        var substrateType = _functionsForPWA500.GetSubstrateTypeByLoadPortIndex(i);
        //        if (false == substrateType.Equals(SubstrateType.Core))
        //            continue;

        //        if (false == _loadPortManager.IsLoadPortEnabled(i))
        //            continue;

        //        int portId = _loadPortManager.GetLoadPortPortId(i);
        //        if (false == _carrierServer.HasCarrier(portId))
        //            continue;

        //        if (useAutoSelection)
        //        {
        //            var accessStatus = _carrierServer.GetCarrierAccessingStatus(portId);
        //            if (accessStatus.Equals(Defines.LoadPort.CarrierAccessStates.InAccessed))
        //            {
        //                int prevSelectedCarrierPort = _selectedCoreCarrierPort;
        //                _selectedCoreCarrierPort = portId;   
        //                _selectedCarrierId = _carrierServer.GetCarrierId(portId);

        //                if (prevSelectedCarrierPort != _selectedCoreCarrierPort)
        //                    isChanged = true;
        //            }
        //        }

        //        CurrentCoreCarriers[portId] = _carrierServer.GetCarrierId(portId);
        //    }

        //    if (useAutoSelection)
        //    {
        //        if (CurrentCoreCarriers.Count <= 0)
        //        {
        //            _selectedCoreCarrierPort = -1;
        //            _selectedCarrierId = string.Empty;
        //        }
        //    }
        //}
        private bool RefreshCoreCarrierHistory()
        {
            var filePath = GetCurrentCarrierPath();
            UserControlCoreCarrierHistory.DisplayHistory(filePath);

            if (string.IsNullOrEmpty(filePath))
                return false;

            var dirPath = Path.GetDirectoryName(filePath);
            if (false == Directory.Exists(dirPath))
                return false;

            return File.Exists(filePath);
        }

        private string GetCurrentCarrierPath()
        {
            if (false == CurrentCoreCarriers.TryGetValue(_selectedCoreCarrierPort, out string carrierId))
            {
                _selectedCarrierId = string.Empty;
                return string.Empty;
            }

            _selectedCarrierId = carrierId;
            return _lotHistoryLog.GetCarrierHistoryPath(_selectedCoreCarrierPort, carrierId);
        }
        #endregion </Internals>

        #endregion </Methods>
    }
}