using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Config;
using EFEM.Modules;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.Defines.RFID;

namespace FrameOfSystem3.Views.Setup.RFID
{
    public partial class SetupSubViewRfid : ParameterPanel
    {
        public SetupSubViewRfid(LoadPortLoadingMode loadingMode)
        {
            InitializeComponent();
            
            //switch (loadingMode)
            //{
            //    case LoadPortLoadingMode.Cassette:
            //        {
            //            m_lblLotLength.Visible = true;
            //            m_labelSetLotIdLength.Visible = true;
            //            m_labelLotIdLength.Visible = true;

            //            m_lblCarrierLength.Visible = true;
            //            m_labelSetCarrierIdLength.Visible = true;
            //            m_labelCarrierIdLength.Visible = true;
            //        }
            //        break;
            //    case LoadPortLoadingMode.Foup:
            //        {
            //            m_lblLotLength.Visible = false;
            //            m_labelSetLotIdLength.Visible = false;
            //            m_labelLotIdLength.Visible = false;

            //            m_lblCarrierLength.Visible = false;
            //            m_labelSetCarrierIdLength.Visible = false;
            //            m_labelCarrierIdLength.Visible = false;
            //        }
            //        break;
            //    default:
            //        return;
            //}
            
            LoadingMode = loadingMode;
            RfidNames = Enum.GetNames(typeof(EN_PANEL_LIST));

            this.Dock = DockStyle.Fill;
        }

        #region <Const>
        private readonly string MIN_OF_PARAM = "0";
        private readonly string MAX_OF_PARAM = "999999";
        #endregion </Const>

        #region <Fields>
        private static RFIDManager _rfidManager = RFIDManager.Instance;

        private readonly Functional.Form_SelectionList m_InstanceOfSelectionList = Functional.Form_SelectionList.GetInstance();
        private readonly Functional.Form_Keyboard m_InstanceOfKeyboard = Functional.Form_Keyboard.GetInstance();
        private readonly Functional.Form_Calculator m_InstanceOfCalculator = Functional.Form_Calculator.GetInstance();
        private readonly Functional.Form_MessageBox _messageBox = Functional.Form_MessageBox.GetInstance();
        private readonly StringBuilder StringBuilder = new StringBuilder();

        private int CurrentCassetteId;
        private string[] TotalLotId = new string[6];
        private string[] TotalCarrierId = new string[6];

        private int _myIndex;
        private readonly LoadPortLoadingMode LoadingMode;
        private readonly string[] RfidNames = null;
        #endregion </Fields>

        #region <Enum>
        enum EN_PANEL_LIST
        {
            LoadPort1 = 0,
            LoadPort2,
            LoadPort3,
            LoadPort4,
            LoadPort5,
            LoadPort6,
        }
        #endregion </Enum>

