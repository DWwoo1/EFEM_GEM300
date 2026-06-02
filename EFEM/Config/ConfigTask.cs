using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineEnumProject.Task;

namespace FrameOfSystem3.Config
{
    /// <summary>
    /// 2020.07.27 by yjlee [ADD] Manages the information of the tasks on the system.
    /// </summary>
    class ConfigTask
    {
        #region SingleTon
        private ConfigTask() { }

        private static ConfigTask m_instance      = new ConfigTask();
        public static ConfigTask GetInstance() { return m_instance; }
        #endregion

        #region Constants
        const string GROUP_TASK     = "TASK";
        const string ITEM_NAME      = "NAME";
        const string ITEM_CLASSNAME = "CLASS_NAME";
        const string ITEM_STATUS    = "STATUS";

        const string ITEM_TARGET_INDEX      = "TARGET_INDEX";
        #endregion

        #region Variables
        int[] m_arIndexOfInstance       = null;
        string[] m_arListOfTask         = null;
        string[] m_arListOfClassName    = null;

        string[] m_arMappingInstanceForName     = null;

        Dictionary<string, string> m_mappingForInstance     = new Dictionary<string,string>();

        Dictionary<string, EN_TASK_STATUS> m_dicOfStatus                = new Dictionary<string,EN_TASK_STATUS>();
        Dictionary<string, EN_TASK_STATUS> m_mappingForStringStatus     = new Dictionary<string,EN_TASK_STATUS>();
        Dictionary<EN_TASK_STATUS, string> m_mappingForEnumStatus       = new Dictionary<EN_TASK_STATUS,string>();

        Dictionary<EN_TASK_DEVICE, string> m_mappingForEnumDevice       = new Dictionary<EN_TASK_DEVICE,string>();
        #endregion

        #region Internal Interface
        /// <summary>
        /// 2020.07.28 by yjlee [ADD] Makes the mapping tables for the status.
        /// </summary>
        private void MakeMappingTables()
        {
            foreach(EN_TASK_STATUS enStatus in Enum.GetValues(typeof(EN_TASK_STATUS)))
            {
                string strStatus        = enStatus.ToString();

                m_mappingForEnumStatus.Add(enStatus, strStatus);
                m_mappingForStringStatus.Add(strStatus, enStatus);
            }

            foreach(EN_TASK_DEVICE enDevice in Enum.GetValues(typeof(EN_TASK_DEVICE)))
            {
                m_mappingForEnumDevice.Add(enDevice, enDevice.ToString());
            }
        }

        /// <summary>
        /// 2020.07.27 by yjlee [ADD] Set the index of the instance of the task.
        /// </summary>
        private bool SetInformationOfTask(ref string[] arListOfGroup)
        {
            int nCountOfTask        = arListOfGroup.Length;
            int nMaximumInstance    = 0;

            m_arIndexOfInstance     = new int[nCountOfTask];
            m_arListOfTask          = new string[nCountOfTask];
            m_arListOfClassName     = new string[nCountOfTask];

            for(int nIndex = 0 ; nIndex < nCountOfTask ; ++ nIndex)
            {
                string[] arGroup        = new string[1]{arListOfGroup[nIndex]};
                int nIndexOfInstance    = 0;
                string strTaskName      = string.Empty;
                string strClassName     = string.Empty;

                if(false == int.TryParse(arListOfGroup[nIndex], out nIndexOfInstance)
                    || false == Functional.Storage.GetInstance().GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK
                    , ITEM_NAME
                    , ref strTaskName
                    , ref arGroup)
                    || false == Functional.Storage.GetInstance().GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK
                    , ITEM_CLASSNAME
                    , ref strClassName
                    , ref arGroup))
                {
                    m_arIndexOfInstance     = null;
                    m_arListOfTask          = null;
                    m_arListOfClassName     = null;

                    m_dicOfStatus.Clear();
                    m_mappingForInstance.Clear();

                    return false;
                }
                
                m_arIndexOfInstance[nIndex]     = nIndexOfInstance;
                m_arListOfTask[nIndex]          = strTaskName;
                m_arListOfClassName[nIndex]     = strClassName;

                m_mappingForInstance.Add(strTaskName, arListOfGroup[nIndex]);

                nMaximumInstance        = nIndexOfInstance > nMaximumInstance ? nIndexOfInstance : nMaximumInstance;

                string strStatus        = string.Empty;
                EN_TASK_STATUS enStatus = EN_TASK_STATUS.DISABLED;

                if(Functional.Storage.GetInstance().GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK
                    , ITEM_STATUS
                    , ref strStatus
                    , ref arGroup)
                    && m_mappingForStringStatus.ContainsKey(strStatus))
                {
                    enStatus        = m_mappingForStringStatus[strStatus];
                }

