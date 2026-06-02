using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.Defines.ProcessModule;

namespace EFEM.Modules.ProcessModule.Communicator
{
    public abstract class BaseProcessModuleCommunicator
    {
        #region <Constructors>
        public BaseProcessModuleCommunicator(string[] locations, bool simulation, bool digitalIOSimulation)
        {
            SignalHandler = new SignalBasedInterfaceHandler(digitalIOSimulation);

            LocationNames = new string[locations.Length];
            Array.Copy(locations, LocationNames, locations.Length);

            CommunicateInformation = new NetworkInformation();
            Simulation = simulation;
            MappingSignals();
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly string[] LocationNames = null;
        protected readonly NetworkInformation CommunicateInformation = null;
        protected readonly SignalBasedInterfaceHandler SignalHandler = null;          // 스메마용 요청
        protected ProcessModuleLogger _logger;
        #endregion </Fields>

        #region <Properties>
        public NetworkInformation CommunicatorInfo
        {
            get
            {
                return CommunicateInformation;
            }
        }
        protected bool Simulation { get; }
        #endregion </Properties>

        #region <Methods>

        #region <Mapping>
        public void AssignLogger(ref ProcessModuleLogger logger)
        {
            _logger = logger;
        }
        private void MappingSignals()
        {
            Dictionary<string, int> loadingInputSignalInfos = new Dictionary<string, int>();
            MappingLoadingInputSignals(ref loadingInputSignalInfos);
            foreach (var item in loadingInputSignalInfos)
            {
                AssignInputSignalsInLoadingLocation(item.Key, item.Value);
            }

            Dictionary<string, int> unloadingInputSignalInfos = new Dictionary<string, int>();
            MappingUnloadingInputSignals(ref unloadingInputSignalInfos);
            foreach (var item in unloadingInputSignalInfos)
            {
                AssignInputSignalsInUnloadingLocation(item.Key, item.Value);
            }

            Dictionary<string, int> loadingOutputSignalInfos = new Dictionary<string, int>();
            MappingLoadingOutputSignals(ref loadingOutputSignalInfos);
            foreach (var item in loadingOutputSignalInfos)
            {
                AssignOutputSignalsInLoadingLocation(item.Key, item.Value);
            }

            Dictionary<string, int> unloadingOutputSignalInfos = new Dictionary<string, int>();
            MappingUnloadingOutputSignals(ref unloadingOutputSignalInfos);
            foreach (var item in unloadingOutputSignalInfos)
            {
                AssignOutputSignalsInUnloadingLocation(item.Key, item.Value);
            }
            
            ResetSignalsAll();
        }
        public void AssignInputSignalsInLoadingLocation(string location, int signalIndex)
        {
            SignalHandler.AssignInputSignalsInLoadingLocation(location, signalIndex);
        }
        public void AssignInputSignalsInUnloadingLocation(string location, int signalIndex)
        {
            SignalHandler.AssignInputSignalsInUnloadingLocation(location, signalIndex);
        }
        protected abstract void MappingLoadingInputSignals(ref Dictionary<string, int> signalInfos);
        protected abstract void MappingUnloadingInputSignals(ref Dictionary<string, int> signalInfos);
        protected abstract void MappingLoadingOutputSignals(ref Dictionary<string, int> signalInfos);
        protected abstract void MappingUnloadingOutputSignals(ref Dictionary<string, int> signalInfos);
        #endregion </Mapping>

        #region <Connection>
        public abstract bool InitConnection();
        public abstract bool ExitCommunication();
        #endregion </Connection>

        #region <Signal>

        #region <Input>
        public bool IsLoadingRequestedBySignal(ref List<string> requestedLocation)
        {
            if (Simulation)
                return false;

            return SignalHandler.IsLoadingRequestedBySignal(ref requestedLocation);
        }

        public bool IsUnloadingRequestedBySignal(ref List<string> requestedLocation)
        {
            if (Simulation)
                return false;

            return SignalHandler.IsUnloadingRequestedBySignal(ref requestedLocation);
        }
        #endregion </Input>

        #region <Output>
        public void AssignOutputSignalsInLoadingLocation(string location, int signalIndex)
        {
            SignalHandler.AssignOutputSignalsInLoadingLocation(location, signalIndex);
        }
        public void AssignOutputSignalsInUnloadingLocation(string location, int signalIndex)
        {
            SignalHandler.AssignOutputSignalsInUnloadingLocation(location, signalIndex);
        }
        public void ResetSignalsAll()
        {
            SignalHandler.ResetSignalsAll();
        }
        public void SetLoadingSignal(string location, bool enabled)
        {
            SignalHandler.SetLoadingSignal(location, enabled);
        }

        public void SetUnloadingSignal(string location, bool enabled)
        {
            SignalHandler.SetUnloadingSignal(location, enabled);
        }

        public void SetInputLoadingSignalForSimulation(string location, bool enabled)
        {
            SignalHandler.SetLoadingSignalForSimulation(location, enabled);
        }

        public void SetInputUnloadingSignalForSimulation(string location, bool enabled)
        {
            SignalHandler.SetUnloadingSignalForSimulation(location, enabled);
        }
        #endregion </Output>

        #endregion </Signal>

        #region <Communication>

        #region <Send>
        public abstract bool SendMessage(int port, string title, Dictionary<string, string> messages);
        public abstract CommunicationResult IsSendingCompleted(int port, string title);
        public abstract Dictionary<string, string> GetSendingResult(int port, string title);
        #endregion </Send>

        #region <Receive>
        public abstract void SetAckToReceivedMessage(int port, string title, CommunicationResult result, string description);
        public abstract CommunicationResult IsMessageReceived(int port, string title);
        public abstract bool GetReceivedData(int port, string title, out Dictionary<string, string> receivedData);
        #endregion </Receive>

        #endregion </Communication>

        #region <Executing>
        public abstract void UpdateConnectionStatus();

        public void UpdateReceivingInformations()
        {
            UpdateConnectionStatus();

            SignalHandler.UpdateInformations();
        }
        #endregion </Executing>

        #endregion </Methods>
    }
}
