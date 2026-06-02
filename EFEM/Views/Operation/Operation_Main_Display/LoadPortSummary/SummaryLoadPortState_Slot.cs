using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Views.MapManager;

namespace FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary
{
    public partial class SummaryLoadPortState_Slot : ParameterPanel
    {
        public SummaryLoadPortState_Slot(int lpIndex, DelegateCellClicked cellClickedEventHandler = null)
        {
            InitializeComponent();            

            MyLoadPortIndex = lpIndex;
            _carrierMapManager = CarrierMapManager.Instance;
            _carrierMapManager.AssignMapControls(lpIndex, ref mapLoadPortSlot, cellClickedEventHandler);

            this.Dock = DockStyle.Fill;
        }

        private readonly int MyLoadPortIndex;
        private static CarrierMapManager _carrierMapManager = null;        

        public override void CallFunctionByTimer()
        {
            UpdateSlotMap();

            base.CallFunctionByTimer();
        }

        public void DisableHighlight()
        {
            _carrierMapManager.DisableHighlight(MyLoadPortIndex, ref mapLoadPortSlot);
        }
        
        // 2024.10.30. jhlim [MOD] Invoke 추가
        private delegate void OnUpdateSlotMap();
        private void UpdateSlotMap()
        {
            if (InvokeRequired)
            {
                OnUpdateSlotMap d = new OnUpdateSlotMap(UpdateSlotMap);
                BeginInvoke(d, null);
            }
            else
            {
                _carrierMapManager.UpdateControls(MyLoadPortIndex);
            }
        }
        //private void UpdateSlotMap()
        //{
        //    _carrierMapManager.UpdateControls(MyLoadPortIndex);            
        //}
        // 2024.10.30. jhlim [END]
    }
}
