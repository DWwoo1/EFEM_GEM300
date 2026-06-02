using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Define.DefineConstant;

using Interlock_;

namespace FrameOfSystem3.Views.Config
{
	public partial class Interlock_Cylinder : UserControlForMainView.CustomView
	{
        public Interlock_Cylinder()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfStorage							= FrameOfSystem3.Functional.Storage.GetInstance();

			m_InstanceOfMotion							= FrameOfSystem3.Config.ConfigMotion.GetInstance();
            m_InstanceOfCylinder                        = FrameOfSystem3.Config.ConfigCylinder.GetInstance();
            m_InstanceOfDigital                         = FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();

			m_InstanceOfConfigInterlock					= FrameOfSystem3.Config.ConfigInterlock.GetInstance();
			m_InstanceOfInterlock						= Interlock.GetInstance();
			m_InstanceOfSelectionList					= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfCalculator						= Functional.Form_Calculator.GetInstance();
			m_InstanceOfKeyboard						= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfMessageBox						= Functional.Form_MessageBox.GetInstance();
			#endregion

			#region MakeMapping
			foreach(FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE enDevice in Enum.GetValues(typeof(FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE)))
			{
				m_dicForDevice.Add(enDevice.ToString(), enDevice);
			}
			#endregion
		}

		#region 상수
		private const int STARTING_INDEX				= 0;
		private const int SELECT_NONE					= -1;

		#region GUI
		private const int HEIGHT_OF_ROWS				= 30;

		private const int COLUMN_INDEX_OF_INDEX			= 0;
		private const int COLUMN_INDEX_OF_NAME			= 1;
		private const int COLUMN_INDEX_OF_TARGET		= 2;

		// 미사용?
		//private readonly string MIN_VALUE				= "1";
		//private readonly string MAX_VALUE				= "999999";
		//private readonly string DEFAULT_LABEL			= "--";
		#endregion

        private readonly Color c_clrTrue = Color.DodgerBlue;
        private readonly Color c_clrFalse = Color.White;
		#endregion

		#region 변수

		#region Instance
		Functional.Form_SelectionList m_InstanceOfSelectionList			= null;
        Functional.Form_Calculator m_InstanceOfCalculator = null;
        Functional.Form_Keyboard m_InstanceOfKeyboard = null;
		Functional.Form_MessageBox m_InstanceOfMessageBox				= null;

		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion			= null;
		FrameOfSystem3.Config.ConfigCylinder m_InstanceOfCylinder       = null;
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDigital			= null;
        FrameOfSystem3.Config.ConfigInterlock m_InstanceOfConfigInterlock = null;
        Interlock m_InstanceOfInterlock = null;

		FrameOfSystem3.Functional.Storage m_InstanceOfStorage			= null;
		#endregion

		private Dictionary<string, FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE> m_dicForDevice	= new Dictionary<string,FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE>();

        private int m_nSelectedCylinder = -1;

