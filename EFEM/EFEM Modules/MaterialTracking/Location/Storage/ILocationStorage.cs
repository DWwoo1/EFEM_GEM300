using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFEM.MaterialTracking.LocationStorage
{
    public interface ILocationStorage
    {
        Task AddOrUpdateLocationsAsync(IEnumerable<LocationItem> items);
    }
}
