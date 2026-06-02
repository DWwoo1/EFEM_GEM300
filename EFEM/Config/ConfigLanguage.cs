using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Language_;
using FileBorn_;

namespace FrameOfSystem3.Config
{
	public class ConfigLanguage
	{
		private ConfigLanguage() { }

		#region 싱글톤
		private static ConfigLanguage _Instance	= new ConfigLanguage();
		public static ConfigLanguage GetInstance()
		{
			return _Instance;
		}
		#endregion

		#region 열거형
		public enum EN_PARAM_LANGUAGE
		{
			ENGLISH,
			KOREAN,
            CHINESE,
            JAPANESE,
		}
		public enum EN_TYPE_FORMAT
		{
			WORD,
			SENTENCE
		}
		#endregion

		#region 상수
		private readonly string ITEM_ENGLISH				= "ENGLISH";
		private readonly string ITEM_KOREAN					= "KOREAN";
		private readonly string ITEM_CHINESE				= "CHINESE";
		private readonly string ITEM_JAPANESE				= "JAPANESE";
		#endregion

		#region 변수

		List<string> m_ListForInitialize							= new List<string>();
		Dictionary<EN_TYPE_FORMAT, TYPE_FORMAT> m_DicForFormat		= new Dictionary<EN_TYPE_FORMAT,TYPE_FORMAT>();
		Dictionary<EN_PARAM_LANGUAGE, PARAM_LIST> m_DicForParam		= new Dictionary<EN_PARAM_LANGUAGE,PARAM_LIST>();
		Dictionary<EN_PARAM_LANGUAGE, string> m_DicForStorage		= new Dictionary<EN_PARAM_LANGUAGE,string>();
		Dictionary<EN_PARAM_LANGUAGE, TYPE_LANGUAGE> m_DicForType	= new Dictionary<EN_PARAM_LANGUAGE,TYPE_LANGUAGE>();

		Functional.Storage m_InstanceOfStorage		= null;
		Language m_InstanceOfLanguageDll			= null;
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.19 by twkang [ADD] Language 클래스를 초기화한다.
		/// </summary>
		public bool Init()
		{
			MakeMappingTable();
			

			m_InstanceOfLanguageDll				= Language.GetInstance();
			m_InstanceOfStorage					= Functional.Storage.GetInstance();

			if(false == m_InstanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE))
			{
				return false;
			}

			InitializeLanguageParam();

			return true;
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 아이템을 추가한다.
		/// </summary>
		public int AddLanguage(EN_TYPE_FORMAT enFormat)
		{
			int nItemNum = m_InstanceOfLanguageDll.AddItem(m_DicForFormat[enFormat]);

			string[] arGroup	= new string[] {enFormat.ToString()};
			string[] arData		= null;

			if(FileBorn.GetInstance().GetItemFrame(BORN_LIST.LANGUAGE, ref arGroup, nItemNum.ToString(), ref arData)
				&& m_InstanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, ref arData))
			{
				SaveParameterToStorage(enFormat, nItemNum);
				return  nItemNum;
			}

			m_InstanceOfLanguageDll.RemoveItem(m_DicForFormat[enFormat], nItemNum);
			return -1;
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 아이템을 삭제한다.
		/// </summary>
		public bool RemoveItem(EN_TYPE_FORMAT enFormat, int nIndexItem)
		{
			string[] strArrGroup	= new string[] {enFormat.ToString(), nIndexItem.ToString()};

			if(m_InstanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, ref strArrGroup))
			{
				return m_InstanceOfLanguageDll.RemoveItem(m_DicForFormat[enFormat], nIndexItem);
			}
			
			return false;
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 아이템의 리스트를 반환한다.
		/// </summary>
		public bool GetListOfItem(EN_TYPE_FORMAT enFormat, ref int[] arIndexes)
		{			
			return m_InstanceOfLanguageDll.GetListOfItem(m_DicForFormat[enFormat], ref arIndexes);
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] Language 파라미터 값을 변경한다.
		/// </summary>
		public bool SetParameter(EN_TYPE_FORMAT enFormat , int nIndexOfItem, EN_PARAM_LANGUAGE enList, string strValue)
		{
			string[] strArrGroup	= new string[2] {enFormat.ToString(), nIndexOfItem.ToString()};

			if(m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, m_DicForStorage[enList], strValue, ref strArrGroup))
			{
				return m_InstanceOfLanguageDll.SetParameter(m_DicForFormat[enFormat], nIndexOfItem, m_DicForParam[enList], strValue);
			}