        private int m_nSelectedRowCylinder = 0;
		private int m_nSelectedRowInterlockCondition					= 0;
		private int m_nSelectedRowCompareGroup			    		= 0;
		private int m_nSelectedRowCompareCondition			    		= 0;

		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
		{
            UpdateCylinderList();
//             UpdateInterLockCompareGroupList();
//             UpdateInterLockCompareItemList();
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
		/// 2020.11.16 by twkang [ADD] 라벨들의 값을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
            m_nSelectedCylinder = -1;
            m_nSelectedRowCompareGroup = -1;

			SetActiveControls(false);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 컨트롤의 활성화 유무 설정
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{

		}

		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		#endregion

		#region UI인터페이스
        #region Grid Update
        private void UpdateCylinderList()
        {
            m_dgViewCylinder.Rows.Clear();
            int[] arIndex = new int[] { };
            bool[] arEnable = new bool[] { };
            m_InstanceOfInterlock.GetInterlockCylinderList(ref arIndex, ref arEnable);
            for (int nRow = 0; nRow < arIndex.Length; nRow++)
            {
                m_dgViewCylinder.Rows.Add();
                m_dgViewCylinder[0, nRow].Value = arIndex[nRow];
                m_dgViewCylinder[1, nRow].Style.BackColor = arEnable[nRow] ? c_clrTrue : c_clrFalse;
                m_dgViewCylinder[1, nRow].Style.SelectionBackColor = arEnable[nRow] ? c_clrTrue : c_clrFalse;
                m_dgViewCylinder[2, nRow].Value = m_InstanceOfCylinder.GetName(arIndex[nRow]);
            }
            
            if (m_dgViewCylinder.Rows.Count > 0)
                m_dgViewCylinder.Rows[m_nSelectedRowCylinder].Selected = true;
        }

        private void UpdateInterLockConditionList()
        {
            m_dgViewCondition.Rows.Clear();

            object[] arCompareKey = new object[] { };
            object[] arThreshold = new object[] { };
            object[] arDirection = new object[] { };
            object[] arMovingDirection = new object[] { };

            // 2022.12.30  by jhlee [ADD] arDiretion 추가
            if (m_InstanceOfInterlock.GetCylinderInterLockConditionList(m_nSelectedCylinder, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, ref arCompareKey)
             && m_InstanceOfInterlock.GetCylinderInterLockConditionList(m_nSelectedCylinder, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD, ref arThreshold)
             && m_InstanceOfInterlock.GetCylinderInterLockConditionList(m_nSelectedCylinder, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, ref arDirection))
            {
                for (int nRow = 0; nRow < arCompareKey.Length; nRow++)
                {
                    m_dgViewCondition.Rows.Add();
                    m_dgViewCondition[0, nRow].Value = arCompareKey[nRow];
                    if((bool)arDirection[nRow])
                        m_dgViewCondition[1, nRow].Value = EN_MOTION_COMPARE_DIRECTION.OVER;
                    else
                        m_dgViewCondition[1, nRow].Value = EN_MOTION_COMPARE_DIRECTION.UNDER;
                }
            }
            if (m_dgViewCondition.Rows.Count > 0)
                m_dgViewCondition.Rows[m_nSelectedRowInterlockCondition].Selected = true;
        }

        private void UpdateCompareGroupList()
        {
            m_dgViewCompareGroup.Rows.Clear();

            object[] arIndex = new object[] { };
            object[] arEnable = new object[] { };
            object[] arName = new object[] { };

            if (m_InstanceOfInterlock.GetCylinderCompareGroupList(m_nSelectedCylinder, EN_COMPARE_GROUP_PARAMTER_TYPE.INDEX, ref arIndex)
           && m_InstanceOfInterlock.GetCylinderCompareGroupList(m_nSelectedCylinder, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, ref arEnable)
           && m_InstanceOfInterlock.GetCylinderCompareGroupList(m_nSelectedCylinder, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, ref arName))
            {
                for (int nRow = 0; nRow < arIndex.Length; nRow++)
                {
                    m_dgViewCompareGroup.Rows.Add();
                    m_dgViewCompareGroup[0, nRow].Value = arIndex[nRow];
                    m_dgViewCompareGroup[1, nRow].Style.BackColor = (bool)arEnable[nRow] ? c_clrTrue : c_clrFalse;
                    m_dgViewCompareGroup[1, nRow].Style.SelectionBackColor = (bool)arEnable[nRow] ? c_clrTrue : c_clrFalse;
                    m_dgViewCompareGroup[2, nRow].Value = arName[nRow];
                }
            }
            if (m_dgViewCompareGroup.Rows.Count > 0)
                m_dgViewCompareGroup.Rows[m_nSelectedRowCompareGroup].Selected = true;
        }

        private void UpdateCompareConditionList()
        {
            m_dgViewCompareCondition.Rows.Clear();


            object[] arGroupKey = new object[] { };
            object[] arDevice = new object[] { };
            object[] arDeviceIndex = new object[] { };
            object[] arRelativeIndex = new object[] { };
            object[] arDirection = new object[] { };
            object[] arThreshold = new object[] { };

            if (Interlock.GetInstance().GetCylinderCompareConditionList(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, ref arGroupKey)
                && Interlock.GetInstance().GetCylinderCompareConditionList(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, ref arDevice)
                && Interlock.GetInstance().GetCylinderCompareConditionList(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, ref arDeviceIndex)
                && Interlock.GetInstance().GetCylinderCompareConditionList(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, ref arRelativeIndex)
                && Interlock.GetInstance().GetCylinderCompareConditionList(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, ref arDirection)
                && Interlock.GetInstance().GetCylinderCompareConditionList(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, ref arThreshold))
            {
                for (int nRow = 0; nRow < arGroupKey.Length; nRow++)
                {
                    m_dgViewCompareCondition.Rows.Add();
                    m_dgViewCompareCondition[0, nRow].Value = arGroupKey[nRow];
                    m_dgViewCompareCondition[1, nRow].Value = arDevice[nRow];
                    m_dgViewCompareCondition[2, nRow].Value = arDeviceIndex[nRow];
                    m_dgViewCompareCondition[3, nRow].Value = arRelativeIndex[nRow];
                    if ((EN_INTERLOCK_DEVICE)arDevice[nRow] == EN_INTERLOCK_DEVICE.MOTION
                        || (EN_INTERLOCK_DEVICE)arDevice[nRow] == EN_INTERLOCK_DEVICE.MOTION_RELATIVE)
                    {
                        m_dgViewCompareCondition[4, nRow].Style.BackColor = Color.Empty;
                        m_dgViewCompareCondition[4, nRow].Style.SelectionBackColor = Color.Empty;
                        m_dgViewCompareCondition[4, nRow].Value = (bool)arDirection[nRow] ? EN_MOTION_COMPARE_DIRECTION.OVER : EN_MOTION_COMPARE_DIRECTION.UNDER;
                    }
                    else if ((EN_INTERLOCK_DEVICE)arDevice[nRow] == EN_INTERLOCK_DEVICE.CYLINDER)
                    {
                        m_dgViewCompareCondition[4, nRow].Style.BackColor = Color.Empty;
                        m_dgViewCompareCondition[4, nRow].Style.SelectionBackColor = Color.Empty;
                        m_dgViewCompareCondition[4, nRow].Value = (bool)arDirection[nRow] ? EN_CYLINDER_COMPARE_DIRECTION.FORWARD : EN_CYLINDER_COMPARE_DIRECTION.BACKWARD;
                    }
                    else
                    {
                        m_dgViewCompareCondition[4, nRow].Style.BackColor = (bool)arDirection[nRow] ? c_clrTrue : c_clrFalse;
                        m_dgViewCompareCondition[4, nRow].Style.SelectionBackColor = (bool)arDirection[nRow] ? c_clrTrue : c_clrFalse;
                    }
                    m_dgViewCompareCondition[5, nRow].Value = arThreshold[nRow];
                }
            }
            if (m_dgViewCompareCondition.Rows.Count > 0)
                m_dgViewCompareCondition.Rows[m_nSelectedRowCompareCondition].Selected = true;
        }
        #endregion /Grid Update
        #endregion

        #region UI Event
        private void Click_AddButton(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            switch (ctrl.TabIndex)
            {
                case 0: // Axis ADD
                    int[] nArrIndex = new int[0];
                    string[] strArrName = new string[0];
                    m_InstanceOfCylinder.GetListOfItem(ref nArrIndex);
                    m_InstanceOfCylinder.GetListOfName(ref strArrName);
                    if (m_InstanceOfSelectionList.CreateForm("CYLINDER", strArrName, nArrIndex, 0))
                    {
                        int nValue = 0;
                     
                        m_InstanceOfSelectionList.GetResult(ref nValue);

                        m_InstanceOfConfigInterlock.AddCylinder(nValue);
                    }
                    UpdateCylinderList();
                    break;

                case 1: // Interlock Condition ADD
                    m_InstanceOfConfigInterlock.AddCylinderInterlockCondition(m_nSelectedCylinder, m_dgViewCondition.Rows.Count);
                    UpdateInterLockConditionList();
                    break;
                case 2: // CompareGroup ADD
                    m_InstanceOfConfigInterlock.AddCylinderCompareGroup(m_nSelectedCylinder, m_dgViewCompareGroup.Rows.Count);
                    UpdateCompareGroupList();
                    break;

                case 3: // Compare Condition   ADD
                    m_InstanceOfConfigInterlock.AddCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_dgViewCompareCondition.Rows.Count);
                    UpdateCompareConditionList();
                    break;
            }
        }

