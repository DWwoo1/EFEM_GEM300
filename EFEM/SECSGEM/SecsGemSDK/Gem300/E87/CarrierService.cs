using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Concurrent;

using EFEM.Defines.LoadPort;
using EFEM.Defines.CarrierManagement;

using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class CarrierService : ICarrierService
    {
        private readonly object _driverLock = new object();

        private ICarrierManagementDriver _driver;

        private readonly ConcurrentDictionary<string, ICarrierServiceCallback> _callbacks =
            new ConcurrentDictionary<string, ICarrierServiceCallback>();

        public bool IsDriverAttached
        {
            get
            {
                lock (_driverLock)
                {
                    return _driver != null;
                }
            }
        }

        public void AttachDriver(ICarrierManagementDriver driver)
        {
            if (driver == null)
                throw new ArgumentNullException("driver");

            lock (_driverLock)
            {
                if (object.ReferenceEquals(_driver, driver))
                    return;

                DetachDriverCore();

                _driver = driver;
                SubscribeDriverEvents();
            }
        }

        public void DetachDriver()
        {
            lock (_driverLock)
            {
                DetachDriverCore();
            }
        }

        private void DetachDriverCore()
        {
            if (_driver == null)
                return;

            UnsubscribeDriverEvents();
            _driver = null;
        }

        private ICarrierManagementDriver GetDriver()
        {
            lock (_driverLock)
            {
                if (_driver == null)
                    throw new InvalidOperationException("Carrier driver is not attached.");

                return _driver;
            }
        }

        public void RegisterCallback(
            string locationName,
            ICarrierServiceCallback callback)
        {
            if (string.IsNullOrWhiteSpace(locationName))
                return;

            if (callback == null)
                throw new ArgumentNullException("callback");

            _callbacks.TryAdd(locationName, callback);
        }

        public void UnregisterCallback(string locationName)
        {
            _callbacks.TryRemove(locationName, out _);
        }

        private void NotifyCallbacks(string locationName, Action<ICarrierServiceCallback> notify)
        {
            if (_callbacks.TryGetValue(locationName, out var callback))
            {
                notify(callback);
            }
        }

        public long NotifyCarrierDetection(string locationId, string carrierId, CarrierIdVerificationStates idVerificationResult, bool detectionStatus)
        {
            return _driver.NotifyCarrierDetection(locationId, carrierId, idVerificationResult, detectionStatus);
        }
        public long Bind(string locationId, string carrierId, string slotMap)
        {
            return _driver.ReqBind(locationId, carrierId, slotMap);
        }

        public long CancelBinding(string locationId, string carrierId)
        {
            return _driver.ReqCancelBind(locationId, carrierId);
        }

        //public long RequestCarrierIn(string locationId, string carrierId)
        //{
        //    return _driver.CmsReqCarrierIn(locationId, carrierId);
        //}

        //public long RequestCarrierOut(string locationId, string carrierId)
        //{
        //    return _driver.CmsReqCarrierOut(locationId, carrierId);
        //}

        public long RequestCarrierRecreate(string locationId, string carrierId)
        {
            return _driver.ReqCarrierReCreate(locationId, carrierId);
        }

        public long RequestCancelCarrier(string locationId, string carrierId)
        {
            return _driver.ReqCancelCarrier(locationId, carrierId);
        }

        public long RequestProceedCarrier(
            string locationId,
            string carrierId,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            IReadOnlyDictionary<int, string> lots,
            IReadOnlyDictionary<int, string> substrateNames,
            string usage)
        {
            return _driver.ReqProceedCarrier(locationId, carrierId, map, lots, substrateNames, usage);
        }

        public long AcknowledgeCarrierIn(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.RspCarrierIn(messageId, locationId, carrierId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeCarrierOut(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.RspCarrierOut(messageId, locationId, carrierId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeCancelCarrier(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.RspCancelCarrier(messageId, locationId, carrierId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeCarrierRelease(long messageId, string locationId, string carrierId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.RspCarrierRelease(messageId, locationId, carrierId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeChangeAccess(long messageId, long mode, long result, string[] locationIds, long[] errorCodes, string[] errorTexts)
        {
            return _driver.RspChangeAccess(messageId, mode, result, locationIds, errorCodes, errorTexts);
        }

        public long AcknowledgeChangeServiceStatus(long messageId, string locationId, long state, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.RspChangeServiceStatus(messageId, locationId, state, result, errorCodes, errorTexts);
        }

        public long SetLoadPortInfo(string locationId, LoadPortStateInformation state, string carrierId)
        {
            return _driver.SetLoadPortInfo(locationId, state, carrierId);
        }
        public long ChangeAccessMode(string locationId, LoadPortAccessMode mode)
        {
            return _driver.ChangeAccessMode(locationId, mode);
        }
        public long SetCarrierLocation(string locationId, string carrierId)
        {
            return _driver.SetCarrierLocation(locationId, carrierId);
        }
        public long SetCarrierMovement(string locationId, string carrierId)
        {
            return _driver.SetCarrierMovement(locationId, carrierId);
        }
        public long SetCarrierAccessing(string locationId, CarrierAccessStates state, string carrierId)
        {
            return _driver.SetCarrierAccessing(locationId, state, carrierId);
        }
        public long SetCarrierIdentifier(string locationId, string carrierId, VerificationResult result)
        {
            return _driver.SetCarrierIdentifier(locationId, carrierId, result);
        }
        public long SetCarrierIdStatus(string carrierId, CarrierIdVerificationStates state)
        {
            return _driver.SetCarrierIdStatus(carrierId, state);
        }
        public long SetSlotMap(string locationId, IReadOnlyDictionary<int, CarrierSlotMapStates> map, string carrierId, VerificationResult result)
        {
            return _driver.SetSlotMap(locationId, map, carrierId, result);
        }
        public long SetSlotMapStatus(string carrierId, CarrierSlotMapStates state)
        {
            return _driver.SetSlotMapStatus(carrierId, state);
        }
        public long SetCarrierInfo(string carrierId,
            string locationId,
            CarrierIdVerificationStates carrierIdStatus,
            CarrierSlotMapStates slotMapStatus,
            CarrierAccessStates accessingStatus,
            IReadOnlyDictionary<int, CarrierSlotMapStates> map,
            string[] lotIds, string[] substrateIds, string usage)
        {
            return _driver.SetCarrierInfo(
                carrierId, 
                locationId, 
                carrierIdStatus, 
                slotMapStatus,
                accessingStatus,
                map,
                lotIds,
                substrateIds,
                usage);
        }
        public long SetCarrierOutStart(string locationId, string carrierId)
        {
            return _driver.SetCarrierOutStart(locationId, carrierId);
        }
        public long SetSubstrateCount(string carrierId, long substrateCount)
        {
            return _driver.SetSubstrateCount(carrierId, substrateCount);
        }
        public long SetUsage(string carrierId, string usage)
        {
            return _driver.SetUsage(carrierId, usage);
        }
        public long SetMaterialArrived(string materialId)
        {
            return _driver.SetMaterialArrived(materialId);
        }
        public long SetPioSignal(string locationId, long signal, long state)
        {
            return _driver.SetPioSignal(locationId, signal, state);
        }
        public long SetReadyToLoad(string locationId)
        {
            return _driver.SetReadyToLoad(locationId);
        }
        public long SetReadyToUnload(string locationId)
        {
            return _driver.SetReadyToUnload(locationId);
        }
        //public long SetTransferReady(string locationId, long state) => _driver.CmsSetTransferReady(locationId, state);

        private void SubscribeDriverEvents()
        {
            _driver.CarrierInStarted += HandleCarrierInStarted;
            _driver.CarrierDeleted += HandleCarrierDeleted;
            _driver.CarrierTransferStateChanged += HandleTransferStateChanged;
            _driver.CarrierAccessModeChanged += HandleAccessModeChanged;
            _driver.CarrierVerificationSucceeded += HandleVerificationSucceeded;
            _driver.CarrierVerificationFailed += HandleVerificationFailed;
            _driver.CarrierVerificationResultWithoutRemote += HandleVerificationResultWithoutRemote;
            _driver.CarrierInRequestedByHost += HandleCarrierInRequestedByHost;
            _driver.CarrierOutRequestedByHost += HandleCarrierOutRequestedByHost;
            _driver.CarrierCancelRequestedByHost += HandleCarrierCancelRequestedByHost;
            _driver.AccessChangeRequestedByHost += HandleAccessChangeRequestedByHost;
            _driver.ServiceStatusChangeRequestedByHost += HandleServiceStatusChangeRequestedByHost;
        }
        private void UnsubscribeDriverEvents()
        {
            _driver.CarrierInStarted -= HandleCarrierInStarted;
            _driver.CarrierDeleted -= HandleCarrierDeleted;
            _driver.CarrierTransferStateChanged -= HandleTransferStateChanged;
            _driver.CarrierAccessModeChanged -= HandleAccessModeChanged;
            _driver.CarrierVerificationSucceeded -= HandleVerificationSucceeded;
            _driver.CarrierVerificationFailed -= HandleVerificationFailed;
            _driver.CarrierVerificationResultWithoutRemote -= HandleVerificationResultWithoutRemote;
            _driver.CarrierInRequestedByHost -= HandleCarrierInRequestedByHost;
            _driver.CarrierOutRequestedByHost -= HandleCarrierOutRequestedByHost;
            _driver.CarrierCancelRequestedByHost -= HandleCarrierCancelRequestedByHost;
            _driver.AccessChangeRequestedByHost -= HandleAccessChangeRequestedByHost;
            _driver.ServiceStatusChangeRequestedByHost -= HandleServiceStatusChangeRequestedByHost;
        }
        private void HandleCarrierInStarted(object sender, CarrierPortCarrierEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnCarrierInStarted(e));
        }

        private void HandleCarrierDeleted(object sender, CarrierDeletedEventArgs e)
        {
            // TODO : 
            //NotifyCallbacks(e.LocationId, callback => callback.OnCarrierDeleted(e));
        }

        private void HandleTransferStateChanged(object sender, LoadPortStateChangedEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnTransferStateChanged(e));
        }

        private void HandleAccessModeChanged(object sender, LoadPortStateChangedEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnAccessModeChanged(e));
        }

        private void HandleVerificationSucceeded(object sender, CarrierVerificationSucceededEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnVerificationSucceeded(e));
        }
        private void HandleVerificationResultWithoutRemote(object sender, CarrierVerificationResultWithoutRemoteArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnVerificationResultWithoutRemote(e));
        }

        private void HandleVerificationFailed(object sender, CarrierVerificationFailedEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnVerificationFailed(e));
        }

        private void HandleCarrierInRequestedByHost(object sender, HostCarrierRequestEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnCarrierInRequestedByHost(e));
        }

        private void HandleCarrierOutRequestedByHost(object sender, HostCarrierRequestEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnCarrierOutRequestedByHost(e));
        }

        private void HandleCarrierCancelRequestedByHost(object sender, HostCarrierRequestEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnCarrierCancelRequestedByHost(e));
        }

        private void HandleAccessChangeRequestedByHost(object sender, HostChangeAccessRequestEventArgs e)
        {
            if (e.LocationIds != null)
            {
                for (int i = 0; i < e.LocationIds.Length; ++i)
                {
                    NotifyCallbacks(e.LocationIds[i], callback => callback.OnAccessChangeRequestedByHost(e));
                }
            }
        }

        private void HandleServiceStatusChangeRequestedByHost(object sender, HostChangeServiceStatusRequestEventArgs e)
        {
            NotifyCallbacks(e.LocationId, callback => callback.OnServiceStatusChangeRequestedByHost(e));
        }
    }
}