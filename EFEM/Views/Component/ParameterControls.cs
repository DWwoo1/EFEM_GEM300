using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

using Define.DefineEnumProject;
using Sys3Controls;

namespace FrameOfSystem3.Component
{
    #region 열거체
    public enum EN_LABEL_PARAMETER_TYPE
    {
        NONE,
		CALCULATE_DOUBLE,
		CALCULATE_INT,
		CALCULATE_UINT,
        KEYBOARD,
        COLOR,
        SELECT,
        FOLDER_DIALOG,
        DATE_TIME,
    }
    #endregion

	public delegate void DelAftetSetValue(Recipe.EN_RECIPE_TYPE ParameterType, string strTaskName, string strParaName);

	/// <summary>
	/// 2019.04.29 by ssh [ADD] Parameter 를 설정해야 하는 Label
	/// 2020.09.21 by ssh [MOD] PLB-1000 Parameter Type
	/// 2021.06.10. by shkim. [MOD] TaskName 속성 동적 생성되도록 수정
	/// </summary>
	public class CustomParameterLabel : Sys3Controls.Sys3Label
    {
        #region 변수들

        #region Recipe

        #region Type of Recipe

        #region Default Type
        private Recipe.EN_RECIPE_TYPE m_enRecipeType = Recipe.EN_RECIPE_TYPE.PROCESS;
        private int m_nParameterIndex = 0;
        
        private string m_selectedTask = string.Empty;
        private string m_strParameterName = string.Empty;
        private bool m_bNeedRemakeMap = false;
        private bool m_bModifiable = true;

		private string m_AssociatedMap;
        #endregion

		#region Parameter Change Confirm
		// 2021.10.28 by junho [ADD] Recipe Change Confirm
		private bool m_bUseParameterChangeConfirm = false;
		private Color m_clrParameterChangeDefaultColor = Color.Black;
		private Color m_clrParameterChangeWaitColor = Color.Red;
		private bool m_bParameterStored = false;
		private string m_strParameterStorage = string.Empty;
		#endregion

		#endregion

		#region Default Information
		private Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST m_enSelectionList = (Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST)0;
		private EN_LABEL_PARAMETER_TYPE m_enParameterType = EN_LABEL_PARAMETER_TYPE.KEYBOARD;
        private string m_strMinValue = string.Empty;
        private string m_strMaxValue = string.Empty;
		private string m_strUnitValue = string.Empty;
		private string m_strDefaultValue = string.Empty;
        #endregion

		#region Delegates
		public DelAftetSetValue delAfterSetValue;
		#endregion /Delegates

		#endregion

		private ToolTip _descriptionToolTip;

        #endregion

        #region 속성

        #region Recipe

        #region Type of Recipe

        [Category("Recipe"), Description("Choose Parameter setting type")]
        public EN_LABEL_PARAMETER_TYPE ParameterSettingType
        {
            set { m_enParameterType = value; }
            get { return m_enParameterType; }
        }

        [Category("Recipe"), Description("Choose Parameter setting type of selection list")]
        public Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST SelectionList
        {
            set
            {
                m_enSelectionList = value;
            }
            get { return m_enSelectionList; }
        }

        [Category("Recipe"), Description("Type of recipe parameter")]
        public Recipe.EN_RECIPE_TYPE ParameterType
        {
            set { m_enRecipeType = value; }
            get { return m_enRecipeType; }
        }

        [Browsable(false)]
        public int ParameterIndex
        {
            set { m_nParameterIndex = value; }
            get { return m_nParameterIndex; }
        }

        [Category("Recipe"), Description("Parameter Name of Item")]
        public string ParameterName
        {
            set
            {
                OnChangedRecipe(this, m_selectedTask, m_strParameterName, m_selectedTask, value);
				m_strParameterName = value;
            }
            get { return m_strParameterName; }
        }

