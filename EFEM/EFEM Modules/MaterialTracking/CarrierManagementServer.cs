using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Work;
using FrameOfSystem3.Functional;

using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking.CarrierStorage;
using EFEM.Defines.ProcessTypeProvider;
using EFEM.Jobs.Binding;

namespace EFEM.MaterialTracking
{
    public class CarrierManagementServer
    {
        #region <Constructors>
        private CarrierManagementServer(ICarrierStorage storage, IMaterialExtraAttribute profile, IProcessTypeProvider provider)
        {
            _profile = profile;
            _storage = storage;

            _substrateManager = SubstrateManager.Instance;
        }
        #endregion </Constructors>

        #region <Fields>
        private static CarrierManagementServer _instance = null;
        private readonly ConcurrentDictionary<int, string> _carrierKeysByPort = new ConcurrentDictionary<int, string>();
        private readonly ConcurrentDictionary<string, Carrier> _carriersByKey = new ConcurrentDictionary<string, Carrier>();

        private static SubstrateManager _substrateManager = null;

        private static readonly object _gate = new object();
        private readonly IMaterialExtraAttribute _profile;
        private ICarrierStorage _storage;
        #endregion </Fields>

        #region <Properties>
        public static CarrierManagementServer Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>
        public static void Configure(ICarrierStorage storage, IMaterialExtraAttribute profile, IProcessTypeProvider provider)
        {
            lock (_gate)
            {
                if (_instance == null)
                {
                    _instance = new CarrierManagementServer(storage, profile, provider);
                }
            }
        }
        public bool LoadRecoveryDataAll()
        {
            _storage.InitializeStorage();

            if (false == _storage.LoadDataFromStorage(out var data))
                return false;

            for (int i = 0; i < data.Count; ++i)
            {
                Carrier carrier = CarrierMapper.ToDomain(data[i]);
                if (carrier == null)
                    continue;

                AssignCarrier(carrier.UniqueKey, carrier.PortId, carrier);
            }

            return true;
        }
        public void CreateCarrier(int portId)
        {
            var key = CarrierMapper.MakeCarrierKey(portId);
            var carrier = new Carrier(key, portId);

            var extra = new Dictionary<string, string>();
            _profile.CreateAttributes(extra);
            _profile.InitializeToPublish(extra, carrier);
            foreach (var item in extra)
            {
                carrier.SetAttribute(item.Key, item.Value);
            }

            // 기존에는 파일을 무조건 만들어두고, 값만 바꿔치기 했었음
            // Substrate 와 동일하게 생성될 때 파일 생성 -> 완료 후 파일 이동(Archive) Rule로 간다.
            AssignCarrier(key, portId, carrier);

            SaveCarrierData(portId);
        }
        public void SaveCarrierData(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return;

            var dto = CarrierMapper.ToData(carrier);
            
            _storage.UpsertAsync(dto).GetAwaiter().GetResult();
        }
        public void RemoveOrArchiveCarrierByPort(int portId, string archiveRootPath)
        {
            UpdateSlotLocationNameToLocation(portId, null);

            // 1) HasCarrier 여부 판단
            if (HasCarrier(portId))
            {
                var carrierId = GetCarrierId(portId);
                var baseArchivePath = System.IO.Path.Combine(
                    archiveRootPath,
                    carrierId,
                    DateTime.Now.ToString("HHmmss"));

                RemoveCarrier(portId, baseArchivePath);
            }
            else
            {
                var carrierId = $"UnknownCarrierId_{portId}";
                var baseArchivePath = System.IO.Path.Combine(archiveRootPath, carrierId);

                MoveToArchiveUnknownCarrier(portId, baseArchivePath);
            }
        }
        public int GetPortIdByCarrierId(string carrierId)
        {
            if (string.IsNullOrWhiteSpace(carrierId))
                return 0;

            foreach (var item in _carrierKeysByPort)
            {
                int portId = item.Key;
                string carrierKey = item.Value;

                if (string.IsNullOrWhiteSpace(carrierKey))
                    continue;

                Carrier carrier;

                if (!_carriersByKey.TryGetValue(carrierKey, out carrier) || carrier == null)
                    continue;

                if (string.Equals(carrier.CarrierId, carrierId, StringComparison.OrdinalIgnoreCase))
                    return portId;
            }

            return 0;
        }
        public bool HasCarrier(int portId)
        {
            return _carrierKeysByPort.ContainsKey(portId);
        }
        public Carrier GetCarrierInfoById(string carrierId)
        {
            if (string.IsNullOrWhiteSpace(carrierId))
                return null;

            foreach (var item in _carriersByKey)
            {
                if (string.IsNullOrWhiteSpace(item.Value.CarrierId))
                    continue;

                if (string.Equals(item.Value.CarrierId, carrierId, StringComparison.OrdinalIgnoreCase))
                    return item.Value;
            }

            return null;
        }
        public string GetCarrierKey(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return string.Empty;

            return carrier.UniqueKey;
        }
        public string GetCarrierId(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return string.Empty;

            return carrier.CarrierId;
        }
        public string GetCarrierLotId(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return string.Empty;

            return carrier.LotId;
        }
        public int GetCapacity(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return 0;

            return carrier.Capacity;
        }
        public string GetAttribute(int portId, string attributeKey)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return string.Empty;

