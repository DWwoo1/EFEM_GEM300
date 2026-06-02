using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskDevice_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigDevice
	{
		private ConfigDevice() { }

		#region 싱글톤
		public static ConfigDevice _Instance	= new ConfigDevice();
		public static ConfigDevice GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_TYPE_DEVICE
		{
			MOTION			= 0,
			DIGITAL_INPUT	= 1,
			DIGITAL_OUTPUT	= 2,
			CYLINDER		= 3,
			ANALOG_INPUT	= 4,
			ANALOG_OUTPUT	= 5,
		}
		#endregion

		#region 상수
		private const int DEFAULT_TARGET_INDEX			= -1;
		private readonly string DEFAULT_TARGET_NAME		= "NONE";

		private readonly string ITEM_NAME				= "NAME";
		private readonly string ITEM_TARGET				= "TARGET_INDEX";
		#endregion

		#region 변수
		private Dictionary<EN_TYPE_DEVICE, TYPE_DEVICE> m_dicForDeviceType	= new Dictionary<EN_TYPE_DEVICE,TYPE_DEVICE>();

		TaskDevice m_InstanceOfTaskDevice					= null;
		Functional.Storage m_InstanceOfStorage				= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Device 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();

			m_InstanceOfStorage			= Functional.Storage.GetInstance();
			m_InstanceOfTaskDevice		= TaskDevice.GetInstance();

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE))
			{
				return false;
			}

			InitializeParameter();
			
			return true;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Device 아이템을 추가한다.
		/// </summary>
		public bool AddDeviceItem(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem)
		{
			if(false == m_InstanceOfTaskDevice.AddDeviceItem(strTask, m_dicForDeviceType[enDevice], nIndexOfItem))
			{
				return false;
			}

			SetDefaultValue(strTask, enDevice, nIndexOfItem);

			string[] arGroup		= new string[] {strTask, enDevice.ToString()};
			string[] arData			= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.TASK_DEVICE, ref arGroup, nIndexOfItem.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ref arData))
			{
				SaveParamToStorage(strTask, enDevice, nIndexOfItem);
				return true;
			}
			m_InstanceOfTaskDevice.RemoveDeviceItem(strTask, m_dicForDeviceType[enDevice], nIndexOfItem);
			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Device 아이템을 삭제한다.
		/// </summary>
		public bool RemoveDeviceItem(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem)
		{
			string[] arGroup	= new string[] {strTask, enDevice.ToString(), nIndexOfItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ref arGroup))
			{
				return m_InstanceOfTaskDevice.RemoveDeviceItem(strTask, m_dicForDeviceType[enDevice], nIndexOfItem);
			}
			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 아이템의 Target Index 파라미터를 변경한다.
		/// </summary>
		public bool SetDeviceTargetIndex(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem, int nTargetIndex)
		{
			string[] arGroup	= new string[] {strTask, enDevice.ToString(), nIndexOfItem.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE
				, ITEM_TARGET
				, nTargetIndex
				, ref arGroup))
			{
				return false;
			}

			if(m_dicForDeviceType.ContainsKey(enDevice))
			{
				return m_InstanceOfTaskDevice.SetDeviceTargetIndex(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, nTargetIndex);
			}
			
			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 아이템의 Name 을 바꾼다.
		/// </summary>
		public bool SetDeviceTargetName(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem, string strTargetName)
		{
			string[] arGroup	= new string[] {strTask, enDevice.ToString(), nIndexOfItem.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE
				, ITEM_NAME
				, strTargetName
				, ref arGroup))
			{
				return false;
			}

			if(m_dicForDeviceType.ContainsKey(enDevice))
			{
				return m_InstanceOfTaskDevice.SetDeviceTargetName(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, strTargetName);
			}
			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Task List 를 가져온다.
		/// </summary>
		public void GetListOfTask(ref int nCountOfTask, ref string[] arTask)
		{
			m_InstanceOfTaskDevice.GetListOfTask(ref nCountOfTask, ref arTask);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 해당 디바이스의 아이템 리스트를 가져온다.
		/// </summary>
		public void GetIndexesOfDevice(string strTask, EN_TYPE_DEVICE enDevice, ref int nCountOfDevice, ref int[] arDevice)
		{ 
			if(m_dicForDeviceType.ContainsKey(enDevice))
			{
				m_InstanceOfTaskDevice.GetIndexesOfDevice(strTask, m_dicForDeviceType[enDevice], ref nCountOfDevice, ref arDevice);
			}
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 해당 아이템의 Target Index 파라미터 값을 가져온다.
		/// </summary>
		public bool GetDeviceTargetIndex(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem, ref int nTargetIndex)
		{
			if(m_dicForDeviceType.ContainsKey(enDevice))
			{
				return m_InstanceOfTaskDevice.GetDeviceTargetIndex(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, ref nTargetIndex);
			}
			return false;
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 해당 아이템의 Name 파라미터 값을 가져온다.
		/// </summary>
		public bool GetDeviceTargetName(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem, ref string strTargetName)
		{
			if(m_dicForDeviceType.ContainsKey(enDevice))
			{
				return m_InstanceOfTaskDevice.GetDeviceTargetName(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, ref strTargetName);
			}
			return false;
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.11.16 by twkang [ADD] Storage 데이터로 Device 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string[] arTask		= null;

			if(false == m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ref arTask))
			{
                // 2024.01.17. by shkim. [ADD] 최초 Config 파일이 작성되어있지 않으면 UI에서의 등록이 안됬다.
                // (TaskDevice에서 파일로부터 읽은 task 정보로 task를 등록하기 때문에...)
                arTask = null;
				if (true == ConfigTask.GetInstance().GetListOfTask(ref arTask))
				{
                    for (int nIndex = 0, nEnd = arTask.Length; nIndex < nEnd; ++nIndex)
					{
                        string strTask = arTask[nIndex];

                        m_InstanceOfTaskDevice.AddTask(strTask);
                    }
                }
                return;
            }
			for(int nIndex = 0, nEnd = arTask.Length; nIndex < nEnd; ++nIndex)
			{
				string strTask	= arTask[nIndex];

				m_InstanceOfTaskDevice.AddTask(strTask);

				foreach(EN_TYPE_DEVICE enDevice in Enum.GetValues(typeof(EN_TYPE_DEVICE)))
				{
					string[] arGroupLevel	= new string[] {strTask, enDevice.ToString()};
					string[] arDeviceItem	= null;

					if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ref arDeviceItem, arGroupLevel))
					{
						for(int i = 0, nLast = arDeviceItem.Length; i < nLast; ++i)
						{
							string[] arGroup		= new string[] {strTask, enDevice.ToString(), arDeviceItem[i]};
							int nIndexOfItem		= int.Parse(arDeviceItem[i]);

							m_InstanceOfTaskDevice.AddDeviceItem(strTask, m_dicForDeviceType[enDevice], nIndexOfItem);
							SetDefaultValue(strTask, enDevice, nIndexOfItem);

							string strValue			= string.Empty;
							int nValue				= -1;

							if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ITEM_NAME, ref strValue, ref arGroup))
							{
								m_InstanceOfTaskDevice.SetDeviceTargetName(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, strValue);
							}
							if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ITEM_TARGET, ref nValue, ref arGroup))
							{
								m_InstanceOfTaskDevice.SetDeviceTargetIndex(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, nValue);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private void SaveParamToStorage(string strTask, EN_TYPE_DEVICE enList, int nIndexOfItem)
		{
			int nTargetIndex		= -1;
			string strTagetName		= string.Empty;

			m_InstanceOfTaskDevice.GetDeviceTargetIndex(strTask, m_dicForDeviceType[enList], nIndexOfItem, ref nTargetIndex);
			m_InstanceOfTaskDevice.GetDeviceTargetName(strTask, m_dicForDeviceType[enList], nIndexOfItem, ref strTagetName);

			string[] arGroup		= new string[] { strTask, enList.ToString(), nIndexOfItem.ToString()};

			m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ITEM_TARGET, nTargetIndex, ref arGroup, false);
			m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, ITEM_NAME, strTagetName, ref arGroup, false);
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 클래스에서 사용한 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			m_dicForDeviceType.Clear();
			m_dicForDeviceType.Add(EN_TYPE_DEVICE.ANALOG_INPUT, TYPE_DEVICE.ANALOG_INPUT);
			m_dicForDeviceType.Add(EN_TYPE_DEVICE.ANALOG_OUTPUT, TYPE_DEVICE.ANALOG_OUTPUT);
			m_dicForDeviceType.Add(EN_TYPE_DEVICE.CYLINDER, TYPE_DEVICE.CYLINDER);
			m_dicForDeviceType.Add(EN_TYPE_DEVICE.DIGITAL_INPUT, TYPE_DEVICE.DIGITAL_INPUT);
			m_dicForDeviceType.Add(EN_TYPE_DEVICE.DIGITAL_OUTPUT, TYPE_DEVICE.DIGITAL_OUTPUT);
			m_dicForDeviceType.Add(EN_TYPE_DEVICE.MOTION, TYPE_DEVICE.MOTION);
		}
		/// <summary>
		/// 2020.11.16 by twkang [ADD] 해당 아이템의 Default Value 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(string strTask, EN_TYPE_DEVICE enDevice, int nIndexOfItem)
		{
			m_InstanceOfTaskDevice.SetDeviceTargetIndex(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, DEFAULT_TARGET_INDEX);
			m_InstanceOfTaskDevice.SetDeviceTargetName(strTask, m_dicForDeviceType[enDevice], nIndexOfItem, DEFAULT_TARGET_NAME);
		}
		#endregion
	}
}
