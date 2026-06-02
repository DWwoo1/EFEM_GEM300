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

using Define.DefineEnumProject.DigitalIO;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500BIN
{
    public partial class MainMonitoringSubPanel500BINDigitals : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public MainMonitoringSubPanel500BINDigitals()
        {
            InitializeComponent();

            _digitalIO = DigitalIO_.DigitalIO.GetInstance();
            InputLabels = new Dictionary<int, Sys3Controls.Sys3LedLabelWithText>();

            InitializeIoMonitorList();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private static DigitalIO_.DigitalIO _digitalIO = null;
        private readonly Dictionary<int, Sys3Controls.Sys3LedLabelWithText> InputLabels = null;

        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override void CallFunctionByTimer()
        {
            foreach (var item in InputLabels)
            {
                item.Value.Active = _digitalIO.ReadInput(item.Key);
            }
        }

        private void InitializeIoMonitorList()
        {
            // 2024.12.23. jhlim [MOD] 테이블 레이아웃패널 추가로 인한 사전요소추가 변경
            int colCount = tableLayoutPanel1.ColumnCount;
            int rowCount = tableLayoutPanel1.RowCount;
            for (int row = 0; row < rowCount; ++row)
            {
                for (int col = 0; col < colCount; ++col)
                {
                    if (!(tableLayoutPanel1.GetControlFromPosition(col, row) is Sys3Controls.Sys3LedLabelWithText control))
                        continue;

                    if (false == int.TryParse(control.Tag.ToString(), out int index))
                        continue;

                    InputLabels[index] = control;
                }
            }

            //foreach (Control item in Controls)
            //{
            //    if (!(item is Sys3Controls.Sys3LedLabelWithText control))
            //        continue;

            //    if (false == int.TryParse(control.Tag.ToString(), out int index))
            //        continue;

            //    InputLabels[index] = control;
            //}
            // 2024.12.23. jhlim [END]
        }
        #endregion </Methods>
    }
}
