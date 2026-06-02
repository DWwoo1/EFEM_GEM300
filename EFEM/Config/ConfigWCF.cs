using System;
using System.Collections.Generic;
using System.Collections;
using FileBorn_;
using WCFManager_;

//using Define.DefineEnumProject.WCF;
using System.Text;
//using System.Web.UI.WebControls.Expressions;
using System.Linq;
using FrameOfSystem3.Views.Config;
using FrameOfSystem3.Recipe;
//using System.ServiceModel.Description;

namespace FrameOfSystem3.Config
{
    public class ConfigWCF
    {
        enum ConfigWCFParameter
        {
            Name = 0,
            Enable,
            ServiceIP,
            ServicePort,
            RelatedServiceIndex,
        }

        #region 싱글톤
        ConfigWCF() 
        {

        }

        static ConfigWCF _instance = new ConfigWCF();
        public static ConfigWCF GetInstance()
        {
            return _instance;
        }

        #endregion  /싱글톤

        #region 필드

        Functional.Storage _instanceOfStorage = null;
        WCFManager _instanceOfWCFManager = null;

        const string FILE_GROUP_SERVICE = "Service";
        const string FILE_GROUP_CLIENT = "Client";
        List<string[]> m_listForInitialize = new List<string[]>();

        readonly int INVALID_ITEM_INDEX = -1;

        Action<string> _logEventHandler = null;

        #endregion  /필드

        Dictionary<ConfigWCFParameter, ParameterTypeForService> _configParameterMappingForService = new Dictionary<ConfigWCFParameter, ParameterTypeForService>();
        Dictionary<ConfigWCFParameter, ParameterTypeForClient> _configParameterMappingForClient = new Dictionary<ConfigWCFParameter, ParameterTypeForClient>();

        Dictionary<ParameterTypeForService, ConfigWCFParameter> _serviceParameterMappingForConfigParameter = new Dictionary<ParameterTypeForService, ConfigWCFParameter>();
        Dictionary<ParameterTypeForClient, ConfigWCFParameter> _clientParameterMappingForConfigParameter = new Dictionary<ParameterTypeForClient, ConfigWCFParameter>();

        #region Item관리

        /// <summary>
        /// Config 파일을 기반으로 WCFManager에 Item을 추가하고, 파라미터를 설정한다.
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            _instanceOfStorage = Functional.Storage.GetInstance();
            _instanceOfWCFManager = WCFManager.GetInstance();
            _instanceOfWCFManager.Init(new Action<int, int, string>(CallbackForLog));

            ConfigParameterMapping();

            string[] strArrService = new string[] { FILE_GROUP_SERVICE };
            string[] strArrClient = new string[] { FILE_GROUP_CLIENT };

            m_listForInitialize.Clear();
            m_listForInitialize.Add(strArrService);
            m_listForInitialize.Add(strArrClient);

            if (false == _instanceOfStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF))
            {
                return false;
            }

            InitializeParameter();

