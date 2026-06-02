using FrameOfSystem3.Config;
using FrameOfSystem3.Views.Functional;
using Sys3Controls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCFManager_;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_WCF : UserControlForMainView.CustomView
    {
        public Config_WCF()
        {
            InitializeComponent();

            _messageBoxInstance = Form_MessageBox.GetInstance();
            _keyboardInstance = Form_Keyboard.GetInstance();

            _wcfConfigInstance = ConfigWCF.GetInstance();

            _logFinder = new Timer();
            _logFinder.Interval = 100;
            _logFinder.Tick += LogFinder_Tick;
            _logFinder.Start();

            //_wcfConfigInstance.SetLogEventHandlerForUI(new Action<string>(GetLog));
        }

        #region 필드

        const int COLUMN_OF_SERVICE_INDEX   = 0;
        const int COLUMN_OF_SERVICE_ENABLE  = 1;
        const int COLUMN_OF_SERVICE_NAME    = 2;
        const int COLUMN_OF_SERVICE_IP      = 3;
        const int COLUMN_OF_SERVICE_PORT    = 4;
        const int COLUMN_OF_SERVICE_STATE    = 5;

        const int COLUMN_OF_CLIENT_INDEX = 0;
        const int COLUMN_OF_CLIENT_ENABLE = 1;
        const int COLUMN_OF_CLIENT_NAME = 2;
        const int COLUMN_OF_CLIENT_IP = 3;
        const int COLUMN_OF_CLIENT_PORT = 4;
        const int COLUMN_OF_CLIENT_STATE = 5;
        const int COLUMN_OF_CLIENT_SERVICE_ITEM = 6;

        const int HEIGHT_OF_ROWS = 30;
        const int SELECT_NONE = -1;

        readonly int INVALID_ITEM_INDEX = -1;
        readonly Color _clrTrue = Color.DodgerBlue;
        readonly Color _clrFalse = Color.White;

        // WCFManager _wcfConfigInstance = null;
        ConfigWCF _wcfConfigInstance = null;
        ItemType _selectedItemType = ItemType.Service;
        int _selectedGridViewRowIndex = -1;

        //Action<string> _updateLog = null;

        Form_Keyboard _keyboardInstance = null;
        Form_MessageBox _messageBoxInstance = null;
        int _confirmPort = 0;
        Timer _logFinder = null;

        ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();

        #endregion  /필드

        #region 상속인터페이스
        protected override void ProcessWhenActivation()
        {
            InitGrid();
            _lbl_Input_SelectedItem_ConfirmPort.Text = _confirmPort.ToString();

            //if (_selectedItemType == ItemType.Service)
            //{

            //}

        }
        protected override void ProcessWhenDeactivation()
        {

        }
        public override void CallFunctionByTimer()
        {
            UpdateState();
        }

        #endregion  /상속인터페이스

        void InitGrid()
        {
            int[] serviceItemIndexes = null;
            int[] clientItemIndexes = null;

            _wcfConfigInstance.GetIndexListOfServiceItem(ref serviceItemIndexes);
            _wcfConfigInstance.GetIndexListOfClientItem(ref clientItemIndexes);

            int serviceItemCount = serviceItemIndexes == null ? 0 : serviceItemIndexes.Length;
            int clientItemCount = clientItemIndexes == null ? 0 : clientItemIndexes.Length;

            _dgv_WCF_Service.Rows.Clear();
            _dgv_WCF_Client.Rows.Clear();

            for(int i = 0; i < serviceItemCount; i++)
            {
                AddServiceItemOnGridView(serviceItemIndexes[i]);
            }

            for(int i = 0; i < clientItemCount; i++)
            {
                AddClientItemOnGridView(clientItemIndexes[i]);
            }
        }

        int AddServiceItemOnGridView(int itemIndex)
        {
            int gridRowIndex = _dgv_WCF_Service.Rows.Count;
            _dgv_WCF_Service.Rows.Add();

			//int nValue = -1;		// 미사용?
            bool bEnable = false;
            string strValue = string.Empty;

            _dgv_WCF_Service.Rows[gridRowIndex].Height = HEIGHT_OF_ROWS;
            _dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, gridRowIndex].Value = itemIndex;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.Enable, ref bEnable);
            _dgv_WCF_Service[COLUMN_OF_SERVICE_ENABLE, gridRowIndex].Style.BackColor = bEnable ? _clrTrue : _clrFalse;
            _dgv_WCF_Service[COLUMN_OF_SERVICE_ENABLE, gridRowIndex].Style.SelectionBackColor = bEnable ? _clrTrue : _clrFalse;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.Name, ref strValue);
            _dgv_WCF_Service[COLUMN_OF_SERVICE_NAME, gridRowIndex].Value = strValue;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.ServiceIP, ref strValue);
            _dgv_WCF_Service[COLUMN_OF_SERVICE_IP, gridRowIndex].Value = strValue;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.ServicePort, ref strValue);
            _dgv_WCF_Service[COLUMN_OF_SERVICE_PORT, gridRowIndex].Value = strValue;

            return gridRowIndex;
        }
        int AddClientItemOnGridView(int itemIndex)
        {
            int gridRowIndex = _dgv_WCF_Client.Rows.Count;
            _dgv_WCF_Client.Rows.Add();

			//int nValue = -1;		// 미사용?
            bool bEnable = false;
            string strValue = string.Empty;

            _dgv_WCF_Client.Rows[gridRowIndex].Height = HEIGHT_OF_ROWS;
            _dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, gridRowIndex].Value = itemIndex;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.Enable, ref bEnable);
            _dgv_WCF_Client[COLUMN_OF_CLIENT_ENABLE, gridRowIndex].Style.BackColor = bEnable ? _clrTrue : _clrFalse;
            _dgv_WCF_Client[COLUMN_OF_CLIENT_ENABLE, gridRowIndex].Style.SelectionBackColor = bEnable ? _clrTrue : _clrFalse;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.Name, ref strValue);
            _dgv_WCF_Client[COLUMN_OF_CLIENT_NAME, gridRowIndex].Value = strValue;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.TargetServiceIP, ref strValue);
            _dgv_WCF_Client[COLUMN_OF_CLIENT_IP, gridRowIndex].Value = strValue;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.TargetServicePort, ref strValue);
            _dgv_WCF_Client[COLUMN_OF_CLIENT_PORT, gridRowIndex].Value = strValue;

            _wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.RelatedServiceIndex, ref strValue);
            _dgv_WCF_Client[COLUMN_OF_CLIENT_SERVICE_ITEM, gridRowIndex].Value = strValue;

            return gridRowIndex;
        }
        void UpdateState()
        {
            int serviceItemCount = _dgv_WCF_Service.Rows.Count;
            int clientItemCount = _dgv_WCF_Client.Rows.Count;

            for(int i = 0; i < serviceItemCount; i++)
            {
                int itemIndex = 0;
                if(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, i].Value != null
                    && int.TryParse(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, i].Value.ToString(), out itemIndex))
                {
                    _dgv_WCF_Service[COLUMN_OF_SERVICE_STATE, i].Value
                        = _wcfConfigInstance.GetServiceHostCommunicataionState(itemIndex);
                }
            }
            for (int i = 0; i < clientItemCount; i++)
            {
                int itemIndex = 0;
                
                if (_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, i].Value != null
                    && int.TryParse(_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, i].Value.ToString(), out itemIndex))
                {
                    _dgv_WCF_Client[COLUMN_OF_CLIENT_STATE, i].Value
                        = _wcfConfigInstance.GetConnectionStateWithService(itemIndex);
                }
            }
        }

        void Click_DataGridViewForService(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex < 0 || rowIndex >= _dgv_WCF_Service.RowCount)
            { 
                return;
            }

            SetSelectedServiceItem(rowIndex);
        }
        void ReleaseSelectedRow_DataGridViewForService(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _dgv_WCF_Service.RowCount)
            {
                return;
            }
            _dgv_WCF_Service.Rows[rowIndex].Selected = false;
        }

        void UpdateWCFConfigurationControlsForService(int itemIndex)
        {
            string strValue = string.Empty;

            _lbl_Input_SelectedItem_Index.Text = itemIndex.ToString();
            _lbl_Input_SelectedItem_Type.Text = ItemType.Service.ToString();

            _lbl_SelectedItem_IP.Text = "SERVICE IP";
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.Name, ref strValue))
            {
                _lbl_Input_SelectedItem_Name.Text = strValue;
            }
            _lbl_SelectedItem_Port.Text = "SERVICE PORT";
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.ServiceIP, ref strValue))
            {
                _lbl_Input_SelectedItem_IP.Text = strValue;
            }
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForService.ServicePort, ref strValue))
            {
                _lbl_Input_SelectedItem_Port.Text = strValue;
            }

            bool isService = true;
            _lbl_SelectedItem_ServiceItemIndex.Visible = !isService;
            _lbl_Input_SelectedItem_ServiceItemIndex.Visible = !isService;
            _btn_CheckReceivedRequest.Visible = isService;
            _lbl_SelectedItem_ConfirmPort.Visible = isService;
            _lbl_Input_SelectedItem_ConfirmPort.Visible = isService;
            _btn_CheckReceivedRequest_SpecificPort.Visible = isService;

            _btn_Open.Enabled = isService;
            _btn_Close.Enabled = isService;
            _btn_Connect.Enabled = !isService;
            _btn_Disconnect.Enabled = !isService;

            _btn_Request.Enabled = !isService;
            _btn_ConfirmAck.Enabled = !isService;
        }

        void Click_DataGridViewForClient(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex < 0 || rowIndex >= _dgv_WCF_Client.RowCount)
            {
                return;
            }

            SetSelectedClientItem(rowIndex);
        }
        void ReleaseSelectedRow_DataGridViewForClient(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _dgv_WCF_Client.RowCount)
            {
                return;
            }
            _dgv_WCF_Client.Rows[rowIndex].Selected = false;
        }

        void UpdateWCFConfigurationControlsForClient(int itemIndex)
        {
            string strValue = string.Empty;

            _lbl_Input_SelectedItem_Index.Text = itemIndex.ToString();
            _lbl_Input_SelectedItem_Type.Text = ItemType.Service.ToString();

            _lbl_SelectedItem_IP.Text = "TARGET IP";
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.Name, ref strValue))
            {
                _lbl_Input_SelectedItem_Name.Text = strValue;
            }
            _lbl_SelectedItem_Port.Text = "TARGET PORT";
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.TargetServiceIP, ref strValue))
            {
                _lbl_Input_SelectedItem_IP.Text = strValue;
            }
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.TargetServicePort, ref strValue))
            {
                _lbl_Input_SelectedItem_Port.Text = strValue;
            }
            if (_wcfConfigInstance.GetParameter(itemIndex, ParameterTypeForClient.RelatedServiceIndex, ref strValue))
            {
                _lbl_Input_SelectedItem_ServiceItemIndex.Text = strValue;
            }

            bool isService = false;
            _lbl_SelectedItem_ServiceItemIndex.Visible = !isService;
            _lbl_Input_SelectedItem_ServiceItemIndex.Visible = !isService;
            _btn_CheckReceivedRequest.Visible = isService;
            _lbl_SelectedItem_ConfirmPort.Visible = isService;
            _lbl_Input_SelectedItem_ConfirmPort.Visible = isService;
            _btn_CheckReceivedRequest_SpecificPort.Visible = isService;

            _btn_Open.Enabled = isService;
            _btn_Close.Enabled = isService;
            _btn_Connect.Enabled = !isService;
            _btn_Disconnect.Enabled = !isService;

            _btn_Request.Enabled = !isService;
            _btn_ConfirmAck.Enabled = !isService;
        }

        void SetSelectedServiceItem(int gridRowIndex)
        {
            int itemIndex = 0;

            if (int.TryParse(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, gridRowIndex].Value.ToString(), out itemIndex))
            {
                ReleaseSelectedRow_DataGridViewForClient(_selectedGridViewRowIndex);
                UpdateWCFConfigurationControlsForService(itemIndex);
                _selectedItemType = ItemType.Service;
                _selectedGridViewRowIndex = gridRowIndex;
            }
            else
            {
                _dgv_WCF_Service.Rows[gridRowIndex].Selected = false;
            }
        }

        void SetSelectedClientItem(int gridRowIndex)
        {
            int itemIndex = 0;

            if (int.TryParse(_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, gridRowIndex].Value.ToString(), out itemIndex))
            {
                ReleaseSelectedRow_DataGridViewForService(_selectedGridViewRowIndex);
                UpdateWCFConfigurationControlsForClient(itemIndex);
                _selectedItemType = ItemType.Client;
                _selectedGridViewRowIndex = gridRowIndex;
            }
            else
            {
                _dgv_WCF_Client.Rows[gridRowIndex].Selected = false;
            }
        }

        bool GetSelectedServiceItemIndex(ref int itemIndex)
        {
            if (_selectedGridViewRowIndex == SELECT_NONE)
            {
                return false;
            }

            if (_selectedItemType == ItemType.Service)
            {
                if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Service.RowCount)
                {
                    return false;
                }

                if (int.TryParse(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, _selectedGridViewRowIndex].Value.ToString()
                    , out itemIndex))
                {
                    return true;
                }
            }
            return false;
        }

        bool GetSelectedClientItemIndex(ref int itemIndex)
        {
            if (_selectedGridViewRowIndex == SELECT_NONE)
            {
                return false;
            }

            if (_selectedItemType == ItemType.Client)
            {
                if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Client.RowCount)
                {
                    return false;
                }

                if (int.TryParse(_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, _selectedGridViewRowIndex].Value.ToString()
                    , out itemIndex))
                {
                    return true;
                }
            }
            return false;
        }

        #region UIEvent_Config

        void Clicked_ItemName(object sender, EventArgs e)
        {
            Form_Keyboard keyboard = Form_Keyboard.GetInstance();

            int itemIndex = SELECT_NONE;

            if (_selectedItemType == ItemType.Service)
            {
                if (false == GetSelectedServiceItemIndex(ref itemIndex))
                {
                    return;
                }
            }
            else
            {
                if (false == GetSelectedClientItemIndex(ref itemIndex))
                {
                    return;
                }
            }

            string itemName = string.Empty;
            if (keyboard.CreateForm())
            {
                keyboard.GetResult(ref itemName);

                if (_selectedItemType == ItemType.Service)
                {
                    if (_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForService.Name, itemName))
                    {
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_NAME, _selectedGridViewRowIndex].Value = itemName;
                        _lbl_Input_SelectedItem_Name.Text = itemName;
                    }
                }
                else
                {
                    if (_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForClient.Name, itemName))
                    {
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_NAME, _selectedGridViewRowIndex].Value = itemName;
                        _lbl_Input_SelectedItem_Name.Text = itemName;
                    }
                }
            }
        }
        void Clicked_Ip(object sender, EventArgs e)
        {
            Form_Keyboard keyboard = Form_Keyboard.GetInstance();

            int itemIndex = SELECT_NONE;

            if(_selectedItemType == ItemType.Service)
            {
                if(false == GetSelectedServiceItemIndex(ref itemIndex))
                {
                    return;
                }
            }
            else
            {
                if(false == GetSelectedClientItemIndex(ref itemIndex))
                {
                    return;
                }
            }

            string strIpAddress = string.Empty;
            if(keyboard.CreateForm())
            {
                keyboard.GetResult(ref strIpAddress);

                if (_selectedItemType == ItemType.Service)
                {
                    if(_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForService.ServiceIP, strIpAddress))
                    {
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_IP, _selectedGridViewRowIndex].Value = strIpAddress;
                        _lbl_Input_SelectedItem_IP.Text = strIpAddress;
                    }
                }
                else
                {
                    if(_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForClient.TargetServiceIP, strIpAddress))
                    {
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_IP, _selectedGridViewRowIndex].Value = strIpAddress;
                        _lbl_Input_SelectedItem_IP.Text = strIpAddress;
                    }
                }
            }
        }
        void Clicked_Port(object sender, EventArgs e)
        {
            Form_Calculator calculator = Form_Calculator.GetInstance();

            int itemIndex = SELECT_NONE;

            if (_selectedItemType == ItemType.Service)
            {
                if (false == GetSelectedServiceItemIndex(ref itemIndex))
                {
                    return;
                }
            }
            else
            {
                if (false == GetSelectedClientItemIndex(ref itemIndex))
                {
                    return;
                }
            }

            int port = SELECT_NONE;

            if(calculator.CreateForm("","1","65535"))
            {
                calculator.GetResult(ref port);

                if (_selectedItemType == ItemType.Service)
                {
                    if(_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForService.ServicePort, port))
                    {
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_PORT, _selectedGridViewRowIndex].Value = port.ToString();
                        UpdateWCFConfigurationControlsForService(itemIndex);
                    }
                }
                else
                {
                    if(_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForClient.TargetServicePort, port))
                    {
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_PORT, _selectedGridViewRowIndex].Value = port.ToString();
                        UpdateWCFConfigurationControlsForClient(itemIndex);
                    }
                }
            }
        }
        void Clicked_ServiceItem(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if(GetSelectedClientItemIndex(ref itemIndex))
            {
                int[] serviceItemIndexes = null;

                if(_wcfConfigInstance.GetIndexListOfServiceItem(ref serviceItemIndexes))
                {
                    Form_SelectionList formSelectionList = Form_SelectionList.GetInstance();
                    if (formSelectionList.CreateForm("Service Items"
                        , Array.ConvertAll(serviceItemIndexes, x => x.ToString())
                        , ""))
                    {
                        int selectedIndex = SELECT_NONE;
                        formSelectionList.GetResult(ref selectedIndex);

                        if(_wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForClient.RelatedServiceIndex, selectedIndex))
                        {
                            string strSelectedIndex = selectedIndex.ToString();
                            _dgv_WCF_Client[COLUMN_OF_CLIENT_SERVICE_ITEM, _selectedGridViewRowIndex].Value = strSelectedIndex;
                            _lbl_Input_SelectedItem_ServiceItemIndex.Text = strSelectedIndex;
                        }
                    }
                }
            }
        }
        void Clicked_ConfirmPort(object sender, EventArgs e)
        {
            Form_Calculator calculator = Form_Calculator.GetInstance();
            
            if (calculator.CreateForm("", "1", "65535"))
            {
                calculator.GetResult(ref _confirmPort);
                _lbl_Input_SelectedItem_ConfirmPort.Text = _confirmPort.ToString();
            }
        }

        void Clicked_CheckReceivedRequest(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if(GetSelectedServiceItemIndex(ref itemIndex))
            {
                int requestCount = 0;
                string[] requestPorts = null;
                string[] requestTitles = null;

                if(_wcfConfigInstance.GetRequestData(itemIndex, ref requestCount, ref requestPorts, ref requestTitles))
                {
                    for(int i = 0; i < requestCount; i++)
                    {
                        string requestTitle = string.Empty;
                        string[] keys = null;
                        string[] values = null;

                        if(_wcfConfigInstance.GetRequestData(itemIndex, requestPorts[i], ref requestTitle, ref keys, ref values))
                        {
                            StringBuilder sb = new StringBuilder(1023);

                            sb.AppendFormat("Port : {0}\r\n", requestPorts[i]);
                            sb.AppendFormat("Title : {0}\r\n", requestTitles[i]);

                            int dataCount = keys == null ? 0 : keys.Length;
                            for (int dataIndex = 0; dataIndex < dataCount; dataIndex++)
                            {
                                sb.AppendFormat("Data{0} Key : {1} Value : {2}\r\n", dataIndex, keys[dataIndex], values[dataIndex]);
                            }
                            _messageBoxInstance.ShowMessage(sb.ToString(), "Confirm Request");
                            sb.Clear();
                        }
                    }
                }
            }
        }
        void Clicked_CheckReceivedRequest_SpecificPort(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if (GetSelectedServiceItemIndex(ref itemIndex))
            {
                string[] keys = null;
                string[] values = null;
                string requestTitle = string.Empty;

                string port  = string.Empty;

                port = _lbl_Input_SelectedItem_ConfirmPort.Text;

                if (_wcfConfigInstance.GetRequestData(itemIndex, port, ref requestTitle, ref keys, ref values))
                {
                    StringBuilder sb = new StringBuilder(1023);

                    sb.AppendFormat("Port : {0}\r\n", port);
                    sb.AppendFormat("Title : {0}\r\n", requestTitle);

                    int dataCount = keys == null ? 0 : keys.Length;
                    for (int dataIndex = 0; dataIndex < dataCount; dataIndex++)
                    {
                        sb.AppendFormat("Data{0} Key : {1} Value : {2}\r\n", dataIndex, keys[dataIndex], values[dataIndex]);
                    }
                    _messageBoxInstance.ShowMessage(sb.ToString(), "Confirm Request");
                    sb.Clear();
                }
            }
        }
        void Clicked_Enable(object sender, EventArgs e)
        {
            if (_selectedGridViewRowIndex == SELECT_NONE)
            {
                return;
            }

            int itemIndex;

            if (_selectedItemType == ItemType.Service)
            {
                if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Service.RowCount)
                {
                    return;
                }

                if (int.TryParse(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, _selectedGridViewRowIndex].Value.ToString()
                    , out itemIndex))
                {
                    if(true == _wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForService.Enable, true))
                    {
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_ENABLE, _selectedGridViewRowIndex].Style.BackColor = _clrTrue;
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_ENABLE, _selectedGridViewRowIndex].Style.SelectionBackColor = _clrTrue;
                    }
                }
            }
            else
            {
                if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Client.RowCount)
                {
                    return;
                }

                if (int.TryParse(_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, _selectedGridViewRowIndex].Value.ToString()
                    , out itemIndex))
                {
                    if (true == _wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForClient.Enable, true))
                    {
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_ENABLE, _selectedGridViewRowIndex].Style.BackColor = _clrTrue;
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_ENABLE, _selectedGridViewRowIndex].Style.SelectionBackColor = _clrTrue;
                    }
                }
            }
        }
        void Clicked_Disable(object sender, EventArgs e)
        {
            if (_selectedGridViewRowIndex == SELECT_NONE)
            {
                return;
            }

            int itemIndex;

            if (_selectedItemType == ItemType.Service)
            {
                if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Service.RowCount)
                {
                    return;
                }

                if (int.TryParse(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, _selectedGridViewRowIndex].Value.ToString()
                    , out itemIndex))
                {
                    if (true == _wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForService.Enable, false))
                    {
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_ENABLE, _selectedGridViewRowIndex].Style.BackColor = _clrFalse;
                        _dgv_WCF_Service[COLUMN_OF_SERVICE_ENABLE, _selectedGridViewRowIndex].Style.SelectionBackColor = _clrFalse;
                    }
                }
            }
            else
            {
                if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Client.RowCount)
                {
                    return;
                }

                if (int.TryParse(_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, _selectedGridViewRowIndex].Value.ToString()
                    , out itemIndex))
                {
                    if (true == _wcfConfigInstance.SetParameter(itemIndex, ParameterTypeForClient.Enable, false))
                    {
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_ENABLE, _selectedGridViewRowIndex].Style.BackColor = _clrFalse;
                        _dgv_WCF_Client[COLUMN_OF_CLIENT_ENABLE, _selectedGridViewRowIndex].Style.SelectionBackColor = _clrFalse;
                    }
                }
            }
        }

        void Clicked_Add_Service(object sender, EventArgs e)
        {
            int newItemIndex = _wcfConfigInstance.AddServiceItem();

            if (INVALID_ITEM_INDEX != newItemIndex)
            {
                _selectedGridViewRowIndex = AddServiceItemOnGridView(newItemIndex);
                SetSelectedServiceItem(_selectedGridViewRowIndex);
            }
        }
        void Clicked_Remove_Service(object sender, EventArgs e)
        {
            if (SELECT_NONE == _selectedGridViewRowIndex || _selectedItemType != ItemType.Service)
            {
                return;
            }

            if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Service.RowCount)
            {
                return;
            }

            int itemIndex;

            if (int.TryParse(_dgv_WCF_Service[COLUMN_OF_SERVICE_INDEX, _selectedGridViewRowIndex].Value.ToString()
                , out itemIndex))
            {
                if(true == _wcfConfigInstance.RemoveServiceItem(itemIndex))
                {
                    _dgv_WCF_Service.Rows.RemoveAt(_selectedGridViewRowIndex);
                    _selectedGridViewRowIndex = SELECT_NONE;
                }
            }
        }

        void Clicked_Add_Client(object sender, EventArgs e)
        {
            int newItemIndex = _wcfConfigInstance.AddClientItem();

            if (INVALID_ITEM_INDEX != newItemIndex)
            {
                _selectedGridViewRowIndex = AddClientItemOnGridView(newItemIndex);
                SetSelectedClientItem(_selectedGridViewRowIndex);
            }
        }
        void Clicked_Remove_Client(object sender, EventArgs e)
        {
            if (SELECT_NONE == _selectedGridViewRowIndex || _selectedItemType != ItemType.Client)
            {
                return;
            }

            if (_selectedGridViewRowIndex < 0 || _selectedGridViewRowIndex >= _dgv_WCF_Client.RowCount)
            {
                return;
            }

            int itemIndex;

            if (int.TryParse(_dgv_WCF_Client[COLUMN_OF_CLIENT_INDEX, _selectedGridViewRowIndex].Value.ToString()
                , out itemIndex))
            {
                if (true == _wcfConfigInstance.RemoveClientItem(itemIndex))
                {
                    _dgv_WCF_Client.Rows.RemoveAt(_selectedGridViewRowIndex);
                    _selectedGridViewRowIndex = SELECT_NONE;
                }
            }
        }
        #endregion  /UIEvent_Config

        #region Log
        void GetLog(string log)
        {
            _logQueue.Enqueue(log);
        }

        void LogFinder_Tick(object sender, EventArgs e)
        {
            string log = string.Empty;
            while (_logQueue.TryPeek(out log))
            {
                if (true == _logQueue.TryDequeue(out log))
                {
                    UpdateLogView(log);
                }
            }
        }
        #endregion  /Log

        #region UIEvent_Demo
        void UpdateLogView(string msg)
        {
            if (_textBoxForLog.InvokeRequired)
            {
                Action<string> action = new Action<string>(UpdateLogView);
                _textBoxForLog.BeginInvoke(action, new object[] { msg });
            }
            else
            {
                if (false == _textBoxForLog.IsDisposed)
                {
                    if(_textBoxForLog.Text.Length > 3000)
                    {
                        _textBoxForLog.Text = _textBoxForLog.Text.Substring(500, _textBoxForLog.Text.Length - 501);
                    }

                    _textBoxForLog.AppendText(msg);
                    _textBoxForLog.SelectionStart = _textBoxForLog.Text.Length;
                    _textBoxForLog.ScrollToCaret();
                }
            }
        }
        void Clicked_Title(object sender, EventArgs e)
        {
            Sys3Label label = _lbl_Input_Title;

            if (_keyboardInstance.CreateForm(label.Text))
            {
                string strResult = string.Empty;
                _keyboardInstance.GetResult(ref strResult);
                label.Text = strResult;
            }
        }
        void Clicked_Data1_Key(object sender, EventArgs e)
        {
            Sys3Label label = _lbl_Input_Data1_Key;
            
            if (_keyboardInstance.CreateForm(label.Text))
            {
                string strResult = string.Empty;
                _keyboardInstance.GetResult(ref strResult);
                label.Text = strResult;
            }
        }
        void Clicked_Data1_Value(object sender, EventArgs e)
        {
            Sys3Label label = _lbl_Input_Data1_Value;

            if (_keyboardInstance.CreateForm(label.Text))
            {
                string strResult = string.Empty;
                _keyboardInstance.GetResult(ref strResult);
                label.Text = strResult;
            }
        }
        void Clicked_Data2_Key(object sender, EventArgs e)
        {
            Sys3Label label = _lbl_Input_Data2_Key;

            if (_keyboardInstance.CreateForm(label.Text))
            {
                string strResult = string.Empty;
                _keyboardInstance.GetResult(ref strResult);
                label.Text = strResult;
            }
        }
        void Clicked_Data2_Value(object sender, EventArgs e)
        {
            Sys3Label label = _lbl_Input_Data2_Value;

            if (_keyboardInstance.CreateForm(label.Text))
            {
                string strResult = string.Empty;
                _keyboardInstance.GetResult(ref strResult);
                label.Text = strResult;
            }
        }

        void Clicked_Request(object sender, EventArgs e)
        {
            int itemIndex = -1;

            if(GetSelectedClientItemIndex(ref itemIndex))
            {
                string title = _lbl_Input_Title.Text;
                
                List<string> keys = new List<string>();
                List<string> values = new List<string>();

                if (false == string.IsNullOrEmpty(_lbl_Input_Data1_Key.Text)
                    && false == string.IsNullOrEmpty(_lbl_Input_Data1_Value.Text))
                {
                    keys.Add(_lbl_Input_Data1_Key.Text);
                    values.Add(_lbl_Input_Data1_Value.Text);

                    if (false == string.IsNullOrEmpty(_lbl_Input_Data2_Key.Text)
                    && false == string.IsNullOrEmpty(_lbl_Input_Data2_Value.Text))
                    {
                        keys.Add(_lbl_Input_Data2_Key.Text);
                        values.Add(_lbl_Input_Data2_Value.Text);
                    }
                }
                _wcfConfigInstance.RequestDataToService(itemIndex, title, keys.ToArray(), values.ToArray());
            }
        }
        void Clicked_ConfirmAck(object sender, EventArgs e)
        {
            int itemIndex = -1;

            if (GetSelectedClientItemIndex(ref itemIndex))
            {
                string title = _lbl_Input_Title.Text;

                string result = string.Empty;
                string description = string.Empty;
                StringBuilder sb = new StringBuilder(1023);

                if (true == _wcfConfigInstance.GetResponseData(itemIndex, title, ref result, ref description))
                {
                    sb.AppendFormat("Title : {0}\r\n", title);
                    sb.AppendFormat("Result : {0}\r\n", result);
                    sb.AppendFormat("Description : {0}", description);

                    _messageBoxInstance.ShowMessage(sb.ToString(), "Confirm Ack");
                }
                else
                {
                    sb.AppendFormat("Title : {0}\r\n", title);
                    sb.AppendFormat("ACK 없음");

                    _messageBoxInstance.ShowMessage(sb.ToString(), "Confirm Ack");
                }
            }
        }
        void Clicked_ClearMessage(object  sender, EventArgs e)
        {
            _textBoxForLog.Clear();
        }
        void Clicked_MakeDummyData(object sender, EventArgs e)
        {
            _lbl_Input_Title.Text = "RESPONSE_TEST";
            _lbl_Input_Data1_Key.Text = "Key1";
            _lbl_Input_Data1_Value.Text = "Data1";
            _lbl_Input_Data2_Key.Text = "Key2";
            _lbl_Input_Data2_Value.Text = "Data2";
        }
        void Clicked_Open(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if(GetSelectedServiceItemIndex(ref itemIndex))
            {
                _wcfConfigInstance.OpenServiceHost(itemIndex);
            }
        }
        void Clicked_Close(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if (GetSelectedServiceItemIndex(ref itemIndex))
            {
                _wcfConfigInstance.CloseServiceHost(itemIndex);
            }
        }
        void Clicked_Connect(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if(GetSelectedClientItemIndex(ref itemIndex))
            {
                _wcfConfigInstance.ConnectToService(itemIndex);
            }
        }
        void Clicked_Disconnect(object sender, EventArgs e)
        {
            int itemIndex = SELECT_NONE;
            if (GetSelectedClientItemIndex(ref itemIndex))
            {
                _wcfConfigInstance.DisconnectFromService(itemIndex);
            }
        }

        #endregion  /UIEvent_Demo
    }
}
