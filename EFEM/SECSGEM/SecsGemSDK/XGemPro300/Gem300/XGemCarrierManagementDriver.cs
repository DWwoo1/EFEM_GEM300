using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using EFEM.Defines.CarrierManagement;

using EFEM.Defines.LoadPort;

using XGEM300PRO.Library;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class XGemCarrierManagementDriver : ICarrierManagementDriver
    {
        private readonly XGem300ProW _driver;
        private const int CarrierMaxCapacity = 25;

        public XGemCarrierManagementDriver(XGem300ProW driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            SubscribeDriverEvents();
        }

        public event EventHandler<CarrierPortCarrierEventArgs> CarrierInStarted;
        public event EventHandler<CarrierDeletedEventArgs> CarrierDeleted;
        public event EventHandler<LoadPortStateChangedEventArgs> CarrierTransferStateChanged;
        public event EventHandler<LoadPortStateChangedEventArgs> CarrierAccessModeChanged;
        public event EventHandler<CarrierVerificationSucceededEventArgs> CarrierVerificationSucceeded;
        public event EventHandler<CarrierVerificationResultWithoutRemoteArgs> CarrierVerificationResultWithoutRemote;
        public event EventHandler<CarrierVerificationFailedEventArgs> CarrierVerificationFailed;
        public event EventHandler<HostCarrierRequestEventArgs> CarrierInRequestedByHost;
        public event EventHandler<HostCarrierRequestEventArgs> CarrierOutRequestedByHost;
        public event EventHandler<HostCarrierRequestEventArgs> CarrierCancelRequestedByHost;
        public event EventHandler<HostChangeAccessRequestEventArgs> AccessChangeRequestedByHost;
        public event EventHandler<HostChangeServiceStatusRequestEventArgs> ServiceStatusChangeRequestedByHost;

        public long NotifyCarrierDetection(string locationId,
            string carrierId,
            CarrierIdVerificationStates idVerificationResult,
            bool detectionStatus)
        {
            long status = detectionStatus ? 1 : 0;
            // 신호 통지는 패스해도 되나 확인 필요
            //long result = _driver.CMSSetPresenceSensor(locationId, status);
            //if (result != 0)
            //    return result;

            var result = _driver.CMSSetCarrierOnOff(locationId, status);

            if (false == detectionStatus)
            {
                long objectId = 0, count = 0;
                result = _driver.CMSGetAllCarrierInfo(ref objectId, ref count);
                if (result == 0)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        string id = string.Empty;
                        result = _driver.GetCarrierLocID(objectId, i, ref id);
                        if (result == 0 && string.Equals(locationId, id, StringComparison.OrdinalIgnoreCase))
                        {
                            result = _driver.GetCarrierID(objectId, i, ref carrierId);
                            if (result == 0)
                            {
                                _driver.GetCarrierClose(objectId);
                                break;
                            }
                        }
                    }
                }
                
                return _driver.CMSDelCarrierInfo(carrierId);
            }

            return result;
        }
        public long ReqBind(string locationId, string carrierId, string slotMap)
        {
            return _driver.CMSReqBind(locationId ?? string.Empty, carrierId ?? string.Empty, slotMap ?? string.Empty);
        }

        public long ReqCancelBind(string locationId, string carrierId)
        {
            return _driver.CMSReqCancelBind(locationId ?? string.Empty, carrierId ?? string.Empty);
        }

        public long ReqCarrierReCreate(string locationId, string carrierId)
        {
            return _driver.CMSReqCarrierReCreate(locationId ?? string.Empty, carrierId ?? string.Empty);
        }

        public long ReqCancelCarrier(string locationId, string carrierId)
        {
            return _driver.CMSReqCancelCarrier(locationId ?? string.Empty, carrierId ?? string.Empty);
        }

        public long ReqProceedCarrier(
            string locationId,
            string carrierId,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            IReadOnlyDictionary<int, string> lots,
            IReadOnlyDictionary<int, string> substrateNames,
            string usage)
        {
            ValidateCarrierCapacity(CarrierMaxCapacity);

            ValidateSlotKeys(map, CarrierMaxCapacity, nameof(map));
            ValidateSlotKeys(lots, CarrierMaxCapacity, nameof(lots));
            ValidateSlotKeys(substrateNames, CarrierMaxCapacity, nameof(substrateNames));

            string slotMap = BuildSlotMap(map, CarrierMaxCapacity);
            string[] safeLotIds = BuildSlotStringArray(lots, CarrierMaxCapacity);
            string[] safeSubstrateIds = BuildSlotStringArray(substrateNames, CarrierMaxCapacity);

            return _driver.CMSReqProceedCarrier(
                locationId ?? string.Empty,
                carrierId ?? string.Empty,
                slotMap,
                safeLotIds.Length,
                safeLotIds,
                safeSubstrateIds,
                usage ?? string.Empty);
        }

        public long RspCarrierIn(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];
            int count = ResolveParallelCount(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.CMSRspCarrierIn(messageId, locationId ?? string.Empty, carrierId ?? string.Empty, result, count, safeCodes, safeTexts);
        }

        public long RspCarrierOut(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];
            int count = ResolveParallelCount(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.CMSRspCarrierOut(messageId, locationId ?? string.Empty, carrierId ?? string.Empty, result, count, safeCodes, safeTexts);
        }

        public long RspCancelCarrier(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];
            int count = ResolveParallelCount(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.CMSRspCancelCarrier(messageId, locationId ?? string.Empty, carrierId ?? string.Empty, result, count, safeCodes, safeTexts);
        }

        public long RspCarrierRelease(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];
            int count = ResolveParallelCount(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.CMSRspCarrierRelease(messageId, locationId ?? string.Empty, carrierId ?? string.Empty, result, count, safeCodes, safeTexts);
        }

        public long RspChangeAccess(long messageId, long mode, long result, string[] locationIds, long[] errorCodes, string[] errorTexts)
        {
            string[] safeLocIds = locationIds ?? new string[0];
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];
            int count = ResolveParallelCount(safeLocIds.Length, safeCodes.Length, safeTexts.Length, nameof(locationIds), nameof(errorCodes), nameof(errorTexts));

            return _driver.CMSRspChangeAccess(messageId, mode, result, count, safeLocIds, safeCodes, safeTexts);
        }

        public long RspChangeServiceStatus(long messageId, string locationId, long state, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];
            int count = ResolveParallelCount(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.CMSRspChangeServiceStatus(messageId, locationId ?? string.Empty, state, result, count, safeCodes, safeTexts);
        }

        public long SetLoadPortInfo(string locationId, LoadPortStateInformation state, string carrierId)
        {
            //long result = 0;
            //switch (transferState)
            //{
            //    case LoadPortTransferStates.ReadyToLoad:
            //        {
            //            result = _driver.CMSSetReadyToLoad(locationId);
            //        }
            //        break;
            //    case LoadPortTransferStates.ReadyToUnload:
            //        {
            //            result = _driver.CMSSetReadyToUnload(locationId);
            //        }
            //        break;

            //    default:
            //        break;
            //}

            //if (result != 0)
            //    return result;
            //carrierId;
            if (state.CarrierIdVerificationState == CarrierIdVerificationStates.NotRead)
            {
                carrierId = string.Empty;
            }

            //_driver.CMSSetCarrierLocationInfo(
            //    locationId,
            //    carrierId);

            long transferState;
            switch (state.TransferState)
            {
                //case LoadPortTransferStates.Unknown:
                //case LoadPortTransferStates.OutOfService:
                    
                //    break;
                case LoadPortTransferStates.InService:
                    transferState = 100;
                    break;
                case LoadPortTransferStates.TransferBlocked:
                    transferState = 1;
                    break;
                case LoadPortTransferStates.ReadyToLoad:
                    transferState = 2;
                    break;
                case LoadPortTransferStates.ReadyToUnload:
                    transferState = 3;
                    break;
                default:
                    transferState = 0;
                    break;
            }
            long result = _driver.CMSSetLPInfo(
                locationId ?? string.Empty,
                transferState,
                (long)state.AccessMode,
                (long)state.ReservationState,
                (long)state.AssociationState,
                carrierId ?? string.Empty);

            if (result != 0)
            {

            }

            switch (state.TransferState)
            {
                case LoadPortTransferStates.ReadyToLoad:
                    result = SetReadyToLoad(locationId);
                    break;
                case LoadPortTransferStates.ReadyToUnload:
                    result = SetReadyToUnload(locationId);
                    break;
                default:
                    break;
            }

            return result;
        }
        public long ChangeAccessMode(string locationId, LoadPortAccessMode mode)
        {
            return _driver.CMSReqChangeAccess((long)mode, locationId);
        }
        public long SetCarrierLocation(string locationId, string carrierId)
        {
            return _driver.CMSSetCarrierLocationInfo(locationId ?? string.Empty, carrierId ?? string.Empty);
        }
        public long SetCarrierMovement(string locationId, string carrierId)
        {
            return _driver.CMSSetCarrierMovement(locationId ?? string.Empty, carrierId ?? string.Empty);
        }
        public long SetCarrierAccessing(string locationId, CarrierAccessStates state, string carrierId)
        {
            return _driver.CMSSetCarrierAccessing(
                locationId ?? string.Empty, 
                (long)state,
                carrierId ?? string.Empty);
        }
        public long SetCarrierIdentifier(string locationId, string carrierId, VerificationResult result)
        {
            return _driver.CMSSetCarrierID(locationId ?? string.Empty, carrierId ?? string.Empty, (long)result);
        }
        public long SetCarrierIdStatus(string carrierId, CarrierIdVerificationStates state)
        {
            return _driver.CMSSetCarrierIDStatus(carrierId ?? string.Empty, (long)state);
        }
        public long SetSlotMap(
            string locationId,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            string carrierId,
            VerificationResult result)
        {
            ValidateCarrierCapacity(CarrierMaxCapacity);
            ValidateSlotKeys(map, CarrierMaxCapacity, nameof(map));

            string slotMap = BuildSlotMap(map, CarrierMaxCapacity);

            var r = _driver.CMSSetSlotMap(
                locationId ?? string.Empty,
                slotMap,
                carrierId ?? string.Empty,
                (long)result);

            return r;
        }
        public long SetSlotMapStatus(string carrierId, CarrierSlotMapStates state)
        {
            return _driver.CMSSetSlotMapStatus(carrierId ?? string.Empty, (long)state);
        }
        public long SetCarrierInfo(
            string carrierId,
            string locationId,
            CarrierIdVerificationStates carrierIdStatus,
            CarrierSlotMapStates slotMapStatus,
            CarrierAccessStates accessingStatus,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            string[] lotIds,
            string[] substrateIds,
            string usage)
        {
            ValidateCarrierCapacity(CarrierMaxCapacity);
            ValidateSlotKeys(map, CarrierMaxCapacity, nameof(map));

            string[] safeLotIds = NormalizeStringArray(lotIds);
            string[] safeSubstrateIds = NormalizeStringArray(substrateIds);

            ValidateArrayLength(
                safeLotIds,
                CarrierMaxCapacity,
                nameof(lotIds));

            ValidateArrayLength(
                safeSubstrateIds,
                CarrierMaxCapacity,
                nameof(substrateIds));

            int count = ResolveParallelCount(
                safeLotIds.Length,
                safeSubstrateIds.Length,
                nameof(lotIds),
                nameof(substrateIds));

            string slotMap = BuildSlotMap(map, CarrierMaxCapacity);

            return _driver.CMSSetCarrierInfo(
                carrierId ?? string.Empty,
                locationId ?? string.Empty,
                (long)carrierIdStatus,
                (long)slotMapStatus,
                (long)accessingStatus,
                slotMap,
                count,
                safeLotIds,
                safeSubstrateIds,
                usage ?? string.Empty);
        }

        public long SetCarrierOutStart(string locationId, string carrierId)
        {
            return _driver.CMSSetCarrierOutStart(locationId ?? string.Empty, carrierId ?? string.Empty);
        }
        public long SetSubstrateCount(string carrierId, long substrateCount)
        {
            return _driver.CMSSetSubstrateCount(carrierId ?? string.Empty, substrateCount);
        }
        public long SetUsage(string carrierId, string usage)
        {
            return _driver.CMSSetUsage(carrierId ?? string.Empty, usage ?? string.Empty);
        }
        public long SetMaterialArrived(string materialId)
        {
            return _driver.CMSSetMaterialArrived(materialId ?? string.Empty);
        }
        public long SetPioSignal(string locationId, long signal, long state)
        {
            return _driver.CMSSetPIOSignalState(locationId ?? string.Empty, signal, state);
        }
        public long SetReadyToLoad(string locationId)
        {
            return _driver.CMSSetReadyToLoad(locationId ?? string.Empty);
        }
        public long SetReadyToUnload(string locationId)
        {
            return _driver.CMSSetReadyToUnload(locationId ?? string.Empty);
        }

        private void SubscribeDriverEvents()
        {
            _driver.OnCMSCarrierInStart += HandleCarrierInStart;
            _driver.OnCMSCarrierDeleted += HandleCarrierDeleted;
            _driver.OnCMSTransferStateChanged += HandleTransferStateChanged;
            _driver.OnCMSAccessModeStateChanged += HandleAccessModeChanged;
            _driver.OnCMSCarrierVerifySucceeded += HandleVerificationSucceeded;
            _driver.OnCMSCarrierVerifyFailed += HandleVerificationFailed;
            _driver.OnCMSRspProceedCarrier += HandleVerificationResultWithoutRemote;
            _driver.OnCMSReqCarrierIn += HandleCarrierInRequestedByHost;
            _driver.OnCMSReqCarrierOut += HandleCarrierOutRequestedByHost;
            _driver.OnCMSReqCancelCarrier += HandleCarrierCancelRequestedByHost;
            _driver.OnCMSReqChangeAccess += HandleAccessChangeRequestedByHost;
            _driver.OnCMSReqChangeServiceStatus += HandleServiceStatusChangeRequestedByHost;
        }

        private void HandleCarrierInStart(string locationId, string carrierId)
        {
            CarrierInStarted?.Invoke(this, new CarrierPortCarrierEventArgs(locationId, carrierId));
        }

        private void HandleCarrierDeleted(string carrierId)
        {
            CarrierDeleted?.Invoke(this, new CarrierDeletedEventArgs(carrierId));
        }

        private void HandleTransferStateChanged(string locationId, long state)
        {
            CarrierTransferStateChanged?.Invoke(this, new LoadPortStateChangedEventArgs(locationId, state));
        }

        private void HandleAccessModeChanged(string locationId, long state)
        {
            CarrierAccessModeChanged?.Invoke(this, new LoadPortStateChangedEventArgs(locationId, state));
        }

        private void HandleVerificationSucceeded(long verifyType, string locationId, string carrierId, string slotMap, long count, string[] lotIds, string[] substrateIds, string usage)
        {
            CarrierVerificationSucceeded?.Invoke(
                this,
                new CarrierVerificationSucceededEventArgs(
                    (VerificationType)verifyType,
                    locationId,
                    carrierId,
                    slotMap,
                    lotIds,
                    substrateIds,
                    usage));
        }
        private void HandleVerificationResultWithoutRemote(
            string locationId, 
            string carrierId,
            long count, 
            string[] lotIds, 
            string[] substrateIds, 
            string usage, 
            long result)
        {
            Dictionary<int, string> lots = new Dictionary<int, string>();
            Dictionary<int, string> ids = new Dictionary<int, string>();
            for (int i = 0; i < count; ++i)
            {
                lots[i + 1] = lotIds[i];
                ids[i + 1] = substrateIds[i];
            }
            CarrierVerificationResultWithoutRemote?.Invoke(
                this,
                new CarrierVerificationResultWithoutRemoteArgs(
                    locationId,
                    carrierId,
                    lots,
                    ids,
                    usage,
                    (VerificationResult)result));
        }
        private void HandleVerificationFailed(long verifyType, string locationId, string carrierId, long failReason)
        {
            CarrierVerificationFailed?.Invoke(
                this,
                new CarrierVerificationFailedEventArgs(
                    (VerificationType)verifyType,
                    locationId,
                    carrierId,
                    failReason));
        }

        private void HandleCarrierInRequestedByHost(long messageId, string locationId, string carrierId)
        {
            CarrierInRequestedByHost?.Invoke(this, new HostCarrierRequestEventArgs(messageId, locationId, carrierId));
        }

        private void HandleCarrierOutRequestedByHost(long messageId, string locationId, string carrierId)
        {
            CarrierOutRequestedByHost?.Invoke(this, new HostCarrierRequestEventArgs(messageId, locationId, carrierId));
        }

        private void HandleCarrierCancelRequestedByHost(long messageId, string locationId, string carrierId)
        {
            CarrierCancelRequestedByHost?.Invoke(this, new HostCarrierRequestEventArgs(messageId, locationId, carrierId));
        }

        private void HandleAccessChangeRequestedByHost(long messageId, long mode, long count, string[] locationIds)
        {
            AccessChangeRequestedByHost?.Invoke(this, new HostChangeAccessRequestEventArgs(messageId, mode, locationIds));
        }

        private void HandleServiceStatusChangeRequestedByHost(long messageId, string locationId, long state)
        {
            ServiceStatusChangeRequestedByHost?.Invoke(this, new HostChangeServiceStatusRequestEventArgs(messageId, locationId, state));
        }

        private static string BuildSlotMap(
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            int capacity)
        {
            var builder = new StringBuilder(capacity);

            for (int slotNo = 1; slotNo <= capacity; ++slotNo)
            {
                CarrierSlotMapStates state;

                if (map != null && map.TryGetValue(slotNo, out state))
                {
                    AppendSlotMapState(builder, state, slotNo);
                }
                else
                {
                    AppendSlotMapState(builder, CarrierSlotMapStates.Empty, slotNo);
                }
            }

            return builder.ToString();
        }

        private static void AppendSlotMapState(
            StringBuilder builder,
            CarrierSlotMapStates state,
            int slotNo)
        {
            int value = (int)state;

            if (value < 0 || value > 9)
            {
                throw new InvalidOperationException(
                    "Carrier slot map state must be a single digit value. SlotNo: "
                    + slotNo
                    + ", Value: "
                    + value);
            }

            builder.Append(value);
        }
        private static void ValidateCarrierCapacity(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity),
                    capacity,
                    "Carrier capacity must be greater than zero.");
            }
        }

        private static void ValidateSlotKeys<T>(
            IReadOnlyDictionary<int, T> source,
            int capacity,
            string parameterName)
        {
            if (source == null)
            {
                return;
            }

            foreach (int slotNo in source.Keys)
            {
                if (slotNo < 1 || slotNo > capacity)
                {
                    throw new ArgumentOutOfRangeException(
                        parameterName,
                        slotNo,
                        "Slot number must be between 1 and carrier capacity.");
                }
            }
        }
        private static string[] NormalizeStringArray(string[] source)
        {
            if (source == null || source.Length == 0)
            {
                return new string[0];
            }

            var values = new string[source.Length];

            for (int i = 0; i < source.Length; ++i)
            {
                values[i] = source[i] ?? string.Empty;
            }

            return values;
        }

        private static void ValidateArrayLength(
            string[] source,
            int maxLength,
            string parameterName)
        {
            if (source == null)
            {
                return;
            }

            if (source.Length > maxLength)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    source.Length,
                    "Array length must not exceed carrier capacity.");
            }
        }
        private static string[] BuildSlotStringArray(
            IReadOnlyDictionary<int, string> source,
            int capacity)
        {
            var values = new string[capacity];

            for (int slotNo = 1; slotNo <= capacity; ++slotNo)
            {
                string value;

                if (source != null && source.TryGetValue(slotNo, out value))
                {
                    values[slotNo - 1] = value ?? string.Empty;
                }
                else
                {
                    values[slotNo - 1] = string.Empty;
                }
            }

            return values;
        }
        private static int ResolveParallelCount(int firstLength, int secondLength, string firstName, string secondName)
        {
            if (firstLength != 0 && secondLength != 0 && firstLength != secondLength)
                throw new ArgumentException(string.Format("{0} and {1} length must match. {0}:{2}, {1}:{3}", firstName, secondName, firstLength, secondLength));

            return Math.Max(firstLength, secondLength);
        }

        private static int ResolveParallelCount(int firstLength, int secondLength, int thirdLength, string firstName, string secondName, string thirdName)
        {
            int max = Math.Max(firstLength, Math.Max(secondLength, thirdLength));

            if (firstLength != 0 && firstLength != max)
                throw new ArgumentException(string.Format("{0} length must match the other arrays. {0}:{1}", firstName, firstLength));

            if (secondLength != 0 && secondLength != max)
                throw new ArgumentException(string.Format("{0} length must match the other arrays. {0}:{1}", secondName, secondLength));

            if (thirdLength != 0 && thirdLength != max)
                throw new ArgumentException(string.Format("{0} length must match the other arrays. {0}:{1}", thirdName, thirdLength));

            return max;
        }
    }
}