using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileBorn_;
using FileComposite_;
using FileIOManager_;

using FrameOfSystem3.Log;
using Define.DefineEnumBase.Log;

namespace FrameOfSystem3.Functional
{
    /// <summary>
    /// 2020.06.04 by yjlee [ADD] Store the data in the file as a system data.
    /// </summary>
	public class Storage
    {
        #region Constructor
        private Storage() { }
        private static Storage m_instance       = new Storage();
        public static Storage GetInstance() { return m_instance; }
        #endregion

        #region Variable
        private FileComposite m_instanceComposite       = null;
        private FileIOManager m_instanceFileIO          = null;
        private LogManager m_instanceLog                = null;

        private Dictionary<Define.DefineEnumBase.Storage.EN_STORAGE_LIST, string> m_mapForStorageString  = new Dictionary<Define.DefineEnumBase.Storage.EN_STORAGE_LIST,string>();
        private Dictionary<Define.DefineEnumBase.Storage.EN_STORAGE_LIST, string> m_mapForFileName       = new Dictionary<Define.DefineEnumBase.Storage.EN_STORAGE_LIST,string>();
        private Dictionary<Define.DefineEnumBase.Storage.EN_STORAGE_LIST, BORN_LIST> m_mapForBorn       = new Dictionary<Define.DefineEnumBase.Storage.EN_STORAGE_LIST, BORN_LIST>();
        #endregion

        #region Internal Interface
        #endregion