        private void Click_SetPosition(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            switch (ctrl.TabIndex)
            {
                case 1: // Compare Item
                    double dPosition = 0;
					if (m_dgViewCompareCondition.Rows.Count <= m_nSelectedRowCompareCondition) return;

                    EN_INTERLOCK_DEVICE enDevice = (EN_INTERLOCK_DEVICE)m_dgViewCompareCondition.Rows[m_nSelectedRowCompareCondition].Cells[1].Value;
                    if (enDevice == EN_INTERLOCK_DEVICE.MOTION)
                    {
                        int nDeviceIndex = (int)m_dgViewCompareCondition.Rows[m_nSelectedRowCompareCondition].Cells[2].Value;
                        
                        dPosition = m_InstanceOfMotion.GetActualPosition(nDeviceIndex);

                        m_InstanceOfInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, dPosition);

                        UpdateCompareConditionList();
                    }
                    if (enDevice == EN_INTERLOCK_DEVICE.MOTION_RELATIVE)
                    {
                        int nDeviceIndex = (int)m_dgViewCompareCondition.Rows[m_nSelectedRowCompareCondition].Cells[2].Value;
                        int nRelativeIndex = (int)m_dgViewCompareCondition.Rows[m_nSelectedRowCompareCondition].Cells[3].Value;

                        dPosition = m_InstanceOfMotion.GetActualPosition(nRelativeIndex) - m_InstanceOfMotion.GetActualPosition(nDeviceIndex);
                        dPosition = Math.Abs(Math.Round(dPosition, 3));

                        m_InstanceOfInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, dPosition);

                        UpdateCompareConditionList();
                    }
                    break;
            }
        }

        #region Axis
        private void Click_CylinderGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCylinder.RowCount) { return; }

            int nIndex = int.Parse(m_dgViewCylinder[0, nRowindex].Value.ToString());

            m_nSelectedCylinder = nIndex;
            m_nSelectedRowCylinder = nRowindex;
            m_nSelectedRowInterlockCondition = 0;
            m_nSelectedRowCompareGroup = 0;
            m_nSelectedRowCompareCondition = 0;
            UpdateInterLockConditionList();
            UpdateCompareGroupList();
            UpdateCompareConditionList();
        }
        private void DoubleClick_CylinderGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCylinder.RowCount) { return; }

            int nIndex = int.Parse(m_dgViewCylinder[0, nRowindex].Value.ToString());

            m_nSelectedCylinder = nIndex;
            m_nSelectedRowCylinder = nRowindex;
            m_nSelectedRowInterlockCondition = 0;
            m_nSelectedRowCompareCondition = 0;
            m_nSelectedRowCompareGroup = 0;
            UpdateInterLockConditionList();
            UpdateCompareGroupList();
            UpdateCompareConditionList();

            switch (nColumnIndex)
            {
                case 1:
                    if (m_InstanceOfSelectionList.CreateForm(SelectionList.ENABLE
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , m_dgViewCylinder.Rows[nRowindex].Cells[1].Style.BackColor == c_clrTrue ? SelectionList.ENABLE : SelectionList.DISABLE
                        , false
                        , SelectionList.DISABLE))
                    {
                        string strResult = "";
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        bool bResult = strResult.Equals(SelectionList.ENABLE);
                        m_InstanceOfConfigInterlock.SetCylinderEnable(m_nSelectedCylinder, bResult);
                        UpdateCylinderList();
                    }
                    break;
            }
        }
        #endregion 

        #region Interlock Condition
        private void Click_InterlockConditionGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCondition.RowCount) { return; }

            m_nSelectedRowInterlockCondition = nRowindex;
        }
        private void DoubleClick_InterLockConditionGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCondition.RowCount) { return; }

            m_nSelectedRowInterlockCondition = nRowindex;

            int nCompareKey = (int)m_dgViewCondition.Rows[nRowindex].Cells[0].Value;
            bool bDirection = m_dgViewCondition.Rows[nRowindex].Cells[1].Value.Equals(EN_CYLINDER_COMPARE_DIRECTION.FORWARD);

            switch (nColumnIndex)
            {
                case 0:
                    if (m_InstanceOfCalculator.CreateForm(nCompareKey.ToString(), "1", "100"))
                    {
                        m_InstanceOfCalculator.GetResult(ref nCompareKey);
                        m_InstanceOfConfigInterlock.SetCylinderInterlockCondition(m_nSelectedCylinder, m_nSelectedRowInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, nCompareKey);

                    }
                    break;
                case 1:
                    if (m_InstanceOfSelectionList.CreateForm("CYLINDER COMPARE DIRECTION"
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.CYLINDER_COMPARE_DIRECTION
                        , m_dgViewCondition.Rows[nRowindex].Cells[1].Value.ToString()))
                    {
                        string strResult = "";
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        bDirection = strResult.Equals(EN_CYLINDER_COMPARE_DIRECTION.FORWARD.ToString());
   m_InstanceOfConfigInterlock.SetCylinderInterlockCondition(m_nSelectedCylinder, m_nSelectedRowInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, bDirection);
                    }
                    break;
            }
            UpdateInterLockConditionList();
        }
        #endregion

        #region Compare Group
        private void Click_CompareGroupGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCompareGroup.RowCount) { return; }

            m_nSelectedRowCompareGroup = nRowindex;
            m_nSelectedRowCompareCondition = 0;
            UpdateCompareConditionList();
        }
        private void DoubleClick_CompareGroupGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCompareGroup.RowCount) { return; }

            int nIndex = int.Parse(m_dgViewCompareGroup[0, nRowindex].Value.ToString());

            m_nSelectedRowCompareGroup = nIndex;
            m_nSelectedRowCompareCondition = 0;
            UpdateCompareConditionList();

            bool bEnable = m_dgViewCompareGroup.Rows[nRowindex].Cells[1].Style.BackColor.Equals(c_clrTrue);
            string strName = m_dgViewCompareGroup.Rows[nRowindex].Cells[2].Value.ToString();

            switch (nColumnIndex)
            {
                case 1:
                    if (m_InstanceOfSelectionList.CreateForm(SelectionList.ENABLE
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , m_dgViewCompareGroup.Rows[nRowindex].Cells[1].Style.BackColor == c_clrTrue ? SelectionList.ENABLE : SelectionList.DISABLE
                        , false
                        , SelectionList.DISABLE))
                    {
                        string strResult = "";
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        bEnable = strResult.Equals(SelectionList.ENABLE);
                        m_InstanceOfConfigInterlock.SetCylinderCompareGroup(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, bEnable);
                    }
                    break;
                case 2:
                    if (m_InstanceOfKeyboard.CreateForm())
                    {
                        m_InstanceOfKeyboard.GetResult(ref strName);
                        m_InstanceOfConfigInterlock.SetCylinderCompareGroup(m_nSelectedCylinder, m_nSelectedRowCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, strName);
                    }
                    break;
            }

            UpdateCompareGroupList();

        }
        #endregion

        #region Compare Item
        private void Click_CompareConditionGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCompareCondition.RowCount) { return; }

            m_nSelectedRowCompareCondition = nRowindex;
        }
        private void DoubleClick_CompareConditionGrid(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewCompareCondition.RowCount) { return; }


            m_nSelectedRowCompareCondition = nRowindex;

            int nGroupKey = (int)m_dgViewCompareCondition.Rows[nRowindex].Cells[0].Value;
            EN_INTERLOCK_DEVICE enDevice = (EN_INTERLOCK_DEVICE)m_dgViewCompareCondition.Rows[nRowindex].Cells[1].Value;
            int nDeviceIndex = (int)m_dgViewCompareCondition.Rows[nRowindex].Cells[2].Value;
            int nRelativeDeviceIndex = (int)m_dgViewCompareCondition.Rows[nRowindex].Cells[3].Value;
            bool bDirection = true;
            if (enDevice == EN_INTERLOCK_DEVICE.MOTION
                || enDevice == EN_INTERLOCK_DEVICE.MOTION_RELATIVE)
                bDirection = (EN_MOTION_COMPARE_DIRECTION)m_dgViewCompareCondition.Rows[nRowindex].Cells[4].Value 
                    == EN_MOTION_COMPARE_DIRECTION.OVER;
            else if (enDevice == EN_INTERLOCK_DEVICE.CYLINDER)
                bDirection = (EN_CYLINDER_COMPARE_DIRECTION)m_dgViewCompareCondition.Rows[nRowindex].Cells[4].Value 
                    == EN_CYLINDER_COMPARE_DIRECTION.FORWARD;
            else
                bDirection = m_dgViewCompareCondition.Rows[nRowindex].Cells[4].Style.BackColor == c_clrTrue;
            double dThereshold = (double)m_dgViewCompareCondition.Rows[nRowindex].Cells[5].Value;

            int[] nArrIndex = new int[0];
            string[] strArrName = new string[0];

            switch (nColumnIndex)
            {
                case 0:
                    if (m_InstanceOfCalculator.CreateForm(nGroupKey.ToString(), "0", "1000"))
                    {
                        m_InstanceOfCalculator.GetResult(ref nGroupKey);
                        m_InstanceOfConfigInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, nGroupKey);
                    }
                    break;
                case 1:
                    if (m_InstanceOfSelectionList.CreateForm(""
                      , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.INTERLOCK_COMPARE_DEVICE
                      , enDevice.ToString()))
                    {
                        string strResult = "";
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        Enum.TryParse(strResult, out enDevice);
                        m_InstanceOfConfigInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, enDevice);
                    }
                    break;
                case 2:
                 
                    switch (enDevice)
                    {
                        default:
                            if (m_InstanceOfCalculator.CreateForm(nDeviceIndex.ToString(), "0", "1000"))
                            {
                                m_InstanceOfCalculator.GetResult(ref nDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.MOTION:
                        case EN_INTERLOCK_DEVICE.MOTION_RELATIVE:
                            m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
                            m_InstanceOfMotion.GetListOfName(ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("AXIS", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.CYLINDER:
                            m_InstanceOfCylinder.GetListOfItem(ref nArrIndex);
                            m_InstanceOfCylinder.GetListOfName(ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("CYLINDER", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.DI:
                            m_InstanceOfDigital.GetListOfItem(true, ref nArrIndex);
                            m_InstanceOfDigital.GetListOfName(true, ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("DIGITAL INPUT", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.DO:
                            m_InstanceOfDigital.GetListOfItem(false, ref nArrIndex);
                            m_InstanceOfDigital.GetListOfName(false, ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("DIGITAL OUTPUT", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nDeviceIndex);
                            }
                            break;
                    }
                    m_InstanceOfConfigInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, nDeviceIndex);
                    break;
                case 3:
                    switch (enDevice)
                    {
                        default:
                            if (m_InstanceOfCalculator.CreateForm(nRelativeDeviceIndex.ToString(), "0", "1000"))
                            {
                                m_InstanceOfCalculator.GetResult(ref nRelativeDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.MOTION:
                        case EN_INTERLOCK_DEVICE.MOTION_RELATIVE:
                            m_InstanceOfMotion.GetListOfItem(ref nArrIndex);
                            m_InstanceOfMotion.GetListOfName(ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("AXIS", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nRelativeDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.CYLINDER:
                           m_InstanceOfCylinder.GetListOfItem(ref nArrIndex);
                           m_InstanceOfCylinder.GetListOfName(ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("CYLINDER", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nRelativeDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.DI:
                            m_InstanceOfDigital.GetListOfItem(true, ref nArrIndex);
                            m_InstanceOfDigital.GetListOfName(true, ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("DIGITAL INPUT", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nRelativeDeviceIndex);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.DO:
                            m_InstanceOfDigital.GetListOfItem(false, ref nArrIndex);
                            m_InstanceOfDigital.GetListOfName(false, ref strArrName);
                            if (m_InstanceOfSelectionList.CreateForm("DIGITAL OUTPUT", strArrName, nArrIndex, 0))
                            {
                                m_InstanceOfSelectionList.GetResult(ref nRelativeDeviceIndex);
                            }
                            break;
                    }
                    m_InstanceOfConfigInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, nRelativeDeviceIndex);
                    break;
                case 4:
                    switch (enDevice)
                    {
                        default:
                            if (m_InstanceOfSelectionList.CreateForm(SelectionList.TRUE
                                , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.TRUE_FALSE
                                , bDirection ? SelectionList.TRUE : SelectionList.FALSE))
                            {
                                string strResult = "";
                                m_InstanceOfSelectionList.GetResult(ref strResult);
                                bDirection = strResult.Equals(SelectionList.TRUE);
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.MOTION:
                        case EN_INTERLOCK_DEVICE.MOTION_RELATIVE:
                            if (m_InstanceOfSelectionList.CreateForm("MOTION COMPARE DIRECTION"
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.MOTION_COMPARE_DIRECTION
                        , m_dgViewCompareCondition.Rows[nRowindex].Cells[3].Value.ToString()))
                            {
                                string strResult = "";
                                m_InstanceOfSelectionList.GetResult(ref strResult);
                                bDirection = strResult.Equals(EN_MOTION_COMPARE_DIRECTION.OVER.ToString());
                            }
                            break;
                        case EN_INTERLOCK_DEVICE.CYLINDER:
                            if (m_InstanceOfSelectionList.CreateForm("CYLINDER COMPARE DIRECTION"
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.CYLINDER_COMPARE_DIRECTION
                        , m_dgViewCompareCondition.Rows[nRowindex].Cells[3].Value.ToString()))
                            {
                                string strResult = "";
                                m_InstanceOfSelectionList.GetResult(ref strResult);
                                bDirection = strResult.Equals(EN_CYLINDER_COMPARE_DIRECTION.FORWARD.ToString());
                            }
                            break;
                    }
                    m_InstanceOfConfigInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, bDirection);
                    break;
                case 5:
                    if (m_InstanceOfCalculator.CreateForm(dThereshold.ToString(), "-1000", "1000"))
                    {
                        m_InstanceOfCalculator.GetResult(ref dThereshold);
                        m_InstanceOfConfigInterlock.SetCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, dThereshold);
                    }
                    break;
            }

            UpdateCompareConditionList();

        }
        #endregion

        private void Click_RemoveButton(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            switch (ctrl.TabIndex)
            {
                case 0: // Axis Remove
                    m_InstanceOfConfigInterlock.RemoveCylinder(m_nSelectedCylinder);
                    m_nSelectedCylinder = 0;                   
                    UpdateCylinderList();
                    break;

                case 1: // Interlock Condition Remove
                    m_InstanceOfConfigInterlock.RemoveCylinderInterlockCondition(m_nSelectedCylinder, m_nSelectedRowInterlockCondition);
                    m_nSelectedRowInterlockCondition = 0;
                    UpdateInterLockConditionList();
                    break;
                case 2: // CompareGroup Remove
                    m_InstanceOfConfigInterlock.RemoveCylinderCompareGroup(m_nSelectedCylinder, m_nSelectedRowCompareGroup);
                    m_nSelectedRowCompareGroup = 0;
                    UpdateCompareGroupList();
                    break;

                case 3: // Compare Condition Remove
                    m_InstanceOfConfigInterlock.RemoveCylinderCompareCondition(m_nSelectedCylinder, m_nSelectedRowCompareGroup, m_nSelectedRowCompareCondition);
                    m_nSelectedRowCompareCondition = 0;
                    UpdateCompareConditionList();
                    break;
            }
        }

        #endregion

    }
}
