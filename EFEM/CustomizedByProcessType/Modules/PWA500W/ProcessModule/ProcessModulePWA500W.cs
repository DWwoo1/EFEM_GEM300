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

namespace EFEM.CustomizedByProcessType.PWA500W
{
    public class ProcessModulePWA500W : BaseProcessModule
    {
        #region <Constructors>
        public ProcessModulePWA500W(int moduleIndex, BaseProcessModuleCommunicator communicator, string name, bool simulation, bool digitalIOSimulation)
            : base(moduleIndex, communicator, name, simulation, digitalIOSimulation)
        {
            Task.Run(() => InitCommunication());
        }
        #endregion </Constructors>

        #region <Fields>
        private const string HandlingResultOk = "Ok";

        private readonly TickCounter _ticksForCore = new TickCounter();
        private readonly TickCounter _ticksForBin = new TickCounter();

        private readonly ConcurrentDictionary<string, Substrate> _coreSubstrates = new ConcurrentDictionary<string, Substrate>();
        private readonly ConcurrentDictionary<string, Substrate> _binSubstrates = new ConcurrentDictionary<string, Substrate>();

        private List<string> _loadingReqSnapshot = new List<string>();
        private List<string> _unloadingReqSnapshot = new List<string>();
        private readonly object _reqSnapLock = new object();

        private ConcurrentDictionary<string, Substrate> _inputCoreSubstrates = new ConcurrentDictionary<string, Substrate>();
        private ConcurrentDictionary<string, Substrate> _inputBinSubstrates = new ConcurrentDictionary<string, Substrate>();

