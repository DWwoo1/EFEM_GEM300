using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking.ProcessingHistory.Storage;

namespace EFEM.MaterialTracking.ProcessingHistory
{
    public sealed class SubstrateProcessingService
    {
        #region <Constructors>

        public SubstrateProcessingService(
            ISubstrateProcessingHistoryStorage historyStorage,
            Func<DateTime> clock)
        {
            if (historyStorage == null) 
                throw new ArgumentNullException(nameof(historyStorage));

            _historyStorage = historyStorage;
            _clock = clock ?? (() => DateTime.UtcNow);
        }
        #endregion </Constructors>

        #region <Fields>

        private readonly ISubstrateProcessingHistoryStorage _historyStorage;
        private readonly Func<DateTime> _clock;

        #endregion </Fields>

        #region <Methods>
        public bool OnSubstrateLocationStateChanged(SubstrateProcessingStateChangedEvent e)
        {
            ChangeState(
                e.SubstrateKey,
                e.OldState,
                e.NewState,
                e.LocationId,
                e.ControlJobId,
                e.ProcessJobId,
                e.Description);

            return true;
        }

        private void ChangeState(
            string substrateKey,
            ProcessingStates oldState,
            ProcessingStates newState,
            string locationId,
            string controlJobId,
            string processJobId,
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            // 2) 히스토리 레코드 작성
            var item = new SubstrateProcessingHistoryItem
            {
                SubstrateKey = substrateKey,
                EventTime = _clock(),
                OldState = oldState.ToString(),
                NewState = newState.ToString(),
                ControlJobId = controlJobId,
                ProcessJobId = processJobId,
                LocationId = locationId,
                Description = description
            };

            // 3) 히스토리 저장
            _historyStorage.Record(item);
        }
        #endregion </Methods>
    }
}
