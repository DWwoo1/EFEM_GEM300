using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.MaterialTracking;
using EFEM.MaterialTracking.LocationHistory.Storage;

namespace EFEM.MaterialTracking.LocationHistory
{
    public sealed class StayInProgress
    {
        public StayInProgress(
            string substrateKey,
            string locationName,
            string locationType,
            DateTime startTime,
            string startAction)
        {
            SubstrateKey = substrateKey;
            LocationName = locationName;
            LocationType = locationType;
            StartTime = startTime;
            StartAction = startAction;
        }

        public string SubstrateKey { get; }
        public string LocationName { get; }
        public string LocationType { get; }
        public DateTime StartTime { get; }
        public string StartAction { get; }
    }
    public sealed class SubstrateStaySegment
    {
        public SubstrateStaySegment(
            string substrateKey,
            string locationName,
            string locationType,
            DateTime startTime,
            DateTime endTime,
            string startAction,
            string endAction)
        {
            SubstrateKey = substrateKey;
            LocationName = locationName;
            LocationType = locationType;
            StartTime = startTime;
            EndTime = endTime;
            StartAction = startAction ?? string.Empty;
            EndAction = endAction ?? string.Empty;
        }

        public string SubstrateKey { get; }
        public string LocationName { get; }
        public string LocationType { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public string StartAction { get; }
        public string EndAction { get; }
    }

    /// <summary>
    /// SubstrateKey 기준 체류 시작/종료를 받아서, 완성된 구간을 Storage로 내보내는 트래커.
    /// </summary>
    public interface ISubstrateHistoryTracker
    {
        Task RecoverFromChangesAsync(string substrateKey);
        void StartStay(string substrateKey, Location location,
                       DateTime startTime, OccupancyChangeReason reason);

        void EndStay(string substrateKey, Location location,
                     DateTime endTime, OccupancyChangeReason reason);

        StayInProgress GetCurrentStayedInfo(string substrateKey);
        IReadOnlyList<SubstrateStaySegment> GetStayHistory(string substrateKey);
        void ClearStayHistory(string substrateKey);

        Task RecordChanged(string substrateKey, string fromLocationName, ModuleType fromLocationKind, string toLocationName, ModuleType toLocationKind, DateTime changeTime, OccupancyChangeReason reason);
        Task RecordCreated(string substrateKey, string toLocationName, ModuleType toLocationKind, DateTime time, OccupancyChangeReason reason);
        Task RecordRemoved(string substrateKey, string fromLocationName, ModuleType fromLocationKind, DateTime time, OccupancyChangeReason reason);
    }

    public sealed class SubstrateHistoryTracker : ISubstrateHistoryTracker, ISubstrateEventObserver
    {
        private readonly object _syncRoot = new object();
        private readonly Dictionary<string, StayInProgress> _currentStayBySubstrateKey;
        private readonly Dictionary<string, List<SubstrateStaySegment>> _historyBySubstrateKey;
        private readonly ISubstrateLocationHistoryStorage _storage;
        private readonly Dictionary<string, ModuleType> _locationTypes;
        public SubstrateHistoryTracker(ISubstrateLocationHistoryStorage storage)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            _storage = storage;
            _currentStayBySubstrateKey = new Dictionary<string, StayInProgress>();
            _historyBySubstrateKey = new Dictionary<string, List<SubstrateStaySegment>>();
        }
        public async Task RecoverFromChangesAsync(string substrateKey)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            // 1) Change 로그 읽기
            var changes = await _storage.ReadChangesAsync(substrateKey);
            if (changes == null || changes.Count == 0)
                return;

            // 2) 기존 메모리 상태(해당 key) 제거 후 재생
            lock (_syncRoot)
            {
                ClearStayHistory(substrateKey);
                ClearOpenStayBySubstrateKey(substrateKey);
            }

