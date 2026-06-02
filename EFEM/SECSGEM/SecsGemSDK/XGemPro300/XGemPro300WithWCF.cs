using FrameOfSystem3.SECSGEM.DefineSecsGem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WCFManager_;
using XGemPro300WithWCFOnly;

namespace FrameOfSystem3.SECSGEM.SecsGemDll
{
    class XGemPro300WithWCF : XGemPro300
    {
        #region <Constructors>
        public XGemPro300WithWCF(int indexOfService, int[] indexOfClients)
        {
            _wcfManager = WCFManager.GetInstance();

            IndexOfService = indexOfService;

            // Service
            _wcfManager.SetOpenServiceCallback(IndexOfService, OnServiceOpened);
            _wcfManager.SetCloseServiceCallback(IndexOfService, OnServiceClosed);
            _wcfManager.SetReceiveRequestCallback(IndexOfService, OnServiceRequested);

            // Client
            Clients = new Dictionary<string, ClientManager>();
            for (int i = 0; i < indexOfClients.Length; ++i)
            {
                string deviceName = string.Empty;
                int indexOfItem = indexOfClients[i];
                if (false == _wcfManager.GetParameter(indexOfItem, ParameterTypeForClient.Name, ref deviceName))
                    continue;

                int port = 0;
                if (false == _wcfManager.GetParameter(indexOfItem, ParameterTypeForClient.TargetServicePort, ref port))
                    continue;

                Clients[deviceName] = new ClientManager(indexOfItem, port);

                _wcfManager.SetConnectedCallback(indexOfItem, Clients[deviceName].OnConnectedToServiceHost);
                _wcfManager.SetDisconnectedCallback(indexOfItem, Clients[deviceName].OnDisconnectedFromServiceHost);
                _wcfManager.SetFaultedConnectionCallback(indexOfItem, Clients[deviceName].OnFaultedConnectionStateWithServiceHost);
            }

            _wcfManager.OpenServiceHost(IndexOfService);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly int IndexOfService;
        private readonly Dictionary<string, ClientManager> Clients;
        private static WCFManager _wcfManager = null;
        #endregion </Fields>

        #region <Properties>
        public bool IsOpened 
        {
            get
            {
                return _wcfManager.IsOpened(IndexOfService);
            }
        }
        public bool IsConnected { get; private set; }
        #endregion </Properties>

        #region <Methods>

        #region <Externals>
        //TaskStatus status;
        public override void Execute()
        {
            base.Execute();

            foreach (var item in Clients)
            {
                if (item.Value.ConnectionStatus.Equals(ConnectionStatus.Connected))
                    continue;

                if (item.Value.IsTryConnectionCompleted)
                {
                    item.Value.TryConnectToServiceAsync = System.Threading.Tasks.Task.Run(() => _wcfManager.ConnectToService(item.Value.IndexOfItem));
                }
            }
        }

        // SendingType, ScenarioName, result 는 Receive 에만 사용...
        public override bool SendClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result)
        {
            if (false == Clients.ContainsKey(device))
                return false;

            if (false == Clients[device].ConnectionStatus.Equals(ConnectionStatus.Connected))
                return false;

            int index = Clients[device].IndexOfItem;
            
            return _wcfManager.RequestDataToService(index, messageName, contentNames, messages);
        }
        #endregion </Externals>

        #region <Callback>

        #region <Service>
        private void OnServiceOpened(string sessionId)
        {
        }

        private void OnServiceClosed(string sessionId)
        {
        }
        private void OnServiceRequested(string clientPort, string title, int dataCount, string[] dataKeys, string[] dataValues, out string result, out string description)
        {
            result = string.Empty;
            description = string.Empty;
            
            ClassifyReceivedMessage(clientPort, title, dataCount, dataKeys, dataValues, ref result, ref description);
        }
        #endregion </Service>

        #endregion </Callback>

