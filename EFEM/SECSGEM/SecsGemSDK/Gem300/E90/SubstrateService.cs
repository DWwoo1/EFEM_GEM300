using System;
using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using EFEM.Defines.MaterialTracking;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class SubstrateService : ISubstrateService
    {
        private readonly object _driverLock = new object();
        private ISubstrateTrackingDriver _driver;
        private readonly object _callbackLock = new object();
        private readonly List<ISubstrateServiceCallback> _callbacks = new List<ISubstrateServiceCallback>();

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

        public void AttachDriver(ISubstrateTrackingDriver driver)
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

        private ISubstrateTrackingDriver GetDriver()
        {
            lock (_driverLock)
            {
                if (_driver == null)
                    throw new InvalidOperationException("Substrate driver is not attached.");

                return _driver;
            }
        }

        public void RegisterCallback(ISubstrateServiceCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            lock (_callbackLock)
            {
                if (_callbacks.Contains(callback))
                    return;

                _callbacks.Add(callback);
            }
        }

        public void UnregisterCallback(ISubstrateServiceCallback callback)
        {
            if (callback == null)
                return;

            lock (_callbackLock)
            {
                _callbacks.Remove(callback);
            }
        }
        private void NotifyCallbacks(Action<ISubstrateServiceCallback> notify)
        {
            foreach (var item in _callbacks)
            {
                notify(item);
            }
        }
        public long InitializeLocation(string locationId, string substrateId)
        {
            return _driver.InitializeLocation(locationId, substrateId);
        }

        public long InitializeBatchLocation(string batchLocationId, string substrateId)
        {
            return _driver.InitializeBatchLocation(batchLocationId, substrateId);
        }

        public long SetTransport(string locationId, string substrateId, TransportStates transportState) 
        {
            return _driver.SetTransport(locationId, substrateId, transportState);
        }
        public long SetBatchTransport(string[] locationIds, string[] substrateIds, TransportStates transportState)
        {
            return _driver.SetBatchTransport(locationIds, substrateIds, transportState);
        }

        public long SetProcessing(string locationId, string substrateId, ProcessingStates processingState) 
        {
            return _driver.SetProcessing(locationId, substrateId, processingState);
        }
        public long SetBatchProcessing(string[] locationIds, string[] substrateIds, ProcessingStates processingState)
        {
            return _driver.SetBatchProcessing(locationIds, substrateIds, processingState);
        }

        public long SetInfo(string locationId, string substrateId, TransportStates transportState, ProcessingStates processingState, IdReadingStates readingState)
        {
            return _driver.SetInfo(locationId, substrateId, transportState, processingState, readingState);
        }
        public long SetReadResult(string locationId, string substrateId, string readSubstrateId, long result) 
        {
            return _driver.SetReadResult(locationId, substrateId, readSubstrateId, result);
        }
        public long NotifyMaterialArrived(string materialId)
        {
            return _driver.NotifyMaterialArrived(materialId);
        }

        public long Create(string locationId, string substrateId) 
        {
            return _driver.Create(locationId, substrateId);
        }
        public long Cancel(string locationId, string substrateId)
        {
            return _driver.Cancel(locationId, substrateId);
        }
        public long Proceed(string locationId, string substrateId, string readSubstrateId)
        {
            return _driver.Proceed(locationId, substrateId, readSubstrateId);
        }
        public long Delete(string locationId, string substrateId) 
        {
            return _driver.Delete(locationId, substrateId);
        }
        public long AcknowledgeCreate(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeCreate(messageId, locationId, substrateId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeCancel(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeCancel(messageId, locationId, substrateId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeUpdate(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeUpdate(messageId, locationId, substrateId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeDelete(long messageId, string locationId, string substrateId, long result)
        {
            return _driver.AcknowledgeDelete(messageId, locationId, substrateId, result);
        }

        public long Remove(string substrateId)
        {
            return _driver.Remove(substrateId);
        }

        public long RemoveAll()
        {
            return _driver.RemoveAll();
        }

        private void SubscribeDriverEvents()
        {
            _driver.SubstrateCreated += HandleCreated;
            _driver.SubstrateDeleted += HandleDeleted;
            _driver.SubstrateTransportChanged += HandleTransportChanged;
            _driver.SubstrateProcessingChanged += HandleProcessingChanged;
            _driver.SubstrateReadingChanged += HandleReadingChanged;
            _driver.SubstrateCreateRequestedByHost += HandleCreateRequestedByHost;
            _driver.SubstrateUpdateRequestedByHost += HandleUpdateRequestedByHost;
            _driver.SubstrateDeleteRequestedByHost += HandleDeleteRequestedByHost;
            _driver.SubstrateCancelRequestedByHost += HandleCancelRequestedByHost;
            _driver.SubstrateConfirmationDisplayed += HandleConfirmationDisplayed;
            _driver.SubstrateConfirmationSucceeded += HandleConfirmationSucceeded;
            _driver.SubstrateConfirmationFailed += HandleConfirmationFailed;
        }
        private void UnsubscribeDriverEvents()
        {
            _driver.SubstrateCreated -= HandleCreated;
            _driver.SubstrateDeleted -= HandleDeleted;
            _driver.SubstrateTransportChanged -= HandleTransportChanged;
            _driver.SubstrateProcessingChanged -= HandleProcessingChanged;
            _driver.SubstrateReadingChanged -= HandleReadingChanged;
            _driver.SubstrateCreateRequestedByHost -= HandleCreateRequestedByHost;
            _driver.SubstrateUpdateRequestedByHost -= HandleUpdateRequestedByHost;
            _driver.SubstrateDeleteRequestedByHost -= HandleDeleteRequestedByHost;
            _driver.SubstrateCancelRequestedByHost -= HandleCancelRequestedByHost;
            _driver.SubstrateConfirmationDisplayed -= HandleConfirmationDisplayed;
            _driver.SubstrateConfirmationSucceeded -= HandleConfirmationSucceeded;
            _driver.SubstrateConfirmationFailed -= HandleConfirmationFailed;
        }
        private void HandleCreated(object sender, SubstrateCreatedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCreated(e));
        }

        private void HandleDeleted(object sender, SubstrateDeletedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnDeleted(e));
        }

        private void HandleTransportChanged(object sender, SubstrateTransportStateChangedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnTransportChanged(e));
        }

        private void HandleProcessingChanged(object sender, SubstrateProcessingStateChangedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnProcessingChanged(e));
        }

        private void HandleReadingChanged(object sender, SubstrateReadingStateChangedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnReadingChanged(e));
        }

        private void HandleCreateRequestedByHost(object sender, SubstrateCreateRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCreateRequestedByHost(e));
        }

        private void HandleUpdateRequestedByHost(object sender, SubstrateUpdateRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnUpdateRequestedByHost(e));
        }

        private void HandleDeleteRequestedByHost(object sender, SubstrateDeleteRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnDeleteRequestedByHost(e));
        }

        private void HandleCancelRequestedByHost(object sender, SubstrateCancelRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCancelRequestedByHost(e));
        }

        private void HandleConfirmationDisplayed(object sender, SubstrateConfirmEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnConfirmationDisplayed(e));
        }

        private void HandleConfirmationSucceeded(object sender, SubstrateConfirmEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnConfirmationSucceeded(e));
        }

        private void HandleConfirmationFailed(object sender, SubstrateConfirmFailedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnConfirmationFailed(e));
        }
    }
}