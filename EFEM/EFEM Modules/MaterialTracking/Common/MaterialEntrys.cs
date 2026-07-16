using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using EFEM.Defines.Common;            // ModuleType
using EFEM.Defines.LoadPort;          // CarrierAccessStates, CarrierSlotMapStates
using EFEM.Defines.MaterialTracking;  // TransportStates, ProcessingStates, IdReadingStates

namespace EFEM.MaterialTracking
{
    [DataContract(Name = "Substrate")]
    public sealed class SubstrateItem
    {
        [DataMember(Order = 1)] public string UniqueKey { get; set; }
        [DataMember(Order = 2)] public string Name { get; set; }
        [DataMember(Order = 3)] public string OriginName { get; set; }
        [DataMember(Order = 4)] public string LocationId { get; set; }
        [DataMember(Order = 5)] public int SourcePortId { get; set; }
        [DataMember(Order = 6)] public int SourceSlot { get; set; }
        [DataMember(Order = 7)] public string SourceCarrierId { get; set; }
        [DataMember(Order = 8)] public string CurrentCarrierKey { get; set; }
        [DataMember(Order = 9)] public int DestinationPortId { get; set; }
        [DataMember(Order = 10)] public int DestinationSlot { get; set; }
        [DataMember(Order = 11)] public string LotId { get; set; }
        [DataMember(Order = 12)] public string RecipeId { get; set; }
        [DataMember(Order = 13)] public string ProcessJobId { get; set; }
        [DataMember(Order = 14)] public string ControlJobId { get; set; }
        [DataMember(Order = 15)] public TransportStates TransportStatus { get; set; }
        [DataMember(Order = 16)] public ProcessingStates ProcessingStatus { get; set; }
        [DataMember(Order = 17)] public IdReadingStates IdReadingStatus { get; set; }
        [DataMember(Order = 18)] public bool DoNotProcessFlag { get; set; }
        [DataMember(Order = 19)] public bool Usage { get; set; }

        [DataMember(Order = 99)]
        public Dictionary<string, string> Extra { get; set; }
    }

    [DataContract(Name = "Carrier")]
    public sealed class CarrierItem
    {
        [DataMember(Order = 1)]
        public string UniqueKey { get; set; }
        [DataMember(Order = 2)] 
        public string LotId { get; set; }
        [DataMember(Order = 3)] 
        public string CarrierId { get; set; }
        [DataMember(Order = 4)] 
        public int PortId { get; set; }
        [DataMember(Order = 5)]
        public CarrierAccessStates AccessStatus { get; set; }
        [DataMember(Order = 6)]
        public int Capacity { get; set; }
        [DataMember(Order = 96)]
        public string LoadTime { get; set; }
        [DataMember(Order = 97)]
        public string UnloadTime { get; set; }
        [DataMember(Order = 98)]
        public Dictionary<int, CarrierSlotMapStates> SlotMaps { get; set; }
        [DataMember(Order = 99)]
        public Dictionary<string, string> Extra { get; set; }
    }

    //public sealed class SubstrateStayHistoryItem
    //{
    //    public SubstrateStayHistoryItem(
    //        string substrateKey,
    //        string locationName,
    //        string locationType,
    //        DateTime stayStartTime,
    //        DateTime stayEndTime,
    //        string startAction,
    //        string endAction)
    //    {
    //        if (string.IsNullOrWhiteSpace(substrateKey))
    //            throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));
    //        if (string.IsNullOrWhiteSpace(locationName))
    //            throw new ArgumentException("LocationName is required.", nameof(locationName));

    //        SubstrateKey = substrateKey;
    //        LocationName = locationName;
    //        LocationType = locationType;
    //        StayStartTime = stayStartTime;
    //        StayEndTime = stayEndTime;
    //        StartAction = startAction ?? string.Empty;
    //        EndAction = endAction ?? string.Empty;
    //        //StartSourceModule = startSourceModule ?? string.Empty;
    //        //EndSourceModule = endSourceModule ?? string.Empty;
    //    }

    //    public string SubstrateKey { get; }
    //    public string LocationName { get; }
    //    public string LocationType { get; }

    //    /// <summary>이 Location에 올라온 시간 </summary>
    //    public DateTime StayStartTime { get; }

    //    /// <summary>이 Location에서 떠난 시간 </summary>
    //    public DateTime StayEndTime { get; }

    //    /// <summary>체류 시작 트리거(예: Created, Placed, LoadedFromCassette 등)</summary>
    //    public string StartAction { get; }

    //    /// <summary>체류 종료 트리거(예: Picked, Unloaded, Scrapped 등)</summary>
    //    public string EndAction { get; }

    //    //public string StartSourceModule { get; }
    //    //public string EndSourceModule { get; }
    //}

    /// <summary>
    /// Substrate의 Location 이동(From -> To) 이벤트 기록용 DTO.
    /// </summary>
    public sealed class SubstrateLocationChangeItem
    {
        public SubstrateLocationChangeItem(
            string substrateKey,
            string fromLocationName,
            ModuleType fromLocationKind,
            string toLocationName,
            ModuleType toLocationKind,
            DateTime changeTime,
            string reason)
        {
            if (string.IsNullOrWhiteSpace(substrateKey))
                throw new ArgumentException("SubstrateKey is required.", nameof(substrateKey));

            // from/to 중 하나는 비어도 되지만, 둘 다 비면 안 됨
            if (string.IsNullOrWhiteSpace(fromLocationName) && string.IsNullOrWhiteSpace(toLocationName))
                throw new ArgumentException("Either FromLocationName or ToLocationName is required.");

            SubstrateKey = substrateKey;
            FromLocationName = fromLocationName;
            FromLocationKind = fromLocationKind;
            ToLocationName = toLocationName;
            ToLocationKind = toLocationKind;
            ChangeTime = changeTime;
            Reason = reason ?? string.Empty;
        }

        public string SubstrateKey { get; }
        public string FromLocationName { get; }
        public ModuleType FromLocationKind { get; }
        public string ToLocationName { get; }
        public ModuleType ToLocationKind { get; }
        public DateTime ChangeTime { get; }
        public string Reason { get; }
    }

    public sealed class LocationItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ModuleType LocationKind { get; set; }
        public int Capacity { get; set; }
    }
    public sealed class SubstrateProcessingHistoryItem
    {
        /// <summary>자재 고유 키 (예: "CARRIER01_LP1.01")</summary>
        public string SubstrateKey { get; set; }

        /// <summary>상태 변경 시각 </summary>
        public DateTime EventTime { get; set; }

        /// <summary>변경 전 상태</summary>
        public string OldState { get; set; }

        /// <summary>변경 후 상태</summary>
        public string NewState { get; set; }

        /// <summary>Control Job Id</summary>
        public string ControlJobId { get; set; }

        /// <summary>Process Job Id</summary>
        public string ProcessJobId { get; set; }

        /// <summary>이 이벤트 시점의 Location 스냅샷 (예: "LP1.01", "Robot1.UpperArm")</summary>
        public string LocationId { get; set; }

        /// <summary>추가 설명 / 코멘트</summary>
        public string Description { get; set; }
    }
}
