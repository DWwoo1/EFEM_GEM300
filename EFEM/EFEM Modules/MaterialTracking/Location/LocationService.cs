using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EFEM.MaterialTracking.LocationStorage;

namespace EFEM.MaterialTracking
{
    public interface ILocationService
    {
        Task<IReadOnlyList<Location>> AddLoadPortLocationsAsync(
            string lp,
            int portId,
            int slotCount);

        Task<IReadOnlyList<Location>> AddRobotLocationsAsync(
            string robotName);

        Task<IReadOnlyList<Location>> AddProcessModuleLocationsAsync(
            string moduleName,
            IReadOnlyDictionary<string, string> locationsByEntry,
            IReadOnlyDictionary<string, int> locationCapacity);

        Task SaveAsync(Location location);
        Task SaveAsync(IEnumerable<Location> locations);
        Task SaveByIdAsync(string locationId);
        Task SyncAllAsync();

        Task<bool> UpdateDisplayNameAsync(
            string locationId,
            string displayName);
    }

    public sealed class LocationService : ILocationService
    {
        private static readonly object _syncRoot = new object();
        private static LocationService _instance;
        private readonly ILocationStorage _repository;

        private LocationService(ILocationStorage repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public static LocationService Instance
        {
            get
            {
                if (_instance == null)
                    throw new ArgumentNullException("Not executed configure yet");

                return _instance;
            }
        }
        public static void ConfigureService(ILocationStorage repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository is null");

            lock (_syncRoot)
            {
                _instance = new LocationService(repository);
            }
        }
        public async Task<IReadOnlyList<Location>> AddLoadPortLocationsAsync(
            string lp,
            int portId,
            int slotCount)
        {
            var locations = LocationServer.AddLoadPortLocations(
                lp,
                portId,
                slotCount);

            await SaveAsync(locations).ConfigureAwait(false);

            return locations;
        }

        public async Task<IReadOnlyList<Location>> AddRobotLocationsAsync(
            string robotName)
        {
            var locations = LocationServer.AddRobotLocations(robotName);

            await SaveAsync(locations).ConfigureAwait(false);

            return locations;
        }

        public async Task<IReadOnlyList<Location>> AddProcessModuleLocationsAsync(
            string moduleName,
            IReadOnlyDictionary<string, string> locationsByEntry,
            IReadOnlyDictionary<string, int> locationCapacity)
        {
            var locations = LocationServer.AddProcessModuleLocations(
                moduleName,
                locationsByEntry,
                locationCapacity);

            await SaveAsync(locations).ConfigureAwait(false);

            return locations;
        }

        public Task SaveAsync(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            var item = ToLocationItem(location);

            return _repository.AddOrUpdateLocationsAsync(new[] { item });
        }

        public Task SaveAsync(IEnumerable<Location> locations)
        {
            if (locations == null)
                throw new ArgumentNullException(nameof(locations));

            var items = locations
                .Where(location => location != null)
                .Select(ToLocationItem)
                .ToArray();

            return _repository.AddOrUpdateLocationsAsync(items);
        }

        public Task SaveByIdAsync(string locationId)
        {
            if (string.IsNullOrWhiteSpace(locationId))
                throw new ArgumentException("Location id is required.", nameof(locationId));

            Location location;
            if (LocationServer.FindLocationById(locationId, out location) == false)
                throw new InvalidOperationException("Location not found. LocationId=" + locationId);

            return SaveAsync(location);
        }

        public Task SyncAllAsync()
        {
            return SaveAsync(LocationServer.GetLocations());
        }

        public async Task<bool> UpdateDisplayNameAsync(
            string locationId,
            string displayName)
        {
            if (string.IsNullOrWhiteSpace(locationId))
                throw new ArgumentException("Location id is required.", nameof(locationId));

            Location updatedLocation;
            if (LocationServer.TryUpdateNameById(
                locationId,
                displayName,
                out updatedLocation) == false)
            {
                return false;
            }

            await SaveAsync(updatedLocation).ConfigureAwait(false);

            return true;
        }

        private static LocationItem ToLocationItem(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            return new LocationItem
            {
                Id = location.Id,
                LocationKind = (int)location.LocationKind,
                Capacity = location.Capacity,
                Name = location.Name
            };
        }
    }
}