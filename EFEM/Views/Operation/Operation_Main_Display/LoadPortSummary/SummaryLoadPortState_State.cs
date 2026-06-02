using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;

namespace FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary
{
    public partial class SummaryLoadPortState_State : ParameterPanel
    {
        #region <Constructors>
        public SummaryLoadPortState_State(int lpIndex)
        {
            InitializeComponent();
            
            _loadPortManager = LoadPortManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            
            _transferState = LoadPortTransferStates.Unknown;
            _carrierIdState = CarrierIdVerificationStates.NotRead;
            _carrierSlotState = CarrierSlotMapVerificationStates.NotRead;
            //_carrierAccessState = CarrierAccessStates.Unknown;
            _loadingMode = LoadPortLoadingMode.Unknown;
            _accessMode = LoadPortAccessMode.Manual;
            
            Index = lpIndex;
            PortId = _loadPortManager.GetLoadPortPortId(Index);

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly int Index;
        private readonly int PortId;
        private static LoadPortManager _loadPortManager = null;
        private static CarrierManagementServer _carrierServer = null;

        private LoadPortTransferStates _transferState;
        private CarrierIdVerificationStates _carrierIdState;
        private CarrierSlotMapVerificationStates _carrierSlotState;
        private CarrierAccessStates? _carrierAccessState;
        private LoadPortLoadingMode _loadingMode;
        private LoadPortAccessMode _accessMode;

        private const string Unknown = "Unknown";
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            UpdateStatus(true);

            base.ProcessWhenActivation();
        }

        protected override void ProcessWhenDeactivation()
        {
            base.ProcessWhenDeactivation();
        }

        public override void CallFunctionByTimer()
        {
            UpdateStatus(false);

            base.CallFunctionByTimer();
        }
        #endregion </Override Methods>

        #region <Display Methods>
        private void UpdateStatus(bool initialized)
        {
            // Transfer State
            if (_loadPortManager.GetLoadPortTransferState(Index, ref _transferState) || initialized)
            {
                lblTransferState.Text = _transferState.ToString();
            }
            else
            {
                lblTransferState.Text = Unknown;
            }

            // Id State
            if (_loadPortManager.GetLoadPortCarrierIdState(Index, ref _carrierIdState) || initialized)
            {
                lblIdStatus.Text = _carrierIdState.ToString();
            }
            else
            {
                lblIdStatus.Text = Unknown;
            }

            // Slot State
            if (_loadPortManager.GetLoadPortCarrierSlotMapState(Index, ref _carrierSlotState) || initialized)
            {
                lblSlotStatus.Text = _carrierSlotState.ToString();
            }
            else
            {
                lblSlotStatus.Text = Unknown;
            }

            // AccessMode State
            if (false == _carrierAccessState.HasValue ||
                _carrierServer.GetCarrierAccessingStatus(PortId) != _carrierAccessState || initialized)
            {
                _carrierAccessState = _carrierServer.GetCarrierAccessingStatus(PortId);
                lblAccessStatus.Text = _carrierAccessState.ToString();
            }

            // LoadingType
            if (false == _loadPortManager.GetCarrierLoadingType(Index).Equals(_loadingMode) || initialized)
            {
                _loadingMode = _loadPortManager.GetCarrierLoadingType(Index);
                lblLoadingMode.Text = _loadingMode.ToString();
            }

            // AHMS
            if (false == _loadPortManager.GetAccessMode(Index).Equals(_accessMode) || initialized)
            {
                _accessMode = _loadPortManager.GetAccessMode(Index);
                sys3Label10.Text = _accessMode.ToString();
            }
        }
        #endregion </Display Methods>

        #endregion </Methods>
    }
}