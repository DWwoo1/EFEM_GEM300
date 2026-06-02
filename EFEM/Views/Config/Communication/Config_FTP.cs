using FrameOfSystem3.Config;
using FrameOfSystem3.Views.Functional;
using Sys3Controls;
using System;
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
    public partial class Config_FTP : UserControlForMainView.CustomView
    {
        public Config_FTP()
        {
            InitializeComponent();

            _messageBoxInstance = Form_MessageBox.GetInstance();
            _keyboardInstance = Form_Keyboard.GetInstance();

            m_FTP = ConfigFTP.GetInstance();
        }

        #region 필드

        const int COLUMN_OF_SERVER_INDEX        = 0;
        const int COLUMN_OF_SERVER_NAME         = 1;
        const int COLUMN_OF_SERVER_ADDRESS      = 2;
        const int COLUMN_OF_SERVER_ID           = 3;
        const int COLUMN_OF_SERVER_PASSWORD     = 4;
        const int COLUMN_OF_SERVER_PORT         = 5;


        ConfigFTP m_FTP = null;

        Form_Keyboard _keyboardInstance = null;
        Form_MessageBox _messageBoxInstance = null;

        int m_nSelectedServerIndex = -1;
        string m_strSelectedFileName = "";
        #endregion  /필드

        #region 상속인터페이스
        protected override void ProcessWhenActivation()
        {
            UpdateGrid();
            UpdateLabel();
        }
        protected override void ProcessWhenDeactivation()
        {

        }
        public override void CallFunctionByTimer()
        {
        }

        #endregion  /상속인터페이스

        void UpdateGrid()
        {
            int[] serviceItemIndexes = m_FTP.GetServerIndexList();

            _dgv_FTP_Server.Rows.Clear();

            for (int i = 0; i < serviceItemIndexes.Length; i++)
            {
                _dgv_FTP_Server.Rows.Add();

                _dgv_FTP_Server[COLUMN_OF_SERVER_INDEX, i].Value = serviceItemIndexes[i];
                _dgv_FTP_Server[COLUMN_OF_SERVER_NAME, i].Value = m_FTP.GetConfigValue(serviceItemIndexes[i], ConfigFTP.EN_CONFIG_ITEM.NAME);
                _dgv_FTP_Server[COLUMN_OF_SERVER_ADDRESS, i].Value = m_FTP.GetConfigValue(serviceItemIndexes[i], ConfigFTP.EN_CONFIG_ITEM.ADDRESS);
                _dgv_FTP_Server[COLUMN_OF_SERVER_ID, i].Value = m_FTP.GetConfigValue(serviceItemIndexes[i], ConfigFTP.EN_CONFIG_ITEM.ID);
                _dgv_FTP_Server[COLUMN_OF_SERVER_PASSWORD, i].Value = m_FTP.GetConfigValue(serviceItemIndexes[i], ConfigFTP.EN_CONFIG_ITEM.PASSWORD);
                _dgv_FTP_Server[COLUMN_OF_SERVER_PORT, i].Value = m_FTP.GetConfigValue(serviceItemIndexes[i], ConfigFTP.EN_CONFIG_ITEM.PORT);
                _dgv_FTP_Server[COLUMN_OF_SERVER_PORT + 1, i].Value = m_FTP.GetConfigValue(serviceItemIndexes[i], ConfigFTP.EN_CONFIG_ITEM.PATH);
            }
        }

        void UpdateLabel()
        {
            _lbl_SelectedIndex.Text = m_nSelectedServerIndex.ToString();
            _lbl_SelectedName.Text = m_FTP.GetConfigValue(m_nSelectedServerIndex, ConfigFTP.EN_CONFIG_ITEM.NAME);
        }
        private void Click_Button(object sender, EventArgs e)
        {
            Control ctr = sender as Control;
            switch (ctr.TabIndex)
            {
                case 0: //ADD SERVER
                    m_FTP.AddSever();
                    UpdateGrid();
                    break;
                case 1: //DOWNLOAD
                    OpenFileDialog fDialog = new OpenFileDialog();

                    switch (fDialog.ShowDialog())
                    {
                        case DialogResult.OK:
                            string SoucerFilName = fDialog.FileName;
                            string FilePath = System.IO.Path.GetDirectoryName(SoucerFilName);
                            string FileName = System.IO.Path.GetFileName(SoucerFilName);

                            m_FTP.Upload(m_nSelectedServerIndex, FilePath, FileName);
                            break;
                        default:
                            break;
                    }
                    break;

                case 2: //Create Folder
                    string strValue = "새폴더";
                    if (_keyboardInstance.CreateForm(strValue))
                    {
                        _keyboardInstance.GetResult(ref strValue);
                        m_FTP.CreateDirectory(m_nSelectedServerIndex, strValue);
                    }
                    break;

                case 3: //Delete
                    m_FTP.Delete(m_nSelectedServerIndex, m_strSelectedFileName);
                    break;

                case 4: // refresh fileList
                    UpdateFileGrid();
                    break;

                case 5: //Download
                    FolderBrowserDialog folderDialog = new FolderBrowserDialog();

                    switch (folderDialog.ShowDialog())
                    {
                        case DialogResult.OK:

                            m_FTP.Download(m_nSelectedServerIndex, folderDialog.SelectedPath, m_strSelectedFileName);

                            System.Diagnostics.Process.Start(folderDialog.SelectedPath);

                            break;
                        default:
                            return;
                    }
                    break;
            }
        }

        private void DoubleClick_ServerGrid(object sender, DataGridViewCellEventArgs e)
        {
               int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            int nServerIndex = (int)_dgv_FTP_Server[0, nRowindex].Value;

            string strValue = "";

            switch(nColumnIndex)
            {
                case COLUMN_OF_SERVER_NAME:
                    if(_keyboardInstance.CreateForm(_dgv_FTP_Server[COLUMN_OF_SERVER_NAME, nRowindex].Value.ToString()))
                    {
                        _keyboardInstance.GetResult(ref strValue);
                        m_FTP.SetConfigValue(nServerIndex, ConfigFTP.EN_CONFIG_ITEM.NAME, strValue);
                    }
                    break;
                case COLUMN_OF_SERVER_ADDRESS:
                    if (_keyboardInstance.CreateForm(_dgv_FTP_Server[COLUMN_OF_SERVER_ADDRESS, nRowindex].Value.ToString()))
                    {
                        _keyboardInstance.GetResult(ref strValue);
                        m_FTP.SetConfigValue(nServerIndex, ConfigFTP.EN_CONFIG_ITEM.ADDRESS, strValue);
                    }
                    break;

                case COLUMN_OF_SERVER_ID:
                    if (_keyboardInstance.CreateForm(_dgv_FTP_Server[COLUMN_OF_SERVER_ID, nRowindex].Value.ToString()))
                    {
                        _keyboardInstance.GetResult(ref strValue);
                        m_FTP.SetConfigValue(nServerIndex, ConfigFTP.EN_CONFIG_ITEM.ID, strValue);
                    }
                    break;

                case COLUMN_OF_SERVER_PASSWORD:
                    if (_keyboardInstance.CreateForm(_dgv_FTP_Server[COLUMN_OF_SERVER_PASSWORD, nRowindex].Value.ToString()))
                    {
                        _keyboardInstance.GetResult(ref strValue);
                        m_FTP.SetConfigValue(nServerIndex, ConfigFTP.EN_CONFIG_ITEM.PASSWORD, strValue);
                    }
                    break;

                case COLUMN_OF_SERVER_PORT:
                    if (Form_Calculator.GetInstance().CreateForm(_dgv_FTP_Server[COLUMN_OF_SERVER_PORT, nRowindex].Value.ToString(), "0", "10000"))
                    {
                        Form_Calculator.GetInstance().GetResult(ref strValue);
                        m_FTP.SetConfigValue(nServerIndex, ConfigFTP.EN_CONFIG_ITEM.PORT, strValue);
                    }
                    break;
            }
            UpdateGrid();
        }

        private void Click_ServerGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            if (nRowindex < 0)
                return; 

            int nServerIndex = (int)_dgv_FTP_Server[COLUMN_OF_SERVER_INDEX, nRowindex].Value;

            m_nSelectedServerIndex = nServerIndex;
            UpdateLabel();
            UpdateFileGrid();
        }

        private void UpdateFileGrid()
        {
            m_FTP.GetFileList(m_nSelectedServerIndex);

            string[] arFileLIst = m_FTP.arServerFileLIst;
            _dgv_FTP_FileList.Rows.Clear();

            _dgv_FTP_FileList.Rows.Add();
            _dgv_FTP_FileList[0, 0].Value = "..";

            for (int i = 1; i <= arFileLIst.Length; i++)
            {
                _dgv_FTP_FileList.Rows.Add();
                _dgv_FTP_FileList[0, i].Value = arFileLIst[i - 1];
            }
        }

        private void DoubleClick_FileGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            string strName = _dgv_FTP_FileList[0, nRowindex].Value.ToString();

            if(strName == "..")
            {
                string CurrentPath = m_FTP.GetConfigValue(m_nSelectedServerIndex, ConfigFTP.EN_CONFIG_ITEM.PATH);
                string[] arDirectoryList = CurrentPath.Split('/');
                string Path = "";
                for (int nIndex = 0; nIndex < arDirectoryList.Length - 1; nIndex++)
                {
                    if (arDirectoryList[nIndex] != "")
                        Path += "/" + arDirectoryList[nIndex];
                }
                m_FTP.SetConfigValue(m_nSelectedServerIndex, ConfigFTP.EN_CONFIG_ITEM.PATH, Path);
                UpdateFileGrid();
                UpdateGrid();
            }
            else if(strName.Split('.').Length == 1) //directory
            {
                string Path = m_FTP.GetConfigValue(m_nSelectedServerIndex, ConfigFTP.EN_CONFIG_ITEM.PATH) + "/" + strName;
                m_FTP.SetConfigValue(m_nSelectedServerIndex, ConfigFTP.EN_CONFIG_ITEM.PATH, Path);
                UpdateFileGrid();
                UpdateGrid();
            }
        }
        private void Click_FileGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            m_strSelectedFileName = _dgv_FTP_FileList[0, nRowindex].Value.ToString();
        }
    }
}
