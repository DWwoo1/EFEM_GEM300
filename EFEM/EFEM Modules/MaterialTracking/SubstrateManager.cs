using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using FrameOfSystem3.Functional;

using EFEM.Defines.LoadPort;
using EFEM.Defines.AtmRobot;
using EFEM.Defines.ProcessTypeProvider;
using EFEM.Defines.MaterialTracking;
using EFEM.Modules;
using EFEM.Defines.Common;
using EFEM.MaterialTracking.SubstrateStorage;
using EFEM.Jobs.Manager;

namespace EFEM.MaterialTracking
{
    public interface ISubstrateByCarrier
    {
        IReadOnlyList<string> GetSubstrateKeysAtLoadPort(int portId);
    }
    public sealed class SubstrateManager : ISubstrateByCarrier, ISubstrateServiceCallback
    {
        #region <Constructors>
        private SubstrateManager(
            ISubstrateStorage storage,
            List<ISubstrateEventObserver> observers,
            IMaterialExtraAttribute profile, 
            IProcessTypeProvider provider,
            FrameOfSystem3.SECSGEM.IGem300ScenarioService gem300Service)
        {
            _profile = profile;
            _storage = storage;

            _substratesAtLoadPortSlots = new ConcurrentDictionary<int, ConcurrentDictionary<int, string>>();
            _substratesAtProcessModule = new ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>();
            _substratesAtRobot = new ConcurrentDictionary<string, ConcurrentDictionary<RobotArmTypes, string>>();
            _gem300Service = gem300Service;

            foreach (var item in observers)
            {
                _storage.RegisterCallbackListner(item);
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private ISubstrateStorage _storage;
        //private ISubstrateProcessProfileFactory _factory;
        private readonly IMaterialExtraAttribute _profile;
        public event Action<string> SubstrateRecovered;
        public event Func<SubstrateProcessingStateChangedEvent, bool> SubstrateProcessingStateChanged;
        public event Func<SubstrateLocationStateChangedEvent, Task<bool>> SubstrateLocationStateChanged;
        public event Func<SubstrateLocationChangedEvent, Task<bool>> SubstrateLocationChanged;
        public event Func<SubstrateSwappedEvent, Task<bool>> SubstrateSwapped;
        
        private static readonly object _gate = new object();

        private static SubstrateManager _instance = null;

        private readonly ConcurrentDictionary<string, Substrate> _substratesByKey = new ConcurrentDictionary<string, Substrate>();

        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, string>> _substratesAtLoadPortSlots = null;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _substratesAtProcessModule = null;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<RobotArmTypes, string>> _substratesAtRobot = null;

        private readonly FrameOfSystem3.SECSGEM.IGem300ScenarioService _gem300Service;
        #endregion </Fields>

        #region <Properties>
        public static SubstrateManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <File Control>
        public static void Configure(
            ISubstrateStorage storage,
            List<ISubstrateEventObserver> observers, 
            IMaterialExtraAttribute profile, 
            IProcessTypeProvider provider,
            FrameOfSystem3.SECSGEM.IGem300ScenarioService gem300Service
            )
        {
            if (profile == null || provider == null)
                throw new ArgumentNullException("deps");

            lock (_gate)
            {
                if (_instance == null)
                {
                    _instance = new SubstrateManager(storage, observers, profile, provider, gem300Service);
                }
            }
        }
        public bool LoadRecoveryDataAll()
        {
            try
            {
                if (false == Directory.Exists(RecoveryFileDefines.RecoveryFilePath))
                {
                    Directory.CreateDirectory(RecoveryFileDefines.RecoveryFilePath);
                    return false;
                }

                _storage.InitializeStorage();

                if (false == _storage.LoadDataFromStorage(out var data))
                {
                    return false;
                }

                for (int i = 0; i < data.Count; ++i)
                {
                    var dto = data[i];

                    // 2) 맵퍼를 통해 DTO -> 객체로 변환
                    Substrate substrate = SubstrateMapper.ToDomain(dto);

                    // 3) 객체를 위치 캐시에 등록
                    RegisterSubstrate(substrate).GetAwaiter().GetResult();

                    RaiseSubstrateRecovered(substrate.UniqueKey);
                }
                //string[] files = Directory.GetFiles(RecoveryFileDefines.RecoveryFilePath);
                //if (files.Length <= 0)
                //    return false;

                //for (int i = 0; i < files.Length; ++i)
                //{
                //    string fileName = Path.GetFileNameWithoutExtension(files[i]);

                //    // 1) 저장소에서 전송용 Data(DTO) 형태로 읽어옴
                //    var dto = _storage.GetByKeyAsync(fileName).GetAwaiter().GetResult();
                //    if (dto == null)
                //        continue;

                //    // 2) 맵퍼를 통해 DTO -> 객체로 변환
                //    Substrate substrate = SubstrateMapper.ToDomain(dto);
                    
                //    // 3) 객체를 위치 캐시에 등록
                //    RegisterSubstrate(substrate);
                //}
            }
            catch (Exception ex)
            {
                DebugLogger.Instance.WriteDebugLog(string.Format("LoadRecoveryDataAll Exception > {0}, {1}",
                    ex.Message, ex.StackTrace));

                return false;
            }
            return true;
        }
        public async Task<bool> SaveDataByKeys(IEnumerable<string> keys)
        {
            foreach (var item in keys)
            {
                if (false == _substratesByKey.TryGetValue(item, out var s) ||
                    s == null)
                    continue;

                var dto = SubstrateMapper.ToData(s);
                await _storage.UpsertAsync(dto).ConfigureAwait(false);
            }

            return true;
        }
        public async Task<bool> SaveDataByKey(string key)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) ||
                s == null)
                return false;

            var dto = SubstrateMapper.ToData(s);
            await _storage.UpsertAsync(dto).ConfigureAwait(false);
            
            //s.SaveRecoveryData();

            return true;
        }
        //public bool SaveRecoveryDataAll()
        //{
        //    try
        //    {
        //        if (false == Directory.Exists(RecoveryFileDefines.RecoveryFilePath))
        //        {
        //            Directory.CreateDirectory(RecoveryFileDefines.RecoveryFilePath);
        //            return false;
        //        }

        //        foreach (var lp in _substratesAtLoadPortSlots)
        //        {
        //            foreach (var item in lp.Value)
        //            {
        //                if (false == SaveDataByKey(item.Value))
        //                {
        //                    DebugLogger.Instance.WriteDebugLog(string.Format("SaveRecoveryData failed > {0}", item.Key));
        //                }
        //            }
        //        }

        //        foreach (var pm in _substratesAtProcessModule)
        //        {
        //            foreach (var item in pm.Value)
        //            {
        //                if (false == SaveDataByKey(item.Key))
        //                {
        //                    DebugLogger.Instance.WriteDebugLog(string.Format("SaveRecoveryData failed > {0}", item.Key));
        //                }
        //            }
        //            //for (int i = 0; i < pm.Value.Count; ++i)
        //            //{
        //            //    if (false == pm.Value[i].SaveRecoveryData())
        //            //    {
        //            //        DebugLogger.Instance.WriteDebugLog(string.Format("SaveRecoveryData failed > {0}", pm.Key));
        //            //    }
        //            //}
        //        }