            return carrier.GetAttribute(attributeKey);
        }
        public IReadOnlyDictionary<int, CarrierSlotMapStates> GetCarrierSlotMap(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return null;

            return carrier.SlotMaps;
        }
        public CarrierAccessStates GetCarrierAccessingStatus(int portId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return CarrierAccessStates.NotAccessed;

            return carrier.AccessingStatus;
        }

        public bool SetCarrierId(int portId, string carrierId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return false;

            carrier.SetCarrierId(carrierId);

            return true;
        }
        public bool SetCarrierLotId(int portId, string lotId)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return false;

            carrier.SetLotId(lotId);

            return true;
        }
        public void SetCarrierSlotMap(int portId, IDictionary<int, CarrierSlotMapStates> slotMap)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return;

            ModifySlotMapByStatus(portId, carrier.LotId, carrier.UniqueKey, carrier.CarrierId, slotMap, out var modifiedMap);
            carrier.SetSlotMaps(modifiedMap);

            SaveCarrierData(portId);

            // 중요:
            // Job이 재료보다 먼저 생성된 경우,
            // PRJobCreate / ControlJobCreate 시점에는 아직 Substrate가 없어서 바인딩하지 못한다.
            //
            // 따라서 SlotMap 반영이 끝나고 Substrate가 생성된 직후,
            // 해당 Carrier/Port 기준으로 다시 Job-Substrate 바인딩을 시도한다.
            //
            // 이 호출은 실패해도 Carrier/SlotMap 책임을 깨면 안 되므로
            // Binder가 미설정이면 조용히 skip한다.
            if (SubstrateJobBindingService.Instance != null)
                SubstrateJobBindingService.Instance.BindByCarrierPort(portId);
        }
        public void SetCarrierAccessingStatus(int portId, CarrierAccessStates newState)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return;

            carrier.SetAccessingStatus(newState);
        }
        public bool SetAttribute(int portId, string attributeKey, string attributeValue)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return false;

            carrier.SetAttribute(attributeKey, attributeValue);
            return true;
        }
        public void UpdateSlotLocationNameToLocation(int portId, string carrierId)
        {
            if (LocationServer.GetLoadPortLocations(portId, out var locs))
            {
                foreach (var item in locs)
                {
                    string toName = string.Empty;
                    if (false == string.IsNullOrWhiteSpace(carrierId))
                    {
                        toName = LocationNameConverter.CreateInitialNameAtLoadPort(carrierId, item.Value.Slot);
                    }

                    item.Value.Name = toName;
                    LocationService.Instance.UpdateDisplayNameAsync(item.Value.Id, toName);
                }
            }
        }
        private void AssignCarrier(string key, int portId, Carrier carrier)
        {
            _carriersByKey[key] = carrier;
            _carrierKeysByPort[portId] = key;

            var carrierId = carrier.CarrierId;
            UpdateSlotLocationNameToLocation(carrier.PortId, carrierId);
        }
        private void RemoveCarrier(int portId, string baseArchivePath)
        {
            if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
                string.IsNullOrWhiteSpace(key) ||
                false == _carriersByKey.TryGetValue(key, out var carrier) ||
                carrier == null)
                return;

            WriteHistoryAndArchive(
                portId,
                key,
                baseArchivePath,
                false);

            //Task.Run(() => WriteHistoryAndArchive(
            //    portId,
            //    key,
            //    baseArchivePath,
            //    false));

            RemoveCarrierAtIndex(key, portId);

            //if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
            //    string.IsNullOrWhiteSpace(key) ||
            //    false == _carriersByKey.TryGetValue(key, out var carrier) ||
            //    carrier == null)
            //    return;

            //if (false == isUnexpectedRemoval)
            //{
            //    carrier.OnCarrierRemoved();
            //}

            //WriteHistoryBeforeUnload(portId);

            //if (false == string.IsNullOrWhiteSpace(baseArchivePath))
            //{
            //    _storage.ArchiveAsync(key, portId, baseArchivePath);
            //}

            ////Carriers[portId].ClearAttributes();
            //RemoveCarrierAtIndex(key, portId);
        }
        private void RemoveCarrierAtIndex(string key, int portId)
        {
            _carrierKeysByPort.TryRemove(portId, out _);
            _carriersByKey.TryRemove(key, out _);
        }
        private void MoveToArchiveUnknownCarrier(int portId, string baseArchivePath)
        {
            string key = null;
            //Carrier carrier = null; 

            // 포트 기준으로 known carrier인지 확인
            if (_carrierKeysByPort.TryGetValue(portId, out var foundKey) &&
                !string.IsNullOrWhiteSpace(foundKey) &&
                _carriersByKey.TryGetValue(foundKey, out var foundCarrier) &&
                foundCarrier != null)
            {
                key = foundKey;
                //carrier = foundCarrier;
            }

            var isUnknown = string.IsNullOrWhiteSpace(key);
            WriteHistoryAndArchive(
                portId,
                key,
                baseArchivePath,
                isUnknown);

            //Task.Run(() => WriteHistoryAndArchive(
            //    portId,
            //    key,
            //    baseArchivePath,
            //    isUnknown));

            //if (false == _carrierKeysByPort.TryGetValue(portId, out var key) ||
            //    string.IsNullOrWhiteSpace(key) ||
            //    false == _carriersByKey.TryGetValue(key, out var carrier) ||
            //    carrier == null)
            //{
            //    WriteHistoryBeforeUnload(portId);

            //    if (_storage.IsExists(portId, out var findKey) &&
            //        false == string.IsNullOrWhiteSpace(findKey))
            //    {
            //        _storage.ArchiveAsync(findKey, portId, baseArchivePath);
            //    }
            //}
            //else
            //{
            //    WriteHistoryBeforeUnload(portId);

            //    // 혹시라도 있으면..?
            //    if (false == string.IsNullOrWhiteSpace(baseArchivePath))
            //    {
            //        _storage.ArchiveAsync(key, portId, baseArchivePath);
            //    }
            //}
        }
        private async void WriteHistoryAndArchive(
            int portId,
            string key,
            string baseArchivePath,
            bool allowStorageLookupWhenKeyMissing)
        {
            if (string.IsNullOrWhiteSpace(baseArchivePath))
                return;

            await _substrateManager.WriteHistoryBeforeRemoving(portId, baseArchivePath);

            if (false == string.IsNullOrWhiteSpace(key))
            {
                // 1) 캐리어 삭제
                await _storage.ArchiveAsync(key, portId, baseArchivePath);

                _substrateManager.BackupAndRemoveSubstrateAtLoadPortAll(portId, baseArchivePath);
                return;
            }

            // 4) key가 없고, 저장소에서라도 찾고 싶을 때만 추가 처리
            if (allowStorageLookupWhenKeyMissing &&
                _storage.IsExists(portId, out var findKey) &&
                false == string.IsNullOrWhiteSpace(findKey))
            {
                await _storage.ArchiveAsync(findKey, portId, baseArchivePath);
            }

            _substrateManager.BackupAndRemoveSubstrateAtLoadPortAll(portId, baseArchivePath);
        }
        //private void WriteHistoryBeforeUnload(int portId, string archive)
        //{
        //    var keys = _substrateManager.GetSubstrateKeysAtLoadPort(portId);
        //    foreach (var item in keys)
        //    {
        //        if (false == _substrateManager.GetSubstrateByKey(item, out var s) || s == null)
        //            continue;

        //        if (false == LocationServer.FindLocationByName(s.LocationId, out var loc) || loc == null)
        //            continue;
        //        MaterialHistoryManager.LocationStateService.Vacate(loc, item, OccupancyChangeReason.Removed);
        //    }
        //}

        private void ModifySlotMapByStatus(int portId, string lotId, string carrierKey, string carrierId, IDictionary<int, CarrierSlotMapStates> slotMap, out Dictionary<int, CarrierSlotMapStates> modifiedMap)
        {
            List<string> keysToUpdate = new List<string>();
            modifiedMap = new Dictionary<int, CarrierSlotMapStates>();
            foreach (var item in slotMap)
            {
                int slot = item.Key;
                if (LocationServer.GetLoadPortLocation(portId, slot, out var location))
                {
                    switch (item.Value)
                    {
                        case CarrierSlotMapStates.CorrectlyOccupied:
                            {
                                modifiedMap[item.Key] = item.Value;

                                // 1) 해당 포트에 자재가 없으면 -> 만든다.
                                if (false == _substrateManager.HasSubstrateAtLoadPort(portId, slot))
                                {
                                    var key = SubstrateMapper.MakeUniqueKey(carrierId, location.Id);
                                    var name = SubstrateMapper.MakeDefualtName(carrierId, location.Id);
                                    _substrateManager.CreateSubstrate(key, name, location);

                                    if (_substrateManager.GetSubstrateByKey(key, out var substrate) ||
                                        _substrateManager.GetSubstrateByLocationAndKey(location, key, out substrate))
                                    {
                                        if (substrate != null)
                                        {
                                            _substrateManager.SetLotIdByKey(key, lotId);
                                            _substrateManager.SetRecipeIdByKey(key, string.Empty);
                                            _substrateManager.SetSourceCarrierIdByKey(key, carrierId);
                                            _substrateManager.SetCurrentCarrierKeyByKey(key, carrierKey);

                                            //_substrateManager.SaveDataByKey(key);
                                            keysToUpdate.Add(key);
                                        }
                                    }
                                }
                            }
                            break;

                        case CarrierSlotMapStates.Empty:
                            {
                                if (_substrateManager.GetSubstrateBySourceCarrierInfo(portId, slot, carrierId, out var substrate))
                                {
                                    // 2) 포트에 자재가 없는데, 포트번호+슬롯+캐리어정보가 같은 자재가 있으면 있는 것으로 변경 -> 공정을 위해 나가있다.
                                    var processingStatus = substrate.ProcessingStatus;
                                    switch (processingStatus)
                                    {
                                        case ProcessingStates.Lost:
                                            {
                                                // 1. 자재가 제거되었다. : 제거된 경우니 없는게 정상이다.
                                            }
                                            break;
                                        default:
                                            {
                                                modifiedMap[item.Key] = item.Value;
                                                //SlotInformation[slot] = CarrierSlotMapStates.CorrectlyOccupied;
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    modifiedMap[item.Key] = item.Value;
                                }
                            }
                            break;

                        case CarrierSlotMapStates.NotEmpty:
                        case CarrierSlotMapStates.CrossSlotted:
                        case CarrierSlotMapStates.DoubleSlotted:
                            {
                                modifiedMap[item.Key] = item.Value;

                                // 1) 해당 포트에 자재가 없으면 -> 만든다.
                                if (false == _substrateManager.HasSubstrateAtLoadPort(portId, slot))
                                {
                                    var key = SubstrateMapper.MakeUniqueKey(carrierId, location.Id);
                                    var name = SubstrateMapper.MakeDefualtName(carrierId, location.Id);
                                    _substrateManager.CreateSubstrate(key, name, location);

                                    if (_substrateManager.GetSubstrateByKey(key, out var substrate) ||
                                        _substrateManager.GetSubstrateByLocationAndKey(location, key, out substrate))
                                    {
                                        if (substrate != null)
                                        {
                                            _substrateManager.SetLotIdByKey(key, lotId);
                                            _substrateManager.SetRecipeIdByKey(key, string.Empty);
                                            _substrateManager.SetSourceCarrierIdByKey(key, carrierId);
                                            _substrateManager.SetCurrentCarrierKeyByKey(key, carrierKey);
                                            //_substrateManager.SaveDataByKey(key);
                                            keysToUpdate.Add(key);
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
            _substrateManager.SaveDataByKeys(keysToUpdate).GetAwaiter().GetResult();
        }
        #endregion </Methods>
    }
}