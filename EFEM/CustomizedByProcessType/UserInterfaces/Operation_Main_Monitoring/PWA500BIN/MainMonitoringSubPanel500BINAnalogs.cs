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

using Define.DefineEnumProject.DigitalIO.PWA500BIN;
using Define.DefineEnumProject.AnalogIO.PWA500BIN;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainMonitoring.PWA500BIN
{
    public partial class MainMonitoringSubPanel500BINAnalogs : UserControlForMainView.CustomView
    {
        #region <Constructors>
        public MainMonitoringSubPanel500BINAnalogs()
        {
            InitializeComponent();

            _analogIO = AnalogIO_.AnalogIO.GetInstance();
            _digitalIO = DigitalIO_.DigitalIO.GetInstance();

            IoMonitorList = new Dictionary<Tuple<int, int>, Sys3Controls.Sys3LedLabelWithText>();
            AnalogInputs = new ConcurrentDictionary<int, double>();
            DigitalInputs = new ConcurrentDictionary<int, bool>();

            UtilitiesOffsetedIndex = (int)EN_DIGITAL_IN.EFEM_MAIN_CDA_PRESSURE_SWITCH;

            InitializeIoMonitorList();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private static DigitalIO_.DigitalIO _digitalIO = null;
        private static AnalogIO_.AnalogIO _analogIO = null;
        // 2024.12.23. jhlim [MOD] 디지털, 아날로그 인덱스를 저장하기 위해 키를 Tuple로 변경
        private readonly Dictionary<Tuple<int, int>, Sys3Controls.Sys3LedLabelWithText> IoMonitorList = null;
        //private readonly Dictionary<int, Control> IoMonitorList = null;
        // 2024.12.23. jhlim [END]
        private readonly ConcurrentDictionary<int, double> AnalogInputs = null;
        private readonly ConcurrentDictionary<int, bool> DigitalInputs = null;
        private double _temporaryValue = 0.0;
        private const double InitialValue = -1000;
        private readonly int UtilitiesOffsetedIndex;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override void CallFunctionByTimer()
        {
            // 2024.12.23. jhlim [MOD] 업데이트 방법 변경
            foreach (var item in IoMonitorList)
            {
                item.Value.Text = Math.Round(UpdateAnalogValues(item.Key.Item1), 3).ToString();
                item.Value.Active = UpdateDigitalValues(item.Key.Item2);
            }
            //foreach (Control item in Controls)
            //{
            //    if (!(item is Sys3Controls.Sys3LedLabelWithText control))
            //        continue;

            //    if (false == int.TryParse(control.Tag.ToString(), out int relIndex))
            //        continue;

            //    UpdateAnalogValues(relIndex);

            //    int diIndex = UtilitiesOffsetedIndex + relIndex;
            //    control.Active = UpdateDigitalValues(diIndex);
            //    //control.Invalidate();
            //}
            // 2024.12.23. jhlim [END]
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

                    if (false == int.TryParse(control.Tag.ToString(), out int relIndex))
                        continue;

                    int diIndex = UtilitiesOffsetedIndex + relIndex;
                    Tuple<int, int> ioInfo = Tuple.Create(relIndex, diIndex);
                    DigitalInputs[diIndex] = false;
                    
                    IoMonitorList[ioInfo] = control;
                }
            }

            //foreach (Control item in Controls)
            //{
            //    if (!(item is Sys3Controls.Sys3LedLabelWithText control))
            //        continue;

            //    if (false == int.TryParse(control.Tag.ToString(), out int relIndex))
            //        continue;

            //    IoMonitorList.Add(relIndex, control);

            //    int diIndex = UtilitiesOffsetedIndex + relIndex;
            //    DigitalInputs[diIndex] = false;
            //}
            // 2024.12.23. jhlim [END]

            var anValue = (EN_ANALOG_IN[])Enum.GetValues(typeof(EN_ANALOG_IN));
            for (int i = 0; i < anValue.Length; ++i)
            {
                AnalogInputs.TryAdd(i, InitialValue);
            }
        }
        private double UpdateAnalogValues(int index)
        {
            if (false == AnalogInputs.TryGetValue(index, out _temporaryValue))
                return 0.0;

            if (_temporaryValue != _analogIO.ReadInputValue(index))
            {
                _temporaryValue = _analogIO.ReadInputValue(index);
                
                AnalogInputs[index] = _temporaryValue;
                //IoMonitorList[index].Text = Math.Round(_temporaryValue, 3).ToString();
            }

            return AnalogInputs[index];
        }
        private bool UpdateDigitalValues(int index)
        {
            if (false == DigitalInputs.TryGetValue(index, out bool result))
                return false;

            if (result != _digitalIO.ReadInput(index))
            {
                result = _digitalIO.ReadInput(index);
            }

            return result;
        }
        #endregion </Methods>
    }
}