                m_dicOfStatus.Add(strTaskName, enStatus);
            }

            // 2021.01.15 by yjlee [ADD] Make the mapping array for the names by the instance.
            m_arMappingInstanceForName      = new string[nMaximumInstance + 1];

            for(int nIndex = 0 ; nIndex < nCountOfTask ; ++ nIndex)
            {
                m_arMappingInstanceForName[m_arIndexOfInstance[nIndex]] = m_arListOfTask[nIndex];
            }

            return true;
        }

        #endregion

        #region External Interface
        /// <summary>
        /// 2020.07.27 by yjlee [ADD] Initialize the components
        /// </summary>
        public bool Init()
        {
            MakeMappingTables();

            // Load the information of the task.
            if(Functional.Storage.GetInstance().Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK))
            {
                string[] arListOfGroup  = null;

                if(Functional.Storage.GetInstance().GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK
                    , ref arListOfGroup))
                {
					return SetInformationOfTask(ref arListOfGroup);
                    // Load the information of the devices of the task.
					//return Functional.Storage.GetInstance().Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE);
                }
            }

            return false;
        }

        #region Get Information
        /// <summary>
        /// 2020.07.27 by yjlee [ADD] Get the list of the task.
        /// </summary>
        public bool GetListOfTask(ref string[] arTask)
        {
            if(null != m_arListOfTask)
            {
                arTask      = new string[m_arListOfTask.Length];

                m_arListOfTask.CopyTo(arTask, 0);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 2020.07.27 by yjlee [ADD] Get the list of the index.
        /// </summary>
        public bool GetListOfIndexOfInstance(ref int[] arIndexOfInstance)
        {
            if(null != m_arIndexOfInstance)
            {
                arIndexOfInstance       = new int[m_arIndexOfInstance.Length];

                m_arIndexOfInstance.CopyTo(arIndexOfInstance, 0);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 2021.01.15 by yjlee [ADD] Get the task name by the instance.
        /// </summary>
        public void GetTaskNameByInstance(ref int nIndexOfInstance, ref string strTaskName)
        {
            if(nIndexOfInstance >= 0 && nIndexOfInstance < m_arMappingInstanceForName.Length)
            {
                strTaskName     = m_arMappingInstanceForName[nIndexOfInstance];
            }
        }

        /// <summary>
        /// 2020.07.29 by yjlee [ADD] Get the list of the class name.
        /// </summary>
        public bool GetListOfClassName(ref string[] arListOfClassName)
        {
            if(null != m_arListOfClassName)
            {
                arListOfClassName       = new string[m_arListOfClassName.Length];

                m_arListOfClassName.CopyTo(arListOfClassName, 0);

                return true;
            }

            return false;
        }
        #endregion

        #region Status
        /// <summary>
        /// 2020.07.28 by yjlee [ADD] Set the status of the task.
        /// </summary>
        public bool SetTaskStatus(string strTaskName, EN_TASK_STATUS enStatus)
        {
            if(m_dicOfStatus.ContainsKey(strTaskName))
            {
                string[] arGroup        = new string[]{m_mappingForInstance[strTaskName]};

                if(Functional.Storage.GetInstance().SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK
                    , ITEM_STATUS
                    , m_mappingForEnumStatus[enStatus]
                    , ref arGroup))
                {
                    m_dicOfStatus[strTaskName]      = enStatus;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 2020.07.28 by yjlee [ADD] Get the status of the task.
        /// </summary>
        public bool GetTaskStatus(string strTaskName, ref EN_TASK_STATUS enStatus)
        {
            if(m_dicOfStatus.ContainsKey(strTaskName))
            {
                enStatus        = m_dicOfStatus[strTaskName];

                return true;
            }

            return false;
        }
        #endregion

        #region Device Information
        /// <summary>
        /// 2020.07.29 by yjlee [ADD] Get the device information for the tasks.
        /// </summary>
		//public bool GetDeviceInfomation(ref string strTaskName, EN_TASK_DEVICE enDeviceType, ref int[] arKey, ref int[] arTargetIndex)
		//{
		//	if(m_dicOfStatus.ContainsKey(strTaskName))
		//	{
		//		string[] arGroup            = null;
		//		string[] arGroupLevel       = new string[]{strTaskName, m_mappingForEnumDevice[enDeviceType]};

		//		if(Functional.Storage.GetInstance().GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE
		//			, ref arGroup
		//			, arGroupLevel))
		//		{
		//			int nCountOfGroup   = arGroup.Length;

		//			arKey               = new int[nCountOfGroup];
		//			arTargetIndex       = new int[nCountOfGroup];

		//			int nKey            = -1;
		//			int nTargetIndex    = -1;

		//			string[] arItemLevel    = new string[]{strTaskName, m_mappingForEnumDevice[enDeviceType], string.Empty};

		//			for(int nIndex = 0 ; nIndex < nCountOfGroup ; ++ nIndex)
		//			{
		//				arItemLevel[2]      = arGroup[nIndex];

		//				if(false == int.TryParse(arGroup[nIndex], out nKey)
		//					|| false == Functional.Storage.GetInstance().GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.TASK_DEVICE
		//					, ITEM_TARGET_INDEX
		//					, ref nTargetIndex
		//					, ref arItemLevel))
		//				{
		//					arKey           = null;
		//					arTargetIndex   = null;

		//					return false;
		//				}

		//				arKey[nIndex]               = nKey;
		//				arTargetIndex[nIndex]       = nTargetIndex;
		//			}

		//			return true;
		//		}
		//	}

		//	return false;
		//}
        #endregion

        #endregion
    }
}