        [Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        [TaskListAttribute()]
        public string TaskName
        {
            set
            {
                OnChangedRecipe(this, m_selectedTask, m_strParameterName, value, m_strParameterName);
                m_selectedTask = value;
            }
            get { return m_selectedTask; }
        }
        [Category("Recipe"), Description("Changed Value Need Remake map")]
        public bool NeedRemakeMap
        {
            set { m_bNeedRemakeMap = value; }
            get { return m_bNeedRemakeMap; }
        }

        [Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [MapList()]
        public string AssociatedMap
        {
            get
            {
                return m_AssociatedMap;
            }
            set
            {
                m_AssociatedMap = value;
            }
        }

        [Category("Recipe"), Description("User Can Change Value")]
        public bool Modifiable
        {
            get { return m_bModifiable; }
            set { m_bModifiable = value; }
        }
        #endregion

        #region Defualt Information
        [Category("Recipe-Default"), Description("Parameter Min Value")]
        public string ParameterMIN
        {
            set { m_strMinValue = value; }
            get { return m_strMinValue; }
        }
        [Category("Recipe-Default"), Description("Parameter Max Value")]
        public string ParameterMAX
        {
            set { m_strMaxValue = value; }
            get { return m_strMaxValue; }
        }
        [Category("Recipe-Default"), Description("Parameter Unit Value")]
        public string ParameterUNIT
        {
            set { m_strUnitValue = value; }
            get { return m_strUnitValue; }
        }
		[Category("Recipe-Default"), Description("Parameter Unit Value")]
		public string ParameterDefaultValue
		{
			set { m_strDefaultValue = value; }
			get { return m_strDefaultValue; }
		}
        #endregion

        #endregion

		#region Parameter Change Confirm
		// 2021.10.28 by junho [ADD] Recipe Change Confirm
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public bool UseParameterChangeConfirm
		{
			get { return m_bUseParameterChangeConfirm; }
			set { m_bUseParameterChangeConfirm = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeDefaultColor
		{
			get { return m_clrParameterChangeDefaultColor; }
			set { m_clrParameterChangeDefaultColor = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeWaitColor
		{
			get { return m_clrParameterChangeWaitColor; }
			set { m_clrParameterChangeWaitColor = value; }
		}
		[Browsable(false)]
		public string ParameterStorage
		{
			get { return m_strParameterStorage; }
			set { m_strParameterStorage = value; }
		}
		[Browsable(false)]
		public bool ParameterStored
		{
			get { return m_bParameterStored; }
			set { m_bParameterStored = value; }
		}
        #endregion

	    public delegate void DeleChangedRecipeProperty(CustomParameterLabel target, string fromTask, string fromParameter, string toTask, string toParameter);
		public static event DeleChangedRecipeProperty ChangedRecipeProperty = null;
		#endregion

		#region method
        private void OnChangedRecipe(CustomParameterLabel target, string fromTask, string fromParameter, string toTask, string toParameter)
		{
            if (ChangedRecipeProperty == null)
                return;

            ChangedRecipeProperty(target, fromTask, fromParameter, toTask, toParameter);
        }
		#endregion /method

		#region interface
		public void ShowDescriptionOnTooltip(string description)
		{
			if (string.IsNullOrWhiteSpace(description))
				return;

			_descriptionToolTip.SetToolTip(this, description);
		}
		#endregion /interface

		#region 생성자 및 소멸자
		/// <summary>
        /// 2018.06.12 by yjlee [ADD] 생성자
        /// </summary>
        public CustomParameterLabel()
        {
            DoubleBuffered = true;
			_descriptionToolTip = new ToolTip();
			_descriptionToolTip.IsBalloon = true;
        }
        #endregion
	}

    /// <summary>
    /// 2019.05.10 by ssh [ADD] 커스텀 파라미터 관련 버튼에 관한 클래스이다.
    /// 사용하기위해서는 System.Design Assembly를 프로젝트에 추가해야한다.
    /// </summary>
    /// 
    [Designer(typeof(DesignerWithTaskAxisList))]
    public class CustomParameterButton : Sys3Controls.Sys3button
    {
        #region 변수들

        #region Recipe

        #region Type of Recipe
        private Recipe.EN_RECIPE_TYPE m_enParameterType = Recipe.EN_RECIPE_TYPE.PROCESS;
        private int m_nParameterIndex = 0;
        
        private string m_selectedTask = string.Empty;
        private string m_strParameterName = string.Empty;
        private bool m_bNeedRemakeMap = false;
        private string m_AssociatedMap;
        #endregion

        #region Axis
		
        private string m_sAxisName = string.Empty;
        #endregion

		#region ForUIAfterSetValue
		public DelAftetSetValue delAfterSetValue;
		#endregion

        #endregion

        #endregion

        #region 속성

        #region Recipe

        #region Type of Recipe
        [Category("Recipe"), Description("Type of recipe parameter")]
        public Recipe.EN_RECIPE_TYPE ParameterType
        {
            set { m_enParameterType = value; }
            get { return m_enParameterType; }
        }

        [Category("Recipe"), Description("Parameter Index of Item")]
        public int ParameterIndex
        {
            set { m_nParameterIndex = value; }
            get { return m_nParameterIndex; }
        }

        [Category("Recipe"), Description("Parameter Name of Item")]
        public string ParameterName
        {
            set { m_strParameterName = value; }
            get { return m_strParameterName; }
        }

        [Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        [TaskListAttribute()]
        public string TaskName
        {
            set
            {
                m_selectedTask = value;
            }
            get { return m_selectedTask; }
        }
        [Category("Recipe"), Description("Need remake map when value changed ")]
        public bool NeedRemakeMap
        {
            set { m_bNeedRemakeMap = value; }
            get { return m_bNeedRemakeMap; }
        }

        [Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [MapList()]
        public string AssociatedMap
        {
            get
            {
                return m_AssociatedMap;
            }
            set
            {
                m_AssociatedMap = value;
            }
        }
        #endregion

        #region Axis
        
        [Browsable(false)]
        public string AxisName
        {
            get
            {
                return m_sAxisName;
            }
            set
            {
                m_sAxisName = value;
            }
        }

        #endregion

        #endregion

        #endregion

        #region 생성자 및 소멸자
        /// <summary>
        /// 2018.06.11 by yjlee [ADD] 생성자
        /// </summary>
        public CustomParameterButton()
        {
            DoubleBuffered = true;
        }

        #endregion
    }

    public class CustomParameterToggleButton : Sys3Controls.Sys3ToggleButton
    {
        #region 변수들

        #region Recipe

        #region Type of Recipe

        #region Default Type
        private Recipe.EN_RECIPE_TYPE m_enRecipeType = Recipe.EN_RECIPE_TYPE.PROCESS;
        private int m_nParameterIndex = 0;
        private string m_selectedTask = string.Empty;
        private string m_strParameterName = string.Empty;
        private bool m_bNeedRemakeMap = false;
        private string m_AssociatedMap;
        #endregion

        #endregion

		#region Parameter Change Confirm
		// 2021.10.28 by junho [ADD] Recipe Change Confirm
		private bool m_bUseParameterChangeConfirm = false;
		private Color m_clrParameterChangeDefaultActiveColorFirst = Color.RoyalBlue;
		private Color m_clrParameterChangeDefaultActiveColorSecond = Color.DodgerBlue;
		private Color m_clrParameterChangeDefaultNormalColorFirst = Color.DarkGray;
		private Color m_clrParameterChangeDefaultNormalColorSecond = Color.Silver;
		private Color m_clrParameterChangeWaitColorFirst = Color.DarkRed;
		private Color m_clrParameterChangeWaitColorSecond = Color.Firebrick;
		private bool m_bParameterStored = false;
		private string m_strParameterStorage = string.Empty;
		#endregion

		#region ForUIAfterSetValue
		public DelAftetSetValue delAfterSetValue;
		#endregion

        #endregion

		private ToolTip _descriptionToolTip;

        #endregion

        #region 속성

        #region Recipe

        #region Type of Recipe

        [Category("Recipe"), Description("Type of recipe parameter")]
        public Recipe.EN_RECIPE_TYPE ParameterType
        {
            set { m_enRecipeType = value; }
            get { return m_enRecipeType; }
        }
		[Browsable(false)]
        public int ParameterIndex
        {
            set { m_nParameterIndex = value; }
            get { return m_nParameterIndex; }
        }
        [Category("Recipe"), Description("Parameter Name of Item")]
        public string ParameterName
		{
			set
			{
				OnChangedRecipe(this, m_selectedTask, m_strParameterName, m_selectedTask, value);
				m_strParameterName = value;
			}
			get { return m_strParameterName; }
		}

		[Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        [TaskListAttribute()]
        public string TaskName
        {
            set
            {
                OnChangedRecipe(this, m_selectedTask, m_strParameterName, value, m_strParameterName);
				m_selectedTask = value;
            }
            get { return m_selectedTask; }
        }
        [Category("Recipe"), Description("Changed Value Need Remake map")]
        public bool NeedRemakeMap
        {
            set { m_bNeedRemakeMap = value; }
            get { return m_bNeedRemakeMap; }
        }

        [Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [MapList()]
        public string AssociatedMap
        {
            get
            {
                return m_AssociatedMap;
            }
            set
            {
                m_AssociatedMap = value;
            }
        }
        #endregion

        #endregion

		#region Parameter Change Confirm
		// 2021.10.28 by junho [ADD] Recipe Change Confirm
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public bool UseParameterChangeConfirm
		{
			get { return m_bUseParameterChangeConfirm; }
			set { m_bUseParameterChangeConfirm = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeDefaultActiveColorFirst
		{
			get { return m_clrParameterChangeDefaultActiveColorFirst; }
			set { m_clrParameterChangeDefaultActiveColorFirst = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeDefaultActiveColorSecond
		{
			get { return m_clrParameterChangeDefaultActiveColorSecond; }
			set { m_clrParameterChangeDefaultActiveColorSecond = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeDefaultNormalColorFirst
		{
			get { return m_clrParameterChangeDefaultNormalColorFirst; }
			set { m_clrParameterChangeDefaultNormalColorFirst = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeDefaultNormalColorSecond
		{
			get { return m_clrParameterChangeDefaultNormalColorSecond; }
			set { m_clrParameterChangeDefaultNormalColorSecond = value; }
		}

		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeWaitColorFirst
		{
			get { return m_clrParameterChangeWaitColorFirst; }
			set { m_clrParameterChangeWaitColorFirst = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeWaitColorSecond
		{
			get { return m_clrParameterChangeWaitColorSecond; }
			set { m_clrParameterChangeWaitColorSecond = value; }
		}

		[Browsable(false)]
		public string ParameterStorage
		{
			get { return m_strParameterStorage; }
			set { m_strParameterStorage = value; }
		}
		[Browsable(false)]
		public bool ParameterStored
		{
			get { return m_bParameterStored; }
			set { m_bParameterStored = value; }
		}
		#endregion

        #endregion

        public delegate void DelActiveChanged(object sender, EventArgs e);
        public event DelActiveChanged ActiveChanged;

        public delegate void DelInformParameterValueChanged(CustomParameterToggleButton sender, bool bNewValue);
        public event DelInformParameterValueChanged ParameterValueChanged;

		public delegate void DeleChangedRecipeProperty(CustomParameterToggleButton target, string fromTask, string fromParameter, string toTask, string toParameter);
		public static event DeleChangedRecipeProperty ChangedRecipeProperty = null;

		protected override void OnMouseDown(MouseEventArgs e)
        {
 	         base.OnMouseDown(e);

             ActiveChanged(this, null);
        }

        public void InformParameterValueChanged(CustomParameterToggleButton sender, bool bNewValue)
        {
            if (ParameterValueChanged != null)
            {
                ParameterValueChanged(sender, bNewValue);
            }
        }

		#region method
		private void OnChangedRecipe(CustomParameterToggleButton target, string fromTask, string fromParameter, string toTask, string toParameter)
		{
			if (ChangedRecipeProperty == null)
				return;

			ChangedRecipeProperty(target, fromTask, fromParameter, toTask, toParameter);
		}
		#endregion /method

		#region interface
		public void ShowDescriptionOnTooltip(string description)
		{
			if (string.IsNullOrWhiteSpace(description))
				return;

			_descriptionToolTip.SetToolTip(this, description);
		}
		#endregion /interface

        public CustomParameterToggleButton()
        {
            DoubleBuffered = true;
			_descriptionToolTip = new ToolTip();
			_descriptionToolTip.IsBalloon = true;
        }
    }

    /// <summary>
    /// 2021.08.02. by shkim. [ADD] 등록된 축으로 조그 이동하고 파라미터를 설정할 수 있는 버튼
    /// 사용하기위해서는 System.Design Assembly를 프로젝트에 추가해야한다.
    /// </summary>
    /// 
    [Designer(typeof(DesignerCustomJogButtonAxisList))]
    public class CustomJogButton : Sys3Controls.Sys3button
    {
        #region 변수들

        #region Recipe

        #region Type of Recipe
		// 2021.08.03 by junho [MOD] Seperate Parameter Type 1,2
		//private Recipe.EN_RECIPE_TYPE m_enParameterType = Recipe.EN_RECIPE_TYPE.PROCESS;
		private Recipe.EN_RECIPE_TYPE m_enParameterType1 = Recipe.EN_RECIPE_TYPE.PROCESS;
		private Recipe.EN_RECIPE_TYPE m_enParameterType2 = Recipe.EN_RECIPE_TYPE.PROCESS;

		// 2021.08.03 by junho [DEL] don't use
		//private int m_nParameterIndex = 0;

		// 2021.08.03 by junho [MOD] Seperate Selected Task 1,2
		//private string m_selectedTask = string.Empty;
		private string m_selectedTask1 = string.Empty;
		private string m_selectedTask2 = string.Empty;
        private string m_strParameterName1 = string.Empty;
        private string m_strParameterName2 = string.Empty;
        #endregion

        #region Axis
        private int m_nAxisCount = 1;
        private string m_sJogTitle = string.Empty;
        private string m_sAxisName1 = string.Empty;
        private string m_sAxisName2 = string.Empty;
        #endregion

		#region ForUIAfterSetValue
		public DelAftetSetValue delAfterSetValue;
		#endregion

        #endregion

        #endregion

        #region 속성

        #region Recipe

        #region Type of Recipe
		// 2021.08.03 by junho [MOD] Seperate Parameter Type 1,2
		//[Category("Recipe"), Description("Type of recipe parameter")]
		//public Recipe.EN_RECIPE_TYPE ParameterType
		//{
		//	set { m_enParameterType = value; }
		//	get { return m_enParameterType; }
		//}
		[Category("Recipe"), Description("Type of recipe parameter")]
		public Recipe.EN_RECIPE_TYPE ParameterType1
		{
			set { m_enParameterType1 = value; }
			get { return m_enParameterType1; }
		}
		[Category("Recipe"), Description("Type of recipe parameter")]
		public Recipe.EN_RECIPE_TYPE ParameterType2
		{
			set { m_enParameterType2 = value; }
			get { return m_enParameterType2; }
		}

		// 2021.08.03 by junho [DEL] don't use
		//[Category("Recipe"), Description("Parameter Index of Item")]
		//public int ParameterIndex
		//{
		//	set { m_nParameterIndex = value; }
		//	get { return m_nParameterIndex; }
		//}

        [Category("Recipe"), Description("Parameter Name1 of Item")]
        public string ParameterName1
        {
            set { m_strParameterName1 = value; }
            get { return m_strParameterName1; }
        }
        [Category("Recipe"), Description("Parameter Name1 of Item")]
        public string ParameterName2
        {
            set { m_strParameterName2 = value; }
            get { return m_strParameterName2; }
        }

		// 2021.08.03 by junho [MOD] Seperate Selected Task 1,2
		//[Category("Recipe"), Description("Task")]
		//[TypeConverter(typeof(CustomTypeConverter))]
		//[RefreshProperties(RefreshProperties.All)]
		//[TaskListAttribute()]
		//public string TaskName
		//{
		//	set
		//	{
		//		m_selectedTask = value;
		//	}
		//	get { return m_selectedTask; }
		//}
		[Category("Recipe"), Description("Task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        [TaskListAttribute()]
        public string TaskName1
        {
            set
            {
                m_selectedTask1 = value;
            }
            get { return m_selectedTask1; }
        }
		[Category("Recipe"), Description("Task")]
		[TypeConverter(typeof(CustomTypeConverter))]
		[RefreshProperties(RefreshProperties.All)]
		[TaskListAttribute()]
		public string TaskName2
		{
			set
			{
				m_selectedTask2 = value;
			}
			get { return m_selectedTask2; }
		}
        #endregion

        #region Axis
        [Category("Recipe"), Description("Jog Axis Count")]
        [RefreshProperties(RefreshProperties.All)]

        public int AxisCount
        {
            get
            {
                return m_nAxisCount;
            }
            set
            {
                if(value != m_nAxisCount)
                {
                    if (value < 1)
                    {
                        m_nAxisCount = 1;
                    }
                    else if (value > 2)
                    {
                        m_nAxisCount = 2;
                    }
                    else
                    {
                        m_nAxisCount = value;
                    }
                }
            }
        }

        [Category("Recipe"), Description("Jog Form Title")]
        public string JogTitle
        {
            get
            {
                return m_sJogTitle;
            }
            set
            {
                m_sJogTitle = value;
            }
        }

        [Browsable(false)]
        public string AxisName1
        {
            get
            {
                return m_sAxisName1;
            }
            set
            {
                m_sAxisName1 = value;
            }
        }

        [Browsable(false)]
        public string AxisName2
        {
            get
            {
                if(AxisCount != 2)
                {
                    return string.Empty;
                }
                else
                {
                    return m_sAxisName2;
                }
            }
            set
            {
                if(AxisCount != 2)
                {
                    m_sAxisName2 = string.Empty;
                }
                else
                {
                    m_sAxisName2 = value;
                }
            }
        }


        [Category("Recipe"), Description("Jog Axis Index 1")]
        public int AxisIndex1
        {
            get
            {
                Define.DefineEnumProject.Motion.EN_AXIS axis;
                if(false == Enum.TryParse<Define.DefineEnumProject.Motion.EN_AXIS>(AxisName1, out axis))
                {
                    return -1;
                }
                else
                {
                    return (int)axis;
                }
            }
        }

        [Category("Recipe"), Description("Jog Axis Index 2")]
        public int AxisIndex2
        {
            get
            {
                if (AxisCount != 2)
                {
                    return -1;
                }
                else
                {
                    Define.DefineEnumProject.Motion.EN_AXIS axis;
                    if (false == Enum.TryParse<Define.DefineEnumProject.Motion.EN_AXIS>(AxisName2, out axis))
                    {
                        return -1;
                    }
                    else
                    {
                        return (int)axis;
                    }
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region 생성자 및 소멸자
        /// <summary>
        /// 2018.06.11 by yjlee [ADD] 생성자
        /// </summary>
        public CustomJogButton()
        {
            DoubleBuffered = true;
        }

        #endregion
    }


	[Designer(typeof(DesignerWithTaskActionList))]
	public class CustomActionButton : Sys3Controls.Sys3button
	{
		#region <CONSTRUCT>
		public CustomActionButton()
		{

		}
		#endregion </CONSTRUCT>
		#region <FIELD>
		private string _taskName = string.Empty;
		private string _actionName = string.Empty;
		#endregion </FIELD>

		#region <PROPERTY>
		[Category("Information"), Description("Task")]
		[TypeConverter(typeof(CustomTypeConverter))]
		[RefreshProperties(RefreshProperties.All)]
		[TaskListAttribute()]
		public string TaskName { get { return _taskName; } set { _taskName = value; } }

		[Category("Information"), Description("Target Action")]
		[Browsable(false)]
		public string ActionName { get { return _actionName; } set { _actionName = value; } }
		#endregion </PROPERTY>

	}

    /// <summary>
    /// 2024.12.10. by shkim. [ADD] True, False Parameter에 사용
    /// </summary>
    public class CustomParameterLedLabel : Sys3LedLabelWithText
    {
		#region 변수들

		#region Recipe

		#region Type of Recipe

		#region Default Type
		private Recipe.EN_RECIPE_TYPE m_enRecipeType = Recipe.EN_RECIPE_TYPE.PROCESS;
		private int m_nParameterIndex = 0;

		private string m_selectedTask = string.Empty;
		private string m_strParameterName = string.Empty;
		private bool m_bNeedRemakeMap = false;
		private bool m_bModifiable = true;

		private string m_AssociatedMap;
		#endregion

		#region Parameter Change Confirm
		// 2021.10.28 by junho [ADD] Recipe Change Confirm
		private bool m_bUseParameterChangeConfirm = false;
		private Color m_clrParameterChangeDefaultColor = Color.Black;
		private Color m_clrParameterChangeWaitColor = Color.Red;
		private bool m_bParameterStored = false;
		private string m_strParameterStorage = string.Empty;
		#endregion

		#endregion

		#region Default Information
		private Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST m_enSelectionList = (Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST)0;
		private EN_LABEL_PARAMETER_TYPE m_enParameterType = EN_LABEL_PARAMETER_TYPE.KEYBOARD;
		private string m_strMinValue = string.Empty;
		private string m_strMaxValue = string.Empty;
		private string m_strUnitValue = string.Empty;
		private string m_strDefaultValue = string.Empty;
		#endregion

		#region Delegates
		public DelAftetSetValue delAfterSetValue;
		#endregion /Delegates

		#endregion

		private ToolTip _descriptionToolTip;

		#endregion

		#region 속성

		#region Recipe

		#region Type of Recipe

		[Category("Recipe"), Description("Choose Parameter setting type")]
		public EN_LABEL_PARAMETER_TYPE ParameterSettingType
		{
			set { m_enParameterType = value; }
			get { return m_enParameterType; }
		}

		[Category("Recipe"), Description("Choose Parameter setting type of selection list")]
		public Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST SelectionList
		{
			set
			{
				m_enSelectionList = value;
			}
			get { return m_enSelectionList; }
		}

		[Category("Recipe"), Description("Type of recipe parameter")]
		public Recipe.EN_RECIPE_TYPE ParameterType
		{
			set { m_enRecipeType = value; }
			get { return m_enRecipeType; }
		}

		[Browsable(false)]
		public int ParameterIndex
		{
			set { m_nParameterIndex = value; }
			get { return m_nParameterIndex; }
		}

		[Category("Recipe"), Description("Parameter Name of Item")]
		public string ParameterName
		{
			set { m_strParameterName = value; }
			get { return m_strParameterName; }
		}

		[Category("Recipe"), Description("Task")]
		[TypeConverter(typeof(CustomTypeConverter))]
		[RefreshProperties(RefreshProperties.All)]
		[TaskListAttribute()]
		public string TaskName
		{
			set
			{
				m_selectedTask = value;
			}
			get { return m_selectedTask; }
		}
		[Category("Recipe"), Description("Changed Value Need Remake map")]
		public bool NeedRemakeMap
		{
			set { m_bNeedRemakeMap = value; }
			get { return m_bNeedRemakeMap; }
		}

		[Category("Recipe"), Description("Task")]
		[TypeConverter(typeof(CustomTypeConverter))]
		[MapList()]
		public string AssociatedMap
		{
			get
			{
				return m_AssociatedMap;
			}
			set
			{
				m_AssociatedMap = value;
			}
		}

		[Category("Recipe"), Description("User Can Change Value")]
		public bool Modifiable
		{
			get { return m_bModifiable; }
			set { m_bModifiable = value; }
		}
		#endregion

		#region Defualt Information
		[Category("Recipe-Default"), Description("Parameter Min Value")]
		public string ParameterMIN
		{
			set { m_strMinValue = value; }
			get { return m_strMinValue; }
		}
		[Category("Recipe-Default"), Description("Parameter Max Value")]
		public string ParameterMAX
		{
			set { m_strMaxValue = value; }
			get { return m_strMaxValue; }
		}
		[Category("Recipe-Default"), Description("Parameter Unit Value")]
		public string ParameterUNIT
		{
			set { m_strUnitValue = value; }
			get { return m_strUnitValue; }
		}
		[Category("Recipe-Default"), Description("Parameter Unit Value")]
		public string ParameterDefaultValue
		{
			set { m_strDefaultValue = value; }
			get { return m_strDefaultValue; }
		}
		#endregion

		#endregion

		#region Parameter Change Confirm
		// 2021.10.28 by junho [ADD] Recipe Change Confirm
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public bool UseParameterChangeConfirm
		{
			get { return m_bUseParameterChangeConfirm; }
			set { m_bUseParameterChangeConfirm = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeDefaultColor
		{
			get { return m_clrParameterChangeDefaultColor; }
			set { m_clrParameterChangeDefaultColor = value; }
		}
		[Category("ParameterConfirm"), Description("Recipe Change Confirm Option")]
		public Color ParameterChangeWaitColor
		{
			get { return m_clrParameterChangeWaitColor; }
			set { m_clrParameterChangeWaitColor = value; }
		}
		[Browsable(false)]
		public string ParameterStorage
		{
			get { return m_strParameterStorage; }
			set { m_strParameterStorage = value; }
		}
		[Browsable(false)]
		public bool ParameterStored
		{
			get { return m_bParameterStored; }
			set { m_bParameterStored = value; }
		}
		#endregion

		#endregion

		public delegate void DelActiveChanged(object sender, EventArgs e);
		public event DelActiveChanged ActiveChanged;

		public delegate void DelInformParameterValueChanged(CustomParameterLedLabel sender, bool bNewValue);
		public event DelInformParameterValueChanged ParameterValueChanged;

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);

			ActiveChanged(this, null);
		}

		public void InformParameterValueChanged(CustomParameterLedLabel sender, bool bNewValue)
		{
			if (ParameterValueChanged != null)
			{
				ParameterValueChanged(sender, bNewValue);
			}
		}

		#region interface
		public void ShowDescriptionOnTooltip(string description)
		{
			if (string.IsNullOrWhiteSpace(description))
				return;

			_descriptionToolTip.SetToolTip(this, description);
		}
		#endregion /interface

		public CustomParameterLedLabel()
		{
			DoubleBuffered = true;
			_descriptionToolTip = new ToolTip();
			_descriptionToolTip.IsBalloon = true;
		}
	}
}