            // 3) 재생
            foreach (var c in changes)
            {
                if (c == null) continue;

                // "" -> null 정규화 (FK 이슈 재발 방지 + 안정성)
                var from = string.IsNullOrWhiteSpace(c.FromLocationName) ? null : c.FromLocationName;
                var to = string.IsNullOrWhiteSpace(c.ToLocationName) ? null : c.ToLocationName;

                ApplyChange(substrateKey, from, to, c.ChangeTime, c.Reason);
            }
        }
        private void ApplyChange(
            string substrateKey,
            string fromLocationName,
            string toLocationName,
            DateTime changeTime,
            string reason)
        {
            lock (_syncRoot)
            {
                _currentStayBySubstrateKey.TryGetValue(substrateKey, out var currentOpen);

                if (currentOpen != null)
                {
                    // 구간 닫기
                    var seg = new SubstrateStaySegment(
                        substrateKey: currentOpen.SubstrateKey,
                        locationName: currentOpen.LocationName,
                        locationType: currentOpen.LocationType,
                        startTime: currentOpen.StartTime,
                        endTime: changeTime,
                        startAction: currentOpen.StartAction,
                        endAction: reason ?? string.Empty);

                    if (false == _historyBySubstrateKey.TryGetValue(substrateKey, out var list))
                    {
                        list = new List<SubstrateStaySegment>();
                        _historyBySubstrateKey.Add(substrateKey, list);
                    }

                    list.Add(seg);

                    // open 제거
                    _currentStayBySubstrateKey.Remove(substrateKey);
                }

                // 새 open 시작 (to가 있을 때만)
                if (toLocationName != null)
                {
                    // locationType은 Change 로그만으로는 모를 수 있음
                    // 최소 변경: "Unknown" 사용 (필요하면 LocationServer로 lookup 가능)
                    var locationType = "Unknown";

                    // startAction도 Change에서 별도로 없으면 reason을 넣어도 됨
                    var startAction = reason ?? string.Empty;

                    _currentStayBySubstrateKey[substrateKey] = new StayInProgress(
                        substrateKey: substrateKey,
                        locationName: toLocationName,
                        locationType: locationType,
                        startTime: changeTime,
                        startAction: startAction);
                }
            }
        }
        public StayInProgress GetCurrentStayedInfo(string substrateKey)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                return null;

