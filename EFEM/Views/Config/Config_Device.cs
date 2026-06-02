using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Config
{
	public partial class Config_Device : UserControlForMainView.CustomView
	{
		public Config_Device()
		{
			InitializeComponent();

			#region Instance
			m_InstanceOfStorage							= FrameOfSystem3.Functional.Storage.GetInstance();
			m_InstanceOfDynamicLink = FrameOfSystem3.Config.ConfigDynamicLink.GetInstance();
			m_InstanceOfAnalogIO						= FrameOfSystem3.Config.ConfigAnalogIO.GetInstance();
			m_InstanceOfCylinder						= FrameOfSystem3.Config.ConfigCylinder.GetInstance();
			m_InstanceOfDigitalIO						= FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();
			m_InstanceOfMotion							= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceOfDevice							= FrameOfSystem3.Config.ConfigDevice.GetInstance();
			m_InstanceOfSelectionList					= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfCalculator						= Functional.Form_Calculator.GetInstance();
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

		private readonly string MIN_VALUE				= "1";
		private readonly string MAX_VALUE				= "999999";
		private readonly string DEFAULT_LABEL			= "--";
		#endregion

		#endregion

		#region 변수

		#region Instance
		Functional.Form_SelectionList m_InstanceOfSelectionList			= null;
		Functional.Form_Calculator m_InstanceOfCalculator				= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox				= null;
		FrameOfSystem3.Config.ConfigDynamicLink m_InstanceOfDynamicLink = null;
		FrameOfSystem3.Config.ConfigAnalogIO m_InstanceOfAnalogIO		= null;
		FrameOfSystem3.Config.ConfigCylinder m_InstanceOfCylinder		= null;
		FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDigitalIO		= null;
		FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion			= null;
		FrameOfSystem3.Config.ConfigDevice m_InstanceOfDevice			= null;

		FrameOfSystem3.Functional.Storage m_InstanceOfStorage			= null;
		#endregion

		private Dictionary<string, FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE> m_dicForDevice	= new Dictionary<string,FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE>();

		private int m_nSelectedRowTask							= -1;
		private string m_strSelectedTask						= string.Empty;

		private int m_nSelectedRowDevice						= -1;
		private string m_strSelectedDevice						= string.Empty;

		private int m_nSelectedRowItem							= -1;
		private int m_nSelectedItemIndex						= -1;
		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
		{
			UpdateTaskList();

			UpdateDeviceList();
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
		/// 2020.11.05 by twkang [ADD] TaskList 를 업데이트한다.
		/// </summary>
		private void UpdateTaskList()
		{
			m_dgViewTask.Rows.Clear();

			int[] arTaksIndex	= null;
			string[] arTask		= null;

			m_InstanceOfDynamicLink.GetListOfTask(ref arTaksIndex, ref arTask);
			
			if(arTask != null)
			{
				for (int nIndex = 0, nEnd = arTask.Length; nIndex < nEnd; ++nIndex)
				{
					m_dgViewTask.Rows.Add();

					m_dgViewTask[COLUMN_INDEX_OF_INDEX, nIndex].Value	= arTaksIndex[nIndex];
					m_dgViewTask[COLUMN_INDEX_OF_NAME, nIndex].Value	= arTask[nIndex];
				}

				m_dgViewTask.Rows[STARTING_INDEX].Selected	= true;

				m_nSelectedRowTask	= STARTING_INDEX;
				m_strSelectedTask	= arTask[STARTING_INDEX];
			}
			else
			{
				m_nSelectedRowTask	= SELECT_NONE;
			}
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] DeviceList 를 업데이트한다.
		/// </summary>
		private void UpdateDeviceList()
		{
			m_dgViewDevice.Rows.Clear();

			if (SELECT_NONE == m_nSelectedRowTask) { return; }
			
			int nCount	= 0;

			foreach(FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE enList in Enum.GetValues(typeof(FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE)))
			{
				m_dgViewDevice.Rows.Add();
				m_dgViewDevice[COLUMN_INDEX_OF_INDEX, nCount].Value		= nCount;
				m_dgViewDevice[COLUMN_INDEX_OF_NAME, nCount].Value		= enList.ToString();
				++nCount;
			}

			m_nSelectedRowDevice		= STARTING_INDEX;
			m_strSelectedDevice			= FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION.ToString();

			UpdateItemList();
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] 아이템 리스트를 업데이트한다.
		/// </summary>
		private void UpdateItemList()
		{
			ResetSelectedItem();

			if(m_nSelectedRowDevice == SELECT_NONE || m_nSelectedRowTask == SELECT_NONE)
			{
				return;
			}

			string strTask			= m_dgViewTask[COLUMN_INDEX_OF_NAME, m_nSelectedRowTask].Value.ToString();
			string strDeviceType	= m_dgViewDevice[COLUMN_INDEX_OF_NAME, m_nSelectedRowDevice].Value.ToString();

			int nCountOfDevice		= -1;
			int[] arDevice			= null;

			m_InstanceOfDevice.GetIndexesOfDevice(strTask, m_dicForDevice[strDeviceType], ref nCountOfDevice, ref arDevice);
			
			m_dgViewItem.Rows.Clear();

			if(nCountOfDevice > 0)
			{
				for(int nIndex = 0; nIndex < nCountOfDevice; ++nIndex)
				{
					AddItemOnGrid(arDevice[nIndex]);
				}
			}
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Item Grid View 에 아이템을 추가한다.
		/// </summary>
		private void AddItemOnGrid(int nIndexOfItem)
		{
			if (nIndexOfItem == STARTING_INDEX) { return; }

			int nIndexOfRow		= m_dgViewItem.Rows.Count;

			m_dgViewItem.Rows.Add();

			string strValue			= string.Empty;
			int nValue				= -1;

			m_dgViewItem.Rows[nIndexOfRow].Height					= HEIGHT_OF_ROWS;
			m_dgViewItem[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value	= nIndexOfItem;

			if(m_dicForDevice.ContainsKey(m_strSelectedDevice))
			{
				m_InstanceOfDevice.GetDeviceTargetName(m_strSelectedTask, m_dicForDevice[m_strSelectedDevice], nIndexOfItem, ref strValue);
				m_dgViewItem[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value		= strValue;

				m_InstanceOfDevice.GetDeviceTargetIndex(m_strSelectedTask, m_dicForDevice[m_strSelectedDevice], nIndexOfItem, ref nValue);
				m_dgViewItem[COLUMN_INDEX_OF_TARGET, nIndexOfRow].Value		= nValue;
			}

			m_dgViewItem.Rows[nIndexOfRow].Selected		= false;
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 아이템의 이름을 가져온다.
		/// </summary>
		private bool GetDeviceName(FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE enType, int nIndex, ref string strName)
		{
			switch(enType)
			{
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_OUTPUT:
					return m_InstanceOfAnalogIO.GetParameter(false, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.NAME, ref strName);
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT:
					return m_InstanceOfAnalogIO.GetParameter(true, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.NAME, ref strName);
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.CYLINDER:
					return m_InstanceOfCylinder.GetParameter(nIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.NAME, ref strName);
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_OUTPUT:
					return m_InstanceOfDigitalIO.GetParameter(false, nIndex, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strName);
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_INPUT:
					return m_InstanceOfDigitalIO.GetParameter(true, nIndex, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strName);
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION:
					return m_InstanceOfMotion.GetMotionParameter(nIndex, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strName);
				default:
					return false;
			}
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 특정 타입의 아이템 리스트를 가져온다.
		/// </summary>
		private void GetItemIndexFromSelectionList(FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE enType, ref int nIndexOfSelected,  int nPreSelected = -1)
		{
			string[] arItem	= null;
			int[] arIndex	= null;
			
			switch(enType)
			{
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_OUTPUT:
					if(m_InstanceOfAnalogIO.GetListOfItem(false, ref arIndex) && m_InstanceOfAnalogIO.GetListOfName(false, ref arItem))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, nPreSelected))
						{
							m_InstanceOfSelectionList.GetResult(ref nIndexOfSelected);
						}
					}
					break;
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT:
					if (m_InstanceOfAnalogIO.GetListOfItem(true, ref arIndex) && m_InstanceOfAnalogIO.GetListOfName(true, ref arItem))
					{
						if (m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, nPreSelected))
						{
							m_InstanceOfSelectionList.GetResult(ref nIndexOfSelected);
						}
					}
					break;
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.CYLINDER:
					if(m_InstanceOfCylinder.GetListOfItem(ref arIndex) && m_InstanceOfCylinder.GetListOfName(ref arItem))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, nPreSelected))
						{
							m_InstanceOfSelectionList.GetResult(ref nIndexOfSelected);
						}
					}
					break;
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_OUTPUT:
					if(m_InstanceOfDigitalIO.GetListOfItem(false, ref arIndex) && m_InstanceOfDigitalIO.GetListOfName(false, ref arItem))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, nPreSelected))
						{
							m_InstanceOfSelectionList.GetResult(ref nIndexOfSelected);
						}
					}
					break;
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_INPUT:
					if (m_InstanceOfDigitalIO.GetListOfItem(true, ref arIndex) && m_InstanceOfDigitalIO.GetListOfName(true, ref arItem))
					{
						if (m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, nPreSelected))
						{
							m_InstanceOfSelectionList.GetResult(ref nIndexOfSelected);
						}
					}
					break;
				case FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION:
					if(m_InstanceOfMotion.GetListOfName(ref arItem) && m_InstanceOfMotion.GetListOfItem(ref arIndex))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, nPreSelected))
						{
							m_InstanceOfSelectionList.GetResult(ref nIndexOfSelected);
						}
					}
					break;
				default:
					break;
			}
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 라벨들의 값을 초기화한다.
		/// </summary>
		private void ResetSelectedItem()
		{
			m_nSelectedRowItem			= -1;
			m_nSelectedItemIndex		= -1;

			m_labelName.Text			= string.Empty;
			m_labelTargetIndex.Text		= DEFAULT_LABEL;

			SetActiveControls(false);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 컨트롤의 활성화 유무 설정
		/// </summary>
		private void SetActiveControls(bool bEnable)
		{
			m_labelTargetIndex.Enabled	= bEnable;

			m_btnRemove.Enabled			= bEnable;
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
		/// <summary>
		/// 2020.11.05 by twkang [ADD] TaskList GridView 클릭 이벤트 
		/// </summary>
		private void Click_TaskList(object sender, DataGridViewCellEventArgs e)
		{
			int nRowIndex	= e.RowIndex;

			if(nRowIndex < 0 || nRowIndex >= m_dgViewTask.RowCount) return;

			m_nSelectedRowTask	= nRowIndex;
			m_strSelectedTask	= m_dgViewTask[COLUMN_INDEX_OF_NAME, nRowIndex].Value.ToString();

			UpdateItemList();
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] DeviceList GridView 클릭 이벤트
		/// </summary>
		private void Click_DeviceList(object sender, DataGridViewCellEventArgs e)
		{
			int nRowindex	= e.RowIndex;

			if(nRowindex < 0 || nRowindex >= m_dgViewDevice.RowCount) return;

			m_nSelectedRowDevice	= nRowindex;
			m_strSelectedDevice		= m_dgViewDevice[COLUMN_INDEX_OF_NAME, nRowindex].Value.ToString();
			
			UpdateItemList();
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] ItemList GridView 클릭 이벤트
		/// </summary>
		private void Click_ItemList(object sender, DataGridViewCellEventArgs e)
		{
			int nRowindex		= e.RowIndex;
			int nColumnIndex	= e.ColumnIndex;

			if (nRowindex < 0 || nRowindex >= m_dgViewItem.RowCount) { return; }

			m_nSelectedRowItem		= nRowindex;
			m_nSelectedItemIndex	= int.Parse(m_dgViewItem[COLUMN_INDEX_OF_INDEX, nRowindex].Value.ToString());

			m_labelName.Text			= m_dgViewItem[COLUMN_INDEX_OF_NAME, nRowindex].Value.ToString();
			m_labelTargetIndex.Text		= m_dgViewItem[COLUMN_INDEX_OF_TARGET, nRowindex].Value.ToString();

			SetActiveControls(true);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] ADD, REMOVE 버튼 클릭 이벤트
		/// </summary>
		private void Click_Button(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			switch (ctrl.TabIndex)
			{
				case 0: // ADD
					if(m_InstanceOfCalculator.CreateForm("0", MIN_VALUE, MAX_VALUE))
					{
						int nIndexOfItem			= -1;
						m_InstanceOfCalculator.GetResult(ref nIndexOfItem);
						m_InstanceOfDevice.AddDeviceItem(m_strSelectedTask, m_dicForDevice[m_strSelectedDevice], nIndexOfItem);
					}
					break;
				case 1: // REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						m_InstanceOfDevice.RemoveDeviceItem(m_strSelectedTask, m_dicForDevice[m_strSelectedDevice], m_nSelectedItemIndex);
					}
					break;
			}
			UpdateItemList();
			ResetSelectedItem();
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 파라미터 설정관련 컨트롤 클릭
		/// </summary>
		private void Click_TargetIndex(object sender, EventArgs e)
		{
			int nIndexOfItem		= -1;
			string strItemName		= string.Empty;
			int nPreSelected		= int.Parse(m_labelTargetIndex.Text);

			GetItemIndexFromSelectionList(m_dicForDevice[m_strSelectedDevice], ref nIndexOfItem, nPreSelected);
			if (nIndexOfItem != SELECT_NONE)
			{
				if (m_InstanceOfDevice.SetDeviceTargetIndex(m_strSelectedTask, m_dicForDevice[m_strSelectedDevice], m_nSelectedItemIndex, nIndexOfItem))
				{
					m_dgViewItem[COLUMN_INDEX_OF_TARGET, m_nSelectedRowItem].Value = nIndexOfItem;
					m_labelTargetIndex.Text = nIndexOfItem.ToString();
				}
				GetDeviceName(m_dicForDevice[m_strSelectedDevice], nIndexOfItem, ref strItemName);

				if (m_InstanceOfDevice.SetDeviceTargetName(m_strSelectedTask, m_dicForDevice[m_strSelectedDevice], m_nSelectedItemIndex, strItemName))
				{
					m_labelName.Text = strItemName;
					m_dgViewItem[COLUMN_INDEX_OF_NAME, m_nSelectedRowItem].Value = strItemName;
				}
			}
		}
		#endregion
	}
}
