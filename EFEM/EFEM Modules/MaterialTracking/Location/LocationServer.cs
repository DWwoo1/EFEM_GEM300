using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

//using EFEM.MaterialTracking.LocationServer.LocationType;
using EFEM.Defines.Common;
using EFEM.Defines.LoadPort;
using EFEM.Defines.AtmRobot;
using EFEM.Defines.MaterialTracking;
using EFEM.Modules;

namespace EFEM.MaterialTracking
{
    public static class LocationNameConverter
    {
        public static string CreateInitialNameAtLoadPort(string name, int slot)
        {
            return $"{name}.{slot:d2}";
        }
        public static string CreateInitialNameAtRobot(string robotName, RobotArmTypes arm)
        {
            return $"{robotName}.{arm}";
        }

        public static string CreateInitialNameAtProcessModule(string locationName)
        {
            return locationName;
        }
    }
    public static class LocationServer
    {
        #region <Fields>
        private static readonly Dictionary<int, Dictionary<int, LoadPortLocation>> _loadPortSlotLocations = new Dictionary<int, Dictionary<int, LoadPortLocation>>();
        private static readonly Dictionary<string, Dictionary<RobotArmTypes, RobotLocation>> _robotLocations = new Dictionary<string, Dictionary<RobotArmTypes, RobotLocation>>();
        private static readonly Dictionary<string, Dictionary<string, ProcessModuleLocation>> _processModuleLocations = new Dictionary<string, Dictionary<string, ProcessModuleLocation>>();
        private static readonly Dictionary<string, string> _processModuleEntrys = new Dictionary<string, string>();

        private static readonly ConcurrentDictionary<string, Location> _byId = new ConcurrentDictionary<string, Location>();
        #endregion </Fields>

        #region <Methods>

        #region <Add Location>
        public static IReadOnlyList<Location> AddLoadPortLocations(
            string lp,
            int portId,
            int slotCount)
        {
            if (string.IsNullOrWhiteSpace(lp))
                throw new ArgumentException("LoadPort name is required.", nameof(lp));

            if (slotCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(slotCount), "Slot count must be greater than zero.");

            var slots = new Dictionary<int, LoadPortLocation>();
            var createdLocations = new List<Location>();

            for (int i = 1; i <= slotCount; ++i)
            {
                var location = new LoadPortLocation(lp, portId, i);

                slots[i] = location;
                createdLocations.Add(location);
            }

            _loadPortSlotLocations[portId] = slots;

            foreach (var location in createdLocations)
            {
                _byId[location.Id] = location;
            }

            return createdLocations.ToArray();
        }
        public static IReadOnlyList<Location> AddRobotLocations(string rb)
        {
            if (string.IsNullOrWhiteSpace(rb))
                throw new ArgumentException("Robot name is required.", nameof(rb));

            var locs = new Dictionary<RobotArmTypes, RobotLocation>();
            var createdLocations = new List<Location>();

            foreach (RobotArmTypes arm in Enum.GetValues(typeof(RobotArmTypes)))
            {
                if (arm == RobotArmTypes.All)
                    continue;

                var location = new RobotLocation(rb, arm);

                locs[arm] = location;
                createdLocations.Add(location);
            }

            _robotLocations[rb] = locs;

            foreach (var location in createdLocations)
            {
                _byId[location.Id] = location;
            }

            return createdLocations.ToArray();
        }
        public static IReadOnlyList<Location> AddProcessModuleLocations(
            string pm,
            IReadOnlyDictionary<string, string> locationsByEntry,
            IReadOnlyDictionary<string, int> locationCapacity)
        {
            if (string.IsNullOrWhiteSpace(pm))
                throw new ArgumentException("Process module name is required.", nameof(pm));

            if (locationsByEntry == null)
                throw new ArgumentNullException(nameof(locationsByEntry));

            if (locationCapacity == null)
                throw new ArgumentNullException(nameof(locationCapacity));

            var locs = new Dictionary<string, ProcessModuleLocation>();
            var createdLocations = new List<Location>();

            foreach (var item in locationsByEntry)
            {
                var locName = item.Value;

                if (string.IsNullOrWhiteSpace(locName))
                    continue;

                if (locs.ContainsKey(locName))
                    continue;

                int capacity;
                if (false == locationCapacity.TryGetValue(locName, out capacity))
                    continue;

                var location = new ProcessModuleLocation(pm, locName, capacity);

                locs[locName] = location;
                createdLocations.Add(location);
            }

            _processModuleLocations[pm] = locs;

            foreach (var location in createdLocations)
            {
                _byId[location.Id] = location;
            }

            foreach (var item in locationsByEntry)
            {
                if (string.IsNullOrWhiteSpace(item.Key))
                    continue;

                if (string.IsNullOrWhiteSpace(item.Value))
                    continue;

                _processModuleEntrys[item.Key] = item.Value;
            }

            return createdLocations.ToArray();
        }
        #endregion </Add Location>