        #region <Methods>
        private void ChangeReader(int index)
        {
            m_labelSetLotIdAddress.Text = _rfidManager.GetLotIdAddress(index, LoadingMode).ToString();
            m_labelSetLotIdLength.Text = _rfidManager.GetLotIdLength(index, LoadingMode).ToString();
            m_labelSetCarrierIdAddress.Text = _rfidManager.GetCarrierIdAddress(index, LoadingMode).ToString();
            m_labelSetCarrierIdLength.Text = _rfidManager.GetCarrierIdLength(index, LoadingMode).ToString();

            m_labelLotId.Text = TotalLotId[CurrentCassetteId];
            m_labelCarrierId.Text = TotalCarrierId[CurrentCassetteId];
        }
        private void Click_Configuration(object sender, EventArgs e)
        {
			Control ctrl = sender as Control;

			string strResult = string.Empty;
            string strNumCheck = string.Empty;

            switch (ctrl.TabIndex)
            {
				case 0:
                    
                    if (m_InstanceOfSelectionList.CreateForm(m_lblRFIDName.Text, RfidNames, m_labelRFIDName.Text))
                    {
                        m_InstanceOfSelectionList.GetResult(ref _myIndex);
                        if (_myIndex < 0)
                            _myIndex = 0;

                        m_labelRFIDName.Text = RfidNames[_myIndex];
                        CurrentCassetteId = _myIndex;
                        ChangeReader(_myIndex);
                    }
                    break;
                case 1:
					if (m_InstanceOfKeyboard.CreateForm(m_labelLotId.Text))
					{
						m_InstanceOfKeyboard.GetResult(ref strResult);
						m_labelLotId.Text = strResult;
                        TotalLotId[CurrentCassetteId] = strResult;
                    }
                    break;
                case 2:
					if (m_InstanceOfKeyboard.CreateForm(m_labelCarrierId.Text))
                    {
                        m_InstanceOfKeyboard.GetResult(ref strResult);
                        m_labelCarrierId.Text = strResult;
                        TotalCarrierId[CurrentCassetteId] = strResult;
                    }
                    break;
                case 3:
                    if (m_InstanceOfCalculator.CreateForm(m_labelLotIdAddress.Text,MIN_OF_PARAM, MIN_OF_PARAM))
                    {
                        m_InstanceOfCalculator.GetResult(ref strResult);
                        m_labelLotIdAddress.Text = strResult;
                    }
                    break;
                case 4:
                    if (m_InstanceOfCalculator.CreateForm(m_labelLotIdLength.Text, MIN_OF_PARAM, MAX_OF_PARAM))
                    {
                        m_InstanceOfCalculator.GetResult(ref strResult);
                        m_labelLotIdLength.Text = strResult;
                    }
                    break;
                case 5:
                    if (m_InstanceOfCalculator.CreateForm(m_labelCarrierIdAddress.Text, MIN_OF_PARAM, MIN_OF_PARAM))
                    {
                        m_InstanceOfCalculator.GetResult(ref strResult);
                        m_labelCarrierIdAddress.Text = strResult;
                    }
                    break;
                case 6:
                    if (m_InstanceOfCalculator.CreateForm(m_labelCarrierIdLength.Text, MIN_OF_PARAM, MAX_OF_PARAM))
                    {
                        m_InstanceOfCalculator.GetResult(ref strResult);
                        m_labelCarrierIdLength.Text = strResult;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Click_Control(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE) &&
                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
                return;
            Sys3Controls.Sys3button button = sender as Sys3Controls.Sys3button;
            if (button == null)
                return;

            //string errorMessage = "Invalid value entered.";
            string ApplyMessage = "Applied successfully.";

            if (m_labelRFIDName.Text == "--")
            {
                string errorMessage_cassette = "Cassette is not selected.";
                _messageBox.ShowMessage(errorMessage_cassette);
                return;
            }

            //switch (ctrl.TabIndex)
            //{
            //    case 0:     // Lot Id, Length Set
            //        if (m_labelLotIdAddress.Text == "--" /*|| m_labelLotIdLength.Text == "--"*/
            //            || m_labelLotIdLength.Text == "0")
            //        {
            //            _messageBox.ShowMessage(errorMessage);
            //            return;
            //        }
            //        _rfidManager.SetLotIdAddress(_myIndex, LoadingMode, int.Parse(m_labelLotIdAddress.Text), int.Parse(m_labelLotIdLength.Text));
            //        break;
            //    case 1:     // Lot Carrier, Length Set
            //        if (m_labelCarrierIdAddress.Text == "--" /*|| m_labelCarrierIdLength.Text == "--"*/
            //            || m_labelCarrierIdLength.Text == "0")
            //        {
            //            _messageBox.ShowMessage(errorMessage);
            //            return;
            //        }
            //        _rfidManager.SetCarrierIdAddress(_myIndex, LoadingMode, int.Parse(m_labelCarrierIdAddress.Text), int.Parse(m_labelCarrierIdLength.Text));
                    
            //        break;
            //    default:
            //        return;
            //}

            ChangeReader(_myIndex);
            _messageBox.ShowMessage(ApplyMessage);
        }
        private async void Click_Command(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.IDLE) &&
                false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.PAUSE))
                return;
            Sys3Controls.Sys3button button = sender as Sys3Controls.Sys3button;
            if (button == null)
                return;

            RfidCommand command;
            if (false == Enum.TryParse(button.Tag.ToString(), out command))
                return;

            //string errorMessage = "Invalid value entered.";