        #region External Interface
        /// <summary>
        /// 2020.06.04 by yjlee [ADD] Initialize the variables for this class.
        /// </summary>
        public void Init()
        {
            m_instanceComposite     = FileComposite.GetInstance();
            m_instanceFileIO        = FileIOManager.GetInstance();
            m_instanceLog           = LogManager.GetInstance();

            #region Make the mapping tables

            #region File Born
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, BORN_LIST.ALARM);      
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK, BORN_LIST.ACTION);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, BORN_LIST.ANALOG);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, BORN_LIST.ANALOG_LOOKUPTABLE);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, BORN_LIST.CYLINDER);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, BORN_LIST.DIGITALIO);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER, BORN_LIST.TRIGGER);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, BORN_LIST.LANGUAGE);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, BORN_LIST.MOTION);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED, BORN_LIST.MOTION_SPEED);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, BORN_LIST.SERIAL);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET, BORN_LIST.SOCKET);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT, BORN_LIST.INTERRUPT);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW, BORN_LIST.FLOW);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK, BORN_LIST.TASK);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, BORN_LIST.TASK_DEVICE);
			m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG, BORN_LIST.JOG);
			m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE, BORN_LIST.JOG_REVERSE);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT, BORN_LIST.ACCOUNT);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT, BORN_LIST.PORT);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TOOL, BORN_LIST.TOOL);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, BORN_LIST.INTERLOCK);
            m_mapForBorn.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, BORN_LIST.WCF);
            #endregion

            #region List name
            foreach (Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList in Enum.GetValues(typeof(Define.DefineEnumBase.Storage.EN_STORAGE_LIST)))
            {
                m_mapForStorageString.Add(enList, enList.ToString());
            }
            #endregion

            #region File name
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ALARM, Define.DefineConstant.FileName.FILENAME_CONFIG_ALARM + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG, Define.DefineConstant.FileName.FILENAME_CONFIG_ANALOGIO + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.CYLINDER, Define.DefineConstant.FileName.FILENAME_CONFIG_CYLINDER + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DIGITALIO, Define.DefineConstant.FileName.FILENAME_CONFIG_DIGITALIO + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TRIGGER, Define.DefineConstant.FileName.FILENAME_CONFIG_TRIGGER + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.LANGUAGE, Define.DefineConstant.FileName.FILENAME_CONFIG_LANGUAGE + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION, Define.DefineConstant.FileName.FILENAME_CONFIG_MOTION + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.MOTION_SPEED,Define.DefineConstant.FileName.FILENAME_CONFIG_MOTION_SPEED + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SERIAL, Define.DefineConstant.FileName.FILENAME_CONFIG_SERIAL + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.SOCKET, Define.DefineConstant.FileName.FILENAME_CONFIG_SOCKET + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERRUPT, Define.DefineConstant.FileName.FILENAME_CONFIG_INTERRUPT + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ANALOG_LOOKUPTABLE, Define.DefineConstant.FileName.FILENAME_CONFIG_ANALOG_TABLE + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.FLOW, Define.DefineConstant.FileName.FILENAME_CONFIG_FLOW + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG, Define.DefineConstant.FileName.FILENAME_CONFIG_JOG + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.JOG_REVERSE, Define.DefineConstant.FileName.FILENAME_CONFIG_JOG_REVERSE + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK, Define.DefineConstant.FileName.FILENAME_CONFIG_TASK + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE, Define.DefineConstant.FileName.FILENAME_CONFIG_TASK_DEVICE + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.ACCOUNT, Define.DefineConstant.FileName.FILENAME_CONFIG_ACCOUNT + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.DYNAMIC_LINK, Define.DefineConstant.FileName.FILENAME_CONFIG_ACTION + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
			m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT, Define.DefineConstant.FileName.FILENAME_CONFIG_PORT + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TOOL, Define.DefineConstant.FileName.FILENAME_CONFIG_TOOL + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, Define.DefineConstant.FileName.FILENAME_CONFIG_INTERLOCK + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            m_mapForFileName.Add(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, Define.DefineConstant.FileName.FILENAME_CONFIG_WCF + Define.DefineConstant.FileFormat.FILEFORMAT_CONFIG);
            #endregion

            #endregion
        }

        /// <summary>
        /// 2020.06.04 by yjlee [ADD] Read a file and Make the structure.
        /// </summary>
        public bool Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList)
        {
            string strFullFilePath      = System.IO.Path.Combine(Define.DefineConstant.FilePath.FILEPATH_CONFIG
                , m_mapForFileName[enList]);

            System.IO.FileInfo fInfo    = new System.IO.FileInfo(strFullFilePath);

            if(fInfo.Exists)
            {
                string strData          = string.Empty;
                //24.04.25 [FIX] by wdw Change UTF8 for Languege crash
                if (m_instanceFileIO.Read(Define.DefineConstant.FilePath.FILEPATH_CONFIG
                    , m_mapForFileName[enList]
                    , ref strData
                    , Define.DefineConstant.FileIO.TIMEOUT_READ
                    , Encoding.UTF8))
                {
                    string[] arData     = strData.Split('\n');

                    return m_instanceComposite.CreateRootByString(ref arData);
                }
            }
            else
            {
                string[] arData         = null;
                string[] arTask         = null;

                Config.ConfigTask.GetInstance().GetListOfTask(ref arTask);

                FileBorn.GetInstance().GetBornFrame(m_mapForBorn[enList], ref arTask, ref arData);

                if(m_instanceComposite.CreateRootByString(ref arData))
                {
                    return Save(enList);
                }
            }

            return false;
        }
        /// <summary>
        /// 2020.06.04 by yjlee [ADD] Get a parameter value from the structure.
        /// </summary>
        public bool GetParameter<T>(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, string strParameterName, ref T tValue, ref string[] arGroups)
        {
            return m_instanceComposite.GetValue(m_mapForStorageString[enList], strParameterName, ref tValue, arGroups);
        }
        /// <summary>
        /// 2020.06.04 by yjlee [ADD] Set a parameter value to the structure.
        /// 2021.01.18 by yjlee [ADD] Add the configuration log.
        /// </summary>
        public bool SetParameter<T>(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, string strParameterName, T tValue, ref string[] arGroups, bool bSave = true)
        {
            string strPreviosValue      = string.Empty;
            string strType              = m_mapForStorageString[enList];

			// 2022.09.20 by junho [MOD] 신규 추가된 parameter가 있으면, 자동 추가 되도록 개선
			//if(m_instanceComposite.GetValue(strType, strParameterName, ref strPreviosValue, arGroups)
			//	&& m_instanceComposite.SetValue(strType, strParameterName, tValue, arGroups))
			//{
			//	// 2021.01.18 by yjlee [ADD] Make the full parameter name.
			//	string strFullParameterName     = arGroups[0];
                
			//	for(int nIndex = 1, nEnd = arGroups.Length ; nIndex < nEnd ; ++ nIndex)
			//	{
			//		strFullParameterName        += string.Format("_{0}", arGroups[nIndex]);
			//	}
                
			//	strFullParameterName            += string.Format("_{0}", strParameterName);
                
			//	// 2021.01.18 by yjlee [ADD] Write the configuration log.
			//	m_instanceLog.WriteConfigurationLog(ref strType, EN_CFG_TYPE.CHANGE, strFullParameterName, strPreviosValue, tValue.ToString());

			//	if(bSave)
			//	{
			//		return Save(enList);
			//	}

			//	return true;
			//}
			if (false == (m_instanceComposite.GetValue(strType, strParameterName, ref strPreviosValue, arGroups)
				&& m_instanceComposite.SetValue(strType, strParameterName, tValue, arGroups)))
			{
				if (false == m_instanceComposite.AddItem(strType, strParameterName, tValue, arGroups))
					return false;
			}

			// 2021.01.18 by yjlee [ADD] Make the full parameter name.
			string strFullParameterName = arGroups[0];

			for (int nIndex = 1, nEnd = arGroups.Length; nIndex < nEnd; ++nIndex)
			{
				strFullParameterName += string.Format("_{0}", arGroups[nIndex]);
			}

			strFullParameterName += string.Format("_{0}", strParameterName);

			// 2021.01.18 by yjlee [ADD] Write the configuration log.
			m_instanceLog.WriteConfigurationLog(ref strType, EN_CFG_TYPE.CHANGE, strFullParameterName, strPreviosValue, tValue.ToString());

			if (bSave)
			{
				return Save(enList);
			}

			return true;
        }
		/// <summary>
        /// 2023.03.22 by WDW [MOD] Save 변수 추가.
        /// 2020.06.11 by twkang [ADD] 새로운 그룹을 추가한다.
		/// </summary>
		public bool AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, ref string[] arGroups, bool bSave = true)
		{
			if(m_instanceComposite.AddGroup(m_mapForStorageString[enList], arGroups))
			{
                if (bSave)
                    return Save(enList);
                    
                return true;
			}
			
			return false;
		}

        /// <summary>
        /// 2020.06.25 by yjlee [ADD] Add the groups and the items in the root.
        /// </summary>
        public bool AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, ref string[] arData)
        {
            if (m_instanceComposite.CreateGroupAndItemByString(m_mapForStorageString[enList], ref arData))
            {
                return Save(enList);
            }

            return false;
        }

        /// <summary>
        /// 2020.06.22 by yjlee [ADD] Copy the group to a destination.
        /// </summary>
        public bool CopyGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, string strDestinationGroup, ref string[] arGroups)
        {
            if (m_instanceComposite.CopyGroup(m_mapForStorageString[enList], strDestinationGroup, arGroups))
            {
                return Save(enList);
            }

            return false;
        }
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 새로운 아이템을 추가한다.
		/// </summary>
		public bool AddItem<T>(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, string strParameterName, T tValue, ref string[] arGroups, bool bSave = true)
		{
			if(m_instanceComposite.AddItem(m_mapForStorageString[enList], strParameterName, tValue, arGroups))
			{
				if(bSave)
					return Save(enList);
				else
					return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] 특정 그룹을 제거한다.
		/// </summary>
		public bool RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, ref string[] arGroups)
		{
			if(m_instanceComposite.RemoveGroup(m_mapForStorageString[enList], arGroups))
			{
				return Save(enList);
			}
			
			return false;
		}
		/// <summary>
		/// 2020.06.11 by twkang [ADD] FileComposite 의 데이터를 저장한다.
		/// </summary>
		public bool Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList)
		{
			string strData			= string.Empty;
            string strType          = m_mapForStorageString[enList];

			if (m_instanceComposite.GetStructureAsString(strType, ref strData))
			{
                string strFileName      = m_mapForFileName[enList];

                //24.04.25 [FIX] by wdw Change UTF8 for Languege crash
                if (m_instanceFileIO.Write(Define.DefineConstant.FilePath.FILEPATH_CONFIG
                    , strFileName
					, strData
					, false
                    , false
                    , Encoding.UTF8))
                {
                    string strFullFilePath      = System.IO.Path.Combine(Define.DefineConstant.FilePath.FILEPATH_CONFIG, strFileName);

                    // 2021.01.18 by yjlee [ADD] Write the configuration log.
                    m_instanceLog.WriteConfigurationLog(ref strType, EN_CFG_TYPE.SAVE, string.Empty, string.Empty, strFullFilePath);

                    return true;
                }
			}

			return false;
		}
		/// <summary>
		/// 2020.06.22 by twkang [ADD] 파일의 그룹 이름 리스트를 받는다.
		/// </summary>
		public bool GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST enList, ref string[] arGroup, string[] arGroupLevel = null)
		{
            if(null == arGroupLevel)
            {
                return m_instanceComposite.GetListOfGroup(m_mapForStorageString[enList], ref arGroup);
            }
            else
            {
                return m_instanceComposite.GetListOfGroup(m_mapForStorageString[enList], ref arGroup, arGroupLevel);
            }
		}
        /// <summary>
        /// 2021.11.08 by wdw [ADD] 그룹 레벨을 추가한다.
        /// </summary>
        public string[] AddGroupLevel(string[] arGroupLevel, string strAddItem)
        {
            string[] arReturn = new string[arGroupLevel.Length + 1];
            int i;
            for (i = 0; i < arGroupLevel.Length; i++)
            {
                arReturn[i] = arGroupLevel[i];
            }
            arReturn[i] = strAddItem;
            return arReturn;
        }
        #endregion
    }
}
