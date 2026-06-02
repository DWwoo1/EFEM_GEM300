using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskAction_;
using FileBorn_;

using FrameOfSystem3.DynamicLink_;

namespace FrameOfSystem3.Config
{
    public class ConfigPort
    {
        #region SINGLETON
        private ConfigPort() { }
        private static ConfigPort m_instance = new ConfigPort();
        public static ConfigPort GetInstance() { return m_instance; }
        #endregion

        #region CONSTANT
        private static readonly string ITEM_PORT_ENABLE = "ENABLE";
        private static readonly string ITEM_PORT_STATUS = "STATUS";
        #endregion

        #region FIELD
        Functional.Storage m_instanceStorage = null;

        DynamicLink m_DynamicLink = null;

		/// <summary>
		/// KEY  : TASK NAME,
		/// VALUE: PORT NAMES
		/// </summary>
		Dictionary<string, string[]> m_dicPortNames = new Dictionary<string, string[]>();

        #region CONVERSION
        Dictionary<string, EN_PORT_STATUS> m_dicStatusStringToEnum = new Dictionary<string, EN_PORT_STATUS>();
        Dictionary<EN_PORT_STATUS, string> m_dicStatusEnumToString = new Dictionary<EN_PORT_STATUS, string>();
        #endregion

        #endregion

        #region INTERFACE

        #region INITIALIZE
        public bool Init()
        {
            m_instanceStorage = Functional.Storage.GetInstance();

            if (!m_instanceStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT))
                return false;

            m_DynamicLink = DynamicLink.GetInstance();

            MakeConversionTables();

            return InitPort();
        }
        #endregion

		#region GET/SET
		public bool GetPortEnable(string sTask, string sPortName)
		{
			bool bEnable	= false;

			if(m_DynamicLink.GetPortEnable(sTask, sPortName, ref bEnable))
				return bEnable;

			return false;
		}

		public bool GetPortStatus(string sTask, string sPortName, ref string strStatus)
		{
			EN_PORT_STATUS enStatue	= EN_PORT_STATUS.DISABLE;

			if(m_DynamicLink.GetPortStatus(sTask, sPortName, ref enStatue))
			{
				strStatus	= enStatue.ToString();

				return true;
			}

			return false;
		}

		public bool SetPortEnable(string sTask, string sPortName, bool bEnable)
		{
			string[] arGroup	= new string[] { sTask, sPortName };

			if (m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT
				, ITEM_PORT_ENABLE
				, bEnable
				, ref arGroup))
			{
				return m_DynamicLink.SetPortEnable(sTask, sPortName, bEnable);
			}

			return false;
		}

		/// <summary>
		/// 2021.09.13 by twkang [ADD] Set the port status
		/// </summary>
		public bool SetPortStatus(string sTask, string sPortName, string strStatus)
		{
			if(false == m_dicStatusStringToEnum.ContainsKey(strStatus))
				return false;

			return m_DynamicLink.SetPortStatus(sTask, sPortName, m_dicStatusStringToEnum[strStatus]);
		}

		/// <summary>
		/// 2021.09.13 by twkang [ADD] Get the port list
		/// </summary>
		public bool GetPortList(string sTask, ref string[] arPortName)
		{
			if(m_dicPortNames.ContainsKey(sTask))
			{
				arPortName = m_dicPortNames[sTask];

				return true;
			}

			return false;
		}
		#endregion

		#endregion

		#region METHOD

		#region INITIALIZE
		private bool InitPort()
        {
            string[] arTask = null;
			
            if (!Config.ConfigTask.GetInstance().GetListOfTask(ref arTask))
                return false;

            for (int nIndex = 0, nEnd = arTask.Length; nIndex < nEnd; ++nIndex)
            {
                string sTask = arTask[nIndex];
				
                string[] arGroupList = null;    // ARRAY 만 선언 (참조를 전달 받는 객체에서 메모리 할당)

				List<string> listOfTemp	= new List<string>();

                if (LoadListOfPortGroup(sTask, ref arGroupList))  // GROUP LIST: PORT NAME
                {
                    for (int nPort = 0; nPort < arGroupList.Length; ++nPort)
                    {
                        string sPort = arGroupList[nPort];

                        if(m_DynamicLink.AddPort(sTask, sPort))
							listOfTemp.Add(sPort);

                        bool bEnable = false;
                        string sStatus = string.Empty;
                        
                        if (LoadItemOfPort(sTask, sPort, ref bEnable, ref sStatus))
                        {
                            m_DynamicLink.SetPortEnable(sTask, sPort, bEnable);

                            if(m_dicStatusStringToEnum.ContainsKey(sStatus))
                            {
                                m_DynamicLink.SetPortStatus(sTask, sPort, m_dicStatusStringToEnum[sStatus]);
                            }
                        }
                    }

                    if (m_dicPortNames.ContainsKey(sTask).Equals(false))
                    {
                        m_dicPortNames.Add(sTask, listOfTemp.ToArray());
                    }
                }
            }

            return true;
        }
        #endregion

        #region LOAD FILE
        private bool LoadListOfPortGroup(string sTask, ref string[] arGroupList)
        {
            string[] arGroup = new string[] { sTask };

            return m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT
                , ref arGroupList
                , arGroup);
        }
        private bool LoadItemOfPort(string sTask, string sPort, ref bool bEnable, ref string sStatus)
        {
            string[] arGroup = new string[] { sTask, sPort };

            return m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT
                , ITEM_PORT_ENABLE
                , ref bEnable
                , ref arGroup)
                && m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.PORT
                , ITEM_PORT_STATUS
                , ref sStatus
                , ref arGroup);
        }
        #endregion

        #region CONVERSION
        private void MakeConversionTables()
        {
            var arEnumStatus = Enum.GetValues(typeof(EN_PORT_STATUS));

            foreach (EN_PORT_STATUS enStatus in arEnumStatus)
            {
                string sStatus = enStatus.ToString();

                m_dicStatusStringToEnum.Add(sStatus, enStatus);
                m_dicStatusEnumToString.Add(enStatus, sStatus);
            }
        }
        #endregion

        #endregion

	}
}
