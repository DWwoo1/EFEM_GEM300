using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using EquipmentState_;
using FileIOManager_;
using FileComposite_;

using EFEM.MaterialTracking;
using EFEM.Defines.Common;
using EFEM.Defines.ProcessModule;

namespace EFEM.Modules.ProcessModule
{
    public abstract class BaseProcessModule
    {
        #region <Constructors>
        public BaseProcessModule(int moduleIndex, Communicator.BaseProcessModuleCommunicator communicator, string name, bool simulation, bool digitalIOSimulation)
        {
            Name = name;
            ModuleIndex = moduleIndex;
            IsSimulation = simulation;
            Logger = new ProcessModuleLogger(name);

            EquipmentState = EQUIPMENT_STATE.UNDEFINED;
            RecipeId = "";

            _communicator = communicator;
            _communicator.AssignLogger(ref Logger);

            //_locationServer = LocationServer.Instance;
            //ProcessModuleLocations = new Dictionary<string, ProcessModuleLocation>();
            //LocationNames = new string[locationNames.Length];
            //for (int i = 0; i < locationNames.Length; ++i)
            //{
            //    LocationNames[i] = locationNames[i];
            //    ProcessModuleLocations[locationNames[i]] = new ProcessModuleLocation(Name, locationNames[i]);
            //}

            RegisterLocations(out var locationAndCapacity, out var entrys);
            int count = locationAndCapacity.Count;
            Capacity = new int[count];
            Locations = new string[] { Name };

            for (int i = 0; i < locationAndCapacity.Count; ++i)
            {
                Locations[i] = locationAndCapacity[i].Item1;
                Capacity[i] = locationAndCapacity[i].Item2;
            }

            if (entrys == null)
            {
                Entrys = new string[] { Name };
            }
            else
            {
                Entrys = entrys.ToArray();
            }

            MappingLocationAndEntryWay(Locations.ToList(), entrys, out _locsByEntry);
            _portIdsByEntry = new Dictionary<string, int>();
            if (_locsByEntry != null)
            {
                foreach (var item in _locsByEntry)
                {
                    _portIdsByEntry[item.Key] = 0;
                }
            }

            int[] ports = new int[_portIdsByEntry.Count];
            MappingCommunicatorPortByLocation(Entrys, ref ports);
            if (ports != null)
            {
                for (int i = 0; i < ports.Length; ++i)
                {
                    string location = entrys[i];
                    int port = ports[i];

                    _portIdsByEntry[location] = port;
                }
            }

            IsDigitalIOSimulation = digitalIOSimulation;
            _receivedData = new ConcurrentDictionary<int, Dictionary<string, string>>();
        }
        #endregion </Constructors>

        #region <Fields>
        //protected readonly ConcurrentDictionary<string, Substrate> SubstratesInHandlingLocation = null;
        //protected readonly ConcurrentDictionary<DateTime, Substrate> Substrates = null;

        protected ConcurrentDictionary<int, Dictionary<string, string>> _receivedData = null;
        protected Communicator.BaseProcessModuleCommunicator _communicator = null;
        protected readonly ProcessModuleLogger Logger = null;
        protected bool _requestExit = false;

        //private static SubstrateManager _substrateManager = null;
               
        //private static LocationServer _locationServer = null;
        private readonly Dictionary<string, int> _portIdsByEntry = null;
        private readonly Dictionary<string, string> _locsByEntry = null;
        //protected readonly Dictionary<string, ProcessModuleLocation> ProcessModuleLocations = null;
        #endregion </Fields>

        #region <Properties>
        public string Name { get; private set; }
        public int ModuleIndex { get; private set; }
        public bool IsSimulation { get; protected set; }
        public string LotId { get; set; }
        public EQUIPMENT_STATE EquipmentState { get; set; }
        public string RecipeId { get; set; }
        public NetworkInformation CommunicationInfo
        {
            get
            {
                if (_communicator == null)
                    return null;

                return _communicator.CommunicatorInfo;
            }
        }
        public string[] Locations { get; }
        public int[] Capacity { get; }
        public string[] Entrys { get; private set; }
        public IReadOnlyDictionary<string, string> LocationsByEntry => _locsByEntry;
        public IReadOnlyDictionary<string, int> PortIdsByEntry => _portIdsByEntry;
        protected static SubstrateManager SubstrateManager
        {
            get
            {
                return SubstrateManager.Instance;
            }
        }
        protected bool IsDigitalIOSimulation { get; }
        #endregion </Properties>

        #region <Methods>

        #region <SMEMA>

        #region <Send>
        public void ResetSignalsAll()
        {
            _communicator.ResetSignalsAll();
        }
        public virtual void SetLoadingSignal(string entry, bool enabled)
        {
            _communicator.SetLoadingSignal(entry, enabled);
        }
        public virtual void SetUnloadingSignal(string entry, bool enabled)
        {
            _communicator.SetUnloadingSignal(entry, enabled);
        }
        #endregion </Send>

        #region <Receive>
        public bool IsLoadingRequested(ref List<string> entrys)
        {
            if (false == IsSimulation)
            {
                if (_communicator.IsLoadingRequestedBySignal(ref entrys))
                    return true;
            }

            return IsLoadingRequestReceived(ref entrys);
        }
        public bool IsUnloadingRequested(ref List<string> entrys)
        {
            if (false == IsSimulation)
            {
                if (_communicator.IsUnloadingRequestedBySignal(ref entrys))
                    return true;
            }

            return IsUnloadingRequestReceived(ref entrys);
        }
        #endregion </Receive>

        #endregion </SMEMA>

        #region <Communication>

        #region <Connection>
        public bool InitCommunication()
        {
            return _communicator.InitConnection();
        }
        public bool ExitProcessModule()
        {
            _requestExit = true;
            return _communicator.ExitCommunication();
        }
        #endregion </Connection>

        #endregion </Communication>

        #region <Executing>
        public void Execute()
        {
            if (_requestExit)
                return;

            _communicator.UpdateReceivingInformations();

            Executing();
        }
        #endregion </Executing>

        #region <Abstract>
        public abstract void RegisterLocations(out List<Tuple<string, int>> locations, out List<string> entrys);
        public abstract void MappingLocationAndEntryWay(List<string> locations, List<string> entrys, out Dictionary<string, string> mappedEntrys);
        public abstract void MappingCommunicatorPortByLocation(string[] entrys, ref int[] ports);
        protected abstract bool IsLoadingRequestReceived(ref List<string> entrys);
        protected abstract bool IsUnloadingRequestReceived(ref List<string> entrys);
        protected abstract void Executing();

        #region <WCF>

        #region <Send>
        public abstract bool SendMessage(string entry, string title, Dictionary<string, string> messagePairs);
        public abstract bool SendMessage(string entry, string title, string substrateName);
        public abstract CommunicationResult IsSendingCompleted(string entry, string title);
        public abstract bool GetSendingResult(string entry, string title, ref Dictionary<string, string> receivedData);
        #endregion </Send>

        #region <Receive>
        public abstract void SetAckToReceivedMessage(string entry, string title, CommunicationResult result, string description);
        public abstract CommunicationResult IsMessageReceived(string entry, string title);
        public abstract bool GetReceivedData(string entry, string title, out Dictionary<string, string> receivedData);
        #endregion </Receive>

        #endregion </WCF>

        #endregion </Abstract>

        #endregion </Methods>
    }
}
