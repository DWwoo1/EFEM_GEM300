using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using FrameOfSystem3.Recipe;

using Sys3Controls;
using Define.DefineConstant;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Parameters : UserControlForMainView.CustomView
	{
		public Config_Parameters()
		{
			InitializeComponent();

			#region Instance
			m_instanceRecipe			= FrameOfSystem3.Recipe.Recipe.GetInstance();
			m_InstanceLanguage			= FrameOfSystem3.Config.ConfigLanguage.GetInstance();
			m_InstanceOfKeyboard		= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfCalculator		= Functional.Form_Calculator.GetInstance();
			m_InstanceSelectionList		= Functional.Form_SelectionList.GetInstance();
			m_InstanceMessageBox		= Functional.Form_MessageBox.GetInstance();
            #endregion

            #region Initial
            InitializeParameterPage();
            #endregion

            _tableLayoutPanel_Contents.RowStyles[0].Height = _tableLayoutPanel_Contents.Size.Height;
            _tableLayoutPanel_Contents.RowStyles[1].Height = 0;
            _tableLayoutPanel_Contents.RowStyles[2].Height = 0;
        }

		#region Struct
		private struct RecipeItem
		{
			public string strMin;		
			public string strMax;		
			public string strValue;		
			public string strUnit;
            public string strAuthority;
            public string strDataType;
            public string strDescription;
			public string strDefaultValue; 		// 2023.05.19 by junho [ADD] add default value
		}
		#endregion

		#region Enum
		private enum EN_TABINDEX_TOGGLE 
		{ 
			INDEX		= 0,
			PARAMETER	= 1,
			GROUPINDEX	= 2,
			MIN			= 3,
			MAX			= 4,
			VALUE		= 5,
			UNIT		= 6,
			AUTHORITY	= 7,
			TASKNAME	= 8,
			DATA_TYPE	= 9,
			DESCRIPTION	= 10,
			DEFAULT_VALUE = 11,	// 2023.05.19 by junho [ADD] add default value
		}

		private enum EN_COLUMN_GRIDVIEW
		{
			INDEX		= 0,
			PARAMETER	= 1,
			GROUPINDEX	= 2,
			MIN			= 3,
			MAX			= 4,
			VALUE		= 5,
			UNIT		= 6,
			AUTHORITY	= 7,
			TASKNAME	= 8,
			DATA_TYPE	= 9,
			DESCRIPTION = 10,
			DEFAULT_VALUE = 11,	// 2023.05.19 by junho [ADD] add default value
		}
		#endregion

		#region Constant
		private const int SELECTED_NONE			= -1;

        #region GUI
		private readonly Color FIRST_COLOR				= Color.White;
		private readonly Color SECOND_COLOR				= Color.DarkBlue;

		private const int HEIGHT_OF_ROWS				= 30;

		private readonly string[] INPUT_MODE			= new string[] {"CALCULTOR", "KEYBOARD"};

		private readonly string COLUMN_TEXT_INDEX		= "INDEX";
		private readonly string COLUMN_TEXT_PARAMETER	= "PARAMETER";
		private readonly string COLUMN_TEXT_GROUP		= "GROUP";
		private readonly string COLUMN_TEXT_MIN			= "MIN";
		private readonly string COLUMN_TEXT_MAX			= "MAX";
		private readonly string COLUMN_TEXT_VALUE		= "VALUE";
		private readonly string COLUMN_TEXT_UNIT		= "UNIT";
		private readonly string COLUMN_TEXT_AUTORITY	= "AUTHORITY";
		private readonly string COLUMN_TEXT_TASK		= "TASK";

		private readonly string ASCENDING_ICON			= " ▲ ";
		private readonly string DESCENDING_ICON			= " ▼ ";
		#endregion

		#region Export
		private readonly string SEPERATOR	= ",";
		#endregion

		#endregion

		#region Variables

        #region Instance
        FrameOfSystem3.Recipe.Recipe m_instanceRecipe			= null;
		FrameOfSystem3.Config.ConfigLanguage m_InstanceLanguage = null;

		Functional.Form_Keyboard m_InstanceOfKeyboard			= null;
		Functional.Form_Calculator m_InstanceOfCalculator		= null;
        Functional.Form_SelectionList m_InstanceSelectionList	= null;
		Functional.Form_MessageBox m_InstanceMessageBox			= null;
        #endregion

        private Sys3button m_tabClicked							= null;

		// Key : Recipe Type, Value : GridView
		private Dictionary<EN_RECIPE_TYPE, Sys3DoubleBufferedDataGridView> m_dicForGridView	= new Dictionary<EN_RECIPE_TYPE,Sys3DoubleBufferedDataGridView>();

		private EN_RECIPE_TYPE m_enCurrentViewMode	= EN_RECIPE_TYPE.COMMON;
		private int m_nSelectedRowIndex				= 0;
		private int m_nSelectedColumnIndex			= 0;
		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
        {
			UpdateGridView(m_enCurrentViewMode);
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
		/// 2021.03.15 by twkang [ADD] Page 정보를 초기화한다. 생성자에서 한 번 호출
		/// 2021.09.16 by twkang [MOD] 내용수정
		/// </summary>
		private void InitializeParameterPage()
		{
			m_tabClicked				= m_btnCommon;

			m_dgViewEquipment.Visible	= false;
			m_dgViewProcess.Visible		= false;

			// Mapping
			m_dicForGridView.Add(EN_RECIPE_TYPE.COMMON, m_dgViewCommon);
			m_dicForGridView.Add(EN_RECIPE_TYPE.EQUIPMENT, m_dgViewEquipment);
			m_dicForGridView.Add(EN_RECIPE_TYPE.PROCESS, m_dgViewProcess);

			#region TabIndex
			m_ToggleAuthority.TabIndex		= (int)EN_TABINDEX_TOGGLE.AUTHORITY;
			m_ToggleParameter.TabIndex		= (int)EN_TABINDEX_TOGGLE.PARAMETER;
			m_ToggleTaskName.TabIndex		= (int)EN_TABINDEX_TOGGLE.TASKNAME;
			m_ToggleGroup.TabIndex			= (int)EN_TABINDEX_TOGGLE.GROUPINDEX;
			m_ToggleValue.TabIndex			= (int)EN_TABINDEX_TOGGLE.VALUE;
			m_ToggleIndex.TabIndex			= (int)EN_TABINDEX_TOGGLE.INDEX;
			m_ToggleUnit.TabIndex			= (int)EN_TABINDEX_TOGGLE.UNIT;
			m_ToggleMax.TabIndex			= (int)EN_TABINDEX_TOGGLE.MAX;
			m_ToggleMin.TabIndex			= (int)EN_TABINDEX_TOGGLE.MIN;
            m_ToggleDataType.TabIndex		= (int)EN_TABINDEX_TOGGLE.DATA_TYPE;
			m_ToggleDescription.TabIndex = (int)EN_TABINDEX_TOGGLE.DESCRIPTION;
			m_ToggleDefaultValue.TabIndex = (int)EN_TABINDEX_TOGGLE.DEFAULT_VALUE;

			m_dgViewEquipment.TabIndex		= (int)EN_RECIPE_TYPE.EQUIPMENT;
			m_dgViewProcess.TabIndex		= (int)EN_RECIPE_TYPE.PROCESS;
			m_dgViewCommon.TabIndex			= (int)EN_RECIPE_TYPE.COMMON;
			#endregion

			InitializeGridView(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON);
			InitializeGridView(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT);
			InitializeGridView(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.PROCESS);
		}
		
		/// <summary>
		/// 2021.09.16 by twkang [ADD] 계산기, 키보드를 열어서 벨류값 가져옴
		/// </summary>
		private bool GetValueByFuntionalForm(EN_RECIPE_PARAM_TYPE enType, ref string strResult)
		{
            Sys3DoubleBufferedDataGridView dgCurrnetView = m_dicForGridView[m_enCurrentViewMode];

            string strPreValue = dgCurrnetView[m_nSelectedColumnIndex, m_nSelectedRowIndex].Value.ToString();

            switch (enType)
            {
                default:
                    if (m_InstanceSelectionList.CreateForm("FORM", INPUT_MODE, string.Empty))
                    {
                        int nResult = -1;

                        m_InstanceSelectionList.GetResult(ref nResult);

                        switch (nResult)
                        {
                            case 0: // CALCULATOR
                                string strMin = dgCurrnetView[(int)EN_COLUMN_GRIDVIEW.MIN, m_nSelectedRowIndex].Value.ToString();
                                string strMax = dgCurrnetView[(int)EN_COLUMN_GRIDVIEW.MAX, m_nSelectedRowIndex].Value.ToString();
                                string strUnit = dgCurrnetView[(int)EN_COLUMN_GRIDVIEW.UNIT, m_nSelectedRowIndex].Value.ToString();

                                if (m_InstanceOfCalculator.CreateForm(strPreValue, strMin, strMax, strUnit))
                                {
                                    m_InstanceOfCalculator.GetResult(ref strResult);

                                    return true;
                                }
                                break;
                            case 1: // KEYBOARD
                                if (m_InstanceOfKeyboard.CreateForm(strPreValue))
                                {
                                    m_InstanceOfKeyboard.GetResult(ref strResult);

                                    return true;
                                }
                                break;
                        }
                    }
                    break;
                case EN_RECIPE_PARAM_TYPE.DESCRIPTION:
                case EN_RECIPE_PARAM_TYPE.UNIT:
                    if (m_InstanceOfKeyboard.CreateForm(strPreValue))
                    {
                        m_InstanceOfKeyboard.GetResult(ref strResult);

                        return true;
                    }
                    break;
                case EN_RECIPE_PARAM_TYPE.DATA_TYPE:
                    if (m_InstanceSelectionList.CreateForm("FORM", Enum.GetNames(typeof(FrameOfSystem3.Recipe.EN_DATA_TYPE)), strPreValue))
                    {
                        m_InstanceSelectionList.GetResult(ref strResult);

                        return true;
                    }
                    break;

            }
			return false;
		}

		/// <summary>
		/// 2021.09.15 by twkang [ADD] 레시피 셋 벨류
		/// </summary>
		private void SetRecipeValue()
		{
			if(m_nSelectedRowIndex == SELECTED_NONE)
				return;

			EN_RECIPE_PARAM_TYPE enParameterType	= EN_RECIPE_PARAM_TYPE.VALUE;

			switch(m_nSelectedColumnIndex)
			{
				case (int)EN_COLUMN_GRIDVIEW.AUTHORITY:
					enParameterType	= EN_RECIPE_PARAM_TYPE.AUTHORITY;
					break;

				case (int)EN_COLUMN_GRIDVIEW.MAX:
					enParameterType	= EN_RECIPE_PARAM_TYPE.MAX;
					break;

				case (int)EN_COLUMN_GRIDVIEW.MIN:
					enParameterType	= EN_RECIPE_PARAM_TYPE.MIN;
					break;

				case (int)EN_COLUMN_GRIDVIEW.VALUE:
					enParameterType	= EN_RECIPE_PARAM_TYPE.VALUE;
					break;

				case (int)EN_COLUMN_GRIDVIEW.UNIT:
					enParameterType = EN_RECIPE_PARAM_TYPE.UNIT;
					break;

                case (int)EN_COLUMN_GRIDVIEW.DATA_TYPE:
                    enParameterType = EN_RECIPE_PARAM_TYPE.DATA_TYPE;
                    break;

                case (int)EN_COLUMN_GRIDVIEW.DESCRIPTION:
                    enParameterType = EN_RECIPE_PARAM_TYPE.DESCRIPTION;
                    break;

				case (int)EN_COLUMN_GRIDVIEW.DEFAULT_VALUE:
					enParameterType = EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE;
					break;

				default:
					return;
			}

			string strResult	= string.Empty;

			// 계산기 키보드를 열어서 데이터를 받아옴
			if(false == GetValueByFuntionalForm(enParameterType, ref strResult))
				return;

			Sys3DoubleBufferedDataGridView dgViewCurrent	= m_dicForGridView[m_enCurrentViewMode];

			string strParam	= dgViewCurrent[(int)EN_COLUMN_GRIDVIEW.PARAMETER, m_nSelectedRowIndex].Value.ToString();
			int nGroupIndex	= 0;

			int.TryParse(dgViewCurrent[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX, m_nSelectedRowIndex].Value.ToString(), out nGroupIndex);

			switch(m_enCurrentViewMode)
			{
				case EN_RECIPE_TYPE.PROCESS: // 프로세스 레시피 셋
					string strTaskName	= dgViewCurrent[(int)EN_COLUMN_GRIDVIEW.TASKNAME, m_nSelectedRowIndex].Value.ToString();

					if(m_instanceRecipe.SetValue(strTaskName, strParam, nGroupIndex, enParameterType, strResult))
					{
						dgViewCurrent[m_nSelectedColumnIndex, m_nSelectedRowIndex].Value	= strResult;
					}
					break;

				default:
					if (m_instanceRecipe.SetValue(m_enCurrentViewMode, strParam, nGroupIndex, enParameterType, strResult))
					{
						dgViewCurrent[m_nSelectedColumnIndex, m_nSelectedRowIndex].Value = strResult;
					}
					break;
			}
		}

		/// <summary>
		/// 2021.09.15 by twkang [ADD] 레시피 아이템의 그룹수를 알아옴
		/// </summary>
		private int GetRecipeGroupCountByName(string strParameterName)
		{
			int nRecipeGroupCount	= 1;

			// 아이템 수를 확인하기 위함 ex) XXXX_20  <-- 아이템 20개
			int nIndexOfItemCount	= strParameterName.LastIndexOf("_");

			if (int.TryParse(strParameterName.Substring(nIndexOfItemCount + 1), out nRecipeGroupCount))
			{
				return nRecipeGroupCount;
			}

			return 1;
		}

		/// <summary>
		/// 2021.09.16 by twkang [ADD] 아이템 선택 정보 삭제
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nSelectedColumnIndex	= SELECTED_NONE;
			m_nSelectedRowIndex		= SELECTED_NONE;
		}

		/// <summary>
        /// 2023.05.09 by wdw [MOD] DATATYPE, DESCRIPTION 아이템 추가
        /// 2021.09.16 by twkang [ADD] Gridview 아이템 업데이트
		/// </summary>
		private void UpdateGridView(EN_RECIPE_TYPE enType)
		{
			Sys3DoubleBufferedDataGridView dgViewCurrnet	= m_dicForGridView[enType];

			for (int nIndex = 0, nEnd = dgViewCurrnet.Rows.Count; nIndex < nEnd; ++nIndex)
			{
				string strParamName		= dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.PARAMETER, nIndex].Value.ToString();
				string strTaskName		= dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.TASKNAME, nIndex].Value.ToString();

				int nGroupIndex			= 0;

				int.TryParse(dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX, nIndex].Value.ToString(), out nGroupIndex);

				var pRecipeItem = GetRecipeValue(enType, strParamName, nGroupIndex, strTaskName);

				dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.AUTHORITY, nIndex].Value	= pRecipeItem.strAuthority;
				dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.VALUE, nIndex].Value		= pRecipeItem.strValue;
				dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.UNIT, nIndex].Value		= pRecipeItem.strUnit;
				dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.MIN, nIndex].Value		= pRecipeItem.strMin;
                dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.MAX, nIndex].Value = pRecipeItem.strMax;
                dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.DATA_TYPE, nIndex].Value = pRecipeItem.strDataType;
				dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.DESCRIPTION, nIndex].Value = pRecipeItem.strDescription;
				dgViewCurrnet[(int)EN_COLUMN_GRIDVIEW.DEFAULT_VALUE, nIndex].Value = pRecipeItem.strDefaultValue;
			}
		}

		/// <summary>
		/// 2021.09.16 by twkang [ADD] 레시피 아이템을 한번에 받아옴
		/// </summary>
		private RecipeItem GetRecipeValue(EN_RECIPE_TYPE enType, string strParamName, int nIndexOfItem, string strTaskName = "")
		{
			RecipeItem pItemList	= new RecipeItem();

			switch(enType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					pItemList.strMin = m_instanceRecipe.GetValue(strTaskName
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
						, string.Empty);

					pItemList.strMax = m_instanceRecipe.GetValue(strTaskName
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
						, string.Empty);

					pItemList.strValue = m_instanceRecipe.GetValue(strTaskName
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
						, string.Empty);

					pItemList.strUnit = m_instanceRecipe.GetValue(strTaskName
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
						, string.Empty);

					pItemList.strAuthority = m_instanceRecipe.GetValue(strTaskName
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.AUTHORITY
						, string.Empty);

                    pItemList.strDataType = m_instanceRecipe.GetValue(strTaskName
                        , strParamName
                        , nIndexOfItem
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
                        , string.Empty);

                    pItemList.strDescription = m_instanceRecipe.GetValue(strTaskName
                        , strParamName
                        , nIndexOfItem
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DESCRIPTION
                        , string.Empty);

					// 2023.05.19 by junho [ADD] add default value
                    pItemList.strDefaultValue = m_instanceRecipe.GetValue(strTaskName
                        , strParamName
                        , nIndexOfItem
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
                        , string.Empty);
					break;
				default:
					pItemList.strMin = m_instanceRecipe.GetValue(enType
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
						, string.Empty);

					pItemList.strMax = m_instanceRecipe.GetValue(enType
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
						, string.Empty);

					pItemList.strValue = m_instanceRecipe.GetValue(enType
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
						, string.Empty);

					pItemList.strUnit = m_instanceRecipe.GetValue(enType
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
						, string.Empty);

					pItemList.strAuthority = m_instanceRecipe.GetValue(enType
						, strParamName
						, nIndexOfItem
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.AUTHORITY
						, string.Empty);


                    pItemList.strDataType = m_instanceRecipe.GetValue(enType
                        , strParamName
                        , nIndexOfItem
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
                        , string.Empty);


                    pItemList.strDescription = m_instanceRecipe.GetValue(enType
                        , strParamName
                        , nIndexOfItem
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DESCRIPTION
                        , string.Empty);

					// 2023.05.19 by junho [ADD] add default value
                    pItemList.strDefaultValue = m_instanceRecipe.GetValue(enType
                        , strParamName
                        , nIndexOfItem
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
                        , string.Empty);
					break;
			}

			return pItemList;
		}

		/// <summary>
		/// 
		/// </summary>
        private string[] getTaskName()
        {
            string[] arTaskeName			= null;
			string[] arParameterList		= null;
			int[] arCountOfItem				= null;
			int nCountOfParameter			= 0;
            m_instanceRecipe.GetListOfProcessParameter(ref arTaskeName, ref nCountOfParameter, ref arParameterList, ref arCountOfItem);

            return arTaskeName;
        }

		#region GUI
		/// <summary>
		/// 2021.09.15 by twkang [ADD] 버튼 선택 리액션 Gridview 바꿔줌
		/// </summary>
		private void ChangeGridView(int nButtonTabIndex)
		{
			if (m_tabClicked.TabIndex == nButtonTabIndex) { return;}

			m_tabClicked.GradientFirstColor		= FIRST_COLOR;
			m_tabClicked.GradientSecondColor	= FIRST_COLOR;
			m_tabClicked.MainFontColor			= SECOND_COLOR;
			m_tabClicked.ButtonClicked			= false;

			m_dicForGridView[m_enCurrentViewMode].Visible	= false;

			switch (nButtonTabIndex)
			{
				case 0: // COMMON
					m_enCurrentViewMode	= EN_RECIPE_TYPE.COMMON;
					m_tabClicked		= m_btnCommon;

                    _tableLayoutPanel_Contents.RowStyles[0].Height = _tableLayoutPanel_Contents.Size.Height;
                    _tableLayoutPanel_Contents.RowStyles[1].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[2].Height = 0;
					break;
				case 1:	// EQUIPMENT
					m_enCurrentViewMode	= EN_RECIPE_TYPE.EQUIPMENT;
					m_tabClicked		= m_btnEquipment;
					
                    _tableLayoutPanel_Contents.RowStyles[0].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[1].Height = _tableLayoutPanel_Contents.Size.Height;;
                    _tableLayoutPanel_Contents.RowStyles[2].Height = 0;
					break;
				case 2:	// PROCESS
					m_enCurrentViewMode	= EN_RECIPE_TYPE.PROCESS;
					m_tabClicked		= m_btnProcess;

                    _tableLayoutPanel_Contents.RowStyles[0].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[1].Height = 0;
                    _tableLayoutPanel_Contents.RowStyles[2].Height = _tableLayoutPanel_Contents.Size.Height;;
					break;
			}

			m_dicForGridView[m_enCurrentViewMode].Visible	= true;

			m_tabClicked.ButtonClicked			= true;
			m_tabClicked.GradientFirstColor		= SECOND_COLOR;
			m_tabClicked.GradientSecondColor	= SECOND_COLOR;
			m_tabClicked.MainFontColor			= FIRST_COLOR;

			UpdateGridView(m_enCurrentViewMode);
		}

		/// <summary>
		/// 2021.09.15 by twkang [ADD] Column 설정 ViewMode 설정
		/// </summary>
		private void UpdateViewMode(int nToggleTabIndex, bool bActive)
		{
			m_dgViewCommon.Columns[nToggleTabIndex].Visible		= bActive;
			m_dgViewProcess.Columns[nToggleTabIndex].Visible	= bActive;
			m_dgViewEquipment.Columns[nToggleTabIndex].Visible	= bActive;
		}

		/// <summary>
		/// 2021.09.15 by twkang [ADD] 데이터를 그리드뷰에 추가
		/// </summary>
        private void AddValueToGrid(FrameOfSystem3.Recipe.EN_RECIPE_TYPE enType, string strRecipeName, int nIndexOfItem, string TaskName = "")
		{
			Sys3DoubleBufferedDataGridView RecipeGridView	= m_dicForGridView[enType];

			RecipeItem pRecipeItem	= GetRecipeValue(enType, strRecipeName, nIndexOfItem, TaskName);

			int nIndexOfRow	= RecipeGridView.Rows.Count;

			RecipeGridView.Rows.Add();

			RecipeGridView.Rows[nIndexOfRow].Height						= HEIGHT_OF_ROWS;

			RecipeGridView[(int)EN_COLUMN_GRIDVIEW.INDEX, nIndexOfRow].Value	= nIndexOfRow.ToString();
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.INDEX].Style.BackColor			= Color.Silver;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.INDEX].Style.SelectionBackColor	= Color.Silver;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.INDEX].Style.SelectionForeColor	= Color.Black;

			RecipeGridView[(int)EN_COLUMN_GRIDVIEW.PARAMETER, nIndexOfRow].Value	= strRecipeName;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.PARAMETER].Style.BackColor			= Color.Silver;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.PARAMETER].Style.SelectionBackColor	= Color.Silver;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.PARAMETER].Style.SelectionForeColor	= Color.Black;

			RecipeGridView[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX, nIndexOfRow].Value	= nIndexOfItem;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX].Style.BackColor			= Color.Silver;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX].Style.SelectionBackColor	= Color.Silver;
			RecipeGridView.Rows[nIndexOfRow].Cells[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX].Style.SelectionForeColor	= Color.Black;

            RecipeGridView[(int)EN_COLUMN_GRIDVIEW.AUTHORITY, nIndexOfRow].Value	= pRecipeItem.strAuthority;
			RecipeGridView[(int)EN_COLUMN_GRIDVIEW.TASKNAME, nIndexOfRow].Value		= TaskName;
            RecipeGridView[(int)EN_COLUMN_GRIDVIEW.VALUE, nIndexOfRow].Value		= pRecipeItem.strValue;
            RecipeGridView[(int)EN_COLUMN_GRIDVIEW.UNIT, nIndexOfRow].Value			= pRecipeItem.strUnit;
			RecipeGridView[(int)EN_COLUMN_GRIDVIEW.MIN, nIndexOfRow].Value			= pRecipeItem.strMin;
			RecipeGridView[(int)EN_COLUMN_GRIDVIEW.MAX, nIndexOfRow].Value			= pRecipeItem.strMax;
		}

		/// <summary>
		/// 2021.03.15 by twkang [ADD] 그리드뷰 업데이트
		/// 2021.09.16 by twkang [MOD] 내용 수정
		/// </summary>
		private void InitializeGridView(FrameOfSystem3.Recipe.EN_RECIPE_TYPE enType)
		{
			Array enumList	= null;

			switch (enType)
			{
				case FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON:
					m_dgViewCommon.Rows.Clear();

					EnterDefalutColumnText(m_dgViewCommon);

					enumList	= Enum.GetValues(typeof(FrameOfSystem3.Recipe.PARAM_COMMON));
					break;

				case FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT:
					m_dgViewEquipment.Rows.Clear();

					EnterDefalutColumnText(m_dgViewEquipment);

					enumList	= Enum.GetValues(typeof(FrameOfSystem3.Recipe.PARAM_EQUIPMENT));
					break;

				case FrameOfSystem3.Recipe.EN_RECIPE_TYPE.PROCESS:
					m_dgViewProcess.Rows.Clear();

					EnterDefalutColumnText(m_dgViewProcess);

					string[] arTaskName		= null;
					string[] arParamList	= null;
					int[] arCountOfItem		= null;
					int nCountOfParam		= 0;

					if(false == m_instanceRecipe.GetListOfProcessParameter(ref arTaskName, ref nCountOfParam, ref arParamList, ref arCountOfItem))
						return;

					for(int nIndex = 0; nIndex < nCountOfParam; ++nIndex)
					{
						for(int nIndexOfItem = 0, nEnd = arCountOfItem[nIndex]; nIndexOfItem < nEnd; ++nIndexOfItem)
						{
                            AddValueToGrid(enType, arParamList[nIndex], nIndexOfItem, arTaskName[nIndex]);
						}
					}
					return;
			}
			
			foreach (var enRecipeItem in enumList)
			{
				int nItemCount = GetRecipeGroupCountByName(enRecipeItem.ToString());

				// GRID VIEW 아이템 추가
				for (int nIndex = 0; nIndex < nItemCount; ++nIndex)
				{
					AddValueToGrid(enType, enRecipeItem.ToString(), nIndex);
				}
			}
		}

		#region Sort
		/// <summary>
		/// 2020.12.03 by yjlee [ADD] Sort the content by the column.
		/// </summary>
		private void SortContent(ref int nColumnIndex, Sys3DoubleBufferedDataGridView RecipeGridView)
		{
			ListSortDirection sortDir       = RecipeGridView.SortOrder == SortOrder.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

			RecipeGridView.Sort(RecipeGridView.Columns[nColumnIndex], sortDir);

			UpdateColumnSortUI(nColumnIndex, sortDir == ListSortDirection.Ascending, RecipeGridView);

			RecipeGridView.SortCompare += new DataGridViewSortCompareEventHandler(SortCompareByContents);
		}

		/// <summary>
		/// 2021.05.12 by twkang [ADD] GUI DataGridView Column 모양 업데이트
		/// </summary>
		private void UpdateColumnSortUI(int nColumnIndex, bool bAscending, Sys3DoubleBufferedDataGridView RecipeGridView)
		{
			string strSorting	= bAscending ? ASCENDING_ICON : DESCENDING_ICON;

			EnterDefalutColumnText(RecipeGridView);

			RecipeGridView.Columns[nColumnIndex].HeaderText += strSorting;
		}

		/// <summary>
		/// 2021.09.16 by twkang [ADD] 기본 칼럼 설정
		/// </summary>
		private void EnterDefalutColumnText(Sys3DoubleBufferedDataGridView RecipeGridView)
		{
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.GROUPINDEX].HeaderText	= COLUMN_TEXT_GROUP;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.PARAMETER].HeaderText	= COLUMN_TEXT_PARAMETER;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.AUTHORITY].HeaderText	= COLUMN_TEXT_AUTORITY;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.TASKNAME].HeaderText		= COLUMN_TEXT_TASK;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.VALUE].HeaderText		= COLUMN_TEXT_VALUE;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.INDEX].HeaderText		= COLUMN_TEXT_INDEX;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.UNIT].HeaderText			= COLUMN_TEXT_UNIT;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.MIN].HeaderText			= COLUMN_TEXT_MIN;
			RecipeGridView.Columns[(int)EN_COLUMN_GRIDVIEW.MAX].HeaderText			= COLUMN_TEXT_MAX;
		}

		/// <summary>
		/// 2020.12.03 by yjlee [ADD] Sort the content by the alarm code.
		/// </summary>
		private void SortCompareByContents(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (null == e.CellValue1 || null == e.CellValue2)
				return;

			string strValueFirst    = e.CellValue1.ToString();
			string strValueSecond   = e.CellValue2.ToString();

			int nValueFirst         = 0;
			int nValueSecond        = 0;

			if (int.TryParse(strValueFirst, out nValueFirst)
				&& int.TryParse(strValueSecond, out nValueSecond))
			{
				e.SortResult = nValueFirst.CompareTo(nValueSecond);
			}
			else
			{
				e.SortResult = string.Compare(strValueFirst, strValueSecond);
			}

			e.Handled = true;
		}
		#endregion

		#endregion

		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2021.03.09 by twkang [ADD] 레시피 타입 버튼 클릭 이벤트
		/// </summary>
		private void Click_RecipeType(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			ChangeGridView(ctrl.TabIndex);
        }

		/// <summary>
		/// 2021.03.19 by jhlee [ADD] Toggle 클릭시 발생 Event
		/// </summary>
		private void Click_ViewOption(object sender, EventArgs e)
		{
			Sys3ToggleButton ctrl	= sender as Sys3ToggleButton;

			UpdateViewMode(ctrl.TabIndex, ctrl.Active);
		}

		/// <summary>
		/// 2021.09.16 by twkang [ADD] Toggle 클릭시 발생 Event
		/// </summary>
		private void Click_ViewOption(object sender, MouseEventArgs e)
		{
			Sys3ToggleButton ctrl	= sender as Sys3ToggleButton;

			UpdateViewMode(ctrl.TabIndex, ctrl.Active);
		}

		/// <summary>
		/// 2021.09.15 by twkang [ADD] 더블클릭 이벤트
		/// </summary>
		private void DoubleClick_DataGridView(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex		= e.RowIndex;
			int nColumnIndex	= e.ColumnIndex;

			Sys3DoubleBufferedDataGridView pGridView	= sender as Sys3DoubleBufferedDataGridView;

			if(nRowIndex < 0)
			{
				SortContent(ref nColumnIndex, pGridView);

				ResetSelectedItem();
			}
			else if(nRowIndex < pGridView.RowCount)
			{
				m_nSelectedRowIndex		= nRowIndex;
				m_nSelectedColumnIndex	= nColumnIndex;

				SetRecipeValue();
                UpdateGridView(m_enCurrentViewMode);	// 2025.03.18. [ADD] by shkim. 값 갱신이 안되는 증상이 있어 보완
            }
		}
		#endregion

		#region Export Interface
		/// <summary>
		/// 2021.09.10 by twkang [ADD] 파일경로
		/// </summary>
		private bool GetFolderPath(ref string strFullFolderPath)
		{
			FolderBrowserDialog fDialog = new FolderBrowserDialog();

			fDialog.ShowDialog();

			if (string.IsNullOrEmpty(fDialog.SelectedPath))
			{
				return false;
			}

			strFullFolderPath = fDialog.SelectedPath;

			return true;
		}

		/// <summary>
		/// 2021.09.10 by twkang [ADD] 파일이름
		/// </summary>
		private string GetCurrentTimeWithString(string strType)
		{
			DateTime nowDate	= DateTime.Now;

			return string.Format("{0:00}{1:00}_{2:00}{3:00}{4:00}_{5}"
				, nowDate.Month
				, nowDate.Day
				, nowDate.Hour
				, nowDate.Minute
				, nowDate.Second
				, strType);
		}

		/// <summary>
		/// 2021.09.10 by twknag [ADD] 레시피 파일 저장
		/// </summary>
		private bool SaveRecipeFileByCSV(string strFilePath, EN_RECIPE_TYPE enType)
		{
			Sys3DoubleBufferedDataGridView ctrlGridView	= m_dicForGridView[enType];
			
			List<string> listOfTemp	= new List<string>();

			if(ctrlGridView.Rows.Count < 1)
				return false;

			DateTime nowDate	= DateTime.Now;

			string sFileName		= GetCurrentTimeWithString(enType.ToString()) + FileFormat.FILEFORMAT_LOG;

			string sFullFileName	= string.Format("{0}\\{1}", strFilePath, sFileName);

			if (!Directory.Exists(Path.GetDirectoryName(sFullFileName)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(sFullFileName));
			}

			FileStream pFStream			= new FileStream(sFullFileName, FileMode.Append, FileAccess.Write);
			
			StreamWriter pStreamWriter	= new StreamWriter(pFStream, Encoding.UTF8);

			foreach (DataGridViewColumn col in ctrlGridView.Columns)
			{
				listOfTemp.Add(col.HeaderText);
			}

			pStreamWriter.WriteLine(string.Join(SEPERATOR, listOfTemp));

			for(int nRowCount = 0 , nEndRowCount = ctrlGridView.Rows.Count; nRowCount < nEndRowCount; ++nRowCount)
			{
				listOfTemp.Clear();

				for(int nColCount = 0, nEndColCount = ctrlGridView.Columns.Count; nColCount < nEndColCount; ++nColCount)
				{
					listOfTemp.Add(ctrlGridView.Rows[nRowCount].Cells[nColCount].Value.ToString());
				}

				pStreamWriter.WriteLine(string.Join(SEPERATOR, listOfTemp));
			}

			pStreamWriter.Close();
			pFStream.Close();

			return true;

		}

		/// <summary>
		/// 2021.09.10 by twkang [ADD] 버튼 클릭 이벤트
		/// </summary>
		private void Click_ExportButton(object sender, EventArgs e)
		{
			string strResult	= string.Empty;
			string strFilePath	= string.Empty;

			if (false == m_InstanceSelectionList.CreateForm("RECIPE TYPE", Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.RECIPE_TYPE, m_enCurrentViewMode.ToString())
				|| false == GetFolderPath(ref strFilePath))
				return;

			m_InstanceSelectionList.GetResult(ref strResult);

			EN_RECIPE_TYPE enType	= EN_RECIPE_TYPE.COMMON;
			
			if(false == Enum.TryParse(strResult, out enType))
				return;
				
			if(SaveRecipeFileByCSV(strFilePath, enType))
				m_InstanceMessageBox.ShowMessage(string.Format("Save complete, Separator is \"{0}\" ",SEPERATOR));
			else
				m_InstanceMessageBox.ShowMessage("Save Fail");

		}

		private void btn_DescriptionExportImport_Click(object sender, EventArgs e)
		{
			string selectedValue = string.Empty;
			if (false == m_InstanceSelectionList.CreateForm("WORK TYPE", new string[] { "EXPORT", "IMPORT" }, ""))
				return;

			m_InstanceSelectionList.GetResult(ref selectedValue);
			switch(selectedValue)
			{
				case "EXPORT": DescriptionExport(); return;
				case "IMPORT": DescriptionImport(); return;
				default: return;
			}
		}
		private void DescriptionExport()
		{
			List<string> dataList = new List<string>();
			var recipe = m_instanceRecipe;
			EN_RECIPE_TYPE recipeType = EN_RECIPE_TYPE.COMMON;
			string parameterName, strType = recipeType.ToString();

			// common
			foreach(var en in Enum.GetValues(typeof(PARAM_COMMON)))
			{
				parameterName = en.ToString();
				dataList.Add(string.Format("{0},{1},{2}", strType, parameterName, recipe.GetValue(recipeType, parameterName, 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, "")));
			}

			// equipment
			recipeType = EN_RECIPE_TYPE.EQUIPMENT;
			strType = recipeType.ToString();
			foreach (var en in Enum.GetValues(typeof(PARAM_EQUIPMENT)))
			{
				parameterName = en.ToString();
				dataList.Add(string.Format("{0},{1},{2}", strType, parameterName, recipe.GetValue(recipeType, parameterName, 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, "")));
			}

			// process
			string[] arTaskName = null;
			string[] arParamList = null;
			int[] arCountOfItem = null;
			int nCountOfParam = 0;
			if (false == recipe.GetListOfProcessParameter(ref arTaskName, ref nCountOfParam, ref arParamList, ref arCountOfItem))
				return;

			for (int nIndex = 0; nIndex < nCountOfParam; ++nIndex)
			{
				for (int nIndexOfItem = 0, nEnd = arCountOfItem[nIndex]; nIndexOfItem < nEnd; ++nIndexOfItem)
				{
					dataList.Add(string.Format("{0},{1},{2}", arTaskName[nIndex], arParamList[nIndex], recipe.GetValue(arTaskName[nIndex], arParamList[nIndex], 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, "")));
				}
			}

			string filePath, fileName;
			if (false == FrameOfSystem3.Functional.FunctionsETC.GetFilePathWithSaveFileDialog(out filePath, out fileName))
				return;

			filePath = string.Format("{0}\\{1}.csv", filePath, fileName);

			if (false == Directory.Exists(Path.GetDirectoryName(filePath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			}

			try
			{
				FileStream pFStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
				StreamWriter pStreamWriter = new StreamWriter(pFStream, Encoding.UTF8);

				foreach (string data in dataList)
				{
					pStreamWriter.WriteLine(data);
				}

				pStreamWriter.Close();
				pFStream.Close();

				m_InstanceMessageBox.ShowMessage(string.Format("Success ({0}, Line:{1})", filePath, dataList.Count));
			}
			catch (Exception e)
			{
				m_InstanceMessageBox.ShowMessage(string.Format("Failed : {0}", e.Message));
			}
		}
		private void DescriptionImport()
		{
			var openFileDialog = new System.Windows.Forms.OpenFileDialog();
			if (System.Windows.Forms.DialogResult.OK != openFileDialog.ShowDialog())
				return;

			string[] readLines = FrameOfSystem3.Functional.FunctionsETC.GetFileRead(openFileDialog.FileName);
			if (readLines == null || readLines.Length <= 0)
			{
				m_InstanceMessageBox.ShowMessage(string.Format("File read failed"));
				return;
			}

			var commonDescriptions = new Dictionary<string, string>();
			var equipmentDescriptions = new Dictionary<string, string>();
			var processDescriptions = new Dictionary<string, Dictionary<string, string>>();
			foreach(string oneLine in readLines)
			{
				string[] splitData = oneLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if(splitData.Length != 3)
				{
					Console.WriteLine("item count is wrong : " + oneLine);
					continue;
				}

				EN_RECIPE_TYPE recipeType;
				if(Enum.TryParse(splitData[0], out recipeType))
				{
					switch(recipeType)
					{
						case EN_RECIPE_TYPE.COMMON:
							commonDescriptions.Add(splitData[1], splitData[2]);
							break;
						case EN_RECIPE_TYPE.EQUIPMENT:
							equipmentDescriptions.Add(splitData[1], splitData[2]);
							break;
					}
				}
				else
				{	// process
					if (false == processDescriptions.ContainsKey(splitData[0]))
						processDescriptions.Add(splitData[0], new Dictionary<string, string>());

					processDescriptions[splitData[0]].Add(splitData[1], splitData[2]);
				}
			}

			var recipe = m_instanceRecipe;
			string paramName;
			int applyCount = 0;
			foreach(PARAM_COMMON en in Enum.GetValues(typeof(PARAM_COMMON)))
			{
				paramName = en.ToString();
				if (recipe.GetValue(EN_RECIPE_TYPE.COMMON, paramName, 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, "") == "")
				{
					if (commonDescriptions.ContainsKey(paramName))
					{
						recipe.SetValue(EN_RECIPE_TYPE.COMMON, paramName, 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, commonDescriptions[paramName]);
						++applyCount;
					}
				}
			}

			foreach(PARAM_EQUIPMENT en in Enum.GetValues(typeof(PARAM_EQUIPMENT)))
			{
				paramName = en.ToString();
				if (recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, paramName, 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, "") == "")
				{
					if (equipmentDescriptions.ContainsKey(paramName))
					{
						recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, paramName, 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, equipmentDescriptions[paramName]);
						++applyCount;
					}
				}
			}

			// process
			string[] arTaskName = null;
			string[] arParamList = null;
			int[] arCountOfItem = null;
			int nCountOfParam = 0;
			if (false == recipe.GetListOfProcessParameter(ref arTaskName, ref nCountOfParam, ref arParamList, ref arCountOfItem))
				return;

			for (int nIndex = 0; nIndex < nCountOfParam; ++nIndex)
			{
				for (int nIndexOfItem = 0, nEnd = arCountOfItem[nIndex]; nIndexOfItem < nEnd; ++nIndexOfItem)
				{
					if (recipe.GetValue(arTaskName[nIndex], arParamList[nIndex], 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, "") == "")
					{
						if (processDescriptions.ContainsKey(arTaskName[nIndex]) && processDescriptions[arTaskName[nIndex]].ContainsKey(arParamList[nIndex]))
						{
							recipe.SetValue(arTaskName[nIndex], arParamList[nIndex], 0, EN_RECIPE_PARAM_TYPE.DESCRIPTION, processDescriptions[arTaskName[nIndex]][arParamList[nIndex]]);
							++applyCount;
						}
					}
				}
			}

			m_InstanceMessageBox.ShowMessage(string.Format("Success ({0}, Line:{1}/{2})", openFileDialog.FileName, applyCount, readLines.Length));
		}
		#endregion
	}
}
