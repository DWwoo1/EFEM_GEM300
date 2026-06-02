using System;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

using EFEM.Defines.Common;
using EFEM.Defines.MaterialTracking;

using XGEM300PRO.Library;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class XGemSubstrateTrackingDriver : ISubstrateTrackingDriver
    {
        private readonly XGem300ProW _driver;

        public XGemSubstrateTrackingDriver(XGem300ProW driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            SubscribeDriverEvents();
        }

        public event EventHandler<SubstrateCreatedEventArgs> SubstrateCreated;
        public event EventHandler<SubstrateDeletedEventArgs> SubstrateDeleted;
        public event EventHandler<SubstrateTransportStateChangedEventArgs> SubstrateTransportChanged;
        public event EventHandler<SubstrateProcessingStateChangedEventArgs> SubstrateProcessingChanged;
        public event EventHandler<SubstrateReadingStateChangedEventArgs> SubstrateReadingChanged;
        public event EventHandler<SubstrateCreateRequestedEventArgs> SubstrateCreateRequestedByHost;
        public event EventHandler<SubstrateUpdateRequestedEventArgs> SubstrateUpdateRequestedByHost;
        public event EventHandler<SubstrateDeleteRequestedEventArgs> SubstrateDeleteRequestedByHost;
        public event EventHandler<SubstrateCancelRequestedEventArgs> SubstrateCancelRequestedByHost;
        public event EventHandler<SubstrateConfirmEventArgs> SubstrateConfirmationDisplayed;
        public event EventHandler<SubstrateConfirmEventArgs> SubstrateConfirmationSucceeded;
        public event EventHandler<SubstrateConfirmFailedEventArgs> SubstrateConfirmationFailed;

        public long InitializeLocation(string locationId, string substrateId)
        {
            return _driver.STSSetSubstLocationInfo(locationId ?? string.Empty, substrateId ?? string.Empty);
        }

        public long InitializeBatchLocation(string batchLocationId, string substrateId)
        {
            return _driver.STSSetBatchLocationInfo(batchLocationId ?? string.Empty, substrateId ?? string.Empty);
        }

        public long SetTransport(string locationId, string substrateId, TransportStates transportState)
        {
            return _driver.STSSetTransport(locationId ?? string.Empty, substrateId ?? string.Empty, (long)transportState);
        }

        public long SetBatchTransport(string[] locationIds, string[] substrateIds, TransportStates transportState)
        {
            string[] safeLocations = locationIds ?? new string[0];
            string[] safeSubstrates = substrateIds ?? new string[0];

            EnsureEqualLength(safeLocations.Length, safeSubstrates.Length, nameof(locationIds), nameof(substrateIds));

            return _driver.STSSetBatchTransport(safeLocations.Length, safeLocations, safeSubstrates, (long)transportState);
        }

        public long SetProcessing(string locationId, string substrateId, ProcessingStates processingState)
        {
            return _driver.STSSetProcessing(locationId ?? string.Empty, substrateId ?? string.Empty, (long)processingState);
        }

        public long SetBatchProcessing(string[] locationIds, string[] substrateIds, ProcessingStates processingState)
        {
            string[] safeLocations = locationIds ?? new string[0];
            string[] safeSubstrates = substrateIds ?? new string[0];

            EnsureEqualLength(safeLocations.Length, safeSubstrates.Length, nameof(locationIds), nameof(substrateIds));

            return _driver.STSSetBatchProcessing(safeLocations.Length, safeLocations, safeSubstrates, (long)processingState);
        }

        public long SetInfo(string locationId, string substrateId, TransportStates transportState, ProcessingStates processingState, IdReadingStates readingState)
        {            
            var result = _driver.STSSetSubstrateInfo(
                locationId ?? string.Empty,
                substrateId ?? string.Empty,
                (long)transportState,
                (long)processingState,
                (long)readingState);

            return result;
        }

        public long SetReadResult(string locationId, string substrateId, string readSubstrateId, long result)
        {
            return _driver.STSSetSubstrateID(locationId ?? string.Empty, substrateId ?? string.Empty, readSubstrateId ?? string.Empty, result);
        }

        public long NotifyMaterialArrived(string materialId)
        {
            return _driver.STSSetMaterialArrived(materialId ?? string.Empty);
        }

        public long Create(string locationId, string substrateId)
        {
            return _driver.STSReqCreateSubstrate(locationId ?? string.Empty, substrateId ?? string.Empty);
        }

        public long Cancel(string locationId, string substrateId)
        {
            return _driver.STSReqCancelSubstrate(locationId ?? string.Empty, substrateId ?? string.Empty);
        }

        public long Proceed(string locationId, string substrateId, string readSubstrateId)
        {
            return _driver.STSReqProceedSubstrate(locationId ?? string.Empty, substrateId ?? string.Empty, readSubstrateId ?? string.Empty);
        }

        public long Delete(string locationId, string substrateId)
        {
            return _driver.STSReqDeleteSubstrate(locationId ?? string.Empty, substrateId ?? string.Empty);
        }

        public long AcknowledgeCreate(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.STSRspCreateSubstrate(
                messageId,
                locationId ?? string.Empty,
                substrateId ?? string.Empty,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeCancel(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.STSRspCancelSubstrate(
                messageId,
                locationId ?? string.Empty,
                substrateId ?? string.Empty,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeUpdate(long messageId, string locationId, string substrateId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.STSRspUpdateSubstrate(
                messageId,
                locationId ?? string.Empty,
                substrateId ?? string.Empty,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeDelete(long messageId, string locationId, string substrateId, long result)
        {
            return _driver.STSRspDeleteSubstrate(
                messageId,
                locationId ?? string.Empty,
                substrateId ?? string.Empty,
                result,
                0,
                new long[0],
                new string[0]);
        }

        public long Remove(string substrateId)
        {
            return _driver.STSDelSubstrateInfo(substrateId ?? string.Empty);
        }

        public long RemoveAll()
        {
            return _driver.STSDelAllSubstrateInfo();
        }

        private void SubscribeDriverEvents()
        {
            _driver.OnSTSSubstrateCreated += HandleCreated;
            _driver.OnSTSSubstrateDeleted += HandleDeleted;
            _driver.OnSTSTransportChanged += HandleTransportChanged;
            _driver.OnSTSProcessingChanged += HandleProcessingChanged;
            _driver.OnSTSReadingChanged += HandleReadingChanged;
            _driver.OnSTSReqCreateSubstrate += HandleCreateRequestedByHost;
            _driver.OnSTSReqUpdateSubstrate += HandleUpdateRequestedByHost;
            _driver.OnSTSReqDeleteSubstrate += HandleDeleteRequestedByHost;
            _driver.OnSTSReqCancelSubstrate += HandleCancelRequestedByHost;
            _driver.OnSTSConfirmDisplay += HandleConfirmationDisplayed;
            _driver.OnSTSConfirmSucceeded += HandleConfirmationSucceeded;
            _driver.OnSTSConfirmFailed += HandleConfirmationFailed;
        }

        private void HandleCreated(string locationId, string substrateId, long substrateType, long transportState, long processingState)
        {
            SubstrateCreated?.Invoke(
                this,
                new SubstrateCreatedEventArgs(
                    locationId,
                    substrateId,
                    (EFEM.Defines.Common.MaterialFormat)substrateType,
                    (TransportStates)transportState,
                    (ProcessingStates)processingState));
        }

        private void HandleDeleted(string substrateId)
        {
            SubstrateDeleted?.Invoke(this, new SubstrateDeletedEventArgs(substrateId));
        }

        private void HandleTransportChanged(string locationId, string substrateId, long state)
        {
            SubstrateTransportChanged?.Invoke(
                this, 
                new SubstrateTransportStateChangedEventArgs(
                    locationId,
                    substrateId, 
                    (TransportStates)state));
        }

        private void HandleProcessingChanged(string locationId, string substrateId, long state)
        {
            SubstrateProcessingChanged?.Invoke(
                this, 
                new SubstrateProcessingStateChangedEventArgs(
                    locationId, 
                    substrateId, 
                    (ProcessingStates)state));
        }

        private void HandleReadingChanged(string locationId, string substrateId, long state)
        {
            SubstrateReadingChanged?.Invoke(
                this, 
                new SubstrateReadingStateChangedEventArgs(
                    locationId, 
                    substrateId, 
                    (IdReadingStates)state));
        }

        private void HandleCreateRequestedByHost(long messageId, string locationId, string substrateId)
        {
            SubstrateCreateRequestedByHost?.Invoke(this, new SubstrateCreateRequestedEventArgs(messageId, locationId, substrateId));
        }

        private void HandleUpdateRequestedByHost(long messageId, string locationId, string substrateId, long substrateType, long transportState, long processingState)
        {
            SubstrateUpdateRequestedByHost?.Invoke(
                this,
                new SubstrateUpdateRequestedEventArgs(
                    messageId,
                    locationId,
                    substrateId,
                    (EFEM.Defines.Common.MaterialFormat)substrateType,
                    (TransportStates)transportState,
                    (ProcessingStates)processingState));
        }

        private void HandleDeleteRequestedByHost(long messageId, string locationId, string substrateId)
        {
            SubstrateDeleteRequestedByHost?.Invoke(this, new SubstrateDeleteRequestedEventArgs(messageId, locationId, substrateId));
        }

        private void HandleCancelRequestedByHost(long messageId, string locationId, string substrateId)
        {
            SubstrateCancelRequestedByHost?.Invoke(this, new SubstrateCancelRequestedEventArgs(messageId, locationId, substrateId));
        }

        private void HandleConfirmationDisplayed(string locationId, string substrateId, string readSubstrateId)
        {
            SubstrateConfirmationDisplayed?.Invoke(this, new SubstrateConfirmEventArgs(locationId, substrateId, readSubstrateId));
        }

        private void HandleConfirmationSucceeded(string locationId, string substrateId, string readSubstrateId)
        {
            SubstrateConfirmationSucceeded?.Invoke(this, new SubstrateConfirmEventArgs(locationId, substrateId, readSubstrateId));
        }

        private void HandleConfirmationFailed(string locationId, string substrateId)
        {
            SubstrateConfirmationFailed?.Invoke(this, new SubstrateConfirmFailedEventArgs(locationId, substrateId));
        }

        private static void EnsureEqualLength(int first, int second, string firstName, string secondName)
        {
            if (first != second)
            {
                throw new ArgumentException(
                    string.Format(
                        "{0} and {1} length must match. {0}:{2}, {1}:{3}",
                        firstName,
                        secondName,
                        first,
                        second));
            }
        }
    }
}