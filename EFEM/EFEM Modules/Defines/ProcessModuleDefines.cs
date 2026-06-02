using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;

namespace EFEM.Defines.ProcessModule
{
    public class NetworkInformation
    {
        #region <Constructors>
        public NetworkInformation()
        {
            ServiceInfo = new ConcurrentDictionary<int, AddressInformation>();
            ClientInfo = new ConcurrentDictionary<int, AddressInformation>();
        }
        #endregion </Constructors>

        #region <Properties>
        public ConcurrentDictionary<int, AddressInformation> ServiceInfo { get; private set; }
        public ConcurrentDictionary<int, AddressInformation> ClientInfo { get; private set; }
        #endregion </Properties>

        #region <Methods>
        public void AssignServiceInfo(string name, int itemIndex, string ipAddress, int port)
        {
            ServiceInfo[itemIndex] = new AddressInformation(name, ipAddress, port);
        }
        public void AssignClientInfo(string name, int itemIndex, string ipAddress, int port)
        {
            ClientInfo[itemIndex] = new AddressInformation(name, ipAddress, port);
        }
        public bool GetClientItemIndex(/*string ipAddress, */int port, ref int itemIndex)
        {
            itemIndex = -1;
            if (ClientInfo == null)
                return false;

            foreach (var item in ClientInfo)
            {
                if (/*item.Value.IpAddress.Equals(ipAddress) && */item.Value.Port.Equals(port))
                {
                    itemIndex = item.Key;
                    return true;
                }
            }

            return false;
        }
        #endregion </Methods>
    }
    public class AddressInformation
    {
        public AddressInformation(string name, string ipAddress, int port)
        {
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            Timer = new TickCounter_.TickCounter();
        }

        public const uint RetryConnectionIntervalTime = 10000;
        private readonly TickCounter_.TickCounter Timer = null;        
        public string Name { get; private set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public bool ConnectionStatus { get; set; }
        public Task<bool> TryConnectToServiceAsync { get; set; }

        public bool TimeOver
        {
            get
            {
                return Timer.IsTickOver(true);
            }
        }
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
                            if (false == TimeOver)
                                return false;

                            ResetInterval();
                            return true;
                        }

                    default:
                        return false;
                }
            }
        }
        public void ResetInterval()
        {
            Timer.SetTickCount(RetryConnectionIntervalTime);
        }
    }
    public class ProcessModuleLogger : ModuleLogger
    {
        public ProcessModuleLogger(string name) : base(BaseLogTypes.LogTypeProcessModule, name, true) 
        {
            BuilderToWrite = new StringBuilder();
        }
        private readonly StringBuilder BuilderToWrite = null;
        public void WriteCommLog(string port, string title, Dictionary<string, string> data, bool received)
        {
            BuilderToWrite.Clear();
            BuilderToWrite.Append(string.Format("[Port : {0}], [Title : {1}]", port, title));
            
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    BuilderToWrite.Append(string.Format(", [Key : {0} Value : {1}]", item.Key, item.Value));
                }
            }

            WriteCommLog(BuilderToWrite.ToString(), received);
        }
        public void WriteCommLog(string message, bool received)
        {
            if (false == received)
            {
                WriteLog(LogTitleTypes.SEND, message);
            }
            else
            {
                WriteLog(LogTitleTypes.RECV, message);
            }            
        }
    }
}