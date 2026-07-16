using System;

using EFEM.Defines.LoadPort;          // CarrierAccessStates
using EFEM.Defines.MaterialTracking;  // TransportStates, ProcessingStates, IdReadingStates

namespace EFEM.MaterialTracking.Inspection
{
    /// <summary>
    /// 2026.07.09. jhlim [ADD] DB 조회(검색) 결과 행의 출처.
    /// - Main    : 현재 설비 내(미제거) 자재. MaterialDbContext main DB.
    /// - Archive : 배출/제거되어 일자별 Archive\yyyyMMdd.db 로 이동된 자재.
    /// 상세 조회 시 같은 출처(같은 DB 파일)로 되돌아가기 위해 행에 함께 태깅한다.
    /// </summary>
    public enum MaterialSource
    {
        Main,
        Archive
    }

    /// <summary>
    /// 캐리어 검색 결과 1행. (상세 조회 앵커 = UniqueKey + Source + ArchiveDbPath)
    /// Carrier 테이블의 Extra 를 제외한 기본 속성 전부를 담는다(결과 그리드 표시용).
    /// </summary>
    public sealed class CarrierSearchRow
    {
        public string UniqueKey = string.Empty;
        public string CarrierId = string.Empty;
        public string LotId = string.Empty;
        public int PortId;
        public CarrierAccessStates AccessStatus;
        public int Capacity;
        public string LoadTime = string.Empty;
        public string UnloadTime = string.Empty;

        public MaterialSource Source;
        /// <summary>Source == Archive 일 때 원본 archive DB 파일 경로. Main 이면 null.</summary>
        public string ArchiveDbPath;
    }

    /// <summary>
    /// 기판 검색 결과 1행. (상세 조회 앵커 = UniqueKey + Source + ArchiveDbPath)
    /// Substrate 테이블의 Extra 를 제외한 기본 속성 전부를 담는다(결과 그리드 표시용, SubstrateItem 과 대응).
    /// </summary>
    public sealed class SubstrateSearchRow
    {
        public string UniqueKey = string.Empty;
        public string Name = string.Empty;
        public string OriginName = string.Empty;
        public string LocationId = string.Empty;
        public int SourcePortId;
        public int SourceSlot;
        public string SourceCarrierId = string.Empty;
        public string CurrentCarrierKey = string.Empty;
        public int DestinationPortId;
        public int DestinationSlot;
        public string LotId = string.Empty;
        public string RecipeId = string.Empty;
        public string ProcessJobId = string.Empty;
        public string ControlJobId = string.Empty;
        public TransportStates TransportStatus;
        public ProcessingStates ProcessingStatus;
        public IdReadingStates IdReadingStatus;
        public bool DoNotProcessFlag;
        public bool Usage;

        public MaterialSource Source;
        /// <summary>Source == Archive 일 때 원본 archive DB 파일 경로. Main 이면 null.</summary>
        public string ArchiveDbPath;
    }

    /// <summary>
    /// 캐리어 검색 조건. 텍스트 조건은 부분일치(LIKE), 빈 값이면 무시. 날짜 범위는 archive 스캔 대상만 한정한다.
    /// OtherFieldName 은 기본속성/Extra 속성 중 사용자가 SelectionList 로 고른 필드명(비어있으면 미사용).
    /// </summary>
    public sealed class CarrierSearchCriteria
    {
        public string CarrierId = string.Empty;
        public string LotId = string.Empty;
        public int? PortId = null;

        /// <summary>추가 조건 필드명(CarrierBaseFieldNames 또는 CarrierExtraKeys 중 하나, 비어있으면 미사용).</summary>
        public string OtherFieldName = string.Empty;
        public string OtherFieldValue = string.Empty;
        /// <summary>false=부분일치(LIKE), true=정확일치(=).</summary>
        public bool OtherFieldExactMatch = false;

        public DateTime StartDate;
        public DateTime EndDate;
    }

    /// <summary>
    /// 기판 검색 조건. 이름 조건은 Name/OriginName 양쪽 부분일치, 포트번호(DestinationPortId)는 정확 일치. 날짜 범위는 archive 스캔 대상만 한정한다.
    /// OtherFieldName 은 기본속성/Extra 속성 중 사용자가 SelectionList 로 고른 필드명(비어있으면 미사용).
    /// </summary>
    public sealed class SubstrateSearchCriteria
    {
        public string Name = string.Empty;
        public string LotId = string.Empty;
        public int? DestinationPortId = null;

        /// <summary>추가 조건 필드명(SubstrateBaseFieldNames 또는 SubstrateExtraKeys 중 하나, 비어있으면 미사용).</summary>
        public string OtherFieldName = string.Empty;
        public string OtherFieldValue = string.Empty;
        /// <summary>false=부분일치(LIKE), true=정확일치(=).</summary>
        public bool OtherFieldExactMatch = false;

        public DateTime StartDate;
        public DateTime EndDate;
    }
}
