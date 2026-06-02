using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace EFEM.MaterialTracking
{
    /// <summary>
    /// 캐리어 저장소에서 발생하는 수명 이벤트에 반응하는 옵저버.
    /// SubstrateStorage와 연동해서 캐리어와 함께 Substrate 를 아카이브/삭제
    /// </summary>
    public interface ICarrierEventObserver
    {
        void OnCarrierArchived(int portId, string carrierArchivePath);

        // 현 시점에 아래는 미사용
        //void OnCarrierCreated(int portId, CancellationToken ct = default);
        //void OnCarrierDeleted(int portId, CancellationToken ct = default);
    }

    public interface ISubstrateEventObserver
    {
        /// <summary>
        /// SubstrateKey가 처음 생성되었을 때 호출.
        /// (최초 Upsert로 파일이 생길 때)
        /// </summary>
        void OnSubstrateCreated(string substrateKey);

        /// <summary>
        /// SubstrateKey가 active 영역에서 archive 영역으로 이동할 때 호출.
        /// destinationPath는 SubstrateData가 이동한 경로.
        /// </summary>
        void OnSubstrateArchived(string substrateKey, string destinationPath);

        /// <summary>
        /// SubstrateKey가 완전히 삭제될 때 호출.
        /// </summary>
        void OnSubstrateDeleted(string substrateKey);
    }
}