            lock (_syncRoot)
            {
                _currentStayBySubstrateKey.TryGetValue(substrateKey, out var s);

                return s;
            }
        }

        public void StartStay(string substrateKey, Location location, DateTime startTime, OccupancyChangeReason reason)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            lock (_syncRoot)
            {
                if (false == _currentStayBySubstrateKey.ContainsKey(substrateKey))
                {
                    _currentStayBySubstrateKey[substrateKey] = new StayInProgress(
                        substrateKey,
                        location.Id,
                        location.LocationKind.ToString(),
                        startTime,
                        reason.ToString());
                }
                //_open[key] = new StayInProgress(
                //    substrateKey,
                //    location.Name,
                //    location.LocationKind.ToString(),
                //    startTime,
                //    reason.ToString());
            }
        }
        public void EndStay(string substrateKey, Location location, DateTime endTime, OccupancyChangeReason reason)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            StayInProgress inProgress;

            lock (_syncRoot)
            {
                if (false == _currentStayBySubstrateKey.TryGetValue(substrateKey, out inProgress))
                {
                    // 시작 없이 종료가 들어온 경우: 로직/복구 문제
                    // 여기서는 무시. 필요하면 로그 추가.
                    return;
                }

                if (false == string.Equals(inProgress.LocationName, location.Id, StringComparison.OrdinalIgnoreCase))
                    return;

                _currentStayBySubstrateKey.Remove(substrateKey);

                // (신규) 메모리 히스토리에 구간 누적
                var seg = new SubstrateStaySegment(
                    substrateKey: inProgress.SubstrateKey,
                    locationName: inProgress.LocationName,
                    locationType: inProgress.LocationType,
                    startTime: inProgress.StartTime,
                    endTime: endTime,
                    startAction: inProgress.StartAction,
                    endAction: reason.ToString());

                if (false == _historyBySubstrateKey.TryGetValue(substrateKey, out var list))
                {
                    list = new List<SubstrateStaySegment>();
                    _historyBySubstrateKey.Add(substrateKey, list);
                }

                list.Add(seg);
            }

            //var entry = new SubstrateStayHistoryItem(
            //    substrateKey: inProgress.SubstrateKey,
            //    locationName: inProgress.LocationName,
            //    locationType: inProgress.LocationType,
            //    stayStartTime: inProgress.StartTime,
            //    stayEndTime: endTime,
            //    startAction: inProgress.StartAction,
            //    reason.ToString());

            //_storage.RecordStay(entry);
        }
        public IReadOnlyList<SubstrateStaySegment> GetStayHistory(string substrateKey)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                return Array.Empty<SubstrateStaySegment>();

            lock (_syncRoot)
            {
                if (_historyBySubstrateKey.TryGetValue(substrateKey, out var list))
                    return list.ToArray(); // snapshot
            }

            return Array.Empty<SubstrateStaySegment>();
        }

        public void ClearStayHistory(string substrateKey)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                return;

            lock (_syncRoot)
            {
                _historyBySubstrateKey.Remove(substrateKey);
            }
        }

        public Task RecordChanged(
            string substrateKey,
            string fromLocationName,
            ModuleType fromLocationKind,
            string toLocationName,
            ModuleType toLocationKind,
            DateTime changeTime,
            OccupancyChangeReason reason)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            if (reason == OccupancyChangeReason.Recovery)
                return Task.CompletedTask;

            var entry = new SubstrateLocationChangeItem(
                substrateKey: substrateKey,
                fromLocationName: fromLocationName,
                fromLocationKind: fromLocationKind,
                toLocationName: toLocationName,
                toLocationKind: toLocationKind,
                changeTime: changeTime,
                reason: reason.ToString());

            _storage.RecordChange(entry);

            return Task.CompletedTask;
        }

        public Task RecordCreated(string substrateKey, string toLocationName, ModuleType toLocationKind, DateTime time, OccupancyChangeReason reason)
        {
            if (reason == OccupancyChangeReason.Recovery)
                return Task.CompletedTask;

            var entry = new SubstrateLocationChangeItem(substrateKey, null, ModuleType.Unknown, toLocationName, toLocationKind, time, reason.ToString());
            _storage.RecordChange(entry);
            return Task.CompletedTask;
        }
        public Task RecordRemoved(string substrateKey, string fromLocationName, ModuleType fromLocationKind, DateTime time, OccupancyChangeReason reason)
        {
            if (reason == OccupancyChangeReason.Recovery)
                return Task.CompletedTask;

            var entry = new SubstrateLocationChangeItem(substrateKey, fromLocationName, fromLocationKind, null, ModuleType.Unknown, time, reason.ToString());
            _storage.RecordChange(entry);

            return Task.CompletedTask;
        }
        public void OnSubstrateCreated(string substrateKey)
        {
            // 생성 이벤트에서는 굳이 Clear할 필요 없음(원하면 초기화/보정 가능)
            // 최소 변경: 아무 것도 하지 않음
        }
        private void ClearOpenStayBySubstrateKey(string substrateKey)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                return;

            lock (_syncRoot)
            {
                _currentStayBySubstrateKey.Remove(substrateKey);
            }
        }
        public void OnSubstrateArchived(string substrateKey, string destinationPath)
        {
            // "아카이브 이동 전까지 메모리에 히스토리 유지" 요구사항 충족:
            // 아카이브가 완료되었음을 콜백으로 받는 순간, 메모리 히스토리를 비운다.
            ClearStayHistory(substrateKey);

            // 혹시 아직 open stay가 남아있으면 같이 제거(선택: 권장)
            // ※ 아카이브 시점에 open이 남아있는 건 정상일 수 있지만,
            //   '아카이브 이후에는 메모리에서 완전히 잊는다'가 목표면 clear가 더 깔끔함
            ClearOpenStayBySubstrateKey(substrateKey);
        }

        public void OnSubstrateDeleted(string substrateKey)
        {
            // 완전 삭제 시에도 메모리 정리
            ClearStayHistory(substrateKey);
            ClearOpenStayBySubstrateKey(substrateKey);
        }
    }
}