            return true;
        }

        public int AddServiceItem()
        {
            int itemIndex = _instanceOfWCFManager.AddServiceItem();

            string[] arGroup = new string[] { FILE_GROUP_SERVICE };
            string[] arData = null;

            if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.WCF, ref arGroup, itemIndex.ToString(), ref arData)
                && _instanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, ref arData))
            {
                if (false == SaveParameterToStorage(ItemType.Service, itemIndex))
                {
                    _instanceOfWCFManager.RemoveServiceItem(itemIndex);
                    return INVALID_ITEM_INDEX;
                }
            }

            return itemIndex;
        }
        public bool RemoveServiceItem(int itemIndex)
        {
            return RemoveItem(ItemType.Service, itemIndex);
        }
        public bool GetIndexListOfServiceItem(ref int[] indexes)
        {
            return _instanceOfWCFManager.GetListofServiceItems(ref indexes);
        }

        public int AddClientItem()
        {
            int itemIndex = _instanceOfWCFManager.AddClientItem();

            string[] arGroup = new string[] { FILE_GROUP_CLIENT };
            string[] arData = null;

            if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.WCF, ref arGroup, itemIndex.ToString(), ref arData)
                && _instanceOfStorage.AddGroupAndItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, ref arData))
            {
                if (false == SaveParameterToStorage(ItemType.Client, itemIndex))
                {
                    _instanceOfWCFManager.RemoveClientItem(itemIndex);
                    return INVALID_ITEM_INDEX;
                }
            }

            return itemIndex;
        }
        public bool RemoveClientItem(int itemIndex)
        {
            return RemoveItem(ItemType.Client, itemIndex);
        }
        public bool GetIndexListOfClientItem(ref int[] indexes)
        {
            return _instanceOfWCFManager.GetListofClientItems(ref indexes);
        }

        public bool SetParameter<T>(int itemIndex, ParameterTypeForService parameterType, T tParam)
        {
            string[] arGroup = new string[] { FILE_GROUP_SERVICE, itemIndex.ToString() };

            ConfigWCFParameter configParameter = ConfigWCFParameter.Enable;

            if (true == GetConfigParameterFromServiceParameter(parameterType, ref configParameter)
                && _instanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, configParameter.ToString(), tParam, ref arGroup, true))
            {
                return _instanceOfWCFManager.SetParameter(itemIndex, parameterType, tParam);
            }

            return false;
        }
        public bool GetParameter<T>(int itemIndex, ParameterTypeForService parameterType, ref T tParam)
        {
            return _instanceOfWCFManager.GetParameter(itemIndex, parameterType, ref tParam);
        }

        public bool SetParameter<T>(int itemIndex, ParameterTypeForClient parameterType, T tParam)
        {
            string[] arGroup = new string[] { FILE_GROUP_CLIENT, itemIndex.ToString() };

            ConfigWCFParameter configParameter = ConfigWCFParameter.Enable;

            if (true == GetConfigParameterFromClientParameter(parameterType, ref configParameter)
                && _instanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, configParameter.ToString(), tParam.ToString(), ref arGroup, true))
            {
                return _instanceOfWCFManager.SetParameter(itemIndex, parameterType, tParam);
            }

            return false;
        }
        public bool GetParameter<T>(int itemIndex, ParameterTypeForClient parameterType, ref T tParam)
        {
            return _instanceOfWCFManager.GetParameter(itemIndex, parameterType, ref tParam);
        }

        #endregion  /Item관리

        #region ServiceItem관련인터페이스

        public bool OpenServiceHost(int itemIndex)
        {
            return _instanceOfWCFManager.OpenServiceHost(itemIndex);
        }
        public bool CloseServiceHost(int itemIndex)
        {
            return _instanceOfWCFManager.CloseServiceHost(itemIndex);
        }
        public bool GetRequestData(int itemIndex, ref int requestCount, ref string[] ports, ref string[] requestTitles)
        {
            return _instanceOfWCFManager.GetRequestData(itemIndex, ref requestCount, ref ports, ref requestTitles);
        }
        public bool GetRequestData(int itemIndex, string port, ref string requestTitle, ref string[] keys, ref string[] values)
        {
            return _instanceOfWCFManager.GetRequestData(itemIndex, port, ref requestTitle, ref keys, ref values);
        }
        public string GetServiceHostCommunicataionState(int itemIndex)
        {
            return _instanceOfWCFManager.GetServiceHostCommunicataionState(itemIndex);
        }

        #endregion /ServiceItem관련인터페이스

        #region ClientItem관련인터페이스

        public bool ConnectToService(int itemIndex)
        {
            return _instanceOfWCFManager.ConnectToService(itemIndex);
        }

        public bool DisconnectFromService(int itemIndex)
        {
            return _instanceOfWCFManager.DisconnectFromService(itemIndex);
        }
        public bool RequestDataToService(int itemIndex, string strTitle, string[] datakeys, string[] dataValues)
        {
            return _instanceOfWCFManager.RequestDataToService(itemIndex, strTitle, datakeys, dataValues);
        }
        public string GetConnectionStateWithService(int itemIndex)
        {
            return _instanceOfWCFManager.GetConnectionStateWithService(itemIndex);
        }
        public bool GetResponseData(int itemIndex, string strTitle, ref string strResult, ref string strDescription)
        {
            return _instanceOfWCFManager.GetResponseData(itemIndex, strTitle, ref strResult, ref strDescription);
        }

        public bool GetResponseData(int itemIndex, ref int responseCount, ref string[] strTitles, ref string[] strResults, ref string[] strDescriptions)
        {
            return _instanceOfWCFManager.GetResponseData(itemIndex, ref responseCount, ref strTitles, ref strResults, ref strDescriptions);
        }

        #endregion  /ClientItem관련인터페이스

        #region 로그관련인터페이스
        public void SetLogEventHandlerForUI(Action<string> logEventHandler)
        {
            _logEventHandler = logEventHandler;
        }

        void CallbackForLog(int itemType, int itemIndex, string log)
        {
            if(_logEventHandler != null)
            {
                DateTime now = DateTime.Now;

                StringBuilder sb = new StringBuilder(1023);
                sb.AppendFormat("===========================================================\r\n");
                sb.AppendFormat("<TIME> {0}\r\n", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"));
                sb.AppendFormat("<ITEM TYPE : {0}>\t<ITEM INDEX> : {1}\r\n\r\n", ((WCFManager_.ItemType)itemType).ToString(), itemIndex);
                sb.AppendFormat("{0}\r\n", log);
                
                _logEventHandler(sb.ToString());

                sb.Clear();
            }
        }
        #endregion  /로그관련인터페이스

        #region 내부메소드
        void ConfigParameterMapping()
        {
            _configParameterMappingForService.Add(ConfigWCFParameter.Name, ParameterTypeForService.Name);
            _configParameterMappingForService.Add(ConfigWCFParameter.Enable, ParameterTypeForService.Enable);
            _configParameterMappingForService.Add(ConfigWCFParameter.ServiceIP, ParameterTypeForService.ServiceIP);
            _configParameterMappingForService.Add(ConfigWCFParameter.ServicePort, ParameterTypeForService.ServicePort);

            _serviceParameterMappingForConfigParameter.Add(ParameterTypeForService.Name, ConfigWCFParameter.Name);
            _serviceParameterMappingForConfigParameter.Add(ParameterTypeForService.Enable, ConfigWCFParameter.Enable);
            _serviceParameterMappingForConfigParameter.Add(ParameterTypeForService.ServiceIP, ConfigWCFParameter.ServiceIP);
            _serviceParameterMappingForConfigParameter.Add(ParameterTypeForService.ServicePort, ConfigWCFParameter.ServicePort);

            _configParameterMappingForClient.Add(ConfigWCFParameter.Name, ParameterTypeForClient.Name);
            _configParameterMappingForClient.Add(ConfigWCFParameter.Enable, ParameterTypeForClient.Enable);
            _configParameterMappingForClient.Add(ConfigWCFParameter.ServiceIP, ParameterTypeForClient.TargetServiceIP);
            _configParameterMappingForClient.Add(ConfigWCFParameter.ServicePort, ParameterTypeForClient.TargetServicePort);
            _configParameterMappingForClient.Add(ConfigWCFParameter.RelatedServiceIndex, ParameterTypeForClient.RelatedServiceIndex);

            _clientParameterMappingForConfigParameter.Add(ParameterTypeForClient.Name, ConfigWCFParameter.Name);
            _clientParameterMappingForConfigParameter.Add(ParameterTypeForClient.Enable, ConfigWCFParameter.Enable);
            _clientParameterMappingForConfigParameter.Add(ParameterTypeForClient.TargetServiceIP, ConfigWCFParameter.ServiceIP);
            _clientParameterMappingForConfigParameter.Add(ParameterTypeForClient.TargetServicePort, ConfigWCFParameter.ServicePort);
            _clientParameterMappingForConfigParameter.Add(ParameterTypeForClient.RelatedServiceIndex, ConfigWCFParameter.RelatedServiceIndex);
        }
        bool GetServiceParameterFromConfigParameter(ConfigWCFParameter configParameter, ref ParameterTypeForService parameterType)
        {
            if(false == _configParameterMappingForService.TryGetValue(configParameter, out parameterType))
            {
                return false;
            }
            return true;
        }
        bool GetClientParameterFromConfigParameter(ConfigWCFParameter configParameter, ref ParameterTypeForClient parameterType)
        {
            if (false == _configParameterMappingForClient.TryGetValue(configParameter, out parameterType))
            {
                return false;
            }
            return true;
        }

        bool GetConfigParameterFromServiceParameter(ParameterTypeForService parameterType, ref ConfigWCFParameter configParameter)
        {
            if(false == _serviceParameterMappingForConfigParameter.TryGetValue(parameterType, out configParameter))
            {
                return false;
            }
            return true;
        }
        bool GetConfigParameterFromClientParameter(ParameterTypeForClient parameterType, ref ConfigWCFParameter configParameter)
        {
            if (false == _clientParameterMappingForConfigParameter.TryGetValue(parameterType, out configParameter))
            {
                return false;
            }
            return true;
        }

        void InitializeParameter()
        {
            string strValue = string.Empty;
            string strParamName = string.Empty;
            ItemType itemType = ItemType.Service;

            for (int i = 0; i < m_listForInitialize.Count; ++i)
            {
                string[] strArrGroup = m_listForInitialize.ElementAt(i);
                string[] arGroup = null;

                if (_instanceOfStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, ref arGroup, strArrGroup))
                {
                    Array.Resize(ref strArrGroup, 2);
                    for (int nIndex = 0, nEnd = arGroup.Length; nIndex < nEnd; ++nIndex)
                    {
                        strArrGroup[1] = arGroup[nIndex];

                        itemType = strArrGroup[0].Equals(FILE_GROUP_SERVICE) ? ItemType.Service : ItemType.Client;

                        int nIndexOfItem = int.Parse(arGroup[nIndex]);

                        if(itemType == ItemType.Service)
                        {
                            if(true == _instanceOfWCFManager.AddServiceItem(nIndexOfItem))
                            {
                                foreach (ConfigWCFParameter en in Enum.GetValues(typeof(ConfigWCFParameter)))
                                {
                                    ParameterTypeForService parameterType = ParameterTypeForService.Name;
                                    if (GetServiceParameterFromConfigParameter(en, ref parameterType))
                                    {
                                        if (_instanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF
                                       , en.ToString()
                                       , ref strValue
                                       , ref strArrGroup))
                                        {
                                            _instanceOfWCFManager.SetParameter(nIndexOfItem, parameterType, strValue);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if(true == _instanceOfWCFManager.AddClientItem(nIndexOfItem))
                            {
                                foreach (ConfigWCFParameter en in Enum.GetValues(typeof(ConfigWCFParameter)))
                                {
                                    ParameterTypeForClient parameterType = ParameterTypeForClient.Name;
                                    if (GetClientParameterFromConfigParameter(en, ref parameterType))
                                    {
                                        if (_instanceOfStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF
                                       , en.ToString()
                                       , ref strValue
                                       , ref strArrGroup))
                                        {
                                            _instanceOfWCFManager.SetParameter(nIndexOfItem, parameterType, strValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        bool SaveParameterToStorage(ItemType itemType, int itemIndex)
        {
            string strValue = string.Empty;

            string[] arGroup = new string[] { itemType == ItemType.Service ? FILE_GROUP_SERVICE : FILE_GROUP_CLIENT, itemIndex.ToString() };

            ConfigWCFParameter configParameter = ConfigWCFParameter.Enable;

            if (itemType == ItemType.Service)
            {
                foreach (ParameterTypeForService dllParam in Enum.GetValues(typeof(ParameterTypeForService)))
                {
                    if(true == GetConfigParameterFromServiceParameter(dllParam, ref configParameter)
                        && _instanceOfWCFManager.GetParameter(itemIndex, dllParam, ref strValue))
                    {
                        _instanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, configParameter.ToString(), strValue, ref arGroup, false);
                    }
                }
                _instanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, ConfigWCFParameter.RelatedServiceIndex.ToString(), "-1", ref arGroup, false);
            }
            else
            {
                foreach (ParameterTypeForClient dllParam in Enum.GetValues(typeof(ParameterTypeForClient)))
                {
                    if (true == GetConfigParameterFromClientParameter(dllParam, ref configParameter)
                        && _instanceOfWCFManager.GetParameter(itemIndex, dllParam, ref strValue))
                    {
                        _instanceOfStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, configParameter.ToString(), strValue, ref arGroup, false);
                    }
                }
            }
            return _instanceOfStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF);
        }

        bool RemoveItem(ItemType itemType, int itemIndex)
        {
            string[] arGroup = new string[] { itemType == ItemType.Service ? FILE_GROUP_SERVICE : FILE_GROUP_CLIENT, itemIndex.ToString() };
            if (_instanceOfStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.WCF, ref arGroup))
            {
                if(itemType == ItemType.Service)
                {
                    return _instanceOfWCFManager.RemoveServiceItem(itemIndex);
                }
                else
                {
                    return _instanceOfWCFManager.RemoveClientItem(itemIndex);
                }
            }

            return false;
        }
        #endregion /내부메소드
    }
}
