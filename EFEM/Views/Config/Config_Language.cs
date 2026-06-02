using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Config;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Language : UserControlForMainView.CustomView
    {
        public Config_Language()
        {
            InitializeComponent();

			m_dgViewSelected		= m_dgViewUI;
			m_btnSelectedTab		= m_btnTabUI;

			#region Insatnce
			m_InstanceOfLanguage					= FrameOfSystem3.Config.ConfigLanguage.GetInstance();
			m_InstanceOfKeyborad					= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfMessageBox					= Functional.Form_MessageBox.GetInstance();
			#endregion

            _tableLayoutPanel_Contents.RowStyles[0].Height = _tableLayoutPanel_Contents.Height;
            _tableLayoutPanel_Contents.RowStyles[1].Height = 0;
            _tableLayoutPanel_Contents.RowStyles[2].Height = 0;

			m_lblOther.Text = Work.AppConfigManager.Instance.Language.ToString();
            m_dgViewUI.Columns[COLUMN_INDEX_OF_OTHERLANGUAGE].HeaderText = Work.AppConfigManager.Instance.Language.ToString();
            m_dgViewMessage.Columns[COLUMN_INDEX_OF_OTHERLANGUAGE].HeaderText = Work.AppConfigManager.Instance.Language.ToString();
            m_dgViewLog.Columns[COLUMN_INDEX_OF_OTHERLANGUAGE].HeaderText = Work.AppConfigManager.Instance.Language.ToString(); 
        }

		#region 열거형
		private enum EN_GRID_TAB
		{
			GUI,
			MESSAGE,
			LOG,
		}
		#endregion

		#region 상수
		private const int HEIGHT_OF_ROWS			= 30;
		private const int SELECT_NONE				= -1;

		#region GUI INDEX
		private const int INDEX_OF_GUI_BUTTON		= 0;
		private const int INDEX_OF_MESSAGE_BUTTON	= 1;
		private const int INDEX_OF_LOG_BUTTON		= 2;

		private const int COLUMN_INDEX_OF_INDEX		= 0;
		private const int COLUMN_INDEX_OF_ENGLISH	= 1;
		private const int COLUMN_INDEX_OF_OTHERLANGUAGE	    = 2;
		#endregion

		#endregion

		#region 변수
		private int m_nIndexOfSelectedRows			= -1;
		private int m_nIndexOfSelectedItem			= -1;

		private int m_nSelectedTab					= 0;

		private int[] m_nArrOfWord					= null;
		private int[] m_nArrOfSentence				= null;
		
		private ConfigLanguage.EN_TYPE_FORMAT m_enSelectedType		= ConfigLanguage.EN_TYPE_FORMAT.WORD;
		private Sys3Controls.Sys3button m_btnSelectedTab			= null;
		private DataGridView m_dgViewSelected						= null;

		#region Instance
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;
		Functional.Form_Keyboard m_InstanceOfKeyborad				= null;
		FrameOfSystem3.Config.ConfigLanguage m_InstanceOfLanguage	= null;
		#endregion
		
		#endregion

		#region 상속 인터페이스
		/// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenActivation()
        {
			UpdateGridForWord();

			UpdateGridForSentence();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public override void CallFunctionByTimer()
        {
		}		
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.19 by twkang [ADD] Word Grid 를 업데이트한다.
		/// </summary>
		private void UpdateGridForWord()
		{
			if(false == m_InstanceOfLanguage.GetListOfItem(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD, ref m_nArrOfWord))
			{
				return;
			}

			int nCountOfItem = m_nArrOfWord.Length;			
			m_dgViewUI.Rows.Clear();

			for(int i = 0; i < nCountOfItem; ++i)
			{
				AddItemOnGrid(EN_GRID_TAB.GUI, m_nArrOfWord[i]);
			}
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] Message Grid 를 업데이트한다.
		/// </summary>
		private void UpdateGridForSentence()
		{
			if (false == m_InstanceOfLanguage.GetListOfItem(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE, ref m_nArrOfSentence))
			{
				return;
			}

			int nCountOfItem = m_nArrOfSentence.Length;
			m_dgViewMessage.Rows.Clear();

			for (int i = 0; i < nCountOfItem; ++i)
			{
				AddItemOnGrid(EN_GRID_TAB.MESSAGE, m_nArrOfSentence[i]);
			}
		}
		/// <summary>
		/// 2020.10.07 by twkang [ADD] 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(EN_GRID_TAB enTab, int nIndex)
		{
			DataGridView dgViewList		= null;
			string strValue				= string.Empty;

			FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT enFormat = FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD;
			switch(enTab)
			{
				case EN_GRID_TAB.GUI:
					dgViewList			= m_dgViewUI;
					enFormat			= FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD;
					break;
				case EN_GRID_TAB.MESSAGE:
					dgViewList			= m_dgViewMessage;
					enFormat			= FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE;
					break;
				case EN_GRID_TAB.LOG:
					return;
			}
			
			int nIndexOfRow			= dgViewList.Rows.Count;
			dgViewList.Rows.Add();

			dgViewList.Rows[nIndexOfRow].Height = HEIGHT_OF_ROWS;
			dgViewList[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value = nIndex.ToString();
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor = Color.Black;

			if (false == m_InstanceOfLanguage.GetParameter(enFormat
				, nIndex
				, FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH
				, ref strValue))
			{
				strValue = string.Empty;
			}
			dgViewList[COLUMN_INDEX_OF_ENGLISH, nIndexOfRow].Value = strValue;

			if (false == m_InstanceOfLanguage.GetParameter(enFormat
				, nIndex
                , GetConfigLanguege()
				, ref strValue))
			{
				strValue = string.Empty;
			}
            dgViewList[COLUMN_INDEX_OF_OTHERLANGUAGE, nIndexOfRow].Value = strValue;

		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 선택아이템과 라벨들을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nIndexOfSelectedRows		= -1;
			m_nIndexOfSelectedItem		= -1;

			m_labelOther.Enabled			= false;
			m_labelEng.Enabled			= false;

			m_labelKey.Text             = "";
            m_labelEng.Text             = "";
            m_labelOther.Text             = "";
						
			m_btnRemove.Enabled			= false;
            //m_labelChinese.Text         = "";
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 선택 탭의 아이템을 추가한다.
		/// </summary>
		private void AddItem(int nIndexSelectedTab)
		{
			switch(nIndexSelectedTab)
			{
				case INDEX_OF_GUI_BUTTON:
					m_InstanceOfLanguage.AddLanguage(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD);
					break;
				case INDEX_OF_MESSAGE_BUTTON:
					m_InstanceOfLanguage.AddLanguage(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE);
					break;
				case INDEX_OF_LOG_BUTTON:
					break;
			}
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 라벨들의 값을 설정한다.
		/// </summary>
		private void SetLanguageLabels(ref int nRowIndex)
		{
			m_labelKey.Text = m_dgViewSelected[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString();
			m_labelEng.Text = m_dgViewSelected[COLUMN_INDEX_OF_ENGLISH, nRowIndex].Value.ToString();
            m_labelOther.Text = m_dgViewSelected[COLUMN_INDEX_OF_OTHERLANGUAGE, nRowIndex].Value.ToString();
			
			m_labelOther.Enabled		= true;
			m_labelEng.Enabled		= true;

			switch(m_nSelectedTab)
			{
				case 0: // GUI
					m_btnRemove.Enabled	= true;
					break;
				case 1: //MESSAGE
					m_btnRemove.Enabled	= false;
					break;
				case 2: //LOG
					break;
			}
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		#endregion

		#region UI이벤트
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 탭 전환 버튼을 클릭했을 경우 발생한다.
		/// </summary>
		private void Click_TabButtons(object sender, EventArgs e)
		{
			Control	ctrl	= sender as Control;

			if(ctrl.TabIndex == m_nSelectedTab) return;

			m_btnSelectedTab.ButtonClicked	= false;
			m_dgViewSelected.Visible		= false;
			
			switch(ctrl.TabIndex)
			{
				case 0: // UI
					m_btnSelectedTab    = m_btnTabUI;
                    m_dgViewSelected    = m_dgViewUI;
					m_enSelectedType	= ConfigLanguage.EN_TYPE_FORMAT.WORD;
					UpdateGridForWord();
                    m_btnAdd.Enabled    = true;
                    m_btnRemove.Enabled = true;

                    _tableLayoutPanel_Contents.RowStyles[0].Height = _tableLayoutPanel_Contents.Height;
                    _tableLayoutPanel_Contents.RowStyles[1].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[2].Height = 0;
                    break;
				case 1:	// MESAGE
					m_btnSelectedTab    = m_btnTabMessage;
                    m_dgViewSelected    = m_dgViewMessage;
					m_enSelectedType	= ConfigLanguage.EN_TYPE_FORMAT.SENTENCE;
					UpdateGridForSentence();
                    m_btnAdd.Enabled    = true;
                    m_btnRemove.Enabled = true;

                    _tableLayoutPanel_Contents.RowStyles[0].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[1].Height = _tableLayoutPanel_Contents.Height;
                    _tableLayoutPanel_Contents.RowStyles[2].Height = 0;
                    break;
				case 2:	// LOG
					m_btnSelectedTab    = m_btnTabLog;
                    m_dgViewSelected    = m_dgViewLog;
                    m_btnAdd.Enabled    = false;
                    m_btnRemove.Enabled = false;

                    _tableLayoutPanel_Contents.RowStyles[0].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[1].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[2].Height = _tableLayoutPanel_Contents.Height;
					break;
			}
			m_nSelectedTab				= ctrl.TabIndex;
            m_btnSelectedTab.ButtonClicked  = true;
            m_dgViewSelected.Visible        = true;

			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] ADD, MODIFY, REMOVE 버튼 클릭 이벤트
		/// </summary>
		private void Click_ManagementButton(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // ADD
					int nItemNum	= m_InstanceOfLanguage.AddLanguage(m_enSelectedType);
					break;				
				case 2:	// REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						m_InstanceOfLanguage.RemoveItem(FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD, m_nIndexOfSelectedItem);
					}
					break;
			}

			ResetSelectedItem();
			
			switch(m_nSelectedTab)
			{
				case 0: // GUI
					UpdateGridForWord();
					break;
				case 1:	// MESSAGE
					UpdateGridForSentence();
					break;
				case 2:	// LOG
					break;
			}
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 그리드 뷰 더블 클릭 이벤트
		/// </summary>
		private void DoubleClick_Dgview(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex		= e.RowIndex;

			if (nRowIndex < 0 || nRowIndex >= m_dgViewSelected.RowCount)
			{
				return;
			}

			m_nIndexOfSelectedItem	= int.Parse(m_dgViewSelected[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString());
			m_nIndexOfSelectedRows	= nRowIndex;

			SetLanguageLabels(ref nRowIndex);
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 각 라벨 클릭 이벤트
		/// </summary>
		private void Click_Labels(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;
			FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT en_Format	= FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD;
			switch(m_nSelectedTab)
			{
				case 0:
					en_Format	= FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.WORD;
					break;
				case 1:
					en_Format	= FrameOfSystem3.Config.ConfigLanguage.EN_TYPE_FORMAT.SENTENCE;
					break;
			}

			string strResult	= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0: // ENGLISH
					if(m_InstanceOfKeyborad.CreateForm(m_labelEng.Text))
					{
						m_InstanceOfKeyborad.GetResult(ref strResult);
						if(m_InstanceOfLanguage.SetParameter(en_Format, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH, strResult))
						{
							// 2024.12.17. by shkim. [MOD] 같은 언어로 설정됬을 예외상황이 있어 보완함.
							//m_dgViewSelected[COLUMN_INDEX_OF_ENGLISH, m_nIndexOfSelectedRows].Value = strResult;
							//m_labelEng.Text = strResult;

							m_InstanceOfLanguage.GetParameter(en_Format, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH, ref strResult);
							m_dgViewSelected[COLUMN_INDEX_OF_ENGLISH, m_nIndexOfSelectedRows].Value = strResult;
							m_labelEng.Text = strResult;

							m_InstanceOfLanguage.GetParameter(en_Format, m_nIndexOfSelectedItem, GetConfigLanguege(), ref strResult);
							m_dgViewSelected[COLUMN_INDEX_OF_OTHERLANGUAGE, m_nIndexOfSelectedRows].Value = strResult;
							m_labelOther.Text = strResult;
							// 2024.12.17. by shkim. [END]
						}
					}
					break;
				case 1: //KOREAN
					if(m_InstanceOfKeyborad.CreateForm(m_labelOther.Text))
					{
						m_InstanceOfKeyborad.GetResult(ref strResult);
                        if (m_InstanceOfLanguage.SetParameter(en_Format, m_nIndexOfSelectedItem, GetConfigLanguege(), strResult))
						{
							// 2024.12.17. by shkim. [MOD] 같은 언어로 설정됬을 예외상황이 있어 보완함.
							//                     m_dgViewSelected[COLUMN_INDEX_OF_OTHERLANGUAGE, m_nIndexOfSelectedRows].Value = strResult;
							//m_labelOther.Text = strResult;

							m_InstanceOfLanguage.GetParameter(en_Format, m_nIndexOfSelectedItem, FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH, ref strResult);
							m_dgViewSelected[COLUMN_INDEX_OF_ENGLISH, m_nIndexOfSelectedRows].Value = strResult;
							m_labelEng.Text = strResult;

							m_InstanceOfLanguage.GetParameter(en_Format, m_nIndexOfSelectedItem, GetConfigLanguege(), ref strResult);
							m_dgViewSelected[COLUMN_INDEX_OF_OTHERLANGUAGE, m_nIndexOfSelectedRows].Value = strResult;
							m_labelOther.Text = strResult;
							// 2024.12.17. by shkim. [END]
						}
					}
					break;
				
			}
		}
		#endregion

        private FrameOfSystem3.Config.ConfigLanguage.EN_PARAM_LANGUAGE GetConfigLanguege()
        {
         switch( Work.AppConfigManager.Instance.Language)
         {
             case Language_.TYPE_LANGUAGE.KOREAN:
                 return ConfigLanguage.EN_PARAM_LANGUAGE.KOREAN;
             case Language_.TYPE_LANGUAGE.CHINESE:
                 return ConfigLanguage.EN_PARAM_LANGUAGE.CHINESE;
             case Language_.TYPE_LANGUAGE.JAPANESE:
                 return ConfigLanguage.EN_PARAM_LANGUAGE.JAPANESE;
             default:
                 return ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH;
         }
       
        }
		

	}
}
