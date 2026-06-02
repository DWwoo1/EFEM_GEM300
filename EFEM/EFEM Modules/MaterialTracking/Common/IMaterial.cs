using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEM.MaterialTracking
{
    public interface IMaterial
    {
        string MaterialType { get; }
        string UniqueKey { get; }
    }
}