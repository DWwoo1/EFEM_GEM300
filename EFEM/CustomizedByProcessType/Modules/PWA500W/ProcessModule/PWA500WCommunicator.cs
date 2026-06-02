using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WCFManager_;

using Define.DefineEnumProject.DigitalIO.PWA500W;

using EFEM.Defines.Common;
using EFEM.Modules.ProcessModule.Communicator;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.PWA500W
{
    class PWA500WCommunicator : BaseProcessModuleCommunicator
    {
        #region <Constructors>
        public PWA500WCommunicator(string[] locations, bool simulation, bool digitalIOSimulation) : base(locations, simulation, digitalIOSimulation)
        {
            _wcfManager = WCFManager.GetInstance();

            var wcfServiceIndex = (WCFServiceIndex[])Enum.GetValues(typeof(WCFServiceIndex));
            for (int i = 0; i < wcfServiceIndex.Length; ++i)
            {
                int itemIndex = (int)wcfServiceIndex[i];

                // Service
                int servicePort = 0;
                string serviceIpAddress = string.Empty;
                if (false == _wcfManager.GetParameter(itemIndex, ParameterTypeForService.ServiceIP, ref serviceIpAddress))
                    continue;
                if (false == _wcfManager.GetParameter(itemIndex, ParameterTypeForService.ServicePort, ref servicePort))
                    continue;
                CommunicateInformation.AssignServiceInfo(wcfServiceIndex[i].ToString(), itemIndex, serviceIpAddress, servicePort);

                var commTypes = wcfServiceIndex[i];
                switch (commTypes)
                {
                    case WCFServiceIndex.EFEM:
                        _wcfManager.SetOpenServiceCallback(itemIndex, OnServiceOpenedEFEM);
                        _wcfManager.SetCloseServiceCallback(itemIndex, OnServiceClosedEFEM);
                        _wcfManager.SetReceiveRequestCallback(itemIndex, OnServiceRequestedEFEM);
                        break;

                    default:
                        break;
                }
            }

            var wcfClientIndex = (WCFClientIndex[])Enum.GetValues(typeof(WCFClientIndex));
            SentMessages = new ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>>();
            ReceivedMessages = new ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>>();
            //AckToReceivedMessage = new ConcurrentDictionary<int, Dictionary<string, Tuple<CommunicationResult, string>>>();

            int simulationOffset = 0;

            for (int i = 0; i < wcfClientIndex.Length; ++i)
            {
                int itemIndex = (int)wcfClientIndex[i];
                
                string clientIpAddress = string.Empty;
                int clientPort = 0;
                if (false == _wcfManager.GetParameter(itemIndex, ParameterTypeForClient.TargetServiceIP, ref clientIpAddress))
                    continue;
                if (false == _wcfManager.GetParameter(itemIndex, ParameterTypeForClient.TargetServicePort, ref clientPort))
                    continue;
                CommunicateInformation.AssignClientInfo(wcfClientIndex[i].ToString(), itemIndex, clientIpAddress, clientPort);

                var commTypes = wcfClientIndex[i];
                switch (commTypes)
                {
                    //case WCFClientIndex.Main:
                    //    _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostMain);
                    //    _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostMain);
                    //    _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostMain);
                    //    break;
                    case WCFClientIndex.Core_8_In:
                        _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostCore_8_In);
                        _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostCore_8_In);
                        _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostCore_8_In);
                        break;
                    case WCFClientIndex.Core_8_Out:
                        _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostCore_8_Out);
                        _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostCore_8_Out);
                        _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostCore_8_Out);
                        break;
                    case WCFClientIndex.Core_12_In:
                        _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostCore_12_In);
                        _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostCore_12_In);
                        _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostCore_12_In);
                        break;
                    case WCFClientIndex.Core_12_Out:
                        _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostCore_12_Out);
                        _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostCore_12_Out);
                        _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostCore_12_Out);
                        break;
                    case WCFClientIndex.Sort_12_In:
                        _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostSort_12_In);
                        _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostSort_12_In);
                        _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostSort_12_In);
                        break;
                    case WCFClientIndex.Sort_12_Out:
                        _wcfManager.SetConnectedCallback(itemIndex, CallbackConnectedToServiceHostSort_12_Out);
                        _wcfManager.SetDisconnectedCallback(itemIndex, CallbackDisconnectedFromServiceHostSort_12_Out);
                        _wcfManager.SetFaultedConnectionCallback(itemIndex, CallbackFaultedConnectionStateWithServiceHostSort_12_Out);
                        break;
                    default:
                        break;
                }

                SentMessages[itemIndex] = new Dictionary<string, Dictionary<string, string>>();
                ReceivedMessages[itemIndex] = new Dictionary<string, Dictionary<string, string>>();
                //AckToReceivedMessage[itemIndex] = new Dictionary<string, Tuple<CommunicationResult, string>>();
            }
        }
        #endregion </Constructors>

        #region <Fields>

        #region <Constants>
        private static WCFManager _wcfManager = null;

        private const string MessageResultGood = "Ack";
        //private const string MessageResultBusy = "Busy";
        private const string MessageResultNg = "Nack";

        private const string InvalidDataPairs = "Invalid data pairs";
        private const string InvalidPortNamber = "Invalid Port Number";
        private const string DoesntHavePort = "Does not have port number";

        private const uint ReConnectInterval = 10000;

        private const string CommunicationStatusCreated = "Created";
        private const string CommunicationStatusOpening = "Opening";        
        private const string CommunicationStatusOpened = "Opened";       
        private const string CommunicationStatusClosing = "Closing";        
        private const string CommunicationStatusClosed = "Closed";
        private const string CommunicationStatusFaulted = "Faulted";
        #endregion </Constants>

        //private readonly ConcurrentDictionary<int, Dictionary<string, Tuple<CommunicationResult, string>>> AckToReceivedMessage = null;
        private readonly ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>> ReceivedMessages = null;
        private readonly ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>> SentMessages = null;
        private bool _requestExit = false;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Mapping>
        protected override void MappingLoadingInputSignals(ref Dictionary<string, int> signalInfos)
        {
            signalInfos = new Dictionary<string, int>();
            for (int i = 0; i < LocationNames.Length; ++i)
            {
                string entryway = LocationNames[i];
                if (entryway.Equals(Constants.ProcessModuleCore_8_InputName))       // ST - 5, EQ 1 - 8
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_IN.EQ_1_8_PLACE_HANDSHAKE;
                }
                else if (entryway.Equals(Constants.ProcessModuleCore_12_InputName))  // ST - 7, EQ 1 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_IN.EQ_1_12_PLACE_HANDSHAKE;
                }
                else if (entryway.Equals(Constants.ProcessModuleSort_12_InputName))  // ST - 9, EQ 2 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_IN.EQ_2_12_PLACE_HANDSHAKE;
                }
            }
        }
        protected override void MappingUnloadingInputSignals(ref Dictionary<string, int> signalInfos)
        {
            signalInfos = new Dictionary<string, int>();
            for (int i = 0; i < LocationNames.Length; ++i)
            {
                string entryway = LocationNames[i];
                if (entryway.Equals(Constants.ProcessModuleCore_8_OutputName)) // ST - 6, EQ 1 - 8
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_IN.EQ_1_8_PICK_HANDSHAKE;
                }
                else if (entryway.Equals(Constants.ProcessModuleCore_12_OutputName)) // ST - 8, EQ 1 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_IN.EQ_1_12_PICK_HANDSHAKE;
                }
                else if (entryway.Equals(Constants.ProcessModuleSort_12_OutputName)) // ST - 10, EQ 2 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_IN.EQ_2_12_PICK_HANDSHAKE;
                }
            }
        }
        protected override void MappingLoadingOutputSignals(ref Dictionary<string, int> signalInfos)
        {
            signalInfos = new Dictionary<string, int>();
            for (int i = 0; i < LocationNames.Length; ++i)
            {
                string entryway = LocationNames[i];
                if (entryway.Equals(Constants.ProcessModuleCore_8_InputName))       // ST - 5, EQ 1 - 8
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_OUT.ATM_ROBOT_HANDSHAKE_EQ_1_8_PLACE;
                }
                else if (entryway.Equals(Constants.ProcessModuleCore_12_InputName))  // ST - 7, EQ 1 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_OUT.ATM_ROBOT_HANDSHAKE_EQ_1_12_PLACE;
                }
                else if (entryway.Equals(Constants.ProcessModuleSort_12_InputName))  // ST - 9, EQ 2 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_OUT.ATM_ROBOT_HANDSHAKE_EQ_2_12_PLACE;
                }
            }
        }
        protected override void MappingUnloadingOutputSignals(ref Dictionary<string, int> signalInfos)
        {
            signalInfos = new Dictionary<string, int>();
            for (int i = 0; i < LocationNames.Length; ++i)
            {
                string entryway = LocationNames[i];
                if (entryway.Equals(Constants.ProcessModuleCore_8_OutputName)) // ST - 6, EQ 1 - 8
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_OUT.ATM_ROBOT_HANDSHAKE_EQ_1_8_PICK;
                }
                else if (entryway.Equals(Constants.ProcessModuleCore_12_OutputName)) // ST - 8, EQ 1 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_OUT.ATM_ROBOT_HANDSHAKE_EQ_1_12_PICK;
                }
                else if (entryway.Equals(Constants.ProcessModuleSort_12_OutputName)) // ST - 10, EQ 2 - 12
                {
                    signalInfos[entryway] = (int)EN_DIGITAL_OUT.ATM_ROBOT_HANDSHAKE_EQ_2_12_PICK;
                }
            }
        }

        #endregion </Mapping>

        #region <WCF>

        #region <Connection>
        public override bool InitConnection()
        {
            //foreach (var item in CommunicateInformation.ServiceInfo)
            //{
            //    int index = (int)item.Key;

            //    if (false == _wcfManager.OpenServiceHost(index))
            //        return false;
            //}

            //foreach (var item in CommunicateInformation.ClientInfo)
            //{
            //    int index = (int)item.Key;

            //    if (false == _wcfManager.ConnectToService(index))
            //        return false;
            //}
            foreach (var item in CommunicateInformation.ServiceInfo)
            {
                Task.Run(() => _wcfManager.OpenServiceHost(item.Key));
            }

            return true;
        }
        public override bool ExitCommunication()
        {
            _requestExit = true;

            return true;
        }
        private void UpdateServiceStatusAll()
        {
            if (_requestExit)
                return;

            foreach (var item in CommunicateInformation.ServiceInfo)
            {
                bool needSkip = false;
                var status = _wcfManager.GetServiceHostCommunicataionState(item.Key);
                if (string.IsNullOrEmpty(status))
                {
                    item.Value.ConnectionStatus = false;
                }
                else
                {
                    switch (status)
                    {
                        case CommunicationStatusOpened:
                            {
                                item.Value.ConnectionStatus = true;
                            }
                            break;
                        case CommunicationStatusClosed:
                        case CommunicationStatusFaulted:
                            {
                                item.Value.ConnectionStatus = false;
                            }
                            break;
                        default:
                            needSkip = true;
                            item.Value.ConnectionStatus = false;
                            break;
                    }
                }

                // Created, Opening, Closing 은 Skip..
                if (needSkip)
                    continue;

                //if (false == item.Value.ConnectionStatus)
                //{
                //    if (item.Value.IsTryConnectionCompleted)
                //    {
                //        item.Value.TryConnectToServiceAsync = Task.Run(() => _wcfManager.OpenServiceHost(item.Key));
                //    }
                //}
                //else
                //{
                //    item.Value.ResetInterval();
                //}

                #region <기존>
                //item.Value.ConnectionStatus = _wcfManager.IsOpened(item.Key);
                //if (false == item.Value.ConnectionStatus)
                //{
                //    //Created,
                //    //Opening,        
                //    //Opened,        
                //    //Closing,        
                //    //Closed,        
                //    //Faulted,
                //    if (_wcfManager.GetServiceHostCommunicataionState(item.Key).Equals("Closed") ||
                //        _wcfManager.GetServiceHostCommunicataionState(item.Key).Equals("Faulted"))
                //    {
                //        if (item.Value.IsTryConnectionCompleted)
                //        {
                //            _wcfManager.CloseServiceHost(item.Key);

                //            Console.WriteLine("-------------------------- {0} --------------------------", _wcfManager.GetServiceHostCommunicataionState(item.Key));
                //            //_wcfManager.OpenServiceHost(item.Key);
                //            item.Value.TryConnectToServiceAsync = Task.Run(() => _wcfManager.OpenServiceHost(item.Key));
                //        }
                //    }
                //    else
                //    {
                //        Console.WriteLine(_wcfManager.GetServiceHostCommunicataionState(item.Key));
                //        item.Value.SetTime = 5000;
                //    }
                //}
                //else
                //{
                //    item.Value.SetTime = 5000;
                //}
                #endregion </기존>
            }
        }
        private void UpdateServiceStatus(int index, bool connected)
        {
            if (false == CommunicateInformation.ServiceInfo.ContainsKey(index))
                return;

            CommunicateInformation.ServiceInfo[index].ConnectionStatus = connected;
        }
        private void UpdateClientStatusAll()
        {
            foreach (var item in CommunicateInformation.ClientInfo)
            {
                bool needSkip = false;
                var status = _wcfManager.GetConnectionStateWithService(item.Key);
                if (string.IsNullOrEmpty(status))
                {
                    item.Value.ConnectionStatus = false;
                }
                else
                {
                    switch (status)
                    {
                        case CommunicationStatusOpened:
                            {
                                item.Value.ConnectionStatus = true;
                            }
                            break;
                        case CommunicationStatusClosed:
                        case CommunicationStatusFaulted:
                            {
                                item.Value.ConnectionStatus = false;
                            }
                            break;
                        default:
                            needSkip = true;
                            item.Value.ConnectionStatus = false;
                            break;
                    }
                }

                // Created, Opening, Closing 은 Skip..
                if (needSkip)
                    continue;

                if (false == item.Value.ConnectionStatus)
                {
                    if (item.Value.IsTryConnectionCompleted)
                    {
                        item.Value.TryConnectToServiceAsync = Task.Run(() => _wcfManager.ConnectToService(item.Key));
                    }
                }
                else
                {
                    item.Value.ResetInterval();
                }

                #region <기존>
                //item.Value.ConnectionStatus = _wcfManager.IsConnectedToService(item.Key);
                //if (false == item.Value.ConnectionStatus)
                //{
                //    if (item.Value.IsTryConnectionCompleted)
                //    {
                //        item.Value.TryConnectToServiceAsync = Task.Run(() => _wcfManager.ConnectToService(item.Key));
                //    }
                //    //if (item.Value.TimeOver)
                //    //{
                //    //    item.Value.SetTime = ReConnectInterval;
                //    //    Task.Run(() => _wcfManager.ConnectToService(item.Key));
                //    //}
                //}
                #endregion </기존>
            }
        }
        private void UpdateClientStatus(int index, bool connected)
        {
            if (false == CommunicateInformation.ClientInfo.ContainsKey(index))
                return;

            CommunicateInformation.ClientInfo[index].ConnectionStatus = connected;
        }
        #endregion </Connection>

        #region <Callback>

        #region <Client>


        #region <Core_8_In>
        private void CallbackConnectedToServiceHostCore_8_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_8_In, true);
        }

        private void CallbackDisconnectedFromServiceHostCore_8_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_8_In, false);
        }

        private void CallbackFaultedConnectionStateWithServiceHostCore_8_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_8_In, false);
        }
        #endregion </Core_8_In>

        #region <Core_8_Out>
        private void CallbackConnectedToServiceHostCore_8_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_8_Out, true);
        }

        private void CallbackDisconnectedFromServiceHostCore_8_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_8_Out, false);
        }

        private void CallbackFaultedConnectionStateWithServiceHostCore_8_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_8_Out, false);
        }
        #endregion </Core_8_Out>

        #region <Core_12_In>
        private void CallbackConnectedToServiceHostCore_12_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_12_In, true);
        }

        private void CallbackDisconnectedFromServiceHostCore_12_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_12_In, false);
        }

        private void CallbackFaultedConnectionStateWithServiceHostCore_12_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_12_In, false);
        }
        #endregion </Core_12_In>

        #region <Core_12_Out>
        private void CallbackConnectedToServiceHostCore_12_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_12_Out, true);
        }

        private void CallbackDisconnectedFromServiceHostCore_12_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_12_Out, false);
        }

        private void CallbackFaultedConnectionStateWithServiceHostCore_12_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Core_12_Out, false);
        }
        #endregion </Core_12_Out>

        #region <Sort_12_In>
        private void CallbackConnectedToServiceHostSort_12_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Sort_12_In, true);
        }

        private void CallbackDisconnectedFromServiceHostSort_12_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Sort_12_In, false);
        }

        private void CallbackFaultedConnectionStateWithServiceHostSort_12_In(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Sort_12_In, false);
        }
        #endregion </Sort_12_In>

        #region <Sort_12_Out>
        private void CallbackConnectedToServiceHostSort_12_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Sort_12_Out, true);
        }

        private void CallbackDisconnectedFromServiceHostSort_12_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Sort_12_Out, false);
        }

        private void CallbackFaultedConnectionStateWithServiceHostSort_12_Out(string sessionId)
        {
            UpdateClientStatus((int)WCFClientIndex.Sort_12_Out, false);
        }
        #endregion </Sort_12_Out>

        #endregion </Client>

        #region <Service>

        #region <Main>
        private void OnServiceOpenedEFEM(string sessionId)
        {
            UpdateServiceStatus((int)WCFServiceIndex.EFEM, true);
        }

        private void OnServiceClosedEFEM(string sessionId)
        {
            UpdateServiceStatus((int)WCFServiceIndex.EFEM, false);
        }
        private void OnServiceRequestedEFEM(string clientPort, string title, int dataCount, string[] dataKeys, string[] dataValues, out string result, out string description)
        {
            result = string.Empty;
            description = string.Empty;

            //if (dataCount > 0)
            //{
            //    Console.WriteLine("[{0,00:d2}:{1,00:d2}:{2,00:d2}:{3,000:d3}] Service Requested >> From : {4}, Title : {5}",
            //        DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, clientPort, title);
            //}

            ClassifyReceivedMessage(clientPort, title, dataCount, dataKeys, dataValues, ref result, ref description);
        }
        #endregion </Main>

        #endregion </Service>

        #endregion </Callback>

        #region <Communication>

        #region <Send>
        public override bool SendMessage(int port, string title, Dictionary<string, string> messages)
        {
            if (messages == null)
                return false;

            if (SentMessages[port].ContainsKey(title))
            {
                SentMessages[port].Remove(title);
            }

            string responseTitle = title.Replace("Request", "Response");
            if (ReceivedMessages[port].ContainsKey(responseTitle))
            {
                ReceivedMessages[port].Remove(responseTitle);
            }

            //if (AckToReceivedMessage[port].ContainsKey(responseTitle))
            //{
            //    AckToReceivedMessage[port].Remove(responseTitle);
            //}

            _logger.WriteCommLog(port.ToString(), title, messages, false);

            if (false == _wcfManager.IsConnectedToService(port))
                return false;

            return _wcfManager.RequestDataToService(port, title, messages.Keys.ToArray(), messages.Values.ToArray());
        }
        public override CommunicationResult IsSendingCompleted(int port, string title)
        {
            string receivedResult = string.Empty;
            string receivedDescription = string.Empty;
            bool result = _wcfManager.GetResponseData(port, title, ref receivedResult, ref receivedDescription);

            if (false == result)
                return CommunicationResult.Proceed;

            string messageToLog = string.Format("[Port : {0}], [Title : {1}], [Ack : {2}], [Description : {3}]", port, title, receivedResult, receivedDescription);
            _logger.WriteCommLog(messageToLog, true);

            if (receivedResult.Equals(MessageResultGood))
                return CommunicationResult.Ack;
            else
                return CommunicationResult.Nack;
        }
        public override Dictionary<string, string> GetSendingResult(int port, string title)
        {
            return new Dictionary<string, string>();
            //if (false == IsValidPortName(port))
            //    return null;

            //if (false == SentRequestData[port].TryGetValue(title, out Dictionary<string, string> result))
            //    return null;

            //return result;
        }
        //private void CheckSendingMessageResult()
        //{
        //    for (int i = 0; i < SentMessages.Length; ++i)
        //    {
        //        int responseCount = 0;
        //        if (false == _wcfManager.GetResponseData(i, ref responseCount, ref _responseTemporaryTitles, ref _responseTemporaryResults, ref _responseTemporaryDescriptions))
        //            continue;

        //        for (int resp = 0; resp < responseCount; ++resp)
        //        {
        //            if (SentMessages[i].ContainsKey(_responseTemporaryTitles[i]))
        //            {

        //            }
        //        }
        //        foreach (var item in SentMessages[i])
        //        {

        //        }
        //    }
        //}
        #endregion </Send>

        #region <Receive>
        public override void SetAckToReceivedMessage(int port, string title, CommunicationResult result, string description)
        {
            //AckToReceivedMessage[port][title] = new Tuple<CommunicationResult, string>(result, string.Empty);
        }
        public override CommunicationResult IsMessageReceived(int port, string title)
        {
            return CommunicationResult.Ack;
        }
        public override bool GetReceivedData(int port, string title, out Dictionary<string, string> receivedData)
        {
            bool result = ReceivedMessages[port].TryGetValue(title, out receivedData);
            if (result)
            {
                ReceivedMessages[port].Remove(title);
            }
            //bool result = ReceivedMessages[port].TryGetValue(title, out receivedData);
            //if(result)
            //{
            //    ReceivedMessages[port].TryRemove(title, out _);
            //}
            return result;
        }
        private void ClassifyReceivedMessage(string clientPort, string title, int dataCount, string[] dataKeys, string[] dataValues, ref string result, ref string description)
        {
            if (string.IsNullOrEmpty(title))
                return;

            if (false == Enum.TryParse(title, out RequestMessages _) &&
                false == Enum.TryParse(title, out ResponseMessages _))
            {
                result = MessageResultNg;
                description = "약속되지 않은 커맨드입니다.";
                return;
            }
            // 데이터 유효성 확인
            if (dataKeys == null || dataValues == null || dataCount != dataKeys.Length || dataCount != dataValues.Length)
            {
                result = MessageResultNg;
                description = InvalidDataPairs;
                return;
            }

            if (false == int.TryParse(clientPort, out int clientPortNum))
            {
                result = MessageResultNg;
                description = InvalidPortNamber;
                return;
            }
            int clientItem = -1;
            if (false == CommunicateInformation.GetClientItemIndex(clientPortNum, ref clientItem))
            {
                result = MessageResultNg;
                description = DoesntHavePort;
                return;
            }

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            for (int i = 0; i < dataCount; ++i)
            {
                keyValuePairs.Add(dataKeys[i], dataValues[i]);
            }
            
            _logger.WriteCommLog(clientItem.ToString(), title, keyValuePairs, true);

            SetReceivedData(clientItem, title, keyValuePairs, ref result, ref description);
        }
        private void SetReceivedData(int clientItem, string title, Dictionary<string, string> data, ref string result, ref string description)
        {
            ReceivedMessages[clientItem][title] = data;

            result = MessageResultGood;
            description = string.Empty;
            _logger.WriteCommLog(string.Format("[Port : {0}], [Title : {1}], [Ack : {2}], [Description : {3}]",
                clientItem, title, result, description), false);
        }
        #endregion </Receive>

        #endregion </Communication>

        #endregion </WCF>

        #region <Executing>
        public override void UpdateConnectionStatus()
        {
            if (_requestExit)
                return;

            if (EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.UNDEFINED))
                return;

            UpdateServiceStatusAll();
            UpdateClientStatusAll();
        }
        #endregion </Executing>

        #endregion </Methods>
    }
}