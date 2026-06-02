using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;

namespace EFEM.MaterialTracking.LocationState
{
    public sealed class LocationOccupancyEvent
    {
        public LocationOccupancyEvent(
            Location location,
            string substrateKey,
            DateTime eventTime,
            OccupancyState eventType,
            OccupancyChangeReason reason)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
            SubstrateKey = substrateKey ?? string.Empty;
            EventTime = eventTime;
            EventType = eventType;
            Reason = reason;
        }

        public Location Location { get; }
        public string SubstrateKey { get; }
        public DateTime EventTime { get; }
        public OccupancyState EventType { get; }
        public OccupancyChangeReason Reason { get; }
    }

    public interface ILocationEvent
    {
        void OnLocationEvent(LocationOccupancyEvent ev);
    }

    /// <summary>아무 것도 하지 않는 기본 구현 (옵션)</summary>
    public sealed class NullLocationEvent : ILocationEvent
    {
        public void OnLocationEvent(LocationOccupancyEvent ev)
        {
            // no-op
        }
    }
}
