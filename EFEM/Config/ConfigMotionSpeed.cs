using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Motion_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	extern alias MotionInstance;

	public class ConfigMotionSpeed
	{
		public ConfigMotionSpeed() { }

		#region 싱글톤
		private static ConfigMotionSpeed _Instance	= new ConfigMotionSpeed();
		public static ConfigMotionSpeed GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_SPEED
		{
			SPEED_PATTERN,
			VELOCITY,
			MAX_VELOCITY,
			ACCELERATION,
			DECELERATION,
			ACCEL_TIME,
			DECEL_TIME,
			JERK_ACCEL,
			JERK_DECEL,
			MOTION_TIMEOUT,
            SHORT_DISTANCE,
            SHORT_DISTANCE_AUTO,
		}
		#endregion

		#region 상수
		private const int DEFAULT_ACCEL				= 1;
		private const int DEFAULT_DECEL				= 1;
		private const int DEFAULT_ACCEL_JERK		= 0;
		private const int DEFAULT_DECEL_JERK		= 0;
		private const int DEFAULT_MOTION_TIMEOUT	= 60000;
		private const int DEFAULT_SPEED_PATTERN		= (int)MOTION_SPEED_PATTERN.S_CURVE;
		private const int DEFAULT_MAX_VELOCITY		= 20;
		private const int DEFAULT_VELOCITY			= 2;
		private const bool DEFAULT_SHORT_DISTANCE	= false;
		#endregion

		#region 변수
		Dictionary<ConfigMotion.EN_SPEED_CONTENT, MotionInstance::Motion_.MOTION_SPEED_CONTENT>	m_DicForSpeedContent
			= new Dictionary<ConfigMotion.EN_SPEED_CONTENT, MotionInstance::Motion_.MOTION_SPEED_CONTENT>();

		Dictionary<EN_PARAM_SPEED ,PARAM_LIST_SPEED> m_DicForParam				= new Dictionary<EN_PARAM_SPEED, PARAM_LIST_SPEED>();
		Dictionary<string, int> m_DicForResult									= new Dictionary<string, int>();
		Dictionary<int, string> m_DicForSpeedPattern							= new Dictionary<int,string>();

		Motion			m_InstanceOfMotionDll									= null;
		Functional.Storage	m_InstanceOfStorage									= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.25 by twkang [ADD] MotionSpeed 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTabel();

			m_InstanceOfMotionDll			= Motion.GetInstance();
			m_InstanceOfStorage				= Functional.Storage.GetInstance();
			
			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED))
			{
				return false;
			}
			
			InitializeParameter();

			return true;
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] Item 의 파라미터 값을 반환한다.
		/// </summary>
		public bool GetSpeedParameter<T>(int nIndexOfItem, ConfigMotion.EN_SPEED_CONTENT enContent, EN_PARAM_SPEED enParam, ref T tValue)
		{
			if(m_DicForSpeedContent.ContainsKey(enContent) && m_DicForParam.ContainsKey(enParam))
			{
				return m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[enParam], ref tValue);
			}			
			return false;
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] Item 의 파라미터 값을 설정한다.
		/// </summary>
		public bool SetSpeedParameter<T>(int nIndexOfItem, ConfigMotion.EN_SPEED_CONTENT enContent, EN_PARAM_SPEED enParam, T tValue)
		{
			string[] arGroup	= new string[] {nIndexOfItem.ToString(), enContent.ToString()};

			if(false == m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, enParam.ToString(), tValue, ref arGroup))
			{
				return false;
			}

			if (m_DicForSpeedContent.ContainsKey(enContent) && m_DicForParam.ContainsKey(enParam))
			{
				switch(enParam)
				{
					case EN_PARAM_SPEED.SPEED_PATTERN:
						if(m_DicForResult.ContainsKey(tValue.ToString()))
						{
							return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[enParam], m_DicForResult[tValue.ToString()]);
						}
						break;
                    case EN_PARAM_SPEED.SHORT_DISTANCE:
                    case EN_PARAM_SPEED.SHORT_DISTANCE_AUTO:
                        return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[enParam], tValue.ToString().Equals(Define.DefineConstant.SelectionList.ENABLE));

					case EN_PARAM_SPEED.ACCEL_TIME:
					case EN_PARAM_SPEED.DECEL_TIME:
					case EN_PARAM_SPEED.ACCELERATION:
					case EN_PARAM_SPEED.DECELERATION:
					case EN_PARAM_SPEED.MAX_VELOCITY:
						if(m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[enParam], tValue))
						{
							return SaveSpeedParameter(nIndexOfItem, enContent, enParam);
						}
						break;

                    default:
                        return m_InstanceOfMotionDll.SetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[enParam], tValue);
				}				
			}
			return false;
		}
		/// <summary>
		/// 2020.07.07 by twkang [ADD] MotionSpeed 아이템을 추가한다.
		/// </summary>
		public bool AddMotionSpeedItem(int nItemNum)
		{
			string[] arGroup	= null;
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.MOTION_SPEED, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, ref arData))
			{
				SetDefaultValue(nItemNum);
				return SaveParamToStorage(ref nItemNum);
			}
			return false;
		}
		/// <summary>
		/// 2020.07.07 by twkang [ADD] Motion Speed 아이템을 Storage 를 통해 삭제한다.
		/// </summary>
		public bool RemoveGroup(string strGroup)
		{
			string[] arGroup	= new string[] { strGroup };
			return m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, ref arGroup);
		}
		/// <summary>
		/// 2021.07.26 by twkang [ADD] Speed 파라미터 값을 복사한다.
		/// </summary>
		public void CopySpeedParamter(int nIndexOfItem, ConfigMotion.EN_SPEED_CONTENT enSource, ConfigMotion.EN_SPEED_CONTENT[] arTarget)
		{
			if(null == arTarget)
				return;

			foreach(EN_PARAM_SPEED enParam in Enum.GetValues(typeof(EN_PARAM_SPEED)))
			{
				string strValue	= string.Empty;

				switch(enParam)
				{
					case EN_PARAM_SPEED.SPEED_PATTERN:
						int nValue	= -1;
						GetSpeedParameter(nIndexOfItem, enSource, enParam, ref nValue);
						if(false == m_DicForSpeedPattern.ContainsKey(nValue))
							continue;
						strValue = m_DicForSpeedPattern[nValue];
						break;
					case EN_PARAM_SPEED.SHORT_DISTANCE:
					case EN_PARAM_SPEED.SHORT_DISTANCE_AUTO:
						bool bResult	= false;
						GetSpeedParameter(nIndexOfItem, enSource, enParam, ref bResult);
						strValue	= bResult ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;
						break;
					default:
						GetSpeedParameter(nIndexOfItem, enSource, enParam, ref strValue);
						break;
				}

				for (int nIndex = 0, nEnd = arTarget.Length; nIndex < nEnd; ++nIndex)
				{
					SetSpeedParameter(nIndexOfItem, arTarget[nIndex], enParam, strValue);
				}
			}
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.25 by twkang [ADD] MotionSpeed 클래스에서 사용할 Mapping Table 을 만든다
		/// </summary>
		private void MakeMappingTabel()
		{
			#region SPEED_PARAM
			m_DicForParam.Clear();
			m_DicForParam.Add(EN_PARAM_SPEED.JERK_ACCEL, PARAM_LIST_SPEED.MOTION_JERK_ACCEL);
			m_DicForParam.Add(EN_PARAM_SPEED.JERK_DECEL, PARAM_LIST_SPEED.MOTION_JERK_DECEL);
			m_DicForParam.Add(EN_PARAM_SPEED.SPEED_PATTERN, PARAM_LIST_SPEED.MOTION_SPEED_PATTERN);
			m_DicForParam.Add(EN_PARAM_SPEED.ACCELERATION, PARAM_LIST_SPEED.MOTION_ACCEL);
			m_DicForParam.Add(EN_PARAM_SPEED.DECELERATION, PARAM_LIST_SPEED.MOTION_DECEL);
			m_DicForParam.Add(EN_PARAM_SPEED.ACCEL_TIME, PARAM_LIST_SPEED.MOTION_TIME_ACCEL);
			m_DicForParam.Add(EN_PARAM_SPEED.DECEL_TIME, PARAM_LIST_SPEED.MOTION_TIME_DECEL);
			m_DicForParam.Add(EN_PARAM_SPEED.MOTION_TIMEOUT, PARAM_LIST_SPEED.MOTION_TIMEOUT);
			m_DicForParam.Add(EN_PARAM_SPEED.VELOCITY, PARAM_LIST_SPEED.MOTION_VELOCITY);
			m_DicForParam.Add(EN_PARAM_SPEED.MAX_VELOCITY, PARAM_LIST_SPEED.MOTION_VELOCITY_MAX);
            m_DicForParam.Add(EN_PARAM_SPEED.SHORT_DISTANCE, PARAM_LIST_SPEED.MOTION_USE_SHORT_DISTANCE);
            m_DicForParam.Add(EN_PARAM_SPEED.SHORT_DISTANCE_AUTO, PARAM_LIST_SPEED.MOTION_USE_AUTO_SHORT_DISTANCE);
			#endregion
			
			#region MOTION_SPEED_PATTERN
			m_DicForResult.Clear();
			m_DicForSpeedPattern.Clear();
			foreach (MOTION_SPEED_PATTERN en in Enum.GetValues(typeof(MOTION_SPEED_PATTERN)))
			{
				m_DicForResult.Add(en.ToString(), (int)en);
				m_DicForSpeedPattern.Add((int)en, en.ToString());
			}
			#endregion

			#region MOTION_SPEED_CONTENT
			m_DicForSpeedContent.Clear();			
			m_DicForSpeedContent.Add(ConfigMotion.EN_SPEED_CONTENT.CUSTOM_1, MotionInstance::Motion_.MOTION_SPEED_CONTENT.CUSTOM_1);
			m_DicForSpeedContent.Add(ConfigMotion.EN_SPEED_CONTENT.SHORT_DISTANCE, MotionInstance::Motion_.MOTION_SPEED_CONTENT.SHORT_DISTANCE);
			m_DicForSpeedContent.Add(ConfigMotion.EN_SPEED_CONTENT.JOG_HIGH, MotionInstance::Motion_.MOTION_SPEED_CONTENT.JOG_HIGH);
			m_DicForSpeedContent.Add(ConfigMotion.EN_SPEED_CONTENT.JOG_LOW, MotionInstance::Motion_.MOTION_SPEED_CONTENT.JOG_LOW);
			m_DicForSpeedContent.Add(ConfigMotion.EN_SPEED_CONTENT.MANUAL, MotionInstance::Motion_.MOTION_SPEED_CONTENT.MANUAL);
			m_DicForSpeedContent.Add(ConfigMotion.EN_SPEED_CONTENT.RUN, MotionInstance::Motion_.MOTION_SPEED_CONTENT.RUN);
			#endregion
		}
		/// <summary>
		/// 2020.07.07 by twkang [ADD] 파라미터 값을 Storage 를 통해 저장한다.
		/// </summary>
		private bool SaveParamToStorage(ref int nIndexOfItem)
		{
			string strValue		= string.Empty;
			string[] arGroup	= new string[] {nIndexOfItem.ToString()};

			m_InstanceOfMotionDll.GetParameter(nIndexOfItem, PARAM_LIST.NAME, ref strValue);
			m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, PARAM_LIST.NAME.ToString(), strValue, ref arGroup);
			
			foreach(ConfigMotion.EN_SPEED_CONTENT enContent in Enum.GetValues(typeof(ConfigMotion.EN_SPEED_CONTENT)))
			{
				arGroup	= new string[] {nIndexOfItem.ToString(), enContent.ToString()};
				foreach(EN_PARAM_SPEED en in Enum.GetValues(typeof(EN_PARAM_SPEED)))
				{
					strValue		= string.Empty;
					switch(en)
					{
						case EN_PARAM_SPEED.SHORT_DISTANCE:
						case EN_PARAM_SPEED.SHORT_DISTANCE_AUTO:
							bool bResult			= false;
							m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[en], ref bResult);
							strValue				= bResult ? Define.DefineConstant.SelectionList.ENABLE : Define.DefineConstant.SelectionList.DISABLE;

							break;
						case EN_PARAM_SPEED.SPEED_PATTERN:
							int nTempValue			= -1;
							m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[en], ref nTempValue);
							if(m_DicForSpeedPattern.ContainsKey(nTempValue))
							{
								strValue		= m_DicForSpeedPattern[nTempValue];
							}
							break;
						default:
							m_InstanceOfMotionDll.GetParameter(nIndexOfItem, m_DicForSpeedContent[enContent], m_DicForParam[en], ref strValue);
							break;
					}
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, en.ToString(), strValue, ref arGroup, false);
				}
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED);
			return true;
		}
		/// <summary>
		/// 2020.06.26 by twkang [ADD] Storage 데이터로 MotionSpeed 파라미터를 초기화한다.
		/// </summary>
		private void InitializeParameter()
		{
			string strValue					= string.Empty;
			string[] strArrSpeedGroup		= null;
			int[] nArrMotionGroup			= null;
			List<string> listOfSpeedGroup	= new List<string>();

			m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, ref strArrSpeedGroup);
			m_InstanceOfMotionDll.GetListOfItem(ref nArrMotionGroup);

			// CONFIG 파일이 없을경우
			if(null == strArrSpeedGroup && null == nArrMotionGroup)
			{
				return;
			}
			// MOTION CONFIG 파일만 있을경우,
			else if(null == strArrSpeedGroup)
			{
				foreach(int nItem in nArrMotionGroup)
				{
					AddMotionSpeedItem(nItem);
				}
				return;
			}
			// SPEED CONFIG 파일만 있을경우
			else if(null == nArrMotionGroup)
			{
				foreach (string str in strArrSpeedGroup)
				{
					RemoveGroup(str);
				}
				return ;
			}

			foreach(string str in strArrSpeedGroup)
			{
				if(false == listOfSpeedGroup.Contains(str))
				{
					listOfSpeedGroup.Add(str);
				}
			}

			for(int nIndex = 0, nEnd = nArrMotionGroup.Length; nIndex < nEnd; nIndex++)
			{
				SetDefaultValue(nArrMotionGroup[nIndex]);

				string strGroup		= nArrMotionGroup[nIndex].ToString();
				string[] arGroup	= new string[2] {strGroup, null};
				
				if(listOfSpeedGroup.Remove(strGroup))
				{
					foreach (ConfigMotion.EN_SPEED_CONTENT en in Enum.GetValues(typeof(ConfigMotion.EN_SPEED_CONTENT)))
					{
						arGroup[1] = en.ToString();
						foreach (EN_PARAM_SPEED enParam in Enum.GetValues(typeof(EN_PARAM_SPEED)))
						{
							if (m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED
								, enParam.ToString()
								, ref strValue
								, ref arGroup))
							{
								switch (enParam)
								{
									case EN_PARAM_SPEED.SPEED_PATTERN:
										if (m_DicForResult.ContainsKey(strValue))
										{
											m_InstanceOfMotionDll.SetParameter(nArrMotionGroup[nIndex], m_DicForSpeedContent[en], m_DicForParam[enParam], m_DicForResult[strValue]);
										}
										break;
                                        
                                    case EN_PARAM_SPEED.SHORT_DISTANCE:
                                    case EN_PARAM_SPEED.SHORT_DISTANCE_AUTO:
                                        m_InstanceOfMotionDll.SetParameter(nArrMotionGroup[nIndex], m_DicForSpeedContent[en], m_DicForParam[enParam], strValue.Equals(Define.DefineConstant.SelectionList.ENABLE));
                                        break;

									default:
										m_InstanceOfMotionDll.SetParameter(nArrMotionGroup[nIndex], m_DicForSpeedContent[en], m_DicForParam[enParam], strValue);
										break;
								}
							}
						}
					}
				}
				else
				{
					AddMotionSpeedItem(nArrMotionGroup[nIndex]);
				}
			}

			foreach(string str in listOfSpeedGroup)
			{
				RemoveGroup(str);
			}
		}
		/// <summary>
		/// 2020.10.07 by twkang [ADD] 아이템을 Default 값으로 설정한다.
		/// </summary>
		private void SetDefaultValue(int nIndex)
		{
			foreach(MotionInstance::Motion_.MOTION_SPEED_CONTENT en in Enum.GetValues(typeof(MotionInstance::Motion_.MOTION_SPEED_CONTENT)))
			{
				switch(en)
				{
					case MotionInstance::Motion_.MOTION_SPEED_CONTENT.MAX_COUNT:
						break;
					default:
						// 가속, 감속 시간은 DLL 내부에서 자동 계산
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_ACCEL, DEFAULT_ACCEL);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_DECEL, DEFAULT_DECEL);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_JERK_ACCEL, DEFAULT_ACCEL_JERK);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_JERK_DECEL, DEFAULT_DECEL_JERK);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_SPEED_PATTERN, DEFAULT_SPEED_PATTERN);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_TIMEOUT, DEFAULT_MOTION_TIMEOUT);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_VELOCITY, DEFAULT_VELOCITY);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_VELOCITY_MAX, DEFAULT_MAX_VELOCITY);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_USE_AUTO_SHORT_DISTANCE, DEFAULT_SHORT_DISTANCE);
						m_InstanceOfMotionDll.SetParameter(nIndex, en, PARAM_LIST_SPEED.MOTION_USE_SHORT_DISTANCE, DEFAULT_SHORT_DISTANCE);
						break;
				}
			}
		}

		/// <summary>
		/// 2021.01.20 by twkang [ADD] Speed Parameter 설정 이후 바뀐 다른 데이터값을 Storage 로 저장한다.
		/// </summary>
		private bool SaveSpeedParameter(int nIndexOfItem, ConfigMotion.EN_SPEED_CONTENT enContent, EN_PARAM_SPEED enPreSetedParam)
		{
			if(false == m_DicForSpeedContent.ContainsKey(enContent) || false == m_DicForParam.ContainsKey(enPreSetedParam))
			{
				return false;
			}

			var enSpeedContent	= m_DicForSpeedContent[enContent];
			
			string[] arGroup	= new string[] {nIndexOfItem.ToString(), enContent.ToString()};
			double dblValue		= 0.0;

			switch(enPreSetedParam)
			{
				default:
					break;
				case EN_PARAM_SPEED.MAX_VELOCITY:
					m_InstanceOfMotionDll.GetParameter(nIndexOfItem, enSpeedContent, PARAM_LIST_SPEED.MOTION_TIME_ACCEL, ref dblValue);
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, EN_PARAM_SPEED.ACCEL_TIME.ToString(), dblValue, ref arGroup, false);

					m_InstanceOfMotionDll.GetParameter(nIndexOfItem, enSpeedContent, PARAM_LIST_SPEED.MOTION_TIME_DECEL, ref dblValue);
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, EN_PARAM_SPEED.DECEL_TIME.ToString(), dblValue, ref arGroup, false);
					break;
				case EN_PARAM_SPEED.ACCELERATION:
					m_InstanceOfMotionDll.GetParameter(nIndexOfItem, enSpeedContent, PARAM_LIST_SPEED.MOTION_TIME_ACCEL, ref dblValue);
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, EN_PARAM_SPEED.ACCEL_TIME.ToString(), dblValue, ref arGroup, false);

					break;
				case EN_PARAM_SPEED.DECELERATION:
					m_InstanceOfMotionDll.GetParameter(nIndexOfItem, enSpeedContent, PARAM_LIST_SPEED.MOTION_TIME_DECEL, ref dblValue);
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, EN_PARAM_SPEED.DECEL_TIME.ToString(), dblValue, ref arGroup, false);
					break;
				case EN_PARAM_SPEED.ACCEL_TIME:
					m_InstanceOfMotionDll.GetParameter(nIndexOfItem, enSpeedContent, PARAM_LIST_SPEED.MOTION_ACCEL, ref dblValue);
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, EN_PARAM_SPEED.ACCELERATION.ToString(), dblValue, ref arGroup, false);
					break;
				case EN_PARAM_SPEED.DECEL_TIME:
					m_InstanceOfMotionDll.GetParameter(nIndexOfItem, enSpeedContent, PARAM_LIST_SPEED.MOTION_DECEL, ref dblValue);
					m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, EN_PARAM_SPEED.DECELERATION.ToString(), dblValue, ref arGroup, false);
					break;
			}

			return m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED);
		}

		#endregion
	}
}