using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EquipmentState_;
using TickCounter_;

using EFEM.Defines.Common;
using EFEM.Defines.MaterialTracking;
using EFEM.Modules.ProcessModule;
using EFEM.Modules.ProcessModule.Communicator;
using EFEM.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace EFEM.CustomizedByProcessType.PWA500BIN
{
    public class ProcessModulePWA500BIN : BaseProcessModule
    {
        #region <Constructors>
        public ProcessModulePWA500BIN(int moduleIndex, BaseProcessModuleCommunicator communicator, string name, bool simulation, bool digitalIOSimulation)
            : base(moduleIndex, communicator, name, simulation, digitalIOSimulation) 
        {
            HandlingLoadRequestedForSimulator = new ConcurrentDictionary<string, bool>();
            HandlingUnloadRequestedForSimulator = new ConcurrentDictionary<string, bool>();
            TickForLoading = new Dictionary<string, TickCounter>();
            TickForUnloading = new Dictionary<string, TickCounter>();

            // 시뮬용임. 정리 필요
            for (int i = 0; i < Entrys.Length; ++i)
            {
                if (Entrys[i].Contains(Constants.LoadingToken))
                {
                    HandlingLoadRequestedForSimulator[Entrys[i]] = false;
                    TickForLoading[Entrys[i]] = new TickCounter();
                }
                else
                {
                    HandlingUnloadRequestedForSimulator[Entrys[i]] = false;
                    TickForUnloading[Entrys[i]] = new TickCounter();
                }
            }

            MinTicksForCoreHandling = 5000;
            MinTicksForSortHandling = 7000;

            Task.Run(() => InitCommunication());
        }
        #endregion </Constructors>

        #region <Fields>
        private const int MaxCapacityCore = 3;
        private const int MaxCapacityBin = 3;
        private const int ProcessingTime = 5;       // Sec
        private const string HandlingResultOk = "Ok";
        private readonly ConcurrentDictionary<string, bool> HandlingLoadRequestedForSimulator = null;
        private readonly ConcurrentDictionary<string, bool> HandlingUnloadRequestedForSimulator = null;

        private readonly uint MinTicksForCoreHandling;
        private readonly uint MinTicksForSortHandling;

        private readonly Dictionary<string, TickCounter> TickForLoading = null;
        private readonly Dictionary<string, TickCounter> TickForUnloading = null;

        private ConcurrentDictionary<string, Substrate> _substrates = new ConcurrentDictionary<string, Substrate>();
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Override Methods>       
        public override void SetLoadingSignal(string entry, bool enabled)
        {
            if (IsSimulation && false == enabled)
            {
                if (false == TickForLoading.ContainsKey(entry))
                    return;

                if (entry.Contains("Core"))
                {
                    TickForLoading[entry].SetTickCount(MinTicksForCoreHandling);
                }
                else
                {
                    TickForLoading[entry].SetTickCount(MinTicksForSortHandling);
                }
            }

            base.SetLoadingSignal(entry, enabled);
        }

        public override void SetUnloadingSignal(string entry, bool enabled)
        {
            if (IsSimulation && false == enabled)
            {
                if (false == TickForUnloading.ContainsKey(entry))
                    return;

                if (entry.Contains("Core"))
                {
                    TickForUnloading[entry].SetTickCount(MinTicksForCoreHandling);
                }
                else
                {
                    TickForUnloading[entry].SetTickCount(MinTicksForSortHandling);
                }
            }
            base.SetUnloadingSignal(entry, enabled);
        }
        public override void RegisterLocations(out List<Tuple<string, int>> locations, out List<string> entrys)
        {
            locations = new List<Tuple<string, int>>
            {
                Tuple.Create(Name, 10)
            };

            entrys = new List<string> 
            {
                Constants.ProcessModuleCoreInputName,
                Constants.ProcessModuleSortInputName,
                Constants.ProcessModuleCoreOutputName,
                Constants.ProcessModuleSortOutputName,
            };
        }
        public override void MappingLocationAndEntryWay(List<string> locations, List<string> entrys, out Dictionary<string, string> mappedEntrys)
        {
            mappedEntrys = new Dictionary<string, string>()
            {
                [Constants.ProcessModuleCoreInputName] = Name,
                [Constants.ProcessModuleSortInputName] = Name,
                [Constants.ProcessModuleCoreOutputName] = Name,
                [Constants.ProcessModuleSortOutputName] = Name,
            };
        }
        public override void MappingCommunicatorPortByLocation(string[] entrys, ref int[] ports)
        {
            for (int i = 0; i < entrys.Length; ++i)
            {
                string location = entrys[i];
                WCFClientIndex clientIndex = WCFClientIndex.CoreIn;
                bool invalidLocation = false;
                switch (location)
                {
                    case Constants.ProcessModuleCoreInputName:
                        clientIndex = WCFClientIndex.CoreIn;
                        break;
                    case Constants.ProcessModuleSortInputName:
                        clientIndex = WCFClientIndex.SortIn;
                        break;
                    case Constants.ProcessModuleCoreOutputName:
                        clientIndex = WCFClientIndex.CoreOut;
                        break;
                    case Constants.ProcessModuleSortOutputName:
                        clientIndex = WCFClientIndex.SortOut;
                        break;

                    default:
                        invalidLocation = true;
                        break;
                }

                if (invalidLocation)
                    continue;

                ports[i] = (int)clientIndex;
            }
        }
        protected override bool IsLoadingRequestReceived(ref List<string> entrys)
        {
            if (IsSimulation)
            {
                return GetLoadingRequestForSimulator(ref entrys);
            }

            return false;
        }
        protected override bool IsUnloadingRequestReceived(ref List<string> entrys)
        {
            if (IsSimulation)
            {
                return GetUnloadingRequestForSimulator(ref entrys);
            }

            return false;
        }
        protected override void Executing()
        {
            if (IsSimulation)
            {
                UpdateSubstratesForSimul();
                UpdateProcessStatesForSimul();
                MoveSubstrateLocationLoadToUnloadForSimulator();
            }
            else if (IsDigitalIOSimulation)
            {
                UpdateSmemaAtSimul();
            }
        }

        private void UpdateSmemaAtSimul()
        {
            foreach (var item in PortIdsByEntry)
            {
                if (GetReceivedData(item.Key, RequestMessages.RequestLoadingSmemaOnAtSimul.ToString(), out _))
                {
                    _communicator.SetInputLoadingSignalForSimulation(item.Key, true);
                }

                if (GetReceivedData(item.Key, RequestMessages.RequestLoadingSmemaOffAtSimul.ToString(), out _))
                {
                    _communicator.SetInputLoadingSignalForSimulation(item.Key, false);
                }

                if (GetReceivedData(item.Key, RequestMessages.RequestUnloadingSmemaOnAtSimul.ToString(), out _))
                {
                    _communicator.SetInputUnloadingSignalForSimulation(item.Key, true);
                }

                if (GetReceivedData(item.Key, RequestMessages.RequestUnloadingSmemaOffAtSimul.ToString(), out _))
                {
                    _communicator.SetInputUnloadingSignalForSimulation(item.Key, false);
                }
            }
        }
        #region <WCF>

        #region <Send>
        public override bool SendMessage(string entry, string title, string substrateName)
        {
            return SendRequestMessage(entry, title, substrateName);
        }
        public override bool SendMessage(string entry, string title, Dictionary<string, string> messagePairs)
        {
            return SendRequestMessage(entry, title, messagePairs);
        }
        public override CommunicationResult IsSendingCompleted(string entry, string title)
        {
            return _communicator.IsSendingCompleted(PortIdsByEntry[entry], title);
        }
        public override bool GetSendingResult(string entry, string title, ref Dictionary<string, string> receivedData)
        {
            return false;
        }
        #endregion </Send>

        #region <Receive>
        public override void SetAckToReceivedMessage(string entry, string title, CommunicationResult result, string description)
        {
            _communicator.SetAckToReceivedMessage(PortIdsByEntry[entry], title, result, description);
        }
        public override CommunicationResult IsMessageReceived(string entry, string title)
        {
            return _communicator.IsMessageReceived(PortIdsByEntry[entry], title);
        }
        public override bool GetReceivedData(string entry, string title, out Dictionary<string, string> receivedData)
        {
            return _communicator.GetReceivedData(PortIdsByEntry[entry], title, out receivedData);
        }
        #endregion </Receive>

        #endregion </WCF>

        #endregion </Override Methods>

        #region <Internals>
        private void UpdateSubstratesForSimul()
        {
            var list = new List<Substrate>();
            if (!SubstrateManager.GetSubstratesAtProcessModule(Name, ref list))
            {
                var empty = new ConcurrentDictionary<string, Substrate>(StringComparer.OrdinalIgnoreCase);
                System.Threading.Interlocked.Exchange(ref _substrates, empty);
                return;
            }

            var next = new ConcurrentDictionary<string, Substrate>(StringComparer.OrdinalIgnoreCase);
            foreach (var s in list)
            {
                var key = s?.UniqueKey;
                if (string.IsNullOrWhiteSpace(key)) continue;
                next[key] = s;
            }

            System.Threading.Interlocked.Exchange(ref _substrates, next);
        }
        private void UpdateProcessStatesForSimul()
        {
            foreach (var item in _substrates)
            {
                if (false == item.Value.ProcessingStatus.Equals(ProcessingStates.Processed))
                {
                    SubstrateManager.SetProcessingStatusByKey(item.Value.UniqueKey, ProcessingStates.Processed);
                    SubstrateManager.SaveDataByKey(item.Value.UniqueKey);
                }
            }
        }
        
        #region <Simulation Only>
        private bool GetLoadingRequestForSimulator(ref List<string> entrys)
        {
            foreach (var item in HandlingLoadRequestedForSimulator)
            {
                if (false == TickForLoading[item.Key].IsTickOver(true))
                    continue;

                if (item.Value)
                    entrys.Add(item.Key);
            }

            return entrys.Count > 0;
        }
        private bool GetUnloadingRequestForSimulator(ref List<string> entrys)
        {
            foreach (var item in HandlingUnloadRequestedForSimulator)
            {
                if (false == TickForUnloading[item.Key].IsTickOver(true))
                    continue;

                if (item.Value)
                    entrys.Add(item.Key);
            }

            return entrys.Count > 0;
        }
        private void MoveSubstrateLocationLoadToUnloadForSimulator()
        {
            int countCore = 0, countEmpty = 0;
            int countCoreCompleted = 0, countBin = 0;
            foreach (var item in _substrates)
            {
                string subType = item.Value.GetAttribute(PWA500MaterialHandling.SubstrateType);
                if (false == Enum.TryParse(subType, out SubstrateType convertedType))
                    continue;

                switch (convertedType)
                {
                    case SubstrateType.Core:
                        {
                            ++countCore;
                            if (item.Value.ProcessingStatus.Equals(ProcessingStates.Processed))
                            {
                                ++countCoreCompleted;
                            }
                        }
                        break;
                    case SubstrateType.Empty:
                    case SubstrateType.Bin1:
                    case SubstrateType.Bin2:
                    case SubstrateType.Bin3:
                        {
                            ++countEmpty;
                            if (item.Value.ProcessingStatus.Equals(ProcessingStates.Processed))
                            {
                                ++countBin;
                            }
                        }
                        break;
                    default:
                        break;
                }               
            }

            HandlingLoadRequestedForSimulator[Constants.ProcessModuleCoreInputName] = (countCore < MaxCapacityCore);
            HandlingLoadRequestedForSimulator[Constants.ProcessModuleSortInputName] = (countEmpty < MaxCapacityBin);
            HandlingUnloadRequestedForSimulator[Constants.ProcessModuleCoreOutputName] = (countCoreCompleted >= 1);
            HandlingUnloadRequestedForSimulator[Constants.ProcessModuleSortOutputName] = (countBin >= 1);
        }
        #endregion </Simulation Only>

        #region <WCF>
        private void MakeStructureToSend(Substrate substrate, ref Dictionary<string, string> messageToSend)
        {
            messageToSend.Clear();

            messageToSend[PWA500MaterialHandling.HandlingResult] = HandlingResultOk;
            if (substrate == null)
            {
                messageToSend[PWA500MaterialHandling.SubstrateName] = string.Empty;
                messageToSend[PWA500MaterialHandling.LotId] = string.Empty;
                messageToSend[PWA500MaterialHandling.RecipeId] = string.Empty;
                messageToSend[PWA500MaterialHandling.SubstrateType] = string.Empty;
                messageToSend[PWA500MaterialHandling.RingId] = string.Empty;
                messageToSend[PWA500MaterialHandling.PortId] = string.Empty;
                messageToSend[PWA500MaterialHandling.SlotId] = string.Empty;
            }
            else
            {
                //var attributes = substrate.GetAttributesAll();

                messageToSend[PWA500MaterialHandling.SubstrateName] = substrate.Name;
                messageToSend[PWA500MaterialHandling.LotId] = substrate.LotId;
                messageToSend[PWA500MaterialHandling.RecipeId] = substrate.RecipeId;
                messageToSend[PWA500MaterialHandling.RingId] = substrate.Name;
                messageToSend[PWA500MaterialHandling.PortId] = substrate.SourcePortId.ToString();
                messageToSend[PWA500MaterialHandling.SlotId] = substrate.SourceSlot.ToString();
                
                switch (substrate.SourcePortId)
                {
                    case 1:
                    case 2:
                    case 3:
                        messageToSend[PWA500MaterialHandling.SubstrateType] = SubstrateType.Bin2.ToString();
                        break;
                    case 4:
                        messageToSend[PWA500MaterialHandling.SubstrateType] = SubstrateType.Empty.ToString();
                        break;
                    case 5:
                    case 6:
                        messageToSend[PWA500MaterialHandling.SubstrateType] = SubstrateType.Core.ToString();
                        break;
                    default:
                        break;
                }

            }
        }
        private Dictionary<string, string> MakeMessagesBySubstrateName(string title, string substrateName)
        {
            Dictionary<string, string> messageToSend = new Dictionary<string, string>();

            Substrate substrate;
            if (title == RequestMessages.RequestApproachUnloading.ToString())
            {
                // 알 수도 모를 수도 있음
                FrameOfSystem3.SECSGEM.FunctionsForPWA500BIN_TP.Instance.GetSubstrateAtProcessModuleByName(substrateName, out substrate);
            }
            else if (title == RequestMessages.RequestActionUnloading.ToString())
            {
                // 알아야함(ApproachUnloading 시점에 자재 Key 가 갱신됨)
                if (false == FrameOfSystem3.SECSGEM.FunctionsForPWA500BIN_TP.Instance.GetSubstrateAtProcessModuleByName(substrateName, out substrate) ||
                    substrate == null)
                    return messageToSend;
            }
            else
            {
                if (false == FrameOfSystem3.SECSGEM.FunctionsForPWA500BIN_TP.Instance.GetSubstrateByName(substrateName, out substrate) ||
                    substrate == null)
                    return messageToSend;
            }

            MakeStructureToSend(substrate, ref messageToSend);

            return messageToSend;
        }
        private bool SendRequestMessage(string entry, string title, Dictionary<string, string> messagePairs)
        {
            if (false == Enum.TryParse(title, out RequestMessages _))
                return false;

            if (false == PortIdsByEntry.ContainsKey(entry))
                return false;

            return _communicator.SendMessage(PortIdsByEntry[entry], title, messagePairs);
        }
        private bool SendRequestMessage(string entry, string title, string substrateName)
        {
            if (false == Enum.TryParse(title, out RequestMessages _))
                return false;

            if (false == PortIdsByEntry.ContainsKey(entry))
                return false;

            var structure = MakeMessagesBySubstrateName(title, substrateName);
            if (structure == null)
                return false;

            return _communicator.SendMessage(PortIdsByEntry[entry], title, structure);
        }
        #endregion </WCF>

        #endregion </Internals>

        #endregion </Methods>
    }
}