        //        foreach (var robot in _substratesAtRobot)
        //        {
        //            foreach (var item in robot.Value)
        //            {
        //                if (false == SaveDataByKey(item.Value))
        //                {
        //                    DebugLogger.Instance.WriteDebugLog(string.Format("SaveRecoveryData failed > {0}", item.Key));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DebugLogger.Instance.WriteDebugLog(string.Format("SaveRecoveryDataAll Exception > {0}, {1}",
        //            ex.Message, ex.StackTrace));

        //        return false;
        //    }

        //    return true;
        //}
        public bool IsValidSubstrateName(string filename)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return !filename.Any(c => invalidChars.Contains(c));
        }
        #endregion <File Control>

        #region <Create, Remove>
        public async void CreateSubstrate(string key, string name, Location location)
        {
            var substrate = new Substrate(key, name);

            if (location is LoadPortLocation)
            {
                var lpLocation = location as LoadPortLocation;

                substrate.SourcePortId = lpLocation.PortId;
                substrate.SourceSlot = lpLocation.Slot;
                substrate.DestinationPortId = lpLocation.PortId;
                substrate.DestinationSlot = lpLocation.Slot;

                AssignSubstrateAtLoadPort(lpLocation.PortId, lpLocation.Slot, substrate);
                //
            }
            else if (location is ProcessModuleLocation)
            {
                var pmLocation = location as ProcessModuleLocation;

                AssignSubstrateAtProcessModule(pmLocation.ProcessModuleName, substrate);
            }
            else if (location is RobotLocation)
            {
                var robotLocation = location as RobotLocation;

                AssignSubstrateAtRobot(robotLocation.RobotName, robotLocation.Arm, substrate);
            }

            var ev = new SubstrateLocationStateChangedEvent(
                key,
                location.Id,
                OccupancyState.Occupied,
                OccupancyChangeReason.Created
            );

            SetLocationIdByKey(key, location.Id);

            //MaterialHistoryManager.LocationStateService.Occupy(location, key, OccupancyChangeReason.Created);

            //var profile = _factory.Create(_provider.GetProcessType());
            var extra = new Dictionary<string, string>();
            _profile.CreateAttributes(extra);
            _profile.InitializeToPublish(extra, substrate);
            foreach (var item in extra)
            {
                substrate.SetAttribute(item.Key, item.Value);
            }
            
            await SaveDataByKey(substrate.UniqueKey).ConfigureAwait(false);
            //SetLocationIdByKey(substrate.UniqueKey, location.Name);

            await RaiseSubstrateLocationStateChanged(ev).ConfigureAwait(false);
        }
        public void CreateSubstrateAtDriver(string key)
        {
            bool hasKey = _substratesByKey.TryGetValue(key, out var s) && s != null;
            if (false == hasKey)
                return;

            if (_gem300Service != null)
            {
                var locName = LocationServer.FindNameById(s.LocationId);
                _gem300Service.Substrate.SetInfo(locName, s.Name, s.TransportStatus, s.ProcessingStatus, s.IdReadingStatus);
            }
        }
        public async void RemoveSubstrateByKey(string key, bool includeCache = true)
        {
            bool hasKey = _substratesByKey.TryGetValue(key, out var s) && s != null;

            var locId = s.LocationId;
            if (false == LocationServer.FindLocationById(locId, out var loc))
                return;

            switch (loc)
            {
                case LoadPortLocation lp:
                    {
                        if (_substratesAtLoadPortSlots.TryGetValue(lp.PortId, out var maps) && maps != null)
                        {
                            maps.TryRemove(lp.Slot, out _);
                        }
                    }
                    break;

                case ProcessModuleLocation pm:
                    {
                        if (_substratesAtProcessModule.TryGetValue(pm.ProcessModuleName, out var maps) && maps != null)
                        {
                            maps.TryRemove(key, out _);
                        }
                    }
                    break;

                case RobotLocation rb:
                    {
                        if (_substratesAtRobot.TryGetValue(rb.RobotName, out var maps) && maps != null)
                        {
                            maps.TryRemove(rb.Arm, out _);
                        }
                    }
                    break;

                default:
                    return;
            }

            if (includeCache && hasKey)
            {
                var ev = new SubstrateLocationStateChangedEvent(
                    key,
                    locId,
                    OccupancyState.Unoccupied,
                    OccupancyChangeReason.Removed);

                await RaiseSubstrateLocationStateChanged(ev);
                //MaterialHistoryManager.LocationStateService.Vacate(loc, key, OccupancyChangeReason.Removed);

                RemoveSubstrateKey(key);

                if (_gem300Service != null)
                {
                    // 자재 제거 처리
                    _gem300Service.Substrate.SetProcessing(s.LocationId, s.Name, ProcessingStates.Lost);

                    // 객체 제거
                    _gem300Service.Substrate.Remove(s.Name);

                    if (Jobs.Binding.SubstrateJobBindingService.Instance != null)
                    {
                        Jobs.Binding.SubstrateJobBindingService.Instance.RemoveBindingTarget(
                            s.ProcessJobId,
                            s.SourceCarrierId,
                            s.SourceSlot,
                            "Lost");
                    }
                }
            }
        }
        //
        #endregion </Create, Remove>

        #region <ISubstrateServiceCallback>
        public void OnCreated(SubstrateCreatedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnDeleted(SubstrateDeletedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnTransportChanged(SubstrateTransportStateChangedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnProcessingChanged(SubstrateProcessingStateChangedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnReadingChanged(SubstrateReadingStateChangedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnCreateRequestedByHost(SubstrateCreateRequestedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnUpdateRequestedByHost(SubstrateUpdateRequestedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnDeleteRequestedByHost(SubstrateDeleteRequestedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnCancelRequestedByHost(SubstrateCancelRequestedEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnConfirmationDisplayed(SubstrateConfirmEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnConfirmationSucceeded(SubstrateConfirmEventArgs e)
        {
            // TODO : 구현 필요
        }

        public void OnConfirmationFailed(SubstrateConfirmFailedEventArgs e)
        {
            // TODO : 구현 필요
        }
        #endregion </ISubstrateServiceCallback>

        #region <Assign>
        public void AddLoadPortBuffers(int portId, int capacity)
        {
            _substratesAtLoadPortSlots[portId] = new ConcurrentDictionary<int, string>();
        }
        public void AddRobotBuffers(string robotName)
        {
            _substratesAtRobot[robotName] = new ConcurrentDictionary<RobotArmTypes, string>();
        }
        public void AddProcessModuleBuffers(string processModuleName)
        {
            _substratesAtProcessModule[processModuleName] = new ConcurrentDictionary<string, byte>();
        }

        public void AssignSubstrateAtLoadPort(int portId, int slot, Substrate substrate)
        {
            var key = substrate.UniqueKey;

            var inner = _substratesAtLoadPortSlots.GetOrAdd(
                portId, _ => new ConcurrentDictionary<int, string>());

            inner[slot] = key;

            AddSubstrateKey(key, substrate);
        }
        public async Task WriteHistoryBeforeRemoving(int portId, string destinationPath)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out ConcurrentDictionary<int, string> maps))
                return;

            foreach (var item in maps)
            {
                if (false == _substratesByKey.TryGetValue(item.Value, out var s) || s == null)
                    continue;

                if (false == _storage.IsExists(s.UniqueKey))
                    continue;

                var ev = new SubstrateLocationStateChangedEvent(
                    s.UniqueKey,
                    s.LocationId,
                    OccupancyState.Unoccupied,
                    OccupancyChangeReason.Removed);

                await RaiseSubstrateLocationStateChanged(ev).ConfigureAwait(false);
            }
        }
        public async void BackupAndRemoveSubstrateAtLoadPortAll(int portId, string destinationPath)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out ConcurrentDictionary<int, string> maps))
                return;

            //if (false == string.IsNullOrWhiteSpace(destinationPath) && false == Directory.Exists(destinationPath))
            //{
            //    Directory.CreateDirectory(destinationPath);
            //}

            foreach (var item in maps)
            {
                if (false == _substratesByKey.TryGetValue(item.Value, out var s) || s == null)
                    continue;

                // 2026.06.05. dwlim [MOD] CONTINUE 에 의해 이전 정보가 지워지지 않음
                _storage.IsExists(s.UniqueKey);
                //if (false == _storage.IsExists(s.UniqueKey))
                //{
                //    Console.WriteLine($"Not removed : {s.UniqueKey}");
                //    continue;
                //}

                //if (string.IsNullOrWhiteSpace(destinationPath))
                //    continue;

                var ev = new SubstrateLocationStateChangedEvent(
                    s.UniqueKey,
                    s.LocationId,
                    OccupancyState.Unoccupied,
                    OccupancyChangeReason.Removed);

                await RaiseSubstrateLocationStateChanged(ev);

                //_storage.ArchiveAsync(s.UniqueKey, destinationPath);

                // 아카이브에서 지워졌을 것이니 여기서 또 지울 필요가 없다.
                RemoveSubstrateKey(item.Value, false);

                // Console.WriteLine($"Removed : {s.UniqueKey}");
            }

            _substratesAtLoadPortSlots[portId].Clear();
        }

        // 시뮬에서만 사용되고 있음
        public void RemoveSubstrateAtLoadPortAll(int portId)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out var maps))
                return;

            foreach (var item in maps)
            {
                if (false == _substratesByKey.TryGetValue(item.Value, out var s) || s == null)
                    continue;

                //s.DeleteRecoveryData();

                RemoveSubstrateKey(item.Value);
            }

            _substratesAtLoadPortSlots[portId].Clear();
        }
        public void AssignSubstrateAtProcessModule(string moduleName, Substrate substrate)
        {
            var key = substrate.UniqueKey;
            var inner = _substratesAtProcessModule.GetOrAdd(moduleName, _ => new ConcurrentDictionary<string, byte>());
            inner[key] = 0x00;

            AddSubstrateKey(key, substrate);
        }
        // 시뮬에서만 사용되고 있음
        public void RemoveSubstratesAtProcessModule(string moduleName)
        {
            if (_substratesAtProcessModule.ContainsKey(moduleName))
            {
                foreach (var item in _substratesAtProcessModule[moduleName])
                {
                    if (false == _substratesByKey.TryGetValue(item.Key, out var s) || s == null)
                        continue;

                    //s.DeleteRecoveryData();

                    RemoveSubstrateKey(item.Key);
                }

                _substratesAtProcessModule[moduleName].Clear();
            }
        }
        public void AssignSubstrateAtRobot(string robotName, RobotArmTypes armType, Substrate substrate)
        {
            var key = substrate.UniqueKey;
            if (_substratesAtRobot.ContainsKey(robotName))
            {
                _substratesAtRobot[robotName][armType] = key;
            }

            AddSubstrateKey(key, substrate);
        }
        // 시뮬/드라이런에서만 사용되고 있음
        public void RemoveSubstratesAtRobot(string robotName)
        {
            if (_substratesAtRobot.ContainsKey(robotName))
            {
                foreach (var item in _substratesAtRobot[robotName])
                {
                    if (false == _substratesByKey.TryGetValue(item.Value, out var s) || s == null)
                        continue;

                    //s.DeleteRecoveryData();

                    RemoveSubstrateKey(item.Value);
                }

                _substratesAtRobot[robotName].Clear();
            }
        }
        //
        #endregion </Assign>

        #region <Attributes>
        // 수정함
        public bool GetTransferStatusAtLoadPort(int portId, int slot, ref TransportStates status)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out var slots) || slots == null ||
                false == slots.TryGetValue(slot, out var key) || string.IsNullOrWhiteSpace(key))
                return false;

            if (_substratesByKey.TryGetValue(key, out var s) && s != null)
            {
                status = s.TransportStatus;
                return true;
            }

            return false;
        }
        public bool GetProcessingStatusAtLoadPort(int portId, int slot, ref ProcessingStates status)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out var slots) || slots == null ||
                false == slots.TryGetValue(slot, out var key) || string.IsNullOrWhiteSpace(key))
                return false;

            if (_substratesByKey.TryGetValue(key, out var s) && s != null)
            {
                status = s.ProcessingStatus;
                return true;
            }

            return false;
        }
        public bool TryGetTransportStatusByProcessingStatus(string key, out TransportStates state)
        {
            state = TransportStates.AtSource;
            if (false == _substratesByKey.TryGetValue(key, out var s))
                return false;

            // TODO : Cycle 모드는 임시제거
            //if (FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.COMMON, FrameOfSystem3.Recipe.PARAM_COMMON.UseCycleMode.ToString(), false))
            //{
            //    substrate.SetTransferStatus(SubstrateTransferStates.AtSource);
            //    substrate.SetProcessingStatus(ProcessingStates.NeedsProcessing);
            //}
            //else
            {
                var processingStatus = s.ProcessingStatus;
                if (processingStatus == ProcessingStates.NeedsProcessing)
                {
                    state = TransportStates.AtSource;
                    //SetTransferStatusByKey(key, TransportStates.AtSource);
                    //s.TransportStatus = SubstrateTransferStates.AtSource;
                }
                else
                {
                    state = TransportStates.AtDestination;
                    //SetTransferStatusByKey(key, TransportStates.AtDestination);
                    //s.TransportStatus = SubstrateTransferStates.AtDestination;
                }
            }
            
            return true;
        }
        public string GetAttributeByKey(string key, string attrKey)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return string.Empty;

            return s.GetAttribute(attrKey);
        }

        #region <Attribute Setters>
        public bool SetNameByKey(string key, string name)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.Name = name;
            return true;
        }
        public bool SetCurrentCarrierKeyByKey(string key, string carrierKey)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.CurrentCarrierKey = carrierKey;

            return true;
        }
        public bool SetSourcePortIdByKey(string key, int portId)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.SourcePortId = portId;
            return true;
        }
        public bool SetSourceSlotByKey(string key, int slot)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.SourceSlot = slot;
            return true;
        }
        public bool SetDestinationPortIdByKey(string key, int portId)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.DestinationPortId = portId;
            return true;
        }
        public bool SetDestinationSlotByKey(string key, int slot)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.DestinationSlot = slot;
            return true;
        }
        public bool SetLocationIdByKey(string key, string locationId)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.LocationId = locationId;
            return true;
        }
        public bool SetTransferStatusByKey(string key, TransportStates state)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            if (s.TransportStatus == state)
            {
                return true;
            }

            s.TransportStatus = state;

            if (_gem300Service != null)
            { 
                var name = LocationServer.FindNameById(s.LocationId);
                if (string.IsNullOrWhiteSpace(name))
                    name = s.LocationId;

                _gem300Service.Substrate.SetTransport(name, s.Name, state);
            }

            return true;
        }
        public async Task<bool> SetProcessingStatusByKey(string key, ProcessingStates state)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            if (s.ProcessingStatus == state)
                return true;

            var ev = new SubstrateProcessingStateChangedEvent(
                key,
                s.ProcessingStatus,
                state,
                s.LocationId,
                s.ControlJobId,
                s.ProcessJobId,
                string.Empty);

            RaiseSubstrateProcessingStateChanged(ev);

            if (_gem300Service != null)
            {
                var name = LocationServer.FindNameById(s.LocationId);
                if (string.IsNullOrWhiteSpace(name))
                    name = s.LocationId;

                _gem300Service.Substrate.SetProcessing(name, s.Name, state);
            }

            //MaterialHistoryManager.SubstrateProcessingService.ChangeState(
            //    key,
            //    s.ProcessingStatus,
            //    state,
            //    s.LocationId,
            //    s.ControlJobId,
            //    s.ProcessJobId);

            s.ProcessingStatus = state;
            
            return await SaveDataByKey(key);
        }
        public bool SetIdReadingStatusByKey(string key, IdReadingStates state)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.IdReadingStatus = state;
            return true;
        }
        public bool SetLotIdByKey(string key, string lotId)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.LotId = lotId;
            return true;
        }
        public bool SetSourceCarrierIdByKey(string key, string carrierId)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.SourceCarrierId = carrierId;
            return true;
        }
        public bool SetRecipeIdByKey(string key, string recipeId)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.RecipeId = recipeId;
            return true;
        }
        public bool SetProcessJobIdByKey(string key, string value)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.ProcessJobId = value;
            return true;
        }
        public bool SetControlJobIdByKey(string key, string value)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.ControlJobId = value;
            return true;
        }
        public bool SetIdReadingStateByKey(string key, IdReadingStates value)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.IdReadingStatus = value;
            return true;
        }
        public bool SetDoNotProcessFlagByKey(string key, bool value)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.DoNotProcessFlag = value;
            return true;
        }
        public bool SetUsageByKey(string key, bool value)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.Usage = value;
            return true;
        }
        public bool SetAttributeByKey(string key, string attrKey, string value)
        {
            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            s.SetAttribute(attrKey, value);

            return true;
        }
        public async Task<bool> SetJobBindingInfoByKey(
            string key,
            string controlJobId,
            string processJobId,
            string recipeId)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            // ProcessJobId는 바인딩의 핵심이다.
            // 빈 값도 허용한다.
            // 이유:
            // Unbind 시 string.Empty로 지워야 하기 때문이다.
            s.ProcessJobId = processJobId ?? string.Empty;

            // ControlJob은 ProcessJob보다 나중에 생성될 수 있다.
            // 따라서 null/empty를 허용한다.
            s.ControlJobId = controlJobId ?? string.Empty;

            // RecipeId도 PRJob 생성 시점에는 존재하지만,
            // 일부 장비 정책에서는 비어 있을 수 있으므로 null 보호만 한다.
            s.RecipeId = recipeId ?? string.Empty;

            // Job 관련 정보는 Recovery 대상이어야 한다.
            // 장비 재기동 후에도 해당 Substrate가 어떤 Job에 속했는지 복구되어야
            // ProcessingStateChanged 이벤트에서 Job 추적이 가능하다.
            return await SaveDataByKey(key).ConfigureAwait(false);
        }
        public async Task<bool> ClearJobBindingInfoByKey(
            string key,
            string expectedControlJobId,
            string expectedProcessJobId,
            bool clearRecipeId)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return false;

            if (!string.IsNullOrWhiteSpace(expectedProcessJobId) &&
                !string.Equals(s.ProcessJobId, expectedProcessJobId, StringComparison.Ordinal))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(expectedControlJobId) &&
                !string.Equals(s.ControlJobId, expectedControlJobId, StringComparison.Ordinal))
            {
                return false;
            }

            s.ProcessJobId = string.Empty;
            s.ControlJobId = string.Empty;

            if (clearRecipeId)
                s.RecipeId = string.Empty;

            return await SaveDataByKey(key).ConfigureAwait(false);
        }
        #endregion </Attribute Setters>

        #endregion </Attributes>

        #region <Location>
        private void RaiseSubstrateRecovered(string substrateKey)
        {
            if (SubstrateRecovered != null)
                SubstrateRecovered(substrateKey);
        }
        private void RaiseSubstrateProcessingStateChanged(SubstrateProcessingStateChangedEvent e)
        {
            if (SubstrateProcessingStateChanged != null)
                SubstrateProcessingStateChanged(e);
        }
        private async Task<bool> RaiseSubstrateLocationStateChanged(SubstrateLocationStateChangedEvent e)
        {
            if (SubstrateLocationStateChanged != null)
                return await SubstrateLocationStateChanged(e).ConfigureAwait(false);

            return false;
        }
        private async Task<bool> RaiseSubstrateLocationChanged(SubstrateLocationChangedEvent e)
        {
            if (SubstrateLocationChanged != null)
                return await SubstrateLocationChanged(e).ConfigureAwait(false);

            return false;
        }
        private async Task<bool> RaiseSubstrateSwapped(SubstrateSwappedEvent e)
        {
            if (SubstrateSwapped != null)
                return await SubstrateSwapped(e).ConfigureAwait(false);

            return false;
        }

        // 수정함
        public async Task<bool> TransferSubstrate(
            string key, 
            Location source, 
            Location destination,
            TransportStates state,
            OccupancyChangeReason vacationReason, 
            OccupancyChangeReason occupyReason)
        {
            if (false == LocationServer.FindLocationById(source.Id, out _) ||
                false == LocationServer.FindLocationById(destination.Id, out _) ||
                string.IsNullOrWhiteSpace(key))
                return false;

            if (false == _substratesByKey.TryGetValue(key, out var s) ||
                s == null)
                return false;

            // Source 에서 삭제(캐시는 지우면 안 됨)
            RemoveSubstrateByKey(s.UniqueKey, false);

            SetLocationIdByKey(s.UniqueKey, destination.Id);

            // Target 메모리에 할당
            if (destination is LoadPortLocation)
            {
                var location = destination as LoadPortLocation;
                AssignSubstrateAtLoadPort(location.PortId, location.Slot, s);
            }
            else if (destination is ProcessModuleLocation)
            {
                var location = destination as ProcessModuleLocation;
                AssignSubstrateAtProcessModule(location.ProcessModuleName, s);
            }
            else if (destination is RobotLocation)
            {
                var location = destination as RobotLocation;
                AssignSubstrateAtRobot(location.RobotName, location.Arm, s);
            }

            s.TransportStatus = state;
            SaveDataByKey(s.UniqueKey);

            if (_gem300Service != null)
            {
                var name = destination.Name;
                if (string.IsNullOrWhiteSpace(name))
                    name = destination.Id;

                _gem300Service.Substrate.SetTransport(name, s.Name, state);
            }

            // 2) Location History 이벤트로 알림
            var ev = new SubstrateLocationChangedEvent(
                s.UniqueKey,
                source.Id,
                destination.Id,
                vacationReason,
                occupyReason
            );

            return await RaiseSubstrateLocationChanged(ev).ConfigureAwait(false);
            //return MaterialHistoryManager.LocationStateService.TransferByName(source.Name, destination.Name, key, vacationReason, occupyReason);
        }
        //
        #endregion </Location>

        #region <Get Substrate>
        // 수정함
        public bool GetSubstrateByKey(string key, out Substrate s)
        {
            s = null;
            if (string.IsNullOrWhiteSpace(key))
                return false;

            return _substratesByKey.TryGetValue(key, out s);
        }
        public bool GetSubstrateByLocationAndKey(string locationId, ModuleType moduleType, string key, out Substrate s)
        {
            s = null;

            if (moduleType != ModuleType.ProcessModule)
            {
                if (false == LocationServer.FindLocationById(locationId, out var loc) || loc == null)
                    return false;

                if (loc is LoadPortLocation)
                {
                    var lp = loc as LoadPortLocation;

                    if (_substratesAtLoadPortSlots[lp.PortId].TryGetValue(lp.Slot, out var inKey) && false == string.IsNullOrWhiteSpace(inKey))
                    {
                        return _substratesByKey.TryGetValue(inKey, out s) && s != null;
                    }
                }
                //else if (loc is ProcessModuleLocation)
                //{
                //    var pm = loc as ProcessModuleLocation;

                //    if (_substratesAtProcessModule.TryGetValue(pm.ProcessModuleName, out var maps) && maps != null)
                //    {
                //        if (maps.TryGetValue(key, out _))
                //        {
                //            return _substratesByKey.TryGetValue(key, out s) && key != null;
                //        }
                //    }
                //}
                else if (loc is RobotLocation)
                {
                    var rb = loc as RobotLocation;

                    if (_substratesAtRobot[rb.RobotName].TryGetValue(rb.Arm, out var inKey) && false == string.IsNullOrWhiteSpace(inKey))
                    {
                        return _substratesByKey.TryGetValue(inKey, out s) && s != null;
                    }
                }
            }
            else
            {
                foreach (var pmc in _substratesAtProcessModule)
                {
                    if (pmc.Value.TryGetValue(key, out _))
                    {
                        return _substratesByKey.TryGetValue(key, out s) && key != null;
                    }
                }
            }

            return false;
        }
        public bool GetSubstrateByLocationAndKey(Location loc, string key, out Substrate s)
        {
            s = null;

            if (loc is LoadPortLocation)
            {
                var lp = loc as LoadPortLocation;

                if (_substratesAtLoadPortSlots[lp.PortId].TryGetValue(lp.Slot, out var inKey) && false == string.IsNullOrWhiteSpace(inKey))
                {
                    return _substratesByKey.TryGetValue(inKey, out s) && s != null;
                }
            }
            else if (loc is ProcessModuleLocation)
            {
                var pm = loc as ProcessModuleLocation;

                if (_substratesAtProcessModule.TryGetValue(pm.ProcessModuleName, out var maps) && maps != null)
                {
                    if (maps.TryGetValue(key, out _))
                    {
                        return _substratesByKey.TryGetValue(key, out s) && key != null;
                    }
                }
            }
            else if (loc is RobotLocation)
            {
                var rb = loc as RobotLocation;

                if (_substratesAtRobot[rb.RobotName].TryGetValue(rb.Arm, out var inKey) && false == string.IsNullOrWhiteSpace(inKey))
                {
                    return _substratesByKey.TryGetValue(inKey, out s) && s != null;
                }
            }

            return false;
        }
        public bool GetSubstratesAll(ref List<Substrate> substrates)
        {
            substrates.Clear();
            foreach (var item in _substratesByKey)
            {
                substrates.Add(item.Value);
            }

            //foreach (var lp in SubstratesAtLoadPortSlots)
            //{
            //    substrates.AddRange(lp.Value.Values);
            //    //foreach (var item in lp.Value)
            //    //{
            //    //    substrates.Add(item.Value);
            //    //}
            //}

            //foreach (var item in SubstratesAtProcessModule)
            //{
            //    substrates.AddRange(item.Value.Values);
            //    //for (int i = 0; i < item.Value.Count; ++i)
            //    //{
            //    //    substrates.Add(item.Value[i]);
            //    //}
            //}

            //foreach (var arms in SubstratesAtRobot)
            //{
            //    substrates.AddRange(arms.Value.Values);
            //    //foreach (var item in arms.Value)
            //    //{
            //    //    substrates.Add(item.Value);
            //    //}
            //}

            return substrates.Count > 0;
        }
        public IReadOnlyList<Substrate> GetSubstratesByJobInfo(
            int portId,
            int slot,
            string carrierId,
            string processJobId)
        {
            List<Substrate> substrates = new List<Substrate>();

            if (slot <= 0)
                return substrates;

            if (string.IsNullOrWhiteSpace(carrierId))
                return substrates;

            if (string.IsNullOrWhiteSpace(processJobId))
                return substrates;

            foreach (var item in _substratesByKey)
            {
                Substrate substrate = item.Value;

                if (substrate == null)
                    continue;

                /*
                 * portId가 0 이하이면 현재 Port를 모르는 상태로 본다.
                 * 이 경우 SourceCarrierId + SourceSlot + ProcessJobId로 역조회한다.
                 */
                bool portMatched =
                    portId <= 0 || substrate.SourcePortId == portId;

                if (false == portMatched)
                    continue;

                if (substrate.SourceSlot != slot)
                    continue;

                if (false == string.Equals(
                    substrate.SourceCarrierId,
                    carrierId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (false == string.Equals(
                    substrate.ProcessJobId,
                    processJobId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                substrates.Add(substrate);
            }

            return substrates;
        }
        public IReadOnlyList<Substrate> GetSubstratesByProcessJobId(
            string processJobId)
        {
            List<Substrate> substrates = new List<Substrate>();

            if (string.IsNullOrWhiteSpace(processJobId))
                return substrates;

            foreach (var item in _substratesByKey)
            {
                Substrate substrate = item.Value;

                if (substrate == null)
                    continue;

                if (!string.Equals(
                    substrate.ProcessJobId,
                    processJobId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                substrates.Add(substrate);
            }

            return substrates;
        }

        public IReadOnlyList<Substrate> GetSubstratesByProcessJobAndCarrier(
            string processJobId,
            string carrierId)
        {
            List<Substrate> substrates = new List<Substrate>();

            if (string.IsNullOrWhiteSpace(processJobId))
                return substrates;

            if (string.IsNullOrWhiteSpace(carrierId))
                return substrates;

            foreach (var item in _substratesByKey)
            {
                Substrate substrate = item.Value;

                if (substrate == null)
                    continue;

                if (!string.Equals(
                    substrate.ProcessJobId,
                    processJobId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!string.Equals(
                    substrate.SourceCarrierId,
                    carrierId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                substrates.Add(substrate);
            }

            return substrates;
        }

        #region <LoadPort>
        // 수정함
        public IReadOnlyList<string> GetSubstrateKeysAtLoadPort(int portId)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out var k) ||
                k == null)
                return null;

            var keys = new List<string>(k.Values.ToList());

            return keys;
        }

        public string GetSubstrateKeyAtLoadPort(int portId, int slot)
        {
            // LoadPort 번호는 1 이상만 허용한다.
            // Slot도 장비 기준으로 보통 1-base이므로 0 이하는 잘못된 값으로 본다.
            //
            // 기존 코드는 slot < 0 이었지만,
            // Binder 쪽에서는 slot <= 0을 무효로 처리하므로 여기서도 맞춰주는 편이 안전하다.
            if (portId <= 0 || slot <= 0)
                return string.Empty;

            // 중요:
            // 기존 코드는 _substratesAtLoadPortSlots[portId]로 직접 접근했다.
            // portId가 아직 AddLoadPortBuffers() 또는 AssignSubstrateAtLoadPort()로 등록되지 않았다면
            // KeyNotFoundException이 발생할 수 있다.
            //
            // Binder는 Job 생성 시점, SlotMap 검증 전 시점에도 호출될 수 있으므로
            // "없음"은 예외가 아니라 정상 Pending 상태다.
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out var slots) || slots == null)
                return string.Empty;

            if (false == slots.TryGetValue(slot, out var key) || string.IsNullOrWhiteSpace(key))
                return string.Empty;

            return key;
        }
        public Dictionary<int, Substrate> GetSubstratesAtLoadPort(int portId)
        {
            if (false == _substratesAtLoadPortSlots.TryGetValue(portId, out var keys) ||
                keys == null)
                return null;

            Dictionary<int, Substrate> result = new Dictionary<int, Substrate>();
            foreach (var item in keys)
            {
                if (_substratesByKey.TryGetValue(item.Value, out var s))
                {
                    result[item.Key] = s;
                }
            }

            return result;
        }
        public bool HasAnySubstrateAtLoadPort(int portId)
        {
            return _substratesAtLoadPortSlots[portId].Count > 0;
        }
        public bool HasSubstrateAtLoadPort(int portId, int slot)
        {
            return _substratesAtLoadPortSlots[portId].TryGetValue(slot, out _);
        }
        public string GetSubstrateNameAtLoadPort(int portId, int slot)
        {
            if (portId <= 0 || slot < 0)
                return string.Empty;

            if (false == _substratesAtLoadPortSlots[portId].TryGetValue(slot, out var key) || string.IsNullOrWhiteSpace(key))
                return string.Empty;

            if (false == _substratesByKey.TryGetValue(key, out var s) || s == null)
                return string.Empty;

            return s.Name;
        }
        public string GetSubstrateNameByDestinationPortId(int portId, int slot)
        {
            if (portId <= 0 || slot < 0)
                return string.Empty;

            if (false == GetSubstrateByDestinationInfo(portId, slot, out var s) || s == null)
                return string.Empty;

            return s.Name;
        }
        public bool GetSubstrateBySourceCarrierInfo(int portId, int slot, string carrierId, out Substrate s)
        {
            s = null;
            foreach (var pm in _substratesAtProcessModule)
            {
                foreach (var item in pm.Value)
                {
                    if (false == _substratesByKey.TryGetValue(item.Key, out s) || s == null)
                        continue;

                    if (s.SourcePortId.Equals(portId) &&
                        s.SourceSlot.Equals(slot) &&
                        string.Equals(s.SourceCarrierId, carrierId, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    //if (item.Value.SourcePortId.Equals(portId) &&
                    //    item.Value.SourceSlot.Equals(slot) &&
                    //    item.Value.SourceCarrierId.Equals(carrierId))
                    //{
                    //    substrate = item.Value;
                    //    return true;
                    //}

                }
            }

            foreach (var pm in _substratesAtRobot)
            {
                foreach (var item in pm.Value)
                {
                    if (false == _substratesByKey.TryGetValue(item.Value, out s) || s == null)
                        continue;

                    if (s.SourcePortId.Equals(portId) &&
                        s.SourceSlot.Equals(slot) &&
                        string.Equals(s.SourceCarrierId, carrierId, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    //if (item.Value.SourcePortId.Equals(portId) &&
                    //   item.Value.SourceSlot.Equals(slot) &&
                    //    item.Value.SourceCarrierId.Equals(carrierId))
                    //{
                    //    substrate = item.Value;
                    //    return true;
                    //}
                }
            }

            return false;
        }
        public bool GetSubstrateByDestinationInfo(int portId, int slot, out Substrate s)
        {
            s = null;

            foreach (var pm in _substratesAtProcessModule)
            {
                foreach (var item in pm.Value)
                {
                    if (false == _substratesByKey.TryGetValue(item.Key, out s) || s == null)
                        continue;

                    if (s.DestinationPortId.Equals(portId) &&
                        s.DestinationSlot.Equals(slot))
                    {
                        return true;
                    }

                    //if (item.Value.DestinationPortId.Equals(portId) &&
                    //    item.Value.DestinationSlot.Equals(slot))
                    //{
                    //    s = item.Value;
                    //    return true;
                    //}
                }
            }

            foreach (var robot in _substratesAtRobot)
            {
                foreach (var item in robot.Value)
                {
                    if (false == _substratesByKey.TryGetValue(item.Value, out s) || s == null)
                        continue;

                    if (s.DestinationPortId.Equals(portId) &&
                        s.DestinationSlot.Equals(slot))
                    {
                        return true;
                    }

                    //if (item.Value.DestinationPortId.Equals(portId) &&
                    //   item.Value.DestinationSlot.Equals(slot))
                    //{
                    //    s = item.Value;
                    //    return true;
                    //}
                }
            }

            return false;
        }
        #endregion </LoadPort>

        #region <ProcessModule>
        // 수정함
        public bool GetSubstratesAtProcessModule(string processModuleName, ref List<Substrate> substrates)
        {
            if (false == _substratesAtProcessModule.TryGetValue(processModuleName, out var maps) || maps == null)
                return false;

            List<Substrate> result = new List<Substrate>();
            foreach (var item in maps)
            {
                if (false == _substratesByKey.TryGetValue(item.Key, out var s) ||
                    s == null)
                    continue;

                result.Add(s);
            }

            substrates.Clear();
            substrates.AddRange(result);

            return true;
        }
        //
        #endregion </ProcessModule>

        #region <Robot>
        public bool GetSubstratesAtRobotAll(string robotName, ref Dictionary<RobotArmTypes, Substrate> substrates)
        {
            if (false == _substratesAtRobot.TryGetValue(robotName, out var keys) ||
                keys == null)
                return false;

            substrates.Clear();
            foreach (var item in keys)
            {
                if (false == _substratesByKey.TryGetValue(item.Value, out var s) ||
                    s == null)
                    continue;

                substrates[item.Key] = s;
            }

            return substrates.Count > 0;
        }
        // 수정함
        public bool GetSubstrateAtRobot(string robotName, RobotArmTypes armType, out Substrate s)
        {
            s = null;

            if (false == _substratesAtRobot.TryGetValue(robotName, out var maps) || maps == null ||
                false == maps.TryGetValue(armType, out var key) || string.IsNullOrWhiteSpace(key) ||
                false == _substratesByKey.TryGetValue(key, out s) || s == null)
                return false;

            return true;
        }
        //
        #endregion </Robot>

        #endregion </Get Substrate>

        #region <ETC>
        private async Task<bool> RegisterSubstrate(Substrate substrate)
        {
            if (substrate == null)
                return false;

            var locId = substrate.LocationId;
            if (false == LocationServer.FindLocationById(locId, out var loc))
                return false;

            switch (loc)
            {
                case LoadPortLocation lp:
                    {
                        AssignSubstrateAtLoadPort(lp.PortId, lp.Slot, substrate);
                    }
                    break;

                case ProcessModuleLocation pm:
                    {
                        AssignSubstrateAtProcessModule(pm.ProcessModuleName, substrate);
                    }
                    break;

                case RobotLocation rb:
                    {
                        AssignSubstrateAtRobot(rb.RobotName, rb.Arm, substrate);
                    }
                    break;

                default:
                    return false;
            }

            // 위 Assign.. 내부에서 하므로 불필요
            // AddSubstrateKey(key, ref substrate);
            var ev = new SubstrateLocationStateChangedEvent(
                substrate.UniqueKey,
                locId,
                OccupancyState.Occupied,
                OccupancyChangeReason.Recovery
            );
            
            return await RaiseSubstrateLocationStateChanged(ev).ConfigureAwait(false);
            //MaterialHistoryManager.LocationStateService.Occupy(loc, substrate.UniqueKey, OccupancyChangeReason.Recovery);
        }
        private void AddSubstrateKey(string key, Substrate substrate)
        {
            _substratesByKey[key] = substrate;
        }
        private void RemoveSubstrateKey(string key, bool deleteAtStorage = true)
        {
            if (deleteAtStorage)
            {
                _storage.DeleteAsync(key);
            }

            _substratesByKey.TryRemove(key, out _);
        }
        #endregion </ETC>

        #region <개선 필요>
        // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        [Obsolete("개선 필요")]
        public bool MoveMaterialToATMRobot(Location destinationLocation, string robotName, RobotArmTypes arm, Substrate substrate)
        {
            if (substrate == null)
                return false;

            var locId = substrate.LocationId;
            if (false == LocationServer.FindLocationById(locId, out var sourceLocation))
                return false;

            var transport = substrate.TransportStatus;
            if (sourceLocation is LoadPortLocation)
            {
                var key = substrate.UniqueKey;
                //SetTransferStatusByKey(key, TransportStates.AtWork);
                transport = TransportStates.AtWork;
                //SaveDataByKey(key);
            }
            else if (sourceLocation is ProcessModuleLocation)
            {
            }

            return TransferSubstrate(
                substrate.UniqueKey,
                sourceLocation, 
                destinationLocation,
                transport,
                OccupancyChangeReason.Edited, 
                OccupancyChangeReason.Edited).GetAwaiter().GetResult();
        }
        [Obsolete("개선 필요")]
        public bool MoveMaterialToLoadPort(Location destinationLocation, int portId, int slot, Substrate substrate)
        {
            if (substrate == null)
                return false;

            var locId = substrate.LocationId;
            if (false == LocationServer.FindLocationById(locId, out var sourceLocation))
                return false;

            // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
            //// Source Port ID가 같아야 한다.
            //if (false == substrate.SourcePortId.Equals(portId))
            //    return false;

            var transport = substrate.TransportStatus;
            if (sourceLocation is RobotLocation ||
                sourceLocation is ProcessModuleLocation)
            {
                var key = substrate.UniqueKey;
                transport = TransportStates.AtDestination;
                //SetTransferStatusByKey(key, TransportStates.AtDestination);
                //SaveDataByKey(key);
            }
            //else if (sourceLocation is ProcessModuleLocation)
            //{
            //    substrate.SetTransferStatus(SubstrateTransferStates.AtDestination);
            //}

            // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
            // Destination Slot이 Empty이나, 기판이 Empty Robot이나 PM에 있을 경우, Source와 Destination 두 기판의 Source/Destination PortID, Slot을 Swap한다.
            // 그냥 비어있을 경우, Source 기판의 Source/Denstination PortID, Slot을 Move한다.
            var substrates = new List<string>();
            foreach (var item in _substratesAtProcessModule)
            {
                substrates.AddRange(item.Value.Keys);
            }
            foreach (var arms in _substratesAtRobot)
            {
                substrates.AddRange(arms.Value.Values);
            }

            foreach (var item in substrates)
            {
                if (false == _substratesByKey.TryGetValue(item, out var s) || s == null)
                    continue;

                if (s.SourcePortId.Equals(portId) && s.SourceSlot.Equals(slot))
                {
                    Substrate tempSubstrate = s;
                    SwapPortAndSlotOfSubstrates(ref substrate, ref tempSubstrate);
                    if (TransferSubstrate(
                        substrate.UniqueKey,
                        sourceLocation,
                        destinationLocation,
                        transport,
                        OccupancyChangeReason.Edited, 
                        OccupancyChangeReason.Edited).GetAwaiter().GetResult())
                    {
                        SaveDataByKey(substrate.UniqueKey);
                        SaveDataByKey(tempSubstrate.UniqueKey);

                        return true;
                    }

                    return false;
                }
            }

            MovePortAndSlot(destinationLocation, ref substrate);
            if (TransferSubstrate(substrate.UniqueKey,
                sourceLocation,
                destinationLocation,
                transport,
                OccupancyChangeReason.Edited, 
                OccupancyChangeReason.Edited).GetAwaiter().GetResult())
            {
                SaveDataByKey(substrate.UniqueKey);

                return true;
            }

            SaveDataByKey(substrate.UniqueKey);

            return false;
            // 2025.07.08. dwlim [END]
        }
        [Obsolete("개선 필요")]
        public bool MoveMaterialToProcessModule(Location destinationLocation, string moduleName, string locationName, Substrate substrate)
        {
            if (substrate == null)
                return false;

            var locId = substrate.LocationId;
            if (false == LocationServer.FindLocationById(locId, out var sourceLocation))
                return false;

            var transport = substrate.TransportStatus;
            if (sourceLocation is LoadPortLocation)
            {
                var key = substrate.UniqueKey;
                transport = TransportStates.AtWork;
                SaveDataByKey(key);
            }
            else if (sourceLocation is RobotLocation)
            {
            }

            return TransferSubstrate(
                substrate.UniqueKey,
                sourceLocation,
                destinationLocation,
                transport,
                OccupancyChangeReason.Edited,
                OccupancyChangeReason.Edited).GetAwaiter().GetResult();
        }
        // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        [Obsolete("개선 필요")]
        public bool SwapMaterialBetweenModules(string firstKey, Location firstLoc, string secondKey, Location secondLoc)
        {
            if (false == LocationServer.FindLocationById(firstLoc.Id, out _) ||
                false == LocationServer.FindLocationById(secondLoc.Id, out _))
                return false;

            // 일단 양 Location의 Substrate를 가져온다.
            // 1. First Substrate 가져온다.
            if (false == GetSubstrateByLocationAndKey(firstLoc, firstKey, out var firstSubstrate) ||
                false == GetSubstrateByLocationAndKey(secondLoc, secondKey, out var secondSubstrate))
                return false;

            // 3. First Substrate를 Second Substrate Location에 보낸다.
            #region <Set First Substrate>
            if (secondLoc is LoadPortLocation)
            {
                var location = secondLoc as LoadPortLocation;
                AssignSubstrateAtLoadPort(location.PortId, location.Slot, firstSubstrate);
            }
            else if (secondLoc is ProcessModuleLocation)
            {
                var location = secondLoc as ProcessModuleLocation;
                AssignSubstrateAtProcessModule(location.ProcessModuleName, firstSubstrate);
            }
            else if (secondLoc is RobotLocation)
            {
                var location = secondLoc as RobotLocation;
                AssignSubstrateAtRobot(location.RobotName, location.Arm, firstSubstrate);
            }
            else
                return false;

            // LocationStateService에서 진행하므로 제거
            //SetLocationByKey(firstSubstrate.UniqueKey, secondLoc);
            //SaveDataByKey(firstSubstrate.UniqueKey);
            #endregion </Set First Substrate>

            // 4. Second Substrate를 First Substrate Location에 보낸다.
            #region <Set Second Substrate>
            if (firstLoc is LoadPortLocation)
            {
                var location = firstLoc as LoadPortLocation;
                AssignSubstrateAtLoadPort(location.PortId, location.Slot, secondSubstrate);
            }
            else if (firstLoc is ProcessModuleLocation)
            {
                var location = firstLoc as ProcessModuleLocation;
                AssignSubstrateAtProcessModule(location.ProcessModuleName, secondSubstrate);
            }
            else if (firstLoc is RobotLocation)
            {
                var location = firstLoc as RobotLocation;
                AssignSubstrateAtRobot(location.RobotName, location.Arm, secondSubstrate);
            }
            else
                return false;

            // LocationStateService에서 진행하므로 제거
            //SetLocationByKey(secondSubstrate.UniqueKey, firstLoc);
            //SaveDataByKey(secondSubstrate.UniqueKey);
            #endregion </Set Second Substrate>

            #region <Set Port And Slot>
            SwapPortAndSlotOfSubstrates(ref firstSubstrate, ref secondSubstrate);
            #endregion </Set Port And Slot>

            SaveDataByKey(firstSubstrate.UniqueKey);
            SaveDataByKey(secondSubstrate.UniqueKey);

            // 캐시는 지우면 안 됨
            if (firstLoc is ProcessModuleLocation)
                RemoveSubstrateByKey(firstSubstrate.UniqueKey, false);

            if (secondLoc is ProcessModuleLocation)
                RemoveSubstrateByKey(secondSubstrate.UniqueKey, false);


            // 2) Location History 이벤트로 알림
            var ev = new SubstrateSwappedEvent(
                firstSubstrate.UniqueKey,
                secondSubstrate.UniqueKey,
                firstLoc.Id,
                secondLoc.Id,
                OccupancyChangeReason.Edited);

            return RaiseSubstrateSwapped(ev).GetAwaiter().GetResult();

            //return MaterialHistoryManager.LocationStateService.Swap(firstLoc, secondLoc, firstKey, secondKey, OccupancyChangeReason.Edited);
        }
        // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        [Obsolete("개선 필요")]
        private void SwapPortAndSlotOfSubstrates(ref Substrate firstSubstrate, ref Substrate secondSubstrate)
        {
            int firstSubSourcePort, firstSubSourceSlot, firstSubDestPort, firstSubDestSlot;
            int secondSubSourcePort, secondSubSourceSlot, secondSubDestPort, secondSubDestSlot;

            firstSubSourcePort = firstSubstrate.SourcePortId;
            firstSubSourceSlot = firstSubstrate.SourceSlot;
            firstSubDestPort = firstSubstrate.DestinationPortId;
            firstSubDestSlot = firstSubstrate.DestinationSlot;

            secondSubSourcePort = secondSubstrate.SourcePortId;
            secondSubSourceSlot = secondSubstrate.SourceSlot;
            secondSubDestPort = secondSubstrate.DestinationPortId;
            secondSubDestSlot = secondSubstrate.DestinationSlot;

            var firstKey = firstSubstrate.UniqueKey;
            var secondKey = secondSubstrate.UniqueKey;
            SetSourcePortIdByKey(firstKey, secondSubSourcePort);
            SetSourceSlotByKey(firstKey, secondSubSourceSlot);
            SetDestinationPortIdByKey(firstKey, secondSubDestPort);
            SetDestinationSlotByKey(firstKey, secondSubDestSlot);

            SetSourcePortIdByKey(secondKey, firstSubSourcePort);
            SetSourceSlotByKey(secondKey, firstSubSourceSlot);
            SetDestinationPortIdByKey(secondKey, firstSubDestPort);
            SetDestinationSlotByKey(secondKey, firstSubDestSlot);
        }
        // 2025.07.08. dwlim [ADD] Move Substrate Information 추가
        [Obsolete("개선 필요")]
        private void MovePortAndSlot(Location destinationLocation, ref Substrate substrate)
        {
            int sourcePort, sourceSlot, destinationPort, destinationSlot;

            //Location sourceLocation = substrate.CurrentLocation;

            if (destinationLocation is LoadPortLocation)
            {
                var location = destinationLocation as LoadPortLocation;
                var key = substrate.UniqueKey;

                sourcePort = location.PortId;
                sourceSlot = location.Slot;
                destinationPort = location.PortId;
                destinationSlot = location.Slot;

                SetSourcePortIdByKey(key, sourcePort);
                SetSourceSlotByKey(key, sourceSlot);
                SetDestinationPortIdByKey(key, destinationPort);
                SetDestinationSlotByKey(key, destinationSlot);
            }
            else if (destinationLocation is ProcessModuleLocation)
            {
                var location = destinationLocation as ProcessModuleLocation;
            }
            else if (destinationLocation is RobotLocation)
            {
                var location = destinationLocation as RobotLocation;
            }
        }
        #endregion </개선 필요>

        #endregion </Methods>
    }
}