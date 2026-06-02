using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking;
using EFEM.MaterialTracking.LocationHistory;

namespace EFEM.MaterialTracking.LocationState
{
    public sealed class LocationStateService
    {
        private readonly ISubstrateHistoryTracker _substrateHistory;
        private readonly ILocationEvent _locationEvent;
        private readonly Func<DateTime> _clock;

        public LocationStateService(
            ISubstrateHistoryTracker substrateHistory,
            ILocationEvent locationEvent,
            Func<DateTime> clock)
        {
            _substrateHistory = substrateHistory
                ?? throw new ArgumentNullException(nameof(substrateHistory));

            _locationEvent = locationEvent; // null 허용하면 옵션

            _clock = clock ?? (() => DateTime.UtcNow);
        }
        public void LoadHistoryFromStorage(string substrateKey)
        {
            _substrateHistory.RecoverFromChangesAsync(substrateKey);
        }
        public async Task<bool> OnSubstrateLocationStateChanged(SubstrateLocationStateChangedEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.LocationName) ||
                false == LocationServer.FindLocationById(e.LocationName, out var location))
            {
                return false;
            }

            switch (e.State)
            {
                case OccupancyState.Unoccupied:
                    {
                        //return await Vacate(location, e.SubstrateKey, e.Reason).ConfigureAwait(false);
                        var result = await Vacate(location, e.SubstrateKey, e.Reason).ConfigureAwait(false);
                        if (result)
                        {
                            var now = _clock();
                            await _substrateHistory.RecordRemoved(e.SubstrateKey, location.Id, location.LocationKind, now, e.Reason)
                                                   .ConfigureAwait(false);
                        }
                        return result;
                    }
                    
                case OccupancyState.Occupied:
                    {
                        //return await Occupy(location, e.SubstrateKey, e.Reason).ConfigureAwait(false);
                        var result = await Occupy(location, e.SubstrateKey, e.Reason).ConfigureAwait(false);
                        if (result)
                        {
                            var now = _clock();
                            await _substrateHistory.RecordCreated(e.SubstrateKey, location.Id, location.LocationKind, now, e.Reason)
                                                   .ConfigureAwait(false);
                        }

                        return result;
                    }
                    
                default:
                    return false;
            }
        }
        public async Task<bool> OnSubstrateLocationChanged(SubstrateLocationChangedEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.SourceLocationName) ||
                string.IsNullOrWhiteSpace(e.DestinationLocationName) ||
                false == LocationServer.FindLocationById(e.SourceLocationName, out var source) ||
                false == LocationServer.FindLocationById(e.DestinationLocationName, out var destination))
                return false;

            return await Transfer(
                source,
                destination,
                e.SubstrateKey,
                e.VacateReason,
                e.OccupyReason).ConfigureAwait(false);
        }
        public async Task<bool> OnSubstrateSwapped(SubstrateSwappedEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.FirstLocationName) ||
                string.IsNullOrWhiteSpace(e.SecondLocationName) ||
                false == LocationServer.FindLocationById(e.FirstLocationName, out var firstLoc) ||
                false == LocationServer.FindLocationById(e.SecondLocationName, out var secondLoc))
            {
                return false;
            }

            return await Swap(firstLoc, secondLoc, e.FirstKey, e.SecondKey, e.Reason).ConfigureAwait(false);
        }
        private async Task<bool> Occupy(Location location, string key, OccupancyChangeReason reason)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            var now = _clock();

            // 1) Location 상태 변경
            if (false == location.TryOccupy(key))
                return false;

            // 2) 자재 위치 갱신 -> SubstrateManager에서 갱신하도록 변경

            // 3) SubstrateKey 기준 체류 시작 기록
            if (reason != OccupancyChangeReason.Recovery)
            {
                _substrateHistory.StartStay(key, location, now, reason);

                // 3) Location 기준 이벤트 통지
                _locationEvent?.OnLocationEvent(
                    new LocationOccupancyEvent(
                        location,
                        key,
                        now,
                        OccupancyState.Occupied,
                        reason));
            }

            return true;
        }
        private async Task<bool> Vacate(Location location, string key, OccupancyChangeReason reason)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            var now = _clock();

            // 1) Location 상태 변경
            if (false == location.TryVacate(key))
                return false;

            // 2) SubstrateKey 기준 체류 종료 기록
            _substrateHistory.EndStay(key, location, now, reason);

            // 3) 제거 시 세팅 필요하면 추가 필요

            // 4) Location 기준 이벤트 통지
            _locationEvent?.OnLocationEvent(
                new LocationOccupancyEvent(
                    location,
                    key,
                    now,
                    OccupancyState.Unoccupied,
                    reason));

            return true;
        }
        private async Task<bool> Transfer(Location source, Location destination, string key, OccupancyChangeReason vacateReason, OccupancyChangeReason occupyReason)
        {
            if (source != null)
            {
                if (false == await Vacate(source, key, vacateReason))
                    return false;
            }

            if (source != null && destination != null)
            {
                var now = _clock();

                await _substrateHistory.RecordChanged(
                    key,
                    source.Id,
                    source.LocationKind,
                    destination.Id,
                    destination.LocationKind,
                    now,
                    occupyReason).ConfigureAwait(false);
            }

            if (destination != null)
            {
                if (false == await Occupy(destination, key, occupyReason))
                    return false;
            }

            return true;
        }
        private async Task<bool> Swap(Location firstLoc, Location secondLoc, string firstKey, string secondKey, OccupancyChangeReason reason)
        {
            if (firstLoc != null && secondLoc != null)
            {
                if (false == await Vacate(firstLoc, firstKey, reason) ||
                    false == await Vacate(secondLoc, secondKey, reason))
                    return false;

                if (false == await Occupy(secondLoc, firstKey, reason) ||
                    false == await Occupy(firstLoc, secondKey, reason))
                    return false;

                var now = _clock();

                await _substrateHistory.RecordChanged(firstKey, firstLoc.Id, firstLoc.LocationKind, secondLoc.Id, secondLoc.LocationKind, now, reason).ConfigureAwait(false);
                await _substrateHistory.RecordChanged(secondKey, secondLoc.Id, secondLoc.LocationKind, firstLoc.Id, firstLoc.LocationKind, now, reason).ConfigureAwait(false);
            }

            return true;
        }
    }
}
