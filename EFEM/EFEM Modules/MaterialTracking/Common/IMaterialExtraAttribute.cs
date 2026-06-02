using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEM.MaterialTracking
{
    public interface IMaterialExtraAttribute
    {
        IEnumerable<string> GetExtraKeys();
        void CreateAttributes(Dictionary<string, string> extra);
        void InitializeToPublish(Dictionary<string, string> extra, IMaterial material);
    }
}
