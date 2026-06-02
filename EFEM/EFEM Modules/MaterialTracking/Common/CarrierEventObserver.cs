using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using EFEM.MaterialTracking.SubstrateStorage;

namespace EFEM.MaterialTracking
{
    public sealed class CarrierEventObserver : ICarrierEventObserver
    {
        private readonly ISubstrateStorage _substrateStorage;
        private readonly ISubstrateByCarrier _substratesByCarrier;

        public CarrierEventObserver(ISubstrateStorage substrateStorage, ISubstrateByCarrier substratesByCarrier)
        {
            _substrateStorage = substrateStorage ?? throw new ArgumentNullException(nameof(substrateStorage));
            _substratesByCarrier = substratesByCarrier ?? throw new ArgumentNullException(nameof(substratesByCarrier));
        }

        //public void OnCarrierCreated(int portId)
        //{
        //}

        public void OnCarrierArchived(int portId, string archiveRoot)
        {
            var substrateKeys = _substratesByCarrier.GetSubstrateKeysAtLoadPort(portId);
            if (substrateKeys == null || substrateKeys.Count == 0)
                return;

            foreach (var sKey in substrateKeys)
            {
                _substrateStorage.ArchiveAsync(sKey, archiveRoot);
            }
        }

        //public void OnCarrierDeleted(int portId)
        //{
        //    var substrateKeys = _substratesByCarrier.GetSubstrateKeysAtLoadPort(portId);
        //    if (substrateKeys == null || substrateKeys.Count == 0)
        //        return;

        //    foreach (var sKey in substrateKeys)
        //    {
        //        ct.ThrowIfCancellationRequested();

        //        _substrateStorage.DeleteAsync(sKey, ct)
        //                         .GetAwaiter().GetResult();
        //    }
        //}
    }
}