        private ConcurrentDictionary<string, Substrate> _outputCoreSubstrates = new ConcurrentDictionary<string, Substrate>();
        private ConcurrentDictionary<string, Substrate> _outputBinSubstrates = new ConcurrentDictionary<string, Substrate>();

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
            }
            base.SetLoadingSignal(entry, enabled);
        }
        public override void SetUnloadingSignal(string entry, bool enabled)
        {
            if (IsSimulation && false == enabled)
            {
            }
            base.SetUnloadingSignal(entry, enabled);
        }

        public override void RegisterLocations(out List<Tuple<string, int>> locations, out List<string> entrys)
        {
            locations = new List<Tuple<string, int>>
            {
                Tuple.Create(Name, 6)
            };

            entrys = new List<string> 
            {
                Constants.ProcessModuleCore_8_InputName,
                Constants.ProcessModuleCore_8_OutputName,
                Constants.ProcessModuleCore_12_InputName,
                Constants.ProcessModuleCore_12_OutputName,
                Constants.ProcessModuleSort_12_InputName,
                Constants.ProcessModuleSort_12_OutputName,
            };
        }
        public override void MappingLocationAndEntryWay(List<string> locations, List<string> entrys, out Dictionary<string, string> mappedEntrys)
        {
            mappedEntrys = new Dictionary<string, string>()
            {
                [Constants.ProcessModuleCore_8_InputName] = Name,
                [Constants.ProcessModuleCore_8_OutputName] = Name,
                [Constants.ProcessModuleCore_12_InputName] = Name,
                [Constants.ProcessModuleCore_12_OutputName] = Name,
                [Constants.ProcessModuleSort_12_InputName] = Name,
                [Constants.ProcessModuleSort_12_OutputName] = Name,
            };
        }
        public override void MappingCommunicatorPortByLocation(string[] entrys, ref int[] ports)
        {
            for (int i = 0; i < entrys.Length; ++i)
            {
                string location = entrys[i];
                WCFClientIndex clientIndex = WCFClientIndex.Core_8_In;
                bool invalidLocation = false;
                switch (location)
                {
                    case Constants.ProcessModuleCore_8_InputName:
                        clientIndex = WCFClientIndex.Core_8_In;
                        break;
                    case Constants.ProcessModuleCore_8_OutputName:
                        clientIndex = WCFClientIndex.Core_8_Out;
                        break;
                    case Constants.ProcessModuleCore_12_InputName:
                        clientIndex = WCFClientIndex.Core_12_In;
                        break;
                    case Constants.ProcessModuleCore_12_OutputName:
                        clientIndex = WCFClientIndex.Core_12_Out;
                        break;
                    case Constants.ProcessModuleSort_12_InputName:
                        clientIndex = WCFClientIndex.Sort_12_In;
                        break;
                    case Constants.ProcessModuleSort_12_OutputName:
                        clientIndex = WCFClientIndex.Sort_12_Out;
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
                UpdateSubstrateListForSimul();
                UpdateSubstratesSimul();
                ProcessSubstratesForSimul();
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

        #region <Simulation Only>
        private bool GetLoadingRequestForSimulator(ref List<string> entrys)
        {
            entrys.Clear();
            List<string> snap;
            lock (_reqSnapLock)
            {
                // 얕은 복사로 안전하게 전달
                snap = _loadingReqSnapshot.Count > 0
                    ? new List<string>(_loadingReqSnapshot)
                    : null;
            }

            if (snap != null)
                entrys.AddRange(snap);

            return entrys.Count > 0;
        }
        private bool GetUnloadingRequestForSimulator(ref List<string> entrys)
        {
            entrys.Clear();
            List<string> snap;
            lock (_reqSnapLock)
            {
                snap = _unloadingReqSnapshot.Count > 0
                    ? new List<string>(_unloadingReqSnapshot)
                    : null;
            }

            if (snap != null)
                entrys.AddRange(snap);

            return entrys.Count > 0;
        }
        private void ProcessSubstratesForSimul()
        {
            string coreName = string.Empty, binName = string.Empty;

            #region <Core>
            _inputCoreSubstrates.Clear();
            _outputCoreSubstrates.Clear();

            // 1) 작업 중인 것이 있는지 확인
            foreach (var item in _coreSubstrates)
            {
                var proc = item.Value.ProcessingStatus;
                switch (proc)
                {
                    case ProcessingStates.Processed:
                    case ProcessingStates.Rejected:
                    case ProcessingStates.Stopped:
                    case ProcessingStates.Aborted:
                    case ProcessingStates.Skipped:
                    case ProcessingStates.Lost:
                        {
                            _outputCoreSubstrates[item.Key] = item.Value;
                        }
                        break;
                    default:
                        {
                            _inputCoreSubstrates[item.Key] = item.Value;
                        }
                        break;
                }

                if (item.Value.ProcessingStatus == ProcessingStates.InProcess)
                {
                    coreName = item.Value.Name;
                }
            }

            // 2) 작업 중인 것이 없으면, 대기중인 것이 있는지 확인
            if (string.IsNullOrEmpty(coreName))
            {
                foreach (var item in _inputCoreSubstrates)
                {
                    if (item.Value.ProcessingStatus == ProcessingStates.NeedsProcessing)
                    {
                        coreName = item.Value.Name;
                        break;
                    }
                }
            }
            #endregion </Core>

            #region <Bin>
            _inputBinSubstrates.Clear();
            _outputBinSubstrates.Clear();

            // 1) 작업 중인 것이 있는지 확인
            foreach (var item in _binSubstrates)
            {
                var proc = item.Value.ProcessingStatus;
                switch (proc)
                {
                    case ProcessingStates.Processed:
                    case ProcessingStates.Rejected:
                    case ProcessingStates.Stopped:
                    case ProcessingStates.Aborted:
                    case ProcessingStates.Skipped:
                    case ProcessingStates.Lost:
                        {
                            _outputBinSubstrates[item.Key] = item.Value;
                        }
                        break;
                    default:
                        {
                            _inputBinSubstrates[item.Key] = item.Value;
                        }
                        break;
                }

                if (item.Value.ProcessingStatus == ProcessingStates.InProcess)
                {
                    binName = item.Value.Name;
                }
            }

            // 2) 작업 중인 것이 없으면, 대기중인 것이 있는지 확인
            if (string.IsNullOrEmpty(binName))
            {
                foreach (var item in _inputBinSubstrates)
                {
                    if (item.Value.ProcessingStatus == ProcessingStates.NeedsProcessing)
                    {
                        binName = item.Value.Name;
                        break;
                    }
                }
            }
            #endregion </Bin>

            // 둘 다 있으면..
            if (false == string.IsNullOrEmpty(coreName) &&
                false == string.IsNullOrEmpty(binName))
            {
                // 출구에 자재가 없는 경우만 프로세싱 하도록
                bool processToCore = false, processToBin = false;
                _inputCoreSubstrates.TryGetValue(coreName, out var core);
                if (_outputCoreSubstrates.Count == 0)
                {
                    processToCore = true;
                }

                _inputBinSubstrates.TryGetValue(binName, out var bin);
                if (_outputBinSubstrates.Count == 0)
                {
                    processToBin = true;
                }

                if (processToCore && processToBin)
                {
                    EquipmentState = EQUIPMENT_STATE.EXECUTING;

                    if (core.ProcessingStatus != ProcessingStates.InProcess)
                    {
                        _ticksForCore.SetTickCount(30000);
                        SubstrateManager.SetProcessingStatusByKey(core.UniqueKey, ProcessingStates.InProcess);
                        SubstrateManager.SaveDataByKey(core.UniqueKey);
                    }

                    if (bin.ProcessingStatus != ProcessingStates.InProcess)
                    {
                        _ticksForBin.SetTickCount(30000);
                        SubstrateManager.SetProcessingStatusByKey(bin.UniqueKey, ProcessingStates.InProcess);
                        SubstrateManager.SaveDataByKey(bin.UniqueKey);
                    }
                    if (_ticksForCore.IsTickOver(true))
                    {
                        SubstrateManager.SetProcessingStatusByKey(core.UniqueKey, ProcessingStates.Processed);
                        //MoveSubstrateLocationToUnloadForSimul(ref core);
                        _outputCoreSubstrates[coreName] = core;
                        _inputCoreSubstrates.TryRemove(coreName, out _);
                        SubstrateManager.SaveDataByKey(core.UniqueKey);
                    }

                    if (_ticksForBin.IsTickOver(true))
                    {
                        SubstrateManager.SetAttributeByKey(bin.UniqueKey, PWA500MaterialHandling.SubstrateType, SubstrateType.Bin1.ToString());
                        SubstrateManager.SetProcessingStatusByKey(bin.UniqueKey, ProcessingStates.Processed);
                        //MoveSubstrateLocationToUnloadForSimul(ref bin);
                        _outputBinSubstrates[binName] = bin;
                        _inputBinSubstrates.TryRemove(binName, out _);
                        SubstrateManager.SaveDataByKey(bin.UniqueKey);
                    }
                }
                else
                {
                    EquipmentState = EQUIPMENT_STATE.IDLE;
                }
            }

            // === 요청 스냅샷 계산 & 원자 교체 ===
            var newLoading = new List<string>();
            var newUnloading = new List<string>();

            // 기존 규칙을 그대로 사용 (깜빡임 방지: 완성 후에만 공개)
            if (_inputCoreSubstrates.Count + _outputCoreSubstrates.Count < 3)
            {
                newLoading.Add(GetCoreEntrywayBySizeForSimulation(true));
                // 필요시 Core_8_Input 추가 복구 시 여기에 조건/추가
            }
            if (_inputBinSubstrates.Count + _outputBinSubstrates.Count < 3)
            {
                newLoading.Add(GetBinEntrywayForSimulation(true));
            }

            if (_outputCoreSubstrates.Count > 0)
            {
                newUnloading.Add(GetCoreEntrywayBySizeForSimulation(false));
            }
            if (_outputBinSubstrates.Count > 0)
            {
                newUnloading.Add(GetBinEntrywayForSimulation(false));
            }

            // 스냅샷을 한 번에 교체(외부는 항상 완성된 목록만 본다)
            lock (_reqSnapLock)
            {
                _loadingReqSnapshot = newLoading;
                _unloadingReqSnapshot = newUnloading;
            }
        }

        private string GetCoreEntrywayBySizeForSimulation(bool loading)
        {
            if (FrameOfSystem3.Task.TaskOperator.GetInstance().SimulSubstrateSizeType == 8)
            {
                if (loading)
                {
                    return Constants.ProcessModuleCore_8_InputName;
                }
                else
                {
                    return Constants.ProcessModuleCore_8_OutputName;
                }
            }
            else
            {
                if (loading)
                {
                    return Constants.ProcessModuleCore_12_InputName;
                }
                else
                {
                    return Constants.ProcessModuleCore_12_OutputName;
                }
            }
        }

        private string GetBinEntrywayForSimulation(bool loading)
        {
            if (loading)
            {
                return Constants.ProcessModuleSort_12_InputName;
            }
            else
            {
                return Constants.ProcessModuleSort_12_OutputName;
            }
        }
        //private bool MoveSubstrateLocationToUnloadForSimul(ref Substrate substrate)
        //{
        //    var subType = substrate.GetAttribute(PWA500MaterialHandling.SubstrateType);
        //    if (false == Enum.TryParse(subType, out SubstrateType substrateType))
        //        return false;

        //    string locName = null;
        //    switch (substrateType)
        //    {
        //        case SubstrateType.Core:
        //            {
        //                locName = GetCoreEntrywayBySizeForSimulation(false);
        //                //var size = substrate.GetAttribute(PWA500MaterialHandling._substratesize);
        //                //if (false == Enum.TryParse(size, out _substratesize _substratesize))
        //                //    return false;

        //                //switch (_substratesize)
        //                //{
        //                //    case _substratesize.Inch_8:
        //                //        locName = Constants.ProcessModuleCore_8_OutputName;
        //                //        break;
        //                //    case _substratesize.Inch_12:
        //                //        locName = Constants.ProcessModuleCore_12_OutputName;
        //                //        break;
        //                //    default:
        //                //        return false;
        //                //}

        //                //if (string.IsNullOrEmpty(locName))
        //                //    return false;
        //            }
        //            break;
        //        case SubstrateType.Empty:
        //        case SubstrateType.Bin1:
        //        case SubstrateType.Bin2:
        //        case SubstrateType.Bin3:
        //            {
        //                locName = GetBinEntrywayForSimulation(false);
        //                //locName = Constants.ProcessModuleSort_12_OutputName;
        //            }
        //            break;
        //        default:
        //            return false;
        //    }

        //    //if (false == ProcessModuleLocations.TryGetValue(locName, out ProcessModuleLocation loc))
        //    //    return false;

        //    //SubstrateManager.SetLocationByKey(substrate.UniqueKey, loc);
        //    //SubstrateManager.SaveDataByKey(substrate.UniqueKey);

        //    return true;
        //}
        private void UpdateSubstrateListForSimul()
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
        private void UpdateSubstratesSimul()
        {
            List<Substrate> _substrates = new List<Substrate>();
            if (SubstrateManager.Instance.GetSubstratesAtProcessModule(Name, ref _substrates))
            {
                int count = _coreSubstrates.Count + _binSubstrates.Count;
                if (count != _substrates.Count)
                {
                    // Core 순회
                    _coreSubstrates.Clear();
                    _binSubstrates.Clear();
                    foreach (var item in _substrates)
                    {
                        var name = item.Name;
                        var subTypeString = item.GetAttribute(PWA500MaterialHandling.SubstrateType);
                        Enum.TryParse(subTypeString, out SubstrateType subType);
                        switch (subType)
                        {
                            case SubstrateType.Core:
                                _coreSubstrates[name] = item;
                                break;
                            case SubstrateType.Empty:
                            case SubstrateType.Bin1:
                            case SubstrateType.Bin2:
                            case SubstrateType.Bin3:
                                _binSubstrates[name] = item;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
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
                string ringId;
                if (substrate.Extra == null ||
                    false == substrate.Extra.TryGetValue(PWA500MaterialHandling.RingId, out ringId) ||
                    string.IsNullOrWhiteSpace(ringId))
                {
                    ringId = substrate.Name;
                }
                messageToSend[PWA500MaterialHandling.SubstrateName] = substrate.Name;
                messageToSend[PWA500MaterialHandling.LotId] = substrate.LotId;
                messageToSend[PWA500MaterialHandling.RecipeId] = substrate.RecipeId;
                messageToSend[PWA500MaterialHandling.RingId] = ringId;
                messageToSend[PWA500MaterialHandling.PortId] = substrate.SourcePortId.ToString();
                messageToSend[PWA500MaterialHandling.SlotId] = substrate.SourceSlot.ToString();

                switch (substrate.SourcePortId)
                {
                    case 1:
                        messageToSend[PWA500MaterialHandling.SubstrateType] = "Empty";
                        break;
                    case 2:
                    case 3:
                    case 4:
                        messageToSend[PWA500MaterialHandling.SubstrateType] = "Core";
                        break;
                    default:
                        break;
                }

            }
        }
        // TODO : 추후 Name 핸들링을 Key로 변경필요
        private Dictionary<string, string> MakeMessagesBySubstrateName(string entry, string title, string substrateName)
        {
            Dictionary<string, string> messageToSend = new Dictionary<string, string>();

            Substrate substrate;
            if (title == RequestMessages.RequestApproachUnloading.ToString())
            {
                // 알 수도 모를 수도 있음
                FrameOfSystem3.SECSGEM.FunctionsForPWA500W_NRD.Instance.GetSubstrateAtProcessModuleByName(substrateName, out substrate);
            }
            else if (title == RequestMessages.RequestActionUnloading.ToString())
            {
                // 알아야함(ApproachUnloading 시점에 자재 Key 가 갱신됨)
                if (false == FrameOfSystem3.SECSGEM.FunctionsForPWA500W_NRD.Instance.GetSubstrateAtProcessModuleByName(substrateName, out substrate) ||
                    substrate == null)
                    return messageToSend;
            }
            else
            {
                if (false == FrameOfSystem3.SECSGEM.FunctionsForPWA500W_NRD.Instance.GetSubstrateByName(substrateName, out substrate) ||
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

            var structure = MakeMessagesBySubstrateName(entry, title, substrateName);
            if (structure == null)
                return false;

            return _communicator.SendMessage(PortIdsByEntry[entry], title, structure);
        }
        #endregion </WCF>

        #endregion </Internals>

        #endregion </Methods>
    }
}