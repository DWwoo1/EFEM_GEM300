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

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    public partial class FormProductionSummary : Form
    {
        #region <Constructors>
        private FormProductionSummary()
        {
            DoubleBuffered = true;

            InitializeComponent();

            LotData = new Dictionary<string, List<string>>();
        }
        #endregion </Constructors>

        #region <Fields>
        private static FormProductionSummary _Instance = null;

        private readonly Dictionary<string, List<string>> LotData = null;

        private string _targetTime;
        private string _substrateType;
        private int _qty;
        private const string LotIdForAll = "All";
        #endregion </Fields>

        #region <Properties>
        public static FormProductionSummary Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new FormProductionSummary();
                }

                return _Instance;
            }
        }
        #endregion </Properties>

        #region <Methods>
        #region <UI Events>
        private void ProcessingEvent(Keys enInputedKey)
        {
            switch (enInputedKey)
            {
                case Keys.Enter:
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                case Keys.Escape:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                default:
                    return;
            }

            this.Close();
        }
        private void Click_OkorCancel(object sender, EventArgs e)
        {
            Control ctr = sender as Control;

            switch (ctr.TabIndex)
            {
                case 0: // OK
                    ProcessingEvent(Keys.Enter);
                    break;
                case 1: // CANCEL
                    ProcessingEvent(Keys.Escape);
                    break;
            }
        }
        private void FormProductionSummary_KeyDown(object sender, KeyEventArgs e)
        {
            int nKeyCode = (int)e.KeyCode;

            switch (e.KeyCode)
            {
                case Keys.Escape: // Esc 입력 시
                case Keys.Back: // 백스페이스 입력 시
                    ProcessingEvent(Keys.Escape);
                    break;
                case Keys.Enter: // 엔터 입력 시
                    ProcessingEvent(Keys.Enter);
                    break;
                default:
                    break;
            }
        }
        private void GvCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            string selectedLot = string.Empty;
            if (row >= 0)
            {
                selectedLot = gvLotList[0, row].Value.ToString();
            }
            else
            {
                selectedLot = LotIdForAll;
            }
            DisplayWaferList(selectedLot);
        }

        #endregion </UI Events>

        #region <Externals>
        // 2026.07.06. jhlim [MOD] 폴더 스캔 대신 IHistoryQuery 집계 결과를 받아 표시하도록 변경.
        // 조회는 호출 측(뷰)이 백그라운드에서 수행한다 - UI 스레드 블로킹 방지.
        public bool ShowMessage(bool isCore, DateTime targetDate, Dictionary<string, List<string>> lotSubstrates)
        {
            if (IsFormOpening())
                return false;

            _substrateType = isCore ? "CORE" : "BIN";
            _targetTime = targetDate.ToString("yyyy/MM/dd");

            SetLotData(lotSubstrates);
            DisplayLotList();
            UpdateUI();

            this.CenterToScreen();

            if (!this.Modal)
                this.ShowDialog();

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }

            return false;
        }
        #endregion </Externals>

        #region <Internals>
        private void UpdateUI()
        {
            lblDate.Text = _targetTime;
            lblSubstrateType.Text = _substrateType;
            lblProductionQty.Text = _qty.ToString();
        }
        private void SetLotData(Dictionary<string, List<string>> lotSubstrates)
        {
            LotData.Clear();
            _qty = 0;

            if (lotSubstrates == null)
                return;

            foreach (var item in lotSubstrates)
            {
                LotData[item.Key] = item.Value;
                _qty += item.Value.Count;
            }
        }
        private void DisplayWaferList(string lotId)
        {
            gvWaferList.Rows.Clear();
            if (lotId.Equals(LotIdForAll))
            {
                int indexOfWafer = 0;
                foreach (var item in LotData)
                {
                    for (int i = 0; i < item.Value.Count; ++i)
                    {
                        gvWaferList.Rows.Add();
                        gvWaferList[0, indexOfWafer++].Value = item.Value[i];
                    }
                }
            }
            else
            {
                if (LotData.TryGetValue(lotId, out List<string> wafers))
                {
                    for (int i = 0; i < wafers.Count; ++i)
                    {
                        gvWaferList.Rows.Add();
                        gvWaferList[0, i].Value = wafers[i];
                    }
                }
            }
        }
        private void DisplayLotList()
        {
            gvLotList.Rows.Clear();
            if (LotData.Count <= 0)
            {
                gvWaferList.Rows.Clear();
                return;
            }

            gvLotList.Rows.Add();
            gvLotList[0, 0].Value = LotIdForAll;

            foreach (var item in LotData)
            {
                int index = gvLotList.Rows.Count;
                gvLotList.Rows.Add();
                gvLotList[0, index].Value = item.Key.ToString();
            }

            DisplayWaferList(LotIdForAll);
        }
        private bool IsFormOpening()
        {
            FormCollection fc = Application.OpenForms;
            bool bIsFormOpened = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "FormProductionSummary")
                {
                    bIsFormOpened = true;
                    break;
                }
            }

            return bIsFormOpened;
        }
        #endregion </Internals>

        #endregion </Methods>

    }
}