			return false;
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] Language 파라미터 값을 반환한다.
		/// </summary>
		public bool GetParameter(EN_TYPE_FORMAT enFormat, int nIndexOfItem, EN_PARAM_LANGUAGE enList, ref string strValue)
		{
			return m_InstanceOfLanguageDll.GetParameter(m_DicForFormat[enFormat], nIndexOfItem, m_DicForParam[enList], ref strValue);
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 언어를 선택한다.
		/// </summary>
		public bool SetLanguage(EN_PARAM_LANGUAGE enType)
		{
			return m_InstanceOfLanguageDll.SetLanguage(m_DicForType[enType]);
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 현재 설정 언어를 반환한다.
		/// </summary>
		public EN_PARAM_LANGUAGE GetLangauge()
		{
			TYPE_LANGUAGE enType	= m_InstanceOfLanguageDll.GetLangauge();
			EN_PARAM_LANGUAGE enList	= EN_PARAM_LANGUAGE.KOREAN;
			switch(enType)
			{
				case TYPE_LANGUAGE.ENGLISH:
					enList = EN_PARAM_LANGUAGE.ENGLISH;
					break;
				case TYPE_LANGUAGE.KOREAN:
					enList = EN_PARAM_LANGUAGE.KOREAN;
					break;
                case TYPE_LANGUAGE.CHINESE:
                    enList = EN_PARAM_LANGUAGE.CHINESE;
                    break;
                case TYPE_LANGUAGE.JAPANESE:
                    enList = EN_PARAM_LANGUAGE.JAPANESE;
                    break;
			}
			return enList;
		}
		/// <summary>
		/// 2020.06.19 by twkang [ADD] 
		/// </summary>
		public string TranslateWord(string strWord)
		{
			return m_InstanceOfLanguageDll.TranslateWord(strWord);
		}
		/// <summary>
		/// 2020.06.19. by twkang [ADD]  
		/// </summary>
		public string GetSentence(int nIndexOfItem, string strData)
		{
			return m_InstanceOfLanguageDll.GetSentence(nIndexOfItem, strData);
		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.19 by twkang [ADD] Storage 데이터로 Language 파라미터를 초기화한다.
		/// </summary>
		private void InitializeLanguageParam()
		{
			string strValue		= string.Empty;
			string strParamName	= string.Empty;

			foreach(TYPE_FORMAT en in Enum.GetValues(typeof(TYPE_FORMAT)))
			{
				string[] arGroups	= new string[1];
				string[] strGroupList	= null;
				arGroups[0]			= en.ToString();
				
				if(m_InstanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, ref strGroupList, arGroups))
                {
					Array.Resize(ref arGroups, 2);

                    for(int nIndex = 0, nEnd = strGroupList.Length; nIndex < nEnd; nIndex++)
    				{
    					arGroups[1]			= strGroupList[nIndex];
						int nIndexOfItem	= int.Parse(strGroupList[nIndex]);
						m_InstanceOfLanguageDll.AddItem(en, nIndexOfItem);
    
    					foreach(EN_PARAM_LANGUAGE enParam in Enum.GetValues(typeof(EN_PARAM_LANGUAGE)))
    					{
    						if(m_InstanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE
    						, m_DicForStorage[enParam]
    						, ref strValue
    						, ref arGroups))
    						{
    							m_InstanceOfLanguageDll.SetParameter(en, nIndexOfItem, m_DicForParam[enParam], strValue);
    						}
    					}
    				}
                }
			}
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 클래스에서 사용할 Mapping Table 을 만든다.
		/// </summary>
		private void MakeMappingTable()
		{
			#region Parameter
			m_DicForParam.Clear();
			m_DicForParam.Add(EN_PARAM_LANGUAGE.ENGLISH, PARAM_LIST.ENGLISH);
            m_DicForParam.Add(EN_PARAM_LANGUAGE.KOREAN, PARAM_LIST.KOREAN);
            m_DicForParam.Add(EN_PARAM_LANGUAGE.CHINESE, PARAM_LIST.CHINESE);
            m_DicForParam.Add(EN_PARAM_LANGUAGE.JAPANESE, PARAM_LIST.JAPANESE);
			#endregion

			#region Storage
			m_DicForStorage.Clear();
			m_DicForStorage.Add(EN_PARAM_LANGUAGE.KOREAN, ITEM_KOREAN);
            m_DicForStorage.Add(EN_PARAM_LANGUAGE.ENGLISH, ITEM_ENGLISH);
            m_DicForStorage.Add(EN_PARAM_LANGUAGE.CHINESE, ITEM_CHINESE);
            m_DicForStorage.Add(EN_PARAM_LANGUAGE.JAPANESE, ITEM_JAPANESE);
			#endregion

			#region Format
			m_DicForFormat.Clear();
			m_DicForFormat.Add(EN_TYPE_FORMAT.SENTENCE, TYPE_FORMAT.SENTENCE);
			m_DicForFormat.Add(EN_TYPE_FORMAT.WORD, TYPE_FORMAT.WORD);
			#endregion

			#region Type
			m_DicForType.Clear();
			m_DicForType.Add(EN_PARAM_LANGUAGE.ENGLISH, TYPE_LANGUAGE.ENGLISH);
            m_DicForType.Add(EN_PARAM_LANGUAGE.KOREAN, TYPE_LANGUAGE.KOREAN);
            m_DicForType.Add(EN_PARAM_LANGUAGE.CHINESE, TYPE_LANGUAGE.CHINESE);
            m_DicForType.Add(EN_PARAM_LANGUAGE.JAPANESE, TYPE_LANGUAGE.JAPANESE);
			#endregion
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 파라미터의 값을 Storage 데이터로 저장한다.
		/// </summary>
		private bool SaveParameterToStorage(EN_TYPE_FORMAT enType, int nIndexOfItem)
		{
			string strValue			= string.Empty;
			string[] arGroup		= new string[] {enType.ToString(), nIndexOfItem.ToString()};

			foreach(EN_PARAM_LANGUAGE en in Enum.GetValues(typeof(EN_PARAM_LANGUAGE)))
			{
				m_InstanceOfLanguageDll.GetParameter(m_DicForFormat[enType], nIndexOfItem, m_DicForParam[en], ref strValue);
				m_InstanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, m_DicForStorage[en], strValue, ref arGroup, false);
			}
			m_InstanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE);
			return true;
		}
		#endregion
	}
}