        #region <Internal>     
        private void ClassifyReceivedMessage(string clientPort, string title, int dataCount, string[] dataKeys, string[] dataValues, ref string result, ref string description)
        {
            if (string.IsNullOrEmpty(title))
                return;

            if (false == int.TryParse(clientPort, out int portNumber))
                return;

            bool connectionStatus = false;
            string deviceName = string.Empty;
            foreach (var item in Clients)
            {
                if (item.Value.PortNumber.Equals(portNumber))
                {
                    deviceName = item.Key;
                    connectionStatus = item.Value.ConnectionStatus.Equals(ConnectionStatus.Connected);
                    break;
                }
            }

            if (false == connectionStatus)
                return;

            bool callBackResult = ClientToClientMessageReceived(deviceName, title, "", "", dataKeys, dataValues, EN_MESSAGE_RESULT.OK);

            result = callBackResult ? "Ack" : "Nack";
            description = string.Empty;
            //if (false == Enum.TryParse(title, out RequestMessages _) &&
            //    false == Enum.TryParse(title, out ResponseMessages _))
            //{
            //    result = MessageResultNg;
            //    description = "약속되지 않은 커맨드입니다.";
            //    return;
            //}
            //// 데이터 유효성 확인
            //if (dataKeys == null || dataValues == null || dataCount != dataKeys.Length || dataCount != dataValues.Length)
            //{
            //    result = MessageResultNg;
            //    description = InvalidDataPairs;
            //    return;
            //}

            //if (false == int.TryParse(clientPort, out int clientPortNum))
            //{
            //    result = MessageResultNg;
            //    description = InvalidPortNamber;
            //    return;
            //}
            //int clientItem = -1;
            //if (false == CommunicateInformation.GetClientItemIndex(clientPortNum, ref clientItem))
            //{
            //    result = MessageResultNg;
            //    description = DoesntHavePort;
            //    return;
            //}

            //Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            //for (int i = 0; i < dataCount; ++i)
            //{
            //    keyValuePairs.Add(dataKeys[i], dataValues[i]);
            //}

            //_logger.WriteCommLog(clientItem.ToString(), title, keyValuePairs, true);

            //SetReceivedData(clientItem, title, keyValuePairs, ref result, ref description);
        }
        #endregion </Internal>

        #endregion </Methods>
    }

    public class ClientManager
    {
        public ClientManager()
        {
            ConnectionStatus = ConnectionStatus.Disconnected;
        }

        public readonly int IndexOfItem;
        public readonly int PortNumber;
        //readonly WritingLogEventHandler EventHandlerLog;
        
        public Task<bool> TryConnectToServiceAsync { get; set; }
        private readonly TickCounter_.TickCounter TickForConnection;
        public ConnectionStatus ConnectionStatus { get; private set; }
        public bool IsTryConnectionCompleted
        {
            get
            {
                if (TryConnectToServiceAsync == null)
                    return true;

                switch (TryConnectToServiceAsync.Status)
                {
                    case TaskStatus.Created:
                    case TaskStatus.WaitingForActivation:
                    case TaskStatus.WaitingToRun:
                    case TaskStatus.Running:
                    case TaskStatus.WaitingForChildrenToComplete:
                        {
                            return false;
                        }

                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        {
                            if (false == TickForConnection.IsTickOver(true))
                                return false;

                            TickForConnection.SetTickCount(5000);
                            return true;
                        }

                    default:
                        return false;
                }
            }
        }

        public ClientManager(int indexOfItem, int portNumber/*, WritingLogEventHandler eventHandlerLog*/)
        {
            IndexOfItem = indexOfItem;
            PortNumber = portNumber;
            //EventHandlerLog = eventHandlerLog;
            ConnectionStatus = ConnectionStatus.Disconnected;
            TickForConnection = new TickCounter_.TickCounter();
        }
        public void OnConnectedToServiceHost(string sessionId)
        {
            ConnectionStatus = ConnectionStatus.Connected;
        }

        public void OnDisconnectedFromServiceHost(string sessionId)
        {
            ConnectionStatus = ConnectionStatus.Disconnected;
        }

        public void OnFaultedConnectionStateWithServiceHost(string sessionId)
        {
            ConnectionStatus = ConnectionStatus.Faulted;
        }

        public void WriteLog(string strLog)
        {
            //EventHandlerLog(IndexOfItem, strLog);
        }        
    }
}

namespace XGemPro300WithWCFOnly
{
    public enum ConnectionStatus
    {
        Connected = 0,
        Disconnected,
        Faulted,
    }
    public delegate void ConnectionStatusChangedEventHandler(int indexOfItem, string sessionId, ConnectionStatus newStatus);
    public delegate void WritingLogEventHandler(int itemIndex, string messageToLog);
}
