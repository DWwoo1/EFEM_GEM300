using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.Defines.MaterialTracking;

namespace EFEM.MaterialTracking
{
    #region <Location History>
    public sealed class SubstrateLocationStateChangedEvent
    {
        public string SubstrateKey { get; }
        public string LocationName { get; }
        public OccupancyState State { get; }
        public OccupancyChangeReason Reason { get; }
        public SubstrateLocationStateChangedEvent(
          string substrateKey,
          string locationName,
          OccupancyState state,
          OccupancyChangeReason reason)
        {
            SubstrateKey = substrateKey ??
                throw new ArgumentNullException(nameof(substrateKey));

            LocationName = locationName;
            State = state;
            Reason = reason;
        }
    }
    public sealed class SubstrateLocationChangedEvent
    {
        public string SubstrateKey { get; }
        public string SourceLocationName { get; }
        public string DestinationLocationName { get; }
        public OccupancyChangeReason VacateReason { get; }
        public OccupancyChangeReason OccupyReason { get; }
        public SubstrateLocationChangedEvent(
            string substrateKey,
            string sourceLocationName,
            string destinationLocationName,
            OccupancyChangeReason vacateReason,
            OccupancyChangeReason occupyReason)
        {
            SubstrateKey = substrateKey ?? 
                throw new ArgumentNullException(nameof(substrateKey));

            SourceLocationName = sourceLocationName;
            DestinationLocationName = destinationLocationName;
            VacateReason = vacateReason;
            OccupyReason = occupyReason;
        }
    }
    public sealed class SubstrateSwappedEvent
    {
        public string FirstKey { get; }
        public string SecondKey { get; }
        public string FirstLocationName { get; }
        public string SecondLocationName { get; }
        public OccupancyChangeReason Reason { get; }

        public SubstrateSwappedEvent(
            string firstKey,
            string secondKey,
            string firstLocationName,
            string secondLocationName,
            OccupancyChangeReason reason)
        {
            FirstKey = firstKey;
            SecondKey = secondKey;
            FirstLocationName = firstLocationName;
            SecondLocationName = secondLocationName;
            Reason = reason;
        }
    }
    #endregion </Location History>

    #region <Processing History>
    public sealed class SubstrateProcessingStateChangedEvent
    {
        public string SubstrateKey { get; }
        public ProcessingStates OldState { get; }
        public ProcessingStates NewState { get; }
        public string ControlJobId { get; }
        public string ProcessJobId { get; }
        public string LocationId { get; }
        public string Description { get; }

        public SubstrateProcessingStateChangedEvent(
           string substrateKey,
           ProcessingStates oldState,
           ProcessingStates newState,
           string locationId,
           string controlJobId,
           string processJobId,
           string description)
        {
            SubstrateKey = substrateKey;
            OldState = oldState;
            NewState = newState;
            ControlJobId = controlJobId;
            ProcessJobId = processJobId;
            LocationId = locationId;
            Description = description;
        }
    }
    
    #endregion </Processing History>
}