        #region <Get Location>
        public static IEnumerable<Location> GetLocations()
        {
            return _byId.OrderBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase).Select(kvp => kvp.Value).ToList();
        }
        public static bool GetLocationById(string locationId, out Location location)
        {
            return _byId.TryGetValue(locationId, out location) && location != null;
        }
        public static bool FindLocationById(string locationId, out Location loc)
        {
            if (_processModuleEntrys.ContainsKey(locationId))
            {
                _processModuleEntrys.TryGetValue(locationId, out var locId);

                return _byId.TryGetValue(locId, out loc);
            }

            return _byId.TryGetValue(locationId, out loc);
        }
        public static string GetLoadPortLocationId(int portId)
        {
            if (false == _loadPortSlotLocations.ContainsKey(portId))
                return string.Empty;

            return LoadPortManager.Instance.GetCurrentLocationId(portId);
        }
        public static bool GetLoadPortLocation(int portId, int slot, out LoadPortLocation location)
        {
            location = null;

            if (false == _loadPortSlotLocations.TryGetValue(portId, out var slots))
                return false;

            return _loadPortSlotLocations[portId].TryGetValue(slot, out location) && location != null;
        }
        public static bool GetLoadPortLocations(int portId, out Dictionary<int, LoadPortLocation> locs)
        {
            return _loadPortSlotLocations.TryGetValue(portId, out locs);
        }
        public static bool GetProcessModuleLocation(string moduleName, string targetLocationName, out ProcessModuleLocation location)
        {
            location = null;
            if (false == _processModuleLocations.ContainsKey(moduleName))
                return false;

            return _processModuleLocations[moduleName].TryGetValue(targetLocationName, out location) && location != null;
        }
        public static bool GetRobotLocation(string robotName, RobotArmTypes armType, out RobotLocation location)
        {
            location = null;

            if (false == _robotLocations.ContainsKey(robotName))
                return false;

            if (false == _robotLocations[robotName].ContainsKey(armType))
                return false;

            return _robotLocations[robotName].TryGetValue(armType, out location) && location != null;
        }
        public static bool TryUpdateNameById(
            string locationId,
            string displayName,
            out Location updatedLocation)
        {
            updatedLocation = null;

            if (string.IsNullOrWhiteSpace(locationId))
                return false;

            Location loc;
            if (false == FindLocationById(locationId, out loc))
                return false;

            loc.Name = displayName ?? string.Empty;
            updatedLocation = loc;

            return true;
        }

        public static string FindNameById(string name)
        {
            if (false == FindLocationById(name, out var loc))
                return string.Empty;

            return loc.Name;
        }
        #endregion </Get Location>

        #endregion </Methods>
    }
    public class Location
    {
        public Location(string id, ModuleType locationKind, int capacity)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Location name is required.", nameof(id));

            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be >= 0.");

            Id = id;
            Name = string.Empty;

            LocationKind = locationKind;
            Capacity = capacity;
            
            _substrateKeys = new List<string>(capacity > 0 ? capacity : 0);
        }

        private readonly List<string> _substrateKeys;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        public string Id { get; }
        public string Name { get; set; }
        public ModuleType LocationKind { get; }
        public int Capacity { get; }
        public string SubstrateKey
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _substrateKeys.Count > 0 ? _substrateKeys[0] : string.Empty;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
        public bool IsEmpty
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _substrateKeys.Count == 0;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
        public bool IsFull
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return Capacity > 0 && _substrateKeys.Count >= Capacity;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
        public IReadOnlyList<string> SubstrateKeys
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    // 외부에서 열거하는 동안 내부가 바뀌면 안 되므로 스냅샷 리턴
                    return _substrateKeys.ToArray();
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
        public OccupancyState Status
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _substrateKeys.Count > 0
                    ? OccupancyState.Occupied
                    : OccupancyState.Unoccupied;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
        public bool TryOccupy(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            _lock.EnterWriteLock();
            try
            {
                if (_substrateKeys.Count >= Capacity)
                    return false;

                if (_substrateKeys.Contains(key))
                    return false; // 중복 방지

                _substrateKeys.Add(key);
                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        public bool TryVacate(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            _lock.EnterWriteLock();

            try
            {
                if (_substrateKeys.Count == 0)
                    return false;

                if (Capacity == 1)
                {
                    _substrateKeys.Clear();
                    return true;
                }

                return _substrateKeys.Remove(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        internal void VacateAll()
        {
            _lock.EnterWriteLock();
            try
            {
                _substrateKeys.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public bool Contains(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            _lock.EnterReadLock();

            try
            {
                return _substrateKeys.Contains(key);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
    public sealed class LoadPortLocation : Location
    {
        public LoadPortLocation(string lpName, int portId, int slot)
            : base(LocationNameConverter.CreateInitialNameAtLoadPort(lpName, slot), ModuleType.LoadPort, 1)
        {
            LoadPortName = lpName;
            PortId = portId;
            Slot = slot;
        }
        public string LoadPortName { get; }
        public int PortId { get; }
        public int Slot { get; }
    }
    public sealed class RobotLocation : Location
    {
        public RobotLocation(string robotName, RobotArmTypes arm)
            : base(LocationNameConverter.CreateInitialNameAtRobot(robotName, arm), ModuleType.Robot, 1)
        {
            RobotName = robotName;
            Arm = arm;
            Name = Id;
        }
        public string RobotName { get; }
        public RobotArmTypes Arm { get; }
    }
    public sealed class ProcessModuleLocation : Location
    {
        public ProcessModuleLocation(string moduleName, string locationName, int capacity)
            : base(LocationNameConverter.CreateInitialNameAtProcessModule(locationName), ModuleType.ProcessModule, capacity)
        {
            ProcessModuleName = moduleName;
            Name = Id;
        }
        public string ProcessModuleName { get; }
    }
}