            if (m_labelRFIDName.Text == "--")
            {
                string errorMessage_cassette = "Cassette is not selected.";
                _messageBox.ShowMessage(errorMessage_cassette);
                return;
            }

            //switch (command)
            //{
            //    case RfidCommand.READ_LOT_ID:
            //        if (m_labelLotIdAddress.Text == "--" || m_labelLotIdLength.Text == "--"
            //            || m_labelSetLotIdLength.Text == "0")
            //        {
            //            _messageBox.ShowMessage(errorMessage);
            //            return;
            //        }
            //        break;
            //    case RfidCommand.WRITE_LOT_ID:
            //        if (m_labelLotIdAddress.Text == "--" || string.IsNullOrEmpty(m_labelLotId.Text))
            //        {
            //            _messageBox.ShowMessage(errorMessage);
            //            return;
            //        }
            //        break;
            //    case RfidCommand.READ_CARRIER_ID:
            //        if (m_labelCarrierIdAddress.Text == "--" || m_labelCarrierIdLength.Text == "--"
            //             || m_labelSetCarrierIdLength.Text == "0")
            //        {
            //            _messageBox.ShowMessage(errorMessage);
            //            return;
            //        }
            //        break;
            //    case RfidCommand.WRITE_CARRIER_ID:
            //        if (m_labelCarrierIdAddress.Text == "--" || string.IsNullOrEmpty(m_labelCarrierId.Text))
            //        {
            //            _messageBox.ShowMessage(errorMessage);
            //            return;
            //        }
            //        break;
            //    default:
            //        break;
            //}

            this.Enabled = false;
            var waitResponse = System.Threading.Tasks.Task.Run(() => ExecuteActionsAsync(command));
            var result = await waitResponse;
            this.Enabled = true;

            string message = string.Format("Command : {0}\r\nResult : {1}", command.ToString(), result.CommandResult.ToString()+" "+ result.Description);
            _messageBox.ShowMessage(message);
        }

        #region <Execute Actions>
        private CommandResults ExecuteActionsAsync(RfidCommand command)
        {
            CommandResults result = new CommandResults(command.ToString(), CommandResult.Error);
            TickCounter_.TickCounter tick = new TickCounter_.TickCounter();

            // Action 관련 Flag 초기화
            // Action 실행전 Flag 초기화는 항상 이루어져야한다.

            string LotId = string.Empty;
            string CarrierId = string.Empty;
            _rfidManager.InitAction(_myIndex, LoadingMode);

            tick.SetTickCount(5000);
            while (true)
            {
                System.Threading.Thread.Sleep(100);

                if (tick.IsTickOver(true))
                {
                    result.CommandResult = CommandResult.Timeout;
                    return result;
                }

                result.CommandResult = CommandResult.Proceed;

                if (command == RfidCommand.READ_LOT_ID)
                {
                    result = _rfidManager.ReadLotId(_myIndex, LoadingMode, ref LotId);
                    m_labelLotId.Text = string.Empty;
                    if (result.CommandResult.Equals(CommandResult.Completed))
                    {
                        m_labelLotId.Text = LotId;
                        TotalLotId[CurrentCassetteId] = LotId;
                    }
                }
                else if (command == RfidCommand.READ_CARRIER_ID)
                {
                    result = _rfidManager.ReadCarrierId(_myIndex, LoadingMode, ref CarrierId);
                    m_labelCarrierId.Text = string.Empty;
                    if (result.CommandResult.Equals(CommandResult.Completed))
                    {
                        m_labelCarrierId.Text = CarrierId;
                        TotalCarrierId[CurrentCassetteId] = CarrierId;
                    }
                }
                else if (command == RfidCommand.WRITE_LOT_ID)
                {
                    result = _rfidManager.WriteLotId(_myIndex, LoadingMode, m_labelLotId.Text);
                }
                else if (command == RfidCommand.WRITE_CARRIER_ID)
                {
                    result = _rfidManager.WriteCarrierId(_myIndex, LoadingMode, m_labelCarrierId.Text);
                }
                if (false == result.CommandResult.Equals(CommandResult.Proceed))
                {
                    return result;
                }
            }
        }
        public override void CallFunctionByTimer()
        {
        }
        #endregion </Execute Actions>
        #endregion </Methods>
    }

}
